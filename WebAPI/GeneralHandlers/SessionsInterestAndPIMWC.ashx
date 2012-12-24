<%@ WebHandler Language="C#" Class="SessionsInterestAndPIMWC" %>

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using CodeCampSV;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class SessionsInterestAndPIMWC : IHttpHandler, IReadOnlySessionState
{

    public void ProcessRequest(HttpContext context)
    {
        int codeCampYearId = Utils.GetCurrentCodeCampYear();
        if (HttpContext.Current.Request["codecampyear"] != null)
        {
            codeCampYearId = Convert.ToInt32(HttpContext.Current.Request["codecampyear"]);
        }
        
        var sessionsQuery = new SessionsQuery();
        sessionsQuery.CodeCampYearId = codeCampYearId;
        sessionsQuery.WithSpeakers = true;
        sessionsQuery.WithTags = true;

        string loggedInUsername = HttpContext.Current.User.Identity.Name; // funky way to get data into the manager class but it works
        List<int> sessionIdsInterestedIn;
        if (!String.IsNullOrEmpty(loggedInUsername) && loggedInUsername != "guestmwc")
        {
            var sessionAttendeeOds = new SessionAttendeeODS();
            var recs = sessionAttendeeOds.GetByUsername(loggedInUsername).ToList();
            sessionIdsInterestedIn =
                recs.Where(a => a.Interestlevel == 2 || a.Interestlevel == 3).Select(a => a.Sessions_id).ToList();
        }
        else
        {
            // get all sessions for year
            sessionIdsInterestedIn =
                SessionsManager.I.Get(new SessionsQuery {CodeCampYearId = codeCampYearId}).Select(a => a.Id).ToList();
        }

        var sessionTimesDictionary =
            SessionTimesManager.I.Get(new SessionTimesQuery {CodeCampYearId = codeCampYearId}).ToDictionary(k => k.Id,
                                                                                                            v =>
                                                                                                            v.StartTime);


        var sessions =
            SessionsManager.I.Get(new SessionsQuery() {Ids = sessionIdsInterestedIn}).OrderBy(a=>a.SessionTime).ToList();

        var sessionKeys = new List<string>();
        foreach (var session in sessions)
        {
            if (session.SessionTimesId.HasValue && sessionTimesDictionary.ContainsKey(session.SessionTimesId.Value))
            {
                DateTime? sessionStart = sessionTimesDictionary[session.SessionTimesId.Value];
                if (sessionStart.HasValue)
                {
                    // not 24 hour format
                    string key = sessionStart.Value.ToString("yyMMddhhmm") + session.Title;
                    sessionKeys.Add(key);
                }
                else
                {
                    
                }
            }
            else
            {
                
            }
        }
       
        var ret = new
                      {
                          success = true, 
                          rows = sessionKeys,
                          loggedInUsername,
                          total = sessionsQuery.OutputTotal
                      };
        context.Response.ContentType = "text/plain";
        HttpContext.Current.Response.Write(ret.ToJson());

    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}