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
using System.Data.SqlClient;
using CodeCampSV;
using System.Collections.Generic;

public partial class CodeCampEval : BaseContentPage
{

    

    public string GuidString
    {
        get 
        {
            if (ViewState["GuidString"] == null)
            {
                return string.Empty;
            }
            else
            {
                return (string) ViewState["GuidString"];
            }
        }
        set 
        { 
            ViewState["GuidString"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Context.User.Identity.IsAuthenticated)
        {
            Response.Redirect("~/Default.aspx", true);
        }

        if (!IsPostBack)
        {
            this.AddRadioButtonChoices(RBLMetExpectations);
            this.AddRadioButtonChoices(RBLWillAttendAgain);
            this.AddRadioButtonChoices(RBLEnjoyedFreeFood);
            this.AddRadioButtonChoices(RBLSessionsVariedEnough);
            this.AddRadioButtonChoices(RBLFoothillGoodVenue);
            this.AddRadioButtonChoices(RBLWishToldMoreFriends);
            this.AddRadioButtonChoices(RBLEventWellPlanned);
            this.AddRadioButtonChoices(RBLWirelessInternetImportant);
            this.AddRadioButtonChoices(RBLWiredInternetImportant);
            this.AddRadioButtonChoices(RBLLikedEmailUpdates);
            this.AddRadioButtonChoices(RBLLikedRSSUpdates);
            this.AddRadioButtonChoices(RBLEnoughSessionsMyLevel);


            
            if (Request.Params["PKID"] != null)
            {
                GuidString = Request.Params["PKID"];
                string username = CodeCampSV.Utils.GetAttendeeUsernameByGUID(GuidString);
                if (!System.Web.Security.Roles.IsUserInRole(username, CodeCampSV.Utils.GetAdminRoleName()))
                {
                    if (!string.IsNullOrEmpty(username))
                    {
                        if (User.Identity.IsAuthenticated)
                        {
                            FormsAuthentication.SignOut();
                        }
                        FormsAuthentication.SetAuthCookie(username, true);
                        Response.Redirect("~/CodeCampEval.aspx", true);
                    }
                }
                else
                {
                    // user is admin so toss them out
                    Response.Redirect("~/Default.aspx", true);
                }
            }
            else if (Context.User.Identity.IsAuthenticated)
            {
                GuidString = CodeCampSV.Utils.GetAttendeePKIDByUsername(User.Identity.Name);
            }

            if (!String.IsNullOrEmpty(GuidString))
            {
                // Figure out if eval exists for this person.  If not, add one.
                int num = 0;
                try
                {
                    using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
                    {
                        sqlConnection.Open();

                        const string sqlSelect = "SELECT COUNT(*) FROM CodeCampEvals WHERE AttendeePKID=@AttendeePKID AND CodeCampYearId = @CodeCampYearId";
                        using (var sqlCommand = new SqlCommand(sqlSelect, sqlConnection))
                        {
                            sqlCommand.Parameters.Add("@AttendeePKID", SqlDbType.UniqueIdentifier).Value = new Guid(GuidString);
                            sqlCommand.Parameters.Add("@CodeCampYearId", SqlDbType.Int).Value = Utils.GetCurrentCodeCampYear();
                            num = (int)sqlCommand.ExecuteScalar();
                        }

                        if (num == 0)
                        {
                            const string sqlInsert = "INSERT INTO CodeCampEvals (AttendeePKID,DateSubmitted,CodeCampYearId) Values (@AttendeePKID,@DateSubmitted,@CodeCampYearId)";
                            using (var sqlCommandInsert = new SqlCommand(sqlInsert, sqlConnection))
                            {
                                sqlCommandInsert.Parameters.Add("@AttendeePKID", SqlDbType.UniqueIdentifier).Value = new Guid(GuidString);
                                sqlCommandInsert.Parameters.Add("@DateSubmitted", SqlDbType.DateTime).Value = DateTime.Now;
                                sqlCommandInsert.Parameters.Add("@CodeCampYearId", SqlDbType.Int).Value =
                                    Utils.GetCurrentCodeCampYear();
                                sqlCommandInsert.ExecuteScalar();
                            }
                        }
                    }
                }
                catch (Exception ee)
                {
                    throw new ApplicationException(ee.ToString());
                }
            }

            // Now, if we still don't have a guidString, then bail
            if (!String.IsNullOrEmpty(GuidString))
            {
                ButtonUpdate1.Enabled = true;
                ButtonUpdate2.Enabled = true;

                CodeCampEvalsODS cceODS = new CodeCampEvalsODS();
                List<CodeCampEvalsODS.DataObjectCodeCampEvals> li = cceODS.GetByPKID(new Guid(GuidString));
                if (li.Count == 1)
                {
                    CodeCampEvalsODS.DataObjectCodeCampEvals eval = li[0];
                    CheckBoxCodeCampOnly.Checked = eval.Attendedcconly;
                    CheckBoxCodeCampAndVistaFair.Checked = eval.Attendedvistafairandcc;
                    CheckBoxVistaFairOnly.Checked = eval.Attendedvistafaironly;

                    RadioButtonListSponsorship.SelectedValue = eval.Rathernosponsorandnofreefood.ToString();

                    //CheckBoxNoCorpSponsors.Checked = eval.Rathernosponsorandnofreefood;
                    CheckBoxVistaFairOnly.Checked = eval.Attendedvistafaironly;
                    CheckBoxCodeCampAndVistaFair.Checked = eval.Attendedvistafairandcc;
                    CheckBoxCodeCampOnly.Checked = eval.Attendedcconly;
                    TextBoxWhatEnjoyMost.Text = eval.Bestpartofevent;
                    TextBoxWhatChanges.Text = eval.Whatwouldyouchange;
                    TextBoxIfNotSatisfiedWhy.Text = eval.Notsatisfiedwhy;
                    TextBoxWhatFoothillCoursesAdded.Text = eval.Whatfoothillclassestoadd;
                    CheckBoxLongTermPlanning.Checked = eval.Interesteinlongtermplanning;
                    CheckBoxWebSiteBackEnd.Checked = eval.Interesteinwebbackend;
                    CheckBoxWebSiteCss.Checked = eval.Interestedinwebfrontend;
                    CheckBoxSessionReview.Checked = eval.Interesteinlongsessionreviewpanel;
                    CheckBoxContributorSolicitation.Checked = eval.Interesteincontributorsolicitation;
                    CheckBoxActAsContributor.Checked = eval.Interesteinbeingcontributor;
                    CheckBoxPreEventVolunteer.Checked = eval.Interesteinbeforeevent;
                    CheckBoxDayOfEventVolunteer.Checked = eval.Interesteindayofevent;
                    CheckBoxEventTearDown.Checked = eval.Interesteineventteardown;
                    CheckBoxAfterEventVolunteer.Checked = eval.Interesteinafterevent;

                    this.SetSelectedValue(RBLMetExpectations, eval.Metexpectations);

                    this.SetSelectedValue(RBLMetExpectations, eval.Metexpectations);
                    this.SetSelectedValue(RBLWillAttendAgain, eval.Plantoattendagain);
                    this.SetSelectedValue(RBLEnjoyedFreeFood, eval.Enjoyedfreefood);
                    this.SetSelectedValue(RBLSessionsVariedEnough, eval.Sessionsvariedenough);
                    this.SetSelectedValue(RBLEnoughSessionsMyLevel, eval.Enoughsessionsatmylevel);
                    this.SetSelectedValue(RBLFoothillGoodVenue, eval.Foothillgoodvenue);
                    this.SetSelectedValue(RBLWishToldMoreFriends, eval.Wishtoldmorefriends);
                    this.SetSelectedValue(RBLEventWellPlanned, eval.Eventwellplanned);
                    this.SetSelectedValue(RBLWirelessInternetImportant, eval.Wirelessaccessimportant);
                    this.SetSelectedValue(RBLWiredInternetImportant, eval.Wiredaccessimportant);
                    this.SetSelectedValue(RBLLikedEmailUpdates, eval.Likereceivingupdatebyemail);
                    this.SetSelectedValue(RBLLikedRSSUpdates, eval.Likereceivingupdatebybyrssfeed);

                    TextBoxContactEmail.Text = eval.Forvolunteeringbestwaytocontactemail;
                    TextBoxPhoneNumber.Text = eval.Forvolunteeringbestwaytocontactphone;
                }
            }
            else
            {
                Response.Redirect("~/Default.aspx");
            }
        }

       
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        Page.DataBind();
    }

    private void SetSelectedValue(RadioButtonList radioButtonList, int selectedValueInt)
    {
        if (selectedValueInt != 0)
        {
            radioButtonList.SelectedValue = selectedValueInt.ToString();
        }
    }

    private void AddRadioButtonChoices(RadioButtonList RadioButtonListCourseAsWhole)
    {
        RadioButtonListCourseAsWhole.Items.Add(new ListItem("Strongly Agree", "1"));
        RadioButtonListCourseAsWhole.Items.Add(new ListItem("Agree", "2"));
        RadioButtonListCourseAsWhole.Items.Add(new ListItem("Disagree", "3"));
        RadioButtonListCourseAsWhole.Items.Add(new ListItem("Strongly Disagree", "4"));

        //RadioButtonListCourseAsWhole.Items.Add(new ListItem("N/A", "0"));
        RadioButtonListCourseAsWhole.Items.Add(new ListItem("N/A", "5"));
    }
    protected void ButtonUpdate1_Click(object sender, EventArgs e)
    {
        this.UpdateEvalData();
    }

    private void UpdateEvalData()
    {
        CodeCampEvalsODS cceODS = new CodeCampEvalsODS();
        cceODS.UpdateAllCodeCampEvals(new Guid(GuidString),
            ConvertSelectedValueToInt32(RBLMetExpectations.SelectedValue),
            ConvertSelectedValueToInt32(RBLWillAttendAgain.SelectedValue),
            ConvertSelectedValueToInt32(RBLEnjoyedFreeFood.SelectedValue),
            ConvertSelectedValueToInt32(RBLSessionsVariedEnough.SelectedValue),
            ConvertSelectedValueToInt32(RBLEnoughSessionsMyLevel.SelectedValue),
            ConvertSelectedValueToInt32(RBLFoothillGoodVenue.SelectedValue),
            ConvertSelectedValueToInt32(RBLWishToldMoreFriends.SelectedValue),
            ConvertSelectedValueToInt32(RBLEventWellPlanned.SelectedValue),
            ConvertSelectedValueToInt32(RBLWirelessInternetImportant.SelectedValue),
            ConvertSelectedValueToInt32(RBLWiredInternetImportant.SelectedValue),
            ConvertSelectedValueToInt32(RBLLikedEmailUpdates.SelectedValue),
            ConvertSelectedValueToInt32(RBLLikedRSSUpdates.SelectedValue),
            ConvertSelectedValueToInt32(RadioButtonListSponsorship.SelectedValue),
            CheckBoxVistaFairOnly.Checked,
            CheckBoxCodeCampAndVistaFair.Checked,
            CheckBoxCodeCampOnly.Checked,
            TextBoxWhatEnjoyMost.Text,
            TextBoxWhatChanges.Text,
            TextBoxIfNotSatisfiedWhy.Text,
            TextBoxWhatFoothillCoursesAdded.Text,
            CheckBoxLongTermPlanning.Checked,
            CheckBoxWebSiteBackEnd.Checked,
            CheckBoxWebSiteCss.Checked,
            CheckBoxSessionReview.Checked,
            CheckBoxContributorSolicitation.Checked,
            CheckBoxActAsContributor.Checked,
            CheckBoxPreEventVolunteer.Checked,
            CheckBoxDayOfEventVolunteer.Checked,
            CheckBoxEventTearDown.Checked,
            CheckBoxAfterEventVolunteer.Checked,
            TextBoxContactEmail.Text,
            TextBoxPhoneNumber.Text,
            DateTime.Now);

        Label1.Text = "Evaluation Submitted.  Thank you";
        Label2.Text = "Evaluation Submitted.  Thank you";

        // ~/CodeCampEval.aspx?Return=MyEvals
        if (Request.QueryString["Return"] != null &&
            Request.QueryString["Return"].Equals("MyEvals"))
        {
            Response.Redirect("~/MyEvals.aspx");
        }
        else
        {
            Response.Redirect("~/CodeCampEvalsSuccess.aspx");
        }
       

    }

    private int ConvertSelectedValueToInt32(string strValue)
    {
        if (String.IsNullOrEmpty(strValue))
        {
            return 0;
        }
        else
        {
            return Convert.ToInt32(strValue);
        }
    }
    protected void ButtonUpdate2_Click(object sender, EventArgs e)
    {
        this.UpdateEvalData();
    }
    protected void RadioButtonListSponsorship_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected bool IsModeSurvey()
    {
        return !CheckBoxAnswersMode.Checked;
    }

    protected string GetCheckBoxResults(string columnName)
    {
        if (String.IsNullOrEmpty(columnName))
        {
            return "ColumnName Not Assigned";
        }

        string sqlSelect = "SELECT count(id)," + columnName + " FROM CodeCampEvals Group By " + columnName;
        int numberChecked = 0;
        int numberNotChecked = 0;
        string retString = string.Empty;
        string percentChecked = string.Empty;
        string percentNotChecked = string.Empty;
       
        
   
        // assume no nulls;
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
        {
            sqlConnection.Open();
            SqlDataReader reader = null;
            try
            {
                SqlCommand command = new SqlCommand(sqlSelect, sqlConnection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (!reader.IsDBNull(1))
                    {
                        if (reader.GetBoolean(1) == true)
                        {
                            if (!reader.IsDBNull(0))
                            {
                                numberChecked = reader.GetInt32(0);
                            }
                        }
                        else
                        {
                            if (!reader.IsDBNull(0))
                            {
                                numberNotChecked = reader.GetInt32(0);
                            }
                        }
                    }
                }
            }
            catch (Exception eee)
            {
                throw new ApplicationException(eee.ToString());
            }
        }

        if (numberChecked + numberNotChecked > 0)
        {
            percentChecked = " (" + Convert.ToInt32((Convert.ToDecimal(numberChecked) * 100) /
                (Convert.ToDecimal(numberChecked) + Convert.ToDecimal(numberNotChecked))) + "%)";

            percentNotChecked = " (" + Convert.ToInt32((Convert.ToDecimal(numberNotChecked) * 100) /
                (Convert.ToDecimal(numberChecked) + Convert.ToDecimal(numberNotChecked))) + "%)";
        }
        retString = numberChecked.ToString() + " Checked " + percentChecked + "; " + numberNotChecked.ToString() +
            " Not Checked" + percentNotChecked + ".";

        // 3 Checked (33%); 4 Not Checked (67%)
        return retString;

    }

    protected string GetRBLResults(string columnName)
    {
        if (String.IsNullOrEmpty(columnName))
        {
            return "ColumnName Not Assigned";
        }

        string sqlSelect = "SELECT count(id)," + columnName + " FROM CodeCampEvals Group By " + columnName;
        int number0 = 0;
        int number1 = 0;
        int number2 = 0;
        int number3 = 0;
        int number4 = 0;
        
        string retString = string.Empty;
        string percent0 = string.Empty;
        string percent1 = string.Empty;
        string percent2 = string.Empty;
        string percent3 = string.Empty;
        string percent4 = string.Empty;
        

        // assume no nulls;
        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
        {
            sqlConnection.Open();
            SqlDataReader reader = null;
            try
            {
                SqlCommand command = new SqlCommand(sqlSelect, sqlConnection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (!reader.IsDBNull(1))
                    {
                        if (reader.GetInt32(1) == 0)
                        {
                            if (!reader.IsDBNull(1))
                            {
                                number0 = reader.GetInt32(0);
                            }
                        }
                        else if (reader.GetInt32(1) == 1)
                        {
                            if (!reader.IsDBNull(1))
                            {
                                number1 = reader.GetInt32(0);
                            }
                        }
                        else if (reader.GetInt32(1) == 2)
                        {
                            if (!reader.IsDBNull(1))
                            {
                                number2 = reader.GetInt32(0);
                            }
                        }
                        else if (reader.GetInt32(1) == 3)
                        {
                            if (!reader.IsDBNull(1))
                            {
                                number3 = reader.GetInt32(0);
                            }
                        }
                        else if (reader.GetInt32(1) == 4)
                        {
                            if (!reader.IsDBNull(1))
                            {
                                number4 = reader.GetInt32(0);
                            }
                        }

                    }
                    
                }
            }
            catch (Exception eee)
            {
                throw new ApplicationException(eee.ToString());
            }
        }

        int total = number0 + number1 + number2 + number3 + number4;
        if (total > 0)
        {

            percent1 = " (" + 
                Convert.ToInt32((Convert.ToDecimal(number1) * 100) / Convert.ToDecimal(total))
                + "%)";
            

            percent2 = " (" + Convert.ToInt32((Convert.ToDecimal(number2) * 100) /
                 Convert.ToDecimal(total)) + "%)";

            percent3 = " (" + Convert.ToInt32((Convert.ToDecimal(number3) * 100) /
                 Convert.ToDecimal(total)) + "%)";

            percent4 = " (" + Convert.ToInt32((Convert.ToDecimal(number4) * 100) /
                 Convert.ToDecimal(total)) + "%)";

            percent0 = " (" + Convert.ToInt32((Convert.ToDecimal(number0) * 100) /
                Convert.ToDecimal(total)) + "%); ";
        }
        retString = "1 " + number1.ToString() + "/" + percent1 + "; " +
            "2 " + number2.ToString() + "/" + percent2 + "; " +
            "3 " + number3.ToString() + "/" + percent3 + "; " +
            "4 " + number4.ToString() + "/" + percent4 + "; " +
            "0 " + number0.ToString() + "/" + percent0;
        // 1 17/25%; 2 1/50%; 3 10/10%; 4 3/5%; 0 3/5%;

        return retString;

    }

    protected bool IsRoleSurveyViewerOrAdmin()
    {
        if (CodeCampSV.Utils.CheckUserIsAdmin() || CodeCampSV.Utils.CheckUserIsSurveyViewer())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
}
