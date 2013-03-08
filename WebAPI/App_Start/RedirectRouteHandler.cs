using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace WebAPI.App_Start
{
    /// <summary>
    /// Redirect Route Handler
    /// http://blog.abodit.com/2010/04/a-simple-redirect-route-handler-for-asp-net-3-5-routing/
    /// </summary>
    public class RedirectRouteHandler : IRouteHandler
    {
        private readonly string _newUrl;

        public RedirectRouteHandler(string newUrl)
        {
            this._newUrl = newUrl;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new RedirectHandler(_newUrl);
        }
    }

    /// <summary>
    /// <para>Redirecting MVC handler</para>
    /// </summary>
    public class RedirectHandler : IHttpHandler
    {
        private string newUrl;

        public RedirectHandler(string newUrl)
        {
            this.newUrl = newUrl;
        }

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext httpContext)
        {
            httpContext.Response.Status = "301 Moved Permanently";
            httpContext.Response.StatusCode = 301;
            httpContext.Response.AppendHeader("Location", newUrl);
            return;
        }
    }
}