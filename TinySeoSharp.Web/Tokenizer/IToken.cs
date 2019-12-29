using System;

namespace  TinySeoSharp.Web.Tokenizer
{
  public interface IToken {
    string FullMatch { get; }
    string Pattern { get; }
  }
}