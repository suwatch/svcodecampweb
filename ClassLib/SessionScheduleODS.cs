using System;
using System.Collections.Generic;
using System.Web.UI;

using System.Data.SqlClient;
using System.ComponentModel;
using System.Configuration;
using System.Data;

namespace CodeCampSV
{
    [DataObject()]
    public class Appointments
    {
        [DataObjectMethod(DataObjectMethodType.Select,true)]
        public List<AppointmentInfo> GetAppointmentsDateUser(DateTime aptDate, String userName)
        {
            List<AppointmentInfo> listAppointmentInfo = new List<AppointmentInfo>();
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();
                try
                {
                    String SqlSelect = @"
                        SELECT 
                          dbo.Sessions.title,
                          dbo.Sessions.description,
                          dbo.LectureRooms.Number,
                          dbo.LectureRooms.Description,
                          dbo.SessionTimes.StartTime,
                          dbo.SessionTimes.StartTimeFriendly,
                          dbo.SessionTimes.EndTime,
                          dbo.SessionTimes.EndTimeFriendly,
                          dbo.Sessions.id
                        FROM
                          dbo.SessionAttendee
                          INNER JOIN dbo.Sessions ON (dbo.SessionAttendee.sessions_id = dbo.Sessions.id)
                          INNER JOIN dbo.LectureRooms ON (dbo.Sessions.LectureRoomsId = dbo.LectureRooms.id)
                          INNER JOIN dbo.SessionTimes ON (dbo.Sessions.SessionTimesId = dbo.SessionTimes.id)
                        WHERE
                          (dbo.SessionAttendee.interestlevel = 3) AND 
                          (dbo.SessionAttendee.attendees_username = @UserGuid)
                        ";
                    using (SqlCommand sqlCommand = new SqlCommand(SqlSelect, sqlConnection))
                    {
                        sqlCommand.Parameters.Add("@UserGuid", SqlDbType.UniqueIdentifier).Value = new Guid("1578B3EC-7FF7-4798-BEAE-A0EBAC5EB226");
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string title = reader.IsDBNull(0) ? String.Empty : reader.GetString(0);
                                string description = reader.IsDBNull(1) ? String.Empty : reader.GetString(1);
                                string lectureRoomNumber = reader.IsDBNull(2) ? String.Empty : reader.GetString(2);
                                string lectureRoomDescription = reader.IsDBNull(3) ? String.Empty : reader.GetString(3);
                                DateTime startTime = reader.IsDBNull(4) ? DateTime.Now : reader.GetDateTime(4);
                                string startTimeFriendly = reader.IsDBNull(5) ? String.Empty : reader.GetString(5);
                                DateTime endTime = reader.IsDBNull(6) ? DateTime.Now : reader.GetDateTime(6);
                                string endTimeFriendly = reader.IsDBNull(7) ? String.Empty : reader.GetString(7);
                                int sessionId = reader.IsDBNull(8) ? 0 : reader.GetInt32(8);

                                listAppointmentInfo.Add(new AppointmentInfo(title, startTime, endTime));
                            }
                        }
                    }
                }
                catch (Exception ee)
                {
                    String error = ee.ToString();
                }
            }
            return listAppointmentInfo;
        }


        public class AppointmentInfo
        {
            public AppointmentInfo(String id, DateTime startTime, DateTime endTime, String subject)
            {
                this.id = id;
                this.start = startTime;
                this.end = endTime;
                this.subject = subject;
            }

            private string id;
            private string subject;
            private DateTime start;
            private DateTime end;
            public string ID
            {
                get { return id; }
            }
            public string Subject
            {
                get { return subject; }
                set { subject = value; }
            }
            public DateTime Start
            {
                get { return start; }
                set { start = value; }
            }
            public DateTime End
            {
                get { return end; }
                set { end = value; }
            }
            private AppointmentInfo()
            {
                this.id = Guid.NewGuid().ToString();
            }
            public AppointmentInfo(string subject, DateTime start, DateTime end)
                : this()
            {
                this.subject = subject;
                this.start = start;
                this.end = end;
            }
            //public AppointmentInfo(Appointment source)
            //    : this()
            //{
            //    CopyInfo(source);
            //}
            //public void CopyInfo(Appointment source)
            //{
            //    subject = source.Subject;
            //    start = source.Start;
            //    end = source.End;
            //}
        }
    }


}
    //public partial class DefaultCS : Page
    //{
    //    private const string AppointmentsKey = "Telerik.Web.Examples.Scheduler.BindToList_Apts";
    //    private List<AppointmentInfo> appointments
    //    {
    //        get
    //        {
    //            List<AppointmentInfo> sessApts = Session[AppointmentsKey] as List<AppointmentInfo>;
    //            if (sessApts == null)
    //            {
    //                sessApts = new List<AppointmentInfo>();
    //                Session[AppointmentsKey] = sessApts;
    //            }
    //            return sessApts;
    //        }
    //    }
    //    private void Page_Load(object sender, System.EventArgs e)
    //    {
    //        if (!IsPostBack)
    //        {
    //            Session.Remove(AppointmentsKey);
    //            DateTime now = DateTime.UtcNow;
    //            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
    //            appointments.Add(new AppointmentInfo("Appointment #1", now, now.AddHours(1)));
    //        }
    //        RadScheduler1.DataSource = appointments;
    //    }
    //    protected void RadScheduler1_AppointmentInsertCommand(object sender, SchedulerCancelEventArgs e)
    //    {
    //        appointments.Add(new AppointmentInfo(e.Appointment));
    //    }
    //    protected void RadScheduler1_AppointmentUpdateCommand(object sender, AppointmentUpdateCommandEventArgs e)
    //    {
    //        AppointmentInfo ai = FindById(e.ModifiedAppointment.ID);
    //        ai.CopyInfo(e.ModifiedAppointment);
    //    }
    //    protected void RadScheduler1_AppointmentDeleteCommand(object sender, SchedulerCancelEventArgs e)
    //    {
    //        appointments.Remove(FindById(e.Appointment.ID));
    //    }
    //    private AppointmentInfo FindById(string ID)
    //    {
    //        foreach (AppointmentInfo ai in appointments)
    //        {
    //            if (ai.ID == ID)
    //            {
    //                return ai;
    //            }
    //        }
    //        return null;
    //    }
    //}
//}