using System;

namespace TinySeoSharp.Web.Tokenizer
{
  internal class QueryParameterToken : IToken, IParameterToken {
    public string ParameterName { get; set; }
    public string FullMatch { get; set; }
    public string Pattern { get; set; }
  }
}