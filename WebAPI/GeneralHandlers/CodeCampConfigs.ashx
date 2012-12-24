<%@ WebHandler Language="C#" Class="SessionsInterestEB" %>

// GeneralHandlers/SessionsInterestEB.ashx?eventboardid=yuu672@& codecampyearid=6&email=peter@peterkellner.net

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using CodeCampSV;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class SessionsInterestEB : IHttpHandler {

    

    public class CodeCampConfigValues
    {
        public string AllowCheckInterest { get; set; }
        public string AllowCheckAttend { get; set; }
        public string ShowSessionInterestCounts { get; set; }
        public string ShowRoomOnSchedule { get; set; }
        public string ShowTrackPage { get; set; }
        public string ShowTrackOnSession { get; set; }
        public string ShowAgendaOnSchedule { get; set; }
    }

    public void ProcessRequest(HttpContext context)
    {
        // Web/GeneralHandlers/CodeCampConfigs.ashx?eventboardid=yuu672@
        
        string eventBoardId = string.Empty;
        if (HttpContext.Current.Request["eventboardid"] != null)
        {
            eventBoardId = HttpContext.Current.Request["eventboardid"];
        }

        var codeCampConfigValues = new CodeCampConfigValues();

        string eventBoardIdReal = ConfigurationManager.AppSettings["EventBoardId"] ?? string.Empty;
        if ((HttpContext.Current.Request.IsSecureConnection ||
             HttpContext.Current.Request.Url.Host.ToLower().Equals("localhost")
             || HttpContext.Current.Request.Url.Host.ToLower().Contains("dotnet4"))
            && eventBoardIdReal.Equals(eventBoardId))
        {

            codeCampConfigValues =
                new CodeCampConfigValues()
                    {

                        AllowCheckInterest =
                            System.Configuration.ConfigurationManager.AppSettings[
                                "ShowSessionInterest"] ?? "false",
                        AllowCheckAttend =
                            System.Configuration.ConfigurationManager.AppSettings[
                                "ScheduleAllowCheckAttend"] ?? "false",
                        ShowSessionInterestCounts =
                            System.Configuration.ConfigurationManager.AppSettings[
                                "ShowSessionInterestCount"] ?? "false",
                        ShowRoomOnSchedule =
                            System.Configuration.ConfigurationManager.AppSettings[
                                "ShowRoomOnSchedule"] ??
                            "false",
                        ShowTrackPage =
                            System.Configuration.ConfigurationManager.AppSettings[
                                "ShowAgendaOnSchedule"] ??
                            "false",
                        ShowTrackOnSession =
                            System.Configuration.ConfigurationManager.AppSettings[
                                "ShowAgendaOnSchedule"] ??
                            "false",
                        ShowAgendaOnSchedule =
                            System.Configuration.ConfigurationManager.AppSettings[
                                "ShowAgendaOnSchedule"] ?? "false"
                    };



            context.Response.ContentType = "application/json";

        }
        HttpContext.Current.Response.Write(codeCampConfigValues.ToJson());

    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}