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

public partial class ConfirmRegistration : BaseContentPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string guidString = string.Empty;

        if (Request.Params["id"] != null)
        {
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
            }
            

            guidString = Request.Params["id"];
            string username = CodeCampSV.Utils.GetAttendeeUsernameByGUID(guidString);
            FormsAuthentication.SetAuthCookie(username, true);
            if (!string.IsNullOrEmpty(username))
            {
                string attendeeName = CodeCampSV.Utils.GetAttendeeNameByUsername(username);
                LabelConfirm.Text = "You are now confirmed for attending Code Camp.  To Update your profile information, please click on the URL below.";
                ButtonProfile.Visible = true;
                //$$$ need to do update here for this to work.
            }
            else
            {
                LabelConfirm.Text = "Invalid URL.  Confirmation failed or Attendee does not exist anymore.";
            }
        }
        else
        {
            guidString = CodeCampSV.Utils.GetAttendeePKIDByUsername(User.Identity.Name);
        }
    }
    protected void ButtonProfile_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/ProfileInfoAccount.aspx");
    }
}
