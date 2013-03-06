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

            routes.IgnoreRoute("ExtJSApps/SessionViewer");



            // SPEAKERS
            //routes.MapRoute("SpeakersRouteTest", "Speakers/Test",
            //     new
            //     {
            //         /* Your default route */
            //         controller = "Speakers",
            //         action = "IndexTest",
            //         year = currentYear
            //     });

            //routes.MapRoute("SpeakersRouteAll", "Speakers/{year}",
            //          new
            //          {
            //              /* Your default route */
            //              controller = "Speakers",
            //              action = "Index",
            //              year = currentYear
            //          });
            //routes.MapRoute("SpeakersRouteOne", "Speakers/Detail/{speakername}",
            //            new
            //            {
            //                /* Your default route */
            //                controller = "Speakers",
            //                action = "Detail"
            //            });


            // LOGIN
            routes.MapRoute("LoginRoute", "Login",
                  new
                  {
                      /* Your default route */
                      controller = "Home",
                      action = "Login"
                  });

            // REGISTER
            routes.MapRoute("RegisterRouteAll", "Register",
                   new
                   {
                       /* Your default route */
                       controller = "Register",
                       action = "Index"
                   });

            routes.MapRoute("RegisterRouteTest", "Register/Test",
                new
                {
                    /* Your default route */
                    controller = "Register",
                    action = "IndexTest"
                });



            // PRESENTERS
            routes.MapRoute("PresenterRouteAll", "Presenter/{year}",
                     new
                     {
                         /* Your default route */
                         controller = "Presenter",
                         action = "Index",
                         year = currentYear
                     });
            routes.MapRoute("PresenterRouteOne", "Presenter/{year}/{speakername}",
                        new
                        {
                            /* Your default route */
                            controller = "Presenter",
                            action = "Detail",
                            year = currentYear
                        });
         


            // SESSIONS
            routes.MapRoute("SessionRouteTest1", "Session/Test1",
                   new
                   {
                       /* Your default route */
                       controller = "Session",
                       action = "IndexTest1",
                       year = currentYear
                   });

            routes.MapRoute("SessionRouteTest2", "Session/Test2",
                 new
                 {
                     /* Your default route */
                     controller = "Session",
                     action = "IndexTest2",
                     year = currentYear
                 });

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

            routes.MapRoute("SponsorRouteTest", "Sponsor/Test",
                new
                {
                    /* Your default route */
                    controller = "Sponsor",
                    action = "IndexTest",
                    year = currentYear
                });
         

          

           
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );

           
        }
    }
}