using CrossOverStock.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using CrossOverStock.Data.Domain.Authetication;

namespace CrossOverStock.Filters
{
    public class CustomRequestAttribute : ActionFilterAttribute
    {
        public ICurrentUser CurrentUser { get; set; }
        //public StockUserManager UserManager { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var identity = filterContext.RequestContext.HttpContext.User.Identity;
            if (identity != null)
            {
                CurrentUser.UserId = identity.GetUserId<int>();
                CurrentUser.UserName = identity.GetUserName();
            }
            
        }
    }
}