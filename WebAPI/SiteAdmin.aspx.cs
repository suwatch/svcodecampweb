using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CodeCampSV;
using Gurock.SmartInspect;

public partial class SiteAdmin : BaseContentPage
{
    protected Gurock.SmartInspect.Session LogSession
    {
        get
        {
            return SiAuto.Si[Session.SessionID];
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //LogSession.EnterMethod(this, "Page_Load");

    }
    protected void ButtonReadOptOutConvertToAttendee_Click(object sender, EventArgs e)
    {
        int cnt = 0;
        var recs = EmailOptOutManager.I.GetAll();
        foreach (var rec in recs)
        {
            if (!String.IsNullOrEmpty(rec.Email))
            {
                var attendee = AttendeesManager.I.Get(new AttendeesQuery {Email = rec.Email}).FirstOrDefault();
                if (attendee != null)
                {
                    attendee.EmailSubscription = 1;
                    attendee.EmailSubscriptionStatus = string.Format("{0} {1}", rec.Comment, rec.DateAdded.ToShortDateString());
                    AttendeesManager.I.Update(attendee);
                    cnt++;
                }
            }
        }
        LabelStatus.Text = "Processed " + cnt.ToString() + " records to EmailSubscripiton.";


    }
}
