<%@ WebHandler Language="C#" Class="SessionsInterestUpdater" %>


using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using CodeCampSV;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class SessionsInterestUpdater : IHttpHandler
{

    public class ReturnValues
    {
        public int InterestCountUpdated { get; set; }
        public int PlannedCountUpdated { get; set; }
        public int InterestCountFailedByConfig { get; set; }
        public int PlannedCountFailedByConfig { get; set; }
        public int InterestedRemoved { get; set; }
    }
    
    public void ProcessRequest(HttpContext context)
    {
        var returnValues = new ReturnValues();

        bool userIsAdmin = Utils.CheckUserIsAdmin();
        
        bool testMode = true;
        if (HttpContext.Current.Request["testmode"] != null && HttpContext.Current.Request["testmode"].ToLower().Equals("false"))
        {
            testMode = false;
        }
        
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

        string eventBoardOverride = string.Empty;
        if (HttpContext.Current.Request["eventboardoverride"] != null)
        {
            eventBoardOverride = HttpContext.Current.Request["eventboardoverride"];
        }
        
        var codeCampYearId = Utils.CurrentCodeCampYear;
        if (HttpContext.Current.Request["codecampyearid"] != null)
        {
            Int32.TryParse(HttpContext.Current.Request["codecampyearid"], out codeCampYearId);
        }

        string allowCheckInterest = ConfigurationManager.AppSettings["ShowSessionInterest"] ?? "false";
        string allowCheckAttend = ConfigurationManager.AppSettings["ScheduleAllowCheckAttend"] ?? "false";
        string eventBoardOverideUpdate = ConfigurationManager.AppSettings["EventBoardOverideUpdate"] ?? "false";

        // if passed in eventboardoverride=true AND AppSettings has EventBoardOverideUpdate set to true then allow update for planned attend
        if (userIsAdmin || (eventBoardOverideUpdate.Equals("true") && eventBoardOverride.Equals("true")))
        {
            allowCheckInterest = "true";
            allowCheckAttend = "true";
        }
        
        
        
        //string jsonData =
        //    System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~") + @"\App_Data\EventBoardJson.txt");

        var byteArray = new byte[HttpContext.Current.Request.ContentLength];
        HttpContext.Current.Request.InputStream.Read(byteArray, 0, HttpContext.Current.Request.ContentLength);
        string jsonData = System.Text.Encoding.UTF8.GetString(byteArray);
        
        var ebSessionInterestRecordsSent =
                jsonData.FromJson<List<Utils.EventBoardSessionInterest>>();
        
        var ebSessionInterestRecordsCurrent = 
            Utils.GetEventBoardSessionInterest("CodeCampSV06", emailtoFind, codeCampYearId, eventBoardId);


        foreach (var rec in ebSessionInterestRecordsSent)
        {
            // first, check and see if the record EB sent us is one we know about
            string recEventBoardEmailSent = rec.EventBoardEmail;
            int recSessionIdSent = rec.SessionId;
            var recSessionInterestDatabase =
                ebSessionInterestRecordsCurrent.Where(
                    a => a.EventBoardEmail.Equals(recEventBoardEmailSent) && a.SessionId == recSessionIdSent).FirstOrDefault();

            // if we found it, and EB set it to 1, we need to kill it from our database
            if (recSessionInterestDatabase != null && rec.InterestLevelValue == 1)
            {
                // remove it from out database
                var sessionAttendeeResult = SessionAttendeeManager.I.Get(new SessionAttendeeQuery()
                                                                             {
                                                                                 Sessions_id = recSessionIdSent,
                                                                                 EventBoardEmail =
                                                                                     recEventBoardEmailSent
                                                                             }).FirstOrDefault();
                if (sessionAttendeeResult != null)
                {
                    if (!testMode && (allowCheckInterest.Equals("true") || allowCheckAttend.Equals("true")))
                    {
                        SessionAttendeeManager.I.Delete(sessionAttendeeResult.Id);
                        returnValues.InterestedRemoved++;
                    }
                }
            }
            else
            {
                // now, check and see if interestvalue changed (and sent value is 2 or 3 (not 1))
                if (recSessionInterestDatabase == null)
                {
                    // need to add because it was not there before
                    var attendeesResult =
                        AttendeesManager.I.Get(new AttendeesQuery {EmailEventBoard = rec.EventBoardEmail}).
                            FirstOrDefault();
                    if (attendeesResult != null) // sanity check, should always have an attendeesResult here
                    {
                        int recInterestLevelValue = rec.InterestLevelValue;
                        int recSessionId = rec.SessionId;
                        Guid attendeesResultPKID = attendeesResult.PKID;
                        
                        if (!testMode)
                        {
                            if ((allowCheckAttend.Equals("true") && recInterestLevelValue == 3) ||
                                (allowCheckInterest.Equals("true") && recInterestLevelValue == 2))
                            {
                                if (rec.InterestLevelValue == 3)
                                {
                                    ClearOtherPlannedToAttendAtSameTime(attendeesResult.Username, recSessionId);
                                    returnValues.PlannedCountUpdated++;
                                }
                                if (rec.InterestLevelValue == 2)
                                {
                                    returnValues.InterestCountUpdated++;
                                }

                                SessionAttendeeManager.I.Insert(new SessionAttendeeResult
                                                                    {
                                                                        Attendees_username = attendeesResultPKID,
                                                                        Sessions_id = recSessionId,
                                                                        Interestlevel = recInterestLevelValue,
                                                                        LastUpdatedDate = DateTime.UtcNow,
                                                                        UpdateByProgram = "EventBoardInsert"
                                                                    });
                            }
                            else
                            {
                                if (recInterestLevelValue == 3)
                                {
                                    returnValues.PlannedCountFailedByConfig++;
                                }
                                if (recInterestLevelValue == 2)
                                {
                                    returnValues.InterestCountFailedByConfig++;
                                }
                            }
                        }
                        else
                        {
                            throw new ApplicationException(
                                "SessionsInterestUpdater: attendeesResult == null, not possible!");
                        }
                    }
                }
                else
                {
                    // we need to update the existing record if it changed
                    if (recSessionInterestDatabase.InterestLevelValue != rec.InterestLevelValue && 
                        (rec.InterestLevelValue ==2 || rec.InterestLevelValue == 3) &&
                        ( 
                          (rec.InterestLevelValue == 2 && allowCheckInterest.Equals("true")) || 
                          (rec.InterestLevelValue == 3 && allowCheckAttend.Equals("true"))) 
                        )
                    {
                        var attendeesResult =
                            AttendeesManager.I.Get(new AttendeesQuery() {EmailEventBoard = rec.EventBoardEmail}).
                                FirstOrDefault();
                        if (attendeesResult != null)
                        {
                            // update database with new interest
                            var sessionAttendeeResult = SessionAttendeeManager.I.Get(new SessionAttendeeQuery()
                                                                                         {
                                                                                             Sessions_id = recSessionIdSent,
                                                                                             EventBoardEmail =
                                                                                                 recEventBoardEmailSent
                                                                                         }).FirstOrDefault();
                            if (sessionAttendeeResult != null)
                            {
                                if (!testMode)
                                {
                                    if (rec.InterestLevelValue == 3)
                                    {
                                        ClearOtherPlannedToAttendAtSameTime(attendeesResult.Username, rec.SessionId);
                                        returnValues.PlannedCountUpdated++;
                                    }
                                    if (rec.InterestLevelValue == 2)
                                    {
                                        returnValues.InterestCountUpdated++;
                                    }
                                    sessionAttendeeResult.Interestlevel = rec.InterestLevelValue;
                                    SessionAttendeeManager.I.Update(sessionAttendeeResult);
                                }
                            }
                        }
                    }
                }
            }
        }

        context.Response.ContentType = "application/json";
        HttpContext.Current.Response.Write(returnValues.ToJson());
    }

    private void ClearOtherPlannedToAttendAtSameTime(string username, int sessionId)
    {
        using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
        {
            sqlConnection.Open();

            const string sqlUpdateAllToInterested = @"UPDATE SessionAttendee
                    SET interestlevel = 2,LastUpdatedDate = GETUTCDATE(),UpdateByProgram='SessionAttendeeODS'
                    WHERE interestlevel = 3 AND  attendees_username =
                          (select PKID from attendees WHERE Username = @Username) AND
                          (sessions_id IN (
                             select id from sessions WHERE sessionTimesId =(select sessiontimesid FROM sessions WHERE id = @sessionid)
                          ))";


            using (var cmdUpdateInterest = new SqlCommand(sqlUpdateAllToInterested, sqlConnection))
            {
                cmdUpdateInterest.Parameters.Add("@sessionid", SqlDbType.Int).Value = sessionId;
                cmdUpdateInterest.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
                cmdUpdateInterest.ExecuteNonQuery();
            }
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}