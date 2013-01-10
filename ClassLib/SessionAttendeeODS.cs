


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Web.Security;
using System.Web.Configuration;

using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
//using System.Transactions;
using System.Web;
using System.Configuration;
using System.Transactions;

namespace CodeCampSV
{
    [DataObject(true)]  // This attribute allows the ObjectDataSource wizard to see this class
    public class SessionAttendeeODS
    {
        string connectionString;
        public SessionAttendeeODS()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString;
        }
        public class DataObjectSessionAttendee
        {
            public DataObjectSessionAttendee() { }
            public DataObjectSessionAttendee(int sessions_id, Guid attendees_username, int interestlevel, int id)
            {
                this.sessions_id = sessions_id;
                this.attendees_username = attendees_username;
                this.interestlevel = interestlevel;
                this.id = id;
            }

            private int sessions_id;
            [DataObjectField(false, false, false)]
            public int Sessions_id
            {
                get { return sessions_id; }
                set { sessions_id = value; }
            }

            private Guid attendees_username;
            [DataObjectField(false, false, false)]
            public Guid Attendees_username
            {
                get { return attendees_username; }
                set { attendees_username = value; }
            }

            private int interestlevel;
            [DataObjectField(false, false, false)]
            public int Interestlevel
            {
                get { return interestlevel; }
                set { interestlevel = value; }
            }

            private int id;
            [DataObjectField(true, true, false)]
            public int Id
            {
                get { return id; }
                set { id = value; }
            }
        }

        /// <summary>
        /// Figure out how many are in each session based on interest level
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="interestLevel"></param>
        /// <returns></returns>
        public int GetCountBySessionIdAndInterest(int sessionId, int interestLevel)
        {
            List<DataObjectSessionAttendee> liSessionAttendees = GetBySessionId(sessionId);
            int cntFnd = 0;
            foreach (DataObjectSessionAttendee sa in liSessionAttendees)
            {
                if (sa.Interestlevel == interestLevel)
                {
                    cntFnd++;
                }
            }
            return cntFnd;
        }


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectSessionAttendee> GetBySessionId(int searchsessions_id)
        {
            List<DataObjectSessionAttendee> DataTemplateODSList = null;
            string cacheName = CodeCampSV.Utils.CacheSessionAttendeeBySessionId + "_" + searchsessions_id.ToString();

            //$$$ PROBLEM, MOVE THIS OUT OF IF STATMENT
            if (HttpContext.Current.Cache[cacheName] == null)
            {

                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                DataTemplateODSList = new List<DataObjectSessionAttendee>();
                SqlDataReader reader = null;
                string sqlSelectString = "SELECT sessions_id,attendees_username,interestlevel,id FROM [dbo].[SessionAttendee]  WHERE sessions_id = @searchsessions_id";
                SqlCommand cmd = new SqlCommand(sqlSelectString, conn);
                cmd.Parameters.Add("@searchsessions_id", SqlDbType.Int, 4).Value = searchsessions_id; ;
                reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        int sessions_id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        Guid attendees_username = reader.IsDBNull(1) ? Guid.NewGuid() : reader.GetGuid(1);
                        int interestlevel = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                        int id = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                        DataObjectSessionAttendee td = new DataObjectSessionAttendee(sessions_id, attendees_username, interestlevel, id);
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
                DataTemplateODSList = (List<DataObjectSessionAttendee>)HttpContext.Current.Cache[cacheName];
            }

            return DataTemplateODSList;
        }

        //[DataObjectMethod(DataObjectMethodType.Select, false)]
        //public List<DataObjectSessionAttendee> GetByUsername(string username)
        //{
        //    return GetByUsername(username, "All");
        //}

