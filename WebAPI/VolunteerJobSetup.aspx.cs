using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;
using System.Text;

public partial class VolunteerJobSetup : System.Web.UI.Page
{
    private string _codeCampYear = "";
    private DateTime _codeCampSaturday;
    private DateTime _codeCampSunday;

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var codeCampYearResult = CodeCampYearManager.I.Get(new CodeCampYearQuery
                                                                   {
                                                                       Id = Utils.GetCurrentCodeCampYear()
                                                                       // get from dropdownlist on top of page
                                                                   }).FirstOrDefault();

           

            if (codeCampYearResult != null)
            {
                _codeCampYear = codeCampYearResult.CampStartDate.Year.ToString();
                _codeCampSaturday = codeCampYearResult.CampStartDate;
                _codeCampSunday = codeCampYearResult.CampEndDate;

                LabelCodeCampStartDate.Text = _codeCampSaturday.ToString();
                LabelCodeCampEndDate.Text = _codeCampSunday.ToString();

                // don't let someone add jobs in previous year except admin
                if (codeCampYearResult.Id != Utils.CurrentCodeCampYear)
                {
                    if (!Utils.CheckUserIsAdmin())
                    {
                        DetailsView1.Visible = false;
                    }
                }

                LiteralSaturdayDateFormat.Text = codeCampYearResult.CampStartDate.ToString();

                LabelSaturday.Text = codeCampYearResult.CampStartDate.ToString();
                LabelSunday.Text = codeCampYearResult.CampEndDate.ToString();

                SqlDataSourceVolunteerJobs.SelectCommand = GetSqlSelect(CheckBoxSaturday.Checked, CheckBoxSunday.Checked);

            }

        }
    }

    private string GetSqlSelect(bool saturday, bool sunday)
    {
        var sb = new StringBuilder();
        //if (saturday && sunday)
        //{
            sb.Append(String.Format(
                  @"SELECT [Id], [Description], [JobStartTime], [JobEndTime], [NumberNeeded] FROM [VolunteerJob] 
                   WHERE convert(varchar(4),year(JobStartTime))  = '{0}'
                   ORDER BY [JobStartTime], [Description]",
                  _codeCampYear));
        
        //}
        //else if (saturday || sunday)
        //{
            
        //    sb.Append(
        //        @"SELECT [Id], [Description], [JobStartTime], [JobEndTime], [NumberNeeded] FROM [VolunteerJob] WHERE  ");

        //    if (saturday)
        //    {
        //        sb.Append("(DateAdd(day, datediff(day, 0, JobStartTime), 0) = @CodeCampStartDate)");
        //    }

        //    if (sunday)
        //    {
        //        sb.Append("(DateAdd(day, datediff(day, 0, JobStartTime), 0) = @CodeCampEndDate)");
        //    }

        //    sb.Append(@"ORDER BY [JobStartTime], [Description]");
        //}
       
        return sb.ToString();
    }
   
   
    protected void CheckBoxSaturday_CheckedChanged(object sender, EventArgs e)
    {
        SqlDataSourceVolunteerJobs.SelectCommand = GetSqlSelect(CheckBoxSaturday.Checked, CheckBoxSunday.Checked);
        GridViewVolunteerJobs.DataBind();

    }
    protected void CheckBoxSunday_CheckedChanged(object sender, EventArgs e)
    {
        SqlDataSourceVolunteerJobs.SelectCommand = GetSqlSelect(CheckBoxSaturday.Checked, CheckBoxSunday.Checked);
        GridViewVolunteerJobs.DataBind();
    }


    //protected string GetJobFullTime(string startJobs,string endJobs)
    //{
    //    DateTime startJob = Convert.ToDateTime(startJobs);
    //    DateTime endJob = Convert.ToDateTime(endJobs);
        
    //    string str = startJob.DayOfWeek + " " + startJob.Hour + ":" + startJob.Minute + " => " + endJob.Hour + ":" +
    //                 endJob.Minute + "  ";
    //    return str;
    //}

    protected string GetJobFullTime(DateTime startJob, DateTime endJob)
    {


        string str = "  " + startJob.DayOfWeek + " " +
                     ShowHour(startJob.Hour) + ":" + ShowMinute(startJob.Minute) + " " +
                     ShowAMorPM(startJob.Hour) +
                     " => " +
                     ShowHour(endJob.Hour) + ":" + ShowMinute(endJob.Minute) + " " +
                     ShowAMorPM(endJob.Hour);
        return str;
    }

    private string ShowHour(int hour)
    {
        if (hour > 12)
        {
            hour -= 12;
        }
        return hour.ToString();
    }

    private string ShowAMorPM(int hour)
    {
        if (hour > 12)
        {
            return "PM";
        }
        else
        {
            return "AM";
        }
    }

    private string ShowMinute(int minute)
    {
        if (minute == 0)
        {
            return "00";
        }
        else if (minute < 10)
        {
            return "0" + minute.ToString();
        }
        else
        {
            return minute.ToString();
        }
    }
    protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {

        ExtractPrivateVars();


        SqlDataSourceVolunteerJobs.SelectCommand = GetSqlSelect(CheckBoxSaturday.Checked, CheckBoxSunday.Checked);
        
        GridViewVolunteerJobs.DataBind();
    }

    private void ExtractPrivateVars()
    {
        var codeCampYearResult = CodeCampYearManager.I.Get(new CodeCampYearQuery
        {
            Id = Utils.GetCurrentCodeCampYear()
            // get from dropdownlist on top of page
        }).FirstOrDefault();



        if (codeCampYearResult != null)
        {
            _codeCampYear = codeCampYearResult.CampStartDate.Year.ToString();
            _codeCampSaturday = codeCampYearResult.CampStartDate;
            _codeCampSunday = codeCampYearResult.CampEndDate;
        }
    }
    protected void DetailsView1_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        ExtractPrivateVars();
        SqlDataSourceVolunteerJobs.SelectCommand = GetSqlSelect(CheckBoxSaturday.Checked, CheckBoxSunday.Checked);
        GridViewVolunteerJobs.DataBind();
    }
    protected void DetailsView1_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
    {
        SqlDataSourceVolunteerJobs.SelectCommand = GetSqlSelect(CheckBoxSaturday.Checked, CheckBoxSunday.Checked);
        GridViewVolunteerJobs.DataBind();
    }
   
    protected void ButtonCopyPreviousYearsJobs_Click(object sender, EventArgs e)
    {
        var codeCampYearResultThisYear = CodeCampYearManager.I.Get(new CodeCampYearQuery
                                                                       {
                                                                           Id = Utils.CurrentCodeCampYear
                                                                       }).FirstOrDefault();

         var codeCampYearResultLastYear = CodeCampYearManager.I.Get(new CodeCampYearQuery
                                                                       {
                                                                           Id = Utils.CurrentCodeCampYear
                                                                       }).FirstOrDefault();


        if (codeCampYearResultThisYear != null && codeCampYearResultLastYear != null)
        {
            // get this years jobs
            var cnt = (VolunteerJobManager.I.Get(new VolunteerJobQuery()
                                                     {
                                                         CodeCampYearId = Utils.CurrentCodeCampYear
                                                     })).Count;
            // don't do this unless no jobs this year
            if (cnt == 0)
            {


                var jobsLastYear = VolunteerJobManager.I.Get(new VolunteerJobQuery()
                                                                 {
                                                                     CodeCampYearId = Utils.CurrentCodeCampYear - 1
                                                                 });

                DateTime lastYearSaturday = codeCampYearResultLastYear.CampStartDate;
                DateTime lastYearSunday = codeCampYearResultLastYear.CampEndDate;


                foreach (var rec in jobsLastYear)
                {
                    rec.CodeCampYearId = Utils.CurrentCodeCampYear;
                    bool foundOne = false;
                    if (rec.JobStartTime.DayOfWeek == DayOfWeek.Saturday)
                    {
                        rec.JobStartTime = new DateTime(
                            codeCampYearResultThisYear.CampStartDate.Year,
                             codeCampYearResultThisYear.CampStartDate.Month,
                             codeCampYearResultThisYear.CampStartDate.Day,
                             rec.JobStartTime.Hour,
                             rec.JobStartTime.Minute,0);

                        rec.JobEndTime = new DateTime(
                           codeCampYearResultThisYear.CampStartDate.Year,
                            codeCampYearResultThisYear.CampStartDate.Month,
                            codeCampYearResultThisYear.CampStartDate.Day,
                            rec.JobEndTime.Hour,
                            rec.JobEndTime.Minute, 0);
                        foundOne = true;
                    }
                    else if (rec.JobStartTime.DayOfWeek == DayOfWeek.Sunday)
                    {
                        rec.JobStartTime = new DateTime(
                            codeCampYearResultThisYear.CampEndDate.Year,
                             codeCampYearResultThisYear.CampEndDate.Month,
                             codeCampYearResultThisYear.CampEndDate.Day,
                             rec.JobStartTime.Hour,
                             rec.JobStartTime.Minute, 0);

                        rec.JobEndTime = new DateTime(
                           codeCampYearResultThisYear.CampEndDate.Year,
                            codeCampYearResultThisYear.CampEndDate.Month,
                            codeCampYearResultThisYear.CampEndDate.Day,
                            rec.JobEndTime.Hour,
                            rec.JobEndTime.Minute, 0);
                        foundOne = true;
                    }
                    else if (rec.JobStartTime.DayOfWeek == DayOfWeek.Friday)
                    {
                        DateTime dayBeforeCodeCampStarts =
                            codeCampYearResultThisYear.CampStartDate.Subtract(new TimeSpan(1, 0, 0, 0));


                        rec.JobStartTime = new DateTime(
                            dayBeforeCodeCampStarts.Year,
                             dayBeforeCodeCampStarts.Month,
                             dayBeforeCodeCampStarts.Day,
                             rec.JobStartTime.Hour,
                             rec.JobStartTime.Minute, 0);

                        rec.JobEndTime = new DateTime(
                           dayBeforeCodeCampStarts.Year,
                            dayBeforeCodeCampStarts.Month,
                            dayBeforeCodeCampStarts.Day,
                            rec.JobEndTime.Hour,
                            rec.JobEndTime.Minute, 0);
                        foundOne = true;
                    }

                    // if any volunteer jobs entered that are not fri,sat or sunday then skip
                    if (foundOne)
                    {
                        VolunteerJobManager.I.Insert(rec);
                    }
                }
            }
        }

        SqlDataSourceVolunteerJobs.SelectCommand = GetSqlSelect(CheckBoxSaturday.Checked, CheckBoxSunday.Checked);
        GridViewVolunteerJobs.DataBind();
    }

    protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        var codeCampYearResultThisYear = CodeCampYearManager.I.Get(new CodeCampYearQuery
        {
            Id = Utils.CurrentCodeCampYear
        }).FirstOrDefault();

        if (codeCampYearResultThisYear != null)
        {
            var startJob = e.NewValues["JobStartTime"];
            var endJob = e.NewValues["JobEndTime"];

            DateTime start = Convert.ToDateTime(startJob);
            DateTime end = Convert.ToDateTime(endJob);

            if (start.Year != codeCampYearResultThisYear.CampStartDate.Year || end.Year != codeCampYearResultThisYear.CampStartDate.Year)
            {
                e.Cancel = true;
                Literal1.Text = "can not add or update jobs from previous years";
            }

        }

    }

    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        e.Values["CodeCampYearId"] = Utils.GetCurrentCodeCampYear();

        var codeCampYearResultThisYear = CodeCampYearManager.I.Get(new CodeCampYearQuery
        {
            Id = Utils.CurrentCodeCampYear
        }).FirstOrDefault();

        if (codeCampYearResultThisYear != null)
        {
            var startJob = e.Values["JobStartTime"];
            var endJob = e.Values["JobEndTime"];

            DateTime start = Convert.ToDateTime(startJob);
            DateTime end = Convert.ToDateTime(endJob);

            if (start.Year != codeCampYearResultThisYear.CampStartDate.Year || end.Year != codeCampYearResultThisYear.CampStartDate.Year)
            {
                e.Cancel = true;
                Literal1.Text = "can not add or update jobs from previous years";
            }

        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
}