using System.Text.RegularExpressions;
using System.Linq;

namespace TinySeoSharp.Web.Tokenizer
{
  internal class QueryDelimiterRule : IRule {
    private readonly static Regex Patterns = new Regex(@"=", RegexOptions.Compiled);
    public (IToken, string) TryMatch(string input) {
      var ch = input.ElementAt(0);

      if (ch != '&') {
        return default;
      }

      var token = new QueryDelimiterToken {
        FullMatch = "&",
        Pattern = "&",
      };

      input = input.Remove(startIndex: 0, count: 1);
      return (token, input);
    }
  }
}