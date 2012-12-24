using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using App_Code;
using System.Configuration;
using CodeCampSV;
using LinqToTwitter;
using aspNetEmail;

public partial class TwitterPost : System.Web.UI.Page
{

    private string _bitLyApiKey;
    private string _bitLyApiUsername;
    private string _twitterCredential;
    private bool _tweetNewSessions;
    private int _tweetsToProcessAtOnce;

    private readonly object syncvar = new object();

    private readonly bool testMode = false;

    /// <summary>
    /// this page simply checks to see if there is anything ready to post to twitter and does it.
    /// no security for now, but might want to have a token later just in case.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        // only do this between 7am and 9pm
        DateTime now = DateTime.UtcNow;
        int hour = now.Hour - 7;
        if (hour < 0)
        {
            hour = 24 + hour;
        }

        if (hour < 6 || hour > 23)
        {
            WriteToLog("Not Doing TwitterPost Because outside of 7am to 9pm  UTC: " + DateTime.UtcNow);
            return;
        }
       
        _tweetsToProcessAtOnce = ConfigurationManager.AppSettings["TweetToProcessAtOnce"] != null
                                     ? Convert.ToInt32(ConfigurationManager.AppSettings["TweetToProcessAtOnce"])
                                     : 2;
        _tweetNewSessions = ConfigurationManager.AppSettings["TweetNewSessions"] != null &&
                            ConfigurationManager.AppSettings["TweetNewSessions"].ToLower().Equals("true");
        _bitLyApiKey = ConfigurationManager.AppSettings["BitLiApiKey"] ?? string.Empty;
        _bitLyApiUsername = ConfigurationManager.AppSettings["BitLiApiUsername"] ?? string.Empty;
        _twitterCredential = ConfigurationManager.AppSettings["TwitterCredential"] ?? string.Empty;

        if (HttpContext.Current.Request.QueryString["override"] != null)
        {
            _tweetNewSessions = true;
        }

        string resultTweet = ProcessTweets();

