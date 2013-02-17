using System;
using System.IO;
using System.Web;

namespace WebAPI
{
    public class Upload : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
     

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Files.Count > 0)
            {

                var file = new StreamReader(context.Request.Files[0].InputStream);

                string line;
                while ((line = file.ReadLine()) != null)
                {

                    // process the line or save it to local on server

                    // some code to update the progress as well
                }

                line = line + "";
            }

        }

    }
}
