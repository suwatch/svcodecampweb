using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;
using Roles=System.Web.Security.Roles;

public partial class SaturdayDinnerStatus : BaseContentPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LabelStatus.Text = String.Empty;

        string guidString;
        if (Request.Params["id"] != null)
        {
            guidString = Request.Params["id"];
            string username = Utils.GetAttendeeUsernameByGUID(guidString);
            if (!Roles.IsUserInRole(username, Utils.GetAdminRoleName()))
            {
                if (!string.IsNullOrEmpty(username))
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        FormsAuthentication.SignOut();
                    }
                    FormsAuthentication.SetAuthCookie(username, true);

                    // check and see if person wants to override dinner setting
                    if (Request.QueryString["AttendDinner"] != null)
                    {
                        string answerAttendDinner = Request.QueryString["AttendDinner"];
                        if (answerAttendDinner.ToLower().Equals("true"))
                        {
                            Utils.UpdateAttendDinner(true, username);
                            LabelStatus.Text =
                                "<h2>Your Profile is marked showing you <u>plan</u> to attend the Barbeque Saturday Night</h2>";
                        }
                        else
                        {
                            Utils.UpdateAttendDinner(false, username);
                            LabelStatus.Text =
                                "<h2>Your Profile is marked showing you <u>do not</u> plan to attend the Barbeque Saturday Night</h2>";
                      
                        }
                    }
                    //Response.Redirect("~/ProfileInfoAccount.aspx", true);
                }
            }
        }
        else
        {
            guidString = Utils.GetAttendeePKIDByUsername(User.Identity.Name);
        }

    }
}
