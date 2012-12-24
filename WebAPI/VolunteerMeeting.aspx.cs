using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;

public partial class VolunteerMeeting : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {

        if (Context.User.Identity.IsAuthenticated && !IsPostBack && Utils.CheckUserIsAdmin())
        {
            GridView1.Visible = true;
            LabelAdmin.Visible = true;
            LabelAdmin.Text = "Stats for ADMIN EYES ONLY";
        }
        else
        {
            GridView1.Visible = false;
            LabelAdmin.Visible = false;
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        int maxAttend = 0;
        int currentPlannedAttend = 0;

        bool notInterested = false;
        bool interested = false;
        bool planToAttend = false;

        // only call sql if authenticated to thwart denial of service attacks killing this page not logged in
        if (User.Identity.IsAuthenticated)
        {

            // if authenticated and first time on page, then get current value and if null or 0, set to 1
            if (!IsPostBack && User.Identity.IsAuthenticated)
            {
                var attendeeRec =
                    AttendeesManager.I.Get(new AttendeesQuery {Username = User.Identity.Name}).FirstOrDefault();
                if (attendeeRec != null)
                {
                    if (attendeeRec.VolunteerMeetingStatus == null)
                    {
                        attendeeRec.VolunteerMeetingStatus = 0;
                        AttendeesManager.I.Update(attendeeRec);
                    }
                    else if (attendeeRec.VolunteerMeetingStatus == 1)
                    {
                        notInterested = true;
                    }
                    else if (attendeeRec.VolunteerMeetingStatus == 2)
                    {
                        interested = true;
                    }
                    else if (attendeeRec.VolunteerMeetingStatus == 3)
                    {
                        planToAttend = true;
                    }

                }
            }


            if (ConfigurationManager.AppSettings["VolunteerMeetingMaxAttendance"] != null)
            {
                maxAttend = Convert.ToInt32(ConfigurationManager.AppSettings["VolunteerMeetingMaxAttendance"]);
            }

            // if we have set the capacity above 0, let speakers in always
            currentPlannedAttend = Utils.GetNumberVolunteerMeetingPlannedAttend();
            if (Utils.CheckUserIsPresenter() && maxAttend > 0)
            {
                maxAttend = 500; // let all speakers come
            }


            if (Utils.CheckUserIsAdmin())
            {

            }
        }

        if (!IsPostBack)
        {
            if (currentPlannedAttend >= maxAttend || !Context.User.Identity.IsAuthenticated)
            {
                EventInterestId.Items.AddRange(new[]
                                                   {
                                                       new ListItem(
                                                           "Not Interested Or Not Able to Attend Our Pre-Camp Meeting",
                                                           "1"),
                                                       new ListItem("Interested in Attending Our Pre-Camp Meeting", "2")
                                                   }
                    );
                if (interested)
                {
                    EventInterestId.SelectedIndex = 1;
                }
                else if (notInterested)
                {
                    EventInterestId.SelectedIndex = 0;
                }
            }
            else
            {
                EventInterestId.Items.AddRange(new[]
                                                   {
                                                       new ListItem(
                                                           "Not Interested Or Not Able to Attend Our Pre-Camp Meeting",
                                                           "1"),
                                                       new ListItem("Interested in Attending Our Pre-Camp Meeting", "2")
                                                       ,

                                                       new ListItem("I Will Attend", "3")
                                                   });
                if (interested)
                {
                    EventInterestId.SelectedIndex = 1;
                }
                else if (notInterested)
                {
                    EventInterestId.SelectedIndex = 0;
                }
                else if (planToAttend)
                {
                    EventInterestId.SelectedIndex = 2;
                }
            }
        }

        if (User.Identity.IsAuthenticated)
        {
            LabelUsername.Text = " (" + User.Identity.Name + ") ";
            LabelNeedToBeLoggedIn.Visible = false;
            EventInterestId.Enabled = true;
        }
        else
        {
            LabelNeedToBeLoggedIn.Visible = true;
            EventInterestId.Enabled = false;
        }


        if (Request.QueryString["PKID"] != null && !User.Identity.IsAuthenticated)
        {
            string guidString = Request.QueryString["PKID"];
            string username = Utils.GetAttendeeUsernameByGUID(guidString);
            if (!String.IsNullOrEmpty(username))
            {
                if (!Utils.GetIgnoreAutoSignOnGuid(username))
                {
                    if (!string.IsNullOrEmpty(username))
                    {
                        if (User.Identity.IsAuthenticated)
                        {
                            FormsAuthentication.SignOut();
                        }
                        FormsAuthentication.SetAuthCookie(username, true);
                        Response.Redirect("~/VolunteerMeeting.aspx", true);
                    }
                }
            }
        }




    }


    protected void EventInterestId_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            if (EventInterestId.SelectedIndex == 0)
            {
                var attendeeRec =
                    AttendeesManager.I.Get(new AttendeesQuery {Username = User.Identity.Name}).FirstOrDefault();
                if (attendeeRec != null)
                {
                    attendeeRec.VolunteerMeetingStatus = 1; // not interested
                    AttendeesManager.I.Update(attendeeRec);
                }
            }
            else if (EventInterestId.SelectedIndex == 1)
            {
                var attendeeRec =
                    AttendeesManager.I.Get(new AttendeesQuery {Username = User.Identity.Name}).FirstOrDefault();
                if (attendeeRec != null)
                {
                    attendeeRec.VolunteerMeetingStatus = 2; //  interested
                    AttendeesManager.I.Update(attendeeRec);
                }
            }
            else if (EventInterestId.SelectedIndex == 2)
            {
                var attendeeRec =
                    AttendeesManager.I.Get(new AttendeesQuery {Username = User.Identity.Name}).FirstOrDefault();
                if (attendeeRec != null)
                {
                    attendeeRec.VolunteerMeetingStatus = 3; // will attend
                    AttendeesManager.I.Update(attendeeRec);
                }
            }
        }

    }
}