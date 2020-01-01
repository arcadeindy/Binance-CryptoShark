using CryptoShark.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoShark.Data
{
    public class OrderItem
    {
        public OrderSide Side { get; set; }
        public string Symbol { get; set; }
        //public OrderType OrderType { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal ExecutedQuantity { get; set; }
        public decimal StopPrice { get; set; }
        public decimal Total => (Quantity * Price).RoundTo(6);
        public DateTime Date { get; set; }
    }
}
