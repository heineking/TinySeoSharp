using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TinySeoSharp.Web.Utils {
  internal static class ParserUtils {
    public static IEnumerable<PropertyInfo> GetAssignableProperties(Type type) => ReflectionUtils
      .GetProperties(type)
      .Where(IsAllowedPropertyType);

    private static bool IsAllowedPropertyType(PropertyInfo propertyInfo) {
      var type = propertyInfo.PropertyType;
      return type.IsEnum
        || ConverterUtils.IsPrimitive(type)
        || ConverterUtils.IsPrimitiveArray(type)
        || ConverterUtils.IsEnumArray(type);
    }
  }
}