using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;

public partial class Jobs1 : System.Web.UI.Page
{
    //Dictionary<int,string> _jobListingDict = new Dictionary<int, string>();

    /// <summary>
    /// All with JobName Engineers (any company)
    /// http://localhost:10299/Web/Jobs.aspx?JobName=Engineers
    ///
    /// Just Engineers at box.net
    /// http://localhost:10299/Web/Jobs.aspx?JobName=Engineers@Box.net
    ///
    /// all jobs at box.net
    /// http://localhost:10299/Web/Jobs.aspx?JobName=@Box.net
    /// or of course
    /// http://localhost:10299/Web/Jobs.aspx?CompanyId=6
    /// http://localhost:10299/Web/Jobs.aspx?JobId=9
    ///
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {

        //SELECT 
        //            Id,
        //            JobName,
        //            JobCompanyName,
        //            JobLocation,
        //            JobURL,
        //            JobBrief,
        //            StartRunDate,
        //            EndRunDate,
        //            HideListing
        //     FROM SponsorListJobListing
        //     WHERE GETDATE() &gt;= StartRunDate AND
        //           GETDATE() &lt;= EndRunDate AND
        //           HideListing = 1
        //     ORDER BY StartRunDate DESC


//        var sponsorListIds = new List<int>();
//        var jobIds = new List<int>();


//        if (Request.QueryString["JobName"] != null && Request.QueryString["JobName"].Length > 0)
//        {
//            string jobSearch = Request.QueryString["JobName"];
//            int pos1 = jobSearch.IndexOf("@");

//            var jobTitle = jobSearch;
//            if (pos1 > 0)
//            {
//                jobTitle = jobSearch.Substring(0, pos1);
//            }
//            if (!String.IsNullOrEmpty(jobTitle))
//            {
//                jobIds.AddRange(
//                   SponsorListJobListingManager.I.Get(new SponsorListJobListingQuery { JobName = jobTitle }).Select(a => a.Id).ToList());
//            }

//            if (pos1 >= 0 && jobSearch.Length > pos1 + 1)
//            {
//                string companyName = jobSearch.Substring(pos1 + 1);
//                var companyResult = SponsorListManager.I.Get(new SponsorListQuery() { SponsorName = companyName }).FirstOrDefault();
//                if (companyResult != null)
//                {
//                    sponsorListIds.Add(companyResult.Id);
//                }
//            }
//        }

//        var stringBuilder = new StringBuilder();
//        stringBuilder.Append(
//            @"
//                 SELECT Id,
//                        JobName,
//                        JobCompanyName,
//                        JobLocation,
//                        JobURL,
//                        JobBrief,
//                        StartRunDate,
//                        EndRunDate,
//                        HideListing
//                 FROM SponsorListJobListing
//                 WHERE GETDATE() >= StartRunDate AND
//                       GETDATE() <= EndRunDate AND
//                       HideListing = 1 ");
//              //   ORDER BY StartRunDate DESC");


////                 SELECT dbo.JobListing.Id,
////                        dbo.JobListing.JobName,
////                        dbo.JobListing.JobCompanyName,
////                        dbo.JobListing.JobLocation,
////                        dbo.JobListing.JobURL,
////                        dbo.JobListing.JobBrief,
////                        dbo.JobListingDates.StartRunDate,
////                        dbo.JobListingDates.EndRunDate,
////                        dbo.JobListingDates.HideListing
////                 FROM dbo.JobListing
////                      INNER JOIN dbo.JobListingDates ON (dbo.JobListing.Id =
////                      dbo.JobListingDates.JobListingId)
////                 WHERE GETDATE() >= dbo.JobListingDates.StartRunDate AND
////                       GETDATE() <= dbo.JobListingDates.EndRunDate AND
////                       JobListingDates.HideListing = 1");

//        if (Request.QueryString["JobId"] != null)
//        {
//            int jobIdTemp;
//            if (Int32.TryParse(Request.QueryString["JobId"], out jobIdTemp) && jobIdTemp > 0)
//            {
//                jobIds.Add(jobIdTemp);
//            }
//        }

//        if (jobIds.Count > 0)
//        {
//            // " AND (dbo.JobListing.Id=0 OR dbo.JobListing.Id=2 OR dbo.JobListing.Id=7)"

//            stringBuilder.Append(" AND ( ");
//            for (var i = 0; i < jobIds.Count; i++)
//            {
//                stringBuilder.Append(" Id=" + jobIds[i]);
//                if (i < jobIds.Count - 1)
//                {
//                    stringBuilder.Append(" OR ");
//                }
//            }
//            stringBuilder.Append(" ) ");
//        }

//        if (Request.QueryString["SponsorListId"] != null)
//        {
//            int companyIdTemp;
//            if (Int32.TryParse(Request.QueryString["SponsorListId"], out companyIdTemp) && companyIdTemp > 0)
//            {
//                sponsorListIds.Add(companyIdTemp);
//            }
//        }

//        if (sponsorListIds.Count > 0)
//        {
//            // " AND (dbo.JobListing.CompanyId=0 OR dbo.JobListing.CompanyId=2 OR dbo.JobListing.CompanyId=7)"

//            stringBuilder.Append(" AND ( ");
//            for (var i = 0; i < sponsorListIds.Count; i++)
//            {
//                stringBuilder.Append(" SponsorListId=" + sponsorListIds[i]);
//                if (i < sponsorListIds.Count - 1)
//                {
//                    stringBuilder.Append(" OR ");
//                }
//            }
//            stringBuilder.Append(" ) ");
//        }

//        //stringBuilder.Append(String.Format(" AND dbo.JobListing.CompanyId = {0} ", companyId));

//        stringBuilder.Append(" ORDER BY StartRunDate DESC");

//        SqlDataSourceJobs.SelectCommand = stringBuilder.ToString();


    }

    protected string GetHowLongAgo(DateTime jobStartRunDate)
    {
        TimeSpan timeSpan = DateTime.Now.Subtract(jobStartRunDate);

        string retString = string.Format("Listed On {0}.", jobStartRunDate.ToShortDateString());
        //string.Format("Running For {0} minutes.", timeSpan.Minutes);

        return retString;

       


        //return "abcdefghijklmnopqrstuvwxyz";
    }

    protected string GetJobURLonclick(string jobName, string jobCompany)
    {
        const string outboundLinkTemplate = "javascript:pageTracker._trackPageview ('/outbound/JOBADS-{0}-{1}');";
        string outboundLink = String.Format(outboundLinkTemplate, jobCompany, jobName.Substring(0,5).Trim().ToUpper());
        return outboundLink;
    }
}