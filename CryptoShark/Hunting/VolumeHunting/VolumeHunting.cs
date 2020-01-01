using Binance;
using Binance.Cache;
using Binance.Client;
using Binance.WebSocket;
using CryptoShark.Data;
using CryptoShark.Model;
using CryptoShark.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptoShark.Hunting.VolumeHunting
{
    public class VolumeHunting : Data.Hunting
    {

        #region

        private const int VOLUME_CHECK_INTERVAL = 1; //mins
        private const int ORDERS_CHECK_INTERVAL = 1; //mins

        #endregion

        #region Properties

        public override string TypeName => "volumeHunting";

        // hunting
        public decimal VolumeIncreaseThreshold => Properties.Get("volumeIncreaseThreshold", 0m);
        public decimal MinimumVolume => Properties.Get("minimumVolume", 0m);
        public decimal MinimumPrice => Properties.Get("minimumPrice", 0m);
        public decimal MinimumBidAskDifferencePercent => Properties.Get("minimumBidAskDifferencePercent", 0m);
        public int VolumeCheckIntervalMinutes => Properties.Get("checkIntervalMinutes", 10);
        public int VolumeGetIntervalMinutes => Properties.Get("getVolumeIntervalMinutes", 1);
        public decimal PriceChangeThreshold => Properties.Get("priceChangeThreshold", 0m);
        public decimal PriceDecreaseTriggerThreshold => Properties.Get("priceDecreaseTriggerThreshold", 0m);
        public string PriceDifferenceType => Properties.Get("priceDifferenceType", "lower");

        //trading
        public int MainFundParts => Properties.Get("mainFundParts", 1);
        public string MainFund => Properties.Get("mainFund", "BTC");
        public decimal ProfitPercentage => Properties.Get("profitPercentage", 0m);
        public decimal StopLossPercentage => Properties.Get("stopLossPercentage", 0m);
        public int StopLossWaitMinutes => Properties.Get("stopLossWaitMinutes", 10);

        #endregion

        #region Private Variables

        private Timer volumeCheckTimer;
        private Timer ordersCheckTimer;

        private List<StatisticData> statistics;

        private int freeMainFundPart = 0;
        private decimal mainFundQuantity;

        private bool stopLossTradingLock = false;

        private List<Order> openSellOrders;
        private List<SellOrder> watchingSellOrders;

        CryptoSharkEntities entities;

        #endregion

        public VolumeHunting() : base()
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            volumeCheckTimer = new Timer()
            {
                Interval = VolumeGetIntervalMinutes * 60 * 1000
            };
            volumeCheckTimer.Tick += VolumeTimer_Tick;

            ordersCheckTimer = new Timer()
            {
                Interval = ORDERS_CHECK_INTERVAL * 60 * 1000
            };
            ordersCheckTimer.Tick += OrdersCheckTimer_TickAsync;

            watchingSellOrders = new List<SellOrder>();
            openSellOrders = new List<Order>();
            statistics = new List<StatisticData>();

            if (TradeEnabled && (ProfitPercentage <= 0 || StopLossPercentage <= 0))
                throw new Exception("Profit or StopLoss Percentage is zero.");
        }

        public override async void StartHunting()
        {
            base.StartHunting();

            entities = new CryptoSharkEntities();

            if (TradeEnabled)
            {
                await CheckOpenOrders();
                ordersCheckTimer.Start();

                mainFundQuantity = await GetSymbolBalance(MainFund);
            }

            volumeCheckTimer.Start();

            CheckPrices();
        }

        public override void StopHunting()
        {
            base.StopHunting();

            volumeCheckTimer.Stop();
            ordersCheckTimer.Stop();
        }

        private void VolumeTimer_Tick(object sender, EventArgs e)
        {
            CheckPrices();
        }

        private async void CheckPrices()
        {
            var symbolStatistics = (await Api.Get24HourStatisticsAsync()).Where(x => x.Symbol.EndsWith(MainFund)).ToList();
            Dictionary<string, SymbolStatistics> lastStatistics = null;
            foreach (var statistic in statistics)
            {
                if ((DateTime.Now - statistic.DateTime).TotalMinutes >= VolumeCheckIntervalMinutes && (DateTime.Now - statistic.DateTime).TotalMinutes < VolumeCheckIntervalMinutes + 1)
                {
                    lastStatistics = statistic.Statistics;
                    break;
                }
            }

            if (lastStatistics != null)
            {
                foreach (var currentStatistic in symbolStatistics)
                {
                    if (currentStatistic.Symbol == "BCCBTC")
                        continue;

                    try
                    {
                        if (!openSellOrders.Any(x => x.Symbol == currentStatistic.Symbol))
                        {
                            if (lastStatistics.ContainsKey(currentStatistic.Symbol) && currentStatistic.QuoteVolume >= MinimumVolume
                                                                                    && currentStatistic.LastPrice >= MinimumPrice)
                            {
                                var oldVolume = lastStatistics[currentStatistic.Symbol].QuoteVolume;
                                var volumeDifference = currentStatistic.QuoteVolume - oldVolume;
                                var volumeChangePercent = volumeDifference / oldVolume;
                                volumeChangePercent = volumeChangePercent * 100;

                                var oldPrice = lastStatistics[currentStatistic.Symbol].LastPrice;
                                var priceDifference = currentStatistic.LastPrice - oldPrice;
                                var priceChangePercent = priceDifference / oldPrice;
                                priceChangePercent = priceChangePercent * 100;

                                Console.WriteLine($"{currentStatistic.Symbol} Volume: %{volumeChangePercent.RoundTo(3)}  -  Price: %{priceChangePercent.RoundTo(3)}");

                                if (volumeChangePercent >= VolumeIncreaseThreshold)
                                {
                                    FileLogger.Instance.WriteLog("-----------------------------------------------------");

                                    FileLogger.Instance.WriteLog($"{currentStatistic.Symbol} Volume increased by %{volumeChangePercent.RoundTo(2)}");
                                    FileLogger.Instance.WriteLog($"{currentStatistic.Symbol} Price  changed by %{priceChangePercent.RoundTo(2)}");

                                    var priceDecreaseCondition = false;
                                    var priceChangeCondition = false;
                                    var totalBidAskCondition = false;
                                    var orderBookCondition = false;

                                    if (PriceDifferenceType == "lower")
                                        priceChangeCondition = priceChangePercent <= -PriceChangeThreshold;
                                    else if (PriceDifferenceType == "higher")
                                        priceChangeCondition = priceChangePercent >= PriceChangeThreshold;

                                    if (priceChangeCondition)
                                        FileLogger.Instance.WriteLog($"Price change condition is OK. %{priceChangePercent.RoundTo(2)}");

                                    if (PriceDecreaseTriggerThreshold != 0 && priceChangePercent <= -PriceDecreaseTriggerThreshold)
                                    {
                                        priceDecreaseCondition = true;
                                        FileLogger.Instance.WriteLog($"Price decrease trigger is OK. %{priceChangePercent.RoundTo(2)}");
                                    }

                                    // totalBidAskCondition

                                    var totalBidAskPercentage = ((currentStatistic.BidQuantity - currentStatistic.AskQuantity) / currentStatistic.AskQuantity) * 100m;
                                    totalBidAskCondition = totalBidAskPercentage >= MinimumBidAskDifferencePercent; // TODO: Check!

                                    if (priceChangeCondition)
                                        FileLogger.Instance.WriteLog($"Total ask-bid percentage condition is OK. %{totalBidAskPercentage.RoundTo(2)}");

                                    // orderBookCondition

                                    var totalBid = 0m;
                                    var totalAsk = 0m;
                                    var orderBook = await Api.GetOrderBookAsync(currentStatistic.Symbol, 10);

                                    foreach (var item in orderBook.Bids)
                                        totalBid += item.Quantity * item.Price;
                                    foreach (var item in orderBook.Asks)
                                        totalAsk += item.Quantity * item.Price;

                                    FileLogger.Instance.WriteLog($"{currentStatistic.Symbol} Total Bid in QuoteAsset: {totalBid}");
                                    FileLogger.Instance.WriteLog($"{currentStatistic.Symbol} Total Ask in QuoteAsset: {totalAsk}");

                                    orderBookCondition = totalBid > totalAsk;
                                    if (orderBookCondition && MinimumBidAskDifferencePercent > 0)
                                    {
                                        var percent = ((totalBid - totalAsk) / totalAsk) * 100m;
                                        FileLogger.Instance.WriteLog($"Total Bid-Ask difference percente is {percent.RoundTo(2)}.");
                                        orderBookCondition = percent >= MinimumBidAskDifferencePercent;
                                    }

                                    if (orderBookCondition)
                                        FileLogger.Instance.WriteLog("Total bid > total ask condition is OK.");

                                    var willTrade = false;
                                    if (priceChangeCondition && orderBookCondition && totalBidAskCondition)
                                    {
                                        willTrade = true;
                                        FileLogger.Instance.WriteLog("!$!$! Price, Order Book and Total Bid/Ask percentage conditions provided.");
                                    }
                                    else if (orderBookCondition && priceDecreaseCondition && totalBidAskCondition)
                                    {
                                        willTrade = true;
                                        FileLogger.Instance.WriteLog("!$!$! Order Book and Price Decrease Trigger conditions provided.");
                                    }

                                    if (TradeEnabled && freeMainFundPart > 0 && willTrade /*&& !tradingLock*/)
                                    {
                                        await CheckDb();

                                        var symbolObj = await BinanceApiHelper.Instance.GetSymbol(currentStatistic.Symbol);

                                        var symbol = (from x in entities.Symbols
                                                      where x.BaseAsset == symbolObj.BaseAsset && x.QuoteAsset == symbolObj.QuoteAsset
                                                      select x).SingleOrDefault();
                                        if (symbol == null)
                                        {
                                            symbol = new Model.Symbol()
                                            {
                                                BaseAsset = symbolObj.BaseAsset,
                                                NotionalMinimumValue = symbolObj.NotionalMinimumValue,
                                                PriceIncrement = symbolObj.Price.Increment,
                                                QuantityIncrement = symbolObj.Quantity.Increment,
                                                QuoteAsset = symbolObj.QuoteAsset
                                            };
                                            entities.Symbols.Add(symbol);
                                            await entities.SaveChangesAsync();

                                            symbol = (from x in entities.Symbols
                                                      where x.BaseAsset == symbolObj.BaseAsset && x.QuoteAsset == symbolObj.QuoteAsset
                                                      select x).SingleOrDefault();
                                        }

                                        var dateTimeLast1Hour = DateTime.Now.AddHours(-1);
                                        var stoppedOrderExistInLast1Hour = (from x in entities.SellOrders
                                                                            where x.SymbolId == symbol.Id && x.Date >= dateTimeLast1Hour &&
                                                                            (x.Status == Constants.STATUS_STOPLOSS_EXECUTED || x.Status == Constants.STATUS_CANCELLED)
                                                                            select x).Any();

                                        if (!openSellOrders.Any(x => x.Symbol == currentStatistic.Symbol) && !stoppedOrderExistInLast1Hour)
                                        {
                                            var spendingMainFund = (mainFundQuantity / freeMainFundPart).RoundTo(6);  // TODO: 6 must be from main fund precision
                                            var marketBuyResult = await BuyFromMarket(spendingMainFund, currentStatistic.Symbol);

                                            var priceDecimalPlaces = symbolObj.Price.Increment.DecimalPlaces();

                                            var sellingPrice = marketBuyResult.AveragePrice + (marketBuyResult.AveragePrice * ProfitPercentage / 100m);
                                            sellingPrice = sellingPrice.RoundTo(priceDecimalPlaces);

                                            var stopLossPrice = marketBuyResult.AveragePrice - (marketBuyResult.AveragePrice * StopLossPercentage / 100m);
                                            stopLossPrice = stopLossPrice.RoundTo(priceDecimalPlaces);

                                            if (sellingPrice == marketBuyResult.AveragePrice)
                                                sellingPrice += symbolObj.Price.Increment;

                                            if (stopLossPrice == marketBuyResult.AveragePrice)
                                                stopLossPrice -= 2 * symbolObj.Price.Increment;

                                            FileLogger.Instance.WriteLog($"Market buy process has succeeded, average price is {marketBuyResult.AveragePrice} and quantity is {marketBuyResult.Quantity}, total {marketBuyResult.Total} {MainFund}");

                                            Order sellOrder = null;
                                            try
                                            {
                                                sellOrder = await Api.PlaceAsync(new LimitOrder(User)
                                                {
                                                    Symbol = currentStatistic.Symbol,
                                                    Price = sellingPrice,
                                                    Quantity = marketBuyResult.Quantity,
                                                    Side = OrderSide.Sell
                                                });
                                            }
                                            catch (Exception limitException)
                                            {
                                                FileLogger.Instance.WriteLog("---------------------LimitOrder Exception-----------------------");
                                                FileLogger.Instance.WriteLog($"Exception at CheckPrices with LimitOrder Exception: {limitException.Message}");
                                                FileLogger.Instance.WriteLog("---------------------LimitOrder Exception-----------------------");
                                            }

                                            entities.SellOrders.Add(new SellOrder()
                                            {
                                                OrderId = sellOrder != null ? sellOrder.Id : -1,
                                                Price = sellingPrice,
                                                StopLossPrice = stopLossPrice,
                                                Quantity = marketBuyResult.Quantity,
                                                Status = Constants.STATUS_WORKING,
                                                SymbolId = symbol.Id,
                                                Date = DateTime.Now
                                            });

                                            await entities.SaveChangesAsync();

                                            FileLogger.Instance.WriteLog("-----------------------TRADING-----------------------");
                                            FileLogger.Instance.WriteLog($"{currentStatistic.Symbol} Buy Quantity: {marketBuyResult.Quantity} - From {marketBuyResult.AveragePrice} - Total of {MainFund}: {marketBuyResult.Total}");
                                            FileLogger.Instance.WriteLog($"{currentStatistic.Symbol} Take-Profit Order - Price: {sellingPrice} - StopPrice: {stopLossPrice}");
                                            FileLogger.Instance.WriteLog("-----------------------TRADING-----------------------");

                                            await CheckOpenOrders();
                                        }
                                        else if (stoppedOrderExistInLast1Hour)
                                            FileLogger.Instance.WriteLog($"There is already an order with {currentStatistic.Symbol} stopped loss or cancelled in last 1 hour!");
                                        else
                                            FileLogger.Instance.WriteLog($"There is already an open order on {currentStatistic.Symbol} continue on..");
                                    }
                                    else if (TradeEnabled && freeMainFundPart == 0)
                                        FileLogger.Instance.WriteLog("Free main fund part is 0");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        FileLogger.Instance.WriteLog("---------------------Exception-----------------------");
                        FileLogger.Instance.WriteLog($"Exception at CheckPrices: {ex.Message}");
                        FileLogger.Instance.WriteLog("---------------------Exception-----------------------");

                        await CheckOpenOrders();

                        //tradingLock = false;
                    }
                }
            }

            statistics.Add(new StatisticData()
            {
                Statistics = symbolStatistics.ToDictionary(x => x.Symbol, x => x),
                DateTime = DateTime.Now
            });

            if (statistics.Count > VolumeCheckIntervalMinutes * 2)
                statistics.Remove(statistics.First());

            FileLogger.Instance.WriteLog("-----------------------------------------------------");
            FileLogger.Instance.WriteLog("-----------------Check End------------------");
            FileLogger.Instance.WriteLog("-----------------------------------------------------");
        }

        #region Check Orders

        private async void OrdersCheckTimer_TickAsync(object sender, EventArgs e)
        {
            if (!stopLossTradingLock)
            {
                stopLossTradingLock = true;

                var checkOpenOrdersResult = await CheckOpenOrders();

                var checkStopLossResult = await CheckStopLoss();

                stopLossTradingLock = false;
            }
        }

        private async Task<bool> CheckOpenOrders()
        {
            try
            {
                openSellOrders.Clear();

                await CheckDb();

                // from database
                var sellOrders = (from x in entities.SellOrders where x.Status == Constants.STATUS_WORKING select x).ToList();
                foreach (var sellOrder in sellOrders)
                {
                    var order = await Api.GetOrderAsync(User, sellOrder.Symbol.ToString(), sellOrder.OrderId);

                    if (order.Status == OrderStatus.Filled)
                    {
                        sellOrder.Status = Constants.STATUS_FILLED;
                        await entities.SaveChangesAsync();

                        FileLogger.Instance.WriteLog($"Order {sellOrder.OrderId} on {sellOrder.Symbol.ToString()} with price {sellOrder.Price} filled!");

                        if (watchingSellOrders.Any(x => x.OrderId == sellOrder.OrderId))
                        {
                            var removingOrder = watchingSellOrders.Where(x => x.OrderId == sellOrder.OrderId).FirstOrDefault();
                            watchingSellOrders.Remove(removingOrder);
                        }
                    }
                    else if (order.Status == OrderStatus.Canceled)
                    {
                        sellOrder.Status = Constants.STATUS_CANCELLED;
                        await entities.SaveChangesAsync();

                        FileLogger.Instance.WriteLog($"Order {sellOrder.OrderId} on {sellOrder.Symbol.ToString()} cancelled!");

                        if (watchingSellOrders.Any(x => x.OrderId == sellOrder.OrderId))
                        {
                            var removingOrder = watchingSellOrders.Where(x => x.OrderId == sellOrder.OrderId).FirstOrDefault();
                            watchingSellOrders.Remove(removingOrder);
                        }
                    }
                    else
                    {
                        if (!watchingSellOrders.Any(x => x.OrderId == sellOrder.OrderId))
                            watchingSellOrders.Add(sellOrder);
                    }
                }

                // from api
                openSellOrders = (await Api.GetOpenOrdersAsync(User)).ToList();

                freeMainFundPart = MainFundParts - openSellOrders.Count;
                mainFundQuantity = await GetSymbolBalance(MainFund);
            }
            catch (Exception ex)
            {
                try
                {
                    FileLogger.Instance.WriteLog("CheckOpenOrders Exception: " + ex.Message);
                }
                catch (Exception)
                {
                }
            }

            return true;
        }

        private async Task<bool> CheckStopLoss()
        {
            var removingDbSellOrders = new List<SellOrder>();

            foreach (var watchingSellOrder in watchingSellOrders)
            {
                try
                {
                    var statistic = await Api.Get24HourStatisticsAsync(watchingSellOrder.Symbol.ToString());
                    if (statistic.AskPrice <= watchingSellOrder.StopLossPrice &&
                        (DateTime.Now - watchingSellOrder.Date).TotalMinutes >= StopLossWaitMinutes)
                    {

                        if (statistic.AskQuantity > statistic.BidQuantity)
                        {
                            var totalBid = 0m;
                            var totalAsk = 0m;
                            var orderBook = await Api.GetOrderBookAsync(statistic.Symbol, 10);

                            foreach (var item in orderBook.Bids)
                                totalBid += item.Quantity * item.Price;
                            foreach (var item in orderBook.Asks)
                                totalAsk += item.Quantity * item.Price;

                            if (totalAsk > totalBid)
                            {

                                await CheckDb();

                                // ask price reached to stop loss price, sell all of it
                                var sellOrder_ = (from x in entities.SellOrders where x.Id == watchingSellOrder.Id select x).FirstOrDefault();
                                var orderId = sellOrder_.OrderId;

                                var checkSellOrder = await Api.GetOrderAsync(User, statistic.Symbol, orderId);

                                var cancelResult = string.Empty;

                                try
                                {
                                    if (checkSellOrder.Status == OrderStatus.New)
                                    {
                                        cancelResult = await Api.CancelOrderAsync(User, statistic.Symbol, orderId);
                                        await Task.Delay(100);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    FileLogger.Instance.WriteLog($"Cancel sell order exception: {ex.Message}");
                                }

                                var sellMarketOrder = await Api.PlaceAsync(new MarketOrder(User)
                                {
                                    Symbol = statistic.Symbol,
                                    Quantity = sellOrder_.Quantity.RoundTo(sellOrder_.Symbol.QuantityIncrement.DecimalPlaces()),
                                    Side = OrderSide.Sell
                                });

                                var averagePrice = await BinanceApiHelper.Instance.GetAveragePrice(sellMarketOrder, statistic.Symbol);

                                FileLogger.Instance.WriteLog($"Order for {statistic.Symbol} has stopped loss at {averagePrice}");

                                sellOrder_.Status = Constants.STATUS_STOPLOSS_EXECUTED;
                                sellOrder_.StopLossAt = DateTime.Now;
                                sellOrder_.StopLossPrice = averagePrice.RoundTo(sellOrder_.Symbol.PriceIncrement.DecimalPlaces());
                                await entities.SaveChangesAsync();

                                removingDbSellOrders.Add(watchingSellOrder);
                            }
                            else
                                FileLogger.Instance.WriteLog($"Order for {statistic.Symbol} reached to StopLoss price and total ask is greater than total bid but last 10 order of bid is greater than ask..Continue on..");
                        }
                        else
                            FileLogger.Instance.WriteLog($"Order for {statistic.Symbol} reached to StopLoss price but total bid is greater than total ask..Continue on..");
                    }

                }
                catch (Exception ex)
                {
                    FileLogger.Instance.WriteLog("---------------------Exception-----------------------");
                    FileLogger.Instance.WriteLog($"Exception at CheckStopLoss: {ex.Message}");
                    FileLogger.Instance.WriteLog("---------------------Exception-----------------------");
                }
            }

            foreach (var removingOrder in removingDbSellOrders)
            {
                watchingSellOrders.Remove(removingOrder);
            }

            return true;
        }

        #endregion

        private async Task CheckDb()
        {
            if (entities.Database.Connection.State != System.Data.ConnectionState.Open)
                await entities.Database.Connection.OpenAsync();
        }

    }
}
