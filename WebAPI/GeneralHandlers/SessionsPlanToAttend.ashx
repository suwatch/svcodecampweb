<%@ WebHandler Language="C#" Class="SessionsPlanToAttend" %>

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using CodeCampSV;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class SessionsPlanToAttend : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        var sessionsQuery = new SessionsQuery();
        if (HttpContext.Current.Request["query"] != null)
        {
            sessionsQuery = HttpContext.Current.Request["query"].FromJson<SessionsQuery>();
        }
        sessionsQuery.CodeCampYearId = Utils.CurrentCodeCampYear;
        sessionsQuery.WithSpeakers = true;
        sessionsQuery.WithTags = true;

        bool showPlanToAttendCount = ConfigurationManager.AppSettings["ScheduleAllowCheckAttend"] != null &&
                                     ConfigurationManager.AppSettings["ScheduleAllowCheckAttend"].ToLower().Equals("true");


        if (HttpContext.Current.Request["SessionTimesId"] != null)
        {
            sessionsQuery.SessionTimesId = Convert.ToInt32(HttpContext.Current.Request["SessionTimesId"]);
        }
        

        if (HttpContext.Current.Request[""] != null && HttpContext.Current.Request["limit"] != null)
        {
            sessionsQuery.Start = Convert.ToInt32(HttpContext.Current.Request["start"]);
            sessionsQuery.Limit = Convert.ToInt32(HttpContext.Current.Request["limit"]);
        }


        string loggedInUsername = HttpContext.Current.User.Identity.Name; // funky way to get data into the manager class but it works
        var sessionIdsPlanToAttend = new List<int>();
        if (!String.IsNullOrEmpty(loggedInUsername))
        {
            var sessionAttendeeOds = new SessionAttendeeODS();
            var recs = sessionAttendeeOds.GetByUsername(loggedInUsername).ToList();
            sessionIdsPlanToAttend =
                recs.Where(a => a.Interestlevel == 3).Select(a => a.Sessions_id).ToList();
        }
        var sessionsManager = new SessionsManager();

        // I know I'm not doing proper threading/blocking here
        string cacheName = "SessionPlanToAttendCache";
        if (sessionsQuery.SessionTimesId != null)
        {
            cacheName += "-" + sessionsQuery.SessionTimesId.ToString();
        }
        if (sessionsQuery.CodeCampYearId != null)
        {
            cacheName += "-" + sessionsQuery.CodeCampYearId.ToString();
        }
        
        var sessionsList = (List<SessionsResult>) HttpContext.Current.Cache[cacheName];
        
        if (HttpContext.Current.Cache[cacheName] == null)
        {
            sessionsList = sessionsManager.Get(sessionsQuery);
            HttpContext.Current.Cache.Insert(cacheName, sessionsList,
                      null, DateTime.Now.Add(new TimeSpan(0, 0, Utils.RetrieveSecondsForSessionCacheTimeout())), TimeSpan.Zero);
        }

        // need to add the "after cache" info.
        foreach (SessionsResult t in sessionsList)
        {
            t.LoggedInUserPlanToAttend = sessionIdsPlanToAttend.Contains(t.Id);
            if (!showPlanToAttendCount && Utils.CurrentCodeCampYear == Utils.GetCurrentCodeCampYear())
            {
                t.PlanAheadCount = "-";
                t.PlanAheadCountInt = 0;
            }
        }
        //foreach (var rec in sessionsList)
        //{
        //    rec.LoggedInUserPlanToAttend = sessionIdsPlanToAttend.Contains(rec.Id);
        //}
        
        var ret = new
                      {
                          success = true, 
                          rows = sessionsList.OrderBy(a=> Guid.NewGuid()).ToList(),
                          //rows = sessionsList.OrderBy(a => a.Title).ToList(),
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