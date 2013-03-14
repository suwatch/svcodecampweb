﻿using System;
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

            //config.Routes.MapHttpRoute
            //    ("API Default", "api/{controller}/{id}",
            //     new {id = RouteParameter.Optional});

            config.Routes.MapHttpRoute
               ("API Default Rest", "rest/{controller}/{id}",
                new { id = RouteParameter.Optional });

            config.Routes.MapHttpRoute
              ("API Default RPC", "rpc/{controller}/{action}/{id}",
               new { id = RouteParameter.Optional });


         
        // config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new
            //                  {
            //                      id = RouteParameter.Optional
            //                  }
            //);


            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi2",
            //    routeTemplate: "api/{controller}/{action}/{id}",
            //    defaults: new
            //                  {
            //                      action = "GetAll",
            //                      id = RouteParameter.Optional
            //                  }
            //    //defaults: new {id = RouteParameter.Optional}
            //    );


            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi3",
            //    routeTemplate: "api/{controller}/{id}"
            //    );


        }


    }
}
