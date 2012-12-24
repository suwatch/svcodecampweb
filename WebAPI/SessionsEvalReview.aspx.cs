using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CodeCampSV;
using System.Collections.Generic;

public partial class SessionsEvalReview : BaseContentPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["id"] != null)
            {
                int sessionId = Convert.ToInt32(Request.QueryString["id"].ToString());

                
                Utils utils = new Utils();
                string usernameForSession = Utils.GetUserNameMembershipOfSession(sessionId);
                if (usernameForSession.Equals(Context.User.Identity.Name) || Utils.CheckUserIsSuperUser())
                {
                    LabelSessionId.Text = sessionId.ToString();

                    Log4NetAllManager.I.Insert(new Log4NetAllResult()
                    {
                        Date = DateTime.Now.AddHours(-3),
                        EllapsedTime = 0,
                        ExceptionMessage = "",
                        ExceptionStackTrace = "",
                        Level = "",
                        Logger = "",
                        Message = "SPEAKER_REVIEWED_EVAL",
                        MessageLine1 = sessionId.ToString(CultureInfo.InvariantCulture),
                        MessageLine2 = Context.User.Identity.Name, // user who viewed session
                        Thread = "",
                        UserName = usernameForSession // likely this is the same person who owns the session but not necessarily
                    });



                }
                else
                {
                    Response.Redirect("~/Sessions.aspx");
                }
            }

        }

        if (CodeCampSV.Utils.CheckUserIsAdmin())
        {
           // DetailsView1.Visible = true;
        }

        //RepeaterEvalQuestions.DataBind();
        WebChartControl1.DataBind();
    }

    protected string GetEvalName(bool showEvalName,Guid PKID)
    {
        string retString = "anonymous eval";
        if (showEvalName)
        {
            string username = CodeCampSV.Utils.GetAttendeeUsernameByGUID(PKID.ToString());

            AttendeesODS attendeeODS = new AttendeesODS();
            List<CodeCampSV.AttendeesODS.DataObjectAttendees> li = attendeeODS.GetByUsername(string.Empty, username, false);


            retString = li[0].Userfirstname + " " + li[0].Userlastname + " email: " + li[0].Email;
        }
        return retString;
    }

    protected string GetAttendeeNameFromSessionId(int sessionId)
    {
        return Utils.GetUserNameOfSession(sessionId);
    }

    protected string GetCodeCampTitle()
    {
        return ConfigurationManager.AppSettings["siteTitle"].ToString();
    }

    protected string GetSessionEvalsSubmittedCount(int sessionId)
    {
        SessionEvalsODS sessionEvalsODS = new SessionEvalsODS();
        List<CodeCampSV.SessionEvalsODS.DataObjectSessionEvals> evalsList =  sessionEvalsODS.GetBySessionId(string.Empty,sessionId);
        return evalsList.Count.ToString();
    }
}
