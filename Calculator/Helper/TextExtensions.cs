using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Helper
{
    public static class TextExtensions
    {
        public static string ToClentText(this string sText) => sText.ToLower().Replace(" ", string.Empty).Replace("true", "1").Replace("false", "0");

        public static double ToDouble(this string sText) => Convert.ToDouble(sText);
    }
}
