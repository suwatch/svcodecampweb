using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;

public partial class ReferralUrlLinks : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           
            if (Context.User.Identity.IsAuthenticated)
            {
                Label1.Text = Utils.GetAttendeesIdFromUsername(Context.User.Identity.Name).ToString();
            }

            //List<ReferringUrlAttendeeInfo> referringUrlAttendeeInfos =
            //    Utils.GetAllReferralUrls().OrderByDescending(a => a.ReferralCountAllTime).ToList();

            //RepeaterReferrings.DataSource = referringUrlAttendeeInfos;
            //RepeaterReferrings.DataBind();
        }
    }

    protected string CleanupUrl(string urlIn)
    {
        return !urlIn.StartsWith("http://") ? "http://" + urlIn : urlIn;
    }
}