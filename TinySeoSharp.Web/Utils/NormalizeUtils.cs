using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TinySeoSharp.Web.Utils {
  internal static class NormalizeUtils {
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
    private static IEnumerable<string> StopWords = new List<string> {
      "for",
      "and",
      "with",
      "plus",
      "inc"
    };
    private static Regex StopWordsPattern = new Regex($@"\W({string.Join("|", StopWords)})\W");
  }
}