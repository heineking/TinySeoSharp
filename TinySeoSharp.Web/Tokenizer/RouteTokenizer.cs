using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace TinySeoSharp.Web.Tokenizer
{
  internal class RouteTokenizer {
    public static RouteTokenizer For(string route) {
      var tokenizer = new RouteTokenizer(route);
      return tokenizer;
    }
    private readonly string _path;
    private readonly string _queryString;
    private RouteTokenizer(string route) {
      var parts = Regex.Split(route, @"(?<=[^\{])\?");
      _path = parts.ElementAt(0);
      _queryString = parts.ElementAtOrDefault(1) ?? string.Empty;
    }

    public IEnumerable<RouteTokens> Tokenize() {
      var pathTokensArray = Tokenize(_path, PathRules);
      var queryTokensArray = Tokenize(_queryString, QueryRules);

      return pathTokensArray.SelectMany(pathTokens => {
        return queryTokensArray.Select((queryTokens) => {
          return new RouteTokens {
            PathTokens = pathTokens,
            QueryTokens = queryTokens,
          };
        });
      });
    }

    private static IEnumerable<IEnumerable<IToken>> Tokenize(string input, IEnumerable<IRule> rules, IEnumerable<IToken> tokens = null) {
      tokens = tokens ?? new IToken[] {};

      if (string.IsNullOrEmpty(input)) {
        return new [] { tokens };
      }

      var (token, next) = rules
        .Select(rule => rule.TryMatch(input))
        .FirstOrDefault(result => result != default);
      
      if (token == null) {
        return new [] { tokens };
      }

      var nextTokens = tokens.Concat(new [] { token });

      if (input.StartsWith("{?")) {
        var prevTokens = tokens.Take(tokens.Count() - 1);
        return new [] {
          Tokenize(next, rules, nextTokens),
          Tokenize(input.Remove(0, input.IndexOf("}") + 1), rules, prevTokens)
        }.SelectMany(t => t);
      }

      return Tokenize(next, rules, nextTokens);
    }

    private static readonly IEnumerable<IRule> PathRules = new List<IRule> {
      new RouteDelimiterRule(),
      new RouteFragmentRule(),
      new RouteParameterRule()
    };

    private static readonly IEnumerable<IRule> QueryRules = new List<IRule> {
      new QueryParameterRule(),
      new QueryDelimiterRule(),
    };
  }
}