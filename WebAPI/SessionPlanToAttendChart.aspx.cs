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
using CodeCampSV;

public partial class SessionPlanToAttendChart : BaseContentPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Utils.CheckUserIsPresenterOrAdmin())
        {

            // if plan to attend is active, then always go there first no matter what
            if (ConfigurationManager.AppSettings["ScheduleAllowCheckAttend"] != null &&
                ConfigurationManager.AppSettings["ScheduleAllowCheckAttend"].ToLower().Equals("true"))
            {
                // do nothing if true
            }
            else
            {
                Response.Redirect("~Sessions.aspx", true);
            }
        }
    }
}
