using System;
using System.Web.UI;
using CodeCampSV;

public partial class SessionWiki : BaseContentPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["SessionId"] != null)
        {
            int sessionId = Int32.Parse(Request.QueryString["SessionId"]);
            HyperLinkReturnToSession.NavigateUrl = string.Format("~/Sessions.aspx?ForceSortBySessionTime=true&id={0}",
                                                                 sessionId);
            string firstName = "Lynn";
            string lastName = "Langit";
            string description =
                "Lynn will talk about SQL Server 2008 Data Mining. In a mostly demo-driven presentation, Lynn will explain the what, why and how of predictive analytics in SSAS. ";
            string sessionURL = "http://www.siliconvalley-codecamp.com/Sessions.aspx?id=11";
            string speakerURL = "http://www.siliconvalley-codecamp.com/Speakers.aspx?id=11";
            string sessionTitle = "Data Mining for .NET Developers";
            DateTime sessionStartTime;
            string speakerBio;
            string speakerPictureURL;
            string speakerZipCode;
            string speakerPersonalUrl;
            bool success = Utils.GetSessionInfo(sessionId, out firstName, out lastName, out description, out sessionURL,
                                                out speakerURL, out sessionTitle, out speakerBio, out speakerPictureURL,
                                                out speakerZipCode,
                                                out speakerPersonalUrl, out sessionStartTime);
            if (success)
            {
                IDTitle.Text = sessionTitle;
                IDPresenter.Text = string.Format("{0} {1}", firstName, lastName);
                IDSessionDescription.Text = Utils.ConvertEncodedHTMLToRealHTML(description);


                //  not sure why this didn't work PGK
                //IDPresenterImage.ImageUrl = speakerPictureURL;
                //IDPresenterImage.AlternateText = IDPresenter.Text;


                //IDSessionURL.Text = sessionURL;
                //IDSpeakerURL.Text = speakerURL;
            }

            //WetPaintWebControl1.CellID = sessionId.ToString();
            //WetPaintWebControl1.UserDisplayName = string.Format("{0} {1}", firstName, lastName);
            //WetPaintWebControl1.UserID = firstName;
            //WetPaintWebControl1.UserUrl = speakerURL;
            //WetPaintWebControl1.LoginPageUrl = "sessions.aspx";
        }
    }
}