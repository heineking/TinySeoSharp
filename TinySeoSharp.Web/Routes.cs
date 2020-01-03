using System;
using System.Collections.Generic;
using System.Linq;

namespace TinySeoSharp.Web
{
  public class Routes<T> where T : new() {
    public static Routes<T> For(string[] routes) => new Routes<T>(routes);
    private readonly IEnumerable<Route> _routes;

    private Routes(string[] routes) {
      _routes  = routes.Select(route => Route.For<T>(route));
    }

    public bool IsMatch(string url) {
      return _routes.Any(route => route.IsMatch(url));
    }

    public TQuery Match<TQuery>(string url) where TQuery : new() {
      return _routes
        .Select(route => route.Match<TQuery>(url))
        .FirstOrDefault(query => query != null);
    }

    public string Build<TQuery>(TQuery query) where TQuery : new() {
      return _routes
      .Select(route => {
        try
        {
          return route.Build(query);
        }
        catch (ArgumentException)
        {
          return null;
        }
      })
      .FirstOrDefault(url => string.IsNullOrEmpty(url) == false);
    }
  }
}