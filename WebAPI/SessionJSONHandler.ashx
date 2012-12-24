<%@ WebHandler Language="C#" Class="SessionJSONHandler" %>

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.SessionState;
using CodeCampSV;
using Newtonsoft.Json;

public class SessionJSONHandler : IHttpHandler, IRequiresSessionState
{
    
    #region IHttpHandler Members

    bool IHttpHandler.IsReusable
    {
        get { throw new NotImplementedException(); }
    }

    void IHttpHandler.ProcessRequest(HttpContext context)
    {
        //context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //context.Response.ContentType = "text/plain";
        //if (HttpContext.Current.Request.QueryString["SessionData"] != null)
        //{
        //    SessionsODS sessionODS = new SessionsODS();
        //    List<SessionsODS.DataObjectSessions> sessionsAll = sessionODS.GetAllByStartTime();
        //    string json = JavaScriptConvert.SerializeObject(sessionsAll);
        //    context.Response.Write(json);
        //}
        //else if (HttpContext.Current.Request.QueryString["SpeakerData"] != null)
        //{
        //    AttendeesODS attendeeODS = new AttendeesODS();
        //    List<AttendeesODS.DataObjectAttendees> speakersAll =
        //        attendeeODS.GetAllAttendeesBetweenDatesJustPresenter(DateTime.MinValue, DateTime.MaxValue);
            
        //    List<AttendeeShort> speakerList = new List<AttendeeShort>();
        //    foreach (var attendees in speakersAll)
        //    {
        //        AttendeeShort attendeeShort = new AttendeeShort(attendees.Username,
        //            attendees.Userfirstname,
        //            attendees.Userlastname,
        //            attendees.Userzipcode,
        //            attendees.Pkid.ToString(),
        //            attendees.Userbio,
        //            attendees.Isapproved,
        //            attendees.Creationdate,
        //            attendees.Userwebsite
        //           );
        //        speakerList.Add(attendeeShort);
        //    }

        //    string json = JavaScriptConvert.SerializeObject(speakerList);
        //    context.Response.Write(json);
        //}
        //else if (HttpContext.Current.Request.QueryString["TagData"] != null)
        //{
        //    TagsODS tagODS = new TagsODS();
        //    List<TagsODS.DataObjectTags> tagsAll =
        //        tagODS.GetAllTags(string.Empty, -1);

        //    string json = JavaScriptConvert.SerializeObject(tagsAll);
        //    context.Response.Write(json);
        //}
        //else if (HttpContext.Current.Request.QueryString["LectureRoomData"] != null)
        //{
        //    LectureRoomsODS lectureRoomsODS = new LectureRoomsODS();
        //    List<LectureRoomsODS.DataObjectLectureRooms> listRooms =
        //        lectureRoomsODS.GetAllLectureRooms();

        //    string json = JavaScriptConvert.SerializeObject(listRooms);
        //    context.Response.Write(json);
        //}
        //else if (HttpContext.Current.Request.QueryString["SessionTimeData"] != null)
        //{
        //   SessionTimesODS stODS = new SessionTimesODS();
        //   List<SessionTimesODS.DataObjectSessionTimes> listTimes =
        //        stODS.GetAllSessionTimes();

        //   string json = JavaScriptConvert.SerializeObject(listTimes);
        //   context.Response.Write(json);
        //}

        //else
        //{
        //    WS_Detail wsDetail = null;
        //    if (HttpContext.Current.Request.QueryString["SessionId"] != null)
        //    {
        //        int sessionId = 0;
        //        Int32.TryParse(HttpContext.Current.Request.QueryString["SessionId"], out sessionId);
        //        wsDetail = Utils.RetrieveSessionsOneCodeCamp(sessionId);
        //    }
        //    else
        //    {
        //        wsDetail = Utils.RetrieveSessionsAllCodeCamp();
        //    }
        //    string json = JavaScriptConvert.SerializeObject(wsDetail);
        //    context.Response.Write(json);
        //}


    }

    #endregion
}




 
    