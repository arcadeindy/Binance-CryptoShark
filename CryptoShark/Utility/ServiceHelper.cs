using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace CryptoShark.Utility
{
    public class ServiceHelper
    {
        private const string CURRENCY_USD_TO_TRY_URL = "https://www.doviz.com/api/v1/currencies/USD/latest";

        public static double USDtoTRY()
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(CURRENCY_USD_TO_TRY_URL);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (var response = (HttpWebResponse)request.GetResponse())
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    var jObjectTRY = JObject.Parse(reader.ReadToEnd());
                    return Convert.ToDouble(jObjectTRY["buying"]);
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
