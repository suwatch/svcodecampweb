using System.Globalization;
using CodeCampSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebAPI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("SessionRouteAll", "Session/{year}",
                      new
                      {
                          /* Your default route */
                          controller = "Session",
                          action = "Index",
                          year = Utils.ConvertCodeCampYearToActualYear(Utils.GetCurrentCodeCampYear().ToString(CultureInfo.InvariantCulture))
                      });

            routes.MapRoute("SessionRouteDetail", "Session/{year}/{session}",
                       new
                       {
                           /* Your default route */
                           controller = "Session",
                           action = "Detail",
                           year = -1,
                           session = -1
                       });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );

           
        }
    }
}