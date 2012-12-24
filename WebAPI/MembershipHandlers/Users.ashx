<%@ WebHandler Language="C#" Class="Users" %>

using System;
using System.Collections.Generic;
using System.Web;

using CodeCampSV;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Users : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
      
        
        int start = 0;
        int limit = Int32.MaxValue;
        string emailContains = string.Empty;
        
        if (HttpContext.Current.Request["start"] != null)
        {
            start = Convert.ToInt32(HttpContext.Current.Request["start"]);
        }

        if (HttpContext.Current.Request["limit"] != null)
        {
            limit = Convert.ToInt32(HttpContext.Current.Request["limit"]);
        }

        if (HttpContext.Current.Request["EmailContains"] != null)
        {
            emailContains = HttpContext.Current.Request["EmailContains"];
        }

        var attendeesQuery = new AttendeesQuery
                            {
                                Start = start,
                                Limit = limit
                            };
        
        if (!String.IsNullOrEmpty(emailContains))
        {
            attendeesQuery.EmailContains = emailContains;
        }
        
        context.Response.ContentType = "text/plain";
        var attendeesManager = new AttendeesManager();

        List<AttendeesResult> listAttendees;
        if (HttpContext.Current.User.Identity.IsAuthenticated
          && Utils.CheckUserIsAdmin())
        {
            listAttendees = attendeesManager.Get(attendeesQuery);
        }
        else
        {
            listAttendees = new List<AttendeesResult>()
                                {
                                    new AttendeesResult()
                                        {
                                            UserFirstName = "a",
                                            UserLastName = "b",
                                            Email = "c@d.com",
                                            UserZipCode = "11111"
                                        }
                                };
        }
       
        var o = new
                       {
                           total = attendeesQuery.OutputTotal,
                           success = true,
                           rows = listAttendees
                       };
        string retString = o.ToJson();
        HttpContext.Current.Response.Write(retString);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}