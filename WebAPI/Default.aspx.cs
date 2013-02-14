using System;
using System.Configuration;
using System.Web.UI;
using CodeCampSV;

public partial class Default : BaseContentPage
{

    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Request["Year"] != null)
        {
            int newYear = Convert.ToInt32(Request["Year"]);

            Session["CodeCampYear"] = newYear - 2005;


            //if (newYear == 2008)
            //{
            //    Session["CodeCampYear"] = 3;
            //}
            //else if (newYear == 2009)
            //{
            //    Session["CodeCampYear"] = 4;
            //}
            //else if (newYear == 2010)
            //{
            //    Session["CodeCampYear"] = 5;
            //}
            //else if (newYear == 2011)
            //{
            //    Session["CodeCampYear"] = 6;
            //}
            //else if (newYear == 2012)
            //{
            //    Session["CodeCampYear"] = 7;
            //}
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var testMode = ConfigurationManager.AppSettings["TestingDataOnly"] != null &&
                       ConfigurationManager.AppSettings["TestingDataOnly"].ToLower().Equals("true");

            if (testMode)
            {
                Response.Redirect("~/Session");
            }


            if (ConfigurationManager.AppSettings["ChartAttendanceComparedLastYear"] != null &&
                ConfigurationManager.AppSettings["ChartAttendanceComparedLastYear"].Equals("true"))
            {
                ShowChartWithAttendeesCountsID.Visible = true;
            }
            else
            {
                ShowChartWithAttendeesCountsID.Visible = false;
            }

            if (ConfigurationManager.AppSettings["SteveJobsMemory"] != null &&
                ConfigurationManager.AppSettings["SteveJobsMemory"].Equals("true"))
            {
                SteveJobsMemorialId.Visible = true;
                NormalPageId.Visible = false;
            }
            else
            {
                SteveJobsMemorialId.Visible = false;
                NormalPageId.Visible = true;
            }



            if (!Context.User.Identity.IsAuthenticated)
            {
                // not logged in
                CodeCampAnnounceID.Visible = true;
                TwitterFeed1.Visible = false;
                TwitterFeed2.Visible = false;
                jobsMemorialLogin.Visible = false;
                jobsMemorialLogout.Visible = false;
                horizontalDivider.Visible = false;
                ShowTwitterSessionLinksId.Visible = false;
            }
            else
            {
                // logged in

                if (!testMode)
                {
                    if (ConfigurationManager.AppSettings["SpeakerShirtSizeForce"] != null &&
                        ConfigurationManager.AppSettings["SpeakerShirtSizeForce"].Equals("true"))
                    {
                        int attendeesId = Utils.GetAttendeesIdFromUsername(Context.User.Identity.Name);
                        if (Utils.CheckAttendeeIdIsSpeaker(attendeesId))
                        {
                            bool hasShirtSize = Utils.CheckHasShirtSize(attendeesId);
                            if (!hasShirtSize)
                            {
                                Response.Redirect("~/ShirtSizeSet.aspx");
                            }
                        }
                    }
                }

                if (ConfigurationManager.AppSettings["ShowTwitterLinksOnHomePage"] != null &&
                   ConfigurationManager.AppSettings["ShowTwitterLinksOnHomePage"].Equals("true"))
                {
                    ShowTwitterSessionLinksId.Visible = true;
                }
                else
                {
                    ShowTwitterSessionLinksId.Visible = false;
                }


                CodeCampAnnounceID.Visible = false;
                TwitterFeed1.Visible = false;
                TwitterFeed2.Visible = false;
                jobsMemorialLogin.Visible = false;
                jobsMemorialLogout.Visible = false;
                horizontalDivider.Visible = false;

            }
        }

        string threshHoldString = ConfigurationManager.AppSettings["ShowRegThreshHold"];
        int threshHoldToShowRegisteredNumber = Convert.ToInt32(threshHoldString);
        int numberRegistered = Utils.GetNumberRegistered();
        if (numberRegistered >= threshHoldToShowRegisteredNumber)
        {
            int numberSessions = Utils.GetNumberSessions();
            string str = String.Format("{0} Sessions,{1} Registered", numberSessions, numberRegistered);

            if (ConfigurationManager.AppSettings["ShowVolunteerJobsToAttendees"] != null &&
                ConfigurationManager.AppSettings["ShowVolunteerJobsToAttendees"].ToLower().Equals("true"))
            {

                int numberNeeded = Utils.GetNumberVolunteersNeededYear();
                if (numberNeeded > 0)
                {
                    int numberVolunteered = Utils.GetNumberVolunteeredThisYear();
                    str += " " + String.Format("({0} More Volunteers Needed!)",
                                                       numberNeeded - numberVolunteered);
                }
            }

            LabelStatus.Text = str;

        }
        else
        {
            LabelStatus.Text = String.Empty;
        }
    }








}