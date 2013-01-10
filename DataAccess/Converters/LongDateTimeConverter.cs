using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CodeCampSV
{
    public class LongDateTimeConverter : JsonConverter
    {
        private static bool IsNullableType(Type t)
        {
            return (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        public override bool CanConvert(Type objectType)
        {
            Type t = (IsNullableType(objectType))
                         ? Nullable.GetUnderlyingType(objectType)
                         : objectType;

            if (typeof(DateTime).IsAssignableFrom(t))
                return true;
            if (typeof(DateTimeOffset).IsAssignableFrom(t))
                return true;

            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType)
        {
            Type t = (IsNullableType(objectType))
                         ? Nullable.GetUnderlyingType(objectType)
                         : objectType;

            if (reader.TokenType == JsonToken.Null
                || reader.Value == null
                || string.IsNullOrEmpty(reader.Value.ToString()))
            {
                if (!IsNullableType(objectType))
                    throw new Exception(string.Format("Cannot convert null value to {0}.", objectType));

                return null;
            }

            if (reader.TokenType != JsonToken.Integer)
                throw new Exception(string.Format("Unexpected token parsing date. Expected Integer, got {0}.", reader.TokenType));

            long ticks = (long)reader.Value;

            DateTime d = JsonConvert.ConvertJavaScriptTicksToDateTime(ticks);

            return d;
        }

        public override void WriteJson(JsonWriter writer, object value)
        {
            if (value is DateTime)
            {
                writer.WriteValue(JsonConvert.ConvertDateTimeToJavaScriptTicks((DateTime)value));
            }
            else if (value is DateTime? && ((DateTime?)value).HasValue)
            {
                writer.WriteValue(JsonConvert.ConvertDateTimeToJavaScriptTicks(((DateTime?)value).Value));
            }
            else
            {
                writer.WriteRaw("null");
            }
        }
    }
}