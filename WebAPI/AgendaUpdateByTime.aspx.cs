using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;

public partial class AgendaUpdateByTime : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LabelCodeCampYearId.Text = Utils.GetCurrentCodeCampYear().ToString();
    }
    protected void GridViewNotAssigned_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int timeId = Convert.ToInt32(DropDownListTimeList.SelectedValue);
        int iTest;
        if (Int32.TryParse((string)e.CommandArgument,out iTest))
        {
            int sessionId = Convert.ToInt32(e.CommandArgument);

            var rec = SessionsManager.I.Get(new SessionsQuery() {Id = sessionId}).FirstOrDefault();
            if (rec != null)
            {
                rec.SessionTimesId = timeId;
                SessionsManager.I.Update(rec);
                GridViewNotAssigned.DataBind();
                GridViewAssigned.DataBind();
            }
        }
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int timeId = Convert.ToInt32(DropDownListTimeList.SelectedValue);
        int iTest;
        if (Int32.TryParse((string)e.CommandArgument, out iTest))
        {
            int sessionId = Convert.ToInt32(e.CommandArgument);

            var rec = SessionsManager.I.Get(new SessionsQuery() { Id = sessionId }).FirstOrDefault();
            if (rec != null)
            {
                rec.SessionTimesId = 10; // this is what we have always used, "session time not set"
                SessionsManager.I.Update(rec);
                GridViewNotAssigned.DataBind();
                GridViewAssigned.DataBind();
            }
        }
    }
}