using System.IO;
using System.Web;

namespace WebAPI.Code
{
    //http://forums.asp.net/t/1579755.aspx/1
    public class GuidTracker : IHttpHandler
    {
        public void ProcessRequest(System.Web.HttpContext ctx)
        {

            string path = ctx.Server.MapPath("~/Content/Images/1.gif");
            ctx.Response.StatusCode = 200;
            ctx.Response.ContentType = "image/gif";
            ctx.Response.WriteFile(path);
        }

        public
            bool IsReusable 
            {
                get
                {
                    return true;
                }
            }
        }
    }