        /// <summary>
        ///          interestLevel:
        ///          <asp:ListItem Selected="True">All</asp:ListItem>
        //           <asp:ListItem>Interested</asp:ListItem>
        //           <asp:ListItem>Plan To Attend</asp:ListItem>
        //           <asp:ListItem>I & P2A</asp:ListItem>
        /// </summary>
        /// <param name="username"></param>
        /// <param name="interestLevel"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectSessionAttendee> GetByUsername(string username)
        {
            List<DataObjectSessionAttendee> DataTemplateODSList = new List<DataObjectSessionAttendee>();
            string cacheName = CodeCampSV.Utils.CacheSessionAttendeeByUsername + "_" + username ;
            if (HttpContext.Current.Cache[cacheName] == null)
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                DataTemplateODSList = new List<DataObjectSessionAttendee>();
                SqlDataReader reader = null;
                string sqlSelectString =
                @"
                 SELECT sessions_id,
                       attendees_username,
                       interestlevel,
                       id
                 FROM SessionAttendee
                 WHERE attendees_username =
                       (select PKID from Attendees WHERE Username = @Username)
                ";

                SqlCommand cmd = new SqlCommand(sqlSelectString, conn);
                cmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
                reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        int sessions_id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        Guid attendees_username = reader.IsDBNull(1) ? Guid.NewGuid() : reader.GetGuid(1);
                        int interestlevel = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                        int id = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                        var td = new DataObjectSessionAttendee(sessions_id, attendees_username, interestlevel, id);
                        DataTemplateODSList.Add(td);

                        //// interested = 2; plan to attend  = 3
                        //if (radioButtonInterestLevel.Equals("All"))
                        //{
                        //    DataTemplateODSList.Add(td);
                        //}
                        //else if (radioButtonInterestLevel.Equals("Interested"))
                        //{
                        //    if (interestlevel == 2)
                        //    {
                        //        DataTemplateODSList.Add(td);
                        //    }
                        //}
                        //else if (radioButtonInterestLevel.Equals("Plan To Attend"))
                        //{
                        //    if (interestlevel == 3)
                        //    {
                        //        DataTemplateODSList.Add(td);
                        //    }
                        //}
                        //else if (radioButtonInterestLevel.Equals("I & P2A"))
                        //{
                        //    if (interestlevel == 3 || interestlevel == 2)
                        //    {
                        //        DataTemplateODSList.Add(td);
                        //    }
                        //}
                    }
                }
                finally
                {
                    if (reader != null) reader.Close();
                }
                conn.Close();

                // DON'T CACHE THIS BECAUSE PERSON MAY BE CHANGING SELECTIONS, PROBABLY SHOULD NOT EVEN HAVE I LOOP
                int cacheTime = Utils.RetrieveSecondsForSessionCacheTimeout();
                cacheTime = 3; // make small just in case site hammered
                HttpContext.Current.Cache.Insert(cacheName, DataTemplateODSList,
                    null, DateTime.Now.Add(new TimeSpan(0, 0,cacheTime)), TimeSpan.Zero);
            }
            else
            {
                DataTemplateODSList = (List<DataObjectSessionAttendee>)HttpContext.Current.Cache[cacheName];
            }
            return DataTemplateODSList;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectSessionAttendee> GetAll()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            List<DataObjectSessionAttendee> DataTemplateODSList = new List<DataObjectSessionAttendee>();
            SqlDataReader reader = null;
            string sqlSelectString = "SELECT sessions_id,attendees_username,interestlevel,id FROM SessionAttendee";
            SqlCommand cmd = new SqlCommand(sqlSelectString, conn);
            reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    int sessions_id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    Guid attendees_username = reader.IsDBNull(1) ? Guid.NewGuid() : reader.GetGuid(1);
                    int interestlevel = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                    int id = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                    DataObjectSessionAttendee td = new DataObjectSessionAttendee(sessions_id, attendees_username, interestlevel, id);
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



        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public void Update(int sessions_id, string username, int interestlevel)
        {
            if (interestlevel == Convert.ToInt32(CodeCampSV.Utils.InterestLevel.WillAttend))
            {

                UpdateInterestLevel(sessions_id, username, CodeCampSV.Utils.InterestLevel.WillAttend);
                // Need to make sure that no other WillAttends for sessions at the same time
                // If so, then make the other ones "interested"
                //$$$$$

                //using (TransactionScope scope = new TransactionScope())
                //{
                //    using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
                //    {
                //        try
                //        {
                //            sqlConnection.Open();
                //        }
                //        catch (Exception eee)
                //        {
                //            throw new ApplicationException(eee.ToString());
                //        }
                //        scope.Complete();
                //        sqlConnection.Close();
                //    }
                //    UpdateInterestLevel(sessions_id, username, CodeCampSV.Utils.InterestLevel.WillAttend);
                //}
            }


            if (interestlevel == Convert.ToInt32(CodeCampSV.Utils.InterestLevel.NotInterested))
            {
                // for not interested, just need to delete the record.
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();

                string sqlDeleteString =
                @"DELETE
                FROM SessionAttendee
                WHERE sessions_id = @sessions_id AND
                        attendees_username =
                        (select PKID from Attendees WHERE Username = @Username)";

                SqlCommand cmd = new SqlCommand(sqlDeleteString, conn);
                cmd.Parameters.Add("@sessions_id", SqlDbType.Int).Value = sessions_id;
                cmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
                try
                {
                    int numUpdated = (int)cmd.ExecuteNonQuery();
                }
                catch (Exception eeee)
                {
                    throw new ApplicationException(eeee.ToString());
                }
                conn.Close();
            }
            else if (interestlevel == Convert.ToInt32(CodeCampSV.Utils.InterestLevel.Interested))
            {
                // for interested, need to update the record to reflect interested
                // Easiest thing to do is delete previous entry and re-add. maybe more
                // efficient way to do this, but this will always work.
                UpdateInterestLevel(sessions_id, username, CodeCampSV.Utils.InterestLevel.Interested);
            }

            try
            {
                string cacheName = CodeCampSV.Utils.CacheSessionAttendeeByUsername + "_" + username;
                if (HttpContext.Current.Cache[cacheName] != null)
                {
                    HttpContext.Current.Cache.Remove(cacheName);
                }

                cacheName = CodeCampSV.Utils.CacheSessionAttendeeBySessionId + "_" + sessions_id.ToString();
                if (HttpContext.Current.Cache[cacheName] != null)
                {
                    HttpContext.Current.Cache.Remove(cacheName);
                }
            }
            catch (Exception e7)
            {
                throw new ApplicationException(e7.ToString());
            }
        }


        /// <summary>
        /// Used by Update Only
        /// </summary>
        /// <param name="sessions_id"></param>
        /// <param name="username"></param>
        /// <param name="interestLevel"></param>
        private void UpdateInterestLevel(int sessions_id, string username, Utils.InterestLevel interestLevel)
        {
            // $$$ Broken scope, will not work.  See SessionsEdit.aspx for correct way
            using (TransactionScope scope = new TransactionScope())
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();

                // If intereset level is "willattend" then need to make all sessions at this
                // same time equal to interested
                if (interestLevel == Utils.InterestLevel.WillAttend)
                {
                    const string sqlUpdateAllToInterested = @"UPDATE SessionAttendee
                    SET interestlevel = 2,LastUpdatedDate = GETUTCDATE(),UpdateByProgram='SessionAttendeeODS'
                    WHERE interestlevel = 3 AND attendees_username =
                          (select PKID from attendees WHERE Username = @Username) AND
                          (sessions_id IN (select id from sessions WHERE sessionTimesId =(select
                          sessiontimesid FROM sessions WHERE id = @sessionid)))";


                    SqlCommand cmdUpdateInterest = new SqlCommand(sqlUpdateAllToInterested, conn);
                    cmdUpdateInterest.Parameters.Add("@sessionid", SqlDbType.Int).Value = sessions_id;
                    cmdUpdateInterest.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
                    int numUpdated = cmdUpdateInterest.ExecuteNonQuery();
                }

                //





                // Grab PKID of username
                string sqlSelectString = "SELECT PKID from Attendees WHERE Username=@Username";
                SqlCommand cmd0 = new SqlCommand(sqlSelectString, conn);
                cmd0.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
                Guid PKID = (Guid)cmd0.ExecuteScalar();

                // cut and paste sin committed for next few delete lines.  :(
                string sqlDeleteString =
                @"DELETE
                    FROM SessionAttendee
                    WHERE sessions_id = @sessions_id AND
                            attendees_username = @PKID";

                var cmd1 = new SqlCommand(sqlDeleteString, conn);
                cmd1.Parameters.Add("@sessions_id", SqlDbType.Int).Value = sessions_id;
                cmd1.Parameters.Add("@PKID", SqlDbType.UniqueIdentifier).Value = PKID;
                try
                {
                    int numUpdated = (int)cmd1.ExecuteNonQuery();
                }
                catch (Exception eeee)
                {
                    throw new ApplicationException(eeee.ToString());
                }

                // Now, insert new record
                string sqlInsertString =
                @"INSERT INTO
                        SessionAttendee(
                          LastUpdatedDate,
                          sessions_id,
                          attendees_username,
                          interestlevel)
                        VALUES(
                          GETDATE(),
                          @sessions_id,
                          @attendees_username,
                          @interestlevel)";
                SqlCommand cmd2 = new SqlCommand(sqlInsertString, conn);
                cmd2.Parameters.Add("@sessions_id", SqlDbType.Int).Value = sessions_id;
                cmd2.Parameters.Add("@attendees_username", SqlDbType.UniqueIdentifier).Value = PKID;
                cmd2.Parameters.Add("@interestlevel", SqlDbType.Int).Value = interestLevel;
                try
                {
                    int numInserted = cmd2.ExecuteNonQuery();
                }
                catch (Exception eeeee)
                {
                    throw new ApplicationException(eeeee.ToString());
                }

                scope.Complete();
                conn.Close();
            }
        }

        //[DataObjectMethod(DataObjectMethodType.Delete, true)]
        //public void Delete(int id, int original_id)
        //{
        //    SqlConnection connection = new SqlConnection(connectionString);
        //    string deleteString = "DELETE FROM [dbo].[SessionAttendee] WHERE id = @original_id";
        //    SqlCommand cmd = new SqlCommand(deleteString, connection);

        //    cmd.Parameters.Add("@original_id", SqlDbType.Int, 4).Value = id == 0 ? original_id : id;
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



