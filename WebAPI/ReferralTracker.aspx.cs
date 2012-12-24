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

public partial class ReferralTracker : BaseContentPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString);
        sqlConnection.Open();
        SqlDataReader reader = null;
        try
        {
            string sqlSelect =
                @"SELECT 
                      dbo.Attendees.ReferralGUID,
                      count(*) AS FIELD_1
                    FROM
                      dbo.Attendees
                    GROUP BY
                      dbo.Attendees.ReferralGUID";

            SqlCommand command = new SqlCommand(sqlSelect, sqlConnection);
            reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                try
                {
                    if (!reader.IsDBNull(0) && !reader.IsDBNull(1))
                    {
                        Guid PKID = reader.GetGuid(0);
                        int cnt = reader.GetInt32(1);
                        string userName = CodeCampSV.Utils.GetAttendeeUsernameByGUID(PKID.ToString());
                        string name = CodeCampSV.Utils.GetAttendeeNameByUsername(userName);
                        if (cnt > 1)
                        {
                            ListBox1.Items.Add(name + " " + cnt.ToString());
                        }
                    }
                }
                catch (Exception)
                {
                    
                    
                }
            }
        }
        catch (Exception eee1)
        {
            throw new ApplicationException(eee1.ToString());
        }
        finally
        {
            if (reader != null) reader.Dispose();
        }
        sqlConnection.Close();


    }
}
