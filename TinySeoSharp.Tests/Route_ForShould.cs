using System;
using System.Linq;
using NUnit.Framework;
using TinySeoSharp.Web;


namespace TinySeoSharp.Tests {
  public class Route_ForShould {
    [Test]
    public void AddQueryStringFromType() {
      var route = Route.For<Query>("/{foo}");
      var query = route.Match<Query>("/foo?bar=one");
      Assert.IsNotNull(query);
      Assert.AreEqual(Bar.One, query.Bar);
      Assert.AreEqual("foo", query.Foo);
    }

    class Query {
      public string Foo { get; set; }
      public Bar Bar { get; set; }
    }

    enum Bar {
      Zero = 0,
      One = 1,
      Two = 2
    }
  }
}