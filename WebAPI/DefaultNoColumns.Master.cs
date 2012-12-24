using System;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using CodeCampSV;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class DefaultNoColumns : System.Web.UI.MasterPage
{

    private static List<CodeCampYearResult> GetListCodeCampYear()
    {
        List<CodeCampYearResult> listCodeCampYear = Utils.GetListCodeCampYear();
        return listCodeCampYear;
    }

    protected void Page_Init(object sender, EventArgs e)
    {

        //HyperLinkRegistrationClosedId
        int maxRegistration = Convert.ToInt32(ConfigurationManager.AppSettings["MaxRegistration"] ?? "99999999");
        int numberRegisteredCurrentYear = Utils.GetNumberRegistered();
        bool registrationClosed = numberRegisteredCurrentYear > maxRegistration;
        if (!Context.User.Identity.IsAuthenticated && registrationClosed)
        {
            RegistrationClosedId.Visible = true;
        }

        if (registrationClosed)
        {
            if (!Utils.IsRegisteredForCurrentCodeCampYear(Context.User.Identity.Name,
                                                                     Utils.CurrentCodeCampYear))
            {
                RegistrationClosedId.Visible = true;
            }
        }

        if (Request.Url.LocalPath.EndsWith("Register.aspx") ||
            Request.Url.LocalPath.EndsWith("ProfileInfoAccount.aspx"))
        {
            HeadSubLinksBarID.Visible = false;
        }

        int codeCampYear = Utils.GetCurrentCodeCampYear();

        if (!Page.IsPostBack)
        {
            List<CodeCampYearResult> listCodeCampYear = GetListCodeCampYear();

            if (codeCampYear != Utils.CurrentCodeCampYear)
            {
                DropDownListCodeCampYearID.Visible = true;
            }

            foreach (var rec in listCodeCampYear)
            {
                int year = Convert.ToInt32(rec.CampStartDate.Year);
                if (year > 2007)
                {
                    DropDownListCodeCampYearID.Items.Add(new ListItem(string.Format("{0} Code Camp", year),
                                                                      rec.Id.ToString()));
                }
            }

            if (ConfigurationManager.AppSettings["SVNRevision"] != null)
            {
                //SVNVersionId.Text = ConfigurationManager.AppSettings["SVNRevision"];
            }

        }



        string yearString = Convert.ToString(2005 + codeCampYear);
        CodeCampActualYearId.Text = yearString;
        // this is year in dropdown on upper right (actually, id of codecampyear table)

        // Create a dictionary out of values in a DropDownList.  That is,
        // the DropDownList has a Key Value which is it's primary key
        // and a text value which is the Code Camp Year.
        Dictionary<int, string> codeCampStringDictionary =
            DropDownListCodeCampYearID.Items.Cast<ListItem>().
                OrderBy(a => a.Value).
                ToDictionary(itemdd => Convert.ToInt32(itemdd.Value),
                             itemdd => itemdd.Text);

        // Then, loop through the values in the dictionary and figure out
        // the offset of a particular value.  That is, if there are 3 values
        // in the list of 2008,2009,2010 which have primary keys of 3,4,5
        // we want to know, that given codeCampYear is 4, then selectedIndex 
        // is the second value and should be 2.
        int selectedIndex = codeCampStringDictionary.TakeWhile
            (rec => rec.Key != codeCampYear).Count();

        string dateString = Utils.GetCodeCampDateStringByCodeCampYearId(codeCampYear);
        DropDownListCodeCampYearID.SelectedIndex = selectedIndex;
        HeaderId1.Text = String.Format("Saturday and Sunday, {0}", dateString);

        //if (codeCampYear == 3)
        //{
        //    DropDownListCodeCampYearID.SelectedIndex = 0;
        //    HeaderId1.Text = String.Format("Saturday and Sunday, {0}", dateString);
        //}
        //else if (codeCampYear == 4)
        //{
        //    DropDownListCodeCampYearID.SelectedIndex = 1;
        //    HeaderId1.Text = String.Format("Saturday and Sunday {0}", dateString);
        //}
        //else if (codeCampYear == 5)
        //{
        //    DropDownListCodeCampYearID.SelectedIndex = 2;
        //    HeaderId1.Text = String.Format("Saturday and Sunday {0}", dateString);
        //}

        if (codeCampYear >= Utils.CurrentCodeCampYear)
        {
            int userOptionStatus = Utils.CheckUserEmailOptInStatus(Context.User.Identity.Name);

            if (Context.User.Identity.IsAuthenticated)
            {
                LabelRegistrationStatus.Text = "";
                if (userOptionStatus == 1)
                {
                    LabelRegistrationStatus.ForeColor = System.Drawing.Color.Blue;
                    LabelRegistrationStatus.Text = "Your profile has you set to opt out of receiving all emails.";
                }
                // check for bouncing email first, send them to registration page
                if (userOptionStatus == 2)
                {
                    LabelRegistrationStatus.ForeColor = System.Drawing.Color.Orange;
                    RegistrationHeaderID.Visible = true;
                    LabelRegistrationStatus.Text =
                        string.Format(
                            "The Email you have registered with us bounced. Please Go to Registration Page and update if it is not correct.");
                }

                bool registeredForCurrentYear = Utils.IsRegisteredForCurrentCodeCampYear(Context.User.Identity.Name,
                                                                 Utils.CurrentCodeCampYear);
                if (!registeredForCurrentYear && !registrationClosed)
                {
                    HyperLinkRegister.Visible = true;
                    RegistrationHeaderID.Visible = true;
                    LabelRegistrationStatus.BackColor = System.Drawing.Color.Orange;
                    LabelRegistrationStatus.ForeColor = System.Drawing.Color.Black;

                    string part1 = string.Format("You're logged in, but not registered for code camp on {0}. ",
                                                 Utils.GetCodeCampDateStringByCodeCampYearId(Utils.CurrentCodeCampYear));
                    var part2 = SetProfileOptOutMessage(userOptionStatus);
                    LabelRegistrationStatus.Text = part1 + part2;
                }
                else
                {
                    HyperLinkRegister.Visible = false;
                    var part2 = SetProfileOptOutMessage(userOptionStatus);
                    if (!String.IsNullOrEmpty(part2))
                    {
                        LabelRegistrationStatus.Text = part2;
                        LabelRegistrationStatus.ForeColor = userOptionStatus == 1
                                                                ? System.Drawing.Color.Blue
                                                                : System.Drawing.Color.Orange;
                    }
                    else
                    {
                        RegistrationHeaderID.Visible = false; // if person is registered no need for message.

                    }
                }

                // LabelRegistrationStatus.Text = 
            }
            else
            {
                // not logged in and 2009+
                RegistrationHeaderID.Visible = false;
            }

        }
        else
        {
            RegistrationHeaderID.Visible = false; // don't show extra line for years before 2009
        }

    }

    private static string SetProfileOptOutMessage(int userOptionStatus)
    {
        string part2 = "";
        switch (userOptionStatus)
        {
            case 1:
                part2 = "Your profile is set to opt out of all emails from code camp.";
                break;
            case 2:
                part2 =
                    "The Email you have registered with us bounced. Please Go to Registration Page and confirm if it is not correct.";
                break;
        }
        return part2;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Context.User.Identity.IsAuthenticated)
        {
            IDLoginButton.Visible = false;
            IDLoginStatus1.Visible = true;
            IDLoginName1.Visible = true;
            HyperLinkForgotChangePassword.Text = "Change Password?";
        }
        else
        {
            IDLoginButton.Visible = true;
            IDLoginStatus1.Visible = false;
            IDLoginName1.Visible = false;
            HyperLinkForgotChangePassword.Text = "Forgot Password?";
        }
    }
    protected void IDLoginButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        FormsAuthentication.RedirectToLoginPage();
    }
    protected void CodeCampYearID_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["CodeCampYear"] = Convert.ToInt32(DropDownListCodeCampYearID.SelectedItem.Value);
        string localPath = Context.Request.Url.LocalPath;
        Response.Redirect(localPath);
    }
}

/*
 * 
 * http://www.aspdotnetfaq.com/Faq/How-to-Programmatically-add-CSS-Stylesheet-file-to-Asp-Net-page.aspx
 * 

HtmlLink css = new HtmlLink();

        css.Href = "css/fancyforms.css";

        css.Attributes["rel"] = "stylesheet";

        css.Attributes["type"] = "text/css";

        css.Attributes["media"] = "all";

        Page.Header.Controls.Add(css);

*/