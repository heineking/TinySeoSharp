using System;

namespace TinySeoSharp.Web.Tokenizer {
  interface IRule {
    (IToken token, string next) TryMatch(string input);
  }
}