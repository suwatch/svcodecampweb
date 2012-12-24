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
using System.Collections.Generic;

public partial class SessionEval : BaseContentPage
{
    int sessionId = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        // http://localhost:4443/Web/SessionEval.aspx?id=2
        string requestIdStr = Request.QueryString["id"];
        bool good = Int32.TryParse(requestIdStr, out sessionId);
        if (!good)
        {
            Response.Redirect("~/Sessions.aspx");
        }

        if (!IsPostBack)
        {
            if (Context.User.Identity.IsAuthenticated) // should always be to get here
            {
                Label1.Text = string.Empty;
                Label2.Text = string.Empty;

                HyperLinkReturn1.NavigateUrl = "~/Sessions.aspx?OnlyOne=true&id=" + sessionId.ToString();
                HyperLinkReturn2.NavigateUrl = "~/Sessions.aspx?OnlyOne=true&id=" + sessionId.ToString();

                HyperLinkReturn4.NavigateUrl = "~/SessionsOverview.aspx";
                HyperLinkReturn3.NavigateUrl = "~/SessionsOverview.aspx";

                HyperLink2.NavigateUrl = "~/MyEvals.aspx";
                HyperLink1.NavigateUrl = "~/MyEvals.aspx";


                LabelPresenterName.Text = Utils.GetUserNameOfSession(sessionId);
                SessionsODS sessionsODS = new SessionsODS();
                List<SessionsODS.DataObjectSessions> li =
                    sessionsODS.GetByPrimaryKeySessions(sessionId);
                LabelSessionName.Text = li[0].Title;


                ButtonUpdate1.Enabled = true;
                ButtonUpdate2.Enabled = true;

                this.AddRadioButtonChoices(RBLCourseAsWhole);
                this.AddRadioButtonChoices(RBLCourseContent);
                this.AddRadioButtonChoices(RBLInstructorAbilityExplain);
                this.AddRadioButtonChoices(RBLInstructorEffective);
                this.AddRadioButtonChoices(RBLInstructorKnowledge);
                this.AddRadioButtonChoices(RBLQualityOfFacility);
                this.AddRadioButtonChoices(RBLContentLevel);
                this.AddRadioButtonChoices(RBLOverallCodeCamp);

                SessionEvalsODS seODS = new SessionEvalsODS();
                List<CodeCampSV.SessionEvalsODS.DataObjectSessionEvals> liEvals =
                    seODS.GetByUsernameSessionId(Context.User.Identity.Name, sessionId);
                if (liEvals.Count > 0)
                {
                    SessionEvalsODS.DataObjectSessionEvals se = (SessionEvalsODS.DataObjectSessionEvals)liEvals[0];
                    RBLCourseAsWhole.SelectedValue = se.Courseaswhole.ToString();
                    RBLCourseContent.SelectedValue = se.Coursecontent.ToString();
                    RBLInstructorAbilityExplain.SelectedValue = se.Instructorabilityexplain.ToString();
                    RBLInstructorEffective.SelectedValue = se.Instructoreffective.ToString();
                    RBLInstructorKnowledge.SelectedValue = se.Instructorknowledge.ToString();
                    RBLQualityOfFacility.SelectedValue = se.Qualityoffacility.ToString();
                    RBLContentLevel.SelectedValue = se.Contentlevel.ToString();
                    RBLOverallCodeCamp.SelectedValue = se.Overallcodecamp.ToString();

                    CheckBoxDiscloseName.Checked = se.Discloseeval;
                    TextBoxFavorite.Text = se.Favorite;
                    TextBoxImproved.Text = se.Improved;
                    TextBoxGeneralComments.Text = se.Generalcomments;
                }
            }
            else
            {
                Label1.Text = "You Must Be Logged In To Enter Evaluations";
                Label2.Text = "You Must Be Logged In To Enter Evaluations";

            }
        }
    }

    private void AddRadioButtonChoices(RadioButtonList RadioButtonListCourseAsWhole)
    {
        RadioButtonListCourseAsWhole.Items.Add(new ListItem("Poor", "1"));
        RadioButtonListCourseAsWhole.Items.Add(new ListItem("Fair", "2"));
        RadioButtonListCourseAsWhole.Items.Add(new ListItem("Good", "3"));
        RadioButtonListCourseAsWhole.Items.Add(new ListItem("Excellent", "4"));
        RadioButtonListCourseAsWhole.Items.Add(new ListItem("N/A", "5"));
    }
    protected void ButtonUpdate1_Click(object sender, EventArgs e)
    {
        this.UpdateEvalData();
    }
    protected void ButtonUpdate2_Click(object sender, EventArgs e)
    {
        this.UpdateEvalData();
    }

    private void UpdateEvalData()
    {

        HyperLinkReturn1.Visible = true;
        HyperLinkReturn2.Visible = true;
        HyperLinkReturn3.Visible = true;
        HyperLinkReturn4.Visible = true;
        HyperLink1.Visible = true;
        HyperLink2.Visible = true;
        
        if (Context.User.Identity.IsAuthenticated)   // should always be, but just check anyhow
        {
            SessionEvalsODS seODS = new SessionEvalsODS();
            List<CodeCampSV.SessionEvalsODS.DataObjectSessionEvals> liEvals =
                    seODS.GetByUsernameSessionId(Context.User.Identity.Name, sessionId);

            if (liEvals.Count == 0)
            {
                seODS.InsertAllSessionEvals(Context.User.Identity.Name,
                    DateTime.Now,
                    DateTime.Now,
                    ConvertRBToInt(RBLCourseAsWhole.SelectedValue),
                    ConvertRBToInt(RBLCourseContent.SelectedValue),
                    0,
                    ConvertRBToInt(RBLInstructorAbilityExplain.SelectedValue),
                    ConvertRBToInt(RBLInstructorEffective.SelectedValue),
                    ConvertRBToInt(RBLInstructorKnowledge.SelectedValue),
                    ConvertRBToInt(RBLQualityOfFacility.SelectedValue),
                    ConvertRBToInt(RBLOverallCodeCamp.SelectedValue),
                    ConvertRBToInt(RBLContentLevel.SelectedValue),
                    TextBoxFavorite.Text,
                    TextBoxImproved.Text,
                    TextBoxGeneralComments.Text,
                    CheckBoxDiscloseName.Checked,
                    sessionId);

                Label1.Text = "Evaluation Added";
                Label2.Text = "Evaluation Added";
            }
            else
            {
                seODS.UpdateAllSessionEvals(Context.User.Identity.Name,sessionId,
                    DateTime.Now,
                    ConvertRBToInt(RBLCourseAsWhole.SelectedValue),
                    ConvertRBToInt(RBLCourseContent.SelectedValue),
                    0,
                    ConvertRBToInt(RBLInstructorAbilityExplain.SelectedValue),
                    ConvertRBToInt(RBLInstructorEffective.SelectedValue),
                    ConvertRBToInt(RBLInstructorKnowledge.SelectedValue),
                    ConvertRBToInt(RBLQualityOfFacility.SelectedValue),
                    ConvertRBToInt(RBLOverallCodeCamp.SelectedValue),
                    ConvertRBToInt(RBLContentLevel.SelectedValue),
                    TextBoxFavorite.Text,
                    TextBoxImproved.Text,
                    TextBoxGeneralComments.Text,
                    CheckBoxDiscloseName.Checked);
                Label1.Text = "Evaluation Updated";
                Label2.Text = "Evaluation Updated";

            }

        }
    }

    private int ConvertRBToInt(string selectedValue)
    {
        int selVal = -1;
        bool good = Int32.TryParse(selectedValue,out selVal);
        return selVal;
    }
}
