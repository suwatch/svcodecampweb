//  Copyright  2006, Peter Kellner, 73rd Street Associates
//  All rights reserved.
//  http://PeterKellner.net
//
//
//  - Neither Peter Kellner, nor the names of its
//  contributors may be used to endorse or promote products
//  derived from this software without specific prior written
//  permission.
//
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
//  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
//  FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
//  COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
//  INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES INCLUDING,
//  BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
//  CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
//  LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN
//  ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
//  POSSIBILITY OF SUCH DAMAGE.


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.Web;
using System.Web.Configuration;
using System.Linq;
using System.Linq.Expressions;


//using System.Transactions;

namespace CodeCampSV
{
    [DataObject(true)] // This attribute allows the ObjectDataSource wizard to see this class
    public class SessionsODS
    {
        private readonly string connectionString;

        public SessionsODS()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString;
        }


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectSessions> GetByPrimaryKeySessions(int searchid)
        {
            var conn = new SqlConnection(connectionString);
            conn.Open();
            var DataTemplateODSList = new List<DataObjectSessions>();
            SqlDataReader reader = null;
            string sqlSelectString =
                "SELECT SessionLevel_id,Username,title,description,approved,createdate,updatedate,AdminComments,InterentAccessRequired,LectureRoomsId,SessionTimesId,DoNotShowPrimarySpeaker,SponsorId,id FROM [dbo].[Sessions]  WHERE  CodeCampYearId=@CodeCampYearId AND id = @searchid ";
            var cmd = new SqlCommand(sqlSelectString, conn);
            cmd.Parameters.Add("@searchid", SqlDbType.Int, 4).Value = searchid;
            cmd.Parameters.Add("@CodeCampYearId", SqlDbType.Int, 4).Value = Utils.GetCurrentCodeCampYear();
            
            reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    int sessionlevel_id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    string username = reader.IsDBNull(1) ? "" : reader.GetString(1);
                    string title = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    string description = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    bool approved = reader.IsDBNull(4) ? false : reader.GetBoolean(4);
                    DateTime createdate = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5);
                    DateTime updatedate = reader.IsDBNull(6) ? DateTime.Now : reader.GetDateTime(6);
                    string admincomments = reader.IsDBNull(7) ? "" : reader.GetString(7);
                    bool interentaccessrequired = reader.IsDBNull(8) ? false : reader.GetBoolean(8);
                    int lectureroomsid = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                    int sessiontimesid = reader.IsDBNull(10) ? 0 : reader.GetInt32(10);
                    bool doNotShowPrimarySpeaker = reader.IsDBNull(11) ? false : reader.GetBoolean(11);
                    int sponsorId = reader.IsDBNull(12) ? 0 : reader.GetInt32(12);
                    int id = reader.IsDBNull(13) ? 0 : reader.GetInt32(13);
                    var td = new DataObjectSessions(sessionlevel_id, username, title, description, approved, createdate,
                                                    updatedate, admincomments, interentaccessrequired, lectureroomsid,
                                                    sessiontimesid, doNotShowPrimarySpeaker, id) {SponsorId = sponsorId};

