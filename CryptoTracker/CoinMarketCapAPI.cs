using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;

namespace CryptoTracker
{
    public class CoinMarketCapAPI
    {
        private const string url = "https://api.coinmarketcap.com/v1/ticker/";
        private WebClient client;

        public CoinMarketCapAPI()
        {
            client = new WebClient();
        }

        public TickerJSONResult RequestTicker(string ticker)
        {
            string downloadStr = url + ticker + "/";
            Uri uri = new Uri(downloadStr);
            string resultStr = client.DownloadString(uri);

            List<TickerJSONResult> result = JsonConvert.DeserializeObject<List<TickerJSONResult>>(resultStr);

            return result[0];
        }
    }
}
