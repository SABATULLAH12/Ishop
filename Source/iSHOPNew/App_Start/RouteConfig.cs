using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace iSHOPNew
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
"ADD’L CAPABILITIES - BGM", "ADD’L CAPABILITIES/BGM",
new { controller = "Analysis", action = "AcrossShopper", id = UrlParameter.Optional });

            routes.MapRoute(
     "ADD’L CAPABILITIES - SOAP", "ADD’L CAPABILITIES/SOAP",
     new { controller = "Analysis", action = "AcrossTrips", id = UrlParameter.Optional });

            routes.MapRoute(
      "ADD’L CAPABILITIES - CompareRetailers", "ADD’L CAPABILITIES/CompareRetailers",
      new { controller = "Analysis", action = "WithinShopper", id = UrlParameter.Optional });

            routes.MapRoute(
    "ADD’L CAPABILITIES - RetailerDeepDive", "ADD’L CAPABILITIES/RetailerDeepDive",
    new { controller = "Analysis", action = "WithinTrips", id = UrlParameter.Optional });


            routes.MapRoute(
 "ADD’L CAPABILITIES - CrossRetailerImageries", "ADD’L CAPABILITIES/CrossRetailerImageries",
 new { controller = "Analysis", action = "CrossRetailerImageries", id = UrlParameter.Optional });

            routes.MapRoute(
"ADD’L CAPABILITIES - CrossRetailerFrequencies", "ADD’L CAPABILITIES/CrossRetailerFrequencies",
new { controller = "Analysis", action = "CrossRetailerFrequencies", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Home", id = UrlParameter.Optional }
            );
        }
    }
}