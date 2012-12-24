using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;
using System.Data.Linq;

public partial class SiteAdminUpdateSessionSpeaker : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        using (var codeCampDataContext = new CodeCampDataContext())
        {
            // first, get what is in table so we don't duplicate
            var sessionsSpeakerDict = codeCampDataContext.SessionPresenter.ToDictionary(a => a.SessionId,
                                                                                        b => b.AttendeeId);



            var sessions =
                codeCampDataContext.Sessions;
            var sessionAttendeeDict = 
                sessions.ToDictionary(session => session.Id, session => session.Attendeesid);
            int cnt = 0;
            foreach (var dictVal in sessionAttendeeDict)
            {
                // see if session 
                if (sessionsSpeakerDict.ContainsKey(dictVal.Key) && sessionsSpeakerDict[dictVal.Key] == dictVal.Value)
                {
                    // value is there so don't re-add
                }
                else
                {
                    var sessionPresenter =
                        new SessionPresenter()
                            {
                                AttendeeId = dictVal.Value,
                                SessionId = dictVal.Key
                            };
                    codeCampDataContext.SessionPresenter.InsertOnSubmit(sessionPresenter);
                    cnt++;
                }
            }
            codeCampDataContext.SubmitChanges(ConflictMode.ContinueOnConflict);
            TextBox1.Text = "Updated records: " + cnt;
        }

    }
}
