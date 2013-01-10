using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace CodeCampSV
{
    public static class ListExtension
    {
        /// <summary>
        /// Returns a string representation of the list of objects. Each objects is turned into a string by 
        /// calling the .ToString() method. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentList">The list of objects to turn into a string</param>
        /// <param name="delim">The deliminater you want to use between each item</param>
        /// <returns></returns>
        public static string ToJoinedString<T>(this IEnumerable<T> currentList, string delim)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in currentList)
            {
                sb.Append(item == null ? string.Empty : item.ToString()).Append(delim);
            }
            //remove the last deliminater
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - delim.Length, delim.Length);
            }
            return sb.ToString();
        }
    }
}
