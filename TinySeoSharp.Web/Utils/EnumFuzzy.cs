using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;

namespace TinySeoSharp.Web.Utils
{
  internal static class EnumFuzzy {
    public static object Parse(Type enumType, string target) {
      var bestMatch = GetEnumInformation(enumType).FirstOrDefaultFuzzy(d => NormalizeUtils.String(d.Source), NormalizeUtils.String(target));
      return Enum.Parse(enumType, bestMatch.Name);
    }

    private static List<(string Name, string Source)> GetEnumInformation(Type type) {
      var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
      var results = new List<(string, string)>();
      foreach (var field in fields) {
        var attribute = field.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
        var source = attribute == null ? field.Name : attribute.Description;
        results.Add((field.Name, source));
      }
      return results;
    }
  }
}