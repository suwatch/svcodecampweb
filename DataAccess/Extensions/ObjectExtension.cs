using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeCampSV;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Reflection;

namespace CodeCampSV
{
    public static class ObjectExtension
    {
        /// <summary>
        /// Helper method to generate Json string for simple objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToJson(this object value)
        {
            return JsonConvert.SerializeObject(value, new JavaScriptDateTimeConverter());
        }

        public static T FromJson<T>(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.Converters.Add(new LongDateTimeConverter());
            setting.Converters.Add(new LongConverter());
            setting.Converters.Add(new IntConverter());
            setting.Converters.Add(new DoubleConverter());
            setting.NullValueHandling = NullValueHandling.Ignore;
            setting.MissingMemberHandling = MissingMemberHandling.Ignore;

            return (T)JsonConvert.DeserializeObject(value, typeof(T), setting); 
        }

        /// <summary>
        /// Try to convert any object to an int. 
        /// Return 0 if the object is null or not a string reprentation of a integer. 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this object value)
        {
            if (value == null) return 0;
            int returnValue = 0;
            int.TryParse(value.ToString(), out returnValue);
            return returnValue;
        }

        public static int? ToIntNullable(this object value)
        {
            if (value == null)
            {
                return null;
            }
            else
            {
                return value.ToInt();
            }
        }

        /// <summary>
        /// Applys properties from the newValue to the current object when the property name 
        /// exactly matches. This method does not look at property type at the moment. 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="newValue"></param>
        public static void Apply(this object value, object newValue)
        {
            var typeOfNewValue = newValue.GetType();
            var typeOfValue = value.GetType();
            var publicPropertiesOfNewValue = typeOfNewValue
                .GetProperties();
            foreach (var propertyOfNewValue in publicPropertiesOfNewValue)
            {
                var propertyOfValue = typeOfValue.GetProperty(propertyOfNewValue.Name);

                if (propertyOfValue != null)
                {
                    propertyOfValue.SetValue(value, propertyOfNewValue.GetValue(newValue, null), null);
                }
            }
        }
    }
}
