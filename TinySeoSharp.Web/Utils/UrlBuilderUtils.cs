using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;

namespace TinySeoSharp.Web.Utils
{
  internal static class UrlBuilderUtils {
    public static string CreateFragment(Type valueType, object value) {
      if (valueType == typeof(string)) {
        return NormalizeFragment.String((string)value);
      }
      if (valueType.IsEnum) {
        return EnumFragment(valueType, value);
      }
      if (valueType.IsArray && valueType.GetElementType().IsEnum) {
        return EnumArray(valueType.GetElementType(), value);
      }

      return ((string)Convert.ChangeType(value, typeof(string))).ToLower();
    }

    private static string EnumArray(Type enumType, object value) {
      var values = new List<string>();
      var items = (IList)value;
      foreach (var item in items) {
        var enumValue = Enum.ToObject(enumType, item); 
        var fragment = EnumFragment(enumType, item);
        var parts = GetParts(fragment.Split('-'));
        var part = parts.First(s => EnumFuzzy.Parse(enumType, s).Equals(enumValue));
        values.Add(part);
      }
      values.Sort();
      return string.Join(",", values);
    }

    private static string EnumFragment(Type enumType, object value) {
      var field = enumType.GetField(value.ToString());
      var description = field.GetCustomAttribute(typeof(DescriptionAttribute), false) as DescriptionAttribute;
      var fragment = description == null ? field.Name : description.Description; 
      return NormalizeFragment.String(fragment);
    }

    private static IEnumerable<string> GetParts(IEnumerable<string> parts) {
      var count = parts.Count();
      if (count == 0) {
        return new string[] {};
      }
      var next = parts.Take(count - 1);
      var str = string.Join("-", parts);
      return (new[] { str }).Concat(next).Reverse();
    }
  } 

  internal static class NormalizeFragment {
    public static string String(string input) {
      return Fns.Aggregate(input, (prev, fn) => fn(prev));
    }

    private static IEnumerable<Func<string, string>> Fns = new Func<string, string>[] {
      (input) => input.ToLower(),
      (input) => StopWordsPattern.Replace(input, " "),
      (input) => Regex.Replace(input, @"\W+", "-"),
      (input) => Regex.Replace(input, @"-{2,}", "-"),
      (input) => input.Trim(new char[] {'-', ' '}),
    };

    private static IEnumerable<string> StopWords = new List<string> { "for", "and", "with", "plus" };
    private static Regex StopWordsPattern = new Regex($@"\W({string.Join("|", StopWords)})\W");
  }
}