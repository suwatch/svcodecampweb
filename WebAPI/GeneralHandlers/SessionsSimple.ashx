<%@ WebHandler Language="C#" Class="SessionsSimple" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CodeCampSV;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class SessionsSimple : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        int codeCampYearId = Utils.CurrentCodeCampYear;
        int start = 0;
        int limit = 99999;

        if (HttpContext.Current.Request["codecampyear"] != null)
        {
            Int32.TryParse(HttpContext.Current.Request["codecampyear"], out codeCampYearId);
        }
        
        if (HttpContext.Current.Request["start"] != null && HttpContext.Current.Request["limit"] != null)
        {
            start = Convert.ToInt32(HttpContext.Current.Request["start"]);
            limit = Convert.ToInt32(HttpContext.Current.Request["limit"]);
        }



        var sessionsResults = SessionsManager.I.Get(new SessionsQuery {CodeCampYearId = codeCampYearId,WithSpeakers = true});
        var sessionTimes = SessionTimesManager.I.Get(new SessionTimesQuery
                                                       {
                                                           CodeCampYearId = codeCampYearId
                                                       });

        var listData = (from sessionResult in sessionsResults
                        join sessionTime in sessionTimes on sessionResult.SessionTimesId equals sessionTime.Id
                        into sessionTimeGroup
                        from sessionTime in sessionTimeGroup.DefaultIfEmpty()
                        where sessionResult.CodeCampYearId == codeCampYearId 
                        select new Session
                                   {
                                       Id = sessionResult.Id,
                                       Title = sessionResult.Title,
                                       Description = sessionResult.Description,
                                       DoNotShowPrimarySpeaker = sessionResult.DoNotShowPrimarySpeaker,
                                       SponsorId = sessionResult.SponsorId ?? 0,
                                       SpeakerIds = sessionResult.SpeakersList.Select(a=>a.Id).ToList(),
                                       StartTime = sessionTime != null ? DateToString(sessionTime.StartTime) : "",
                                       EndTime =  sessionTime != null ? DateToString(sessionTime.EndTime) : "",
                                       RoomNumber = sessionResult.RoomNumber
                                   }).ToList();
        
        var ret = new RootObject {success = true, rows = listData, total = listData.Count};
        context.Response.ContentType = "application/json";
        HttpContext.Current.Response.Write(ret.ToJson());
    }

    private string DateToString(DateTime? startTime)
    {
        if (startTime.HasValue)
        {
            return startTime.Value.ToString("yy-MM-dd-HH-mm");
            //return startTime.Value.Year + "-" +
            //    startTime.Value.Month + "-" +
            //    startTime.Value.Day + "-" +
            //    startTime.Value.Hour + "-" +
            //    startTime.Value.Minute;
        }
        else
        {
            return "";
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    public class Session
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool DoNotShowPrimarySpeaker { get; set; }
        public int SponsorId { get; set; }
        public List<int> SpeakerIds { get; set; } 
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string RoomNumber { get; set; }
    }

    public class RootObject
    {
        public bool success { get; set; }
        public List<Session> rows { get; set; }
        public int total { get; set; }
    }

}
