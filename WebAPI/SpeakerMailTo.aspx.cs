using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using aspNetEmail;
using CodeCampSV;
using System.Web.Security;

public partial class SpeakerMailTo : BaseContentPage
{
    private string sessionName;
    private string speakerEmail;
    private string speakerUsername;
    private string speakerName;
    private string attendeeEmail;
    private string attendeeFirstName;
    private string attendeeLastName;
    private string userFirstName;
    private string userLastName;
    private string sessionURL;
    private string sessionTitle;

     protected void Page_PreRender(object sender, EventArgs e)
     {
         SetCaptchaControlLabel("PresenterNameID", speakerName);
         SetCaptchaControlLabel("SessionNameID",sessionName);
         
     }

    protected void Page_Load(object sender, EventArgs e)
    {
        //SetCaptchaControlLabel("LabelStatus", string.Empty);
        if (Request.QueryString["SessionId"] != null)
        {



            int sessionIdTemp = 0;
            string str = Request.QueryString["SessionId"];
            bool good = Int32.TryParse(str, out sessionIdTemp);
            if (good)
            {
                speakerName = Utils.GetUserNameOfSession(sessionIdTemp);
                sessionName = Utils.GetSessionNameOfSession(sessionIdTemp);
                speakerEmail = Utils.GetEmailOfSpeakerFromSession(sessionIdTemp);
                speakerUsername = Utils.GetUserNameFromSessionId(sessionIdTemp);
                

                if (Utils.CheckUserIsAdmin())
                {
                    var speakerRoles = Roles.GetRolesForUser(speakerUsername).ToList();
                    var sb = new StringBuilder();
                    foreach (var rec in speakerRoles)
                    {
                        sb.Append(rec + ":");
                    }

                    SpeakerEmailId.Text = string.Format("<br/>Admin Eyes Only.  Actual Email is: <a href=mailto:{0}>{0}</a><br/><br/>username: {1}<br/>Roles: {2}<br/><br/>",
                        speakerEmail, speakerUsername, sb);
                }

                if (Context.User.Identity.IsAuthenticated)
                {
                    AttendeesODS.DataObjectAttendees attendee = Utils.GetAttendeeByUsername(Context.User.Identity.Name);
                    attendeeEmail = attendee.Email;
                    attendeeFirstName = attendee.Userfirstname;
                    attendeeLastName = attendee.Userlastname;
                }

                string tempSpeakerURL = string.Empty;
                string tempDescr = string.Empty;
                userFirstName = string.Empty;
                userLastName = string.Empty;
                string speakerBio = string.Empty;
                string speakerZipCode = string.Empty;
                string speakerPersonalUrl = string.Empty;
                string speakerPictureUrl = string.Empty;
                DateTime sessionStartTime = DateTime.MinValue;
                Utils.GetSessionInfo(sessionIdTemp, out userFirstName, out userLastName, out tempDescr, out sessionURL,
                                     out tempSpeakerURL, out sessionTitle,
                                     out speakerBio,out speakerPictureUrl,
                                     out speakerZipCode, out speakerPersonalUrl, out sessionStartTime);
                                    



            }
            else
            {
                sessionName = "SessionId Invalid";
                speakerName = "SessionId Invalid";
                speakerEmail = "SessionId Invalid";
            }
        }
    }

    /// <summary>
    /// Set the label text inside the captcha template
    /// </summary>
    /// <param name="labelId"></param>
    /// <param name="labelValue"></param>
    private void SetCaptchaControlLabel(string labelId,string labelValue)
    {
        if (CaptchaUltimateControl1.FindControl(labelId) != null)
        {
            Label label = (Label)
                          (WebControl)
                          CaptchaUltimateControl1.FindControl(labelId);

            if (label != null)
            {
                label.Text = labelValue;
            }
        }

    }

    /// <summary>
    /// Get a control string (either textbox or label
    /// from inside the CaptchaControl's template)
    /// </summary>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    private string GetFromCaptchaControl(string fieldName)
    {

        var wc = (WebControl)
            CaptchaUltimateControl1.FindControl(fieldName);

        string retString = string.Empty;
        if (wc != null)
        {
            if (wc.GetType().ToString().ToLower().Contains("textbox"))
            {
                retString = ((TextBox) wc).Text;
            }
            if (wc.GetType().ToString().ToLower().Contains("label"))
            {
                retString = ((Label)wc).Text;
            }
        }
        return retString;

        //string retString = string.Empty;
        //var textBox =
            
        //    CaptchaUltimateControl1.FindControl(fieldName);

        //if (textBox != null)
        //{
        //    retString = textBox.Text;
        //}
        //return retString;
    }

