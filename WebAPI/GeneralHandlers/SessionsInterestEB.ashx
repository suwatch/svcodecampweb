<%@ WebHandler Language="C#" Class="SessionsInterestEB" %>

// GeneralHandlers/SessionsInterestEB.ashx?eventboardid=yuu672@& codecampyearid=6&email=peter@peterkellner.net

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeCampSV;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class SessionsInterestEB : IHttpHandler {

    
    public void ProcessRequest(HttpContext context)
    {
        string emailtoFind = string.Empty;
        if (HttpContext.Current.Request["email"] != null)
        {
            emailtoFind = HttpContext.Current.Request["email"];
        }
        
        string eventBoardId = string.Empty;
        if (HttpContext.Current.Request["eventboardid"] != null)
        {
            eventBoardId = HttpContext.Current.Request["eventboardid"];
        }
        
        var codeCampYearId = Utils.CurrentCodeCampYear;
        if (HttpContext.Current.Request["codecampyearid"] != null)
        {
            Int32.TryParse(HttpContext.Current.Request["codecampyearid"], out codeCampYearId);
        }

        List<Utils.EventBoardSessionInterest> recs = 
            Utils.GetEventBoardSessionInterest("CodeCampSV06", emailtoFind,codeCampYearId,eventBoardId);

        context.Response.ContentType = "application/json";
        HttpContext.Current.Response.Write(recs.ToJson());
        
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}