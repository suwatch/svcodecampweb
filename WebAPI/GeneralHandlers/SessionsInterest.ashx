<%@ WebHandler Language="C#" Class="SessionsInterest" %>

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using CodeCampSV;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class SessionsInterest : IHttpHandler, IReadOnlySessionState
{

    public void ProcessRequest(HttpContext context)
    {
        var sessionsQuery = new SessionsQuery();
        if (HttpContext.Current.Request["query"] != null)
        {
            sessionsQuery = HttpContext.Current.Request["query"].FromJson<SessionsQuery>();
        }
        sessionsQuery.CodeCampYearId = Utils.GetCurrentCodeCampYear();    // Utils.CurrentCodeCampYear;
        sessionsQuery.WithSpeakers = true;
        sessionsQuery.WithTags = true;

        bool showSessionInterestCount = ConfigurationManager.AppSettings["ShowSessionInterestCount"] != null &&
                                            ConfigurationManager.AppSettings["ShowSessionInterestCount"].ToLower().Equals("true");

        //bool showPlanToAttendCount = ConfigurationManager.AppSettings["ScheduleAllowCheckAttend"] != null &&
        //                             ConfigurationManager.AppSettings["ScheduleAllowCheckAttend"].ToLower().Equals("true");

      


        if (HttpContext.Current.Request["start"] != null && HttpContext.Current.Request["limit"] != null)
        {
            sessionsQuery.Start = Convert.ToInt32(HttpContext.Current.Request["start"]);
            sessionsQuery.Limit = Convert.ToInt32(HttpContext.Current.Request["limit"]);
        }


        string loggedInUsername = HttpContext.Current.User.Identity.Name; // funky way to get data into the manager class but it works
        var sessionIdsInterestedIn = new List<int>();
        if (!String.IsNullOrEmpty(loggedInUsername))
        {
            var sessionAttendeeOds = new SessionAttendeeODS();
            var recs = sessionAttendeeOds.GetByUsername(loggedInUsername).ToList();
            sessionIdsInterestedIn =
                recs.Where(a => a.Interestlevel == 2).Select(a => a.Sessions_id).ToList();
        }
        var sessionsManager = new SessionsManager();

        // I know I'm not doing proper threading/blocking here
        const string cacheName = "SessionInterestCache";
        var sessionsList = (List<SessionsResult>) HttpContext.Current.Cache[cacheName];
        
        if (HttpContext.Current.Cache[cacheName] == null)
        {
            sessionsList = sessionsManager.Get(sessionsQuery);
            HttpContext.Current.Cache.Insert(cacheName, sessionsList,
                      null, DateTime.Now.Add(new TimeSpan(0, 0, Utils.RetrieveSecondsForSessionCacheTimeout())), TimeSpan.Zero);
        }

        // need to add the "after cache" info.
        foreach (var rec in sessionsList)
        {
            rec.LoggedInUserInterested = sessionIdsInterestedIn.Contains(rec.Id);
            if (!showSessionInterestCount && Utils.CurrentCodeCampYear == Utils.GetCurrentCodeCampYear())
            {
                rec.InterestCount = "-";
                rec.InterestCountInt = 0;
            }
        }

        var ret = new
                      {
                          success = true, 
                          rows = sessionsList.OrderByDescending(a=> Guid.NewGuid()).ToList(),  // order by most recently added
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