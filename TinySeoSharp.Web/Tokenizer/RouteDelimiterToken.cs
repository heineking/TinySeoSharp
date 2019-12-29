using System;

namespace TinySeoSharp.Web.Tokenizer {
  internal class RouteDelimiterToken : IToken {
    public string FullMatch { get; set; }
    public string Pattern { get; set; }
  }  
}