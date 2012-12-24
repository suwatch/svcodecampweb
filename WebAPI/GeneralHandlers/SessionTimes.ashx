<%@ WebHandler Language="C#" Class="SessionTimes" %>

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

using CodeCampSV;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class SessionTimes : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        int codeCampYearId = Utils.GetCurrentCodeCampYear();
        if (HttpContext.Current.Request["CodeCampYear"] == null)
        {
            int codeCampYearIdTemp;
            Int32.TryParse(HttpContext.Current.Request["CodeCampYear"], out codeCampYearIdTemp);
            if (codeCampYearIdTemp != 0)
            {
                codeCampYearId = codeCampYearIdTemp;
            }
        }


        var recs = SessionTimesManager.I.Get(new SessionTimesQuery(){CodeCampYearId = codeCampYearId});

        //bool showSessionInterestCount = ConfigurationManager.AppSettings["ShowSessionInterestCount"] != null &&
        //                                    ConfigurationManager.AppSettings["ShowSessionInterestCount"].ToLower().Equals("true");

        //bool showPlanToAttendCount = ConfigurationManager.AppSettings["ScheduleAllowCheckAttend"] != null &&
        //                             ConfigurationManager.AppSettings["ScheduleAllowCheckAttend"].ToLower().Equals("true");

        //if (!showSessionInterestCount)
        //{
        //    foreach (var rec in recs)
        //    {
        //        rec.
        //    }
        //}

       
        var ret = new
                      {
                          success = true,
                          rows = recs, 
                          total = recs.Count
                      };        
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

/*
public ActionResult Get(FormCollection form)
        {
            var newsQuery = new NewsQuery();
            if (form["query"] != null)
            {
                newsQuery = form["query"].FromJson<NewsQuery>();
            }
            else
            {
                // If Query is not specified, this is what gets called.
                newsQuery = new NewsQuery
                                {
                                    EntityTypeId = 1,
                                    EntityId = 229493,
                                    Start = 0,
                                    Limit = 999999
                                };
            }


            if (form["start"] != null && form["limit"] != null)
            {
                newsQuery.Start = Convert.ToInt32(form["start"]);
                newsQuery.Limit = Convert.ToInt32(form["limit"]);
            }

            var newsManager = new NewsManager();
            List<NewsResult> listNews = newsManager.Get(newsQuery);
            return Json(new { success = true, rows = listNews, total = newsQuery.OutputTotal });
        }
*/