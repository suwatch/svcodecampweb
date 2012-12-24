using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using CodeCampSV;

public partial class Speakers : BaseContentPage
{
    private string speakerUsername;

    protected void Page_Init(object sender, EventArgs e)
    {
        LabelCodeCampYearId.Text = Utils.GetCurrentCodeCampYear().ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        

        int cacheTime = Utils.RetrieveSecondsForSessionCacheTimeout();
        ObjectDataSourceAllPresenters.CacheDuration = cacheTime;
        ObjectDataSourceAllPresenters.EnableCaching = true;

        ObjectDataSourceBySession.CacheDuration = cacheTime;
        ObjectDataSourceBySession.EnableCaching = true;


        if (Request.QueryString["id"] != null)
        {
            string str = Request.QueryString["id"];
            int sessionId;
            bool good = Int32.TryParse(str, out sessionId);
            if (!good)
            {
                Response.Redirect("~/Speakers.aspx");
            }
            Repeater1.DataSourceID = "ObjectDataSourceBySession";
        }

        if (Request.QueryString["AttendeeId"] != null)
        {
            string str = Request.QueryString["AttendeeId"];
            int attendeeId;
            bool good = Int32.TryParse(str, out attendeeId);
            if (!good)
            {
                Response.Redirect("~/Speakers.aspx");
            }

            // verify AttendeeId is actually a speaker
            if (Utils.CheckAttendeeIdIsSpeaker(attendeeId))
            {
                Repeater1.DataSourceID = "ObjectDataSourceByAttendeeId";

                if (Utils.CheckUserIsAdmin())
                {
                    AdminsOnlyId.Visible = true;
                    SpeakerShowRolesLabel.Visible = true;
                    ButtonAddSubmit2SessionsRole.Visible = true;
                    ButtonAddSubmit3orMoreSessionsRole.Visible = true;

                    speakerUsername = Utils.GetUsernameFromAttendeeId(attendeeId);

                    var speakerRoles = Roles.GetRolesForUser(speakerUsername).ToList();
                    var sb = new StringBuilder(String.Format("<br>Current Roles for this speaker username {0}:", speakerUsername));
                    foreach (var rec in speakerRoles)
                    {
                        sb.Append("<br/>&nbsp;&nbsp;" +   rec );
                    }
                    sb.Append("<br/>");
                    SpeakerShowRolesLabel.Text = sb.ToString();
                }

            }
            else
            {
                Response.Redirect("~/Speakers.aspx");
            }
        }
    }

    protected static string GetUserWebSite(object userWebSiteObject)
    {
        string retString = string.Empty;
        try
        {
            if (!userWebSiteObject.Equals(DBNull.Value))
            {
                var userWebSite = (string) userWebSiteObject;
                if (userWebSite.ToLower().StartsWith("http://"))
                {
                    retString = userWebSite;
                }
                else
                {
                    retString = "http://" + userWebSite;
                }
                return retString;
            }
        }
        catch (Exception e1)
        {
            throw new ApplicationException(e1.ToString());
        }
        return retString;
    }

    protected static string GetSessionId(Guid PKID)
    {
        int sessionId = -1;
        var sessionsODS = new SessionsODS();
        List<SessionsODS.DataObjectSessions> liSessions = sessionsODS.GetByPKID(PKID);
        if (liSessions.Count > 0)
        {
            sessionId = liSessions[0].Id;
        }

        return sessionId.ToString();
    }

    protected void ButtonAddSubmit2SessionsRole_Click(object sender, EventArgs e)
    {
        Roles.AddUserToRole(speakerUsername, Utils.AddTwoSessionsRoleName);
    }

    protected void ButtonAddSubmit3orMoreSessionsRole_Click(object sender, EventArgs e)
    {
        Roles.AddUserToRole(speakerUsername, Utils.AddMoreThanTwoSessionsRoleName);
    }

    protected void ButtonAddSubmitSession_Click(object sender, EventArgs e)
    {
        Roles.AddUserToRole(speakerUsername, Utils.SubmitSessionRoleName);
    }

    protected void ButtonAddSubmit4Sessions_Click(object sender, EventArgs e)
    {
        Roles.AddUserToRole(speakerUsername, Utils.AddFourSessionsRoleName);
    }

    protected void ButtonAddSubmit3Sessions_Click(object sender, EventArgs e)
    {
        Roles.AddUserToRole(speakerUsername, Utils.AddThreeSessionsRoleName);
    
    }
}