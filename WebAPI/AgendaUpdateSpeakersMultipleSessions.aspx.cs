using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;
using System.Text;

public partial class AgendaUpdateSpeakersMultipleSessions : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LabelCodeCampYearId.Text = Utils.CurrentCodeCampYear.ToString();
            LoadAttendees();
        }
    }


    protected string GetRoomAndTime(int sessionId)
    {
        var rec = SessionsManager.I.Get(new SessionsQuery() { Id = sessionId, WithSchedule = true }).FirstOrDefault();

        string s = String.Format("Room {0}   Time {1}", rec.RoomNumber, rec.SessionTime);
        return s;

    }

    protected string GetRoomNumberWithCapacity(string number)
    {
        var result = LectureRoomsManager.I.Get(new LectureRoomsQuery() { Number = number }).FirstOrDefault();
        return String.Format("{0}, Holds {1}", result.Number, result.Capacity);

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        LoadAttendees();
    }

    private void LoadAttendees()
    {
        // get list of speakers with more than one session
        Dictionary<int, int> recs = Utils.GetSpeakerIdsWithMultipleSessions(Utils.CurrentCodeCampYear);

        // get list of individual speakers
        List<int> attendeesIdList = recs.Select(a => a.Value).Distinct().ToList();

        List<int> attendeesIdTargetsList = new List<int>();
        foreach (var attendeeId in attendeesIdList)
        {
            GetEm(recs, attendeesIdTargetsList, attendeeId);
        }

        if (attendeesIdTargetsList.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var id in attendeesIdTargetsList)
            {
                sb.Append(id.ToString());
                sb.Append(",");
            }
            LabelAttendeesId.Text = sb.ToString().Substring(0, sb.ToString().Length - 1);
            GridViewSpeakerTargets.DataBind();
        }
        //// all session times current code camp year
        //var sessionTimes =
        //   SessionTimesManager.I.Get(new SessionTimesQuery()
        //   {
        //       CodeCampYearId = Utils.CurrentCodeCampYear
        //   }).OrderBy(a => a.StartTime).ToList();

        //foreach (var sessionTime in sessionTimes)
        //{
        //    sb.AppendLine(String.Format("Session Time {0}", sessionTime.StartTimeFriendly));



        //    // Get List Of All Sessions With Speakers
        //    // get the first session slot, order by planned to attend
        //    var sessionsAtTimeSlot = SessionsManager.I.Get(new SessionsQuery()
        //    {
        //        SessionTimesId = sessionTime.Id,
        //        RoomIds = new List<int>() { Utils.RoomNotAssigned }
        //    }).OrderByDescending(a => a.PlanAheadCountInt).ToList();
        //}
    }

    private static void GetEm(Dictionary<int, int> recs, List<int> attendeesIdTargetsList, int attendeeId)
    {
        //var speakerResult = AttendeesManager.I.Get(new AttendeesQuery() { Id = attendeeId }).FirstOrDefault();
        var sessionIds = recs.Where(a => a.Value == attendeeId).Select(a => a.Key).Distinct().ToList();
        var speakerSessions = SessionsManager.I.Get(new SessionsQuery()
        {
            Ids = sessionIds,
            WithSchedule = true,
            RoomIds = new List<int>() { Utils.RoomNotAssigned }
        }).
            Where(a => a.SessionTimesResult != null).OrderBy(a => a.SessionTimesResult.StartTime.Value).ToList();

        // calculate smallest gap between sessions (there should be at least 2 for every speaker or we should not be here)
        for (int i = 0; i < speakerSessions.Count - 1; i++)
        {
            TimeSpan ts = speakerSessions[i + 1].SessionTimesResult.StartTime.Value.Subtract(speakerSessions[i].SessionTimesResult.StartTime.Value);
            if (ts.Hours < 2)
            {
                // we are adding the attendeesId so the [0] is not a bug (thought it may smell like one)
                if (!attendeesIdTargetsList.Contains(speakerSessions[0].Attendeesid))
                {
                    attendeesIdTargetsList.Add(speakerSessions[0].Attendeesid);
                }
            }
        }
    }
    protected void GridViewSpeakerTargets_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int attendeeId = Convert.ToInt32(e.CommandArgument);

        var sessions = SessionsManager.I.Get(new SessionsQuery()
        {
            Attendeesid = attendeeId,
            WithSchedule = true,
            RoomIds = new List<int>() { Utils.RoomNotAssigned }
        });

        var ids = sessions.Select(a => a.Id);
        StringBuilder sb = new StringBuilder();
        foreach (var id in ids)
        {
            sb.Append(id.ToString());
            sb.Append(",");
        }
        LabelSessionIds.Text = sb.ToString().Substring(0, sb.ToString().Length - 1);

        GridViewSessions.DataBind();

       

       
    }
    //protected void ButtonAssignToRoom_Click(object sender, EventArgs e)
    //{

    //}
    protected void ButtonAssignToRoom_Click1(object sender, EventArgs e)
    {
        string roomNumber = DropDownListActiveRooms.SelectedValue;

        Dictionary<string, int> roomDict = (LectureRoomsManager.I.Get(new LectureRoomsQuery() { Available = true })).ToDictionary(k => k.Number, v => v.Id);

        List<String> idsListString = TextBoxSessionIdsForAssignment.Text.Split(',').ToList();
        List<int> ids = new List<int>();
        foreach (var idString in idsListString)
        {
            int id = Convert.ToInt32(idString);
            ids.Add(id);
        }

        var sessions = SessionsManager.I.Get(new SessionsQuery() { Ids = ids });
        foreach (var session in sessions)
        {
            session.LectureRoomsId = roomDict[roomNumber];
            SessionsManager.I.Update(session);
        }

        TextBoxSessionIdsForAssignment.Text = "";

        LoadAttendees();
    }
}