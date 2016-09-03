using CrossOverStock.Filters;
using CrossOverStock.Models.Stocks;
using CrossOverStock.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CrossOverStock.Controllers
{
    [Authorize]
    [CustomRequest]
    public class HomeController : Controller
    {
        readonly IStockService _stockService;
        readonly IStockExchangeSoap _stockExchange;
        public HomeController(IStockService stockService, IStockExchangeSoap stockExchange)
        {
            _stockService = stockService;
            _stockExchange = stockExchange;
        }

        public ActionResult Index()
        {
            var model = new StockHomeModel();
            model.StockCodePrices = _stockExchange.GetPricesByCodes(_stockService.GetStockCodes().ToArray());
            model.CurrentUserStockCodes = _stockService.GetCurrentUserStockCodes();
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}