                    DataTemplateODSList.Add(td);
                }
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            conn.Close();
            return DataTemplateODSList;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectSessions> GetByPKID(Guid userGuid)
        {
            int currentCodeCampYearId = Utils.GetCurrentCodeCampYear();
            List<DataObjectSessions> DataTemplateODSList = null;
            string cacheName = String.Format("{0}_{1}-{2}", Utils.CacheSessionsByUserGuid, userGuid.ToString(), currentCodeCampYearId);

            object o = HttpContext.Current.Cache[cacheName];

            if (o == null)
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    DataTemplateODSList = new List<DataObjectSessions>();

//                    const string sqlSelectString = @"
//                                SELECT SessionLevel_id,
//                                       Username,
//                                       title,
//                                       description,
//                                       approved,
//                                       createdate,
//                                       updatedate,
//                                       AdminComments,
//                                       InterentAccessRequired,
//                                       LectureRoomsId,
//                                       SessionTimesId,
//                                       id
//                                FROM Sessions
//                                WHERE  CodeCampYearId=@CodeCampYearId  AND (Username IN (SELECT Username FROM Attendees WHERE (PKID = @PKID)))
//                                ORDER BY Username";

                    const string sqlSelectString = @"
                                SELECT SessionLevel_id,
                                       Username,
                                       title,
                                       description,
                                       approved,
                                       createdate,
                                       updatedate,
                                       AdminComments,
                                       InterentAccessRequired,
                                       LectureRoomsId,
                                       SessionTimesId,
                                       id
                                FROM Sessions
                                WHERE CodeCampYearId = @CodeCampYearId AND
                                      Id IN (
                                              SELECT SessionId
                                              FROM SessionPresenter
                                              WHERE AttendeeId =
                                                    (
                                                      select Id
                                                      from Attendees
                                                      WHERE PKID = @PKID
                                                    )
                                      )
                                ORDER BY Username";






                    //"SELECT SessionLevel_id,Username,title,description,approved,createdate,
                    //updatedate,AdminComments,InterentAccessRequired,LectureRoomsId,
                    //SessionTimesId,id FROM [dbo].[Sessions]  WHERE id = @searchid";
                    using (var cmd = new SqlCommand(sqlSelectString, conn))
                    {
                        cmd.Parameters.Add("@PKID", SqlDbType.UniqueIdentifier).Value = userGuid;
                        cmd.Parameters.Add("@CodeCampYearId", SqlDbType.Int, 4).Value = Utils.GetCurrentCodeCampYear();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            try
                            {
                                while (reader.Read())
                                {
                                    int sessionlevel_id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                                    string username = reader.IsDBNull(1) ? "" : reader.GetString(1);
                                    string title = reader.IsDBNull(2) ? "" : reader.GetString(2);
                                    string description = reader.IsDBNull(3) ? "" : reader.GetString(3);
                                    bool approved = reader.IsDBNull(4) ? false : reader.GetBoolean(4);
                                    DateTime createdate = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5);
                                    DateTime updatedate = reader.IsDBNull(6) ? DateTime.Now : reader.GetDateTime(6);
                                    string admincomments = reader.IsDBNull(7) ? "" : reader.GetString(7);
                                    bool interentaccessrequired = reader.IsDBNull(8) ? false : reader.GetBoolean(8);
                                    int lectureroomsid = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                                    int sessiontimesid = reader.IsDBNull(10) ? 0 : reader.GetInt32(10);
                                    int id = reader.IsDBNull(11) ? 0 : reader.GetInt32(11);
                                    var td = new DataObjectSessions(sessionlevel_id, username, title, description, approved, createdate, updatedate, admincomments, interentaccessrequired, lectureroomsid, sessiontimesid, id);
                                    DataTemplateODSList.Add(td);
                                }
                            }
                            finally
                            {
                                if (reader != null)
                                    reader.Close();
                            }
                        }
                    }
                    conn.Close();
                    HttpContext.Current.Cache.Insert(cacheName, DataTemplateODSList, null, DateTime.Now.Add(new TimeSpan(0, 0, Utils.RetrieveSecondsForSessionCacheTimeout())), TimeSpan.Zero);
                }
            }
            else
            {
                DataTemplateODSList = (List<DataObjectSessions>) HttpContext.Current.Cache[cacheName];
            }
            return DataTemplateODSList;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectSessions> GetAllSessionsByAttendeeId(int attendeeId)
        {
            int currentCodeCampYearId = Utils.GetCurrentCodeCampYear();
            List<DataObjectSessions> DataTemplateODSList = null;
            string cacheName = String.Format("UtilsGetAllSessionsByAttendeeId_{0}_{1}", attendeeId, currentCodeCampYearId);

            object o = HttpContext.Current.Cache[cacheName];

            if (o == null)
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    DataTemplateODSList = new List<DataObjectSessions>();

                    

                    const string sqlSelectString = @"
                                SELECT SessionLevel_id,
                                       Username,
                                       title,
                                       description,
                                       approved,
                                       createdate,
                                       updatedate,
                                       AdminComments,
                                       InterentAccessRequired,
                                       LectureRoomsId,
                                       SessionTimesId,
                                       id
                                FROM Sessions
                                WHERE CodeCampYearId = @CodeCampYearId AND
                                      Id IN (
                                              SELECT SessionId
                                              FROM SessionPresenter
                                              WHERE AttendeeId = @AttendeeId
                                      )
                                ORDER BY Username";

                    //"SELECT SessionLevel_id,Username,title,description,approved,createdate,
                    //updatedate,AdminComments,InterentAccessRequired,LectureRoomsId,
                    //SessionTimesId,id FROM [dbo].[Sessions]  WHERE id = @searchid";
                    using (var cmd = new SqlCommand(sqlSelectString, conn))
                    {
                        cmd.Parameters.Add("@AttendeeId", SqlDbType.Int).Value = attendeeId;
                        cmd.Parameters.Add("@CodeCampYearId", SqlDbType.Int, 4).Value = Utils.GetCurrentCodeCampYear();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            try
                            {
                                while (reader.Read())
                                {
                                    int sessionlevel_id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                                    string username = reader.IsDBNull(1) ? "" : reader.GetString(1);
                                    string title = reader.IsDBNull(2) ? "" : reader.GetString(2);
                                    string description = reader.IsDBNull(3) ? "" : reader.GetString(3);
                                    bool approved = reader.IsDBNull(4) ? false : reader.GetBoolean(4);
                                    DateTime createdate = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5);
                                    DateTime updatedate = reader.IsDBNull(6) ? DateTime.Now : reader.GetDateTime(6);
                                    string admincomments = reader.IsDBNull(7) ? "" : reader.GetString(7);
                                    bool interentaccessrequired = reader.IsDBNull(8) ? false : reader.GetBoolean(8);
                                    int lectureroomsid = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                                    int sessiontimesid = reader.IsDBNull(10) ? 0 : reader.GetInt32(10);
                                    int id = reader.IsDBNull(11) ? 0 : reader.GetInt32(11);
                                    var td = new DataObjectSessions(sessionlevel_id, username, title, description, approved, createdate, updatedate, admincomments, interentaccessrequired, lectureroomsid, sessiontimesid, id);
                                    DataTemplateODSList.Add(td);
                                }
                            }
                            finally
                            {
                                if (reader != null)
                                    reader.Close();
                            }
                        }
                    }
                    conn.Close();
                    HttpContext.Current.Cache.Insert(cacheName, DataTemplateODSList, null, DateTime.Now.Add(new TimeSpan(0, 0, Utils.RetrieveSecondsForSessionCacheTimeout())), TimeSpan.Zero);
                }
            }
            else
            {
                DataTemplateODSList = (List<DataObjectSessions>)HttpContext.Current.Cache[cacheName];
            }
            return DataTemplateODSList;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectSessions> GetAllSessionsBySessionId(int sessionId)
        {
            var sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            var DataTemplateODSList = new List<DataObjectSessions>();
            SqlDataReader reader = null;
            string sqlSelectString =

                @"
                   SELECT Sessions.SessionLevel_id,
                           Sessions.Username,
                           Sessions.Title,
                           Sessions.description,
                           Sessions.Approved,
                           Sessions.Createdate,
                           Sessions.Updatedate,
                           Sessions.AdminComments,
                           Sessions.InterentAccessRequired,
                           Sessions.LectureRoomsId,
                           Sessions.SessionTimesId,
                           Sessions.id
                    FROM Sessions
                         LEFT OUTER JOIN SessionTimes ON (Sessions.SessionTimesId = SessionTimes.Id)
                    WHERE Sessions.Id = @id
                    ";
//                @"
                //SELECT SessionLevel_id,
                //       Username,
                //       title,
                //       sessions.description,
                //       approved,
                //       createdate,
                //       updatedate,
                //       AdminComments,
                //       InterentAccessRequired,
                //       LectureRoomsId,
                //       SessionTimesId,
                //       sessions.id
                //FROM Sessions,
                //     SessionTimes
                //WHERE   (Sessions.SessionTimesId = sessiontimes.id) AND
                //      (Username IN (SELECT Username FROM Sessions, dbo.SessionTimes WHERE (
                //      sessions.id = @id)))
                //ORDER BY StartTime";

            //"SELECT SessionLevel_id,Username,title,description,approved,createdate,
            //updatedate,AdminComments,InterentAccessRequired,LectureRoomsId,
            //SessionTimesId,id FROM [dbo].[Sessions]  WHERE id = @searchid";
            var cmd = new SqlCommand(sqlSelectString, sqlConnection);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = sessionId;
            
            //cmd.Parameters.Add("@CodeCampYearId", SqlDbType.Int, 4).Value = Utils.GetCurrentCodeCampYear();
           
            reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    int sessionlevel_id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    string username = reader.IsDBNull(1) ? "" : reader.GetString(1);
                    string title = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    string description = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    bool approved = reader.IsDBNull(4) ? false : reader.GetBoolean(4);
                    DateTime createdate = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5);
                    DateTime updatedate = reader.IsDBNull(6) ? DateTime.Now : reader.GetDateTime(6);
                    string admincomments = reader.IsDBNull(7) ? "" : reader.GetString(7);
                    bool interentaccessrequired = reader.IsDBNull(8) ? false : reader.GetBoolean(8);
                    int lectureroomsid = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                    int sessiontimesid = reader.IsDBNull(10) ? 0 : reader.GetInt32(10);
                    int id = reader.IsDBNull(11) ? 0 : reader.GetInt32(11);
                    var td = new DataObjectSessions(sessionlevel_id, username, title, description, approved, createdate,
                                                    updatedate, admincomments, interentaccessrequired, lectureroomsid,
                                                    sessiontimesid, id);
                    DataTemplateODSList.Add(td);
                }
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            sqlConnection.Close();
            return DataTemplateODSList;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectSessions> GetAllSessionsByUsername(string usernameForSessions)
        {
            List<DataObjectSessions> DataTemplateODSList = null;
              int currentCodeCampYearId = Utils.GetCurrentCodeCampYear();
              string cacheName = String.Format("{0}-{1}-{2}", Utils.CacheSessionsByUsername, usernameForSessions, currentCodeCampYearId);

            if (HttpContext.Current.Cache[cacheName] == null)
            {
                var conn = new SqlConnection(connectionString);
                conn.Open();
                DataTemplateODSList = new List<DataObjectSessions>();
                SqlDataReader reader = null;
                string sqlSelectString =
                    @"
                SELECT SessionLevel_id,
                       Username,
                       title,
                       description,
                       approved,
                       createdate,
                       updatedate,
                       AdminComments,
                       InterentAccessRequired,
                       LectureRoomsId,
                       SessionTimesId,
                       id
                FROM Sessions
                WHERE  CodeCampYearId=@CodeCampYearId AND (Username = @Username)
                Order By title";

                //"SELECT SessionLevel_id,Username,title,description,approved,createdate,
                //updatedate,AdminComments,InterentAccessRequired,LectureRoomsId,
                //SessionTimesId,id FROM [dbo].[Sessions]  WHERE id = @searchid";
                var cmd = new SqlCommand(sqlSelectString, conn);
                cmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = usernameForSessions;
                cmd.Parameters.Add("@CodeCampYearId", SqlDbType.Int, 4).Value = Utils.GetCurrentCodeCampYear();
           
                reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        int sessionlevel_id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        string username = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        string title = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        string description = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        bool approved = reader.IsDBNull(4) ? false : reader.GetBoolean(4);
                        DateTime createdate = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5);
                        DateTime updatedate = reader.IsDBNull(6) ? DateTime.Now : reader.GetDateTime(6);
                        string admincomments = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        bool interentaccessrequired = reader.IsDBNull(8) ? false : reader.GetBoolean(8);
                        int lectureroomsid = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                        int sessiontimesid = reader.IsDBNull(10) ? 0 : reader.GetInt32(10);
                        int id = reader.IsDBNull(11) ? 0 : reader.GetInt32(11);
                        var td = new DataObjectSessions(sessionlevel_id, username, title, description, approved,
                                                        createdate, updatedate, admincomments, interentaccessrequired,
                                                        lectureroomsid, sessiontimesid, id);
                        DataTemplateODSList.Add(td);
                    }
                }
                finally
                {
                    if (reader != null) reader.Close();
                }
                conn.Close();
                HttpContext.Current.Cache.Insert(cacheName, DataTemplateODSList,
                                                 null, DateTime.Now.Add(new TimeSpan(0, 0, Utils.RetrieveSecondsForSessionCacheTimeout())), TimeSpan.Zero);
            }
            else
            {
                DataTemplateODSList = (List<DataObjectSessions>) HttpContext.Current.Cache[cacheName];
            }
            return DataTemplateODSList;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectSessions> GetAllByStartTimeFilter(int start, int limit, string sort, string dir, out int count)
        {
            List<DataObjectSessions> result = GetAllByStartTime();
            count = result.Count;
            if (start >= 0 && limit > 0)
            {
                result = result.Skip(start).Take(limit).ToList();
            }
            
            return result;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<DataObjectSessions> GetAllByStartTime(string sessionListInts)
        {
            return SessionsFilterList(sessionListInts, GetAllByStartTime());
        }


        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<DataObjectSessions> GetAllByStartTime()
        {
            List<DataObjectSessions> DataTemplateODSList = null;

            int sessionTimesIdExclude = -1;
            int currentCodeCampYearId = Utils.GetCurrentCodeCampYear();

            string cacheName = String.Format("{0}-{1}-{2}", Utils.CacheSessionsAllByStartTime, sessionTimesIdExclude, currentCodeCampYearId);

            if (HttpContext.Current.Cache[cacheName] == null)
            {
                DataTemplateODSList = GetAllSessions(-1);
                HttpContext.Current.Cache.Insert(cacheName, DataTemplateODSList,
                                                 null, DateTime.Now.Add(new TimeSpan(0, 0, Utils.RetrieveSecondsForSessionCacheTimeout())), TimeSpan.Zero);
            }
            else
            {
                DataTemplateODSList = (List<DataObjectSessions>) HttpContext.Current.Cache[cacheName];
            }

            return DataTemplateODSList;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectSessions> GetAllByStartTimeOnlyAssigned()
        {
            int sessionTimesIdExclude = -1;
            int currentCodeCampYearId = Utils.GetCurrentCodeCampYear();

            List<DataObjectSessions> DataTemplateODSList = null;
            string cacheName = String.Format("{0}-{1}-{2}", Utils.CacheSessionsAllByStartTimeOnlyAssigned, sessionTimesIdExclude, currentCodeCampYearId);

            if (HttpContext.Current.Cache[cacheName] == null)
            {
                DataTemplateODSList = GetAllSessions(Utils.TimeSessionUnassigned);
                //HttpContext.Current.Cache.Insert(cacheName, DataTemplateODSList,
                //   null, DateTime.Now.Add(new TimeSpan(0, cacheTimeOutMinutes, 0)), TimeSpan.Zero);
                HttpContext.Current.Cache.Insert(cacheName, DataTemplateODSList,
                                                 null, DateTime.Now.Add(new TimeSpan(0, 0, Utils.RetrieveSecondsForSessionCacheTimeout())), TimeSpan.Zero);
            }
            else
            {
                DataTemplateODSList = (List<DataObjectSessions>) HttpContext.Current.Cache[cacheName];
            }

            return DataTemplateODSList;
        }

        private List<DataObjectSessions> GetAllSessions(int sessionTimesIdExclude)
        {
            // no cache here because this is private and called by the other public methods

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var DataTemplateODSList = new List<DataObjectSessions>();
                SqlDataReader reader = null;
                string sqlSelectString = @"SELECT 
                              dbo.Sessions.SessionLevel_id,
                              dbo.Sessions.Username,
                              dbo.Sessions.title,
                              dbo.Sessions.description,
                              dbo.Sessions.approved,
                              dbo.Sessions.createdate,
                              dbo.Sessions.updatedate,
                              dbo.Sessions.AdminComments,
                              dbo.Sessions.InterentAccessRequired,
                              dbo.Sessions.LectureRoomsId,
                              dbo.Sessions.SessionTimesId,
                              dbo.Sessions.id
                            FROM
                              dbo.Sessions
                              INNER JOIN dbo.SessionTimes ON (dbo.Sessions.SessionTimesId = dbo.SessionTimes.id)
                            WHERE 
                              dbo.Sessions.CodeCampYearId=@CodeCampYearId AND  dbo.Sessions.SessionTimesId <> @TimeSessionIdExclude
                            ORDER BY
                              dbo.SessionTimes.StartTime";
                //"SELECT SessionLevel_id,Username,title,description,approved,createdate,updatedate,AdminComments,InterentAccessRequired,LectureRoomsId,SessionTimesId,id FROM [dbo].[Sessions] ";
                var cmd = new SqlCommand(sqlSelectString, conn);
                cmd.Parameters.Add("@TimeSessionIdExclude", SqlDbType.Int).Value = sessionTimesIdExclude;
                cmd.Parameters.Add("@CodeCampYearId", SqlDbType.Int, 4).Value = Utils.GetCurrentCodeCampYear();
                reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        int sessionlevel_id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        string username = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        string title = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        string description = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        bool approved = reader.IsDBNull(4) ? false : reader.GetBoolean(4);
                        DateTime createdate = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5);
                        DateTime updatedate = reader.IsDBNull(6) ? DateTime.Now : reader.GetDateTime(6);
                        string admincomments = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        bool interentaccessrequired = reader.IsDBNull(8) ? false : reader.GetBoolean(8);
                        int lectureroomsid = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                        int sessiontimesid = reader.IsDBNull(10) ? 0 : reader.GetInt32(10);
                        int id = reader.IsDBNull(11) ? 0 : reader.GetInt32(11);
                        var td = new DataObjectSessions(sessionlevel_id, username, title, description, approved, createdate, updatedate, admincomments, interentaccessrequired, lectureroomsid, sessiontimesid, id);
                        DataTemplateODSList.Add(td);
                    }
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
                conn.Close();
                return DataTemplateODSList;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectSessions> GetByPresenterLastName(string sessionListInts)
        {
            return SessionsFilterList(sessionListInts, GetByPresenterLastName());
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectSessions> GetByPresenterLastName()
        {
            int currentCodeCampYearId = Utils.GetCurrentCodeCampYear();

            List<DataObjectSessions> DataTemplateODSList = null;
            string cacheName = String.Format("{0}-{1}", Utils.CacheSessionsAllByPresenterLastName, currentCodeCampYearId);

            if (HttpContext.Current.Cache[cacheName] == null)
            {
                var conn = new SqlConnection(connectionString);
                conn.Open();
                DataTemplateODSList = new List<DataObjectSessions>();
                SqlDataReader reader = null;
                string sqlSelectString =
                    @"SELECT dbo.Sessions.SessionLevel_id,
                       dbo.Sessions.Username,
                       dbo.Sessions.title,
                       dbo.Sessions.description,
                       dbo.Sessions.approved,
                       dbo.Sessions.createdate,
                       dbo.Sessions.updatedate,
                       dbo.Sessions.AdminComments,
                       dbo.Sessions.InterentAccessRequired,
                       dbo.Sessions.LectureRoomsId,
                       dbo.Sessions.SessionTimesId,
                       dbo.Sessions.id
                FROM dbo.Attendees
                     INNER JOIN dbo.Sessions ON (dbo.Attendees.Username = dbo.Sessions.Username
                     )

                 WHERE  dbo.Sessions.CodeCampYearId=@CodeCampYearId 
                ORDER BY dbo.Attendees.UserLastName";
                var cmd = new SqlCommand(sqlSelectString, conn);
                cmd.Parameters.Add("@CodeCampYearId", SqlDbType.Int, 4).Value = Utils.GetCurrentCodeCampYear();
                reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        int sessionlevel_id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        string username = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        string title = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        string description = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        bool approved = reader.IsDBNull(4) ? false : reader.GetBoolean(4);
                        DateTime createdate = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5);
                        DateTime updatedate = reader.IsDBNull(6) ? DateTime.Now : reader.GetDateTime(6);
                        string admincomments = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        bool interentaccessrequired = reader.IsDBNull(8) ? false : reader.GetBoolean(8);
                        int lectureroomsid = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                        int sessiontimesid = reader.IsDBNull(10) ? 0 : reader.GetInt32(10);
                        int id = reader.IsDBNull(11) ? 0 : reader.GetInt32(11);
                        var td = new DataObjectSessions(sessionlevel_id, username, title, description, approved,
                                                        createdate, updatedate, admincomments, interentaccessrequired,
                                                        lectureroomsid, sessiontimesid, id);
                        DataTemplateODSList.Add(td);
                    }
                }
                finally
                {
                    if (reader != null) reader.Close();
                }
                conn.Close();
                conn.Dispose();
                HttpContext.Current.Cache.Insert(cacheName, DataTemplateODSList,
                                                 null, DateTime.Now.Add(new TimeSpan(0, 0, Utils.RetrieveSecondsForSessionCacheTimeout())),
                                                 TimeSpan.Zero);
            }
            else
            {
                DataTemplateODSList = (List<DataObjectSessions>) HttpContext.Current.Cache[cacheName];
            }
            return DataTemplateODSList;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectSessions> GetBySessionTitle(string sessionListInts)
        {
            return SessionsFilterList(sessionListInts, GetBySessionTitle());
        }

        // 


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectSessions> GetBySessionTitle()
        {
            int currentCodeCampYearId = Utils.GetCurrentCodeCampYear();

            List<DataObjectSessions> DataTemplateODSList = null;
            string cacheName = String.Format("{0}-{1}", Utils.CacheSessionsAllByTitle, currentCodeCampYearId);

            if (HttpContext.Current.Cache[cacheName] == null)
            {
                var conn = new SqlConnection(connectionString);
                conn.Open();
                DataTemplateODSList = new List<DataObjectSessions>();
                SqlDataReader reader = null;
                string sqlSelectString =
                    @"SELECT 
                  dbo.Sessions.SessionLevel_id,
                  dbo.Sessions.Username,
                  dbo.Sessions.title,
                  dbo.Sessions.description,
                  dbo.Sessions.approved,
                  dbo.Sessions.createdate,
                  dbo.Sessions.updatedate,
                  dbo.Sessions.AdminComments,
                  dbo.Sessions.InterentAccessRequired,
                  dbo.Sessions.LectureRoomsId,
                  dbo.Sessions.SessionTimesId,
                  dbo.Sessions.id
                FROM
                  dbo.Sessions
                WHERE
                 CodeCampYearId=@CodeCampYearId 
                ORDER BY
                  dbo.Sessions.title";


                //"SELECT SessionLevel_id,Username,title,description,approved,createdate,updatedate,AdminComments,InterentAccessRequired,LectureRoomsId,SessionTimesId,id FROM [dbo].[Sessions] ";
                var cmd = new SqlCommand(sqlSelectString, conn);
                cmd.Parameters.Add("@CodeCampYearId", SqlDbType.Int, 4).Value = Utils.GetCurrentCodeCampYear();
                reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        int sessionlevel_id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        string username = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        string title = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        string description = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        bool approved = reader.IsDBNull(4) ? false : reader.GetBoolean(4);
                        DateTime createdate = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5);
                        DateTime updatedate = reader.IsDBNull(6) ? DateTime.Now : reader.GetDateTime(6);
                        string admincomments = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        bool interentaccessrequired = reader.IsDBNull(8) ? false : reader.GetBoolean(8);
                        int lectureroomsid = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                        int sessiontimesid = reader.IsDBNull(10) ? 0 : reader.GetInt32(10);
                        int id = reader.IsDBNull(11) ? 0 : reader.GetInt32(11);
                        var td = new DataObjectSessions(sessionlevel_id, username, title, description, approved,
                                                        createdate, updatedate, admincomments, interentaccessrequired,
                                                        lectureroomsid, sessiontimesid, id);
                        DataTemplateODSList.Add(td);
                    }

                    // DataTemplateODSList is sorted by title that could have funny stuff in it so need to resort with nimas stuff

                    DataTemplateODSList = Utils.SortSessionsByCleanTitle(DataTemplateODSList);

                }
                finally
                {
                    if (reader != null) reader.Close();
                }
                conn.Close();
                conn.Dispose();
                HttpContext.Current.Cache.Insert(cacheName, DataTemplateODSList,
                                                 null, DateTime.Now.Add(new TimeSpan(0, 0, Utils.RetrieveSecondsForSessionCacheTimeout())),
                                                 TimeSpan.Zero);
            }
            else
            {
                DataTemplateODSList = (List<DataObjectSessions>) HttpContext.Current.Cache[cacheName];
            }
            return DataTemplateODSList;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectSessions> GetBySessionSubmittedDate(string sessionListInts)
        {
            //List<DataObjectSessions> sessionsReturnList = GetBySessionSubmittedDate();
            return SessionsFilterList(sessionListInts, GetBySessionSubmittedDate());
        }

        private static List<DataObjectSessions> SessionsFilterList(string sessionListInts, List<DataObjectSessions> sessionsAll)
        {
            if (String.IsNullOrEmpty(sessionListInts))
            {
                return sessionsAll;
            }

            var sessionIdStringList = sessionListInts.Split(new[] {','}).ToList();
            List<int> sessionIdList = sessionIdStringList.Select(rec1 => Convert.ToInt32(rec1)).ToList();
            List<DataObjectSessions> sessionsFiltered =
                sessionsAll.Where(session => sessionIdList.Contains(session.Id)).ToList();
            return sessionsFiltered;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectSessions> GetBySessionSubmittedDate()
        {
            int currentCodeCampYearId = Utils.GetCurrentCodeCampYear();

            List<DataObjectSessions> DataTemplateODSList = null;
            string cacheName = String.Format("{0}-{1}", Utils.CacheSessionsAllBySubmissionDate, currentCodeCampYearId);

            if (HttpContext.Current.Cache[cacheName] == null)
            {
                var conn = new SqlConnection(connectionString);
                conn.Open();
                DataTemplateODSList = new List<DataObjectSessions>();
                SqlDataReader reader = null;
                string sqlSelectString =
                    @"SELECT 
                  dbo.Sessions.SessionLevel_id,
                  dbo.Sessions.Username,
                  dbo.Sessions.title,
                  dbo.Sessions.description,
                  dbo.Sessions.approved,
                  dbo.Sessions.createdate,
                  dbo.Sessions.updatedate,
                  dbo.Sessions.AdminComments,
                  dbo.Sessions.InterentAccessRequired,
                  dbo.Sessions.LectureRoomsId,
                  dbo.Sessions.SessionTimesId,
                  dbo.Sessions.id
                FROM
                  dbo.Sessions
                WHERE
                 CodeCampYearId=@CodeCampYearId 
                ORDER BY
                  dbo.Sessions.createdate";


                //"SELECT SessionLevel_id,Username,title,description,approved,createdate,updatedate,AdminComments,InterentAccessRequired,LectureRoomsId,SessionTimesId,id FROM [dbo].[Sessions] ";
                var cmd = new SqlCommand(sqlSelectString, conn);

                cmd.Parameters.Add("@CodeCampYearId", SqlDbType.Int, 4).Value = Utils.GetCurrentCodeCampYear();
                reader = cmd.ExecuteReader();
               
                try
                {
                    while (reader.Read())
                    {
                        int sessionlevel_id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        string username = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        string title = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        string description = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        bool approved = reader.IsDBNull(4) ? false : reader.GetBoolean(4);
                        DateTime createdate = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5);
                        DateTime updatedate = reader.IsDBNull(6) ? DateTime.Now : reader.GetDateTime(6);
                        string admincomments = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        bool interentaccessrequired = reader.IsDBNull(8) ? false : reader.GetBoolean(8);
                        int lectureroomsid = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                        int sessiontimesid = reader.IsDBNull(10) ? 0 : reader.GetInt32(10);
                        int id = reader.IsDBNull(11) ? 0 : reader.GetInt32(11);
                        var td = new DataObjectSessions(sessionlevel_id, username, title, description, approved,
                                                        createdate, updatedate, admincomments, interentaccessrequired,
                                                        lectureroomsid, sessiontimesid, id);
                        DataTemplateODSList.Add(td);
                    }
                }
                finally
                {
                    if (reader != null) reader.Close();
                }
                conn.Close();
                conn.Dispose();
                HttpContext.Current.Cache.Insert(cacheName, DataTemplateODSList,
                                                 null, DateTime.Now.Add(new TimeSpan(0, 0, Utils.RetrieveSecondsForSessionCacheTimeout())),
                                                 TimeSpan.Zero);
            }
            else
            {
                DataTemplateODSList = (List<DataObjectSessions>) HttpContext.Current.Cache[cacheName];
            }

            return DataTemplateODSList;
        }


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectTags> GetTagsBySession(int sessionId)
        {
            return GetTagsBySession(sessionId, Int32.MaxValue);
        }


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectTags> GetTagsBySession(int sessionId, int maxReturn)
        {
            List<DataObjectTags> DataTemplateODSList = null;
            string cacheName = String.Format("{0}-{1}", Utils.CacheTagsGetBySession, sessionId);

            if (HttpContext.Current.Cache[cacheName] == null)
            {
                var conn = new SqlConnection(connectionString);
                conn.Open();
                DataTemplateODSList = new List<DataObjectTags>();
                //SqlDataReader reader = null;
                const string sqlSelectString = @"
                        SELECT 
                          SessionTags.tagid,
                          Tags.TagName
                        FROM
                          SessionTags
                          INNER JOIN Tags ON (SessionTags.tagid = Tags.id)
                        WHERE
                          SessionTags.sessionId = @sessionId 
                        ORDER BY
                          Tags.TagName
                    ";


                var cmd = new SqlCommand(sqlSelectString, conn);
                cmd.Parameters.Add("@sessionId", SqlDbType.Int).Value = sessionId;
               

                SqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    int cnt = 0;
                    while (reader.Read() && cnt < maxReturn)
                    {
                        cnt++;
                        int tagId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        string tagName = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        var dotags = new DataObjectTags(tagId, tagName);
                        DataTemplateODSList.Add(dotags);
                    }
                }
                finally
                {
                    if (reader != null) reader.Close();
                }
                conn.Close();
                conn.Dispose();
                HttpContext.Current.Cache.Insert(cacheName, DataTemplateODSList,
                                                 null, DateTime.Now.Add(new TimeSpan(0, 0, Utils.RetrieveSecondsForSessionCacheTimeout())),
                                                 TimeSpan.Zero);
            }
            else
            {
                DataTemplateODSList = (List<DataObjectTags>) HttpContext.Current.Cache[cacheName];
            }

            return DataTemplateODSList;
        }


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectSessions> GetByTag(int tagId)
        {
            int currentCodeCampYearId = Utils.GetCurrentCodeCampYear();
            List<DataObjectSessions> DataTemplateODSList = null;
            string cacheName = String.Format("{0}-{1}-{2}", Utils.CacheSessionsGetByTagWithParams, tagId, currentCodeCampYearId);

            if (HttpContext.Current.Cache[cacheName] == null)
            {
                var conn = new SqlConnection(connectionString);
                conn.Open();
                DataTemplateODSList = new List<DataObjectSessions>();
                SqlDataReader reader = null;
                string sqlSelectString =
                    @"SELECT dbo.Sessions.SessionLevel_id,
                           dbo.Sessions.Username,
                           dbo.Sessions.title,
                           dbo.Sessions.description,
                           dbo.Sessions.approved,
                           dbo.Sessions.createdate,
                           dbo.Sessions.updatedate,
                           dbo.Sessions.AdminComments,
                           dbo.Sessions.InterentAccessRequired,
                           dbo.Sessions.LectureRoomsId,
                           dbo.Sessions.SessionTimesId,
                           dbo.Sessions.id
                    FROM dbo.Sessions
                    WHERE  CodeCampYearId=@CodeCampYearId  AND (dbo.Sessions.id IN (SELECT sessionId FROM SessionTags WHERE (tagid =
                     @tagid)))
                    ORDER BY dbo.Sessions.title";


                //"SELECT SessionLevel_id,Username,title,description,approved,createdate,updatedate,AdminComments,InterentAccessRequired,LectureRoomsId,SessionTimesId,id FROM [dbo].[Sessions] ";
                var cmd = new SqlCommand(sqlSelectString, conn);
                cmd.Parameters.Add("@CodeCampYearId", SqlDbType.Int, 4).Value = Utils.GetCurrentCodeCampYear();
                cmd.Parameters.Add("@tagid", SqlDbType.Int).Value = tagId;
                reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        int sessionlevel_id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        string username = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        string title = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        string description = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        bool approved = reader.IsDBNull(4) ? false : reader.GetBoolean(4);
                        DateTime createdate = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5);
                        DateTime updatedate = reader.IsDBNull(6) ? DateTime.Now : reader.GetDateTime(6);
                        string admincomments = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        bool interentaccessrequired = reader.IsDBNull(8) ? false : reader.GetBoolean(8);
                        int lectureroomsid = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                        int sessiontimesid = reader.IsDBNull(10) ? 0 : reader.GetInt32(10);
                        int id = reader.IsDBNull(11) ? 0 : reader.GetInt32(11);
                        var td = new DataObjectSessions(sessionlevel_id, username, title, description, approved,
                                                        createdate, updatedate, admincomments, interentaccessrequired,
                                                        lectureroomsid, sessiontimesid, id);
                        DataTemplateODSList.Add(td);
                    }
                }
                finally
                {
                    if (reader != null) reader.Close();
                }
                conn.Close();
                conn.Dispose();
                HttpContext.Current.Cache.Insert(cacheName, DataTemplateODSList,
                                                 null, DateTime.Now.Add(new TimeSpan(0, 0, Utils.RetrieveSecondsForSessionCacheTimeout())),
                                                 TimeSpan.Zero);
            }
            else
            {
                DataTemplateODSList = (List<DataObjectSessions>) HttpContext.Current.Cache[cacheName];
            }

            return DataTemplateODSList;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectSessions> GetByTrack(int trackId)
        {
            List<DataObjectSessions> DataTemplateODSList = null;

            int currentCodeCampYearId = Utils.GetCurrentCodeCampYear();
            string cacheName = String.Format("{0}-{1}-{2}", Utils.CacheSessionsGetByTrackWithParams, trackId, currentCodeCampYearId);

            if (HttpContext.Current.Cache[cacheName] == null)
            {
                var conn = new SqlConnection(connectionString);
                conn.Open();
                DataTemplateODSList = new List<DataObjectSessions>();
                SqlDataReader reader = null;
                const string sqlSelectString = @"SELECT dbo.Sessions.SessionLevel_id,
                       dbo.Sessions.Username,
                       dbo.Sessions.title,
                       dbo.Sessions.description,
                       dbo.Sessions.approved,
                       dbo.Sessions.createdate,
                       dbo.Sessions.updatedate,
                       dbo.Sessions.AdminComments,
                       dbo.Sessions.InterentAccessRequired,
                       dbo.Sessions.LectureRoomsId,
                       dbo.Sessions.SessionTimesId,
                       dbo.Sessions.id
                FROM dbo.SessionTimes
                     INNER JOIN dbo.Sessions ON (dbo.SessionTimes.id =
                      dbo.Sessions.SessionTimesId)
                WHERE dbo.Sessions.CodeCampYearId = @CodeCampYearId AND
                      dbo.Sessions.id IN (SELECT TrackSession.SessionId FROM TrackSession WHERE
                       TrackId = @trackid)
                ORDER BY dbo.SessionTimes.StartTime
                ";


                //"SELECT SessionLevel_id,Username,title,description,approved,createdate,updatedate,AdminComments,InterentAccessRequired,LectureRoomsId,SessionTimesId,id FROM [dbo].[Sessions] ";
                var cmd = new SqlCommand(sqlSelectString, conn);
                cmd.Parameters.Add("@CodeCampYearId", SqlDbType.Int, 4).Value = Utils.GetCurrentCodeCampYear();
                cmd.Parameters.Add("@trackid", SqlDbType.Int).Value = trackId;
                reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        int sessionlevel_id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        string username = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        string title = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        string description = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        bool approved = reader.IsDBNull(4) ? false : reader.GetBoolean(4);
                        DateTime createdate = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5);
                        DateTime updatedate = reader.IsDBNull(6) ? DateTime.Now : reader.GetDateTime(6);
                        string admincomments = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        bool interentaccessrequired = reader.IsDBNull(8) ? false : reader.GetBoolean(8);
                        int lectureroomsid = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                        int sessiontimesid = reader.IsDBNull(10) ? 0 : reader.GetInt32(10);
                        int id = reader.IsDBNull(11) ? 0 : reader.GetInt32(11);
                        var td = new DataObjectSessions(sessionlevel_id, username, title, description, approved,
                                                        createdate, updatedate, admincomments, interentaccessrequired,
                                                        lectureroomsid, sessiontimesid, id);
                        DataTemplateODSList.Add(td);
                    }
                }
                finally
                {
                    if (reader != null) reader.Close();
                }
                conn.Close();
                conn.Dispose();
                HttpContext.Current.Cache.Insert(cacheName, DataTemplateODSList,
                                                 null, DateTime.Now.Add(new TimeSpan(0, 0, Utils.RetrieveSecondsForSessionCacheTimeout())),
                                                 TimeSpan.Zero);
            }
            else
            {
                DataTemplateODSList = (List<DataObjectSessions>)HttpContext.Current.Cache[cacheName];
            }

            return DataTemplateODSList;
        }



        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void UpdateSession(int sessionId,string slidesUrl,bool queueSlidesNotification,
                                  bool doNotShowPrimarySpeaker, string title,string hashTags, string description,
                                  int sessionLevel, List<int> tagList,int sponsorId)
        {
            using (var scope = new TransactionScope())
            {
                using (
                    var sqlConnection =
                        new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
                {
                    try
                    {
                        sqlConnection.Open();
                        const string sqlUpdate = @"
                            UPDATE 
                              dbo.Sessions
                            SET
                              SessionLevel_id = @SessionLevel_id,
                              SessionsMaterialUrl = @SessionMaterialUrl,
                              SessionsMaterialQueueToSend = @SessionsMaterialQueueToSend,
                              DoNotShowPrimarySpeaker = @DoNotShowPrimarySpeaker,
                              title = @title,
                              twitterhashtags = @twitterhashtags,
                              description = @description,
                              updatedate = @updatedate,
                              SponsorId = @SponsorId
                            WHERE id=@sessionid";
                        

                        var sqlCommand = new SqlCommand(sqlUpdate, sqlConnection);

                        
                         sqlCommand.Parameters.Add("@SponsorId", SqlDbType.Int).Value = sponsorId;
                        

                        sqlCommand.Parameters.Add("@SessionLevel_id", SqlDbType.Int).Value = sessionLevel;
                        sqlCommand.Parameters.Add("@sessionid", SqlDbType.Int).Value = sessionId;
                        sqlCommand.Parameters.Add("@title", SqlDbType.VarChar).Value = title;
                        sqlCommand.Parameters.Add("@TwitterHashTags", SqlDbType.VarChar).Value = hashTags;
                        sqlCommand.Parameters.Add("@description", SqlDbType.VarChar).Value = description;
                        sqlCommand.Parameters.Add("@updatedate", SqlDbType.DateTime).Value = DateTime.Now;
                        sqlCommand.Parameters.Add("@DoNotShowPrimarySpeaker", SqlDbType.Bit).Value = doNotShowPrimarySpeaker;
                        sqlCommand.Parameters.Add("@SessionsMaterialQueueToSend", SqlDbType.Bit).Value = queueSlidesNotification;
                        sqlCommand.Parameters.Add("@SessionMaterialUrl", SqlDbType.VarChar).Value = slidesUrl;


                        sqlCommand.ExecuteNonQuery();

                        // fastest thing to code is delete existing tags and just re-add.

                        string sqlDelete = "DELETE FROM SessionTags WHERE sessionId=@sessionid";
                        var sqlCommandDelete = new SqlCommand(sqlDelete, sqlConnection);
                        sqlCommandDelete.Parameters.Add("@sessionid", SqlDbType.Int).Value = sessionId;
                        int rowsDeleted = sqlCommandDelete.ExecuteNonQuery();

                        string sqlInsert = "INSERT INTO SessionTags (tagid,sessionId) VALUES (@tagid,@sessionid)";
                        var sqlCommandInsert = new SqlCommand(sqlInsert, sqlConnection);
                        sqlCommandInsert.Parameters.Add("@sessionid", SqlDbType.Int).Value = sessionId;
                        sqlCommandInsert.Parameters.Add("@tagid", SqlDbType.Int);
                        foreach (int idTag in tagList)
                        {
                            sqlCommandInsert.Parameters["@tagid"].Value = idTag;
                            int rowsInserted = sqlCommandInsert.ExecuteNonQuery();
                        }
                        scope.Complete();
                        sqlConnection.Close();
                    }
                    catch (Exception ee)
                    {
                        throw new ApplicationException(ee.ToString());
                    }
                }
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void UpdateSessionTitleDescr(int sessionId, string title, string description)
        {

            using (
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                try
                {
                    sqlConnection.Open();

                    const string sqlUpdate =
                        @"
                            UPDATE 
                              dbo.Sessions
                            SET
                              title = @title,
                              description = @description,
                              updatedate = @updatedate
                            WHERE id=@sessionid";

                    var sqlCommand = new SqlCommand(sqlUpdate, sqlConnection);
                    sqlCommand.Parameters.Add("@sessionid", SqlDbType.Int).Value = sessionId;
                    sqlCommand.Parameters.Add("@title", SqlDbType.VarChar).Value = title;
                    sqlCommand.Parameters.Add("@description", SqlDbType.VarChar).Value = description;
                    sqlCommand.Parameters.Add("@updatedate", SqlDbType.DateTime).Value = DateTime.Now;

                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ee)
                {
                    throw new ApplicationException(ee.ToString());
                }
            }
        }


       

        #region Nested type: DataObjectSessions

        public class DataObjectSessions
        {
            public DataObjectSessions()
            {
            }

            public DataObjectSessions(int sessionlevel_id, string username, string title, string description,
                                      bool approved, DateTime createdate, DateTime updatedate, string admincomments,
                                      bool interentaccessrequired, int lectureroomsid, int sessiontimesid, int id)
            {
                Sessionlevel_id = sessionlevel_id;
                Username = username;
                Title = title;
                Description = description;
                Approved = approved;
                Createdate = createdate;
                Updatedate = updatedate;
                Admincomments = admincomments;
                Interentaccessrequired = interentaccessrequired;
                Lectureroomsid = lectureroomsid;
                Sessiontimesid = sessiontimesid;
                Id = id;
            }

            public DataObjectSessions(int sessionlevel_id, string username, string title, string description,
                                      bool approved, DateTime createdate, DateTime updatedate, string admincomments,
                                      bool interentaccessrequired, int lectureroomsid, int sessiontimesid,bool doNotShowPrimarySpeaker, int id)
            {
                Sessionlevel_id = sessionlevel_id;
                Username = username;
                Title = title;
                Description = description;
                Approved = approved;
                Createdate = createdate;
                Updatedate = updatedate;
                Admincomments = admincomments;
                Interentaccessrequired = interentaccessrequired;
                Lectureroomsid = lectureroomsid;
                Sessiontimesid = sessiontimesid;
                DoNotShowPrimarySpeaker = doNotShowPrimarySpeaker;
                Id = id;
            }

            [DataObjectField(false, false, true)]
            public int Sessionlevel_id { get; set; }

            [DataObjectField(false, false, true)]
            public int SponsorId { get; set; }

            [DataObjectField(false, false, true)]
            public string Username { get; set; }

            [DataObjectField(false, false, true)]
            public string Title { get; set; }

            [DataObjectField(false, false, true)]
            public string Description { get; set; }

            [DataObjectField(false, false, true)]
            public bool Approved { get; set; }

            [DataObjectField(false, false, true)]
            public DateTime Createdate { get; set; }

            [DataObjectField(false, false, true)]
            public DateTime Updatedate { get; set; }

            [DataObjectField(false, false, true)]
            public string Admincomments { get; set; }

            [DataObjectField(false, false, true)]
            public bool Interentaccessrequired { get; set; }

            [DataObjectField(false, false, true)]
            public int Lectureroomsid { get; set; }

            [DataObjectField(false, false, true)]
            public int Sessiontimesid { get; set; }

            [DataObjectField(false, false, true)]
            public bool DoNotShowPrimarySpeaker { get; set; }

            [DataObjectField(true, true, false)]
            public int Id { get; set; }
        }

        #endregion

        #region Nested type: DataObjectTags

        public class DataObjectTags
        {
            public DataObjectTags()
            {
            }

            public DataObjectTags(int tagId, string tagName)
            {
                TagId = tagId;
                TagName = tagName;
            }

            [DataObjectField(false, false, true)]
            public int TagId { get; set; }

            [DataObjectField(false, false, true)]
            public string TagName { get; set; }
        }

        #endregion
    }
}