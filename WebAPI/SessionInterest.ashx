<%@ WebHandler Language="C#" Class="SessionInterest" %>

using System;
using System.Configuration;
using System.Web;
using CodeCampSV;

public class SessionInterest : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        string buttonName = context.Request["ButtonName"];
        string userName = context.Request["UserName"];
        string sessionIdStr = context.Request["SessionId"];
        string choiceNumberStr = context.Request["ChoiceNumber"];

        int sessionId;
        int choiceNumber;
        Int32.TryParse(sessionIdStr, out sessionId);
        Int32.TryParse(choiceNumberStr, out choiceNumber);

        string userNameFromContext = context.User.Identity.Name;

        // make sure we are not being spoofed!
        if (userNameFromContext.Equals(userName))
        {

            // public enum InterestLevel
            //{
            //    NotInterested = 1,
            //    Interested = 2,
            //    WillAttend = 3
            //}
            int interestLevel = choiceNumber + 1;


            var sessionAttendeeOds = new SessionAttendeeODS();
            // this actually removes data if not interested.
            sessionAttendeeOds.Update(sessionId, userName, interestLevel);

            bool showSessionInterestCount = ConfigurationManager.AppSettings["ShowSessionInterestCount"] != null &&
                                            ConfigurationManager.AppSettings["ShowSessionInterestCount"].ToLower().Equals("true");

            bool showPlanToAttendCount = ConfigurationManager.AppSettings["ScheduleAllowCheckAttend"] != null &&
                                         ConfigurationManager.AppSettings["ScheduleAllowCheckAttend"].ToLower().Equals("true");
            
            if (!showSessionInterestCount)
            {
                interestLevel = 0;
            }


            var retVal = new
                             {
                                 success = true,
                                 msg =
                                     string.Format("ButtonName: {0} userName: {1} sessionId: {2} interestLevel: {3}",
                                                   buttonName, userName, sessionId, interestLevel)
                             };


            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.ContentType = "application/json";

            //System.Threading.Thread.Sleep(2000);

            context.Response.Write(retVal.ToJson());
        }
        else
        {
            throw new ApplicationException(string.Format("SessionInterest.ashx being spoofed with userNameFromContext: {0} userName: {1}", userNameFromContext, userName));
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}