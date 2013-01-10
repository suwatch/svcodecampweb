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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Web.Security;
using System.Web.Configuration;

using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using System.Web;

namespace CodeCampSV
{
    [DataObject(true)]  // This attribute allows the ObjectDataSource wizard to see this class
    public class SessionsOverviewODS
    {
        string connectionString;
        public SessionsOverviewODS()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString;
        }
        public class DataObjectSessionsOverview
        {
            public DataObjectSessionsOverview() { }
            public DataObjectSessionsOverview(string userfirstname, string userlastname, string number, int capacity, int sessionid, int sessiontimesid, DateTime starttime, string title)
            {
                this.userfirstname = userfirstname;
                this.userlastname = userlastname;
                this.number = number;
                this.capacity = capacity;
                this.sessionid = sessionid;
                this.sessiontimesid = sessiontimesid;
                this.starttime = starttime;
                this.title = title;
            }

            private string userfirstname;
            [DataObjectField(false, false, true)]
            public string Userfirstname
            {
                get { return userfirstname; }
                set { userfirstname = value; }
            }

            private string userlastname;
            [DataObjectField(false, false, true)]
            public string Userlastname
            {
                get { return userlastname; }
                set { userlastname = value; }
            }

            private string number;
            [DataObjectField(false, false, true)]
            public string Number
            {
                get { return number; }
                set { number = value; }
            }

            private int capacity;
            [DataObjectField(false, false, true)]
            public int Capacity
            {
                get { return capacity; }
                set { capacity = value; }
            }

            private int sessionid;
            [DataObjectField(false, false, false)]
            public int Sessionid
            {
                get { return sessionid; }
                set { sessionid = value; }
            }

            private int sessiontimesid;
            [DataObjectField(false, false, false)]
            public int Sessiontimesid
            {
                get { return sessiontimesid; }
                set { sessiontimesid = value; }
            }

            private DateTime starttime;
            [DataObjectField(false, false, true)]
            public DateTime Starttime
            {
                get { return starttime; }
                set { starttime = value; }
            }

            private string title;
            [DataObjectField(true, true, true)]
            public string Title
            {
                get { return title; }
                set { title = value; }
            }


        }


