using System.Text.RegularExpressions;

namespace TinySeoSharp.Web.Tokenizer {
  internal class RouteDelimiterRule : IRule {
    private static Regex Pattern = new Regex(@"^(\/|\?)", RegexOptions.Compiled);
    public (IToken token, string next) TryMatch(string input) {
      var match = Pattern.Match(input);
      if (match.Success == false) {
        return default;
      } 

      input = input.Remove(startIndex: 0, count: match.Length);

      var token = new RouteDelimiterToken {
        FullMatch = match.Value,
        Pattern = $@"\{match.Value}"
      };

      return (token, input);
    }
  }
}