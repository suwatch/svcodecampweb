using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using CodeCampSV;

public partial class ProfileInfoAccountCancel : BaseContentPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (User.Identity.IsAuthenticated)
        {
            if (CheckBox1.Checked)
            {
                ButtonCancel.Enabled = true;
            }
        }
    }
    protected void ButtonCancel_Click(object sender, EventArgs e)
    {

        if (User.Identity.IsAuthenticated)
        {

            //string deleteAttendeeSql =
            //    "delete from CodeCampEvals WHERE AttendeePKID in (SELECT PKID FROM Attendees WHERE Username = @Username);" +
            //    "delete from SessionEvals WHERE PKID in (SELECT PKID FROM Attendees WHERE Username = @Username);" +
            //    "delete from SessionAttendee WHERE attendees_username in (SELECT PKID FROM Attendees WHERE Username = @Username);" +
            //    "delete from ProfileData WHERE PKID in (SELECT PKID FROM Attendees WHERE Username = @Username);" +
            //    "delete from SessionTags WHERE sessionid in " +
            //    "(select id from SESSIONS where username = @Username); " +
            //    "delete from SESSIONS WHERE Username = @Username; " +
            //    "delete from Attendees where Username = @Username;";

            //try
            //{
            //    SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString);
            //    sqlConnection.Open();

            //    SqlCommand sqlCommand = new SqlCommand(deleteAttendeeSql, sqlConnection);
            //    sqlCommand.Parameters.Add("@Username", SqlDbType.VarChar).Value = Context.User.Identity.Name;
            //    sqlCommand.ExecuteNonQuery();
            //    sqlConnection.Close();
            //    sqlConnection.Dispose();
            //}
            //catch (Exception ee)
            //{
            //    throw new ApplicationException(ee.ToString());
            //}

            int attendeesId = Utils.GetAttendeesIdFromUsername(Context.User.Identity.Name);
            var attendeesCodeCampYear =
                (AttendeesCodeCampYearManager.I.GetJustBaseTableColumns(new AttendeesCodeCampYearQuery()
                {
                    CodeCampYearId = Utils.CurrentCodeCampYear,
                    AttendeesId = attendeesId
                })).FirstOrDefault();

            if (attendeesCodeCampYear != null)
            {
                AttendeesCodeCampYearManager.I.Delete(attendeesCodeCampYear);
            }


            // invalidate cache for adds when we add a new user
            CodeCampSV.Utils.ClearDisplayAdCache();

            //FormsAuthentication.SignOut();  

            LabelStatus.Text = "Registration Cancelled";
        }
    }
}
