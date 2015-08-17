using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dev14_Net46_Mvc5.Common;

namespace Dev14_Net46_Mvc5.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            // Call helper that uses .NET 4.6 and C# 6 features
            ViewBag.Message = CultureHelper.GetCulture();

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = $"Your contact {"page"}.";

            return View();
        }
    }
}