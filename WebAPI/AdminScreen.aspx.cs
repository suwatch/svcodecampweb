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

public partial class AdminScreen : BaseContentPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void ButtonRemoveData_Click(object sender, EventArgs e)
    {
        SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString);
        sqlConnection.Open();

        try
        {
            FormsAuthentication.SignOut();  // verify signed out first

            // before doing this, make sure we are not in production (failsafe)
            string sqlCheck = "SELECT value FROM ConfigurationData WHERE keyname='Production'";
            SqlCommand sqlCommandCheck = new SqlCommand(sqlCheck, sqlConnection);
            string production = (string)sqlCommandCheck.ExecuteScalar();

            if (production.ToLower().Equals("false"))
            {
                string sqlDelete = "delete from Pictures;delete from sessionPictures;delete from SessionEvals;delete from ProfileData; delete from sessionAttendee;delete from CodeCampEvals;delete from wp_posts;delete from sessiontags;delete from sessions;delete from usersinroles;delete from attendees;";
                SqlCommand sqlCommand = new SqlCommand(sqlDelete, sqlConnection);
                sqlCommand.ExecuteNonQuery();
                LabelStatus.Text = "All Data Deleted";
            }
            else
            {
                LabelStatus.Text = "production not set to false " + production;
            }
        }
        catch (Exception ex)
        {
            LabelStatus.Text = ex.ToString();
        }

        sqlConnection.Close();
    }
    protected void ButtonAssignpkellneradmin_Click(object sender, EventArgs e)
    {
        try
        {
            if (!Roles.IsUserInRole("pkellner", "admin"))
            {
                Roles.AddUserToRole("pkellner", "admin");
            }
            LabelStatus.Text = "Success Assign pkellner to admin role";
        }
        catch (Exception ex)
        {
            LabelStatus.Text = "Failed " + ex.ToString();
        }
    }
    protected void ButtonSignOff_Click(object sender, EventArgs e)
    {
        try
        {
            FormsAuthentication.SignOut();
            LabelStatus.Text = "Success Signout";
        }
        catch (Exception ex)
        {
             LabelStatus.Text = "Failed " + ex.ToString();
        }
    }


    protected void ButtonRererralUrlData_Click(object sender, EventArgs e)
    {
        using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
        {
            sqlConnection.Open();
            const string sqlDelete = "truncate table ReferringUrlGroup;truncate table ReferringUrl;";
            using (var sqlCommand = new SqlCommand(sqlDelete, sqlConnection))
            {
                sqlCommand.ExecuteNonQuery();
            }
            LabelStatus.Text = "truncate table ReferringUrlGroup;truncate table ReferringUrl; all executed.";

        }
        
       


    }

    protected void ButtonTruncateTwitterData_Click(object sender, EventArgs e)
    {
        using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
        {
            sqlConnection.Open();
            const string sqlDelete = "truncate table TwitterUpdate;";
            using (var sqlCommand = new SqlCommand(sqlDelete, sqlConnection))
            {
                sqlCommand.ExecuteNonQuery();
            }
            LabelStatus.Text = "truncate table TwitterUpdate; all executed.";

        }

    }
    protected void ButtonTruncateLog4NetAll_Click(object sender, EventArgs e)
    {
        using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
        {
            sqlConnection.Open();
            const string sqlDelete = "truncate table Log4NetAll;";
            using (var sqlCommand = new SqlCommand(sqlDelete, sqlConnection))
            {
                sqlCommand.ExecuteNonQuery();
            }
            LabelStatus.Text = "truncate table Log4NetAll; all executed.";

        }
    }
}
