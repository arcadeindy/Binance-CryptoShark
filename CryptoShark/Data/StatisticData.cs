using Binance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoShark.Data
{
    public class StatisticData
    {
        public Dictionary<string, SymbolStatistics> Statistics { get; set; }
        public DateTime DateTime { get; set; }
    }
}
