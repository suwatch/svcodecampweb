using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using CodeCampSV;
using Gurock.SmartInspect;
using ListNanny;

namespace WebAPI
{
    public class Global : System.Web.HttpApplication
    {
        private bool _smartInspectEnabled;

        protected void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);


            //if (_smartInspectEnabled)
            //{
            //    string fileName = HttpContext.Current.Server.MapPath("~\\App_Data\\Logging.sic");

            //    SiAuto.Si.LoadConfiguration(fileName);
            //    SiAuto.Main.EnterProcess();
            //}


            string baseDir = HttpContext.Current.Server.MapPath("~\\App_Data\\");

            aspNetPOP3.POP3.LoadLicenseFile(string.Format("{0}aspNetPOP3.xml.lic", baseDir));
            aspNetEmail.EmailMessage.LoadLicenseFile(string.Format("{0}aspNetEmail.xml.lic", baseDir));
            aspNetMime.MimeMessage.LoadLicenseFile(string.Format("{0}aspNetMime.xml.lic", baseDir));
            NDR.LoadLicenseFile(string.Format("{0}ListNanny.xml.lic", baseDir));



            if (!Roles.RoleExists("superuser")) Roles.CreateRole("superuser");
            if (!Roles.RoleExists("surveyviewer")) Roles.CreateRole("surveyviewer");
            if (!Roles.RoleExists("admin")) Roles.CreateRole("admin");
            if (!Roles.RoleExists("presenter")) Roles.CreateRole("presenter");
            if (!Roles.RoleExists("scheduler")) Roles.CreateRole("scheduler");
            if (!Roles.RoleExists("scheduleviewer")) Roles.CreateRole("scheduleviewer");
            if (!Roles.RoleExists("trackadmin")) Roles.CreateRole("trackadmin");
            if (!Roles.RoleExists("removeprimaryspeaker")) Roles.CreateRole("removeprimaryspeaker");
            if (!Roles.RoleExists("AddMoreThanTwoSessions")) Roles.CreateRole("AddMoreThanTwoSessions");
            if (!Roles.RoleExists("AddTwoSessions")) Roles.CreateRole("AddTwoSessions");
            if (!Roles.RoleExists("AddThreeSessions")) Roles.CreateRole("AddThreeSessions");
            if (!Roles.RoleExists("AddFourSessions")) Roles.CreateRole("AddFourSessions");
            if (!Roles.RoleExists("VolunteerCoordinator")) Roles.CreateRole("VolunteerCoordinator");
            if (!Roles.RoleExists("NoAutoLoginForGUID")) Roles.CreateRole("NoAutoLoginForGUID");
            if (!Roles.RoleExists("VideoEditor")) Roles.CreateRole("VideoEditor");
            if (!Roles.RoleExists("TagGroupGraphViewer")) Roles.CreateRole("TagGroupGraphViewer");
            if (!Roles.RoleExists("ReferralMaker")) Roles.CreateRole("ReferralMaker");
            if (!Roles.RoleExists("AllowRegistration")) Roles.CreateRole("AllowRegistration");
            if (!Roles.RoleExists("SponsorManager")) Roles.CreateRole("SponsorManager");
            if (!Roles.RoleExists("SubmitSession")) Roles.CreateRole("SubmitSession");
            if (!Roles.RoleExists("SessionHashTagger")) Roles.CreateRole("SessionHashTagger");
            if (!Roles.RoleExists("SpeakerAssignOwnMaterialsUrl")) Roles.CreateRole("SpeakerAssignOwnMaterialsUrl");



            MembershipUser mu = Membership.GetUser("pkellner");
            if (mu == null)
            {
                MembershipCreateStatus outStat;
                Membership.CreateUser("pkellner", "peterk", "peter@peterkellner.net", "q", "a", true, out outStat);
            }

            if (!Roles.IsUserInRole("pkellner", "admin"))
            {
                Roles.AddUserToRole("pkellner", "admin");
            }


            ImageResizer.Configuration.Config.Current.Pipeline.RewriteDefaults +=
                (m, c, args) =>
                {
                    var imageDirs = new List<string>
                                                 {
                                                     "/sponsorimage/",
                                                     "/attendeeimage/",
                                                     "/trackimage"
                                                 };

                    foreach (var imageDirPrefix in imageDirs)
                    {
                        if (args.VirtualPath.IndexOf(imageDirPrefix, StringComparison.OrdinalIgnoreCase) > -1)
                            args.QueryString["404"] = "~/Images/404-not-found-error.jpg";
                    }
                };



        }

        protected void Session_Start(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["CurrentCodeCampYearNumber"] != null)
            {
                int codeCampYear = Convert.ToInt32(ConfigurationManager.AppSettings["CurrentCodeCampYearNumber"]);
                Session["CodeCampYear"] = codeCampYear;
                //todo: move to cookie
            }

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        public void Application_OnPostRequestHandlerExecute(Object sender, EventArgs e)
        {
            if (
                Request.UrlReferrer != null && !Request.UrlReferrer.IsLoopback &&
                !Request.UrlReferrer.ToString().ToLower().Contains("siliconvalley-codecamp.com") &&
                !String.IsNullOrEmpty(Request.Path) &&
                ConfigurationManager.AppSettings["LogReferrerRequests"] != null &&
                ConfigurationManager.AppSettings["LogReferrerRequests"].ToLower().Equals("true"))
            {
                ReferralLogger referralLogger;
                if (String.Empty != Request.QueryString.ToString())
                {
                    referralLogger = new ReferralLogger(Request.Path + "?" +
                                                        Request.QueryString,
                                                        Request.UrlReferrer);
                }
                else
                {
                    referralLogger = new ReferralLogger(Request.Path, Request.UrlReferrer);
                }
                referralLogger.Log();
            }
        }
    }
}