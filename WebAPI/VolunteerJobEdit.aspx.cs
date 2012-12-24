using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;

public partial class VolunteerJobEdit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LabelCodeCampYearId.Text = Utils.GetCurrentCodeCampYear().ToString();

       

    }
    protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        GridViewVolunteerJobs.DataBind();
    }
    protected void DetailsView1_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        GridViewVolunteerJobs.DataBind();
    }
    protected void DetailsView1_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
    {
        GridViewVolunteerJobs.DataBind();
    }
    protected void SqlDataSource1_Inserting(object sender, SqlDataSourceCommandEventArgs e)
    {
        
    }
    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        int codeCampYearId = Utils.GetCurrentCodeCampYear();

        e.Values["CodeCampYearId"] = codeCampYearId.ToString(CultureInfo.InvariantCulture);
    }
    protected void GridViewVolunteerJobs_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected string GetNumberSignedUpByJob(int jobId)
    {
        int recs = AttendeeVolunteerManager.I.Get(new AttendeeVolunteerQuery
                                                      {
                                                          VolunteerJobId = jobId
                                                      }).Count;
        return recs.ToString();
    }
    protected void SqlDataSource1_Deleting(object sender, SqlDataSourceCommandEventArgs e)
    {
        
    }

    protected void DetailsView1_ItemDeleting(object sender, DetailsViewDeleteEventArgs e)
    {
        var rec = e.Keys[0];

        List<int> attendeeVolunteerIds =
            AttendeeVolunteerManager.I.Get(new AttendeeVolunteerQuery()
                {
                    VolunteerJobId = Convert.ToInt32(rec)
                }).Select(a => a.Id).ToList();
        AttendeeVolunteerManager.I.Delete(attendeeVolunteerIds);
        GridView1.DataBind();
        GridViewVolunteerJobs.DataBind();
        

    }
}