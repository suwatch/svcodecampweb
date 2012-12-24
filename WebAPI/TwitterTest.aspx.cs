using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqToTwitter;
using CodeCampSV;


public partial class TwitterTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected string GetCodeCampSessionsString(string url)
    {
        if (!String.IsNullOrEmpty(url))
        {
            return String.Format(@"&nbsp;&nbsp;&nbsp;<a  href='{0}'>Code Camp Sessions</a>", url);
        }
        else
        {
            return String.Empty;
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

        var recs = TwitterUpdateManager.I.GetAll();

        //foreach ( var rec in recs)
        //{

        //}

        RepeaterTweet.DataSource = recs;
        RepeaterTweet.DataBind();


        //Console.WriteLine("\nWith advanced options: \n");

        //var searchAdv =
        //    (from tweet in ctx.Search
        //     where tweet.Type == SearchType.Search &&
        //           tweet.WordOr == "#svcc @sv_code_camp" &&
        //           tweet.Since == DateTime.Now.AddDays(-5).Date
        //     select tweet)
        //    .SingleOrDefault();

        //searchAdv.Entries.ForEach(tweet => Console.WriteLine("Tweet: {0}\n", tweet.Content));

        //Console.ReadKey();



        //var twitterCtx = new TwitterContext();

        //var publicTweets =
        //    from tweet in twitterCtx.Status
        //    where tweet.Type == StatusType.Public
        //    select tweet;

        //var search =
        //        (from tweet in twitterCtx.Search
        //         where tweet.Type == SearchType.Search &&
        //               tweet.Query == "@sv_code_camp OR #svcc" &&
        //               tweet.Since == DateTime.Now.AddDays(-5).Date
        //         select tweet).ToList();



        //foreach (var tweet in search)
        //{
        //    //tweet.
        //    //string screenName = tweet.ScreenName;
        //    ////string x = tweet.Text;
        //    //string y = tweet.User.Identifier.ScreenName;
        //    //string z = tweet.StatusID;



        //}

        //publicTweets.ToList().ForEach(
        //    tweet => Console.WriteLine(
        //        "User Name: {0}, Tweet: {1}",
        //        tweet.User.Name,
        //        tweet.Text));

        //var myMentions =
        //       from mention in twitterCtx.Status
        //       where mention.Type == StatusType.Mentions
        //       select mention;

        //myMentions.ToList().ForEach(
        //    mention => Console.WriteLine(
        //        "Name: {0}, Tweet[{1}]: {2}\n",
        //        mention.User.Name, mention.StatusID, mention.Text));


    }
}