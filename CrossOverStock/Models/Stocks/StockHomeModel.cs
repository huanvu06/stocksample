using CrossOverStock.Data.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrossOverStock.Models.Stocks
{
    public class StockHomeModel
    {
        public Services.StockExchange.PriceModel[] StockCodePrices { get; set; }
        public IEnumerable<StockCodeDto> CurrentUserStockCodes { get; set; }
    }
}