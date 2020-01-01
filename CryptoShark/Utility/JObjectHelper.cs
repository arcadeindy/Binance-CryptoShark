using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoShark.Utility
{
    public static class JObjectHelper
    {
        public static string GetString(JObject jObject, string propertyName)
        {
            return GetString(jObject, propertyName, string.Empty);
        }

        public static string GetString(JObject jObject, string propertyName, string defaultValue)
        {
            if (jObject == null)
                return defaultValue;

            return jObject[propertyName] != null ? Convert.ToString(jObject[propertyName]) : defaultValue;
        }

        public static int GetInt32(JObject jObject, string propertyName)
        {
            return GetInt32(jObject, propertyName, default(int));
        }

        public static int GetInt32(JObject jObject, string propertyName, int defaultValue)
        {
            if (jObject == null)
                return defaultValue;

            int value = defaultValue;

            if (jObject[propertyName] != null)
                int.TryParse(Convert.ToString(jObject[propertyName]), out value);

            return value;
        }

        public static double GetDouble(JObject jObject, string propertyName)
        {
            return GetDouble(jObject, propertyName, default(double));
        }

        public static double GetDouble(JObject jObject, string propertyName, double defaultValue)
        {
            if (jObject == null)
                return defaultValue;

            if (jObject[propertyName] != null)
            {
                try
                {
                    string text = Convert.ToString(jObject[propertyName]);
                    text = text.Replace(",", ".");
                    return jObject[propertyName] != null ? double.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture) : defaultValue;
                }
                catch (FormatException)
                {
                    return defaultValue;
                }
            }
            else
                return defaultValue;
        }

        public static decimal GetDecimal(JObject jObject, string propertyName)
        {
            return GetDecimal(jObject, propertyName, default(decimal));
        }

        public static decimal GetDecimal(JObject jObject, string propertyName, decimal defaultValue)
        {
            if (jObject == null)
                return defaultValue;

            if (jObject[propertyName] != null)
            {
                try
                {
                    string text = Convert.ToString(jObject[propertyName]);
                    text = text.Replace(",", ".");
                    return jObject[propertyName] != null ? decimal.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture) : defaultValue;
                }
                catch (FormatException ex)
                {
                    return defaultValue;
                }
            }
            else
                return defaultValue;
        }

        public static bool GetBoolean(JObject jObject, string propertyName)
        {
            return GetBoolean(jObject, propertyName, default(bool));
        }

        public static bool GetBoolean(JObject jObject, string propertyName, bool defaultValue)
        {
            if (jObject == null)
                return defaultValue;

            return jObject[propertyName] != null ? Convert.ToBoolean(jObject[propertyName]) : defaultValue;
        }

        public static Guid GetGuid(JObject jObject, string propertyName)
        {
            return GetGuid(jObject, propertyName, default(Guid));
        }

        public static Guid GetGuid(JObject jObject, string propertyName, Guid defaultValue)
        {
            if (jObject == null)
                return defaultValue;

            return jObject[propertyName] != null ? new Guid(Convert.ToString(jObject[propertyName])) : defaultValue;
        }

    }

}
