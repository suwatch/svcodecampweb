using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CodeCampSV;

public partial class News : BaseContentPage
{
    private const string showPost = "ShowPost";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (ConfigurationManager.AppSettings["UseBlogSiliconValleyCodeCamp"] != null &&
          ConfigurationManager.AppSettings["UseBlogSiliconValleyCodeCamp"].ToLower().Equals("true"))
        {
            string redirectString = "http://blog.siliconvalley-codecamp.com";
            if (ConfigurationManager.AppSettings["SVCCBlogPage"] != null)
            {
                redirectString = ConfigurationManager.AppSettings["SVCCBlogPage"];
            }
            //Response.Redirect(redirectString, true);
            Response.RedirectPermanent(redirectString, true);
        }
        else
        {
            MainHeaderCountID.Visible = false;
            string threshHoldString = ConfigurationManager.AppSettings["ShowRegThreshHold"];
            int threshHoldToShowRegisteredNumber = Convert.ToInt32(threshHoldString);
            int numberRegistered = Utils.GetNumberRegistered();
            if (numberRegistered > threshHoldToShowRegisteredNumber)
            {
                int numberSessions = Utils.GetNumberSessions();
                LabelStatus.Text = String.Format("{0} Sessions,{1} Registered", numberSessions, numberRegistered);
                MainHeaderCountID.Visible = true;
            }
        }
    }


    protected static string GetDateTimeOfPost(DateTime dateTime)
    {
        return dateTime.ToShortDateString() + " @ " + dateTime.ToShortTimeString();
    }


    protected void RepeaterArticle_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        //var labelPostID = (Label) e.Item.FindControl("PostID");
        //var showAllPanel = (Panel) e.Item.FindControl("ShowAllPanel");
        //var panelShowContent = (Panel) e.Item.FindControl("PanelShowContent");
        //var panelShowTitleAndDate = (Panel) e.Item.FindControl("PanelShowTitleAndDate");

        //// first, if we have a postToFind, then need to make all other articles invisible
        //int postToFind = -1;
        //if (Items[showPost] != null)
        //{
        //    postToFind = Convert.ToInt32(Items[showPost]);
        //    panelShowTitleAndDate.Visible = false;
        //    showAllPanel.Visible = false;
        //}

        //int postID = -2;
        //if (!String.IsNullOrEmpty(labelPostID.Text))
        //{
        //    postID = Convert.ToInt32(labelPostID.Text);
        //}

        //if (postID == postToFind)
        //{
        //    panelShowContent.Visible = true;
        //    panelShowTitleAndDate.Visible = true;
        //    showAllPanel.Visible = true;
        //}
    }
}