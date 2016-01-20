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
            ViewBag.Message = "Your contact .";           

            return View();
        }

        private void NativeSO()
        {
            NativeSO();
        }
        public ActionResult StackOverflow()
        {
            NativeSO();

            return View();
        }

        public ActionResult HighCPU()
        {
            ViewBag.Message = "HighCPU Page";

            ConsumeCPU(100,10);

            return View();
        }

        private void ConsumeCPU(int percentage, long seconds)
        {
            if (percentage < 0 || percentage > 100)
                throw new ArgumentException("percentage");
            Stopwatch exitWatch = new Stopwatch();
            Stopwatch watch = new Stopwatch();
            exitWatch.Start();
            watch.Start();
            while (true)
            {
                // Make the loop go on for "percentage" milliseconds then sleep the 
                // remaining percentage milliseconds. So 40% utilization means work 40ms and sleep 60ms
                if (watch.ElapsedMilliseconds > percentage)
                {
                    Thread.Sleep(100 - percentage);
                    watch.Reset();
                    watch.Start();
                }
                if (exitWatch.ElapsedMilliseconds > seconds * 1000)
                    break;
            }
        }
    }
}