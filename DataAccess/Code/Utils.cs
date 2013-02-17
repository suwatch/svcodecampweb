using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DataAccess.Code
{
    public static class Utils
    {
        public static string GenerateSlug(string phrase)
        {
            string str = RemoveAccent(phrase).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        private static string RemoveAccent(string txt)
        {
            byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return Encoding.ASCII.GetString(bytes);
        }

        public static string GetEllipsized(string text, int characterCount, string ellipsis)
        {
            var cleanTailRegex = new Regex(@"\s+\S*$");

            if (string.IsNullOrEmpty(text) || characterCount < 0 || text.Length <= characterCount)
                return text;

            return cleanTailRegex.Replace(text.Substring(0, characterCount + 1), "") + ellipsis;
        }

        /// <summary>
        /// "7" -> "2012"
        /// "6" -> "2012"
        /// ...
        /// </summary>
        /// <param name="codeCampYear"></param>
        /// <returns></returns>
        public static string ConvertCodeCampYearToActualYear(string codeCampYear)
        {
            int codeCampYearId;
            Int32.TryParse(codeCampYear, out codeCampYearId);
            int codeCampYearNumber = 2005 + codeCampYearId;
            return codeCampYearNumber.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// strips all non alpha numeric
        /// </summary>
        /// <param name="inString"></param>
        /// <returns></returns>
        public static string AlphaNumericOnly(string inString)
        {
            if (String.IsNullOrEmpty(inString))
            {
                return "";
            }

            char[] arr = inString.ToCharArray();
            arr = Array.FindAll(arr, (c => (char.IsLetterOrDigit(c))));
            return new string(arr);
        }

    }



}
