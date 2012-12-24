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

public partial class SpreadTheWord : BaseContentPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LabelCodeCampDate.Text = Utils.GetCodeCampDateStringByCodeCampYearId(Utils.CurrentCodeCampYear);    

        string referralURL = string.Empty;
        if (Context.User.Identity.IsAuthenticated)
        {
            referralURL = CodeCampSV.Utils.baseURL;// +"Default.aspx "; //?Referral=" +
                //CodeCampSV.Utils.GetAttendeePKIDByUsername(Context.User.Identity.Name.ToString());
        }
        else
        {
            referralURL = CodeCampSV.Utils.baseURL; // +"Default.aspx";
        }
        HyperLinkURL.Text = referralURL;
        HyperLinkURL.NavigateUrl = referralURL;
        TextBoxURL.Text = referralURL;

        string str1 = string.Empty;

        if (Context.User.Identity.IsAuthenticated)
        {
            string strPKID = CodeCampSV.Utils.GetAttendeePKIDByUsername(Context.User.Identity.Name.ToString());
           // str1 =
           //    "<a href=\"http://www.SiliconValley-Codecamp.com/Home.aspx?Referral=" + strPKID + "\"  >";
            str1 =
                   "<a href=" + "\"http://www.SiliconValley-Codecamp.com" + "\" target=\"_new\" title=\"CodeCamp at FootHill College.  Click Here for Details and Registration\" >";

        
        }
        else
        {
            str1 =
            "<a href=\"http://www.SiliconValley-Codecamp.com\" target=\"_new\" title=\"CodeCamp at FootHill College.  Click Here for Details and Registration\">";
        }

        str1 +=
            "<img src=\"http://www.siliconvalley-codecamp.com/DisplayAd.ashx?ImageType={0}\" " +
            "alt=\"CodeCamp at FootHill College.\" " +
            "longdesc=\"\" /></a>";
        TextBoxAd1.Text = String.Format(str1, "1");
        TextBoxAd1a.Text = String.Format(str1, "2");
        TextBoxAd2.Text = String.Format(str1, "3");
        TextBoxAd2a.Text = String.Format(str1, "4");



    }
}
