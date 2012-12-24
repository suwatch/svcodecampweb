using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;




public partial class TagGroupGraph : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        // Need to get a list of all emails associated with current platinum sponsors (uses current year, not one selected by dropdown)
        var sponsorIds = (SponsorListCodeCampYearManager.I.Get(new SponsorListCodeCampYearQuery
        {
            CodeCampYearId = Utils.CurrentCodeCampYear,
            DonationAmount = 3900.00
        })).Select(a => a.SponsorListId).ToList();

        // get the usernames of all platinum sponsors contacts
        var usernameList = SponsorListContactManager.I.Get(new SponsorListContactQuery
        {
            SponsorListIds = sponsorIds
        }).Select(a => a.UsernameAssociated).ToList();

        if (usernameList.Contains(Context.User.Identity.Name) || Utils.CheckUserIsAdmin() || Utils.CheckUserIsTagGroupGraphViewer())
        {
            int codeCampYearIdForGraph = Utils.GetCurrentCodeCampYear();

            LabelAttendeesId.Text = Utils.GetAttendeesIdFromUsername(Context.User.Identity.Name).ToString();
            LabelCodeCampYearId.Text = codeCampYearIdForGraph.ToString(); // gets from dropdownlist on top of page

            var resultSessionInterestTagsClass =
                new ResultSessionInterestTagsClass();
            var recs = resultSessionInterestTagsClass.GetAll(codeCampYearIdForGraph,
                                                             Convert.ToInt32(LabelAttendeesId.Text));

            if (recs != null && recs.Count > 0)
            {
                Chart1.Titles.Add(
                    "Percentage of unique users in the Tag List out of the total population of Interested Registered users");
                Chart1.Titles.Add(String.Format("(Total Interested Registered Users For Code Camp Year {0}: {1})",
                                                (codeCampYearIdForGraph + 2005).ToString(),
                                                recs[0].TotalAttendeesRegisteringInterest.ToString()));
            }
            LabelNotAvailable.Visible = false;
        }
        else
        {
            Chart1.Visible = false;
            LabelNotAvailable.Visible = true;
        }



        //GridView1.DataSource = recs;
        //GridView1.DataBind();
        




    }
}