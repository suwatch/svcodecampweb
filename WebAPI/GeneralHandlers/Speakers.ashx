<%@ WebHandler Language="C#" Class="Speakers" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CodeCampSV;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Speakers : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        var attendeesQuery = new AttendeesQuery();
        //if (HttpContext.Current.Request["query"] != null)
        //{
        //    attendeesQuery = HttpContext.Current.Request["query"].FromJson<AttendeesQuery>();
        //}
        
        
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
        attendeesQuery.PresentersOnly = true;
        if (HttpContext.Current.Request["start"] != null && HttpContext.Current.Request["limit"] != null)
        {
            attendeesQuery.Start = Convert.ToInt32(HttpContext.Current.Request["start"]);
            attendeesQuery.Limit = Convert.ToInt32(HttpContext.Current.Request["limit"]);
        }

        var attendeesManager = new AttendeesManager();
        var listDataSpeakers = attendeesManager.Get(attendeesQuery);

        
        var sessionsResults = SessionsManager.I.Get(new SessionsQuery()
                                {
                                    CodeCampYearId = attendeesQuery.CodeCampYearId
                                });
        var sessionTimes = SessionTimesManager.I.Get(new SessionTimesQuery()
                                                       {
                                                           CodeCampYearId = attendeesQuery.CodeCampYearId
                                                       });


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
                                              data.SpeakerPictureUrl,
                                              Sessions = (from data1 in sessionsResults
                                                         join data2 in sessionTimes on data1.SessionTimesId equals data2.Id
                                                         where data1.Attendeesid == data.Id &&
                                                               data1.CodeCampYearId == attendeesQuery.CodeCampYearId 
                                                               
                                                         select new
                                                                    {
                                                                        data1.Id,
                                                                        data1.Title,
                                                                        data1.Description,
                                                                        data1.DoNotShowPrimarySpeaker,
                                                                        data1.SponsorId,
                                                                        data2.StartTime,
                                                                        data2.EndTime
                                                                    }).ToList()

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
