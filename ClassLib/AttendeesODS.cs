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
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Web;
using System.Web.Configuration;
using System.Linq;

namespace CodeCampSV
{
    [DataObject(true)] // This attribute allows the ObjectDataSource wizard to see this class
    public class AttendeesODS
    {
        private readonly string connectionString;

        public AttendeesODS()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectAttendees> GetByUsername(string sortData, string searchusername)
        {
            return GetByUsername(sortData, searchusername, true);
        }

        

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectAttendees> GetBySessionId(int sessionId)
        {
            bool cacheResultSet = true;

            List<DataObjectAttendees> DataTemplateODSList;
            string cacheName = string.Format("{0}-{1}", Utils.CacheAttendeeBySessionId, sessionId);

            if (!cacheResultSet)
            {
                HttpContext.Current.Cache.Remove(cacheName);
            }


            if (HttpContext.Current.Cache[cacheName] == null)
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    var attendeeIdsInOrder = new List<int>();
                    // Get list of attendees by session in correct order
                    const string sqlSel =
                        @"SELECT AttendeeId FROM SessionPresenter WHERE SessionID = @SessionId ORDER BY Id";
                    using (var sqlCommandA = new SqlCommand(sqlSel, conn))
                    {
                        sqlCommandA.Parameters.Add("@SessionId", SqlDbType.Int).Value = sessionId;

                        using (var readerA = sqlCommandA.ExecuteReader())
                        {

                            try
                            {
                                while (readerA != null && readerA.Read())
                                {
                                    attendeeIdsInOrder.Add(readerA.GetInt32(0));
                                }
                            }
                            catch (Exception ee1)
                            {
                                throw new ApplicationException(ee1.ToString());
                            }
                        }
                    }

                    const string sqlSelectString =
                      @"SELECT Username,
                       ApplicationName,
                       Email,
                       Comment,
                       Password,
                       PasswordQuestion,
                       PasswordAnswer,
                       IsApproved,
                       LastActivityDate,
                       LastLoginDate,
                       CreationDate,
                       IsOnLine,
                       IsLockedOut,
                       LastLockedOutDate,
                       FailedPasswordAttemptCount,
                       FailedPasswordAttemptWindowStart,
                       FailedPasswordAnswerAttemptCount,
                       FailedPasswordAnswerAttemptWindowStart,
                       LastPasswordChangedDate,
                       UserWebsite,
                       UserLocation,
                       UserImage,
                       UserFirstName,
                       UserLastName,
                       UserZipCode,
                       UserBio,
                       UserShareInfo,
                       ReferralGUID,
                       ConfirmedDate,
                       VistaSlotsId,
                       FullNameUsernameZipcode,
                       VistaOnly,
                       SaturdayClasses,
                       SundayClasses,
                       DesktopOrLaptopForVista,
                       SaturdayDinner,
                       PKID
                       FROM [dbo].[Attendees]
                       WHERE Id = @Id";

                    DataTemplateODSList = new List<DataObjectAttendees>();
                    foreach (int attendIdForProcessing in attendeeIdsInOrder)
                    {
                        var cmd = new SqlCommand(sqlSelectString, conn);
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = attendIdForProcessing;
                        var reader = cmd.ExecuteReader();
                        try
                        {
                            while (reader.Read())
                            {
                                string username = reader.IsDBNull(0) ? "" : reader.GetString(0);
                                string applicationname = reader.IsDBNull(1) ? "" : reader.GetString(1);
                                string email = reader.IsDBNull(2) ? "" : reader.GetString(2);
                                string comment = reader.IsDBNull(3) ? "" : reader.GetString(3);
                                string password = reader.IsDBNull(4) ? "" : reader.GetString(4);
                                string passwordquestion = reader.IsDBNull(5) ? "" : reader.GetString(5);
                                string passwordanswer = reader.IsDBNull(6) ? "" : reader.GetString(6);
                                bool isapproved = reader.IsDBNull(7) ? false : reader.GetBoolean(7);
                                DateTime lastactivitydate = reader.IsDBNull(8) ? DateTime.Now : reader.GetDateTime(8);
                                DateTime lastlogindate = reader.IsDBNull(9) ? DateTime.Now : reader.GetDateTime(9);
                                DateTime creationdate = reader.IsDBNull(10) ? DateTime.Now : reader.GetDateTime(10);
                                bool isonline = reader.IsDBNull(11) ? false : reader.GetBoolean(11);
                                bool islockedout = reader.IsDBNull(12) ? false : reader.GetBoolean(12);
                                DateTime lastlockedoutdate = reader.IsDBNull(13) ? DateTime.Now : reader.GetDateTime(13);
                                int failedpasswordattemptcount = reader.IsDBNull(14) ? 0 : reader.GetInt32(14);
                                DateTime failedpasswordattemptwindowstart = reader.IsDBNull(15)
                                                                                ? DateTime.Now
                                                                                : reader.GetDateTime(15);
                                int failedpasswordanswerattemptcount = reader.IsDBNull(16) ? 0 : reader.GetInt32(16);
                                DateTime failedpasswordanswerattemptwindowstart = reader.IsDBNull(17)
                                                                                      ? DateTime.Now
                                                                                      : reader.GetDateTime(17);
                                DateTime lastpasswordchangeddate = reader.IsDBNull(18)
                                                                       ? DateTime.Now
                                                                       : reader.GetDateTime(18);
                                string userwebsite = reader.IsDBNull(19) ? "" : reader.GetString(19);
                                string userlocation = reader.IsDBNull(20) ? "" : reader.GetString(20);
                                SqlBytes userimage = reader.IsDBNull(21) ? new SqlBytes() : reader.GetSqlBytes(21);
                                string userfirstname = reader.IsDBNull(22) ? "" : reader.GetString(22);
                                string userlastname = reader.IsDBNull(23) ? "" : reader.GetString(23);
                                string userzipcode = reader.IsDBNull(24) ? "" : reader.GetString(24);
                                string userbio = reader.IsDBNull(25) ? "" : reader.GetString(25);
                                bool usershareinfo = reader.IsDBNull(26) ? false : reader.GetBoolean(26);
                                Guid referralguid = reader.IsDBNull(27) ? Guid.NewGuid() : reader.GetGuid(27);
                                DateTime confirmeddate = reader.IsDBNull(28) ? DateTime.Now : reader.GetDateTime(28);
                                int vistaslotsid = reader.IsDBNull(29) ? 0 : reader.GetInt32(29);
                                string fullnameusernamezipcode = reader.IsDBNull(30) ? "" : reader.GetString(30);
                                bool vistaonly = reader.IsDBNull(31) ? false : reader.GetBoolean(31);
                                bool saturdayclasses = reader.IsDBNull(32) ? false : reader.GetBoolean(32);
                                bool sundayclasses = reader.IsDBNull(33) ? false : reader.GetBoolean(33);
                                string desktoporlaptopforvista = reader.IsDBNull(34) ? "" : reader.GetString(34);
                                bool saturdaydinner = reader.IsDBNull(35) ? false : reader.GetBoolean(35);
                                Guid pkid = reader.IsDBNull(36) ? Guid.NewGuid() : reader.GetGuid(36);
                                var td = new DataObjectAttendees(username, applicationname, email, comment, password,
                                                                 passwordquestion, passwordanswer, isapproved,
                                                                 lastactivitydate,
                                                                 lastlogindate, creationdate, isonline, islockedout,
                                                                 lastlockedoutdate, failedpasswordattemptcount,
                                                                 failedpasswordattemptwindowstart,
                                                                 failedpasswordanswerattemptcount,
                                                                 failedpasswordanswerattemptwindowstart,
                                                                 lastpasswordchangeddate,
                                                                 userwebsite, userlocation, userimage, userfirstname,
                                                                 userlastname, userzipcode, userbio, usershareinfo,
                                                                 referralguid,
                                                                 confirmeddate, vistaslotsid, fullnameusernamezipcode,
                                                                 vistaonly,
                                                                 saturdaydinner, saturdayclasses, sundayclasses,
                                                                 desktoporlaptopforvista, pkid);
                                DataTemplateODSList.Add(td);
                            }
                        }
                        finally
                        {
                            if (reader != null) reader.Close();
                        }
                    }
                }
                if (cacheResultSet)
                {
                    HttpContext.Current.Cache.Insert(cacheName, DataTemplateODSList,
                                                     null, DateTime.Now.Add(new TimeSpan(0, 0, Utils.RetrieveSecondsForSessionCacheTimeout())), TimeSpan.Zero);
                }
            }
            else
            {
                DataTemplateODSList = (List<DataObjectAttendees>) HttpContext.Current.Cache[cacheName];
            }
            return DataTemplateODSList;
        }


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectAttendees> GetByUsername(string sortData, string searchusername, bool cacheResultSet)
        {
            List<DataObjectAttendees> DataTemplateODSList;
            string cacheName = Utils.CacheAttendeeByUserName + "_" + searchusername;

            if (!cacheResultSet)
            {
                HttpContext.Current.Cache.Remove(cacheName);
            }


            if (HttpContext.Current.Cache[cacheName] == null)
            {
                var conn = new SqlConnection(connectionString);
                conn.Open();
                DataTemplateODSList = new List<DataObjectAttendees>();
                SqlDataReader reader = null;
                // PGK modified search to not be crazy isnull stuff
                string sqlSelectString =
                    @"SELECT Username,
                       ApplicationName,
                       Email,
                       Comment,
                       Password,
                       PasswordQuestion,
                       PasswordAnswer,
                       IsApproved,
                       LastActivityDate,
                       LastLoginDate,
                       CreationDate,
                       IsOnLine,
                       IsLockedOut,
                       LastLockedOutDate,
                       FailedPasswordAttemptCount,
                       FailedPasswordAttemptWindowStart,
                       FailedPasswordAnswerAttemptCount,
                       FailedPasswordAnswerAttemptWindowStart,
                       LastPasswordChangedDate,
                       UserWebsite,
                       UserLocation,
                       UserImage,
                       UserFirstName,
                       UserLastName,
                       UserZipCode,
                       UserBio,
                       UserShareInfo,
                       ReferralGUID,
                       ConfirmedDate,
                       VistaSlotsId,
                       FullNameUsernameZipcode,
                       VistaOnly,
                       SaturdayClasses,
                       SundayClasses,
                       DesktopOrLaptopForVista,
                       SaturdayDinner,
                       PKID
                FROM [dbo].[Attendees]
                WHERE username = @searchusername";
                var cmd = new SqlCommand(sqlSelectString, conn);
                cmd.Parameters.Add("@searchusername", SqlDbType.VarChar).Value = searchusername ?? string.Empty;
                reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        string username = reader.IsDBNull(0) ? "" : reader.GetString(0);
                        string applicationname = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        string email = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        string comment = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        string password = reader.IsDBNull(4) ? "" : reader.GetString(4);
                        string passwordquestion = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        string passwordanswer = reader.IsDBNull(6) ? "" : reader.GetString(6);
                        bool isapproved = reader.IsDBNull(7) ? false : reader.GetBoolean(7);
                        DateTime lastactivitydate = reader.IsDBNull(8) ? DateTime.Now : reader.GetDateTime(8);
                        DateTime lastlogindate = reader.IsDBNull(9) ? DateTime.Now : reader.GetDateTime(9);
                        DateTime creationdate = reader.IsDBNull(10) ? DateTime.Now : reader.GetDateTime(10);
                        bool isonline = reader.IsDBNull(11) ? false : reader.GetBoolean(11);
                        bool islockedout = reader.IsDBNull(12) ? false : reader.GetBoolean(12);
                        DateTime lastlockedoutdate = reader.IsDBNull(13) ? DateTime.Now : reader.GetDateTime(13);
                        int failedpasswordattemptcount = reader.IsDBNull(14) ? 0 : reader.GetInt32(14);
                        DateTime failedpasswordattemptwindowstart = reader.IsDBNull(15)
                                                                        ? DateTime.Now
                                                                        : reader.GetDateTime(15);
                        int failedpasswordanswerattemptcount = reader.IsDBNull(16) ? 0 : reader.GetInt32(16);
                        DateTime failedpasswordanswerattemptwindowstart = reader.IsDBNull(17)
                                                                              ? DateTime.Now
                                                                              : reader.GetDateTime(17);
                        DateTime lastpasswordchangeddate = reader.IsDBNull(18) ? DateTime.Now : reader.GetDateTime(18);
                        string userwebsite = reader.IsDBNull(19) ? "" : reader.GetString(19);
                        string userlocation = reader.IsDBNull(20) ? "" : reader.GetString(20);
                        SqlBytes userimage = reader.IsDBNull(21) ? new SqlBytes() : reader.GetSqlBytes(21);
                        string userfirstname = reader.IsDBNull(22) ? "" : reader.GetString(22);
                        string userlastname = reader.IsDBNull(23) ? "" : reader.GetString(23);
                        string userzipcode = reader.IsDBNull(24) ? "" : reader.GetString(24);
                        string userbio = reader.IsDBNull(25) ? "" : reader.GetString(25);
                        bool usershareinfo = reader.IsDBNull(26) ? false : reader.GetBoolean(26);
                        Guid referralguid = reader.IsDBNull(27) ? Guid.NewGuid() : reader.GetGuid(27);
                        DateTime confirmeddate = reader.IsDBNull(28) ? DateTime.Now : reader.GetDateTime(28);
                        int vistaslotsid = reader.IsDBNull(29) ? 0 : reader.GetInt32(29);
                        string fullnameusernamezipcode = reader.IsDBNull(30) ? "" : reader.GetString(30);
                        bool vistaonly = reader.IsDBNull(31) ? false : reader.GetBoolean(31);
                        bool saturdayclasses = reader.IsDBNull(32) ? false : reader.GetBoolean(32);
                        bool sundayclasses = reader.IsDBNull(33) ? false : reader.GetBoolean(33);
                        string desktoporlaptopforvista = reader.IsDBNull(34) ? "" : reader.GetString(34);
                        bool saturdaydinner = reader.IsDBNull(35) ? false : reader.GetBoolean(35);
                        Guid pkid = reader.IsDBNull(36) ? Guid.NewGuid() : reader.GetGuid(36);
                        var td = new DataObjectAttendees(username, applicationname, email, comment, password,
                                                         passwordquestion, passwordanswer, isapproved, lastactivitydate,
                                                         lastlogindate, creationdate, isonline, islockedout,
                                                         lastlockedoutdate, failedpasswordattemptcount,
                                                         failedpasswordattemptwindowstart,
                                                         failedpasswordanswerattemptcount,
                                                         failedpasswordanswerattemptwindowstart, lastpasswordchangeddate,
                                                         userwebsite, userlocation, userimage, userfirstname,
                                                         userlastname, userzipcode, userbio, usershareinfo, referralguid,
                                                         confirmeddate, vistaslotsid, fullnameusernamezipcode, vistaonly,
                                                         saturdaydinner, saturdayclasses, sundayclasses,
                                                         desktoporlaptopforvista, pkid);
                        DataTemplateODSList.Add(td);
                    }
                }
                finally
                {
                    if (reader != null) reader.Close();
                }
                conn.Close();
                conn.Dispose();

                if (cacheResultSet)
                {
                    HttpContext.Current.Cache.Insert(cacheName, DataTemplateODSList,
                                                     null, DateTime.Now.Add(new TimeSpan(0, 0, Utils.RetrieveSecondsForSessionCacheTimeout())), TimeSpan.Zero);
                }
            }
            else
            {
                DataTemplateODSList = (List<DataObjectAttendees>) HttpContext.Current.Cache[cacheName];
            }

            return DataTemplateODSList;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectAttendees> GetByShowOnWeb(string sortData, bool searchusershareinfo)
        {
            var conn = new SqlConnection(connectionString);
            conn.Open();
            var DataTemplateODSList = new List<DataObjectAttendees>();
            SqlDataReader reader = null;
            string sqlSelectString =
                "SELECT Username,ApplicationName,Email,Comment,Password,PasswordQuestion,PasswordAnswer,IsApproved,LastActivityDate,LastLoginDate,CreationDate,IsOnLine,IsLockedOut,LastLockedOutDate,FailedPasswordAttemptCount,FailedPasswordAttemptWindowStart,FailedPasswordAnswerAttemptCount,FailedPasswordAnswerAttemptWindowStart,LastPasswordChangedDate,UserWebsite,UserLocation,UserImage,UserFirstName,UserLastName,UserZipCode,UserBio,UserShareInfo,ReferralGUID,ConfirmedDate,VistaSlotsId,FullNameUsernameZipcode,VistaOnly,SaturdayClasses,SundayClasses,DesktopOrLaptopForVista,PKID FROM [dbo].[Attendees]  WHERE usershareinfo = @searchusershareinfo";
            var cmd = new SqlCommand(sqlSelectString, conn);
            cmd.Parameters.Add("@searchusershareinfo", SqlDbType.Bit, 1).Value = searchusershareinfo;
            ;
            reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    string username = reader.IsDBNull(0) ? "" : reader.GetString(0);
                    string applicationname = reader.IsDBNull(1) ? "" : reader.GetString(1);
                    string email = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    string comment = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    string password = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    string passwordquestion = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    string passwordanswer = reader.IsDBNull(6) ? "" : reader.GetString(6);
                    bool isapproved = reader.IsDBNull(7) ? false : reader.GetBoolean(7);
                    DateTime lastactivitydate = reader.IsDBNull(8) ? DateTime.Now : reader.GetDateTime(8);
                    DateTime lastlogindate = reader.IsDBNull(9) ? DateTime.Now : reader.GetDateTime(9);
                    DateTime creationdate = reader.IsDBNull(10) ? DateTime.Now : reader.GetDateTime(10);
                    bool isonline = reader.IsDBNull(11) ? false : reader.GetBoolean(11);
                    bool islockedout = reader.IsDBNull(12) ? false : reader.GetBoolean(12);
                    DateTime lastlockedoutdate = reader.IsDBNull(13) ? DateTime.Now : reader.GetDateTime(13);
                    int failedpasswordattemptcount = reader.IsDBNull(14) ? 0 : reader.GetInt32(14);
                    DateTime failedpasswordattemptwindowstart = reader.IsDBNull(15)
                                                                    ? DateTime.Now
                                                                    : reader.GetDateTime(15);
                    int failedpasswordanswerattemptcount = reader.IsDBNull(16) ? 0 : reader.GetInt32(16);
                    DateTime failedpasswordanswerattemptwindowstart = reader.IsDBNull(17)
                                                                          ? DateTime.Now
                                                                          : reader.GetDateTime(17);
                    DateTime lastpasswordchangeddate = reader.IsDBNull(18) ? DateTime.Now : reader.GetDateTime(18);
                    string userwebsite = reader.IsDBNull(19) ? "" : reader.GetString(19);
                    string userlocation = reader.IsDBNull(20) ? "" : reader.GetString(20);
                    SqlBytes userimage = reader.IsDBNull(21) ? new SqlBytes() : reader.GetSqlBytes(21);
                    string userfirstname = reader.IsDBNull(22) ? "" : reader.GetString(22);
                    string userlastname = reader.IsDBNull(23) ? "" : reader.GetString(23);
                    string userzipcode = reader.IsDBNull(24) ? "" : reader.GetString(24);
                    string userbio = reader.IsDBNull(25) ? "" : reader.GetString(25);
                    bool usershareinfo = reader.IsDBNull(26) ? false : reader.GetBoolean(26);
                    Guid referralguid = reader.IsDBNull(27) ? Guid.NewGuid() : reader.GetGuid(27);
                    DateTime confirmeddate = reader.IsDBNull(28) ? DateTime.Now : reader.GetDateTime(28);
                    int vistaslotsid = reader.IsDBNull(29) ? 0 : reader.GetInt32(29);
                    string fullnameusernamezipcode = reader.IsDBNull(30) ? "" : reader.GetString(30);
                    bool vistaonly = reader.IsDBNull(31) ? false : reader.GetBoolean(31);
                    bool saturdayclasses = reader.IsDBNull(32) ? false : reader.GetBoolean(32);
                    bool sundayclasses = reader.IsDBNull(33) ? false : reader.GetBoolean(33);
                    string desktoporlaptopforvista = reader.IsDBNull(34) ? "" : reader.GetString(34);
                    Guid pkid = reader.IsDBNull(35) ? Guid.NewGuid() : reader.GetGuid(35);
                    // WARNING! SaturdayDinner always returns true. fix later
                    var td = new DataObjectAttendees(username, applicationname, email, comment, password,
                                                     passwordquestion, passwordanswer, isapproved, lastactivitydate,
                                                     lastlogindate, creationdate, isonline, islockedout,
                                                     lastlockedoutdate, failedpasswordattemptcount,
                                                     failedpasswordattemptwindowstart, failedpasswordanswerattemptcount,
                                                     failedpasswordanswerattemptwindowstart, lastpasswordchangeddate,
                                                     userwebsite, userlocation, userimage, userfirstname, userlastname,
                                                     userzipcode, userbio, usershareinfo, referralguid, confirmeddate,
                                                     vistaslotsid, fullnameusernamezipcode, vistaonly, true,
                                                     saturdayclasses, sundayclasses, desktoporlaptopforvista, pkid);
                    DataTemplateODSList.Add(td);
                }
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            conn.Close();

            if (sortData == null)
            {
                sortData = "Pkid";
            }
            if (sortData.Length == 0)
            {
                sortData = "Pkid";
            }
            string sortDataBase = sortData;
            string descString = " DESC";
            if (sortData.EndsWith(descString))
            {
                sortDataBase = sortData.Substring(0, sortData.Length - descString.Length);
            }
            Comparison<DataObjectAttendees> comparison = null;
            switch (sortDataBase)
            {
                case "Username":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Username.CompareTo(rhs.Username); };
                    break;
                case "Applicationname":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Applicationname.CompareTo(rhs.Applicationname); };
                    break;
                case "Email":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Email.CompareTo(rhs.Email); };
                    break;
                case "Comment":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Comment.CompareTo(rhs.Comment); };
                    break;
                case "Password":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Password.CompareTo(rhs.Password); };
                    break;
                case "Passwordquestion":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Passwordquestion.CompareTo(rhs.Passwordquestion); };
                    break;
                case "Passwordanswer":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Passwordanswer.CompareTo(rhs.Passwordanswer); };
                    break;
                case "Isapproved":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Isapproved.CompareTo(rhs.Isapproved); };
                    break;
                case "Lastactivitydate":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Lastactivitydate.CompareTo(rhs.Lastactivitydate); };
                    break;
                case "Lastlogindate":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Lastlogindate.CompareTo(rhs.Lastlogindate); };
                    break;
                case "Creationdate":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Creationdate.CompareTo(rhs.Creationdate); };
                    break;
                case "Isonline":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Isonline.CompareTo(rhs.Isonline); };
                    break;
                case "Islockedout":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Islockedout.CompareTo(rhs.Islockedout); };
                    break;
                case "Lastlockedoutdate":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Lastlockedoutdate.CompareTo(rhs.Lastlockedoutdate); };
                    break;
                case "Failedpasswordattemptcount":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Failedpasswordattemptcount.CompareTo(rhs.Failedpasswordattemptcount); };
                    break;
                case "Failedpasswordattemptwindowstart":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
                            {
                                return
                                    lhs.Failedpasswordattemptwindowstart.CompareTo(rhs.Failedpasswordattemptwindowstart);
                            };
                    break;
                case "Failedpasswordanswerattemptcount":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
                            {
                                return
                                    lhs.Failedpasswordanswerattemptcount.CompareTo(rhs.Failedpasswordanswerattemptcount);
                            };
                    break;
                case "Failedpasswordanswerattemptwindowstart":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
                            {
                                return
                                    lhs.Failedpasswordanswerattemptwindowstart.CompareTo(
                                        rhs.Failedpasswordanswerattemptwindowstart);
                            };
                    break;
                case "Lastpasswordchangeddate":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Lastpasswordchangeddate.CompareTo(rhs.Lastpasswordchangeddate); };
                    break;
                case "Userwebsite":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Userwebsite.CompareTo(rhs.Userwebsite); };
                    break;
                case "Userlocation":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Userlocation.CompareTo(rhs.Userlocation); };
                    break;
                case "Userfirstname":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Userfirstname.CompareTo(rhs.Userfirstname); };
                    break;
                case "Userlastname":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Userlastname.CompareTo(rhs.Userlastname); };
                    break;
                case "Userzipcode":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Userzipcode.CompareTo(rhs.Userzipcode); };
                    break;
                case "Userbio":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Userbio.CompareTo(rhs.Userbio); };
                    break;
                case "Usershareinfo":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Usershareinfo.CompareTo(rhs.Usershareinfo); };
                    break;
                case "Confirmeddate":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Confirmeddate.CompareTo(rhs.Confirmeddate); };
                    break;
                case "Vistaslotsid":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Vistaslotsid.CompareTo(rhs.Vistaslotsid); };
                    break;
                case "Fullnameusernamezipcode":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Fullnameusernamezipcode.CompareTo(rhs.Fullnameusernamezipcode); };
                    break;
                case "Vistaonly":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Vistaonly.CompareTo(rhs.Vistaonly); };
                    break;
                case "Saturdayclasses":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Saturdayclasses.CompareTo(rhs.Saturdayclasses); };
                    break;
                case "Sundayclasses":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Sundayclasses.CompareTo(rhs.Sundayclasses); };
                    break;
                case "Desktoporlaptopforvista":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Desktoporlaptopforvista.CompareTo(rhs.Desktoporlaptopforvista); };
                    break;
            }
            if (comparison != null)
            {
                DataTemplateODSList.Sort(comparison);
                if (sortData.ToLower().EndsWith("desc"))
                {
                    DataTemplateODSList.Reverse();
                }
            }
            return DataTemplateODSList;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<DataObjectAttendees> GetAllAttendeesBetweenDatesBarbeque(DateTime startDate, DateTime endDate)
        {

            List<DataObjectAttendees> attendeesResult = new List<DataObjectAttendees>();
            List<DataObjectAttendees> attendees = GetAllAttendees("");
            foreach (var attendee in attendees)
            {
                if (attendee.Creationdate.CompareTo(startDate) > 0 && attendee.Creationdate.CompareTo(endDate) < 0)
                {
                    if (attendee.Saturdaydinner)
                    {
                        attendeesResult.Add(attendee);
                    }
                }
            }
            return attendeesResult;
        }


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectAttendees> GetAllAttendeesBetweenDatesNotPresenter(DateTime startDate, DateTime endDate)
        {
            // Get List of SessionId's for this year only
            List<int> sessionsListIdThisYear = GetSessionsListIdThisYear();

            List<string> presenterList = new List<string>();
            SessionsODS sessionODS = new SessionsODS();
            List<SessionsODS.DataObjectSessions> sessionList = sessionODS.GetAllByStartTime();
            foreach (var sessions in sessionList)
            {
                if (!presenterList.Contains(sessions.Username))
                {
                    if (sessionsListIdThisYear.Contains(sessions.Id))
                    {
                        presenterList.Add(sessions.Username);
                    }
                }
            }

            // get attendess this year
            var userNamesThisYear = AttendeesCodeCampYearManager.I.GetUsernameListByCodeCampYearId(Utils.CurrentCodeCampYear);


            List<DataObjectAttendees> attendeesResult = new List<DataObjectAttendees>();
            List<DataObjectAttendees> attendees = GetAllAttendees("");
            foreach (var attendee in attendees)
            {
                if (attendee.Creationdate.CompareTo(startDate) > 0 && attendee.Creationdate.CompareTo(endDate) < 0)
                {
                    if (!presenterList.Contains(attendee.Username))
                    {
                        if (userNamesThisYear.Contains(attendee.Username))
                        {
                            attendeesResult.Add(attendee);
                        }
                    }
                }
            }
            return attendeesResult;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<DataObjectAttendees> GetAllAttendeesBetweenDatesJustPresenter(DateTime startDate, DateTime endDate)
        {
            // Get List of SessionId's for this year only
            List<int> sessionsListIdThisYear = GetSessionsListIdThisYear();

            List<string> presenterList = new List<string>();
            SessionsODS sessionODS = new SessionsODS();
            List<SessionsODS.DataObjectSessions> sessionList = sessionODS.GetAllByStartTime();
            foreach (var sessions in sessionList)
            {
                if (!presenterList.Contains(sessions.Username))
                {
                    if (sessionsListIdThisYear.Contains(sessions.Id))
                    {
                        presenterList.Add(sessions.Username);
                    }
                }
            }

            List<DataObjectAttendees> attendeesResult = new List<DataObjectAttendees>();
            List<DataObjectAttendees> attendees = GetAllAttendees("");
            foreach (var attendee in attendees)
            {
                if (attendee.Creationdate.CompareTo(startDate) > 0 && attendee.Creationdate.CompareTo(endDate) < 0)
                {
                    if (presenterList.Contains(attendee.Username))
                    {
                        attendeesResult.Add(attendee);
                    }
                }
            }
            return attendeesResult;
        }

        private List<int> GetSessionsListIdThisYear()
        {
            var sessionsResultList = SessionsManager.I.Get(new SessionsQuery
                                                               {
                                                                   CodeCampYearId = Utils.CurrentCodeCampYear
                                                               });
            return (from data in sessionsResultList select data.Id).ToList();
        }


        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<DataObjectAttendees> GetAllAttendees(string sortData)
        {
            var conn = new SqlConnection(connectionString);
            conn.Open();
            var DataTemplateODSList = new List<DataObjectAttendees>();
            SqlDataReader reader = null;
            const string sqlSelectString = "SELECT Username,ApplicationName,Email,Comment,Password,PasswordQuestion,PasswordAnswer,IsApproved,LastActivityDate,LastLoginDate,CreationDate,IsOnLine,IsLockedOut,LastLockedOutDate,FailedPasswordAttemptCount,FailedPasswordAttemptWindowStart,FailedPasswordAnswerAttemptCount,FailedPasswordAnswerAttemptWindowStart,LastPasswordChangedDate,UserWebsite,UserLocation,UserImage,UserFirstName,UserLastName,UserZipCode,UserBio,UserShareInfo,ReferralGUID,ConfirmedDate,VistaSlotsId,FullNameUsernameZipcode,VistaOnly,SaturdayClasses,SundayClasses,DesktopOrLaptopForVista,SaturdayDinner,PKID FROM [dbo].[Attendees] ORDER BY UserLastName,UserFirstName ";
            var cmd = new SqlCommand(sqlSelectString, conn);
            reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    string username = reader.IsDBNull(0) ? "" : reader.GetString(0);
                    string applicationname = reader.IsDBNull(1) ? "" : reader.GetString(1);
                    string email = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    string comment = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    string password = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    string passwordquestion = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    string passwordanswer = reader.IsDBNull(6) ? "" : reader.GetString(6);
                    bool isapproved = reader.IsDBNull(7) ? false : reader.GetBoolean(7);
                    DateTime lastactivitydate = reader.IsDBNull(8) ? DateTime.Now : reader.GetDateTime(8);
                    DateTime lastlogindate = reader.IsDBNull(9) ? DateTime.Now : reader.GetDateTime(9);
                    DateTime creationdate = reader.IsDBNull(10) ? DateTime.Now : reader.GetDateTime(10);
                    bool isonline = reader.IsDBNull(11) ? false : reader.GetBoolean(11);
                    bool islockedout = reader.IsDBNull(12) ? false : reader.GetBoolean(12);
                    DateTime lastlockedoutdate = reader.IsDBNull(13) ? DateTime.Now : reader.GetDateTime(13);
                    int failedpasswordattemptcount = reader.IsDBNull(14) ? 0 : reader.GetInt32(14);
                    DateTime failedpasswordattemptwindowstart = reader.IsDBNull(15)
                                                                    ? DateTime.Now
                                                                    : reader.GetDateTime(15);
                    int failedpasswordanswerattemptcount = reader.IsDBNull(16) ? 0 : reader.GetInt32(16);
                    DateTime failedpasswordanswerattemptwindowstart = reader.IsDBNull(17)
                                                                          ? DateTime.Now
                                                                          : reader.GetDateTime(17);
                    DateTime lastpasswordchangeddate = reader.IsDBNull(18) ? DateTime.Now : reader.GetDateTime(18);
                    string userwebsite = reader.IsDBNull(19) ? "" : reader.GetString(19);
                    string userlocation = reader.IsDBNull(20) ? "" : reader.GetString(20);
                    SqlBytes userimage = reader.IsDBNull(21) ? new SqlBytes() : reader.GetSqlBytes(21);
                    string userfirstname = reader.IsDBNull(22) ? "" : reader.GetString(22);
                    string userlastname = reader.IsDBNull(23) ? "" : reader.GetString(23);
                    string userzipcode = reader.IsDBNull(24) ? "" : reader.GetString(24);
                    string userbio = reader.IsDBNull(25) ? "" : reader.GetString(25);
                    bool usershareinfo = reader.IsDBNull(26) ? false : reader.GetBoolean(26);
                    Guid referralguid = reader.IsDBNull(27) ? Guid.NewGuid() : reader.GetGuid(27);
                    DateTime confirmeddate = reader.IsDBNull(28) ? DateTime.Now : reader.GetDateTime(28);
                    int vistaslotsid = reader.IsDBNull(29) ? 0 : reader.GetInt32(29);
                    string fullnameusernamezipcode = reader.IsDBNull(30) ? "" : reader.GetString(30);
                    bool vistaonly = reader.IsDBNull(31) ? false : reader.GetBoolean(31);
                    bool saturdayclasses = reader.IsDBNull(32) ? false : reader.GetBoolean(32);
                    bool sundayclasses = reader.IsDBNull(33) ? false : reader.GetBoolean(33);
                    string desktoporlaptopforvista = reader.IsDBNull(34) ? "" : reader.GetString(34);
                    bool saturdaydinner = reader.IsDBNull(35) ? false : reader.GetBoolean(35);
                    Guid pkid = reader.IsDBNull(36) ? Guid.NewGuid() : reader.GetGuid(36);
                    var td = new DataObjectAttendees(username, applicationname, email, comment, password,
                                                     passwordquestion, passwordanswer, isapproved, lastactivitydate,
                                                     lastlogindate, creationdate, isonline, islockedout,
                                                     lastlockedoutdate, failedpasswordattemptcount,
                                                     failedpasswordattemptwindowstart, failedpasswordanswerattemptcount,
                                                     failedpasswordanswerattemptwindowstart, lastpasswordchangeddate,
                                                     userwebsite, userlocation, userimage, userfirstname, userlastname,
                                                     userzipcode, userbio, usershareinfo, referralguid, confirmeddate,
                                                     vistaslotsid, fullnameusernamezipcode, vistaonly, saturdaydinner,
                                                     saturdayclasses, sundayclasses, desktoporlaptopforvista, pkid);
                    DataTemplateODSList.Add(td);
                }
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            conn.Close();

            if (sortData == null)
            {
                sortData = "Pkid";
            }
            if (sortData.Length == 0)
            {
                sortData = "Pkid";
            }
            string sortDataBase = sortData;
            string descString = " DESC";
            if (sortData.EndsWith(descString))
            {
                sortDataBase = sortData.Substring(0, sortData.Length - descString.Length);
            }
            Comparison<DataObjectAttendees> comparison = null;
            switch (sortDataBase)
            {
                case "Username":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Username.CompareTo(rhs.Username); };
                    break;
                case "Applicationname":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Applicationname.CompareTo(rhs.Applicationname); };
                    break;
                case "Email":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Email.CompareTo(rhs.Email); };
                    break;
                case "Comment":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Comment.CompareTo(rhs.Comment); };
                    break;
                case "Password":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Password.CompareTo(rhs.Password); };
                    break;
                case "Passwordquestion":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Passwordquestion.CompareTo(rhs.Passwordquestion); };
                    break;
                case "Passwordanswer":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Passwordanswer.CompareTo(rhs.Passwordanswer); };
                    break;
                case "Isapproved":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Isapproved.CompareTo(rhs.Isapproved); };
                    break;
                case "Lastactivitydate":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Lastactivitydate.CompareTo(rhs.Lastactivitydate); };
                    break;
                case "Lastlogindate":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Lastlogindate.CompareTo(rhs.Lastlogindate); };
                    break;
                case "Creationdate":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Creationdate.CompareTo(rhs.Creationdate); };
                    break;
                case "Isonline":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Isonline.CompareTo(rhs.Isonline); };
                    break;
                case "Islockedout":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Islockedout.CompareTo(rhs.Islockedout); };
                    break;
                case "Lastlockedoutdate":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Lastlockedoutdate.CompareTo(rhs.Lastlockedoutdate); };
                    break;
                case "Failedpasswordattemptcount":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Failedpasswordattemptcount.CompareTo(rhs.Failedpasswordattemptcount); };
                    break;
                case "Failedpasswordattemptwindowstart":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
                            {
                                return
                                    lhs.Failedpasswordattemptwindowstart.CompareTo(rhs.Failedpasswordattemptwindowstart);
                            };
                    break;
                case "Failedpasswordanswerattemptcount":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
                            {
                                return
                                    lhs.Failedpasswordanswerattemptcount.CompareTo(rhs.Failedpasswordanswerattemptcount);
                            };
                    break;
                case "Failedpasswordanswerattemptwindowstart":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
                            {
                                return
                                    lhs.Failedpasswordanswerattemptwindowstart.CompareTo(
                                        rhs.Failedpasswordanswerattemptwindowstart);
                            };
                    break;
                case "Lastpasswordchangeddate":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Lastpasswordchangeddate.CompareTo(rhs.Lastpasswordchangeddate); };
                    break;
                case "Userwebsite":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Userwebsite.CompareTo(rhs.Userwebsite); };
                    break;
                case "Userlocation":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Userlocation.CompareTo(rhs.Userlocation); };
                    break;
                case "Userfirstname":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Userfirstname.CompareTo(rhs.Userfirstname); };
                    break;
                case "Userlastname":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Userlastname.CompareTo(rhs.Userlastname); };
                    break;
                case "Userzipcode":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Userzipcode.CompareTo(rhs.Userzipcode); };
                    break;
                case "Userbio":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Userbio.CompareTo(rhs.Userbio); };
                    break;
                case "Usershareinfo":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Usershareinfo.CompareTo(rhs.Usershareinfo); };
                    break;
                case "Confirmeddate":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Confirmeddate.CompareTo(rhs.Confirmeddate); };
                    break;
                case "Vistaslotsid":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Vistaslotsid.CompareTo(rhs.Vistaslotsid); };
                    break;
                case "Fullnameusernamezipcode":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Fullnameusernamezipcode.CompareTo(rhs.Fullnameusernamezipcode); };
                    break;
                case "Vistaonly":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Vistaonly.CompareTo(rhs.Vistaonly); };
                    break;
                case "Saturdayclasses":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Saturdayclasses.CompareTo(rhs.Saturdayclasses); };
                    break;
                case "Sundayclasses":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Sundayclasses.CompareTo(rhs.Sundayclasses); };
                    break;
                case "Desktoporlaptopforvista":
                    comparison =
                        delegate(DataObjectAttendees lhs, DataObjectAttendees rhs) { return lhs.Desktoporlaptopforvista.CompareTo(rhs.Desktoporlaptopforvista); };
                    break;
            }
            if (comparison != null)
            {
                DataTemplateODSList.Sort(comparison);
                if (sortData.ToLower().EndsWith("desc"))
                {
                    DataTemplateODSList.Reverse();
                }
            }
            return DataTemplateODSList;
        }

        //[DataObjectMethod(DataObjectMethodType.Select, false)]
        //public List<DataObjectAttendees> GetByPrimaryKeyAttendees(string sortData, Guid searchpkid)
        //{
        //    SqlConnection conn = new SqlConnection(connectionString);
        //    conn.Open();
        //    List<DataObjectAttendees> DataTemplateODSList = new List<DataObjectAttendees>();
        //    SqlDataReader reader = null;
        //    // PGK added where clause
        //    string sqlSelectString = "SELECT Username,ApplicationName,Email,Comment,Password,PasswordQuestion,PasswordAnswer,IsApproved,LastActivityDate,LastLoginDate,CreationDate,IsOnLine,IsLockedOut,LastLockedOutDate,FailedPasswordAttemptCount,FailedPasswordAttemptWindowStart,FailedPasswordAnswerAttemptCount,FailedPasswordAnswerAttemptWindowStart,LastPasswordChangedDate,UserWebsite,UserLocation,UserImage,UserFirstName,UserLastName,UserZipCode,UserBio,UserShareInfo,ReferralGUID,ConfirmedDate,VistaSlotsId,FullNameUsernameZipcode,VistaOnly,SaturdayClasses,SundayClasses,DesktopOrLaptopForVista,PKID FROM [dbo].[Attendees] WHERE PKID=@searchpkid ";
        //    SqlCommand cmd = new SqlCommand(sqlSelectString, conn);
        //    cmd.Parameters.Add("@searchpkid", SqlDbType.UniqueIdentifier, 16).Value = searchpkid; ;
        //    reader = cmd.ExecuteReader();
        //    try
        //    {
        //        while (reader.Read())
        //        {
        //            string username = reader.IsDBNull(0) ? "" : reader.GetString(0);
        //            string applicationname = reader.IsDBNull(1) ? "" : reader.GetString(1);
        //            string email = reader.IsDBNull(2) ? "" : reader.GetString(2);
        //            string comment = reader.IsDBNull(3) ? "" : reader.GetString(3);
        //            string password = reader.IsDBNull(4) ? "" : reader.GetString(4);
        //            string passwordquestion = reader.IsDBNull(5) ? "" : reader.GetString(5);
        //            string passwordanswer = reader.IsDBNull(6) ? "" : reader.GetString(6);
        //            bool isapproved = reader.IsDBNull(7) ? false : reader.GetBoolean(7);
        //            DateTime lastactivitydate = reader.IsDBNull(8) ? DateTime.Now : reader.GetDateTime(8);
        //            DateTime lastlogindate = reader.IsDBNull(9) ? DateTime.Now : reader.GetDateTime(9);
        //            DateTime creationdate = reader.IsDBNull(10) ? DateTime.Now : reader.GetDateTime(10);
        //            bool isonline = reader.IsDBNull(11) ? false : reader.GetBoolean(11);
        //            bool islockedout = reader.IsDBNull(12) ? false : reader.GetBoolean(12);
        //            DateTime lastlockedoutdate = reader.IsDBNull(13) ? DateTime.Now : reader.GetDateTime(13);
        //            int failedpasswordattemptcount = reader.IsDBNull(14) ? 0 : reader.GetInt32(14);
        //            DateTime failedpasswordattemptwindowstart = reader.IsDBNull(15) ? DateTime.Now : reader.GetDateTime(15);
        //            int failedpasswordanswerattemptcount = reader.IsDBNull(16) ? 0 : reader.GetInt32(16);
        //            DateTime failedpasswordanswerattemptwindowstart = reader.IsDBNull(17) ? DateTime.Now : reader.GetDateTime(17);
        //            DateTime lastpasswordchangeddate = reader.IsDBNull(18) ? DateTime.Now : reader.GetDateTime(18);
        //            string userwebsite = reader.IsDBNull(19) ? "" : reader.GetString(19);
        //            string userlocation = reader.IsDBNull(20) ? "" : reader.GetString(20);
        //            SqlBytes userimage = reader.IsDBNull(21) ? new SqlBytes() : reader.GetSqlBytes(21);
        //            string userfirstname = reader.IsDBNull(22) ? "" : reader.GetString(22);
        //            string userlastname = reader.IsDBNull(23) ? "" : reader.GetString(23);
        //            string userzipcode = reader.IsDBNull(24) ? "" : reader.GetString(24);
        //            string userbio = reader.IsDBNull(25) ? "" : reader.GetString(25);
        //            bool usershareinfo = reader.IsDBNull(26) ? false : reader.GetBoolean(26);
        //            Guid referralguid = reader.IsDBNull(27) ? Guid.NewGuid() : reader.GetGuid(27);
        //            DateTime confirmeddate = reader.IsDBNull(28) ? DateTime.Now : reader.GetDateTime(28);
        //            int vistaslotsid = reader.IsDBNull(29) ? 0 : reader.GetInt32(29);
        //            string fullnameusernamezipcode = reader.IsDBNull(30) ? "" : reader.GetString(30);
        //            bool vistaonly = reader.IsDBNull(31) ? false : reader.GetBoolean(31);
        //            bool saturdayclasses = reader.IsDBNull(32) ? false : reader.GetBoolean(32);
        //            bool sundayclasses = reader.IsDBNull(33) ? false : reader.GetBoolean(33);
        //            string desktoporlaptopforvista = reader.IsDBNull(34) ? "" : reader.GetString(34);
        //            Guid pkid = reader.IsDBNull(35) ? Guid.NewGuid() : reader.GetGuid(35);
        //            DataObjectAttendees td = new DataObjectAttendees(username, applicationname, email, comment, password, passwordquestion, passwordanswer, isapproved, lastactivitydate, lastlogindate, creationdate, isonline, islockedout, lastlockedoutdate, failedpasswordattemptcount, failedpasswordattemptwindowstart, failedpasswordanswerattemptcount, failedpasswordanswerattemptwindowstart, lastpasswordchangeddate, userwebsite, userlocation, userimage, userfirstname, userlastname, userzipcode, userbio, usershareinfo, referralguid, confirmeddate, vistaslotsid, fullnameusernamezipcode, vistaonly, saturdayclasses, sundayclasses, desktoporlaptopforvista, pkid);
        //            DataTemplateODSList.Add(td);
        //        }
        //    }
        //    finally
        //    {
        //        if (reader != null) reader.Close();
        //    }
        //    conn.Close();

        //    if (sortData == null)
        //    {
        //        sortData = "Pkid";
        //    }
        //    if (sortData.Length == 0)
        //    {
        //        sortData = "Pkid";
        //    }
        //    string sortDataBase = sortData;
        //    string descString = " DESC";
        //    if (sortData.EndsWith(descString))
        //    {
        //        sortDataBase = sortData.Substring(0, sortData.Length - descString.Length);
        //    }
        //    Comparison<DataObjectAttendees> comparison = null;
        //    switch (sortDataBase)
        //    {
        //        case "Username":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Username.CompareTo(rhs.Username);
        //               }
        //             );
        //            break;
        //        case "Applicationname":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Applicationname.CompareTo(rhs.Applicationname);
        //               }
        //             );
        //            break;
        //        case "Email":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Email.CompareTo(rhs.Email);
        //               }
        //             );
        //            break;
        //        case "Comment":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Comment.CompareTo(rhs.Comment);
        //               }
        //             );
        //            break;
        //        case "Password":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Password.CompareTo(rhs.Password);
        //               }
        //             );
        //            break;
        //        case "Passwordquestion":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Passwordquestion.CompareTo(rhs.Passwordquestion);
        //               }
        //             );
        //            break;
        //        case "Passwordanswer":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Passwordanswer.CompareTo(rhs.Passwordanswer);
        //               }
        //             );
        //            break;
        //        case "Isapproved":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Isapproved.CompareTo(rhs.Isapproved);
        //               }
        //             );
        //            break;
        //        case "Lastactivitydate":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Lastactivitydate.CompareTo(rhs.Lastactivitydate);
        //               }
        //             );
        //            break;
        //        case "Lastlogindate":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Lastlogindate.CompareTo(rhs.Lastlogindate);
        //               }
        //             );
        //            break;
        //        case "Creationdate":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Creationdate.CompareTo(rhs.Creationdate);
        //               }
        //             );
        //            break;
        //        case "Isonline":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Isonline.CompareTo(rhs.Isonline);
        //               }
        //             );
        //            break;
        //        case "Islockedout":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Islockedout.CompareTo(rhs.Islockedout);
        //               }
        //             );
        //            break;
        //        case "Lastlockedoutdate":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Lastlockedoutdate.CompareTo(rhs.Lastlockedoutdate);
        //               }
        //             );
        //            break;
        //        case "Failedpasswordattemptcount":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Failedpasswordattemptcount.CompareTo(rhs.Failedpasswordattemptcount);
        //               }
        //             );
        //            break;
        //        case "Failedpasswordattemptwindowstart":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Failedpasswordattemptwindowstart.CompareTo(rhs.Failedpasswordattemptwindowstart);
        //               }
        //             );
        //            break;
        //        case "Failedpasswordanswerattemptcount":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Failedpasswordanswerattemptcount.CompareTo(rhs.Failedpasswordanswerattemptcount);
        //               }
        //             );
        //            break;
        //        case "Failedpasswordanswerattemptwindowstart":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Failedpasswordanswerattemptwindowstart.CompareTo(rhs.Failedpasswordanswerattemptwindowstart);
        //               }
        //             );
        //            break;
        //        case "Lastpasswordchangeddate":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Lastpasswordchangeddate.CompareTo(rhs.Lastpasswordchangeddate);
        //               }
        //             );
        //            break;
        //        case "Userwebsite":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Userwebsite.CompareTo(rhs.Userwebsite);
        //               }
        //             );
        //            break;
        //        case "Userlocation":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Userlocation.CompareTo(rhs.Userlocation);
        //               }
        //             );
        //            break;
        //        case "Userfirstname":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Userfirstname.CompareTo(rhs.Userfirstname);
        //               }
        //             );
        //            break;
        //        case "Userlastname":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Userlastname.CompareTo(rhs.Userlastname);
        //               }
        //             );
        //            break;
        //        case "Userzipcode":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Userzipcode.CompareTo(rhs.Userzipcode);
        //               }
        //             );
        //            break;
        //        case "Userbio":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Userbio.CompareTo(rhs.Userbio);
        //               }
        //             );
        //            break;
        //        case "Usershareinfo":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Usershareinfo.CompareTo(rhs.Usershareinfo);
        //               }
        //             );
        //            break;
        //        case "Confirmeddate":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Confirmeddate.CompareTo(rhs.Confirmeddate);
        //               }
        //             );
        //            break;
        //        case "Vistaslotsid":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Vistaslotsid.CompareTo(rhs.Vistaslotsid);
        //               }
        //             );
        //            break;
        //        case "Fullnameusernamezipcode":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Fullnameusernamezipcode.CompareTo(rhs.Fullnameusernamezipcode);
        //               }
        //             );
        //            break;
        //        case "Vistaonly":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Vistaonly.CompareTo(rhs.Vistaonly);
        //               }
        //             );
        //            break;
        //        case "Saturdayclasses":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Saturdayclasses.CompareTo(rhs.Saturdayclasses);
        //               }
        //             );
        //            break;
        //        case "Sundayclasses":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Sundayclasses.CompareTo(rhs.Sundayclasses);
        //               }
        //             );
        //            break;
        //        case "Desktoporlaptopforvista":
        //            comparison = new Comparison<DataObjectAttendees>(
        //               delegate(DataObjectAttendees lhs, DataObjectAttendees rhs)
        //               {
        //                   return lhs.Desktoporlaptopforvista.CompareTo(rhs.Desktoporlaptopforvista);
        //               }
        //             );
        //            break;
        //    }
        //    if (comparison != null)
        //    {
        //        DataTemplateODSList.Sort(comparison);
        //        if (sortData.ToLower().EndsWith("desc"))
        //        {
        //            DataTemplateODSList.Reverse();
        //        }
        //    }
        //    return DataTemplateODSList;
        //}


        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void UpdateSpecial(string username, int vistaslotsid, string email, string userwebsite,
                                  string userfirstname, string userlastname, string userzipcode, string userbio,
                                  bool usershareinfo, bool vistaonly, bool saturdaydinner, bool saturdayclasses,
                                  bool sundayclasses, string desktoporlaptopforvista)
        {
            var connection = new SqlConnection(connectionString);
            string updateString =
                "UPDATE Attendees SET VistaSlotsId = @vistaslotsid,SaturdayDinner = @saturdaydinner, Email = @email,UserWebsite = @userwebsite,UserFirstName = @userfirstname,UserLastName = @userlastname,UserZipCode = @userzipcode,UserBio = @userbio,UserShareInfo = @usershareinfo,VistaOnly = @vistaonly,SaturdayClasses = @saturdayclasses,SundayClasses = @sundayclasses,DesktopOrLaptopForVista = @desktoporlaptopforvista WHERE Username = @username";
            //string updateString = "Update Attendees Set VistaSlotsId = @vistaslotsid WHERE username = 'pkellner' ";
            var cmd = new SqlCommand(updateString, connection);
            cmd.Parameters.Add("@username", SqlDbType.VarChar, 255).Value = username == null ? String.Empty : username;
            cmd.Parameters.Add("@email", SqlDbType.VarChar, 100).Value = email == null ? String.Empty : email;
            cmd.Parameters.Add("@userwebsite", SqlDbType.VarChar, 256).Value = userwebsite == null
                                                                                   ? String.Empty
                                                                                   : userwebsite;
            //cmd.Parameters.Add("@userimage", SqlDbType.Image, 2147483647).Value = userimage == null ? new SqlBytes() : userimage;
            cmd.Parameters.Add("@userfirstname", SqlDbType.VarChar, 128).Value = userfirstname == null
                                                                                     ? String.Empty
                                                                                     : userfirstname;
            cmd.Parameters.Add("@userlastname", SqlDbType.VarChar, 128).Value = userlastname == null
                                                                                    ? String.Empty
                                                                                    : userlastname;
            cmd.Parameters.Add("@userzipcode", SqlDbType.VarChar, 128).Value = userzipcode == null
                                                                                   ? String.Empty
                                                                                   : userzipcode;
            cmd.Parameters.Add("@userbio", SqlDbType.VarChar, 2147483647).Value = userbio == null
                                                                                      ? String.Empty
                                                                                      : userbio;
            cmd.Parameters.Add("@usershareinfo", SqlDbType.Bit, 1).Value = usershareinfo;
            cmd.Parameters.Add("@vistaslotsid", SqlDbType.Int, 4).Value = vistaslotsid;
            cmd.Parameters.Add("@vistaonly", SqlDbType.Bit, 1).Value = vistaonly;
            cmd.Parameters.Add("@saturdaydinner", SqlDbType.Bit, 1).Value = saturdaydinner;
            cmd.Parameters.Add("@saturdayclasses", SqlDbType.Bit, 1).Value = saturdayclasses;
            cmd.Parameters.Add("@sundayclasses", SqlDbType.Bit, 1).Value = sundayclasses;
            cmd.Parameters.Add("@desktoporlaptopforvista", SqlDbType.VarChar, 10).Value = desktoporlaptopforvista ==
                                                                                          null
                                                                                              ? String.Empty
                                                                                              : desktoporlaptopforvista;

            try
            {
                connection.Open();
                int numRet = cmd.ExecuteNonQuery();
            }
            catch (SqlException err)
            {
                throw new ApplicationException("TestODS Update Error." + err);
            }
            finally
            {
                connection.Close();
            }
        }

        // update PGK
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void UpdateBlobsAttendees(SqlBytes userimage, string username)
        {
            // invalidate cache: DisplayImageWithParams_PKID=8b86c0b3-11d2-4fd3-b3d5-1a4ce0f47d5f
            string pkidString = Utils.GetAttendeePKIDByUsername(username);
            string cacheString = "DisplayImageWithParams_PKID=" + pkidString;
            HttpContext.Current.Cache.Remove(cacheString);


            var connection = new SqlConnection(connectionString);
            string updateString = "UPDATE [dbo].[Attendees] SET UserImage = @userimage WHERE username = @username";
            var cmd = new SqlCommand(updateString, connection);
            cmd.Parameters.Add("@userimage", SqlDbType.Image, 2147483647).Value = userimage == null
                                                                                      ? new SqlBytes()
                                                                                      : userimage;
            cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException err)
            {
                throw new ApplicationException("TestODS Update Error." + err);
            }
            finally
            {
                connection.Close();
            }
        }

        #region Nested type: DataObjectAttendees

        public class DataObjectAttendees
        {
            public DataObjectAttendees()
            {
            }

            public DataObjectAttendees(string username, string applicationname, string email, string comment,
                                       string password, string passwordquestion, string passwordanswer, bool isapproved,
                                       DateTime lastactivitydate, DateTime lastlogindate, DateTime creationdate,
                                       bool isonline, bool islockedout, DateTime lastlockedoutdate,
                                       int failedpasswordattemptcount, DateTime failedpasswordattemptwindowstart,
                                       int failedpasswordanswerattemptcount,
                                       DateTime failedpasswordanswerattemptwindowstart, DateTime lastpasswordchangeddate,
                                       string userwebsite, string userlocation, SqlBytes userimage, string userfirstname,
                                       string userlastname, string userzipcode, string userbio, bool usershareinfo,
                                       Guid referralguid, DateTime confirmeddate, int vistaslotsid,
                                       string fullnameusernamezipcode, bool vistaonly, bool saturdaydinner,
                                       bool saturdayclasses, bool sundayclasses, string desktoporlaptopforvista,
                                       Guid pkid)
            {
                Username = username;
                Applicationname = applicationname;
                Email = email;
                Comment = comment;
                Password = password;
                Passwordquestion = passwordquestion;
                Passwordanswer = passwordanswer;
                Isapproved = isapproved;
                Lastactivitydate = lastactivitydate;
                Lastlogindate = lastlogindate;
                Creationdate = creationdate;
                Isonline = isonline;
                Islockedout = islockedout;
                Lastlockedoutdate = lastlockedoutdate;
                Failedpasswordattemptcount = failedpasswordattemptcount;
                Failedpasswordattemptwindowstart = failedpasswordattemptwindowstart;
                Failedpasswordanswerattemptcount = failedpasswordanswerattemptcount;
                Failedpasswordanswerattemptwindowstart = failedpasswordanswerattemptwindowstart;
                Lastpasswordchangeddate = lastpasswordchangeddate;
                Userwebsite = userwebsite;
                Userlocation = userlocation;
                Userimage = userimage;
                Userfirstname = userfirstname;
                Userlastname = userlastname;
                Userzipcode = userzipcode;
                Userbio = userbio;
                Usershareinfo = usershareinfo;
                Referralguid = referralguid;
                Confirmeddate = confirmeddate;
                Vistaslotsid = vistaslotsid;
                Fullnameusernamezipcode = fullnameusernamezipcode;
                Vistaonly = vistaonly;
                Saturdayclasses = saturdayclasses;
                Sundayclasses = sundayclasses;
                Desktoporlaptopforvista = desktoporlaptopforvista;
                Saturdaydinner = saturdaydinner;
                Pkid = pkid;
            }

            [DataObjectField(false, false, true)]
            public string Username { get; set; }

            [DataObjectField(false, false, true)]
            public string Applicationname { get; set; }

            [DataObjectField(false, false, true)]
            public string Email { get; set; }

            [DataObjectField(false, false, true)]
            public string Comment { get; set; }

            [DataObjectField(false, false, true)]
            public string Password { get; set; }

            [DataObjectField(false, false, true)]
            public string Passwordquestion { get; set; }

            [DataObjectField(false, false, true)]
            public string Passwordanswer { get; set; }

            [DataObjectField(false, false, true)]
            public bool Isapproved { get; set; }

            [DataObjectField(false, false, true)]
            public DateTime Lastactivitydate { get; set; }

            [DataObjectField(false, false, true)]
            public DateTime Lastlogindate { get; set; }

            [DataObjectField(false, false, true)]
            public DateTime Creationdate { get; set; }

            [DataObjectField(false, false, true)]
            public bool Isonline { get; set; }

            [DataObjectField(false, false, true)]
            public bool Islockedout { get; set; }

            [DataObjectField(false, false, true)]
            public DateTime Lastlockedoutdate { get; set; }

            [DataObjectField(false, false, true)]
            public int Failedpasswordattemptcount { get; set; }

            [DataObjectField(false, false, true)]
            public DateTime Failedpasswordattemptwindowstart { get; set; }

            [DataObjectField(false, false, true)]
            public int Failedpasswordanswerattemptcount { get; set; }

            [DataObjectField(false, false, true)]
            public DateTime Failedpasswordanswerattemptwindowstart { get; set; }

            [DataObjectField(false, false, true)]
            public DateTime Lastpasswordchangeddate { get; set; }

            [DataObjectField(false, false, true)]
            public string Userwebsite { get; set; }

            [DataObjectField(false, false, true)]
            public string Userlocation { get; set; }

            [DataObjectField(false, false, true)]
            public SqlBytes Userimage { get; set; }

            [DataObjectField(false, false, true)]
            public string Userfirstname { get; set; }

            [DataObjectField(false, false, true)]
            public string Userlastname { get; set; }

            [DataObjectField(false, false, true)]
            public string Userzipcode { get; set; }

            [DataObjectField(false, false, true)]
            public string Userbio { get; set; }

            [DataObjectField(false, false, true)]
            public bool Usershareinfo { get; set; }

            [DataObjectField(false, false, true)]
            public Guid Referralguid { get; set; }

            [DataObjectField(false, false, true)]
            public DateTime Confirmeddate { get; set; }

            [DataObjectField(false, false, true)]
            public int Vistaslotsid { get; set; }

            [DataObjectField(false, false, true)]
            public string Fullnameusernamezipcode { get; set; }

            [DataObjectField(false, false, true)]
            public bool Vistaonly { get; set; }

            [DataObjectField(false, false, true)]
            public bool Saturdaydinner { get; set; }

            [DataObjectField(false, false, true)]
            public bool Saturdayclasses { get; set; }

            [DataObjectField(false, false, true)]
            public bool Sundayclasses { get; set; }

            [DataObjectField(false, false, true)]
            public string Desktoporlaptopforvista { get; set; }

            [DataObjectField(true, true, false)]
            public Guid Pkid { get; set; }

            [DataObjectField(false,false,false)]
            public int Id { get; set; }
        }

        #endregion

        //[DataObjectMethod(DataObjectMethodType.Delete, true)]
        //public void Delete(Guid pkid, Guid original_pkid)
        //{
        //    SqlConnection connection = new SqlConnection(connectionString);
        //    string deleteString = "DELETE FROM [dbo].[Attendees] WHERE pkid = @original_pkid";
        //    SqlCommand cmd = new SqlCommand(deleteString, connection);

        //    cmd.Parameters.Add("@original_pkid", SqlDbType.UniqueIdentifier, 16).Value = pkid == new Guid("00000000-0000-0000-0000-000000000000") ? original_pkid : pkid;
        //    try
        //    {
        //        connection.Open();
        //        cmd.ExecuteNonQuery();
        //    }
        //    catch (SqlException err)
        //    {
        //        throw new ApplicationException("TestODS Delete Error." + err.ToString());
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //}
    }
}