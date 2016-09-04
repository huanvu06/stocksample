using CrossOverStockExchangeService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace CrossOverStockExchangeService
{
    /// <summary>
    /// Summary description for StockPriceService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class StockPrice : System.Web.Services.WebService
    {

        [WebMethod]
        public List<PriceModel> GetPricesByCodes(string[] codes)
        {
            var model = new List<PriceModel>();
            Random rd = new Random();
            foreach (var code in codes)
            {
                int price = rd.Next(1, 1000);
                model.Add(new PriceModel
                {
                    StockCode = code,
                    Price = price,
                    Change = rd.Next(1, price/10),
                    Volumn = rd.Next(200, 9999999)
                });
            }

            return model;
        }
    }
}
