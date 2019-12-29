using NUnit.Framework;
using TinySeoSharp.Web;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace TinySeoSharp.Tests
{
  public class Route_IsMatchShould {


    private Route Factory(string url = "") {
      return Route.For(url);
    }

    [Test]
    public void ReturnTrueWhenPathMatches() {
      var route = Route.For("/{foo}");
      Assert.IsTrue(route.IsMatch("/foo"));
    }

    [Test]
    public void ReturnTrueWhenPathMatchesCaseInsensitive() {
      var route = Route.For("/{foo}/bar");
      Assert.IsTrue(route.IsMatch("/foo/Bar"));
    }

    [Test]
    public void ReturnFalseWhenPathDoesNotMatch() {
      var route = Route.For("/{foo}/bar");
      Assert.IsFalse(route.IsMatch("/foo"));
    }
  }
}