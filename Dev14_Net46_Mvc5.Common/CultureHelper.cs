using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev14_Net46_Mvc5.Common
{
    public static class CultureHelper
    {
        public static string GetCulture()
        {
            // Setting culture is only valid starting with .NET 4.6
            CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture("fr-fr");

            // Use C# 6 string interpolation
            string culture = CultureInfo.CurrentCulture.Name;
            return "Your application description page. ";
        }
    }
}
