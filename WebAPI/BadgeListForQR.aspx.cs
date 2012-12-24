using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;

public partial class BadgeListForQR : System.Web.UI.Page
{
    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    if (!IsPostBack)
    //    {
    //        var startDate = new DateTime(2009, 1, 1, 0,0,0);
    //        TextBoxRegisterDate.Text = startDate.ToString();
    //    }

      

    //}
    //protected void ButtonDownload_Click(object sender, EventArgs e)
    //{
    //    if (!RadioButtonList1.SelectedValue.Equals("ThisYearAttendeesNoPresenters"))
    //    {
    //        //// get list of attendeeIds who are presenters
    //        //var recs = AttendeesManager.I.Get(new AttendeesQuery()
    //        //                                      {
                                                      
    //        //                                      });


    //        DateTime startDate = Convert.ToDateTime(TextBoxRegisterDate.Text);

    //        var recs = AttendeesCodeCampYearManager.I.Get(new AttendeesCodeCampYearQuery()
    //                                               {
    //                                                   CodeCampYearId = 5,
    //                                                   RegisterDateStart = startDate
                                                      
    //                                               });

    //        List<int> attendeesIdList = recs.Select(a => a.AttendeesId).ToList();

    //        var attendeesTemp = 
    //            AttendeesManager.I.Get(new AttendeesQuery
    //                                                       {
    //                                                           RespectQRCodes = true
    //                                                       });

    //        var attendeesResults = 
    //            attendeesTemp.Where(rec => attendeesIdList.Contains(rec.Id)).OrderBy(a => a.UserLastName).ThenBy(a => a.UserFirstName).ToList();

    //       // LabelStatus.Text = string.Format("{0} output.  starting: {1}", attendeesResults.Count, startDate);


    //        // Create a new workbook.
    //        SpreadsheetGear.IWorkbook workbook = SpreadsheetGear.Factory.GetWorkbook();
    //        SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets["Sheet1"];
    //        SpreadsheetGear.IRange cells = worksheet.Cells;
    //        // Set the worksheet name.
    //        worksheet.Name = "SiliconValleyCodeCamp5";

    //        cells[0, 0].Value = "Id";
    //        cells[0, 1].Value = "FirstName";
    //        cells[0, 2].Value = "LastName";
    //        cells[0, 3].Value = "Website";
    //        cells[0, 4].Value = "AddressLine1";
    //        cells[0, 5].Value = "City";
    //        cells[0, 6].Value = "State";
    //        cells[0, 7].Value = "Zipcode";
    //        cells[0, 8].Value = "Email";
    //        cells[0, 9].Value = "PhoneNumber";



    //        int row = 1;
    //        foreach (var rec in attendeesResults)
    //        {
    //            cells[row, 0].Value = rec.Id;
    //            cells[row, 1].Value = rec.UserFirstName;
    //            cells[row, 2].Value = rec.UserLastName;
    //            cells[row, 3].Value = rec.UserWebsite;
    //            cells[row, 4].Value = rec.AddressLine1;
    //            cells[row, 5].Value = rec.City;
    //            cells[row, 6].Value = rec.State;
    //            cells[row, 7].Value = rec.UserZipCode;
    //            cells[row, 8].Value = rec.Email;
    //            cells[row, 9].Value = rec.PhoneNumber;
    //            row++;
    //        }

           
    //        Response.Clear();
    //        Response.ContentType = "application/vnd.ms-excel";

    //        const string headerStringTemplate = "attachment; filename={0}_.xls";
    //        string dateTimeNow = DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "_" + DateTime.Now.Hour + DateTime.Now.Minute;

    //        Response.AddHeader("Content-Disposition",String.Format(headerStringTemplate,dateTimeNow));
    //        workbook.SaveToStream(Response.OutputStream, SpreadsheetGear.FileFormat.Excel8);

    //        Response.End();
    //    }
    //}
}