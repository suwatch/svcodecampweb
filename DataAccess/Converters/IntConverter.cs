﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CodeCampSV
{
    public class IntConverter : JsonConverter
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

            if (typeof(int).IsAssignableFrom(t))
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

            return Convert.ToInt32(reader.Value);
        }

        public override void WriteJson(JsonWriter writer, object value)
        {
            throw new NotImplementedException();
        }
    }
}