namespace TinySeoSharp.Web.Tokenizer
{
  internal class RouteFragmentToken : IToken {
    public string FullMatch { get; set; }
    public string Pattern { get; set; }
  }
}