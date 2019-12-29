using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace TinySeoSharp.Web.Tokenizer
{
  internal static class TokenizerExtensions {
    public static Regex Pattern(this IEnumerable<IToken> tokens) {
      var pattern = string.Join("", tokens.Select(token => token.Pattern));
      return new Regex($"^{pattern}$");
    }

    public static Match Match(this IEnumerable<IToken> tokens, string input) {
      return tokens.Pattern().Match(input);
    }
  }
}