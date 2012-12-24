using System;
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


public partial class SessionsWorking : BaseContentPage // BasePage {BasePage tracks viewstate size with errors)
{
    protected Dictionary<int, string> SessionLevelsDictionary;

    private List<SessionAttendeeODS.DataObjectSessionAttendee> listSessionAttendees;

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

    protected void DropDownListTracks_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList) sender;

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
        if (ConfigurationManager.AppSettings["SessionsPageUsingExtJS"] != null &&
            ConfigurationManager.AppSettings["SessionsPageUsingExtJS"].ToLower().Equals("true"))
        {
            SessionsContainerId.Visible = false;
            IdSortBy.Visible = false;
            IdTrackDescription.Visible = false;
            CheckBoxShowOnlyAssigned.Visible = false;

            SessionsUsingExtJSFromDOMId.Visible = true;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
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
            listSessionAttendees = new SessionAttendeeODS().GetByUsername(Context.User.Identity.Name);

            //FormsAuthentication.SetAuthCookie("pkellner",true);
            if (!IsPostBack)
            {
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
            }

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
                Repeater1.DataSourceID = "ObjectDataSourceByTag";
            }
            else if (Request.QueryString["track"] != null)
            {
                Repeater1.DataSourceID = "ObjectDataSourceByTrack";
            }
            else if (Request.QueryString["id"] != null)
            {
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
                string guidString = Request.QueryString["PKID"];
                string username = Utils.GetAttendeeUsernameByGUID(guidString);
                if (!String.IsNullOrEmpty(username))
                {
                    if (!Roles.IsUserInRole(username, Utils.GetAdminRoleName()))
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

        



        //// make sure primary presenter is first
        //List<int> attendeeIds = (from data in codeCampDataContext.Sessions
        //                               where data.Id == sessionId
        //                               select data.Attendeesid).ToList();

        //// the orderby id forces the primary speaker to be listed first (hopefully)
        //List<Attendees> attendeeSpeakers = (from data in codeCampDataContext.Attendees
        //                                    where attendeeIds.Contains(data.Id)
        //                                    orderby  data.Id
        //                                    select data).ToList();

        //var rec = from data in codeCampDataContext.Attendees
        //          where data.Username.Equals("pkellner")
        //          select data;
        //attendeeSpeakers.Add(rec.FirstOrDefault());

        //return attendeeSpeakers;
        
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
        else if (e.CommandArgument.ToString().StartsWith("PostWiki"))
        {
            int sessionId = Int32.Parse(e.CommandArgument.ToString().Substring(9));

            string firstName;
            string lastName;
            string description;
            string sessionURL;
            string speakerURL;
            string sessionTitle;
            string speakerBio = string.Empty;
            string speakerZipCode = string.Empty;
            string speakerPersonalUrl = string.Empty;
            string speakerPictureUrl = string.Empty;
            DateTime sessionStartTime = DateTime.MinValue;
            bool success = Utils.GetSessionInfo(sessionId, out firstName, out lastName, out description, out sessionURL,
                                                out speakerURL, out sessionTitle,
                                                out speakerBio, out speakerPictureUrl,
                                                out speakerZipCode, out speakerPersonalUrl, out sessionStartTime);
            if (success)
            {
                string newTitle = CallToGenerateNewPage(sessionTitle, string.Format("{0} {1}", firstName, lastName),
                                                        description,
                                                        sessionURL, speakerURL);

                if (!string.IsNullOrEmpty(newTitle))
                {
                    string url = "http://codecamp.pbwiki.com/" + newTitle;
                    Response.Redirect(url);
                }
            }
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

    protected bool GetDeleteOrEditButtonVisible(string userNameOfSession)
    {
        bool showDeleteButton = false;
        if (CheckBoxHideDescriptions.Checked)
        {
            showDeleteButton = false;
        }
        else if ((Context.User.Identity.IsAuthenticated &&
             Context.User.Identity.Name.ToLower().Equals(userNameOfSession.ToLower())) || Utils.CheckUserIsAdmin())
        {
            showDeleteButton = true;
        }
        //return false;
        return showDeleteButton;
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
        string ccYear = Utils.ConvertCodeCampYearToActualYear(Utils.CurrentCodeCampYear.ToString());

        string responseReturn;
        String sessionNameWithYear = sessionNameIn + "-" + ccYear;
                                     
        string wikiPageName = Regex.Replace(sessionNameWithYear, @"[^\w@-]", "");

        // this try loop is bad. need to reorganize error when page exists.
        // should check first
        try
        {
            string newTitle = PBWikiAPI.AddPage("http://codecamp.pbwiki.com",
                                                ConfigurationManager.AppSettings["PBWikiAPIKey"],
                                                sessionNameWithYear,
                                                "AutoGenerator",
                                                string.Empty,
                                                SessionPageContent(sessionNameIn, authorName, sessionDescription,
                                                                   sessionURL, speakerURL),
                                                out responseReturn
                );
        }
        catch (Exception e)
        {
            String str = e.ToString();
        }

        return wikiPageName;
    }

    protected void LinkButtonWiki_Click(object sender, EventArgs e)
    {
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

    protected bool ShowPBWiki(int sessionId)
    {
        bool showIt = false;
        string wikiMode = ConfigurationManager.AppSettings["WikiMode"];
        if (wikiMode.ToLower().Equals("true") || Utils.CheckUserIsAdmin())
        {
            showIt = Utils.ShowPBWiki(sessionId);
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
        return Context.User.Identity.IsAuthenticated &&
               ConfigurationManager.AppSettings["ScheduleAllowCheckAttend"] != null &&
               ConfigurationManager.AppSettings["ScheduleAllowCheckAttend"].ToLower().Equals("true");
    }

    protected bool IsInterestedEnabled()
    {
        return Context.User.Identity.IsAuthenticated && ConfigurationManager.AppSettings["ShowSessionInterest"] != null &&
               ConfigurationManager.AppSettings["ShowSessionInterest"].ToLower().Equals("true");
    }

    protected bool IsNotInterestedEnabled()
    {
        return Context.User.Identity.IsAuthenticated && ConfigurationManager.AppSettings["ShowSessionInterest"] != null && 
            ConfigurationManager.AppSettings["ShowSessionInterest"].ToLower().Equals("true");
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
            showValue = showValue = string.Format("Interested ({0})", numInterested); 
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
        }
        return showValue;
    }

    protected bool IsCheckedNotInterested(int sessionId)
    {
        if (!Context.User.Identity.IsAuthenticated)
        {
            return false;
        }
        var q =
            from data in listSessionAttendees
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
            from data in listSessionAttendees
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
            from data in listSessionAttendees
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

  
}