    protected void CaptchaUltimateControl1_Verifying(object sender, VerifyingEventArgs e)
    {
        if (ConfigurationManager.AppSettings["OverrideCaptcha"].ToLower().Equals("true")
            || Utils.CheckUserIsAdmin())
        {
            e.ForceVerify = true;
        }
    }

    //CaptchaUltimateControl1_Verified

    protected void CaptchaUltimateControl1_Verified(object sender, EventArgs e)
    {
        string completionMessage = string.Empty;
        string sessionNamex = GetFromCaptchaControl("SessionNameID");
        string presenterName = GetFromCaptchaControl("PresenterNameID");
        string textBoxMessageString = GetFromCaptchaControl("TextBoxMessageID");
 
        var sb = new StringBuilder();

        //sb.AppendLine(String.Format("To: {0} ", presenterName ?? string.Empty));
        //sb.AppendLine(String.Format("From: {0} Sent Through Silicon Valley Code Camp", userEmail ?? string.Empty));
        //sb.AppendLine(String.Format("Subject: Your Session {0}", sessionNamex ?? string.Empty));
        sb.AppendLine(" ");
        sb.AppendLine(textBoxMessageString ?? string.Empty);
        sb.AppendLine(" "); 
        sb.AppendLine("------------------------------------------------------- ");
        sb.AppendLine("Note From Code Camp:");
        sb.AppendLine(" ");
        sb.AppendLine(" ");
        sb.AppendLine(String.Format("This note was generated by user {0} pressing the 'Email Speaker Button' on your session: ", attendeeEmail));
        sb.Append(sessionTitle);
        sb.Append(" ");
        sb.Append(sessionURL);
        sb.AppendLine(" ");
        sb.AppendLine(" ");

        // todo: this needs to be person requesting
        sb.Append(String.Format("{0} {1} at email {2} does not have your email.  Please do not press the reply button. ",
            attendeeFirstName,attendeeLastName,attendeeEmail));


        sb.Append("To reply to this person, use the users ");
        sb.Append(String.Format("email address {0}.",attendeeEmail));
        sb.AppendLine(" ");
        sb.AppendLine(" ");
        sb.Append(
            "If you do not want user to be able to send you emails, log into your code camp account, choose 'My Profile' on the left sidebar, and check the box 'Do Not Display Email Speaker On My Session'. ");
        sb.AppendLine(" ");
        sb.AppendLine(" ");
        sb.AppendLine("Thanks for speaking at Silicon Valley Code Camp!");
        

        try
        {
            var msg = new EmailMessage(true, false)
                          {
                              Logging = true,
                              LogOverwrite = false,
                              LogPath = MapPath(string.Empty) + "\\App_Data\\SpeakerSend.log",
                              FromAddress = Utils.GetServiceEmailAddress(),
                              ReplyTo = attendeeEmail,
                              To = speakerEmail,
                              Subject =
                                  String.Format("Code Camp Email From Attendee: {0} {1} Email: {2} on your Session",
                                                userFirstName, userLastName, attendeeEmail),
                              Body = sb.ToString()
                          };

            if (msg.Server.Equals("smtp.gmail.com"))
            {
                var ssl = new AdvancedIntellect.Ssl.SslSocket();
                msg.LoadSslSocket(ssl);
                msg.Port = 587;
            }
            msg.Send();
            
            SetCaptchaControlLabel("LabelStatus", "Message Sent Successfully");
            HyperLinkHome.Visible = true;
            
        }
        catch (Exception ee)
        {
            SetCaptchaControlLabel("LabelStatus", "Message Did Not Send Correctly.  Sorry  Please email info@siliconvalley-codecamp.com for more help." + ee);
        }
    
    
    }

   
    //protected void ButtonAddSubmit2SessionsRole_Click(object sender, EventArgs e)
    //{
    //    Roles.AddUserToRole(speakerUsername, Utils.AddTwoSessionsRoleName);
    //}
    //protected void ButtonAddSubmit3orMoreSessionsRole_Click(object sender, EventArgs e)
    //{
    //    Roles.AddUserToRole(speakerUsername, Utils.AddMoreThanTwoSessionsRoleName);
    //}
}