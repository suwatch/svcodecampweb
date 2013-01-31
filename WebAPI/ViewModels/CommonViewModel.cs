using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeCampSV;
using WebAPI.Code;

namespace WebAPI.ViewModels
{
    /// <summary>
    /// Need to refactor this and add caching at this layer where relevant
    /// </summary>
    public class CommonViewModel
    {
        public List<SessionsResult> Sessions { get; set; }
        public List<SponsorListResult> Sponsors { get; set; }
        public List<SessionTimesResult> SessionsByTime { get; set; }
        public List<SpeakerResult> Speakers { get; set; }
        public List<SessionTimesResult> SessionTimeResults { get; set; }
        public List<TagsResult> TagsResults { get; set; }
        public List<SponsorListJobListingResult> JobListings { get; set; }
        public List<RSSItem> FeedItems { get; set; }

        public string DaysUntilCodeCampString
        {
            get { return DaysToGoString(); }
        }

        public string CodeCampDateString
        {
            get { return CodeCampDateStringPretty(); }
        }








        private string CodeCampDateStringPretty()
        {
            var rec = CodeCampYearManager.I.Get(new CodeCampYearQuery()
            {
                Id = Utils.GetCurrentCodeCampYear()
            });
            var retStr = "";
            if (rec != null && rec.Count >= 1)
            {
                retStr = rec[0].CodeCampDateString.Replace("and","&").ToUpper();
            }
            return retStr;
        }


        private static string DaysToGoString()
        {
            DateTime codeCampDateTime = Utils.GetCurrentCodeCampYearStartDate();
            var daysToGo = codeCampDateTime.Subtract(DateTime.Now).Days;

            Random random = new Random();
            daysToGo = random.Next(-1, 3);

            var retStr = "";
            if (daysToGo >= 1)
            {
                retStr = String.Format("{0}", daysToGo);
            }
            else
            {
                // please hide this panel so that the jobs panel is on top if this condition is met
                retStr = "HIDE-ME-WITH-CSS";
            }
            return retStr;
        }


    }

}