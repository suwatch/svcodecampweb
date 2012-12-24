using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Data.SqlClient;
using CodeCampSV;
using System.Text;
using System.Linq;

public partial class AgendaUpdate : BaseContentPage
{
    List<DataObjectAgendaUpdateInfo> liAgendaUpdateInfo;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LabelCodeCampYearId.Text = Utils.GetCurrentCodeCampYear().ToString();
            if (Request.QueryString["SessionTimeId"] != null)
            {
                DropDownListSessionTimes.SelectedValue = Request.QueryString["SessionTimeId"];
                
               
            }
        }

        liAgendaUpdateInfo = CodeCampSV.Utils.GetListAgendaUpdateInfo();
        Button1.Enabled = Utils.CheckUserIsSuperUser();
    }

    
    protected void DropDownListSessionTimes_SelectedIndexChanged(object sender, EventArgs e)
    {
        int sessionTimeId = Convert.ToInt32(DropDownListSessionTimes.SelectedValue);
        GridViewRooms.DataBind();
    }

    protected void GridViewRooms_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int sessionTimeId = Convert.ToInt32(DropDownListSessionTimes.SelectedValue);
        string commandArg = (string) e.CommandArgument;
        string roomIdStr = commandArg.Substring(0,commandArg.IndexOf('^'));
        int roomId = Convert.ToInt32(roomIdStr);
        Cache.Remove(CodeCampSV.Utils.CacheAgendaUpdateInfo);
        if (e.CommandName.Equals("AssignSession"))
        {
            string redirectString = "~/AgendaPicker.aspx?sessiontimeid=" +
                sessionTimeId.ToString() +
                "&roomid=" + roomId.ToString();
            Response.Redirect(redirectString);
        }
        else if (e.CommandName.Equals("ClearSession"))
        {
            Utils.AgendaUpdateSession(-1, roomId, sessionTimeId);
            string redirectString = "~/AgendaUpdate.aspx?SessionTimeId=" +
               sessionTimeId.ToString();
            Response.Redirect(redirectString);
        }
        
        GridViewRooms.DataBind();
    }

    protected string GetSessionTitleByRoomId(int roomId)
    {
        string retString = string.Empty;
        int sessionId = this.GetSessionIdFromRoomId(roomId);
        if (sessionId >= 0)
        {
            foreach (DataObjectAgendaUpdateInfo agendaUpdateInfo in liAgendaUpdateInfo)
            {
                if (agendaUpdateInfo.SessionId == sessionId)
                {
                    retString = agendaUpdateInfo.SessionTitle;
                    break;
                }
            }
        }
        return retString;
    }

   
    protected string GetPresenterByRoomId(int roomId)
    {
        string retString = string.Empty;
        int sessionId = this.GetSessionIdFromRoomId(roomId);
        if (sessionId >= 0)
        {
            foreach (DataObjectAgendaUpdateInfo agendaUpdateInfo in liAgendaUpdateInfo)
            {
                if (agendaUpdateInfo.SessionId == sessionId)
                {
                    retString = agendaUpdateInfo.SessionAuthor;
                    break;
                }
            }
        }
        return retString;
    }

    protected string GetInterestedByRoomId(int roomId)
    {
        string retString = string.Empty;
        int sessionId = this.GetSessionIdFromRoomId(roomId);
        if (sessionId >= 0)
        {
            foreach (DataObjectAgendaUpdateInfo agendaUpdateInfo in liAgendaUpdateInfo)
            {
                if (agendaUpdateInfo.SessionId == sessionId)
                {
                    retString = "Interested: " + agendaUpdateInfo.Interested.ToString() + " Attending: " +
                        agendaUpdateInfo.WillAttend.ToString();
                    break;
                }
            }
        }
        return retString;
    }


   
    private int GetSessionIdFromRoomId(int roomId)
    {
        int sessionTimeId = Convert.ToInt32(DropDownListSessionTimes.SelectedValue);
        int sessionId = -1;
        foreach (DataObjectAgendaUpdateInfo agendaUpdateInfo in liAgendaUpdateInfo)
        {
            if (agendaUpdateInfo.LectureRoomId == roomId &&
                agendaUpdateInfo.SessionTimesId == sessionTimeId)
            {
                sessionId = agendaUpdateInfo.SessionId;
                break;
            }
        }
        return sessionId;
    }

    protected bool IsImageVisible()
    {
        return CheckBoxShowRoomPictures.Checked;
    }


    protected void CheckBoxShowRoomPictures_CheckedChanged(object sender, EventArgs e)
    {
        GridViewRooms.DataBind();
    }

    protected void ButtonUnassignAllRooms_Click(object sender, EventArgs e)
    {
        Cache.Remove(CodeCampSV.Utils.CacheAgendaUpdateInfo);
        CodeCampSV.Utils.UnAssignAllRooms();
        GridViewRooms.DataBind();

        int sessionTimeId = Convert.ToInt32(DropDownListSessionTimes.SelectedValue);
        string redirectString = "~/AgendaUpdate.aspx?SessionTimeId=" +
               sessionTimeId.ToString();
        Response.Redirect(redirectString);

    }


    protected void ButtonAutoAssignRooms_Click(object sender, EventArgs e)
    {

        // need to add logic skipping all assigned rooms and sessions





        var sb = new StringBuilder();
        var sessionTimes =
            SessionTimesManager.I.Get(new SessionTimesQuery()
            {
                CodeCampYearId = Utils.CurrentCodeCampYear
            }).OrderBy(a => a.StartTime).ToList();


       

        foreach (var sessionTime in sessionTimes)
        {
            sb.AppendLine(String.Format("Session Time {0}", sessionTime.StartTimeFriendly));

           
            // get the first session slot, order by planned to attend
            var sessionsAtTimeSlot = SessionsManager.I.Get(new SessionsQuery()
            {
                SessionTimesId = sessionTime.Id,
                RoomIds = new List<int>() { Utils.RoomNotAssigned }
            }).OrderByDescending(a => a.PlanAheadCountInt).ToList();

            // get list of rooms in use already
            List<int> roomsIdsInUse = SessionsManager.I.Get(new SessionsQuery()
            {
                SessionTimesId = sessionTime.Id,
                SkipRoomIds = new List<int>() { Utils.RoomNotAssigned }
            }).Where(a=>a.LectureRoomsId.HasValue).Select(a => a.LectureRoomsId.Value).ToList();

            // get a list of rooms ordered by size
            var roomListTopDown = LectureRoomsManager.I.Get(new LectureRoomsQuery()
            {
                Available = true,
                SkipIds = roomsIdsInUse
            }).OrderByDescending(a => a.Capacity).ToList();

            // run thru sessions that are most popular first and put in biggest room
            for (int i = 0; i < sessionsAtTimeSlot.Count; i++)
            {
                sessionsAtTimeSlot[i].LectureRoomsId = roomListTopDown[i].Id;
                SessionsManager.I.Update(sessionsAtTimeSlot[i]);
                sb.AppendLine(String.Format("  Session: {0}  Room {1} / Capacity: {2}",
                    sessionsAtTimeSlot[i].Title.Substring(0, Math.Min(sessionsAtTimeSlot[i].Title.Length, 25)),
                    roomListTopDown[i].Number, roomListTopDown[i].Capacity));
            }
        }
    }

    protected void ButtonUnassignAllTimes_Click(object sender, EventArgs e)
    {
        Cache.Remove(CodeCampSV.Utils.CacheAgendaUpdateInfo);
        CodeCampSV.Utils.UnAssignAllTimes();
        GridViewRooms.DataBind();

        int sessionTimeId = Convert.ToInt32(DropDownListSessionTimes.SelectedValue);
        string redirectString = "~/AgendaUpdate.aspx?SessionTimeId=" +
               sessionTimeId.ToString();
        Response.Redirect(redirectString);

    }
}

