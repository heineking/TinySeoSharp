using System;
using System.Linq;

namespace TinySeoSharp.Web.Utils
{
  internal static class QueryUtils {
    public static string Create<T>(string route) where T : new() {
      var path = route.Split('?').First();
      var properties = ParserUtils.GetAssignableProperties(typeof(T));

      var query = properties
        .Where(p => path.IndexOf($"{{{p.Name}}}", StringComparison.OrdinalIgnoreCase) == -1)
        .Select(p => $"{p.Name}={{{p.Name}}}");

      return $"{path}?{string.Join("&", query).ToLower()}";
    }
  }
}