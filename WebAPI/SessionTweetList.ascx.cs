using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App_Code;
using CodeCampSV;

public partial class SessionTweetList : System.Web.UI.UserControl
{
    private string _bitLyApiKey;
    private string _bitLyApiUsername;

    public bool ShowInterested { get; set; }
    public bool ShowPlanToAttend { get; set; }

    public int AttendeeId { get; set; }

    Dictionary<int,SessionTweetRecord> tweetDict = new Dictionary<int, SessionTweetRecord>();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack & HttpContext.Current.User.Identity.IsAuthenticated)
        {
            InitSessionsToTweet();
        }

    }

    protected string GetURLonclick(int  sessionId, string sessiontitle)
    {
        const string outboundLinkTemplate = "javascript:pageTracker._trackPageview ('/outbound/RETWEET-{0}-{1}');";
        string outboundLink = String.Format(outboundLinkTemplate, sessionId.ToString(CultureInfo.InvariantCulture), sessiontitle.Substring(0, Math.Min(35,sessiontitle.Length)).Replace(" ","_").Trim().ToUpper());
        return outboundLink;
    }

    private void InitSessionsToTweet()
    {
        _bitLyApiKey = ConfigurationManager.AppSettings["BitLiApiKey"] ?? string.Empty;
        _bitLyApiUsername = ConfigurationManager.AppSettings["BitLiApiUsername"] ?? string.Empty;

        var attendeePKID = new Guid(Utils.GetAttendeePKIDByUsername(HttpContext.Current.User.Identity.Name));


        var sessions =
            SessionAttendeeManager.I.Get(new SessionAttendeeQuery
                                             {
                                                 Attendees_username = attendeePKID,
                                                 CodeCampYearId = Utils.GetCurrentCodeCampYear()
                                             }).ToList();

        List<int> sessionsWithPlanToAttend =
            sessions.Where(a => a.Interestlevel == 3).Select(a => a.Sessions_id).ToList();



        List<int> sessionsWithInterest =
            sessions.Where(a => a.Interestlevel == 2).Select(a => a.Sessions_id).ToList();




        var sessionsToTweet = SessionsManager.I.Get(new SessionsQuery
                                                        {
                                                            Ids = sessions.Select(a=>a.Sessions_id).ToList(),
                                                            WithSpeakers = true,
                                                            WithSchedule = true
                                                        }).ToList();

        var sessionTweetRecords = new List<SessionTweetRecord>();
        foreach (var session in sessionsToTweet)
        {
            if (String.IsNullOrEmpty(session.ShortUrl))
            {
                string sessionUrl = String.Format("http://siliconvalley-codecamp.com/Sessions.aspx?sessionid={0}",
                                                  session.Id);
                session.ShortUrl = API.Bit(_bitLyApiUsername, _bitLyApiKey,
                                           HttpContext.Current.Server.HtmlEncode(sessionUrl), "Shorten");
                Utils.UpdateSessionShortUrl(session.Id, session.ShortUrl);
            }

         
            DateTime sessionTime;
            string sessionTimeFriendly;
            string speakerNames;
            string speakerHandles;
            string hashTags;
            Utils.GetSessionHandlesHashTagsTime(session, out speakerNames, out speakerHandles, out hashTags, out sessionTime, out sessionTimeFriendly);

            if (
                (ShowInterested && sessionsWithInterest.Contains(session.Id))
                ||
                (ShowPlanToAttend && sessionsWithPlanToAttend.Contains(session.Id)))
            {
                sessionTweetRecords.Add(new SessionTweetRecord
                                            {
                                                SessionId = session.Id,
                                                SessionTitle = session.Title,
                                                SessionSpeakerNames = speakerNames,
                                                SessionSpeakerHandles = speakerHandles,
                                                SessionHashTags = hashTags,
                                                SessionTimeFriendly = sessionTimeFriendly,
                                                SessionTime = sessionTime,
                                                SessionUrl = session.ShortUrl
                                            });
            }
        }

        tweetDict = sessionTweetRecords.ToDictionary(k => k.SessionId, v => v);

        //http://twitter.com/home?status=your+message+goes+here+and+link+to+http://yourlink.com
        //http://twitter.com/home?status=RT @ColinKlinkert – Colin showing you how to create a retweet 

        RepeaterSessions.DataSource = sessionTweetRecords.OrderBy(a => a.SessionTime.Value).ToList();
        RepeaterSessions.DataBind();



    }


    protected string GetNavigateText(int sessionId)
    {
        var tweetString = GetTweetString(sessionId,false,false);
        string str = String.Format("RT {0}", tweetString);
        return str;
    }

    protected string GetNavigateUrl(int sessionId)
    {
        var tweetString = GetTweetString(sessionId,true,true);
        return String.Format("http://twitter.com/home?status=RT+@sv_code_camp+{0}", tweetString.Replace(" ", "+"));
    }
   
    private string GetTweetString(int sessionId, bool escapeHash,bool includeUrl)
    {
       

        var tweet = tweetDict[sessionId];
        var tweetString =
            string.Format("{0} {1} {2} {3} #svcc  {4}", tweet.SessionTimeFriendly,
            tweet.SessionSpeakerHandles, tweet.SessionSpeakerNames,tweet.SessionHashTags, includeUrl ? tweet.SessionUrl : "");


        // we always have "RT sv_code_camp " and we want to leave it 20 characters short so by say 105 that should do it.

        int leftForTitle = 105 - tweetString.Length;
        if (leftForTitle > 5)
        {
            string titleToAdd = tweet.SessionTitle.Substring(0, Math.Min(tweet.SessionTitle.Length, leftForTitle));
            tweetString = titleToAdd + " " + tweetString;
        }

        if (escapeHash)
        {
            tweetString=  tweetString.Replace("#", "%23");
        }
        
        return tweetString;
    }

   


      //<%-- <asp:HyperLink ID="HyperLinkReTweet" runat="server" 
      //          NavigateUrl="http://twitter.com/home?status=RT+@pkellner+Release+2+in+store+today+http://agelessemail.com"
      //          Text="RT+@pkellner+Release+2+in+store+today+http://agelessemail.com"
      //            >
                
      //      </asp:HyperLink>--%>
}

public class SessionTweetRecord
{
    public int SessionId { set; get; }

    public DateTime? SessionTime { get; set; }
    public string SessionTimeFriendly { get; set; }

    public string SessionUrl { get; set; }

    public string SessionTitle { get; set; }
    public string SessionSpeakerNames { get; set; }
    public string SessionSpeakerHandles { get; set; }
    public string SessionHashTags { get; set; }

}


