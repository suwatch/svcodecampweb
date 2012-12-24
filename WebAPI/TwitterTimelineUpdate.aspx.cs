using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqToTwitter;
using CodeCampSV;
using System.Configuration;

public partial class TwitterTimelineUpdate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        int numDaysToGoBack = 10;
        if (ConfigurationManager.AppSettings["TwitterDaysBack"] != null)
        {
            numDaysToGoBack = Convert.ToInt32(ConfigurationManager.AppSettings["TwitterDaysBack"]);
        }

        List<TwitterUpdateResult> twitterUpdateResults = TwitterUpdateManager.I.Get(new TwitterUpdateQuery()
        {
            // need to put numDaysToGoBack + 1 here
        });


        var ctx = new TwitterContext();

        var search =
            (from tweet in ctx.Search
             where tweet.Type == SearchType.Search &&
                   tweet.Query == "@sv_code_camp OR #svcc OR #SVCC OR #Svcc OR @SV_CODE_CAMP" &&
                   tweet.Since == DateTime.Now.AddDays(numDaysToGoBack * -1).Date
             select tweet)
            .SingleOrDefault();

        List<LinqToTwitter.AtomEntry> recs = search.Entries.ToList();

        int cnt = 0;
        foreach (var rec in recs)
        {
            // check and see if we have it already
            var tweetRecFound = twitterUpdateResults.Where(a=>a.AlternateTweet.Equals(rec.Alternate)).FirstOrDefault();
            if (tweetRecFound == null)
            {
                cnt++;
                LinqToTwitter.AtomAuthor atomAuthor = rec.Author;

                string authorHandle = string.Empty;
                if (atomAuthor.Name.IndexOf("(") > 0)
                {
                    authorHandle = atomAuthor.Name.Substring(0, atomAuthor.Name.IndexOf("(") - 1).Trim();
                }

                // get all attendeeIds that are speakers
                var speakerIds = (SessionsManager.I.Get(new SessionsQuery() { CodeCampYearId = Utils.CurrentCodeCampYear }).Select(a => a.Attendeesid).ToList());

                // Let's see if this person is a code camp speaker.
                string codeCampSessionsUrl = String.Empty;
                string codeCampSpeakerUrl = String.Empty;
                var speaker = AttendeesManager.I.Get(new AttendeesQuery()
                {
                    CodeCampYearId = Utils.CurrentCodeCampYear,
                    PresentersOnly = true,
                    TwitterHandle = authorHandle
                }).FirstOrDefault();
                if (speaker != null && speakerIds.Contains(speaker.Id))
                {
                    codeCampSessionsUrl = String.Format("http://www.siliconvalley-codecamp.com/Sessions.aspx?ForceSortBySessionTime=true&AttendeeId={0}", speaker.Id);
                    codeCampSpeakerUrl = String.Format("http://www.siliconvalley-codecamp.com/Speakers.aspx?AttendeeId={0}", speaker.Id);
                }

                TwitterUpdateManager.I.Insert(new TwitterUpdateResult()
                {
                    AlternateTweet = rec.Alternate,
                    AuthorImageUrl = rec.Image,
                    AuthorUrl = atomAuthor.URI,
                    AuthorName = atomAuthor.Name,
                    AuthorHandle = authorHandle,
                    ContentTweet = rec.Content,
                    Published = rec.Published,
                    Title = rec.Title,
                    TweetInserted = DateTime.UtcNow,
                    TweetUpdate = DateTime.UtcNow,
                    CodeCampSessionsUrl = codeCampSessionsUrl,
                    CodeCampSpeakerUrl = codeCampSpeakerUrl
                });
            }

        }

      TextBox1.Text = cnt.ToString() + " Records inserted;";

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        // simply update again
    }
}