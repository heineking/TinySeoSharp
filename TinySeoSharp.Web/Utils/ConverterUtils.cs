using System;
using System.Linq;
using System.Reflection;

namespace TinySeoSharp.Web.Utils {
  internal static class ConverterUtils {
    public static bool IsPrimitive(Type type) => PrimitiveTypes.Contains(type);
    public static bool IsPrimitiveArray(Type type) => type.IsArray && IsPrimitive(type.GetElementType());
    public static bool IsEnumArray(Type type) => type.IsArray && type.GetElementType().IsEnum;
    private static Type[] PrimitiveTypes = new Type[] {
      typeof(int),
      typeof(int?),
      typeof(decimal),
      typeof(decimal?),
      typeof(string),
      typeof(bool),
      typeof(bool?),
    };
  }
}