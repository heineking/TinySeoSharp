using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace TinySeoSharp.Web.Tokenizer
{
  internal class RouteTokens {
    public IEnumerable<IToken> PathTokens { get; set; }
    public IEnumerable<IToken> QueryTokens { get; set; }
    public bool IsMatch(string pathName) => PathTokens.Pattern().IsMatch(pathName);
  }
}