using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoShark.Utility
{
    public class Constants
    {
        public static string SETTINGS_FILENAME => "settings.json";

        public const string ENCRYPTION_SALT = "#kc!!__-!!oguz123@@!";

        public static string HUNTING_PROPERTIES_POSTFIX = ".settings";
        public static string GENERAL_PROPERTIES = "general";

        public static string STATUS_WORKING => "WORKING";
        public static string STATUS_FILLED => "FILLED";
        public static string STATUS_STOPLOSS_EXECUTED => "STOPLOSS_EXECUTED";
        public static string STATUS_CANCELLED => "CANCELLED";

        public static Color Green => Color.FromArgb(115, 201, 33);
        public static Color Red => Color.FromArgb(202, 44, 120);
    }
}
