using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TwitterTest1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //RepeaterTweet.DataBind();

    }
    protected void RepeaterTweet_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

    }

    protected string GetCodeCampSessionsString(string url)
    {
        if (!String.IsNullOrEmpty(url))
        {
            return String.Format(@"&nbsp;&nbsp;&nbsp;<a  href='{0}'>Code Camp Sessions</a>", url);
        }
        else
        {
            return String.Empty;
        }
    }
}