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

public partial class RegistrationConfirmation : BaseContentPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Params["speaker"] != null)
            {
                DivSpeaker.Visible = true;
            }
            else
            {
                DivSpeaker.Visible = false;
            }

            if (Request.Params["vista"] != null)
            {
                //DivVista.Visible = true;
            }
            else
            {
                //DivVista.Visible = false;
            }

        }



        if (Context.User.Identity.IsAuthenticated)
        {
            //string referralURL = CodeCampSV.Utils.baseURL + "AttendeeRegistration.aspx?Referral=" +
            //    CodeCampSV.Utils.GetAttendeePKIDByUsername(Context.User.Identity.Name.ToString());

            string referralURL = CodeCampSV.Utils.baseURL + "Register.aspx";

            HyperLinkReferral.Text = referralURL;
            HyperLinkReferral.NavigateUrl = referralURL;
            TextBoxReferralLink.Text = referralURL;
        }
        else
        {
            Response.Redirect("~/Default.aspx");
        }



    }
}
