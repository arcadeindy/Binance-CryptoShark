using Binance;
using CryptoShark.Data;
using CryptoShark.Utility.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoShark.Utility
{
    public class BinanceApiHelper
    {
        #region Singleton Members

        private static BinanceApiHelper instance;
        public static BinanceApiHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BinanceApiHelper();
                    instance.Api = new BinanceApi();
                    var apiKey = SettingsHelper.Instance.GetSetting(SettingName.API_KEY, string.Empty);
                    var secretKey = SettingsHelper.Instance.GetSetting(SettingName.SECRET_KEY, string.Empty);

                    if (!string.IsNullOrEmpty(apiKey) && !string.IsNullOrEmpty(secretKey))
                        instance.User = new BinanceApiUser(apiKey, secretKey);
                }
                return instance;
            }
        }

        #endregion

        public BinanceApi Api { get; set; }
        public BinanceApiUser User { get; set; }
        public List<Symbol> Symbols { get; set; }

        public async Task<List<OrderItem>> GetAllOrders(string quoteAsset = "BTC", int limitPerSymbol = 5)
        {
            try
            {
                var orders = new List<OrderItem>();
                //var stopwatch = Stopwatch.StartNew();

                var symbols = (await Api.GetSymbolsAsync()).Where(x => x.QuoteAsset.Symbol == quoteAsset).ToList();
                foreach (var symbol in symbols)
                {
                    var symbolOrders = await Api.GetOrdersAsync(User, symbol.ToString(), -1, limitPerSymbol);
                    foreach (var symbolOrder in symbolOrders)
                    {
                        if (symbolOrder.Status == OrderStatus.Filled)
                        {
                            var orderItem = new OrderItem()
                            {
                                Price = symbolOrder.Price,
                                StopPrice = symbolOrder.StopPrice,
                                Quantity = symbolOrder.OriginalQuantity.RoundTo(symbol.BaseAsset.Precision),
                                ExecutedQuantity = symbolOrder.ExecutedQuantity.RoundTo(symbol.BaseAsset.Precision),
                                Side = symbolOrder.Side == OrderSide.Buy ? CryptoShark.Enum.OrderSide.Buy : CryptoShark.Enum.OrderSide.Sell,
                                Symbol = symbolOrder.Symbol,
                                Date = symbolOrder.Time
                            };

                            if (symbolOrder.Type == OrderType.Market)
                            {
                                var price = 0m;
                                var trades = await Api.GetAccountTradesAsync(User, symbolOrder.Symbol, 200);
                                foreach (var trade in trades)
                                {
                                    if (trade.OrderId == symbolOrder.Id)
                                        price += trade.Price * trade.Quantity;
                                }
                                price = price / symbolOrder.ExecutedQuantity;
                                price = price.RoundTo(6);
                                orderItem.Price = price;
                            }

                            orders.Add(orderItem);
                        }
                    }
                }

                orders = orders.OrderByDescending(x => x.Date).ToList();

                //stopwatch.Stop();
                //Console.WriteLine("Elapsed: " + stopwatch.Elapsed.Seconds);

                return orders;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<OrderItem>> GetOpenOrders()
        {
            try
            {
                var orders = new List<OrderItem>();

                var openOrders = await Api.GetOpenOrdersAsync(User);
                foreach (var openOrder in openOrders)
                {
                    var symbol = await GetSymbol(openOrder.Symbol);
                    var orderItem = new OrderItem()
                    {
                        Price = openOrder.Price,
                        StopPrice = openOrder.StopPrice,
                        Quantity = openOrder.OriginalQuantity.RoundTo(symbol.BaseAsset.Precision),
                        ExecutedQuantity = openOrder.ExecutedQuantity,
                        Side = openOrder.Side == OrderSide.Buy ? CryptoShark.Enum.OrderSide.Buy : CryptoShark.Enum.OrderSide.Sell,
                        Symbol = openOrder.Symbol,
                        Date = openOrder.Time
                    };

                    //if (openOrder.Type == OrderType.Limit)
                    //    orderItem.OrderType = CryptoShark.Enum.OrderType.Limit;
                    //else if (openOrder.Type == OrderType.StopLossLimit)
                    //    orderItem.OrderType = CryptoShark.Enum.OrderType.StopLimit;
                    //else if (openOrder.Type == OrderType.Market)
                    //    orderItem.OrderType = CryptoShark.Enum.OrderType.Market;

                    orders.Add(orderItem);
                }

                return orders;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Symbol> GetSymbol(string symbol)
        {
            if (Symbols == null)
                Symbols = (await Api.GetSymbolsAsync()).ToList();

            foreach (var symbol_ in Symbols)
            {
                if (symbol_.ToString() == symbol)
                    return symbol_;
            }

            return null;
        }

        public decimal GetAveragePrice(Order order, Symbol symbol)
        {
            var totalPrice = 0m;
            var totalQuantity = 0m;

            var fills = order.Fills.ToList();
            foreach (var fill in fills)
            {
                totalPrice += fill.Price * fill.Quantity;
                totalQuantity += fill.Quantity;
            }

            var decimalPlaces = symbol.Price.Increment.DecimalPlaces();

            return (totalPrice / totalQuantity).RoundTo(decimalPlaces);
        }

        public async Task<decimal> GetAveragePrice(Order order, string symbol)
        {
            var symbolObj = await GetSymbol(symbol);
            return GetAveragePrice(order, symbolObj);
        }
    }
}
