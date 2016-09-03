using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrossOverStockExchangeService.Models
{
    [Serializable]
    public class PriceModel
    {
        public string StockCode { get; set; }
        public int Price { get; set; }
    }
}