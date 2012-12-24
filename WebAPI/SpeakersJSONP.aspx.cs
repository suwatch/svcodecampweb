using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;

public partial class SpeakersJSONP : System.Web.UI.Page
{   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["jsonp"]))
            this.JsonPCallback();
    }

    public void JsonPCallback()
    {
        string Callback = Request.QueryString["jsonp"];
        if (!string.IsNullOrEmpty(Callback))
        {
            var attendeesQuery = new AttendeesQuery();
            if (HttpContext.Current.Request["query"] != null)
            {
                attendeesQuery = HttpContext.Current.Request["query"].FromJson<AttendeesQuery>();
            }
            //attendeesQuery.CodeCampYearId = Utils.CurrentCodeCampYear;
           // attendeesQuery.Id = 6061;
            attendeesQuery.PresentersOnly = true;
            attendeesQuery.CodeCampYearIds = new List<int>() {7};

            int bioMaxLen = 4096;
            if (HttpContext.Current.Request["biomaxlen"] != null)
            {
                Int32.TryParse(HttpContext.Current.Request["biomaxlen"] ?? "", out bioMaxLen);
            }


            //if (HttpContext.Current.Request["start"] != null && HttpContext.Current.Request["limit"] != null)
            //{
            //    attendeesQuery.Start = Convert.ToInt32(HttpContext.Current.Request["start"]);
            //    attendeesQuery.Limit = Convert.ToInt32(HttpContext.Current.Request["limit"]);
            //}

            var attendeesManager = new AttendeesManager();
            var listDataSpeakers = attendeesManager.Get(attendeesQuery);

            var sessionsManager = new SessionsManager();
            var sessionsResults = sessionsManager.Get(new SessionsQuery()
            {
                CodeCampYearId = Utils.CurrentCodeCampYear
            });

            var xxx = listDataSpeakers.Where(a => a.Id == 6061).ToList();

            var yyy = sessionsResults.Where(a => a.Attendeesid == 6061).ToList();


            var listDataResults = (from data in listDataSpeakers
                                   orderby data.UserLastName
                                   select new
                                   {
                                       AttendeesId = data.Id,
                                       data.UserFirstName,
                                       data.UserLastName,
                                       data.UserWebsite,
                                       data.PKID,
                                       UserBio = TruncateStringWithEllipse(data.UserBio,4090),
                                       UserBioShort = TruncateStringWithEllipse(data.UserBio, bioMaxLen),
                                       data.SpeakerPictureUrl,
                                       data.TwitterHandle,
                                       Sessions = (from data1 in sessionsResults
                                                  where data1.Attendeesid == data.Id &&
                                                        data1.CodeCampYearId == Utils.CurrentCodeCampYear
                                                  select new
                                                  {
                                                      data1.Id,
                                                      data1.Title,
                                                      data1.Description,
                                                      data.UserFirstName,
                                                      data.UserLastName
                                                  }).ToList()

                                   }).ToList();

            var xx = listDataResults.Where(a => a.AttendeesId == 6061).ToList();

            //foreach (var rec in listDataResults)
            //{
            //    if (bioMaxLen != 0)
            //    {
            //        int oriLen = rec.UserBio.Length;
            //        if (oriLen > bioMaxLen)
            //        {
            //            rec.UserBio = rec.UserBio.Substring(0, bioMaxLen) + "...";
            //        }
            //    }
            //}

            var ret = new { success = true, rows = listDataResults, total = attendeesQuery.OutputTotal };
            Response.Write(Callback + "( " + ret.ToJson() + " );");
        }

        Response.End();
    }

    private string TruncateStringWithEllipse(string userBio, int bioMaxLen)
    {
        string ret = userBio;
        if (bioMaxLen != 0)
        {
            int oriLen = userBio.Length;
            if (oriLen > bioMaxLen)
            {
                ret = userBio.Substring(0, bioMaxLen) + "...";
            }
        }
        return ret;
    }
}