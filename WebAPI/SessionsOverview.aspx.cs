using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;
using System.Configuration;
using System.Diagnostics;

public partial class SessionsOverview : BaseContentPage
{
    protected bool RadioButtonP2APressed
    {
        get
        {
            return ViewState["RadioButtonP2APressed"] != null
                && (bool)ViewState["RadioButtonP2APressed"];
        }
        set
        {
            ViewState["RadioButtonP2APressed"] = value;
        }
    }

    private List<SessionAttendeeODS.DataObjectSessionAttendee> liSessionAttendee;

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //Label lblFooter = RepeaterTimes.HeaderTemplate.
        //lblFooter.Text = "abcd";

        //if (RadioButtonP2APressed && RadioButtonIPTAList.Visible)
        //{

        //    RepeaterTimes.DataBind();
        //}

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        RadioButtonP2APressed = false;
    }

    SessionInterestAnalysis _sessionInterestAnalysis;
    List<int> _sessionsSuggested;

    protected void Page_Load(object sender, EventArgs e)
    {
       

        if (Context.User.Identity.IsAuthenticated)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            _sessionInterestAnalysis = new SessionInterestAnalysis(6, 0);
            stopWatch.Stop();
            string str = stopWatch.ElapsedMilliseconds.ToString() + ":";
            stopWatch = new Stopwatch();
            stopWatch.Start();
            _sessionsSuggested = _sessionInterestAnalysis.GetSessionIds(Context.User.Identity.Name, "P");
            stopWatch.Stop();
            LabelTiming.Text = str + stopWatch.ElapsedMilliseconds.ToString();
        }



        if (!IsPostBack)
        {
            SqlDataSourceTimes.CacheDuration = Utils.RetrieveSecondsForSessionCacheTimeout();

            if (Context.User.Identity.IsAuthenticated)
            {
                LabelTiming.Visible = Utils.CheckUserIsAdmin();
            }

        }

        if (Request.QueryString["PKID"] != null)
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
                        Response.Redirect("~/SessionsOverview.aspx", true);
                    }
                }
            }
        }


        LabelCodeCampYearId.Text = Utils.GetCurrentCodeCampYear().ToString();

        if (Context.User.Identity.IsAuthenticated)
        {
            RadioButtonIPTAList.Visible = true;
            var sessionAttendeeODS = new SessionAttendeeODS();
            liSessionAttendee = sessionAttendeeODS.GetByUsername(Context.User.Identity.Name);
        }
        else
        {
            RadioButtonIPTAList.Visible = false;
        }
    }

    protected string GetHTMLForTimeSlot(int sessionTimesId, object beforeAgendaObj, object afterAgendaObj)
    {
        bool showRoomOnSchedule = ConfigurationManager.AppSettings["ShowRoomOnSchedule"] != null &&
                                  ConfigurationManager.AppSettings["ShowRoomOnSchedule"].ToLower().Equals("true");

        bool showSpeakerHyperLinks = !(ConfigurationManager.AppSettings["ShowSpeakerHyperlinks"] != null &&
                                       ConfigurationManager.AppSettings["ShowSpeakerHyperlinks"].ToLower().Equals("false"));


        string beforeAgenda = beforeAgendaObj.Equals(DBNull.Value) ? string.Empty : (string) beforeAgendaObj;
        string afterAgenda = afterAgendaObj.Equals(DBNull.Value) ? string.Empty : (string) afterAgendaObj;


        var sb = new StringBuilder();

        var soODS = new SessionsOverviewODS();
        List<SessionsOverviewODS.DataObjectSessionsOverview> liSO = soODS.GetBySessionTimesId(sessionTimesId,Convert.ToInt32(LabelCodeCampYearId.Text));

        if (!String.IsNullOrEmpty(beforeAgenda))
        {
            sb.Append(beforeAgenda);
        }

        int totalSessionInSlot = liSO.Count;

        var attendeesList = new List<Attendees>();

        //var speakerIdsInTimeSlot = new List<int>();

        // From Elluis on format
        // <lu>
        //    <li><a href="..." alt="..." class="sessionTitle">Sessions Title</a> <a>Speaker 1a</a>, <a>Speaker 2a</a> <span>Room</span></li>
        //    <li><a class="sessionTitle">Sessions Title</a> <a>Speaker 1b</a>, <a>Speaker 2b</a> <span>Room</span></li>
        //</lu>

        var sessionAttendeeOds = new SessionAttendeeODS();
        bool isAdmin = Utils.CheckUserIsAdmin();

        int totalInterestInSlot = 0;
        int totalPlanToAttendInSlot = 0;
        
        sb.Append("<ul>");
        foreach (SessionsOverviewODS.DataObjectSessionsOverview sessionOverview in liSO)
        {
            var speakerList = Utils.GetSpeakersBySessionId(sessionOverview.Sessionid);

            attendeesList.AddRange(speakerList);


            bool hasInterest = false;
            bool willAttend = false;

            if (liSessionAttendee != null)
            {
                foreach (SessionAttendeeODS.DataObjectSessionAttendee sa in liSessionAttendee)
                {
                    if (sa.Sessions_id == sessionOverview.Sessionid)
                    {
                        if (sa.Interestlevel == Convert.ToInt32(Utils.InterestLevel.Interested))
                        {
                            hasInterest = true;
                        }
                        if (sa.Interestlevel == Convert.ToInt32(Utils.InterestLevel.WillAttend))
                        {
                            willAttend = true;
                        }
                    }
                }
            }

            

            bool includeSessionInList = true;

            if (RadioButtonIPTAList.SelectedValue.Equals("Interested"))
            {
                includeSessionInList = hasInterest;
            }
            else if (RadioButtonIPTAList.SelectedValue.Equals("Plan To Attend"))
            {
                includeSessionInList = willAttend;
            }
            else if (RadioButtonIPTAList.SelectedValue.Equals("I & P2A"))
            {
                includeSessionInList = hasInterest || willAttend;
            }

            if (includeSessionInList)
            {
                string tagListString = string.Empty;
                var tagsODS = new TagsODS();
                List<TagsODS.DataObjectTags> listTags = tagsODS.GetAllBySession(string.Empty, sessionOverview.Sessionid);
                var sbTags = new StringBuilder();

                if (listTags.Count > 0)
                {
                    foreach (TagsODS.DataObjectTags t in listTags)
                    {
                        sbTags.Append(t.Tagname);
                        sbTags.Append(",");
                    }
                    tagListString = sbTags.ToString();
                }

                string titleLink =
                    string.Format(
                        "<a class=\"sessionTitle\" topic=\"{0}\" href=\"Sessions.aspx?OnlyOne=true&id={1}\">{2}</a>",
                        tagListString, sessionOverview.Sessionid, sessionOverview.Title);

                string evalLink = string.Format("<a href=\"SessionEval.aspx?id={0} \">Evaluate</a>",
                                                sessionOverview.Sessionid);

                sb.Append("<li>");
                sb.Append(titleLink);
                sb.Append("&nbsp;");
                sb.Append("&nbsp;");
                var stringBuilderSpeaker = new StringBuilder();
                for (int index = 0; index < speakerList.Count; index++)
                {
                    var attendee = speakerList[index];
                    stringBuilderSpeaker.Append(Utils.FormatSpeakerHyperlink(attendee.UserFirstName,
                                                                             attendee.UserLastName,
                                                                             attendee.Id,
                                                                             attendee.SaturdayClasses,
                                                                             attendee.SundayClasses,
                                                                             showSpeakerHyperLinks));
                    if (index != speakerList.Count - 1)
                    {
                        stringBuilderSpeaker.Append(",&nbsp;&nbsp;");
                    }
                }
                string stringSpeakers = stringBuilderSpeaker.ToString();
                if (!String.IsNullOrEmpty(stringSpeakers))
                {
                    if (stringSpeakers.EndsWith("&nbsp;&nbsp;,"))
                    {
                        stringSpeakers = stringSpeakers.TrimEnd(new[] {','});
                    }
                    sb.Append(stringSpeakers);
                }

                if (showRoomOnSchedule)
                {
                    sb.Append("&nbsp;");
                    sb.Append("&nbsp;");
                    sb.Append("Room:");
                    sb.Append("&nbsp;");
                    sb.Append("&nbsp;");
                    sb.Append(sessionOverview.Number);
                    sb.Append("&nbsp;");
                    sb.Append("&nbsp;");
                }

                if (ConfigurationManager.AppSettings["ShowSessionEvaluations"] != null &&
                    ConfigurationManager.AppSettings["ShowSessionEvaluations"].ToLower().Equals("true") &&
                    Context.User.Identity.IsAuthenticated)
                {
                    sb.Append(evalLink);
                    sb.Append("&nbsp;");
                    sb.Append("&nbsp;");
                }

                if (isAdmin)
                {
                    int numInterested = sessionAttendeeOds.GetCountBySessionIdAndInterest(sessionOverview.Sessionid, 2);
                    int numPlanToAttend = sessionAttendeeOds.GetCountBySessionIdAndInterest(sessionOverview.Sessionid, 3);
                    totalInterestInSlot += numInterested;
                    totalPlanToAttendInSlot += numPlanToAttend;
                    sb.Append(String.Format("<i>{{I:{0},P:{1}}}</i>", numInterested, numPlanToAttend));
                }


                if (hasInterest)
                {
                    sb.Append("<b><span class=\"SessionInterestText\">(Interested)</span></b>");
                }
                else if (willAttend)
                {
                    sb.Append("<b><span class=\"SessionPlanOnAttendText\">(Planning On Attending)</span></b>");
                }
                else
                {
                    if (Context.User.Identity.IsAuthenticated)
                    {
                        // check and see if on suggested list
                        if (_sessionsSuggested.Contains(sessionOverview.Sessionid))
                        {
                            sb.Append("<b><span class=\"SessionSuggestedText\">(Similar To Your Other Choices)</span></b>");
                        }
                    }
                }



                sb.Append("</li>");
                sb.Append("\r\n");
                sb.Append("\r\n");
            }
        }
        sb.Append("</ul>");

        if (Utils.CheckUserIsAdmin())
        {
            sb.AppendLine(
                "<i>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            sb.AppendLine("{Number Sessions This Timeslot (admin only): ");
            sb.AppendLine(totalSessionInSlot.ToString());
            sb.AppendLine(String.Format(" Total I: {0} Total P: {1} ", totalInterestInSlot, totalPlanToAttendInSlot));

            // see if any speaker listed twice here
            //  attendeesList

            var recDups = from p in attendeesList
                        group p by p.Id
                        into g
                        where g.Count() > 1
                        select new
                                   {
                                       g.Key
                                   };
            foreach (var recDup in recDups)
            {
                string firstName =
                   (from data in attendeesList where data.Id == recDup.Key select data.UserFirstName).FirstOrDefault();
                string lastName =
                   (from data in attendeesList where data.Id == recDup.Key select data.UserLastName).FirstOrDefault();
                sb.AppendLine("Duplicated Speaker: " + firstName + " " + lastName);
            }


            sb.AppendLine("}</i><br/><br/>");
        }

        if (!String.IsNullOrEmpty(afterAgenda))
        {
            sb.Append(afterAgenda);
        }

        return sb.ToString();
    }

    protected void RepeaterTimes_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
    }

    protected void RepeaterTimes_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Header)
        {
            string dateString = Utils.GetCodeCampDateStringByCodeCampYearId(Utils.GetCurrentCodeCampYear());
            var l1 = e.Item.FindControl("LabelCodeCampYear1") as Label;
            if (l1 != null)
            {
                l1.Text = dateString;
            }
            var l2 = e.Item.FindControl("LabelCodeCampYear2") as Label;
            if (l2 != null)
            {
                l2.Text = dateString;
            }
        }
    }
    protected void RadioButtonIPTAList_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButtonP2APressed = true;
    }
}