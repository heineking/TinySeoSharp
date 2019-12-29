using NUnit.Framework;
using TinySeoSharp.Web;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace TinySeoSharp.Tests
{
  public class Route_MatchShould {
    
    [Test]
    public void ReturnDefaultWhenPathDoesNotMatch() {
      var route = Route.For("/foo");
      var query = route.Match<Query>("/bar");
      Assert.IsNull(query);
    }

    [Test]
    public void ReturnRouteValuesWhenPathMatches() {
      var route = Route.For("/contact-lenses/brands/{brand}?price={price}&category={category}&page={page}&sale={sale}&rebate={rebate}");
      var query = route.Match<Query>($"/contact-lenses/brands/air-optix?category=weekly,daily&price=25,100&page=1&sale=false&rebate=true");

      Assert.AreEqual(Brand.AirOptix, query.Brand);
      Assert.AreEqual(Category.WeeklyDisposable, query.Category[0]);
      Assert.AreEqual(Category.DailyDisposable, query.Category[1]);
      Assert.AreEqual(false, query.Sale);
      Assert.AreEqual(true, query.Rebate);
      Assert.AreEqual(1, query.Page);
      Assert.AreEqual(25m, query.Price[0]);
      Assert.AreEqual(100m, query.Price[1]);
    }

    class Query {
      public Brand Brand { get; set; }
      public Category[] Category { get; set; }
      public decimal[] Price { get; set; }
      public int Page { get; set; }
      public bool? Sale { get; set; }
      public bool? Rebate { get; set; }
    }

    enum Brand {

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