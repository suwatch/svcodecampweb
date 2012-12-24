using System;
using System.Text;
using System.Threading;
using System.Web.Security;
using System.Web.UI;
using aspNetEmail;
using CodeCampSV;

public partial class PasswordIssues : BaseContentPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Context.User.Identity.IsAuthenticated)
        {
            ChangePassword1.Visible = true;
            //PasswordRecovery1.Visible = false;
            PasswordRecovery2.Visible = false;
        }
        else
        {
            ChangePassword1.Visible = false;
            //PasswordRecovery1.Visible = true;
            PasswordRecovery2.Visible = true;
        }
    }

    protected void ButtonRecoverPassword_Click(object sender, EventArgs e)
    {
        string emailAddress = TextBox1.Text;
        // Get username from email
        string username = Utils.GetUsernameFromEmail(TextBox1.Text);
        if (String.IsNullOrEmpty(username))
        {
            LabelStatus.Text = "Email Address Not Found. Please Register.";
        }
        else
        {
            MembershipUser mu = Membership.GetUser(username);
            String newPassword = mu.ResetPassword();

            var msg = new EmailMessage(true, false);
            msg.Logging = false;
            msg.LogOverwrite = false;
            msg.LogPath = Context.Server.MapPath(String.Empty) + "\\App_Data\\EmailPasswordRecovery.log";
            msg.FromAddress = Utils.GetServiceEmailAddress();
            msg.To = emailAddress;
            msg.Subject = "Your New Password For http://www.siliconvalley-codecamp.com";

            if (msg.Server.Equals("smtp.gmail.com"))
            {
                var ssl = new AdvancedIntellect.Ssl.SslSocket();
                msg.LoadSslSocket(ssl);
                msg.Port = 587;
            }

            var sb = new StringBuilder();
            sb.AppendLine(
                String.Format("Please log in to your codecamp account ({0}) with the new password: {1}",
                              username, newPassword));
            sb.AppendLine(" ");
            sb.AppendLine("We suggest that after you log in, you change your password to something");
            sb.AppendLine("you will likely remember.  We store your password in an encrypted format which is");
            sb.AppendLine("why we are unable to send you your original password.");
            sb.AppendLine(" ");
            sb.AppendLine("We are looking forward to seeing you!");
            sb.AppendLine(" ");
            sb.AppendLine("Best Regards,");
            sb.AppendLine("The Code Camp Volunteers");
            sb.AppendLine("http://www.siliconvalley-codecamp.com");

            msg.Body = sb.ToString();

            msg.Send();
            LabelStatus.Text = "Password Sent to your registered email account.";
        }
    }

}