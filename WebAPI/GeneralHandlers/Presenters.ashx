<%@ WebHandler Language="C#" Class="Presenters" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CodeCampSV;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Presenters : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        var attendeesQuery = new AttendeesQuery();
        if (HttpContext.Current.Request["codecampyear"] != null)
        {
            attendeesQuery.CodeCampYearId = Utils.CurrentCodeCampYear;
            int tempYear;
            if (Int32.TryParse(HttpContext.Current.Request["codecampyear"],
                                                           out tempYear))
            {
                attendeesQuery.CodeCampYearId = tempYear;
            }
        }
       
        if (HttpContext.Current.Request["start"] != null && HttpContext.Current.Request["limit"] != null)
        {
            attendeesQuery.Start = Convert.ToInt32(HttpContext.Current.Request["start"]);
            attendeesQuery.Limit = Convert.ToInt32(HttpContext.Current.Request["limit"]);
        }


        attendeesQuery.PresentersOnly = true;
        attendeesQuery.IncludeSessions = true;

        var baseUrl = Utils.baseURL;
        var urlFormatTemplate = baseUrl + "attendeeimage/{0}.jpg";
        
        var attendeesManager = new AttendeesManager();
        var listDataSpeakers = attendeesManager.Get(attendeesQuery);
        var listDataResults = (from data in listDataSpeakers
                               orderby data.UserLastName
                               select new
                                          {
                                              AttendeesId = data.Id,
                                              data.UserFirstName,
                                              data.UserLastName,
                                              data.UserWebsite,
                                              data.PKID,
                                              data.UserBio,
                                              SpeakerPictureUrl = String.Format(urlFormatTemplate,data.Id),
                                              data.SessionIds
                                          }).ToList();
                                 

        var ret = new { success = true, rows = listDataResults, total = attendeesQuery.OutputTotal };
        context.Response.ContentType = "application/json";
        HttpContext.Current.Response.Write(ret.ToJson());
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