        public List<DataObjectSessionsOverview> GetBySessionTimesId(int sessionTimesId)
        {
            return GetBySessionTimesId(sessionTimesId, Utils.CurrentCodeCampYear);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectSessionsOverview> GetBySessionTimesId(int sessionTimesId,int codeCampYearId)
        {
            int currentCodeCampYearId = Utils.GetCurrentCodeCampYear();

            List<DataObjectSessionsOverview> DataTemplateODSList = null;
            string cacheName = String.Format("{0}-{1}-{2}", CodeCampSV.Utils.CacheBySessionTimesId, sessionTimesId, currentCodeCampYearId);


            if (HttpContext.Current.Cache[cacheName] == null)
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                DataTemplateODSList = new List<DataObjectSessionsOverview>();
                SqlDataReader reader = null;
                string sqlSelectString =
                    @"SELECT UserFirstName,
                       UserLastName,
                       Number,
                       Capacity,
                       SessionId,
                       SessionTimesId,
                       StartTime,
                       title
                FROM SessionsOverview
                WHERE SessionTimesId = @SessionTimesId AND CodeCampYearId = @CodeCampYearId
                ORDER BY title";

                SqlCommand cmd = new SqlCommand(sqlSelectString, conn);
                cmd.Parameters.Add("@CodeCampYearId", SqlDbType.Int).Value = codeCampYearId;
                cmd.Parameters.Add("@SessionTimesId", SqlDbType.Int).Value = sessionTimesId;
                reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        string userfirstname = reader.IsDBNull(0) ? "" : reader.GetString(0);
                        string userlastname = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        string number = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        int capacity = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                        int sessionid = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                        int sessiontimesid = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
                        DateTime starttime = reader.IsDBNull(6) ? DateTime.Now : reader.GetDateTime(6);
                        string title = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        DataObjectSessionsOverview td = new DataObjectSessionsOverview(userfirstname, userlastname, number, capacity, sessionid, sessiontimesid, starttime, title);
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
                DataTemplateODSList = (List<DataObjectSessionsOverview>)HttpContext.Current.Cache[cacheName];
            }

            return DataTemplateODSList;
        }


        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<DataObjectSessionsOverview> GetAllSessionsOverview(string sortData)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            List<DataObjectSessionsOverview> DataTemplateODSList = new List<DataObjectSessionsOverview>();
            SqlDataReader reader = null;
            string sqlSelectString = "SELECT UserFirstName,UserLastName,Number,Capacity,SessionId,SessionTimesId,StartTime,title FROM [dbo].[SessionsOverview] ";
            SqlCommand cmd = new SqlCommand(sqlSelectString, conn);
            reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    string userfirstname = reader.IsDBNull(0) ? "" : reader.GetString(0);
                    string userlastname = reader.IsDBNull(1) ? "" : reader.GetString(1);
                    string number = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    int capacity = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                    int sessionid = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                    int sessiontimesid = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
                    DateTime starttime = reader.IsDBNull(6) ? DateTime.Now : reader.GetDateTime(6);
                    string title = reader.IsDBNull(7) ? "" : reader.GetString(7);
                    DataObjectSessionsOverview td = new DataObjectSessionsOverview(userfirstname, userlastname, number, capacity, sessionid, sessiontimesid, starttime, title);
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
                sortData = "Title";
            }
            if (sortData.Length == 0)
            {
                sortData = "Title";
            }
            string sortDataBase = sortData;
            string descString = " DESC";
            if (sortData.EndsWith(descString))
            {
                sortDataBase = sortData.Substring(0, sortData.Length - descString.Length);
            }
            Comparison<DataObjectSessionsOverview> comparison = null;
            switch (sortDataBase)
            {
                case "Userfirstname":
                    comparison = delegate(DataObjectSessionsOverview lhs, DataObjectSessionsOverview rhs)
                                 {
                                     return lhs.Userfirstname.CompareTo(rhs.Userfirstname);
                                 };
                    break;
                case "Userlastname":
                    comparison = new Comparison<DataObjectSessionsOverview>(
                       delegate(DataObjectSessionsOverview lhs, DataObjectSessionsOverview rhs)
                       {
                           return lhs.Userlastname.CompareTo(rhs.Userlastname);
                       }
                     );
                    break;
                case "Number":
                    comparison = new Comparison<DataObjectSessionsOverview>(
                       delegate(DataObjectSessionsOverview lhs, DataObjectSessionsOverview rhs)
                       {
                           return lhs.Number.CompareTo(rhs.Number);
                       }
                     );
                    break;
                case "Capacity":
                    comparison = new Comparison<DataObjectSessionsOverview>(
                       delegate(DataObjectSessionsOverview lhs, DataObjectSessionsOverview rhs)
                       {
                           return lhs.Capacity.CompareTo(rhs.Capacity);
                       }
                     );
                    break;
                case "Sessionid":
                    comparison = new Comparison<DataObjectSessionsOverview>(
                       delegate(DataObjectSessionsOverview lhs, DataObjectSessionsOverview rhs)
                       {
                           return lhs.Sessionid.CompareTo(rhs.Sessionid);
                       }
                     );
                    break;
                case "Sessiontimesid":
                    comparison = new Comparison<DataObjectSessionsOverview>(
                       delegate(DataObjectSessionsOverview lhs, DataObjectSessionsOverview rhs)
                       {
                           return lhs.Sessiontimesid.CompareTo(rhs.Sessiontimesid);
                       }
                     );
                    break;
                case "Starttime":
                    comparison = new Comparison<DataObjectSessionsOverview>(
                       delegate(DataObjectSessionsOverview lhs, DataObjectSessionsOverview rhs)
                       {
                           return lhs.Starttime.CompareTo(rhs.Starttime);
                       }
                     );
                    break;
                case "Title":
                    comparison = new Comparison<DataObjectSessionsOverview>(
                       delegate(DataObjectSessionsOverview lhs, DataObjectSessionsOverview rhs)
                       {
                           return lhs.Title.CompareTo(rhs.Title);
                       }
                     );
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


       
    }
}





