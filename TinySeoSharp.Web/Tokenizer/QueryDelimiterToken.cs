namespace TinySeoSharp.Web.Tokenizer
{
  internal class QueryDelimiterToken : IToken {
    public string FullMatch { get; set; }
    public string Pattern { get; set; }
  }
}