        WriteToLog(resultTweet);
    }

    private void WriteToLog(string resultTweet)
    {
        if (ConfigurationManager.AppSettings["TweetNewSessionsDiskLog"] != null &&
            ConfigurationManager.AppSettings["TweetNewSessionsDiskLog"].ToLower().Equals("true"))
        {
            String fileName = Context.Server.MapPath("~") + "\\App_Data\\TwitterPost.log";
            lock (syncvar)
            {
                using (var sw = new StreamWriter(fileName, true))
                {
                    sw.WriteLine(DateTime.Now + " " + resultTweet);
                }
            }
        }
    }


    /// <summary>
    /// forces a postback so pageload runs again
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ButtonTestTweetClick(object sender, EventArgs e)
    {
       
    }

    private string ProcessTweets()
    {
        string retString;
        if (!_tweetNewSessions)
        {
            retString = "TweetNewSessions in app.config not set to true " + DateTime.Now;
        }
        else
        {
            StringBuilder sbText;
            var sessionsToTweet1 = SessionsToTweetAfterSignup(out sbText);
            retString = "<br/>SessionsToTweetAfterSignup: Sessions Tweeted to sv_code_camp: " + sessionsToTweet1.Count.ToString(CultureInfo.InvariantCulture) + sbText;

            var sessionsToTweet2 = SessionsToTweetBeforeBigEvent(out sbText);
            retString += "<br/><br/>  SessionsToTweetBeforeBigEvent: Sessions Tweeted to sv_code_camp: " + sessionsToTweet2.Count.ToString(CultureInfo.InvariantCulture) + sbText;


            
            
        }


        Label1.Text = retString;



        return retString;
    }

    private List<SessionsResult> SessionsToTweetBeforeBigEvent(out StringBuilder sbText)
    {
        var codeCampYearResult = CodeCampYearManager.I.Get(new CodeCampYearQuery {Id = Utils.CurrentCodeCampYear}).FirstOrDefault();
        if (codeCampYearResult != null)
        {
            DateTime startTweetsAfter =
                codeCampYearResult.
                    CampStartDate.Subtract(new TimeSpan(12, 0, 0, 0));

            if (startTweetsAfter.CompareTo(DateTime.Now) > 0)
            {
                sbText = new StringBuilder("No Tweeting because too soon for SessionsToTweetBeforeBigEvent()");
                return new List<SessionsResult>();
            }
        }
        else
        {
            sbText = new StringBuilder("No Tweeting because no code camp year found");
            return new List<SessionsResult>();
        }


        // if got here, then we should start tweeting all sessions again

        var sessionsToTweet =
            SessionsManager.I.Get(new SessionsQuery
                                      {
                                          TweetLineTweetedPreCamp = false,
                                          WithSchedule = true,
                                          WithSpeakers = true,
                                          CodeCampYearId = Utils.CurrentCodeCampYear
                                         // Id = 953 // test for just scottgu and email comes to me
                                      }).Where(a => !a.SessionTime.Equals("Agenda Not Set Yet")).
                                      OrderBy(a => a.SessionTimesResult.StartTime.HasValue ? a.SessionTimesResult.StartTime.Value : DateTime.Now).Take(_tweetsToProcessAtOnce).
                ToList();

        sbText = new StringBuilder();
        foreach (var session in sessionsToTweet)
        {
           
            DateTime sessionTime;
            string sessionTimeFriendly;
            string speakerNames;
            string speakerHandles;
            string hashTags;
            Utils.GetSessionHandlesHashTagsTime(session, out speakerNames, out speakerHandles,out hashTags, out sessionTime, out sessionTimeFriendly);
            string sessionTitle = Utils.ClearSpecialCharacters(session.Title);


            string sessionUrl = String.Format("http://siliconvalley-codecamp.com/Sessions.aspx?sessionid={0}",
                                              session.Id);
            string sessionUrlShort = API.Bit(_bitLyApiUsername, _bitLyApiKey,
                                             HttpContext.Current.Server.HtmlEncode(sessionUrl), "Shorten");

            // "at 9:45AM 10/6 Speaker Douglas Crockford @dougc Title: Gonads and Ponads  #svcc #json http://dkfjdkf 
            string template = "{0}  {1} {2} \"{{0}}\" #svcc {3} {4} ReTweet!";


            string tweetFirstPart =
                String.Format(template, sessionTimeFriendly, speakerNames, speakerHandles, hashTags,
                              sessionUrlShort).Trim();
            int spaceLeftForTitle = 130 - tweetFirstPart.Length;
            int titleLen = Math.Min(sessionTitle.Length, spaceLeftForTitle);



            string tweet = String.Format(tweetFirstPart, sessionTitle.Substring(0, titleLen)).Trim();

            // finally, do the tweet
            string credentialString = _twitterCredential;

            var credentials = new SessionStateCredentials();
            credentials.Load(credentialString);

            var auth = new WebAuthorizer
            {
                Credentials = credentials
            };

            var twitterCtx = new TwitterContext(auth);
            const decimal lattitude = (decimal)37.362056;
            const decimal longitude = (decimal)122.131056;
            if (!testMode)
            {
                Status tweetInfo = twitterCtx.UpdateStatus(tweet, lattitude, longitude);
                Label1.Text = tweetInfo.Text; // not sure if this helps
            }
            sbText.AppendLine(tweet);

            // update
            session.TweetLinePreCamp = tweet;
            session.TweetLineTweetedDatePreCamp = DateTime.Now;
            session.TweetLineTweetedPreCamp = true;

            if (!testMode)
            {
                SessionsManager.I.Update(session);
            }

            // finally, send the emails.
            foreach (var rec in session.SpeakersList)
            {
                SendEmailToSpeakerJustBeforeEvent(rec.UserFirstName, rec.UserLastName, rec.Email, session.Title, tweet);
            }
        }
        return sessionsToTweet;
    }

    private List<SessionsResult> SessionsToTweetAfterSignup(out StringBuilder sbText)
    {
        //DateTime compareDateTime = DateTime.Now.Subtract(new TimeSpan(0, 1, 0, 0));
        // do a max of 2 at a time.  these go every 30 minutes
        var sessionsToTweet = SessionsManager.I.Get(new SessionsQuery
                                                        {
                                                            TweetLineTweeted = false,
                                                            WithSpeakers = true,
                                                            WithSchedule = true,
                                                            CodeCampYearId = Utils.CurrentCodeCampYear
                                                        }).Where(
                                                            a =>
                                                            a.Createdate != null &&
                                                            a.Createdate.Value.CompareTo(
                                                                DateTime.Now.Subtract(new TimeSpan(0, 48, 0, 0))) <
                                                            0).
            OrderByDescending(a => a.Id).Take(_tweetsToProcessAtOnce).ToList();

        sbText = new StringBuilder();
        foreach (var session in sessionsToTweet)
        {
            var speakers = Utils.GetSpeakersBySessionId(session.Id);
            var sb = new StringBuilder();
            foreach (var speaker in speakers)
            {
                string speakerName = string.Format("{0} {1},", speaker.UserFirstName, speaker.UserLastName);
                sb.Append(speakerName);
            }
            string speakerList = sb.ToString();
            if (speakerList.Length > 0 && speakerList.EndsWith(","))
            {
                speakerList = speakerList.Substring(0, speakerList.Length - 1);
            }

            string sessionUrl = String.Format("http://siliconvalley-codecamp.com/Sessions.aspx?sessionid={0}",
                                              session.Id);
            string sessionUrlShort = API.Bit(_bitLyApiUsername, _bitLyApiKey,
                                             HttpContext.Current.Server.HtmlEncode(sessionUrl), "Shorten");

            string template = "NEW SESSION: {0}  Speaker: {1} #svcc Title: \"{2}\"";
            if (speakerList.Length > 0 && speakers.Count > 1)
            {
                template = "NEW SESSION: {0}  Speakers: {1} #svcc Title: \"{2}\"";
            }

            string tweet = String.Format(template, sessionUrlShort, speakerList, session.Title);
            if (tweet.Length > 139)
            {
                tweet = tweet.Substring(0, 136) + "...";
            }

            // finally, do the tweet
            string credentialString = _twitterCredential;

            var credentials = new SessionStateCredentials();
            credentials.Load(credentialString);

            var auth = new WebAuthorizer
                           {
                               Credentials = credentials
                           };

            var twitterCtx = new TwitterContext(auth);
            const decimal lattitude = (decimal) 37.362056;
            const decimal longitude = (decimal) 122.131056;
            if (!testMode)
            {
                Status tweetInfo = twitterCtx.UpdateStatus(tweet, lattitude, longitude);
                Label1.Text = tweetInfo.Text; // not sure if this helps
            }
           
            sbText.AppendLine(tweet);

            // update
            session.TweetLine = tweet;
            session.TweetLineTweetedDate = DateTime.Now;
            session.TweetLineTweeted = true;
            if (!testMode)
            {
                SessionsManager.I.Update(session);
            }

            // finally, send the emails.
            foreach (var rec in speakers)
            {
                SendEmailToSpeaker(rec.UserFirstName, rec.UserLastName, rec.Email, session.Title, tweet);
            }
        }
        return sessionsToTweet;
    }

    private void SendEmailToSpeakerJustBeforeEvent(string userFirstName, string userLastName, string email, string sessionTitle, string tweet)
    {

        var sb = new StringBuilder();

        sb.AppendLine(String.Format("Hi {0} {1} (Code Camp Speaker)", userFirstName, userLastName));
        sb.AppendLine(" ");
        sb.AppendLine(String.Format("Your Session {0} Has Been Tweeted With the agenda info!", sessionTitle));
        sb.AppendLine(" ");
        sb.AppendLine("You can now see your session on the Silicon Valley Code Camp Twitter Feed.  That feed can be found");
        sb.AppendLine("at the url: http://twitter.com/sv_code_camp .  Please subscribe to the twitter feed as well as ReTweet");
        sb.AppendLine("it so other will know about it.  Also, please note our Twitter hash tag included in that tweet is #svcc .");
        sb.AppendLine("If you do not have hash tags or your twitter handle (if you have one) in the tweet, please add them by editing your session.");
        sb.AppendLine(" ");
       
        sb.AppendLine("Thanks for speaking at code camp!  See you very soon.  Feel free to respond to this email if you have any questions.");
        sb.AppendLine(" ");
        sb.AppendLine("Regards,");
        sb.AppendLine("Your Code Camp Team");
        sb.AppendLine("http://siliconvalley-codecamp.com");

        try
        {
            var msg = new EmailMessage(true, false)
            {
                Logging = true,
                LogOverwrite = false,
                LogPath = MapPath(string.Empty) + "\\App_Data\\TwitterSend.log",
                FromAddress = Utils.GetServiceEmailAddress(),
                ReplyTo = Utils.GetServiceEmailAddress(),
                To = email,
                Subject = String.Format("Your Silicon Valley Code Camp Session Has Been Tweeted!"),
                Body = sb.ToString()
            };

            if (msg.Server.Equals("smtp.gmail.com"))
            {
                var ssl = new AdvancedIntellect.Ssl.SslSocket();
                msg.LoadSslSocket(ssl);
                msg.Port = 587;
            }
            if (!testMode)
            {
                msg.Send();
            }

        }
        catch (Exception)
        {

        }

    }

    private void SendEmailToSpeaker(string userFirstName, string userLastName, string email,string sessionTitle, string tweet)
    {

        var sb = new StringBuilder();

        sb.AppendLine(String.Format("Hi {0} {1} (Code Camp Speaker)", userFirstName, userLastName));
        sb.AppendLine(" ");
        sb.AppendLine(String.Format("Your Session {0} Has Been Tweeted!", sessionTitle));
        sb.AppendLine(" ");
        sb.AppendLine("You can now see your session on the Silicon Valley Code Camp Twitter Feed.  That feed can be found");
        sb.AppendLine("at the url: http://twitter.com/sv_code_camp .  Please subscribe to the twitter feed as well as ReTweet");
        sb.AppendLine("it so other will know about it.  Also, please note our Twitter hash tag included in that tweet is #svcc .");
        sb.AppendLine(" ");
        sb.AppendLine("Please also assume that your session has been accepted and that we will notify you about 2 weeks before the");
        sb.AppendLine("Event with the exact time of your session and provide you with additional details.");
        sb.AppendLine(" ");
        sb.AppendLine("Thanks for submitting you session.  Feel free to respond to this email if you have any questions.");
        sb.AppendLine(" ");
        sb.AppendLine("Regards,");
        sb.AppendLine("Your Code Camp Team");
        sb.AppendLine("http://siliconvalley-codecamp.com");

        try
        {
            var msg = new EmailMessage(true, false)
            {
                Logging = true,
                LogOverwrite = false,
                LogPath = MapPath(string.Empty) + "\\App_Data\\TwitterSend.log",
                FromAddress = Utils.GetServiceEmailAddress(),
                ReplyTo = Utils.GetServiceEmailAddress(),
                To = email,
                Subject = String.Format("Your Silicon Valley Code Camp Session Has Been Tweeted!"),
                Body = sb.ToString()
            };

            if (msg.Server.Equals("smtp.gmail.com"))
            {
                var ssl = new AdvancedIntellect.Ssl.SslSocket();
                msg.LoadSslSocket(ssl);
                msg.Port = 587;
            }
            if (!testMode)
            {
                msg.Send();
            }

        }
        catch (Exception )
        {
            
        }
        
    }
}