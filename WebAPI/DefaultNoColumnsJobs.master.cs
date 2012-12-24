using System;

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using CodeCampSV;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class DefaultNoColumnsJobs : System.Web.UI.MasterPage
{
    
    private static List<CodeCampYearResult> GetListCodeCampYear()
    {
        List<CodeCampYearResult> listCodeCampYear = Utils.GetListCodeCampYear(); 
        return listCodeCampYear;
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Request.Url.LocalPath.EndsWith("Register.aspx") ||
            Request.Url.LocalPath.EndsWith("ProfileInfoAccount.aspx"))
        {
            HeadSubLinksBarID.Visible = false;
        }

        if (!Page.IsPostBack)
        {
            List<CodeCampYearResult> listCodeCampYear = GetListCodeCampYear();

            foreach (var rec in listCodeCampYear)
            {
                int year = Convert.ToInt32(rec.CampStartDate.Year);
                if (year > 2007)
                {
                    DropDownListCodeCampYearID.Items.Add(new ListItem(string.Format("{0} Code Camp", year),
                                                                      rec.Id.ToString(CultureInfo.InvariantCulture)));
                }
            }

            if (ConfigurationManager.AppSettings["SVNRevision"] != null)
            {
                //SVNVersionId.Text = ConfigurationManager.AppSettings["SVNRevision"];
            }

        }

        int codeCampYear = Utils.GetCurrentCodeCampYear();

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
            if (Context.User.Identity.IsAuthenticated)
            {
                // check for bouncing email first, send them to registration page
                if (Utils.CheckUserEmailIsBouncing(Context.User.Identity.Name))
                {
                    RegistrationHeaderID.Visible = true;
                    LabelRegistrationStatus.Text =
                        string.Format(
                            "The Email you have registered with us bounced. Please Go to Registration Page and update if it is not correct.");
                }
                else
                {



                    bool registeredForCurrentYear = Utils.IsRegisteredForCurrentCodeCampYear(Context.User.Identity.Name,
                                                                                             Utils.CurrentCodeCampYear);
                    if (!registeredForCurrentYear)
                    {
                        RegistrationHeaderID.Visible = true;
                        LabelRegistrationStatus.Text =
                            string.Format("You're logged in, but not registered for code camp on {0}.",
                                          Utils.GetCodeCampDateStringByCodeCampYearId(Utils.CurrentCodeCampYear));
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