using System;
using System.Collections.Generic;
using System.Linq;

namespace TinySeoSharp.Web.Utils {
  internal static class FuzzyExtensions {
    public static T FirstOrDefaultFuzzy<T>(this IEnumerable<T> xs, Func<T, string> f, string target) {
      return xs
        .Select(source => (score: Levenshtein.Score(f(source), target), source))
        .OrderBy(d => d.score)
        .ThenBy(d => f(d.source))
        .First()
        .source;
    }
  }
}