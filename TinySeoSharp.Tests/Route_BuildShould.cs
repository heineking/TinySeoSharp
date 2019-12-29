using System;
using System.Linq;
using NUnit.Framework;
using TinySeoSharp.Web;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace TinySeoSharp.Tests
{

  public class Route_BuildShould {

    [Test]
    public void ReturnString() {
      var route = Route.For("/foo");
      var url = route.Build(new Query());
      Assert.IsInstanceOf(typeof(string), url);
      Assert.AreEqual("/foo", url);
    }

    [Test]
    public void BuildRouteWithQuery() {
      var route = Route.For("/contact-lenses/brands/{brand}?price={price}&category={category}&sale={sale}&page={page}");

      var query = new Query {
        Brand = Brand.AirOptix,
        Category = new[] { Category.WeeklyDisposable, Category.DailyDisposable },
        Page = 1,
        Price = new[] { 25.5m, 100m },
        Sale = true,
      };

      var url = route.Build(query);
      var urlParts = url.Split('?');
      var path = urlParts.First();
      var queryString = urlParts.ElementAtOrDefault(1) ?? string.Empty;

      Assert.AreEqual("/contact-lenses/brands/air-optix", path);
      Assert.AreEqual("category=daily,weekly&page=1&price=25.5,100&sale=true", queryString);
    }

    [Test]
    public void ThrowExceptionWhenRouteParameterIsNotProvided() {
      var route = Route.For("/contact-lenses/brands/{brand}");

      var query = new Query {};

      Assert.Throws<ArgumentException>(() => route.Build(query));
    }

    [Test]
    public void StripQueryParametersThatAreDefaultOrNotProvided() {
      var route = Route.For("/contact-lenses/brands/{brand}?price={price}&category={category}&page={page}");

      var query = new Query {
        Brand = Brand.AirOptix,
      };

      var urlParts = route.Build(query).Split('?');
      var path = urlParts.First();
      var queryString = urlParts.ElementAtOrDefault(1);
      Assert.AreEqual(string.Empty, queryString);
    }

    class Query {
      public Brand Brand { get; set; }
      public Category[] Category { get; set; }
      public decimal[] Price { get; set; }
      public int Page { get; set; }
      public bool? Sale { get; set; }
    }

    enum Brand {
      Unknown = 0,

      [Description("Acuvue")]
      Acuvue = 1,

      [Description("Air Optix")]
      AirOptix = 2
    }

    enum Category {
      [Description("Daily Disposable")]
      DailyDisposable = 1,

      [Description("Weekly Disposable")]
      WeeklyDisposable = 2
    }
  }
}