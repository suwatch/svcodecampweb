f<%@ WebHandler Language="C#" Class="Sessions" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CodeCampSV;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Sessions : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        var sessionsQuery = new SessionsQuery();
        
        if (HttpContext.Current.Request["query"] != null)
        {
            sessionsQuery = HttpContext.Current.Request["query"].FromJson<SessionsQuery>();
        }
        sessionsQuery.CodeCampYearId = Utils.CurrentCodeCampYear;

        if (HttpContext.Current.Request["CodeCampYearId"] != null)
        {
            int codeCampYearId;
            Int32.TryParse(HttpContext.Current.Request["CodeCampYearId"], out codeCampYearId);
            sessionsQuery.CodeCampYearId = codeCampYearId;
        }
        
        if (HttpContext.Current.Request["start"] != null && HttpContext.Current.Request["limit"] != null)
        {
            sessionsQuery.Start = Convert.ToInt32(HttpContext.Current.Request["start"]);
            sessionsQuery.Limit = Convert.ToInt32(HttpContext.Current.Request["limit"]);
        }

        var listData1 = new List<SessionsResult>();
        var sessionsManager = new SessionsManager();
        //if (HttpContext.Current.User.Identity.IsAuthenticated
        // && Utils.CheckUserIsAdmin())
        //{
            listData1 = sessionsManager.Get(sessionsQuery);
        //}

        var listData2 = (from data in listData1 orderby data.SessionTime,data.Title.ToUpper() select data).ToList();
        
        var ret = new {success = true, rows = listData2, total = sessionsQuery.OutputTotal};
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
