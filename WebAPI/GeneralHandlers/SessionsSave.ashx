<%@ WebHandler Language="C#" Class="SessionSave" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using CodeCampSV;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class SessionSave : IHttpHandler
{
    //[AcceptVerbs(HttpVerbs.Post)]
    //public ActionResult Update(string data)
    //{
    //    XmlConfigurator.Configure();
    //    log.Debug(data);

    //    //data = data.FlatToHierachicalJson();

    //    var result = data.FromJson<List<LoadResult>>();
    //    //if (data.StartsWith("{"))
    //    //{
    //    //    data = string.Format("[{0}]", data);
    //    //}
    //    //var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LoadResult>>(data);
    //    LoadManager.I.Update(result);
    //    return Json(new { success = true, rows = result });
    //}
    
   

    public void ProcessRequest(HttpContext context)
    {
        bool successResult = true;
        string errorMessage = string.Empty;
        int numRecsUpdate = 0;
        
        string datax = context.Request.Form[1];
        string datay = string.Empty;
        
        if (datax.Length > 0 && !datax.StartsWith("["))
        {
            datay = "[" + datax + "]";
        }
        else
        {
            datay = datax;
        }
        
        var sessionsResults = datay.FromJson<List<SessionsResult>>();
        
        // verify that all the rooms exist
        List<LectureRoomsResult> lectureRoomsResults = LectureRoomsManager.I.GetAll();
        var sb = new StringBuilder();                                                                
        foreach (var sessionsResult in sessionsResults)
        {
            var sr = sessionsResult;
            int numRecs = (from data in lectureRoomsResults
                        where data.Number.Equals(sr.RoomNumberNew)
                        select data).ToList().Count;
            if (numRecs == 0)
            {
                sb.Append(sessionsResult.RoomNumberNew);
                sb.Append(",");
                successResult = false;
            }
        }
        if (sb.Length > 0)
        {
            errorMessage = "Invalid Room(s): " + sb.ToString();
        }
        if (successResult)
        {
            // verify that the new list is unique
            int numUniqueNewRooms = (from data in sessionsResults select data.RoomNumberNew).ToList().Distinct().Count();
            if (sessionsResults.Count != numUniqueNewRooms)
            {
                successResult = false;
                errorMessage = "Rooms Must be unique.  Duplicates entered.";
            }
        }
        
        // update the new rooms
        if (successResult)
        {
            Dictionary<string, int> roomDict = (from data in lectureRoomsResults select data).ToDictionary(
                a => a.Number, a => a.Id);

            foreach (var sessionsResult in sessionsResults)
            {
                if (!sessionsResult.RoomNumber.Equals(sessionsResult.RoomNumberNew))
                {
                    int roomNumberIdNew = roomDict[sessionsResult.RoomNumberNew];

                    var rec = (SessionsManager.I.Get(new SessionsQuery() {Id = sessionsResult.Id})).FirstOrDefault();
                    rec.LectureRoomsId = roomNumberIdNew;

                    try
                    {
                        // make sure no other rooms have this number set first

                        var recsWithRoom =
                            SessionsManager.I.Get(new SessionsQuery()
                                                      {
                                                          LectureRoomsId = rec.LectureRoomsId,
                                                          SessionTimesId = rec.SessionTimesId,
                                                          CodeCampYearId = Utils.CurrentCodeCampYear
                                                      });
                        if (recsWithRoom.Count > 0)
                        {
                            int roomIdUnassigned = roomDict["0"];
                            foreach (SessionsResult sessionsResult1 in recsWithRoom)
                            {
                                sessionsResult1.LectureRoomsId = roomIdUnassigned;
                                SessionsManager.I.Update(sessionsResult1);
                                numRecsUpdate++;
                            }
                        }
                        
                        // finally update real record
                        SessionsManager.I.Update(rec);
                        numRecsUpdate++;

                        errorMessage = string.Format("Successfully Update {0} records", numRecsUpdate);
                        
                        
                        
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                        successResult = false;
                    }
                }
                
            }
        }

        string ret = (new { success = successResult, message = errorMessage }).ToJson();
        context.Response.ContentType = "text/plain";
        HttpContext.Current.Response.Write(ret);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
