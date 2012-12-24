﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;
using System.Web.UI.HtmlControls;


public partial class Sessions : BaseContentPage // BasePage {BasePage tracks viewstate size with errors)
{
    protected Dictionary<int, string> SessionLevelsDictionary;

    private List<SessionAttendeeODS.DataObjectSessionAttendee> _listSessionAttendees;

    private Dictionary<int, string> _boxFolderIdDictionary;
    private Dictionary<int, string> _materialsDictionary;

   

   

    //private Dictionary<int, SessionsJobListingResult> _sessionJobListingDictionary;
    private Dictionary<int, SponsorListResult> _sessionSponsorListDictionary;

    //private Dictionary<int, bool> _sessionListPbWikiShow; 

    //private Dictionary<int, SponsorListJobListingResult> _jobListingsDictionary;


    protected Dictionary<int, string> TrackNameAssignedToSession
    {
        get
        {
            return (Dictionary<int, string>)ViewState["TrackNameAssignedToSession"];
        }
        set
        {
            ViewState["TrackNameAssignedToSession"] = value;
        }
    }

    
    protected bool RadioButtonSatSunPressed
    {
        get
        {
            return ViewState["RadioButtonSatSunPressed"] != null 
                && (bool) ViewState["RadioButtonSatSunPressed"];
        }
        set
        {
            ViewState["RadioButtonSatSunPressed"] = value;
        }
    }

    protected bool SearchButtonPressed
    {
        get
        {
            return ViewState["SearchButtonPressed"] != null
                && (bool)ViewState["SearchButtonPressed"];
        }
        set
        {
            ViewState["SearchButtonPressed"] = value;
        }
    }

    protected string SearchText
    {
        get
        {
            return (string) ViewState["SearchText"];
        }
        set
        {
            ViewState["SearchText"] = value;
        }
    }

    protected bool RadioButtonTimesListPressed
    {
        get
        {
            return ViewState["RadioButtonTimesListPressed"] != null
                && (bool)ViewState["RadioButtonTimesListPressed"];
        }
        set
        {
            ViewState["RadioButtonTimesListPressed"] = value;
        }
    }

    
    protected List<TrackResult> TrackResults
    {
        get
        {
            return (List<TrackResult>) ViewState["TrackResults"];
        }
        set
        {
            ViewState["TrackResults"] = value;
        }
    }

    protected List<TrackSessionResult> TrackSessionResults
    {
        get
        {
            return (List<TrackSessionResult>)ViewState["TrackSessionResults"];
        }
        set
        {
            ViewState["TrackSessionResults"] = value;
        }
    }

    protected DateTime SaturdayDate
    {
        get
        {
            return (DateTime)ViewState["SaturdayDate"];
        }
        set
        {
            ViewState["SaturdayDate"] = value;
        }
    }

    protected DateTime SundayDate
    {
        get
        {
            return (DateTime)ViewState["SundayDate"];
        }
        set
        {
            ViewState["SundayDate"] = value;
        }
    }

    protected List<DateTime> SaturdaySessionTimesList
    {
        get
        {
            return (List<DateTime>)ViewState["SaturdaySessionTimesList"];
        }
        set
        {
            ViewState["SaturdaySessionTimesList"] = value;
        }
    }

    protected bool RegisteredForCurrentYear
    {
        get
        {
            return (bool)ViewState["RegisteredForCurrentYear"];
        }
        set
        {
            ViewState["RegisteredForCurrentYear"] = value;
        }
    }

  

    protected List<DateTime> SundaySessionTimesList
    {
        get
        {
            return (List<DateTime>)ViewState["SundaySessionTimesList"];
        }
        set
        {
            ViewState["SundaySessionTimesList"] = value;
        }
    }

    protected DateTime? SelectedSessionTimeSlot
    {
        get
        {
            return (DateTime?)ViewState["SelectedSessionTimeSlot"];
        }
        set
        {
            ViewState["SelectedSessionTimeSlot"] = value;
        }
    }

   



    protected void DropDownListTracks_SelectedIndexChanged(object sender, EventArgs e)
    {
        var ddl = (DropDownList) sender;

        if (ddl.SelectedItem.Text.StartsWith("--Remove"))
        {
            int sessionId = Convert.ToInt32(ddl.SelectedItem.Value);
            var recs = TrackSessionManager.I.Get(new TrackSessionQuery() { SessionId = sessionId });
            foreach (var rec in recs)
            {
                TrackSessionManager.I.Delete(rec.Id);
            }
        }
        else
        {

            string trackId = ddl.SelectedValue;
            string sessionIdStr = ddl.ToolTip;

            var tracs = TrackSessionManager.I.Get(new TrackSessionQuery()
                                                      {
                                                          SessionId = Convert.ToInt32(sessionIdStr)
                                                      });
            foreach (var trac in tracs)
            {
                TrackSessionManager.I.Delete(trac.Id);
            }

            if (Convert.ToInt32(trackId) >= 0)
            {
                TrackSessionManager.I.Insert(new TrackSessionResult()
                                                 {
                                                     TrackId = Convert.ToInt32(trackId),
                                                     SessionId = Convert.ToInt32(sessionIdStr)
                                                 });
            }
        }
    }

    // SessionsPageUsingExtJS

    protected void Page_Init(object sender, EventArgs e)
    {
        LabelSessionListToShow.Text = string.Empty;
        MainHeadingDIV.Visible = true;

        // these should be off in production
        LabelSessionListToShow.Visible = false;
        LabelSessionListFromSearch.Visible = LabelSessionListToShow.Visible;
        LabelSessionListFinalToShow.Visible = LabelSessionListToShow.Visible;
        

        if (ConfigurationManager.AppSettings["SessionsPageUsingExtJS"] != null &&
            ConfigurationManager.AppSettings["SessionsPageUsingExtJS"].ToLower().Equals("true"))
        {
            SessionsContainerId.Visible = false;
            IdSortBy.Visible = false;
            IdTrackDescription.Visible = false;
            CheckBoxShowOnlyAssigned.Visible = false;

            SessionsUsingExtJSFromDOMId.Visible = true;
        }

      
        InitializeViewStateForTimeLists();

        
    }

    /// <summary>
    /// Initialize all the view state date for code camp viewstate stuff like
    /// which Date is Sat/Sun, lists of sessions, etc.
    /// </summary>
    private void InitializeViewStateForTimeLists()
    {
        // store in viewstate for convenience dates of cc
        SaturdayDate = Utils.GetCurrentCodeCampYearStartDate();
        SundayDate = SaturdayDate.AddDays(1);

        // Load up session times for both days
        var sessionTimesODS = new SessionTimesODS();
        var sessionTimesList = sessionTimesODS.GetAllSessionTimes();
        SaturdaySessionTimesList = new List<DateTime>();
        foreach (var rec in
            sessionTimesList.Where(rec => rec.Starttime.Year == SaturdayDate.Year && rec.Starttime.Month == SaturdayDate.Month && rec.Starttime.Day == SaturdayDate.Day))
        {
            SaturdaySessionTimesList.Add(rec.Starttime);
        }
        SundaySessionTimesList = new List<DateTime>();
        foreach (var rec in
            sessionTimesList.Where(rec => rec.Starttime.Year == SundayDate.Year && rec.Starttime.Month == SundayDate.Month && rec.Starttime.Day == SundayDate.Day))
        {
            SundaySessionTimesList.Add(rec.Starttime);
        }
    }

    /// <summary>
    /// Simply Update the times based on what day it is (or clear it if no day specified or all days)
    /// if radioButtonTimesListPressed is false, then set to first time of day
    /// </summary>
    private void SessionTimesRadioBoxListUpdate(bool radioButtonTimesListPressed)
    {
        RadioButtonListTimes.Items.Clear();

        // defaults to saturday, but we may want to pick this out of preference or current date at some point
        if (RadioButtonSatOrSundayList.SelectedValue.Equals("Saturday"))
        {
            if (SaturdaySessionTimesList != null)
            {
                foreach (var rec in SaturdaySessionTimesList)
                {
                    string timeFormatted = rec.ToShortTimeString();
                    RadioButtonListTimes.Items.Add(new ListItem(timeFormatted, rec.ToString()));
                }
                RadioButtonListTimes.Items.Add(new ListItem("All Times", SaturdayDate.ToString(), true));

                if (!radioButtonTimesListPressed)
                {
                    RadioButtonListTimes.Items[0].Selected = true;
                }
            }
        }
            
        if (RadioButtonSatOrSundayList.SelectedValue.Equals("Sunday"))
        {
            if (SundaySessionTimesList != null)
            {
                foreach (var rec in SundaySessionTimesList)
                {
                    string timeFormatted = rec.ToShortTimeString();
                    RadioButtonListTimes.Items.Add(new ListItem(timeFormatted, rec.ToString()));
                }
                RadioButtonListTimes.Items.Add(new ListItem("All Times", SundayDate.ToString(), true));
                if (!radioButtonTimesListPressed)
                {
                    RadioButtonListTimes.Items[0].Selected = true;
                }
            }
        }
    }



