<%@ WebHandler Language="C#" Class="Tracks" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CodeCampSV;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Tracks : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        int codeCampYearId = Utils.GetCurrentCodeCampYear();
        
        if (HttpContext.Current.Request["codecampyear"] != null)
        {
            codeCampYearId = Convert.ToInt32(HttpContext.Current.Request["codecampyear"]);
        }

        var trackManager = new TrackManager();
        var listTracks = trackManager.Get(new TrackQuery {CodeCampYearId = codeCampYearId});
        

        var sessionsManager = new SessionsManager();
        var sessionsResults = sessionsManager.Get(new SessionsQuery
                                {
                                    CodeCampYearId = codeCampYearId
                                });

        var trackSessionManager = new TrackSessionManager();
        var trackSessionResults = trackSessionManager.GetAll();


        var listDataResults = (from data in listTracks
                               orderby data.Named
                               select new
                                          {
                                              TrackName = data.Named,
                                              TrackDescription = data.Description,
                                              TrackId = data.Id,
                                              Sessions = from data1 in sessionsResults
                                                         join data2 in trackSessionResults on data1.Id equals
                                                             data2.SessionId
                                                         where data1.CodeCampYearId == codeCampYearId &&
                                                               data2.TrackId == data.Id
                                                         select new
                                                                    {
                                                                        SessionId = data1.Id,
                                                                        data2.TrackId,
                                                                        data1.Title,
                                                                        data1.Description,
                                                                        data1.SpeakerPictureUrl,
                                                                        data1.PresenterName,
                                                                        data1.PresenterURL,
                                                                        data1.Attendeesid
                                                                    }

                                          }).ToList();


        var ret = new { success = true, rows = listDataResults, total = listDataResults.Count };
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
