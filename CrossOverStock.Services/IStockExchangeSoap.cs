using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossOverStock.Services
{
    public interface IStockExchangeSoap
    {
        StockExchange.PriceModel[] GetPricesByCodes(string[] codes);
    }
}
