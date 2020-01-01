using Binance;
using CryptoShark.Data;
using CryptoShark.Utility;
using CryptoShark.Utility.Enum;
using Quantum.Framework.GenericProperties.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoShark.Hunting.Data
{
    public class Hunting
    {
        public virtual string TypeName { get; }
        public GenericPropertyCollection Properties { get; set; }

        public BinanceApi Api { get; private set; }
        public BinanceApiUser User { get; private set; }

        public bool TradeEnabled => genericProperties.Get(SettingName.TRADING_ENABLED, false);

        private GenericPropertyCollection genericProperties;

        public Hunting()
        {
            genericProperties = SettingsHelper.GetGeneralProperties();
        }

        public virtual void Initialize()
        {
            Api = new BinanceApi();

            var apiKey = genericProperties.Get(SettingName.API_KEY, string.Empty);
            var secretKey = genericProperties.Get(SettingName.SECRET_KEY, string.Empty);

            if (!string.IsNullOrEmpty(apiKey) && !string.IsNullOrEmpty(secretKey))
                User = new BinanceApiUser(apiKey, secretKey);

            var jArrayProperties = SettingsHelper.GetJArrayHuntingProperties(TypeName);
            if (jArrayProperties != null)
                GenericPropertySerializer.DeserializePropertiesFromArray(Properties, jArrayProperties);
        }

        public virtual void StartHunting()
        {

        }

        public virtual void StopHunting()
        {

        }

        public virtual Hunting Clone()
        {
            return null;
        }

        public async Task<decimal> GetBTCBalance()
        {
            var account = await Api.GetAccountInfoAsync(User);
            foreach (var balance in account.Balances)
            {
                if (balance.Free > 0 || balance.Locked > 0)
                {
                    if (balance.Asset == Asset.BTC.Symbol)
                        return balance.Free;
                }
            }

            return 0;
        }

        public async Task<decimal> GetUSDTBalance()
        {
            var account = await Api.GetAccountInfoAsync(User);
            foreach (var balance in account.Balances)
            {
                if (balance.Free > 0 || balance.Locked > 0)
                {
                    if (balance.Asset == Asset.USDT.Symbol)
                        return balance.Free;
                }
            }

            return 0;
        }

        public async Task<decimal> GetSymbolBalance(string symbol)
        {
            var account = await Api.GetAccountInfoAsync(User);
            foreach (var balance in account.Balances)
            {
                if (balance.Free > 0 || balance.Locked > 0)
                {
                    if (balance.Asset == symbol)
                        return balance.Free;
                }
            }

            return 0;
        }

        public async Task<MarketOrderResult> BuyFromMarket(decimal mainFundQuantity, string symbol)
        {
            var success = false;
            var boughtAll = true;
            var symbolObj = await BinanceApiHelper.Instance.GetSymbol(symbol);

            var orders = new List<Order>();
            var quantityDecimalPlaces = symbolObj.Quantity.Increment.DecimalPlaces();
            var priceDecimalPlaces = symbolObj.Price.Increment.DecimalPlaces();

            while (!success || !boughtAll)
            {
                var orderBookTop = await Api.GetOrderBookTopAsync(symbol);
                var amountToBuy = mainFundQuantity / orderBookTop.Bid.Price;
                amountToBuy = amountToBuy.RoundTo(quantityDecimalPlaces);

                if (amountToBuy > orderBookTop.Bid.Quantity)
                {
                    amountToBuy = orderBookTop.Bid.Quantity;
                    boughtAll = false;
                }

                if (amountToBuy == 0)
                    break;

                try
                {
                    var order = await Api.PlaceAsync(new MarketOrder(User)
                    {
                        Quantity = amountToBuy,
                        Side = OrderSide.Buy,
                        Symbol = symbol
                    });
                    orders.Add(order);

                    var averagePriceOrder = BinanceApiHelper.Instance.GetAveragePrice(order, symbolObj);
                    mainFundQuantity -= amountToBuy * averagePriceOrder;

                    success = true;
                }
                catch (Exception ex)
                {
                    success = false;
                }

                /*
                if (!success || !boughtAll)
                    await Task.Delay(50);
                    */
            }


            var totalOrderPrice = 0m;
            var totalOrderQuantity = 0m;
            foreach (var order in orders)
            {
                var totalPrice = 0m;
                var fills = order.Fills.ToList();
                foreach (var fill in fills)
                    totalPrice += fill.Price * fill.Quantity;

                var averageOrderPrice = totalPrice / order.ExecutedQuantity;

                totalOrderPrice += averageOrderPrice * order.ExecutedQuantity;
                totalOrderQuantity += order.ExecutedQuantity;
            }

            if (totalOrderQuantity == 0)
                throw new Exception($"Total order quantitiy is 0");

            var averagePrice = (totalOrderPrice / totalOrderQuantity).RoundTo(priceDecimalPlaces);

            return new MarketOrderResult()
            {
                Symbol = symbolObj,
                AveragePrice = averagePrice,
                Quantity = totalOrderQuantity.RoundTo(quantityDecimalPlaces)
            };
        }
    }
}
