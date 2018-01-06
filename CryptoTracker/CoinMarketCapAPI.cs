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
        protected static readonly Dictionary<string, string> nameDictionary = new Dictionary<string, string> {
            { "Bitcoin (BTC)" , "bitcoin" },
            { "Litecoin (LTC)" , "litecoin" },
            { "Tron (TRX)" , "tron" },
            { "Ripple (XRP)" , "ripple" },
            { "Ethereum (ETH)" , "ethereum" },
            { "Bitcoin Cash (BCH)" , "bitcoin-cash" },
            { "Cardano (ADA)" , "cardano" },
            { "Stellar (XLM)" , "stellar" },
            { "IOTA (MIOTA)" , "iota" },
            { "NEM (XEM)" , "nem" },
            { "Dash (DASH)" , "dash" },
            { "Monero (XMR)" , "monero" },
            { "NEO (NEO)" , "neo" },
            { "EOS (EOS)" , "eos" },
            { "Bitcoin Gold (BTG)" , "bitcoin-gold" },
            { "Qtum (QTUM)" , "qtum" },
            { "RaiBlocks (XRB)" , "raiblocks" },
            { "Ethereum Classic (ETC)" , "ethereum-classic" },
            { "BitConnect (BCC)" , "bitconnect" },
            { "Lisk (LSK)" , "lisk" },
            { "ICON (ICX)" , "icon" },
            { "BitShares (BTS)" , "bitshares" },
            { "OmiseGo (OMG)" , "omisego" },
            { "Verge (XVG)" , "verge" },
            { "Steem (STEEM)" , "steem" },
            { "Ardor (ARDR)" , "ardor" },
            { "Zcash (ZEC)" , "zcash" },
            { "Populous (PPT)" , "populous" },
            { "Status (SNT)" , "status" },
            { "Stratis (STRAT)" , "stratis" },
            { "Tether (USDT)" , "tether" },
            { "Waves (WAVES)" , "waves" },
            { "Bytecoin (BCN)" , "bytecoin-bcn" },
            { "Siacoin (SC)" , "siacoin" },
            { "Dogecoin (DOGE)" , "dogecoin" },
            { "Hshare (HSR)" , "hshare" },
            { "VeChain (VEN)" , "vechain" },
            { "Komodo (KMD)" , "komodo" },
            { "Golem (GNT)" , "golem-network-tokens" },
            { "DigiByte (DGB)" , "digibyte" },
            { "Binance Coin (BNB)" , "binance-coin" },
            { "Augur (REP)" , "augur" },
            { "Veritaseum (VERI)" , "veritaseum" },
            { "Experience Points (XP)" , "experience-points" },
            { "Ark (ARK)" , "ark" },
            { "SALT (SALT)" , "salt" },
            { "Decred (DCR)" , "decred" },
            { "FunFair (FUN)" , "funfair" },
            { "Basic Attention Token (BAT)" , "basic-attention-token" },
            { "KuCoin Shares (KCS)" , "kucoin-shares" },
            { "Dragonchain (DRGN)" , "dragonchain" },
            { "Factom (FCT)" , "factom" },
            { "Power Ledger (POWER)" , "power-ledger" },
            { "Nxt (NXT)" , "nxt" },
            { "PIVX (PIVX)" , "pivx" },
            { "ReddCoin (RDD)" , "reddcoin" },
            { "Aion (AION)" , "aion" },
            { "MonaCoin (MONA)" , "monacoin" },
            { "RequestNetwork (REQ)" , "request-network" },
            { "Byteball Bytes (GBYTE)" , "byteball" },
            { "RChain (RHOC)" , "rchain" },
            { "Santiment Network Token (SAN)" , "santiment" },
            { "aelf (ELF)" , "aelf" },
            { "Po.et (POE)" , "poet" }
        };

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

        public Dictionary<string, string> GetNameDictionary()
        {
            return nameDictionary;
        }
    }
}
