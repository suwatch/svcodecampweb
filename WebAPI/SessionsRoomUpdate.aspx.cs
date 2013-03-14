using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;

public partial class SessionsRoomUpdate : System.Web.UI.Page
{
   

    protected string GetNumberPlannedToAttend(int sessionId)
    {
        return "PlanTAttendCnt: " + sessionId;
    }

    protected string GetSessionTitle(int sessionId)
    {
        var rec = SessionsManager.I.Get(new SessionsQuery() {Id = sessionId});
        return rec[0].Title;

    }

    /// <summary>
    /// this is now really get primary session presenter
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    protected string GetSessionPresenter(int sessionId)
    {
        var x = SessionPresenterManager.I.Get(new SessionPresenterQuery()
                                                  {
                                                      SessionId = sessionId,
                                                      Primary = true,
                                                      WithSpeaker = true
                                                  }).FirstOrDefault();
        return x != null && x.Presenter != null
                   ? string.Format("{0} {1}", x.Presenter.UserFirstName, x.Presenter.UserLastName)
                   : "GetSessionPresenter Did Not Find Presenter";

        //var recSessions = SessionsManager.I.Get(new SessionsQuery() { Id = sessionId });
        //var recAttendees = AttendeesManager.I.Get(new AttendeesQuery() {Id = recSessions[0].Attendeesid});
        //return recAttendees[0].UserFirstName + " " + recAttendees[0].UserLastName;
    }

    protected string GetSessionRoom(int sessionId)
    {
        string retString = string.Empty;
        
        var recSessions = SessionsManager.I.Get(new SessionsQuery() { Id = sessionId });
        if (recSessions[0].LectureRoomsId > 0)
        {
            var recRooms = LectureRoomsManager.I.Get(new LectureRoomsQuery()
                                                         {
                                                             Id = recSessions[0].LectureRoomsId
                                                         });
            if (recRooms != null)
            {
               retString = recRooms[0].Number.ToString();
            }
        }
        return retString + "   ";
    }


    /// <summary>
    /// need to make sure room 0 stays active, otherwise, can't be assigned
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ee"></param>
    protected void ButtonUpdateClick(object sender, EventArgs ee)
    {
        // build session results from textstring
        var sessionsResults= new List<SessionsResult>();

        List<string> pairs = TextBox1.Text.Split(';').ToList();
        foreach (var rec in pairs)
        {
            List<string> sessionPair = rec.Split(',').ToList();
            int sessionId = Convert.ToInt32(sessionPair[0]);
            string roomNumber = sessionPair[1];
            SessionsResult sessionsResult =
                SessionsManager.I.Get(new SessionsQuery {Id = sessionId}).SingleOrDefault();
            sessionsResult.RoomNumberNew = roomNumber;
            sessionsResults.Add(sessionsResult);
        }

        int numRecsUpdate = 0;
        bool successResult = true;
        string errorMessage = string.Empty;
      

        // verify that all the rooms exist
        List<LectureRoomsResult> lectureRoomsResults =
            LectureRoomsManager.I.Get(new LectureRoomsQuery() {Available = true});
        var sb = new StringBuilder();
        foreach (var sessionsResult in sessionsResults)
        {
            var sr = sessionsResult;
            int numRecs = (from data in lectureRoomsResults
                           where data.Number.Equals(sr.RoomNumberNew)
                           select data).ToList().Count;
            if (numRecs == 0)
            {
                sb.Append(sessionsResult.RoomNumberNew);
                sb.Append(",");
                successResult = false;
            }
        }
        if (sb.Length > 0)
        {
            errorMessage = "Invalid Room(s): " + sb.ToString();
        }
        if (successResult)
        {
            // verify that the new list is unique
            int numUniqueNewRooms = (from data in sessionsResults select data.RoomNumberNew).ToList().Distinct().Count();
            if (sessionsResults.Count != numUniqueNewRooms)
            {
                successResult = false;
                errorMessage = "Rooms Must be unique.  Duplicates entered.";
            }
        }

        // update the new rooms
        if (successResult)
        {
            var roomDict = new Dictionary<string, int>();
            foreach (var rec in lectureRoomsResults)
            {
                if (!roomDict.ContainsKey(rec.Number))
                {
                    roomDict.Add(rec.Number,rec.Id);
                }
                else
                {
                    
                }
                
            }

            //Dictionary<string, int> roomDict = (from data in lectureRoomsResults select data).ToDictionary(
            //    a => a.Number, a => a.Id);

            foreach (var sessionsResult in sessionsResults)
            {
                if (!sessionsResult.RoomNumber.Equals(sessionsResult.RoomNumberNew))
                {
                    int roomNumberIdNew = roomDict[sessionsResult.RoomNumberNew];

                    var rec = (SessionsManager.I.Get(new SessionsQuery() { Id = sessionsResult.Id })).FirstOrDefault();
                    rec.LectureRoomsId = roomNumberIdNew;

                    try
                    {
                        // make sure no other rooms have this number set first

                        var recsWithRoom =
                            SessionsManager.I.Get(new SessionsQuery()
                            {
                                LectureRoomsId = rec.LectureRoomsId,
                                SessionTimesId = rec.SessionTimesId,
                                CodeCampYearId = Utils.CurrentCodeCampYear
                            });
                        if (recsWithRoom.Count > 0)
                        {
                            int roomIdUnassigned = roomDict["0"];
                            foreach (SessionsResult sessionsResult1 in recsWithRoom)
                            {
                                sessionsResult1.LectureRoomsId = roomIdUnassigned;
                                SessionsManager.I.Update(sessionsResult1);
                                numRecsUpdate++;
                            }
                        }

                        // finally update real record
                        SessionsManager.I.Update(rec);
                        numRecsUpdate++;

                        errorMessage = string.Format("Successfully Update {0} records", numRecsUpdate);



                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                        successResult = false;
                    }
                }

            }
        }
        if (successResult)
        {
            GridView1.DataBind();
            Label2.Text = "success. updated: " + numRecsUpdate.ToString();
        }
        else
        {
            Label2.Text = errorMessage + numRecsUpdate.ToString();
        }


    }
}