    protected void Page_PreRender(object sender, EventArgs e)
    {
        LabelSessionListToShow.Text = string.Empty;

        // on first time, show just saturday first session
        if (!IsPostBack && String.IsNullOrEmpty(SearchText))
        {
            if (RadioButtonSatOrSundayList.Visible)
            {
                ListItem listItem = RadioButtonListTimes.Items.FindByValue(SaturdayDate.ToString());
                if (listItem != null)
                {
                    RadioButtonListTimes.SelectedValue = listItem.Value;
                }
                SessionTimesRadioBoxListUpdate(false);  // puts times in radiobox for correct day.

                // set bottom label with sessions.
                LabelSessionListToShow.Text =
                    Utils.GetSessionsIdList(RadioButtonSatOrSundayList.SelectedValue, RadioButtonListTimes.SelectedValue);
            }
        }
        else if (IsPostBack && CheckBoxJustSessionsWithVideo.Checked)
        {
            RadioButtonSatOrSundayList.SelectedValue = "All";
            RadioButtonListTimes.Visible = false;
            LabelSessionListToShow.Text =
                Utils.GetSessionsJustWithVideoIdList();

            

        }
        else if (!IsPostBack && !String.IsNullOrEmpty(SearchText))
        {
            // if person pressed search (always a postback) then put all sessions on label at bottom for searching
            RadioButtonSatOrSundayList.SelectedValue = "All";
            RadioButtonListTimes.Visible = false;
            LabelSessionListToShow.Text =
               Utils.GetSessionsIdList("All", string.Empty);
        }
        else if (IsPostBack && SearchButtonPressed)
        {
            // if person pressed search (always a postback) then put all sessions on label at bottom for searching
            RadioButtonSatOrSundayList.SelectedValue = "All";
            RadioButtonListTimes.Visible = false;
            LabelSessionListToShow.Text =
               Utils.GetSessionsIdList("All", string.Empty);
        }
        else if (IsPostBack && !SearchButtonPressed)
        {
            LabelSessionListFromSearch.Text = string.Empty;
        }

        if (RadioButtonSatSunPressed || RadioButtonTimesListPressed) // could be first time also
        {
            RadioButtonListTimes.Visible = true;

            // load up times for each day
            if (RadioButtonSatSunPressed || RadioButtonTimesListPressed)
            {
                // make sure times are showing
                RadioButtonListTimes.Visible = true;
                if (!RadioButtonTimesListPressed)
                {
                    RadioButtonListTimes.SelectedIndex = RadioButtonListTimes.Items.Count - 1; // we know last item is all times
                }

                if (RadioButtonSatSunPressed)
                {
                    if (IsPostBack)
                    {
                        // don't do this except first time
                        SessionTimesRadioBoxListUpdate(RadioButtonTimesListPressed);
                    }
                    if (RadioButtonSatOrSundayList.SelectedValue.Equals("Saturday"))
                    {
                        ListItem listItem = RadioButtonListTimes.Items.FindByValue(SaturdayDate.ToString());
                        if (listItem != null)
                        {
                            RadioButtonListTimes.SelectedValue = listItem.Value;
                        }
                    }
                    else if (RadioButtonSatOrSundayList.SelectedValue.Equals("Sunday"))
                    {
                        ListItem listItem = RadioButtonListTimes.Items.FindByValue(SundayDate.ToString());
                        if (listItem != null)
                        {
                            RadioButtonListTimes.SelectedValue = listItem.Value;
                        }
                    }
                }

                if (SelectedSessionTimeSlot != null && RadioButtonListTimes != null && RadioButtonTimesListPressed)
                {
                    ListItem listItem = RadioButtonListTimes.Items.FindByValue(SelectedSessionTimeSlot.ToString());
                    if (listItem != null)
                    {
                        RadioButtonListTimes.SelectedValue = listItem.Value;
                    }
                }

                LabelSessionListToShow.Text =
                    Utils.GetSessionsIdList(RadioButtonSatOrSundayList.SelectedValue, RadioButtonListTimes.SelectedValue);

            }
        }

        if (String.IsNullOrEmpty(LabelSessionListFromSearch.Text))
        {
            LabelSessionListFinalToShow.Text = LabelSessionListToShow.Text;
        }
        else
        {
            // Need to create intersection of lists LabelSessionListToShow and LabelSessionListFromSearch
            List<int> sessionListToShow = String.IsNullOrEmpty(LabelSessionListToShow.Text)
                                              ? new List<int>()
                                              : LabelSessionListToShow.Text.Split(',').ToList().Select(
                                                  rec => Convert.ToInt32(rec)).ToList();

            List<int> sessionListFromSearchInt = LabelSessionListFromSearch.Text.Split(',').ToList().Select(rec => Convert.ToInt32(rec)).ToList();

            List<int> sessionListFinal = sessionListToShow.Intersect(sessionListFromSearchInt).ToList();

            // if no intersection, show no sessions
            if (sessionListFinal.Count == 0)
            {
                sessionListFinal.Add(-999);
            }

            var stringBuilder1 = new StringBuilder();
            for (var index = 0; index < sessionListFinal.Count; index++)
            {
                stringBuilder1.Append(sessionListFinal[index]);
                if (index < sessionListFinal.Count - 1)
                {
                    stringBuilder1.Append(",");
                }
            }
            LabelSessionListFinalToShow.Text = stringBuilder1.ToString();
        }

       
        // if all session showing show title bar with sort stuff.  otherwise, always sort by title
        // todo: BUG WITH GOING FROM SPEAKERS TO SESSIONS PAGE
        //if (RadioButtonSatOrSundayList.SelectedValue.Equals("All"))
        //{
        //    IdSortBy.Visible = true;
        //}
        //else
        //{
        //    IdSortBy.Visible = false;
        //    Repeater1.DataSourceID = "ObjectDataSourceBySessionTitle";
        //}
        Repeater1.DataBind();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        RadioButtonSatSunPressed = false;
        RadioButtonTimesListPressed = false;
        SearchButtonPressed = false;

        //_sessionJobListingDictionary = Utils.GetSessionsJobListingDictionary(Utils.GetCurrentCodeCampYear(),true);

        _sessionSponsorListDictionary = Utils.GetSessionJobDictionary(Utils.GetCurrentCodeCampYear(), true);
 //       _sessionListPbWikiShow = Utils.GetSessionListPbWikiShow(Utils.GetCurrentCodeCampYear(), true);

        // these are cached
        _boxFolderIdDictionary = Utils.GetBoxFolderIdsBySessionDict(Utils.GetCurrentCodeCampYear());
        _materialsDictionary = Utils.GetMaterialsUrlBySessionDict(Utils.GetCurrentCodeCampYear());

        if (!IsPostBack)
        {
          



             if ((ConfigurationManager.AppSettings["ShowVideoOnSessionIfExists"] != null &&
                 ConfigurationManager.AppSettings["ShowVideoOnSessionIfExists"].ToLower().Equals("true")) ||
                Utils.CheckUserIsAdmin() || Utils.CheckUserIsVideoEditor())
             {
                 CheckBoxJustSessionsWithVideo.Visible = true;
             }

            RegisteredForCurrentYear = Utils.IsRegisteredForCurrentCodeCampYear(Context.User.Identity.Name,
                                                                                Utils.CurrentCodeCampYear);
        }

        if (Request.QueryString["Search"] != null && !IsPostBack)
        {
            SearchText = Request.QueryString["Search"];
            SearchButtonPressed = true;
            TextBox1.Text = SearchText;
            SearchButtonCode(SearchText);
        }
        else
        {
            SearchText = string.Empty;
        }



        if (ConfigurationManager.AppSettings["SessionsPageUsingExtJS"] != null &&
            ConfigurationManager.AppSettings["SessionsPageUsingExtJS"].ToLower().Equals("true"))
        {
            using (var codeCampDataContext = new CodeCampDataContext())
            {
                List<int> sessionIds = GetListOfSessionsBasedOnQueryParams(codeCampDataContext);
                ShowAllSessionsWithBasicHtml(codeCampDataContext, sessionIds);
            }
        }
        else
        {
            if (Utils.CheckUserIsScheduler() || Utils.CheckUserIsAdmin())
            {
                CheckBoxShowOnlyAssigned.Visible = true;
            }
            else
            {
                CheckBoxShowOnlyAssigned.Visible = false;
            }

            // always get this because if person logs out, it goes away
            _listSessionAttendees = new SessionAttendeeODS().GetByUsername(Context.User.Identity.Name);

            

            //FormsAuthentication.SetAuthCookie("pkellner",true);
            if (!IsPostBack)
            {
               

                //RadioButtonSatSunPressed = true; // on first time, force prerender event to fire
                //RadioButtonTimesListPressed = true;

                if (ConfigurationManager.AppSettings["ShowAgendaOnSchedule"] != null &&
                    ConfigurationManager.AppSettings["ShowAgendaOnSchedule"].ToLower().Equals("true"))
                {
                    RadioButtonSatOrSundayList.Visible = true; // start out showing sat/sun choice
                    RadioButtonListTimes.Visible = true;
                }
                else
                {
                    RadioButtonSatOrSundayList.Visible = false; 
                    RadioButtonListTimes.Visible = false;
                }

                if (Request.QueryString["track"] != null)
                {
                    IdSortBy.Visible = false;
                    IdTrackDescription.Visible = true;
                    // Get Track Description
                    var recs = TrackManager.I.Get(new TrackQuery { Id = Convert.ToInt32(Request.QueryString["track"]) });
                    if (recs != null && recs.Count > 0)
                    {
                        LiteralTitle.Text = String.Format("{0} Track Sessions", recs[0].Named);
                        LiteralTrackDescription.Text = recs[0].Description;
                    }
                }
                else
                {
                    if (ConfigurationManager.AppSettings["SessionsPageUsingExtJS"] == null ||
                        !ConfigurationManager.AppSettings["SessionsPageUsingExtJS"].ToLower().Equals("true"))
                    {
                        IdSortBy.Visible = true;
                        IdTrackDescription.Visible = false;
                        LiteralTitle.Text = "Sessions";
                    }
                }


                // Get List of Tracks sorted by 
                const string cacheSessionsTrackSessionsResults = "CacheSessionsTrackSessionResults";
                TrackSessionResults = (List<TrackSessionResult>)HttpContext.Current.Cache[cacheSessionsTrackSessionsResults];
                if (TrackSessionResults == null)
                {
                    TrackSessionResults = (from mydata in TrackSessionManager.I.GetAll()
                                           select mydata).ToList();
                    HttpContext.Current.Cache.Insert(cacheSessionsTrackSessionsResults,
                        TrackSessionResults, null, DateTime.Now.Add(new TimeSpan(0, 0, Utils.RetrieveSecondsForSessionCacheTimeout())), TimeSpan.Zero);
                }

                // Get List of Tracks sorted by Name
                string cacheSessionsTrackResults = String.Format("CacheSessionsTrackResults-{0}", Utils.GetCurrentCodeCampYear());
                TrackResults = (List<TrackResult>)HttpContext.Current.Cache[cacheSessionsTrackResults];
                if (TrackResults == null)
                {
                    TrackResults =
                    (from mydata in TrackManager.I.Get(new TrackQuery()
                    {
                        CodeCampYearId =
                            Utils.GetCurrentCodeCampYear(),
                        Visible = true

                    })
                    where mydata.Named != null
                     orderby mydata.Named.ToLower()
                     select mydata).ToList();

                    HttpContext.Current.Cache.Insert(cacheSessionsTrackResults, TrackResults,
                                                     null,
                                                     DateTime.Now.Add(new TimeSpan(0, 0,
                                                                                   Utils.
                                                                                       RetrieveSecondsForSessionCacheTimeout
                                                                                       ())),
                                                     TimeSpan.Zero);
                }

                var trackNamesByIdDictionary =
                    TrackResults.ToDictionary(trackResult => trackResult.Id, trackResult => trackResult.Named);

                TrackNameAssignedToSession = new Dictionary<int, string>();
                foreach (var trackSessionResult in TrackSessionResults)
                {
                    // this takes care of Visible problem from track above
                    if (trackNamesByIdDictionary.ContainsKey(trackSessionResult.TrackId))
                    {
                        TrackNameAssignedToSession.Add(trackSessionResult.SessionId,
                                                       trackNamesByIdDictionary[trackSessionResult.TrackId]);
                    }
                }

                //<asp:ListItem Value="Time">Session Time</asp:ListItem>
                //             <asp:ListItem Value="Speaker">Session Speaker</asp:ListItem>
                //             <asp:ListItem Value="Title">Session Title</asp:ListItem>
                //             <asp:ListItem Value="Submitted">Session Submission</asp:ListItem>

                DropDownListSessionSortBy.Items.Add(new ListItem("Session Speaker", "Speaker"));
                DropDownListSessionSortBy.Items.Add(new ListItem("Session Title", "Title"));
                DropDownListSessionSortBy.Items.Add(new ListItem("Session Submission", "Submitted"));
                DropDownListSessionSortBy.Items[2].Selected = true;

                bool showingTimes = false;
                if (ConfigurationManager.AppSettings["ShowAgendaOnSchedule"].ToLower().Equals("true") ||
                    Utils.CheckUserIsScheduler() ||
                    Utils.CheckUserIsAdmin())
                {
                    DropDownListSessionSortBy.Items.Add(new ListItem("Session Time", "Time"));
                    showingTimes = true;
                }

                // if sortby is title, then need to force it here because of tags
                if (Request.QueryString["sortby"] != null && Request.QueryString["sortby"].Equals("title"))
                {
                    DropDownListSessionSortBy.SelectedIndex = DropDownListSessionSortBy.Items.IndexOf
                        (DropDownListSessionSortBy.Items.FindByValue("Title"));
                }

                if (Context.User.Identity.IsAuthenticated)
                {
                    if (showingTimes)
                    {
                        DropDownListSessionSortBy.SelectedIndex = DropDownListSessionSortBy.Items.IndexOf
                            (DropDownListSessionSortBy.Items.FindByValue("Time"));
                    }

                    CheckBoxShowOnlyAssigned.Checked =
                        Utils.GetProfileDataBool(Utils.ProfileDataShowOnlyAssignedSessions, false);


                }
                else
                {
                    // If not authenticated, make default sort title
                    // make default title
                    DropDownListSessionSortBy.SelectedIndex = DropDownListSessionSortBy.Items.IndexOf
                        (DropDownListSessionSortBy.Items.FindByValue("Title"));
                }

                if (Request.QueryString["ForceSortBySessionTime"] != null &&
                    Request.QueryString["ForceSortBySessionTime"].ToLower().Equals("true"))
                {
                    Repeater1.DataSourceID = "ObjectDataSourceSessions";
                    DropDownListSessionSortBy.SelectedIndex =
                        DropDownListSessionSortBy.Items.IndexOf
                            (DropDownListSessionSortBy.Items.FindByValue("Time"));
                }
            }

            // need this to be default 2 because if choose 2, it will be no postback
            // and the default needs to be by title

            Repeater1.DataSourceID = "ObjectDataSourceBySessionTitle";

            // get checkbox states
            if (Context.User.Identity.IsAuthenticated)
            {
                if (CheckBoxShowOnlyAssigned.Checked)
                {
                    Repeater1.DataSourceID = "ObjectDataSourceSessionsOnlyAssigned";
                }
                else
                {
                    Repeater1.DataSourceID = "ObjectDataSourceSessions";
                }

                // Need to set the correct object for retrieveal
                if (DropDownListSessionSortBy.SelectedValue.Equals("Time"))
                {
                    //if (CheckBoxShowOnlyAssigned.Checked)
                    //{
                    //    Repeater1.DataSourceID = "ObjectDataSourceSessionsOnlyAssigned";
                    //}
                    //else
                    //{
                        Repeater1.DataSourceID = "ObjectDataSourceSessions";
                    //}
                }
                else if (DropDownListSessionSortBy.SelectedValue.Equals("Speaker"))
                {
                    Repeater1.DataSourceID = "ObjectDataSourceBySessionPresenter";
                }
                else if (DropDownListSessionSortBy.SelectedValue.Equals("Title"))
                {
                    Repeater1.DataSourceID = "ObjectDataSourceBySessionTitle";
                }
                else if (DropDownListSessionSortBy.SelectedValue.Equals("Submitted"))
                {
                    Repeater1.DataSourceID = "ObjectDataSourceSessionSubmitted";
                    //ObjectDataSourceSessionSubmitted.FilterExpression = "Id = 540";
                }
            }

            ObjectDataSourceSessionSubmitted.FilterExpression = "";

            // get from Cache on load se we can use in page
            DropDownListLevels.DataBind();

            SessionLevelsDictionary = new Dictionary<int, string>(DropDownListLevels.Items.Count);
            foreach (ListItem listItem in DropDownListLevels.Items)
            {
                SessionLevelsDictionary.Add(Convert.ToInt32(listItem.Value), listItem.Text);
            }

            // else if's protect the "id" below
            if (Request.QueryString["by"] != null && Request.QueryString["tag"] != null)
            {
                RadioButtonSatOrSundayList.Visible = false; // hide sat/sun choice if not doing full list
                RadioButtonListTimes.Visible = false;
                Repeater1.DataSourceID = "ObjectDataSourceByTag";
            }
            else if (Request.QueryString["track"] != null)
            {
                RadioButtonSatOrSundayList.Visible = false; // hide sat/sun choice if not doing full list
                RadioButtonListTimes.Visible = false;
                Repeater1.DataSourceID = "ObjectDataSourceByTrack";
            }
            else if (Request.QueryString["sessionid"] != null)
            {

                RadioButtonSatOrSundayList.Visible = false; // hide sat/sun choice if not doing full list
                RadioButtonListTimes.Visible = false;

                int outInt = 0;
                string str = Request.QueryString["sessionid"];
                bool good = Int32.TryParse(str, out outInt);
                if (!good)
                {
                    Response.Redirect("~/Sessions.aspx");
                }

                Repeater1.DataSourceID = "ObjectDataSourceBySessionIdOnly";
            }
            else if (Request.QueryString["id"] != null)
            {

                RadioButtonSatOrSundayList.Visible = false; // hide sat/sun choice if not doing full list
                RadioButtonListTimes.Visible = false;

                int outInt = 0;
                string str = Request.QueryString["id"];
                bool good = Int32.TryParse(str, out outInt);
                if (!good)
                {
                    Response.Redirect("~/Sessions.aspx");
                }

                // if OnlyOne is in the URL as a request param, then don't get all sessions, just one.
                Repeater1.DataSourceID = Request.QueryString["OnlyOne"] == null
                                             ? "ObjectDataSourceGetSessionsBySession"
                                             : "ObjectDataSourceBySessionId";
            }
            else if (Request.QueryString["PKID"] != null)
            {
                RadioButtonSatOrSundayList.Visible = false; // hide sat/sun choice if not doing full list
                RadioButtonListTimes.Visible = false;

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
                            Response.Redirect("~/Sessions.aspx", true);
                        }
                    }
                }
            }
            else if (Request.QueryString["AttendeeId"] != null)
            {
                RadioButtonSatOrSundayList.Visible = false; // hide sat/sun choice if not doing full list
                RadioButtonListTimes.Visible = false;

                Repeater1.DataSourceID = "ObjectDataSourceGetSessionsByAttendeeId";
            }
        }
    }

    /// <summary>
    /// output html 
    /// </summary>
    /// <param name="sessionIds"></param>
    private void ShowAllSessionsWithBasicHtml(CodeCampDataContext codeCampDataContext, List<int> sessionIds)
    {
        var sb = new StringBuilder();


        var sessions = from data in codeCampDataContext.Sessions where sessionIds.Contains(data.Id) select data;

        List<int> sessionIdList = (from data in sessions select data.Id).ToList();

        // grab all tags for speed
        List<Tags> allTags = (codeCampDataContext.Tags).ToList();
        List<CodeCampSV.SessionTags> allSessionTags = (from mydata in codeCampDataContext.SessionTags
                                                       where sessionIdList.Contains(mydata.SessionId.Value)
                                                       select mydata).ToList();


        //int cnt = sessions.Count();

        sb.Append("<div class=\"pad\">");
        foreach (var session in sessions)
        {
            sb.AppendLine(String.Format("<h2 class=\"CC_SessionTitle\">{0}</h2>", session.Title));
            sb.AppendLine(String.Format("<h4 class=\"CC_SessionStartTime\">{0}</h2>",
                                        GetSessionTimeFromSessionTimeId(session.SessionTimesId)));
            sb.AppendLine(String.Format("<h4 class=\"CC_SessionRoom\">{0}</h2>",
                                        GetRoomNameFromLectureRoomId(session.LectureRoomsId)));
            sb.AppendLine(String.Format("<h4 class=\"CC_SessionTrackName\">{0}</h2>",
                                        GetSessionTrackNameFromId(session.Id)));

            // <a  href="https://codecamp.pbworks.com/newpage.php?show_suggestions&page_name=BeyondtheRelayRoutersandQueuesintheNETServiceBus-2009">Wiki Here</a>
            sb.AppendLine(String.Format(
                "<h4 class=\"CC_WikiAddress\"><a  href=\"ViewWikiPage?SessionId={0}\">Wiki Here</a></h2>",
                session.Id));

            sb.AppendLine(String.Format("<br/><h3><a  href=\"Speakers.aspx?id={0}\">Presenter(s)</a></h3>",session.Id));
            sb.AppendLine("<ul class=\"CC_SessionPresenters\">");
            foreach (var attendeeSpeaker in GetSessionPresenters(codeCampDataContext, session.Id))
            {
                // <a  href="Speakers.aspx?id=157">Juval Lowy</a></
                sb.AppendLine(String.Format("  <li><a  href=\"Speakers.aspx?AttendeeId={2}\">{0} {1}</a></li>",
                                            attendeeSpeaker.UserFirstName, attendeeSpeaker.UserLastName,
                                            attendeeSpeaker.Id));
            }
            sb.AppendLine("</ul>");

            sb.AppendLine("<br/><h3>Session Tags</h3");
            sb.AppendLine("<ul class=\"CC_Tags\">");
            int iCnt = 0;
            foreach (var tag in GetSessionTags(allSessionTags, allTags, session.Id))
            {
                iCnt++;
                // <a  href="Sessions.aspx?sortby=title&amp;by=category&amp;tag=44">Architecture</a>
                sb.AppendLine(
                    String.Format(
                        "  <li><a  href=\"Sessions.aspx?sortby=title&amp;by=category&amp;tag={0}\">{1}</a></li>",
                        tag.Id, tag.TagName));
                if (iCnt > 7)
                {
                    break;
                }
            }
            sb.AppendLine("</ul>");

            sb.AppendLine("<br/><h3>Session Details</h3");
            sb.AppendLine(String.Format("<h4 class=\"CC_SessionDetails\">{0}</h2>",
                                        CheckForValidHTML(session.Description))); // CheckForValidHTML

            sb.AppendLine("<br/><h3>Session Interest Levels</h3");
            sb.AppendLine(String.Format("<span class=\"CC_InterestLevelRadioButtons\">"));
            sb.AppendLine(
                String.Format(
                    "  <input class=\"CC_InterestLevelNotInterested\" NAME=\"SILRB{0}\" type=\"radio\">{1}</input>",
                    session.Id, "Not Interested"));
            sb.AppendLine(
                String.Format(
                    "  <input class=\"CC_InterestLevelInterested\" NAME=\"SILRB{0}\" type=\"radio\">{1}</input> ",
                    session.Id, GetInterestedText(session.Id)));
            sb.AppendLine(
                String.Format(
                    "  <input class=\"CC_InterestLevelPlanToAttend\" NAME=\"SILRB{0}\" type=\"radio\">{1}</input>",
                    session.Id, GetWillAttendText(session.Id)));
            sb.AppendLine(String.Format("</span>"));

            sb.AppendLine("<hr/><br/>");
            //break;
        }
        sb.AppendLine("</div>");


        HtmlForSessionsId.Text = sb.ToString();
    }

    //private string GetWikiLinkFromSessionTitle(string firstName,string lastName,string sessionTitle,string sessionURL,string speakerURL)
    //{
    //    string url = CallToGenerateNewPage(sessionTitle, string.Format("{0} {1}", firstName, lastName),
    //                                                   sessionTitle,
    //                                                   sessionURL, speakerURL);
    //    return url;
    //}

    private static IEnumerable<Tags> GetSessionTags(List<CodeCampSV.SessionTags> allSessionTags, List<CodeCampSV.Tags> allTags, int sessionId)
    {
        List<int> sessionTagIds = (from data in allSessionTags
                                  where data.SessionId == sessionId
                                  select data.TagId).ToList();

        List<Tags> tags = (from data in allTags
                           where sessionTagIds.Contains(data.Id)
                           select data).ToList();

        return tags;
    }

    private static IEnumerable<Attendees> GetSessionPresenters(CodeCampDataContext codeCampDataContext, int sessionId)
    {
        // make sure primary presenter is first
        List<int> attendeeIds = (from data in codeCampDataContext.SessionPresenter
                                                    where data.SessionId == sessionId
                                                    orderby data.Id
                                                    select data.AttendeeId).ToList();
        // going through one at a time to make sure list stays sorted the way we want it
        // rather than doing a .Contains type statement.  Most sessions are one speaker so it's
        // not that important.
        var attendeesList = new List<Attendees>();
        foreach (int attendeeId in attendeeIds)
        {
            int id = attendeeId;
            Attendees attendeeOne = (from data in codeCampDataContext.Attendees
                                     where data.Id == id
                                     select data).FirstOrDefault();
            attendeesList.Add(attendeeOne);
        }

        return attendeesList;
    }

   

    private string GetSessionTimeFromSessionTimeId(int? nullable)
    {
        return "9:45AM";
    }

    private string GetSessionTrackNameFromId(int p)
    {
        return "Java Track";
    }

    private string GetRoomNameFromLectureRoomId(int? nullable)
    {
        return "Hearthside Lounge";
    }



    private List<int> GetListOfSessionsBasedOnQueryParams(CodeCampDataContext codeCampDataContext)
    {
        var sessionIds = new List<int>();

        // need to get all query params and do filter here
        var sessionsOds = new SessionsODS();

        int outInt = 0;
        string str = Request.QueryString["id"];
        bool foundId = Int32.TryParse(str, out outInt);
        if (foundId)
        {
            sessionIds = new List<int>() {outInt};
        }
        else
        {
            //var sessions =
            //    codeCampDataContext.Sessions.Where(a => a.CodeCampYearId == Utils.CurrentCodeCampYear).ToList();

            sessionIds =
                codeCampDataContext.Sessions.Where(a => a.CodeCampYearId == Utils.CurrentCodeCampYear).Select(a => a.Id)
                    .ToList();
        }

        return sessionIds;
    }

    protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
       

        if (e.Item.ItemType == ListItemType.Item ||
            e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var divControl = e.Item.FindControl("SessionPageBreakId");

            // make better later, but for now, admin gets page break.
            divControl.Visible = Utils.CheckUserIsAdmin();

            int sessionID = ((SessionsODS.DataObjectSessions)e.Item.DataItem).Id;

            var dropDownListTracks = (DropDownList)e.Item.FindControl("DropDownListTracks");
            dropDownListTracks.Items.Add(new ListItem("--Choose Track", "-1"));
            dropDownListTracks.Items.Add(new ListItem("--Remove From Track", sessionID.ToString()));
            foreach (var trackResult in TrackResults)
            {
                var listItem = new ListItem(trackResult.Named, trackResult.Id.ToString(), true);
                dropDownListTracks.Items.Add(listItem);
            }

            // select currently assigned track if any
            List<int> tracks =
                (from myData in TrackSessionResults where myData.SessionId == sessionID select myData.TrackId).ToList();
            if (tracks != null && tracks.Count > 0)
            {
                int trackId = tracks[0];
                int cntx = 0;
                foreach (ListItem ddlINdex in dropDownListTracks.Items)
                {
                    if (ddlINdex.Value.Equals(trackId.ToString()))
                    {
                        dropDownListTracks.SelectedIndex = cntx;
                    }
                    cntx++;
                }
            }




            var categoryRepeater = (Repeater) e.Item.FindControl("RepeaterTagNamesBySession");
            //DataRowView drv = (DataRowView)e.Item.DataItem;
          
            //int sessionID =  (int)drv.Row.ItemArray[0];

            // SELECT SessionTags.tagid, Tags.TagName
            // FROM  SessionTags INNER JOIN
            //               Tags ON SessionTags.tagid = Tags.id
            // WHERE (SessionTags.sessionId = @sessionId)


            //ObjectDataSource ods = new ObjectDataSource("SessionTagsByNameTableAdapters.DataTableSessionTagsByNameTableAdapter",
            //    "GetData");

            //// $$$ THIS SHOULD REALLY BE A CACHE THAT WE CAN CLEAR.
            //ods.CacheDuration = 15;
            //ods.EnableCaching = true;
            //ods.SelectParameters.Add("sessionID", sessionID.ToString());

            //categoryRepeater.DataSource = ods;
            //categoryRepeater.DataBind();

            var ods = new ObjectDataSource 
            { 
                DataObjectTypeName = "DataObjectTags", 
                TypeName = "CodeCampSV.SessionsODS", 
                SelectMethod = "GetTagsBySession",
                CacheDuration = Utils.RetrieveSecondsForSessionCacheTimeout(),
                EnableCaching = true
            };

            ods.SelectParameters.Add("sessionID", sessionID.ToString());
            ods.SelectParameters.Add("maxReturn", Utils.MaxSessionTagsToShow.ToString());

            categoryRepeater.DataSource = ods;
            categoryRepeater.DataBind();

            //int cnt = categoryRepeater.Items.Count;




            string userName = Context.User.Identity.IsAuthenticated ? Context.User.Identity.Name : "NotLoggedIn";
            SessionsODS.DataObjectSessions frame = e.Item.DataItem as SessionsODS.DataObjectSessions;


            

            List<string> labels = new List<string> {"Not Interested", "Interested", "Plan On Attending"};

            int cnt = 0;
            foreach (string rbName in labels)
            {
                RadioButton rbClick = (RadioButton)e.Item.FindControl(string.Format("rbClick{0}", (cnt+1)));
                string str = string.Format("javascript:return MakeExclusiveCheck('{0}',{1},'{2}','{3}',{4})", rbClick.ClientID, frame.Id, userName, labels[cnt],cnt);
                rbClick.Attributes.Add("onclick", str);
                //rbClick.Attributes.Add("time", frame.Sessiontimesid.ToString());

                rbClick.CssClass = string.Format("SessionTime{0}", frame.Sessiontimesid);

                cnt++;                
            }
        }
    }

    protected string GetAttendeeNameByUsername(object userNameObject)
    {
        string userName = Convert.ToString(userNameObject);
        string retString = string.Empty;
        if (!String.IsNullOrEmpty(userName))
        {
            retString = Utils.GetAttendeeNameByUsername((string) userNameObject,Utils.CheckUserIsAdmin());
        }
        return retString;
    }

    protected string GetAttendeePKIDByUsername(object userNameObject)
    {
        string userName = Convert.ToString(userNameObject);
        string retString = string.Empty;
        if (!String.IsNullOrEmpty(userName))
        {
            retString = Utils.GetAttendeePKIDByUsername((string) userNameObject);
        }
        return retString;
    }

    /// <summary>
    /// return 
    /// <a id="ctl00_ctl00_ctl00_blankContent_parentContent_MainContent_Repeater1_ctl01_HyperLink1" href="Speakers.aspx?id=345">Peter Kellner(SA:)</a>
    /// Build a horiz list of speakers links with speakerid's
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    protected string GetAllSpeakersHtml(int sessionId)
    {
        return Utils.GetAllSpeakersHtml(sessionId);
    }

    

   

    protected string GetAttendeeAnySessionIdByUsername(string username)
    {
        int sessionId = -1;
        var sessionsODS = new SessionsODS();
        List<SessionsODS.DataObjectSessions> liSessions = sessionsODS.GetAllSessionsByUsername(username);
        if (liSessions.Count > 0)
        {
            sessionId = liSessions[0].Id;
        }

        return sessionId.ToString();
    }


    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandArgument.ToString().StartsWith("Delete"))
        {
            int SessionID = Int32.Parse(e.CommandArgument.ToString().Substring(7));

            using (var scope = new TransactionScope())
            {
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString);
                sqlConnection.Open();

                string sqlDelete =
                    "DELETE FROM SessionTags WHERE sessionId=@sessionId;" +
                    "DELETE FROM SessionAttendee WHERE sessions_id=@sessionId;" +
                    "DELETE FROM TrackSession WHERE SessionId = @sessionId;" +
                    "DELETE FROM SessionEvals WHERE sessionId=@sessionId;" +
                    "DELETE FROM SessionPresenter WHERE SessionId=@sessionId;" +
                    "DELETE FROM Sessions WHERE ID=@sessionId;";

                try
                {
                    var sqlCommand = new SqlCommand(sqlDelete, sqlConnection);
                    sqlCommand.Parameters.Add("@sessionId", SqlDbType.Int).Value = SessionID;
                    int rowsUpdated = sqlCommand.ExecuteNonQuery();

                    sqlConnection.Close();
                    sqlConnection.Dispose();
                }
                catch (Exception ee)
                {
                    throw new ApplicationException("Delete Failed On Session " + ee);
                }
                scope.Complete();
            }

            Utils.CacheClear("sessions");
            Response.Redirect("~/Sessions.aspx", true);
        }
        else if (e.CommandArgument.ToString().StartsWith("MaterailsUrl"))
        {
            int sessionId = Int32.Parse(e.CommandArgument.ToString().Substring(13));

            //var sessionRec = SessionsManager.I.Get(new SessionsQuery() {Id = sessionId}).FirstOrDefault();

            //if (sessionRec != null)
            //{
            if (_materialsDictionary.ContainsKey(sessionId))
            {
                string redirect = _materialsDictionary[sessionId];
                Response.Redirect(redirect, true);
            }


            if (_boxFolderIdDictionary.ContainsKey(sessionId))
            {
                var sessionRec = SessionsManager.I.Get(new SessionsQuery() { Id = sessionId }).FirstOrDefault();
                if (sessionRec != null)
                {
                    Response.Redirect(sessionRec.BoxFolderPublicUrl, true);
                }
            }
            //}
        }
        else if (e.CommandArgument.ToString().StartsWith("PostWiki"))
        {
            //int sessionId = Int32.Parse(e.CommandArgument.ToString().Substring(9));

            //string firstName;
            //string lastName;
            //string description;
            //string sessionURL;
            //string speakerURL;
            //string sessionTitle;
            //string speakerBio = string.Empty;
            //string speakerZipCode = string.Empty;
            //string speakerPersonalUrl = string.Empty;
            //string speakerPictureUrl = string.Empty;
            //DateTime sessionStartTime = DateTime.MinValue;
            //bool success = Utils.GetSessionInfo(sessionId, out firstName, out lastName, out description, out sessionURL,
            //                                    out speakerURL, out sessionTitle,
            //                                    out speakerBio, out speakerPictureUrl,
            //                                    out speakerZipCode, out speakerPersonalUrl, out sessionStartTime);
            //if (success)
            //{
            //    string newTitle = CallToGenerateNewPage(sessionTitle, string.Format("{0} {1}", firstName, lastName),
            //                                            description,
            //                                            sessionURL, speakerURL);

            //    if (!string.IsNullOrEmpty(newTitle))
            //    {
            //        string url = "http://codecamp.pbwiki.com/" + newTitle;
            //        Response.Redirect(url);
            //    }
            //}
        }
        else if (e.CommandArgument.ToString().StartsWith("Edit"))
        {
            int sessionId = Int32.Parse(e.CommandArgument.ToString().Substring(5));
            Response.Redirect("~/SessionsEdit.aspx?id=" + sessionId);
        }
        else if (e.CommandArgument.ToString().StartsWith("Evaluations"))
        {
            int sessionId = Int32.Parse(e.CommandArgument.ToString().Substring(12));
            Response.Redirect("~/SessionsEvalReview.aspx?id=" + sessionId);
        }
        else if (e.CommandArgument.ToString().StartsWith("EmailSpeaker"))
        {
            int sessionId = Int32.Parse(e.CommandArgument.ToString().Substring(13));
            Response.Redirect("~/SpeakerMailTo.aspx?SessionId=" + sessionId);
        }
        else if (e.CommandArgument.ToString().StartsWith("AssignVideo"))
        {
            // int startPos = e.CommandArgument.ToString().Length - 1;
            int sessionId = Int32.Parse(e.CommandArgument.ToString().Substring(12));
            Response.Redirect("~/SessionAssignVideo.aspx?SessionId=" + sessionId);
        }


        Repeater1.DataBind();
        //string str = (string) e.CommandArgument;
    }


    protected bool GetEmailSpeakerVisible(string userNameOfSession)
    {
        bool showEmailButton = Utils.GetAttendeeVistaOnlyByUsername(userNameOfSession);

        // replace 1 == 2 with check to see if speaker has given permission to have emails sent
        bool showEmailSpeaker = false;

        if (CheckBoxHideDescriptions.Checked)
        {
            showEmailSpeaker = false;
        }
        else if (!Context.User.Identity.IsAuthenticated)
        {
            showEmailSpeaker = false; // never show if user not logged in
        }
        else if (Utils.CheckUserIsAdmin())
        {
            showEmailSpeaker = true; // always show if admin
        }
        else if (Utils.GetAttendeeVistaOnlyByUsername(userNameOfSession))
        {
            showEmailSpeaker = true; // must be authenticated, not admin, so check and see if user wants session shown.
        }


        //if ((Context.User.Identity.IsAuthenticated &&
        //     Context.User.Identity.Name.ToLower().Equals(userNameOfSession.ToLower())) || Utils.CheckUserIsAdmin()
        //    || showEmailButton)
        //{
        //    showEmailSpeaker = true;
        //}
        ////return false;
        return showEmailSpeaker;
    }

    protected bool GetDeleteButtonVisible(string userNameOfSession)
    {
        bool showDeleteButton = false;
        if (Utils.CheckUserIsAdmin())
        {
            showDeleteButton = true;
        }
        else if (CheckBoxHideDescriptions.Checked)
        {
            showDeleteButton = false;
        }
        else if (Context.User.Identity.IsAuthenticated &&
             Context.User.Identity.Name.ToLower().Equals(userNameOfSession.ToLower()) )
        {
            if (ConfigurationManager.AppSettings["ShowSessionDeleteToSpeaker"] != null &&
                ConfigurationManager.AppSettings["ShowSessionDeleteToSpeaker"].ToLower().Equals("true"))
            {
                showDeleteButton = true;
            }
            else
            {
                showDeleteButton = false;
            }
        }
        //return false;
        return showDeleteButton;
    }

    protected bool GetEditButtonVisible(string userNameOfSession)
    {
        bool showEditButton = false;
        if (Utils.CheckUserIsAdmin())
        {
            showEditButton = true;
        }
        else if (CheckBoxHideDescriptions.Checked)
        {
            showEditButton = false;
        }
        else if ((Context.User.Identity.IsAuthenticated &&
             Context.User.Identity.Name.ToLower().Equals(userNameOfSession.ToLower())) || Utils.CheckUserIsAdmin())
        {
            showEditButton = true;
        }
        //return false;
        return showEditButton;
    }


    protected bool GetEmailPlannedAndInterestedVisible(string userNameOfSession)
    {
        bool EmailPlannedAndInterested = false;
        if (CheckBoxHideDescriptions.Checked)
        {
            EmailPlannedAndInterested = false;
        }
        else if ((Context.User.Identity.IsAuthenticated &&
             Context.User.Identity.Name.ToLower().Equals(userNameOfSession.ToLower())) && !Utils.CheckUserIsAdmin())
        {
            if (ConfigurationManager.AppSettings["ShowSpeakerToAttendeeEmail"].ToLower().Equals("true"))
            {
                EmailPlannedAndInterested = true;
            }
        }
        
        if (Utils.CheckUserIsAdmin())
        {
            EmailPlannedAndInterested = true;
        }

        return EmailPlannedAndInterested;
    }

    protected bool GetReviewEvaluationsButtonVisible(string userNameOfSession)
    {
        bool ReviewEvals = false;
        if (CheckBoxHideDescriptions.Checked)
        {
            ReviewEvals = false;
        }
        else if ((Context.User.Identity.IsAuthenticated &&
             Context.User.Identity.Name.ToLower().Equals(userNameOfSession.ToLower())) && !Utils.CheckUserIsAdmin())
        {
            if (ConfigurationManager.AppSettings["ShowEvalsToPresenters"].ToLower().Equals("true"))
            {
                ReviewEvals = true;
            }
        }
        
        if (Utils.CheckUserIsAdmin())
        {
            ReviewEvals = true;
        }

        return ReviewEvals;
    }

    protected string CallToGenerateNewPage(string sessionNameIn, string authorName, string sessionDescription,
                                           string sessionURL, string speakerURL)
    {
        //string ccYear = Utils.ConvertCodeCampYearToActualYear(Utils.CurrentCodeCampYear.ToString());

        //string responseReturn;
        //String sessionNameWithYear = sessionNameIn + "-" + ccYear;
                                     
        //string wikiPageName = Regex.Replace(sessionNameWithYear, @"[^\w@-]", "");

        //// this try loop is bad. need to reorganize error when page exists.
        //// should check first
        //try
        //{
        //    string newTitle = PBWikiAPI.AddPage("http://codecamp.pbwiki.com",
        //                                        ConfigurationManager.AppSettings["PBWikiAPIKey"],
        //                                        sessionNameWithYear,
        //                                        "AutoGenerator",
        //                                        string.Empty,
        //                                        SessionPageContent(sessionNameIn, authorName, sessionDescription,
        //                                                           sessionURL, speakerURL),
        //                                        out responseReturn
        //        );
        //}
        //catch (Exception e)
        //{
        //    String str = e.ToString();
        //}

        //return wikiPageName;
        return null;
    }



    //protected bool GetWikiMode()
    //{
    //    string wikiMode = ConfigurationManager.AppSettings["WikiMode"];
    //    if (wikiMode.ToLower().Equals("true") || Utils.CheckUserIsAdmin())
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    protected bool ShowWetPaintWiki(int sessionId)
    {
        bool showIt = false;
        string wikiMode = ConfigurationManager.AppSettings["WikiMode"];
        if (wikiMode.ToLower().Equals("true") || Utils.CheckUserIsAdmin())
        {
            showIt = Utils.ShowWetPaintWiki(sessionId);
        }
        return showIt;
    }

    

    private string SessionPageContent(string sessionName, string speakerName, string description, string sessionURL,
                                      string speakerURL)
    {
        String retString = "!" + sessionName + "\n**[" + speakerURL + " " + speakerName + "]**\n"
                           + "\n[" + sessionURL + " Sessions From this Presenter (Code Camp Website)]"
                           + "\n[http://www.siliconvalley-codecamp.com/Sessions.aspx All Sessions (Code Camp Website)]"
                           + "\n[http://www.siliconvalley-codecamp.com/ Home (Code Camp Website)]"
                           + "\n\n" + description
                           + "\n\nPlease add questions, links, comments, notes, downloads ... below.";

        return retString;
    }

    protected void RadioButtonListInterest_SelectedIndexChanged(object sender, EventArgs e)
    {
        var rblwa = (RadioButtonListWithArg) sender;
        int sessionId = Convert.ToInt32(((RadioButtonListWithArg) sender).CommandArg);

        string cacheName = String.Format("{0}_{1}", Utils.CacheSessionAttendeeBySessionId, sessionId);
        Cache.Remove(cacheName);


        var saODS = new SessionAttendeeODS();
        saODS.Update(sessionId, Context.User.Identity.Name, Convert.ToInt32(rblwa.SelectedValue));

        Repeater1.DataBind(); // gets counters to update
    }


    protected string GetRoomNumberFromRoomId(int roomId)
    {
        string retString = "Room: Unknown";
        if (Utils.CurrentCodeCampYear != Utils.GetCurrentCodeCampYear())
        {
            retString = "SESSION IN PAST";
        }
        else
        {

            if (ConfigurationManager.AppSettings["ShowRoomOnSchedule"].ToLower().Equals("true") ||
                Utils.CheckUserIsAdmin() ||
                Utils.CheckUserIsScheduler() ||
                (ConfigurationManager.AppSettings["ShowRoomOnScheduleForPresenter"].ToLower().Equals("true")) &&
                Utils.CheckUserIsPresenter())
            {
                var lrODS = new LectureRoomsODS();

                List<LectureRoomsODS.DataObjectLectureRooms> lrList = lrODS.GetAllLectureRooms();
                foreach (LectureRoomsODS.DataObjectLectureRooms room in lrList)
                {
                    if (room.Id == roomId)
                    {
                        retString = "Room: " + room.Number;
                        break;
                    }
                }
            }
        }
        return retString;
    }

    protected string GetAgendaDescriptionFromAgendaId(int agendaId)
    {
        string retString = "Agenda Not Made Yet";
        if (Utils.CurrentCodeCampYear != Utils.GetCurrentCodeCampYear())
        {
            retString = "Session Over";
        }
        else
        {

            if (Utils.CheckUserIsAdmin() || ConfigurationManager.AppSettings["ShowAgendaOnSchedule"].ToLower().Equals("true")
                || Utils.CheckUserIsScheduler() ||
                (ConfigurationManager.AppSettings["ShowRoomOnScheduleForPresenter"].ToLower().Equals("true")) &&
                Utils.CheckUserIsPresenter())
            {
                var stODS = new SessionTimesODS();

                List<SessionTimesODS.DataObjectSessionTimes> stList = stODS.GetAllSessionTimes();
                foreach (SessionTimesODS.DataObjectSessionTimes st in stList)
                {
                    if (st.Id == agendaId)
                    {
                        retString = st.Starttimefriendly;
                        break;
                    }
                }
            }
        }
        return retString;
    }

    protected bool GetHideSessionDescription()
    {
        if (CheckBoxHideDescriptions.Checked)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    protected void CheckBoxHideDescriptions_CheckedChanged(object sender, EventArgs e)
    {
        Repeater1.DataBind();

        if (Context.User.Identity.IsAuthenticated)
        {
            var pdODS = new ProfileDataODS();
            pdODS.UpdateProfileData(Context.User.Identity.Name,
                                    Utils.ProfileDataSessionsHideDescriptions,
                                    CheckBoxHideDescriptions.Checked ? "true" : "false");
        }
    }

    protected void CheckBoxShowOnlyAssigned_CheckedChanged(object sender, EventArgs e)
    {
        if (Context.User.Identity.IsAuthenticated)
        {
            var pdODS = new ProfileDataODS();
            pdODS.UpdateProfileData(Context.User.Identity.Name,
                                    Utils.ProfileDataShowOnlyAssignedSessions,
                                    CheckBoxShowOnlyAssigned.Checked ? "true" : "false");
        }

        if (CheckBoxShowOnlyAssigned.Checked)
        {
            Repeater1.DataSourceID = "ObjectDataSourceSessionsOnlyAssigned";
        }
        else
        {
            Repeater1.DataSourceID = "ObjectDataSourceSessions";
        }

        Repeater1.DataBind();
    }


    protected void DropDownListSessionSortBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Context.User.Identity.IsAuthenticated)
        {
            var pdODS = new ProfileDataODS();
            pdODS.UpdateProfileData(Context.User.Identity.Name,
                                    Utils.ProfileDataSessionsSortBy,
                                    DropDownListSessionSortBy.SelectedValue);
        }

        if (DropDownListSessionSortBy.SelectedValue.Equals("Time"))
        {
            if (CheckBoxShowOnlyAssigned.Checked)
            {
                Repeater1.DataSourceID = "ObjectDataSourceSessionsOnlyAssigned";
            }
            else
            {
                Repeater1.DataSourceID = "ObjectDataSourceSessions";
            }
        }
        else if (DropDownListSessionSortBy.SelectedValue.Equals("Speaker"))
        {
            Repeater1.DataSourceID = "ObjectDataSourceBySessionPresenter";
        }
        else if (DropDownListSessionSortBy.SelectedValue.Equals("Title"))
        {
            Repeater1.DataSourceID = "ObjectDataSourceBySessionTitle";
        }
        else if (DropDownListSessionSortBy.SelectedValue.Equals("Submitted"))
        {
            Repeater1.DataSourceID = "ObjectDataSourceSessionSubmitted";
        }
        Repeater1.DataBind();


        //<asp:DropDownList ID="DropDownListSessionSortBy" runat="server" OnSelectedIndexChanged="DropDownListSessionSortBy_SelectedIndexChanged" >
        //    <asp:ListItem Value="Title">Session Title</asp:ListItem>
        //    <asp:ListItem Value="Time">Session Time</asp:ListItem>
        //    <asp:ListItem Value="Speaker">Session Speaker</asp:ListItem>
        //</asp:DropDownList> 
    }


    protected bool IsPictureForSessionVisible()
    {
        bool showit = false;
        if (ConfigurationManager.AppSettings["ShowRoomPictureOnSchedule"].ToLower().Equals("true")
            || ConfigurationManager.AppSettings["ShowSessionPictureOnSchedule"].ToLower().Equals("true"))
        {
            showit = true;
        }
        return showit;
    }

    protected bool IsPictureHyperlinkForSessionVisible()
    {
        bool showit = !ConfigurationManager.AppSettings["ShowRoomPictureOnSchedule"].ToLower().Equals("true")
                 && ConfigurationManager.AppSettings["ShowSessionPictureOnSchedule"].ToLower().Equals("true")
                 && CheckBoxHideDescriptions.Checked == false;
        return showit;
    }


    protected string GetDisplayImageURLForSessionPicture(int sessionId, int roomId)
    {
        string urlString = string.Empty;
        if (ConfigurationManager.AppSettings["ShowRoomPictureOnSchedule"].ToLower().Equals("true"))
        {
            urlString = "roomid=" + roomId;
        }
        else if (ConfigurationManager.AppSettings["ShowSessionPictureOnSchedule"].ToLower().Equals("true"))
        {
            // ImageUrl='<%# "~/DisplayImage.ashx?pictureid=" + (string) GetSessionPictureIdForSession((int) Eval("id"))  %>'
            // See if any pictures for this session.  If there are then show default picture.  otherwise, show empty room
            int defaultPictureId = Utils.GetDefaultPictureIdFromSession(sessionId);
            urlString = defaultPictureId >= 0 ? "pictureid=" + defaultPictureId : "roomid=" + roomId;
        }
        return urlString;
    }


    protected bool IsSessionPicturesVisible()
    {
        if (CheckBoxHideDescriptions.Checked)
        {
            return false;
        }
        else
        {
            return ConfigurationManager.AppSettings["ShowSessionPictureOnSchedule"].ToLower().Equals("true");
        }
    }

    protected bool IsRoomImageVisible()
    {
        bool retVal = false;
        //if (!CheckBoxHideDescriptions.Checked &&
        //    !ConfigurationManager.AppSettings["ShowRoomPictureOnSchedule"].ToLower().Equals("true"))
        //{
        //    retVal = true;
        //}
        return retVal;
    }

    protected bool ShowCourseEvaluation()
    {
        var showSessionEvaluations = ConfigurationManager.AppSettings["ShowSessionEvaluations"];


        return Context.User.Identity.IsAuthenticated && showSessionEvaluations.ToLower().Equals("true");
    }

    protected string CheckForValidHTML(string oldHTML)
    {



        return Utils.ConvertEncodedHTMLToRealHTML(oldHTML);
    }

    protected string GetEvalTextForHyperlink(int idSession)
    {
        var seODS = new SessionEvalsODS();
        int cnt = seODS.GetBySessionIdCount(idSession);
        var sb = new StringBuilder();
        sb.Append("Session Evaluation");
        if (cnt > 0)
        {
            sb.Append(" (");
            sb.Append(seODS.GetBySessionIdCount(idSession).ToString());
            sb.Append(" Submitted)");
        }
        return sb.ToString();
    }

    /// <summary>
    /// give a session, get the default upload picture for that room.
    /// </summary>
    /// <param name="idSession"></param>
    /// <returns></returns>
    protected string GetSessionPictureIdForSession(int idSession)
    {
        return "7"; // purple phone
    }

    protected void CheckBoxHideCloud_CheckedChanged(object sender, EventArgs e)
    {
        Repeater1.DataBind();
        //if (CheckBoxHideCloud.Checked)
        //{
        //    Response.Redirect("~/Sessions.aspx?HideCloud=true");
        //}
        //else
        //{
        //    Response.Redirect("~/Sessions.aspx");
        //}
    }

    protected bool IsPlanToAttendInterestedEnabled()
    {
        return RegisteredForCurrentYear && 
               Context.User.Identity.IsAuthenticated &&
               ConfigurationManager.AppSettings["ScheduleAllowCheckAttend"] != null &&
               ConfigurationManager.AppSettings["ScheduleAllowCheckAttend"].ToLower().Equals("true");
    }

    protected bool IsInterestedEnabled()
    {
        return RegisteredForCurrentYear &&
               Context.User.Identity.IsAuthenticated && ConfigurationManager.AppSettings["ShowSessionInterest"] != null &&
               ConfigurationManager.AppSettings["ShowSessionInterest"].ToLower().Equals("true");
    }

    protected bool IsNotInterestedEnabled()
    {
        // default to false
        return RegisteredForCurrentYear &&
               Context.User.Identity.IsAuthenticated &&
               ConfigurationManager.AppSettings["ShowSessionNotInterest"] != null &&
               ConfigurationManager.AppSettings["ShowSessionNotInterest"].ToLower().Equals("true");
    }

    protected string GetInterestedText(int sessionId)
    {
        var sessionAttendeeOds = new SessionAttendeeODS();
        int numInterested =  sessionAttendeeOds.GetCountBySessionIdAndInterest(sessionId, 2);

        string showValue = "Interested(-)";

        // !Utils.CheckUserIsAdmin())

        if ( (ConfigurationManager.AppSettings["ShowSessionInterest"] != null && 
            ConfigurationManager.AppSettings["ShowSessionInterest"].ToLower().Equals("true")) ||
            Utils.CheckUserIsAdmin())
        {
            if (ConfigurationManager.AppSettings["ShowSessionInterestCount"] != null &&
                ConfigurationManager.AppSettings["ShowSessionInterestCount"].ToLower().Equals("true"))
            {
                showValue = string.Format("Interested ({0})", numInterested);
            }
            else
            {
                showValue = string.Format("Interested");
            }
        }
        return showValue;
        // ShowSessionInterest  ScheduleAllowCheckAttend
        
    }


    /// <summary>
    /// combines attendes permission to be contacted with speaker
    /// </summary>
    /// <param name="interestedOrPlanToAttend"></param>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    protected string GetCountOfSpeakerToAttendee(string interestedOrPlanToAttend, int sessionId)
    {
        string retString = string.Empty;

         // only set this data if you are speaker or admin
         // (should really just let speaker look at their own session and not others) todo:
        if (Utils.CheckUserIsPresenterOrAdmin())
        {

            Dictionary<int, int> emailFromSpeakerPlanToAttendDict;
            Dictionary<int, int> emailFromSpeakerInterestedDict;

            const string cacheName = "CacheGetCountOfSpeakerToAttendee";

            var listOfDictionaries = (List<Dictionary<int, int>>) HttpContext.Current.Cache[cacheName];

            if (listOfDictionaries != null)
            {
                emailFromSpeakerInterestedDict = listOfDictionaries[0];
                emailFromSpeakerPlanToAttendDict = listOfDictionaries[1];
            }
            else
            {

                Utils.GetDictionariesOfAllowEmailFromSpeaker(Utils.GetCurrentCodeCampYear(),
                                                             out emailFromSpeakerInterestedDict,
                                                             out emailFromSpeakerPlanToAttendDict);
                var listToCacheAndUse =
                    new List<Dictionary<int, int>>
                        {
                            emailFromSpeakerInterestedDict,
                            emailFromSpeakerPlanToAttendDict

                        };

                HttpContext.Current.Cache.Insert(cacheName, listToCacheAndUse,
                                                 null,
                                                 DateTime.Now.Add(new TimeSpan(0, 0,
                                                                               Utils.
                                                                                   RetrieveSecondsForSessionCacheTimeout
                                                                                   ())),
                                                 TimeSpan.Zero);
            }

            if (interestedOrPlanToAttend.ToLower().Equals("interested"))
            {
                if (emailFromSpeakerInterestedDict.ContainsKey(sessionId))
                {
                    retString = String.Format(
                        "{0} Attendees Have Given Permission for the speaker to contact them.",
                        emailFromSpeakerInterestedDict.ContainsKey(sessionId)
                            ? emailFromSpeakerInterestedDict[sessionId]
                            : 0);
                }
            }
            else if (interestedOrPlanToAttend.ToLower().Equals("plantoattend"))
            {
                retString = String.Format("{0} Attendees Have Given Permission for the speaker to contact them.",
                                          emailFromSpeakerPlanToAttendDict.ContainsKey(sessionId)
                                              ? emailFromSpeakerPlanToAttendDict[sessionId]
                                              : 0);
            }
        }

        return retString;
    }


    protected string GetWillAttendText(int sessionId)
    {
        string showValue = "Will Attend (-)";

        if ((ConfigurationManager.AppSettings["ScheduleAllowCheckAttend"] != null &&
           ConfigurationManager.AppSettings["ScheduleAllowCheckAttend"].ToLower().Equals("true")) ||
           Utils.CheckUserIsAdmin())
        {
            var sessionAttendeeOds = new SessionAttendeeODS();
            int numInterested = sessionAttendeeOds.GetCountBySessionIdAndInterest(sessionId, 3);
            showValue = string.Format("Will Attend ({0})", numInterested);

            if (!RegisteredForCurrentYear && Context.User.Identity.IsAuthenticated)
            {
                //showValue += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;   (MUST REGISTER TO SELECT INTEREST)";
                showValue += "&nbsp;&nbsp;&nbsp;"; // adding space so hyperlink has some extra room
            }
        }
        return showValue;
    }

    protected bool IsAuthenticatedByNotRegisteredForCurrentYear()
    {
        return (!RegisteredForCurrentYear &&
                 Context.User.Identity.IsAuthenticated);
    }

    protected bool IsCheckedNotInterested(int sessionId)
    {
        if (!Context.User.Identity.IsAuthenticated)
        {
            return false;
        }
        var q =
            from data in _listSessionAttendees
            where data.Sessions_id == sessionId
            select data.Id;
       return q.Count() <= 0;
    }
    protected bool IsCheckedInterested(int sessionId)
    {
        if (!Context.User.Identity.IsAuthenticated)
        {
            return false;
        }

        var q =
            from data in _listSessionAttendees
            where data.Sessions_id == sessionId && (int) data.Interestlevel == 2
            select data.Id;
        
        return q.Count() > 0;



    }
    protected bool IsCheckedPlanToAttend(int sessionId)
    {
        if (!Context.User.Identity.IsAuthenticated)
        {
            return false;
        }

        var q =
            from data in _listSessionAttendees
            where data.Sessions_id == sessionId && (int)data.Interestlevel == 3
            select data.Id;

        return q.Count() > 0;
    }

    protected bool GetTrackAdminVisible()
    {
        return Utils.CheckUserCanViewTrack();
    }

    protected bool GetAdminVisible()
    {
        return Utils.CheckUserIsAdmin();
    }

    protected string GetTrackIdFromSessionId(int sessionId)
    {

        var recRet = from myData in TrackSessionResults where myData.SessionId == sessionId select myData.TrackId;

        string retStr = "-2222";
        if (recRet != null)
        {
            retStr = recRet.ToList().SingleOrDefault().ToString();
        }
        return retStr;
    }

    protected string GetTrackNameFromSessionId(int sessionId)
    {
        string retStr = string.Empty;
        if (TrackNameAssignedToSession.ContainsKey(sessionId))
        {
            retStr = TrackNameAssignedToSession[sessionId] + " Track";
        }
        return retStr;
    }

    protected string GetTrackSpacerSessionId(int sessionId)
    {
        var stringToShow = string.Empty;
        if (TrackNameAssignedToSession.ContainsKey(sessionId))
        {
            stringToShow = "&nbsp; | &nbsp;";
        }
        return stringToShow;
    }

    protected bool GetHideTrackInfo()
    {
        return Utils.CheckUserIsTrackLeadOrAdmin() || ConfigurationManager.AppSettings["ShowTrackOnSession"] != null &
                                                      ConfigurationManager.AppSettings["ShowTrackOnSession"].ToLower().
                                                          Equals("true");
    }


    protected void ObjectDataSourceSessionSubmitted_DataBinding(object sender, EventArgs e)
    {

    }
    protected void Repeater1_ItemCreated(object sender, RepeaterItemEventArgs e)
    {
        
    }
    protected void RadioButtonSatOrSundayList_SelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox1.Text = string.Empty; // clear search box
        RadioButtonSatSunPressed = true;
    }

    //public bool RadioButtonSatSunPressedOrFirstTime { get; set; }

    protected void RadioButtonListTimes_SelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox1.Text = string.Empty; // clear search box

        RadioButtonTimesListPressed = true;
        if (RadioButtonListTimes.SelectedValue != null)
        {
            SelectedSessionTimeSlot = Convert.ToDateTime(RadioButtonListTimes.SelectedValue);
        }
    }
    protected void ButtonSearch_Click(object sender, EventArgs e)
    {
        SearchButtonCode(TextBox1.Text);
    }

    private void SearchButtonCode(string searchText)
    {
        SearchButtonPressed = true;

        var dictOfSessionTerms = Utils.GetSessionsBySearchWordsCached(Utils.GetCurrentCodeCampYear());

        string searchTextOriginal = searchText;
        if (searchTextOriginal.Contains("\"") || searchTextOriginal.Contains("\'"))
        {
            // if any quotes in there, make them use commas
            searchText = searchTextOriginal;  
        }
        else
        {
            List<string> tokens = searchTextOriginal.Split(' ').ToList();
            var stringBuilder1 = new StringBuilder();
            for (var index = 0; index < tokens.Count; index++)
            {
                stringBuilder1.Append(tokens[index]);
                if (index < tokens.Count-1)
                {
                    stringBuilder1.Append(",");
                }
            }
            searchText = stringBuilder1.ToString();
        }

        List<string> listOfSearchFull = GetListOfSearch(searchText);

        var listOfListOfWordsToOr = new List<List<string>>();

        // now look to see if any of the words have multiple words in them and sparate those out
        foreach (var word in listOfSearchFull)
        {
            List<string> wordList = word.Split(' ').ToList();
            if (wordList.Count > 0)
            {
                listOfListOfWordsToOr.Add(wordList);
            }
        }

        // finally, make a list of sessions that correspond with each word in the list

        // this make the list of all the single word match lists
        var sessionListIdsForOr = new List<List<int>>();
        foreach (var wordList in listOfListOfWordsToOr)
        {
            if (wordList.Count == 1)
            {
                // build a unique list of sessions that are associated with this word
                string wordOfInterest = wordList[0].ToLower();
                if (dictOfSessionTerms.ContainsKey(wordOfInterest))
                {
                    sessionListIdsForOr.Add(dictOfSessionTerms[wordOfInterest]);
                }
            }
        }


        // this is a list of list of lists.
        // for example say you put in 'steve evans',peter,kellner
        // the first one gives us lists of ones with steve andones with evans in same list.
        var sessionListIdsForAndList = new List<List<List<int>>>();
        foreach (var wordList in listOfListOfWordsToOr)
        {
            if (wordList.Count > 1)
            {
                // build a unique list of sessions that are associated with this word
                var listx = new List<List<int>>();
                foreach (var wordOfInterest in wordList)
                {
                    if (dictOfSessionTerms.ContainsKey(wordOfInterest.ToLower()))
                    {
                        listx.Add(dictOfSessionTerms[wordOfInterest.ToLower()]);
                    }
                }
                sessionListIdsForAndList.Add(listx);
            }
        }

        List<int> sessionsReturnedFromSearch = GetSessionsFromSearchData(sessionListIdsForOr, sessionListIdsForAndList);
        var stringBuilder = new StringBuilder();
        for (int index = 0; index < sessionsReturnedFromSearch.Count; index++)
        {
            var sessionReturn = sessionsReturnedFromSearch[index];
            stringBuilder.Append(sessionReturn.ToString());
            if (index < sessionsReturnedFromSearch.Count-1)
            {
                stringBuilder.Append(",");
            }
        }
        LabelSessionListFromSearch.Text = stringBuilder.ToString();
    }

    private static List<int> GetSessionsFromSearchData(List<List<int>> sessionListIdsForOr, List<List<List<int>>> sessionListIdsForAndList)
    {
        // for now, simply return just one list of numbers which is first word they typed in
        return sessionListIdsForOr.Count > 0 ? sessionListIdsForOr[0] : new List<int> {-999};
    }

    private List<string> GetListOfSearch(string text)
    {
        // http://omegacoder.com/?p=542
        var listOfSearch = new List<string>();
       
        string pattern = @"
                            (?xm)                        # Tell the compiler we are commenting (x = IgnorePatternWhitespace)
                                                         # and tell the compiler this is multiline (m),
                                                         # In Multiline the ^ matches each start line and $ is each EOL
                                                         # Pattern Start
                            ^(                           # Start at the beginning of the line always
                             (?![\r\n]|$)                # Stop the match if EOL or EOF found.
                             (?([\x27\x22])              # Regex If to check for single/double quotes
                                  (?:[\x27\x22])         # \\x27\\x22 are single/double quotes
                                  (?<Column>[^\x27\x22]*)# Match this in the quotes and place in Named match Column
                                  (?:[\x27\x22])
 
                              |                          # or (else) part of If when Not within quotes
 
                                 (?<Column>[^,\r\n]*)    # Not within quotes, but put it in the column
                              )                          # End of Pattern OR
 
                            (?:,?)                       # Either a comma or EOL/EOF
                            )+                           # 1 or more columns of data.";

        //string text = /* Note the ,, as a null situation */
        //@"'Alpha',,'01000000043','2','4',Regex Space 'Beta',333,444,""Other, Space"",No Quote Space,'555'";

       

        // We specified the Regex options in teh pattern, but we can also specify them here.
        // Both are redundant, decide which you prefer and use one.
        var CSVData = from Match m in Regex.Matches(text, pattern, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline)
                      select new
                                 {
                                     Data = from Capture cp in m.Groups["Column"].Captures
                                            select cp.Value,
                                 };

        foreach (var line1 in CSVData)
        {
            listOfSearch = line1.Data.ToList();
            // just grab first line
            break;
        }
        return listOfSearch;
    }

    protected string GetJobsButtonToolTip(int sessionId)
    {
        string toolTipTextToShow = string.Empty;

        if (_sessionSponsorListDictionary.ContainsKey(sessionId))
        {
            var rec = _sessionSponsorListDictionary[sessionId];
            toolTipTextToShow = String.Format("Click Here For A List of Job(s) Available From {0}", rec.SponsorName);
        }
        //if (_sessionJobListingDictionary.ContainsKey(sessionId))
        //{
        //    var rec = _sessionJobListingDictionary[sessionId];
        //    if (rec.ShowTextOnSession)
        //    {
        //        if (_jobListingsDictionary.ContainsKey(rec.JobListingId))
        //        {
        //            toolTipTextToShow = _jobListingsDictionary[rec.JobListingId].JobButtonToolTip;
        //        }
        //    }
        //}
        return toolTipTextToShow;
    }

    protected bool GetJobsButtonVisible(int sessionId)
    {
       
        return _sessionSponsorListDictionary.ContainsKey(sessionId);
        //if (_sessionJobListingDictionary.ContainsKey(sessionId))
        //{
        //    var rec = _sessionJobListingDictionary[sessionId];
        //    visible = rec.ShowImageOnSession;
        //}
        //return visible;
    }

    protected string GetJobsLabelText(int sessionId)
    {
        string textToShow = string.Empty;
        if (_sessionSponsorListDictionary.ContainsKey(sessionId))
        {
            var rec = _sessionSponsorListDictionary[sessionId];
            textToShow = String.Format("Click Here For A List of Jobs Available From {0}",rec.SponsorName);
        }
        return textToShow;
    
    }

    protected bool GetJobsLabelVisible(int sessionId)
    {
        bool showLabel = false;
        //if (_sessionJobListingDictionary.ContainsKey(sessionId))
        //{
        //    var rec = _sessionJobListingDictionary[sessionId];
        //    if (rec.ShowTextOnSession)
        //    {
        //        showLabel = true;
        //    }
        //}
        return showLabel;
    }



  // '<%# (string) GetJobsLabelVisible((int) Eval("Id")) %>' GetJobsLabelText


    protected string GetJobsPostbackURL(int sessionId)
    {
        string postbackURL = string.Empty;
        if (_sessionSponsorListDictionary.ContainsKey(sessionId))
        {
            int sponsorId = _sessionSponsorListDictionary[sessionId].Id;
            postbackURL = String.Format("~/Jobs.aspx?SponsorListIdStart={0}&SponsorListIdStop={0}", sponsorId);
        }
        return postbackURL;
    }

    protected string GetVideoLiteralText(int sessionId)
    {
        string videoURL = Utils.GetSessionVideoURL(sessionId);

       
        return videoURL;
    }

    protected bool GetVideoLiteralVisible(int sessionId)
    {
        string videoURL = Utils.GetSessionVideoURL(sessionId);
        return !String.IsNullOrEmpty(videoURL);
    }

    protected bool GetIsAdmin()
    {
        return Utils.CheckUserIsAdmin();
    }

    protected bool GetAssignVideoButtonVisible(string sessionId)
    {
        return Utils.CheckUserIsAdmin() || Utils.CheckUserIsVideoEditor();
    }

    protected void CheckBoxSessionsWithVideo_CheckedChanged(object sender, EventArgs e)
    {
      
    }

    protected bool ShowPBWiki(int sessionId)
    {
        return true;
        //if (_sessionListPbWikiShow.ContainsKey(sessionId))
        //{
        //    return _sessionListPbWikiShow[sessionId];
        //}
        //else
        //{
        //    return false;
        //}

        //bool showIt = false;
        //string wikiMode = ConfigurationManager.AppSettings["WikiMode"];
        //if (wikiMode.ToLower().Equals("true") || Utils.CheckUserIsAdmin())
        //{
        //    showIt = Utils.ShowPBWiki(sessionId);
        //}
        //return showIt;
    }

    protected bool ShowMaterialsUrlLink(int sessionId)
    {
        bool hasBoxLin = _boxFolderIdDictionary != null && _boxFolderIdDictionary.ContainsKey(sessionId);
        bool hasMaterialsLink = _materialsDictionary != null && _materialsDictionary.ContainsKey(sessionId);
        return hasBoxLin || hasMaterialsLink;
    }

    protected string GetTextForSlidesAndCode(int sessionId)
    {
        bool hasBoxLin = _boxFolderIdDictionary != null && _boxFolderIdDictionary.ContainsKey(sessionId);
        bool hasMaterialsLink = _materialsDictionary != null && _materialsDictionary.ContainsKey(sessionId);
        string title = hasBoxLin || hasMaterialsLink ? "Link To Slides And Code" : "No Slides Or Code Available";
        return title;
    }
}