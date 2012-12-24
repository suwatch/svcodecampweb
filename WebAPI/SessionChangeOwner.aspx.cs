using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

public partial class SessionChangeOwner : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void ButtonChangePresenter_Click(object sender, EventArgs e)
    {
        if (CheckBoxForReal.Checked)
        {
            string s = TextBoxStatus.Text ?? "";
            s = s + CheckBoxForReal.Checked.ToString();

            using (var sqlConnection =
                            new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();

                try
                {
                    string sqlUpdateTemplate = @"UPDATE SessionPresenter SET AttendeeId = (SELECT Id FROM Attendees WHERE Username='{0}') WHERE SessionId = {1} AND  AttendeeId = (SELECT AttendeesId FROM Sessions WHERE Id = {1})";
                    var sqlUpdate1 = String.Format(sqlUpdateTemplate, TextBoxNewPresenterUsername.Text, TextBoxSessionId.Text);
                    var command1 = new SqlCommand(sqlUpdate1, sqlConnection);
                    int numUpdated1 = command1.ExecuteNonQuery();

                    var sqlUpdate2 = String.Format(@"update sessions set Attendeesid = (SELECT Id FROM Attendees WHERE Username='{0}') where id = {1}", TextBoxNewPresenterUsername.Text, TextBoxSessionId.Text);
                    var command2 = new SqlCommand(sqlUpdate2, sqlConnection);
                    int numUpdated2 = command2.ExecuteNonQuery();

                    s = s + "update1: " + numUpdated1 + " update2: " + numUpdated2;



                }
                catch (Exception eee)
                {
                    throw new ApplicationException(eee.ToString());
                }
            }
            TextBoxStatus.Text = s;
        }

       

    }
}