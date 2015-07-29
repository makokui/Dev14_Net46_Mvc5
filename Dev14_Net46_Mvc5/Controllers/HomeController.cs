using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            // Setting culture is only valid starting with .NET 4.6
            CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture("fr-fr");

            // Use C# 6 string interpolation
            string culture = CultureInfo.CurrentCulture.Name;
            ViewBag.Message = $"Your application description page. Culture is {culture}";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}