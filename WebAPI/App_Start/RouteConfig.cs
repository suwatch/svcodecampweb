using System.Globalization;
using CodeCampSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebAPI.App_Start;
using WebAPI.Code;

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


            // DASHBOARD
            routes.Add(new Route("Dashboarddev", new RedirectRouteHandler("/ExtJSApps/DashboardDev/app.html")));
            routes.Add(new Route("Dashboard", new RedirectRouteHandler("/ExtJSApps/Dashboard/index.html")));
            
            // add handler to track email guids being read
            routes.Add(new Route("m/{0}.gif", new MailGuidHandlerRoute()));

            // PRESENTERS
            routes.MapRoute("UnsubscribeRouteAll", "Register",
                            new
                                {
                                    /* Your default route */
                                    controller = "Register",
                                    action = "Unsubscribe"
                                });
          

            // LOGIN
            routes.MapRoute("LoginRoute", "Login",
                  new
                  {
                      /* Your default route */
                      controller = "Home",
                      action = "Login"
                  });

            // LOGOUT
            routes.MapRoute("LogOutRoute", "Logout",
                  new
                  {
                      /* Your default route */
                      controller = "Home",
                      action = "Logout"
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

    public class MailGuidHandlerRoute : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            //   /g/000000000.gif
            var path = requestContext.HttpContext.Request.Path;
            var guidStr = path.Replace("/m/", "").Replace(".gif", "");
            Guid guid;
            if (Guid.TryParse(guidStr, out guid))
            {
                try
                {
                    Utils.UpdateEmailDetailsStatus(guid);


                    //var rec = EmailDetailsManager.I.Get(new EmailDetailsQuery()
                    //                                                {
                    //                                                    EmailDetailsGuid = guid
                    //                                                }).FirstOrDefault();
                    //if (rec != null)
                    //{
                    //    if (rec.EmailReadCount.HasValue)
                    //    {
                    //        rec.EmailReadCount++;
                    //    }
                    //    else
                    //    {
                    //        rec.EmailReadCount = 1;
                    //    }
                        
                    //    rec.EmailReadDate = DateTime.UtcNow;
                    //    EmailDetailsManager.I.Update(rec);
                    //}
                }
                catch (Exception)
                {
                    // do nothing. not that important. I've seen this skip a beat when two come in quickly in a row for the same row
                }
            }
            return new GuidTracker();
        }
    }
}