using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class MathExtensions
    {

        public static decimal RoundTo(this decimal value, int decimals)
        {
            //var multiplier = (decimal)Math.Pow(10, decimals);
            //return Math.Truncate(value * multiplier) / multiplier;
            return Math.Round(value, decimals);
        }

        public static int DecimalPlaces(this decimal value)
        {
            var decimalPlaces = 0;
            var temp = value;

            while (temp < 1)
            {
                temp = temp * 10;
                decimalPlaces++;
            }

            return decimalPlaces;
        }
    }
}
