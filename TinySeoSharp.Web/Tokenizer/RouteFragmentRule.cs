using System;
using System.Text.RegularExpressions;

namespace TinySeoSharp.Web.Tokenizer {
  internal class RouteFragmentRule : IRule {
    private static readonly Regex Pattern = new Regex(@"^[a-z0-9-]+", RegexOptions.Compiled);

    public (IToken, string) TryMatch(string input) {
      var match = Pattern.Match(input);
      if (match.Success == false) {
        return default;
      }

      input = input.Remove(startIndex: 0, match.Length);

      var token = new RouteFragmentToken {
        FullMatch = match.Value,
        Pattern = match.Value,
      };

      return (token, input);
    }
  }
}