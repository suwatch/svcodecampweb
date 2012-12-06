using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;


namespace WebAPI
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void CheckBoxShowAllCloudTags_CheckedChanged(object sender, EventArgs e)
        {
            CloudControl2.TagsToShow = CheckBoxShowAllCloudTags.Checked ? Utils.DefaultCloudTagsToShow : 0;
            CloudControl2.DataBindCloudControl();
        }

        //void Page_PreInit(Object sender, EventArgs e)
        //{
        //    this.MasterPageFile = "~/NewMaster.master";
        //}

        protected override void OnInit(EventArgs e)
        {
            if (Request.RawUrl.EndsWith("Sponsors.aspx"))
            {
                SponsorChannelDiv.Visible = false;
            }

            if (!IsPostBack)
            {


                if (Request.RawUrl.EndsWith("SessionsOverview.aspx") ||
                    Request.RawUrl.EndsWith("Sessions.aspx") ||
                    Request.RawUrl.EndsWith("Speakers.aspx"))
                {
                    CheckBoxShowAllCloudTags.Checked = true;
                    CloudControl2.TagsToShow = CheckBoxShowAllCloudTags.Checked ? Utils.DefaultCloudTagsToShow : 10000;
                    CloudControl2.DataBindCloudControl();
                }
                else
                {
                    cloudcontrolid.Visible = false;
                }

                if (ConfigurationManager.AppSettings["ShowSessionInterest"] != null &&
                    ConfigurationManager.AppSettings["ShowSessionInterest"].ToLower().Equals("true"))
                {
                    HyperLinkSessionInterest.Visible =
                        ConfigurationManager.AppSettings["ShowSessionInterest"].ToLower().Equals("true");
                }

                if (ConfigurationManager.AppSettings["ScheduleAllowCheckAttend"] != null &&
                     ConfigurationManager.AppSettings["ScheduleAllowCheckAttend"].ToLower().Equals("true"))
                {
                    HyperLinkSessionInterest.Visible = false;
                    HyperLinkSessionPlanToAttend.Visible =
                        ConfigurationManager.AppSettings["ScheduleAllowCheckAttend"].ToLower().Equals("true");
                }

                if (ConfigurationManager.AppSettings["SessionChangeLogPage"] != null &&
                    ConfigurationManager.AppSettings["SessionChangeLogPage"].ToLower().Equals("true"))
                {
                    HyperLink4.NavigateUrl = ConfigurationManager.AppSettings["SessionChangeLogPage"].ToString();
                }

                if (ConfigurationManager.AppSettings["ShowVolunteerMeeting"] != null &&
                    ConfigurationManager.AppSettings["ShowVolunteerMeeting"].ToLower().Equals("true"))
                {
                    HyperLinkVolunteersMeeting.Visible = true;
                }

                if (Context.User.Identity.IsAuthenticated)
                {
                    if (ConfigurationManager.AppSettings["ShowVolunteerJobsToAttendees"].ToLower().Equals("true")
                    || Utils.CheckUserIsAdmin() || Utils.CheckUserIsVolunteerCoordinator())
                    {
                        HyperLinkVolunteerJobs.Visible = true;
                    }
                    else
                    {
                        HyperLinkVolunteerJobs.Visible = false;
                    }
                }
                else
                {
                    HyperLinkVolunteerJobs.Visible = false;
                }


            }

            if (Request.RawUrl.IndexOf("Sessions.aspx?", StringComparison.Ordinal) > 0)
            {
                //const string includeScript = "http://stream.siliconvalley-codecamp.com/chat/bootstrap.js";
                //Page.ClientScript.RegisterClientScriptInclude("chatscript", includeScript);
            }

            //if (ConfigurationManager.AppSettings["IncludeExtJS"].ToLower().Equals("true"))
            //{
            //    //<script src="assets/js/ext-2.2/adapter/ext/ext-base.js" type="text/javascript"></script>
            //    //<script src="assets/js/ext-2.2/ext-all-debug.js" type="text/javascript"></script>
            //    //<link href="assets/js/ext-2.2/resources/css/ext-all.css" rel="stylesheet" type="text/css" />
            //    //string str1 = "assets/js/ext-2.2/adapter/ext/ext-base.js";
            //    //string str2 = "assets/js/ext-2.2/ext-all-debug.js";
            //    ////string str3 = "assets/js/ext-2.2/adapter/ext/ext-base.js";

            //    //Page.ClientScript.RegisterClientScriptInclude("chatscript", str1);
            //    //Page.ClientScript.RegisterClientScriptInclude("chatscript", str2);   
            //}




            //HyperLinkCodeCampEval.Visible = Context.User.Identity.IsAuthenticated;

            base.OnInit(e);
        }





        //THIS DID NOT WORK???
        //protected void Page_PreInit(object sender, EventArgs e)
        //{
        //    //if (Request.RawUrl.EndsWith("Sponsors.aspx"))
        //    //{
        //    //    SponsorChannelDiv.Visible = false;
        //    //}

        //    //if (!IsPostBack)
        //    //{
        //    //    if (Request.RawUrl.EndsWith("SessionsOverview.aspx"))
        //    //    {
        //    //        CheckBoxShowAllCloudTags.Checked = false;
        //    //        //CloudControl2.TagsToShow = CheckBoxShowAllCloudTags.Checked ? Utils.DefaultCloudTagsToShow : 0;
        //    //        //CloudControl2.DataBindCloudControl();
        //    //    }
        //    //}
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["VolunteerPage"] != null)
            {
                //string str = ConfigurationManager.AppSettings["VolunteerPage"].ToString();
                //HyperLinkVolunteer.NavigateUrl = str; // "http://codecamp.pbwiki.com/CodeCampSiliconValleyVolunteers";

                if (!Context.User.Identity.IsAuthenticated)
                {
                    HyperLinkProfileInfo.Visible = false;
                }
            }
        }
    }
}