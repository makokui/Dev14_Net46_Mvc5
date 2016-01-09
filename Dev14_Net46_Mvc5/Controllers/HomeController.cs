using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dev14_Net46_Mvc5.Common;
using System.Diagnostics;
using System.Threading;

namespace Dev14_Net46_Mvc5.Controllers
{
    public class HomeController : Controller
    {
        public static int Counter { get; set; }

        public ActionResult Index()
        {
            Trace.TraceInformation("{0}: This is an informational trace message", DateTime.Now);
            Trace.TraceWarning("{0}: Here is trace warning", DateTime.Now);
            Trace.TraceError("{0}: Something is broken; tracing an error!", DateTime.Now);

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

            Foo();

            return View();
        }

        private void Foo()
        {
            Thread.Sleep(5000);
        }
    }
}