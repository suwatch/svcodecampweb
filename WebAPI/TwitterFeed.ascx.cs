using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;

public partial class TwitterFeed : System.Web.UI.UserControl
{
    public int MaxTweetsToShow { get; set; }

    public bool ShowTopArea { get; set; }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        TopAreaAboveTweetsID.Visible = ShowTopArea;

        LabelCodeCampYearId.Text = Utils.CurrentCodeCampYear.ToString();

        if (Context.User.Identity.IsAuthenticated)
        {
            LabelUsername.Text = Context.User.Identity.Name;
        }

        if (CheckBoxList1.Items[0].Selected)
        {
            LabelSvCodeCamp.Text = "1";
        }
        else
        {
            LabelSvCodeCamp.Text = "0";
        }

        if (CheckBoxList1.Items[1].Selected)
        {
            LabelPoundSVCC.Text = "1";
        }
        else
        {
            LabelPoundSVCC.Text = "0";
        }

        if (CheckBoxList1.Items[2].Selected)
        {
            LabelAllSpeakers.Text = "1";
        }
        else
        {
            LabelAllSpeakers.Text = "0";
        }

        if (CheckBoxList1.Items[3].Selected)
        {
            LabelPlanPlusInterest.Text = "1";
        }
        else
        {
            LabelPlanPlusInterest.Text = "0";
        }

        if (CheckBoxList1.Items[4].Selected)
        {
            LabelPlanToAttend.Text = "1";
        }
        else
        {
            LabelPlanToAttend.Text = "0";
        }

        LabelMaxTweetsToShow.Text = MaxTweetsToShow.ToString();

        RepeaterTweet.DataBind();
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

    protected string GetPictureLink(string ccSpeakerUrl,string authorUrl)
    {
        if (!String.IsNullOrEmpty(ccSpeakerUrl))
        {
            return ccSpeakerUrl;
        }
        else
        {
            return authorUrl;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Cache[ObjectDataSourceTwitter.CacheKeyDependency] = "TwitterFeedCache";

            // shorten cache
            int timeOutSeconds = Utils.RetrieveSecondsForSessionCacheTimeout();
            if (timeOutSeconds > 15)
            {
                timeOutSeconds = 15;
            }
            //ObjectDataSourceTwitter.CacheDuration = timeOutSeconds = timeOutSeconds;
        }
    }
    protected void RepeaterTweet_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        int id = -1;
        Int32.TryParse(e.CommandArgument.ToString(), out id);

        var rec = TwitterUpdateManager.I.Get(new TwitterUpdateQuery() { Id = id }).FirstOrDefault();
        rec.TweetNotRelevant = true;
        TwitterUpdateManager.I.Update(rec);
        Cache[ObjectDataSourceTwitter.CacheKeyDependency] = "TwitterFeedCache";
        RepeaterTweet.DataBind();
        
    }

    protected bool IsAdmin()
    {
        return Utils.CheckUserIsAdmin();
    }
}
