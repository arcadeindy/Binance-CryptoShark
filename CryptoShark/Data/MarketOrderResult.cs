using Binance;
using System;

namespace CryptoShark.Data
{
    public class MarketOrderResult
    {
        public Symbol Symbol { get; set; }
        public decimal Quantity { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal Total => (Quantity * AveragePrice).RoundTo(7);
        public OrderSide Side { get; set; }
    }
}
