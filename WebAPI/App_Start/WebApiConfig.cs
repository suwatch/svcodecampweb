using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
                name: "DefaultApi2",
                routeTemplate: "api/{controller}/{action}"
                //defaults: new {id = RouteParameter.Optional}
                );


            // config.Routes.MapHttpRoute(
            //    name: "AccountRoute",
            //    routeTemplate: "api/{controller}/{method}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            //routes.MapRoute("RegisterRouteAll", "Register",
            //    new
            //    {
            //        /* Your default route */
            //        controller = "Register",
            //        action = "Index"
            //    });

           




        }

       
    }
}
