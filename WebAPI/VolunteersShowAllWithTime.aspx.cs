using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class VolunteersShowAllWithTime : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LabelCodeCampYearId.Text = Utils.GetCurrentCodeCampYear().ToString();
    }

    protected void ButtonExportCSV_Click(object sender, EventArgs e)
    {
        try
        {
            string sql = String.Format(@"
                    SELECT 
                          dbo.VolunteerJob.JobStartTime,
                          dbo.VolunteerJob.JobEndTime,
                          dbo.Attendees.UserLastName,
                          dbo.Attendees.UserFirstName,
                          dbo.Attendees.PhoneNumber,
                          dbo.VolunteerJob.Description
                        FROM
                          dbo.VolunteerJob
                          INNER JOIN dbo.AttendeeVolunteer ON (dbo.VolunteerJob.Id = dbo.AttendeeVolunteer.VolunteerJobId)
                          INNER JOIN dbo.Attendees ON (dbo.AttendeeVolunteer.AttendeeId = dbo.Attendees.Id)
                        WHERE
                          dbo.VolunteerJob.CodeCampYearId = {0}
                        ORDER BY
                          dbo.VolunteerJob.JobStartTime,
                          dbo.Attendees.UserLastName,
                          dbo.Attendees.UserFirstName", Utils.GetCurrentCodeCampYear().ToString());


            SpreadsheetGear.IWorkbook workbook = SpreadsheetGear.Factory.GetWorkbook();
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets["Sheet1"];
            SpreadsheetGear.IRange cells = worksheet.Cells;
            //name.worksheet.Name = "SiliconValleyCodeCamp5";
            cells[0, 0].Value = "JobStartTime";
            cells[0, 1].Value = "JobEndTime";
            cells[0, 2].Value = "UserLastName";
            cells[0, 3].Value = "UserFirstName";
            cells[0, 4].Value = "PhoneNumber";
            cells[0, 5].Value = "Description";
            int row = 1;

            using (
                 var sqlConnection =
                     new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();
                var command = new SqlCommand(sql, sqlConnection);

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                   
                    cells[row, 0].Value = reader.IsDBNull(0) ? DateTime.Now : reader.GetDateTime(0);
                    cells[row, 1].Value = reader.IsDBNull(1) ? DateTime.Now : reader.GetDateTime(1);
                    cells[row, 2].Value = reader.IsDBNull(2) ? String.Empty : reader.GetString(2);
                    cells[row, 3].Value = reader.IsDBNull(3) ? String.Empty : reader.GetString(3);
                    cells[row, 4].Value = reader.IsDBNull(4) ? String.Empty : reader.GetString(4);
                    cells[row, 5].Value = reader.IsDBNull(5) ? String.Empty : reader.GetString(5);

                    row++;
                }
            }
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment; filename=svcc.xls");
            workbook.SaveToStream(Response.OutputStream, SpreadsheetGear.FileFormat.Excel8); Response.End();
        }

        catch (Exception)
        { Response.Write("Can Not Generate CSV File"); }
    }
}