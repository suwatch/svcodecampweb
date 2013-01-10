using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeCampSV
{
    public static class StringExtension
    {
        public static string FormatWith(this string formatString, params object[] args)
        {
            return string.Format(formatString, args);
        }
    }
}
