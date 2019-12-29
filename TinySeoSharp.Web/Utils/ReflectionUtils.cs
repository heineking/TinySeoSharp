using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TinySeoSharp.Web.Utils
{
  internal static class ReflectionUtils
  {
    public static bool IsPublic(PropertyInfo property) {
      return (new MethodInfo[] {
        property.GetGetMethod(),
        property.GetSetMethod()
      }).All(m => m != null && m.IsPublic);
    }

    public static bool IsNullableType(Type type) {
      return type.IsGenericType
        && type.GetGenericTypeDefinition() == typeof(Nullable<>);
    }

    public static Type EnsureNotNullableType(Type type) {
      return IsNullableType(type)
        ? Nullable.GetUnderlyingType(type)
        : type;
    }

    public static IEnumerable<PropertyInfo> GetProperties(Type type) {
      return type
        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
        .Where(IsPublic);
    }

    public static object GetDefaultValue(Type type) {
      if (type.IsValueType) {
        return Activator.CreateInstance(type);
      }
      return null;
    }
  }
}