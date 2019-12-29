using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace TinySeoSharp.Web.Tokenizer {
  internal class RouteParameterRule : IRule
  {
    private readonly static Regex Pattern = new Regex(@"^\{([^\}]+)\}", RegexOptions.Compiled);

    public (IToken, string) TryMatch(string input) {
      var match = Pattern.Match(input);

      if (match.Success == false) {
        return default;
      }

      input = input.Remove(startIndex: 0, count: match.Length);

      var parameterName = match.Groups.Cast<Group>().ElementAtOrDefault(1)?.Value;

      var token = new RouteParameterToken {
        FullMatch = match.Value,
        ParameterName = parameterName,
        Pattern = @"([a-z0-9-]+)"
      };
      
      return (token, input);
    }
  }
}