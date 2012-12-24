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
using CodeCampSV;
using System.Collections.Generic;


public partial class AgendaPicker : BaseContentPage
{
    //int RoomIdFromPrevPage = -1;
    //int SessionTimeIdFromPrevPage = -1;

    public int RoomIdFromPrevPage
    {
        get { return (int)(ViewState["RoomIdFromPrevPage"] ?? 0); }
        set { ViewState["RoomIdFromPrevPage"] = value; }
    }

    public int SessionTimeIdFromPrevPage
    {
        get { return (int)(ViewState["SessionTimeIdFromPrevPage"] ?? 0); }
        set { ViewState["SessionTimeIdFromPrevPage"] = value; }
    }


    List<DataObjectAgendaUpdateInfo> _liAgendaUpdateInfo;

    protected Dictionary<int, string> SessionTimesDictionary;
    protected Dictionary<int, string> RoomDictionary;

    protected void Page_PreRender(object sender,EventArgs e)
    {
        if (!IsPostBack)
        {
            // set dropdown 
            ListItem listItem = DropDownListSessionTimes.Items.FindByValue(SessionTimeIdFromPrevPage.ToString());
            if (listItem != null)
            {
                DropDownListSessionTimes.SelectedValue = listItem.Value;
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // load up the dictionaries from cache or other
        var stODS = new SessionTimesODS();
        List<SessionTimesODS.DataObjectSessionTimes> stList = stODS.GetAllSessionTimes();
        SessionTimesDictionary = new Dictionary<int, string>(stList.Count);
        foreach (SessionTimesODS.DataObjectSessionTimes st in stList)
        {
            SessionTimesDictionary.Add(st.Id, st.Description);
        }
        

        LabelCodeCampYearId.Text = Utils.CurrentCodeCampYear.ToString();

        _liAgendaUpdateInfo = CodeCampSV.Utils.GetListAgendaUpdateInfo();

        // need all three
        //string redirectString = "~/AgendaPicker.aspx?sessiontimeid=" +
        //    sessionTimeId.ToString() + 
        //    "&roomid=" + roomId.ToString();

        if (!IsPostBack)
        {
            if (Request.QueryString["sessiontimeid"] != null &&
                Request.QueryString["roomid"] != null)
            {
                RoomIdFromPrevPage = Convert.ToInt32(Request.QueryString["roomid"]);
                SessionTimeIdFromPrevPage = Convert.ToInt32(Request.QueryString["sessiontimeid"]);

                SetRoomIdAndSessionTimesId();
            }
            else
            {
                throw new ApplicationException("AgendaPicker Needs 2 Request Params");
            }
            DropDownListSessionTimes.DataBind(); // not sure if need this but...
        }
      

    }

    private void SetRoomIdAndSessionTimesId()
    {
        HyperLinkAgenda.NavigateUrl = "~/AgendaUpdate.aspx?SessionTimeId=" + SessionTimeIdFromPrevPage;

       

        var lrODS = new LectureRoomsODS();
        List<LectureRoomsODS.DataObjectLectureRooms> lrList = lrODS.GetAllLectureRooms();
        RoomDictionary = new Dictionary<int, string>(lrList.Count);
        foreach (LectureRoomsODS.DataObjectLectureRooms lr in lrList)
        {
            RoomDictionary.Add(lr.Id, lr.Number);
        }

        LabelRoom.Text = RoomDictionary[RoomIdFromPrevPage];
        LabelTime.Text = SessionTimesDictionary[SessionTimeIdFromPrevPage];

       

    }

    protected void DropDownListSessionTimes_SelectedIndexChanged(object sender, EventArgs e)
    {
        int sessionTimeId = Convert.ToInt32(DropDownListSessionTimes.SelectedValue);


        SessionTimeIdFromPrevPage = sessionTimeId;
        SetRoomIdAndSessionTimesId();



    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
         
        if (e.CommandArgument != null)
        {
            try
            {
                string commandArg = (string)e.CommandArgument;
                int sessionId = Convert.ToInt32(commandArg);


                // assign room and timeslot to session.
                // first, make sure room and timeslot are not on another session.  if they
                // are, then unselect them.

                CodeCampSV.Utils.AgendaUpdateSession(sessionId, RoomIdFromPrevPage, SessionTimeIdFromPrevPage);
                Cache.Remove(CodeCampSV.Utils.CacheAgendaUpdateInfo);
                CodeCampSV.Utils.CacheClear("sessions");

                GridView1.DataBind();
            }
            catch (Exception eeex)
            {
                throw new ApplicationException(eeex.ToString());
            }
        }


        //int roomId = Convert.ToInt32(commandArg.Substring(0, commandArg.IndexOf('^')));
        //int sessionTimeId = Convert.ToInt32(commandArg.Substring(commandArg.IndexOf('^') + 1));

        //

    }

    protected string GetPresenterFromSessionId(int sessionId)
    {
        string retString = string.Empty;
        foreach (DataObjectAgendaUpdateInfo aui in _liAgendaUpdateInfo)
        {
            if (aui.SessionId == sessionId)
            {
                retString = aui.SessionAuthor;
                break;
            }
        }

        return retString;
    }

    protected string GetInterestFromSessionId(int sessionId)
    {
        string retString = string.Empty;
        foreach (DataObjectAgendaUpdateInfo aui in _liAgendaUpdateInfo)
        {
            if (aui.SessionId == sessionId)
            {
                retString = "Interested: " + aui.Interested.ToString() + " Attending: " +
                        aui.WillAttend.ToString();
                break;
            }
        }

        return retString;
    }

    protected string GetTest(int iii)
    {
        return iii.ToString();
    }

    /// <summary>
    /// was:  <%--Text='<%# (string) SessionTimesDictionary[(int) Eval("SessionTimesId")] %>'>--%>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    protected string GetSessiontimesFromDictionary(int id)
    {
        string ret = string.Empty;
        if (SessionTimesDictionary != null && SessionTimesDictionary.ContainsKey(id))
        {
            ret = SessionTimesDictionary[id];
        }
        return ret;
    }

    /// <summary>
    /// was <%--Text='<%# (string) RoomDictionary[(int) Eval("LectureRoomsId")] %>'    --%>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    protected string GetRoomFromDictionary(int id)
    {
        string ret = string.Empty;
        if (RoomDictionary != null && RoomDictionary.ContainsKey(id))
        {
            ret = RoomDictionary[id];
        }
        return ret;
    }
}
