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
            string currentYear =
                Utils.ConvertCodeCampYearToActualYear(
                    Utils.GetCurrentCodeCampYear().ToString(CultureInfo.InvariantCulture));

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("Content/{*pathInfo}");

            // SESSIONS


            routes.MapRoute("SessionRouteTest", "Session/Test",
                     new
                     {
                         /* Your default route */
                         controller = "Session",
                         action = "IndexTest",
                         year = currentYear
                     });

            routes.MapRoute("SessionRouteAll", "Session/{year}",
                      new
                      {
                          /* Your default route */
                          controller = "Session",
                          action = "Index",
                          year = currentYear
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


            // SPONSORS
            routes.MapRoute("SponsorRouteAll", "Sponsor/{year}",
                      new
                      {
                          /* Your default route */
                          controller = "Sponsor",
                          action = "Index",
                          year = currentYear
                      });
            routes.MapRoute("SponsorRouteDetail", "Sponsor/{year}/{sponsor}",
                     new
                     {
                         /* Your default route */
                         controller = "Sponsor",
                         action = "Detail",
                         year = -1,
                         sponsor = ""
                     });

            // SPEAKERS
            routes.MapRoute("SpeakerRouteAll", "Speaker/{year}",
                      new
                      {
                          /* Your default route */
                          controller = "Speaker",
                          action = "Index",
                          year = currentYear
                      });
            routes.MapRoute("SpeakerRouteDetail", "Speaker/{year}/{speaker}",
                     new
                     {
                         /* Your default route */
                         controller = "Speaker",
                         action = "Detail",
                         year = -1,
                         speaker = ""
                     });

           
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );

           
        }
    }
}