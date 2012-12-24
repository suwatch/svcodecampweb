using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Net;
using System.IO;

public partial class RssService : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (ConfigurationManager.AppSettings["UseBlogSiliconValleyCodeCamp"] != null &&
            ConfigurationManager.AppSettings["UseBlogSiliconValleyCodeCamp"].ToLower().Equals("true"))
        {
            string rssData = GetFromBlogSite();
            Repeater1.Visible = false;
        }
    }

    private string GetFromBlogSite()
    {
        //// used to build entire input
        //StringBuilder sb = new StringBuilder();

        //// used on each read operation
        //byte[] buf = new byte[8192];

        string feedString = "http://blog.siliconvalley-codecamp.com/feed";
        if (ConfigurationManager.AppSettings["SVCCBlogPage"] != null)
        {
            feedString = string.Format("{0}/feed",
                ConfigurationManager.AppSettings["SVCCBlogPage"].TrimEnd(new[] { '/' }));
        }

        //Response.Redirect(feedString,true);

        Response.RedirectPermanent(feedString,true);

        return null;

        //// prepare the web page we will be asking for
        //HttpWebRequest request = (HttpWebRequest)
        //    WebRequest.Create(feedString);

        //// execute the request
        //HttpWebResponse response = (HttpWebResponse)
        //    request.GetResponse();

        //Response.ContentType = "text/xml";
        //Response.ContentEncoding = Encoding.UTF8;


        //// we will read data via the response stream
        //Stream resStream = response.GetResponseStream();

        //string tempString = null;
        //int count = 0;

        //do
        //{
        //    // fill the buffer with data
        //    count = resStream.Read(buf, 0, buf.Length);

        //    // make sure we read some data
        //    if (count != 0)
        //    {
        //        // translate from bytes to ASCII text
        //        tempString = Encoding.ASCII.GetString(buf, 0, count);

        //        // continue building the string
        //        sb.Append(tempString);
        //    }
        //}
        //while (count > 0); // any more data to read?

        //return sb.ToString();

        ////HttpContext.Current.Response.Write(sb.ToString());
    }


    protected DateTime GetLastUpdateTime()
    {
        //BlogTableAdapters.PostsViewTableAdapter pvt = new BlogTableAdapters.PostsViewTableAdapter();
        //DateTime? time = pvt.GetLastUpdateTime();
        //return time.GetValueOrDefault(DateTime.Now);
        return DateTime.Now;
    }

    protected String GetCurrentURL()
    {
        return "http://CurrentURL.com";
    }

    protected String FixDescription(String inString)
    {
        String outString = HttpUtility.HtmlEncode(inString);
        return outString;
    }
}

