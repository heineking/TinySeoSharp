using System.Text.RegularExpressions;

namespace TinySeoSharp.Web.Tokenizer
{
  internal class QueryParameterRule : IRule {
    private readonly static Regex Pattern = new Regex(@"^[a-z]{1}[a-z0-9]+=(\{([^\}]+)\})", RegexOptions.Compiled);
    public (IToken, string) TryMatch(string input) {
      var match = Pattern.Match(input);
      if (match.Success == false) {
        return default;
      }

      var value = match.Groups[2].Value;
      var capture = match.Groups[1];

      var pattern = match
        .Value
        .Remove(startIndex: capture.Index, count: capture.Length)
        .Insert(startIndex: capture.Index, @"([a-z0-9-,]+)");

      var token = new QueryParameterToken {
        FullMatch = match.Value,
        ParameterName = value,
        Pattern = pattern,
      };

      input = input.Remove(startIndex: 0, count: match.Length);
      return (token, input);
    }
  }
}