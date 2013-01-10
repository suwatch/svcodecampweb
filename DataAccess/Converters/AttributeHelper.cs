using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeCampSV
{
    public static class AttributeHelper
    {
        public static object GetAttribute(Type fromType, Type attributeType)
        {
            object firstResult = null;
            foreach (var v in fromType.GetCustomAttributes(attributeType, true))
            {
                firstResult = v;
                break;
            }
            return firstResult;
        }
    }
}
