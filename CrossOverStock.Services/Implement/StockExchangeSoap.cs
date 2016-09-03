using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CrossOverStock.Services.Implement
{
    public class StockExchangeSoap : IStockExchangeSoap
    {
        const string username = "user1";
        const string password = "user1password";
        readonly StockExchange.StockPrice _stockPrice;
        public StockExchangeSoap()
        {
            _stockPrice = new StockExchange.StockPrice();

            // Create a new instance of CredentialCache.
            CredentialCache credentialCache = new CredentialCache();

            // Create a new instance of NetworkCredential using the client
            // credentials.
            NetworkCredential credentials = new NetworkCredential(username, password);

            // Add the NetworkCredential to the CredentialCache: Basic or Digest
            credentialCache.Add(new Uri(_stockPrice.Url), "Digest", credentials);

            // Add the CredentialCache to the proxy class credentials.
            _stockPrice.Credentials = credentialCache;

        }

        public StockExchange.PriceModel[] GetPricesByCodes(string[] codes)
        {
            return _stockPrice.GetPricesByCodes(codes);
        }
    }
}
