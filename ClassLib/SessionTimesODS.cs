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
    public class SessionTimesODS
    {
        string connectionString;
        public SessionTimesODS()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString;
        }
        public class DataObjectSessionTimes
        {
            public DataObjectSessionTimes() { }
            public DataObjectSessionTimes(DateTime starttime, string starttimefriendly, DateTime endtime, string endtimefriendly, int sessionminutes, string description, int id,string titleBefore,string titleAfter)
            {
                this.starttime = starttime;
                this.starttimefriendly = starttimefriendly;
                this.endtime = endtime;
                this.endtimefriendly = endtimefriendly;
                this.sessionminutes = sessionminutes;
                this.description = description;
                this.id = id;
                this.titleBeforeOnAgenda = titleBefore;
                this.titleAfterOnAgenda = titleAfter;

            }

            private DateTime starttime;
            [DataObjectField(false, false, true)]
            public DateTime Starttime
            {
                get { return starttime; }
                set { starttime = value; }
            }

            private string starttimefriendly;
            [DataObjectField(false, false, true)]
            public string Starttimefriendly
            {
                get { return starttimefriendly; }
                set { starttimefriendly = value; }
            }

            private DateTime endtime;
            [DataObjectField(false, false, true)]
            public DateTime Endtime
            {
                get { return endtime; }
                set { endtime = value; }
            }

            private string endtimefriendly;
            [DataObjectField(false, false, true)]
            public string Endtimefriendly
            {
                get { return endtimefriendly; }
                set { endtimefriendly = value; }
            }

            private int sessionminutes;
            [DataObjectField(false, false, true)]
            public int Sessionminutes
            {
                get { return sessionminutes; }
                set { sessionminutes = value; }
            }

            private string description;
            [DataObjectField(false, false, true)]
            public string Description
            {
                get { return description; }
                set { description = value; }
            }

            private string titleBeforeOnAgenda;
            [DataObjectField(false, false, true)]
            public string TitleBeforeOnAgenda
            {
                get { return titleBeforeOnAgenda; }
                set { titleBeforeOnAgenda = value; }
            }

            private string titleAfterOnAgenda;
            [DataObjectField(false, false, true)]
            public string TitleAfterOnAgenda
            {
                get { return titleAfterOnAgenda; }
                set { titleAfterOnAgenda = value; }
            }

            private int id;
            [DataObjectField(true, true, false)]
            public int Id
            {
                get { return id; }
                set { id = value; }
            }


        }


        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<DataObjectSessionTimes> GetAllSessionTimes()
        {
            string cacheName = CodeCampSV.Utils.CacheSessionTimes;
            List<DataObjectSessionTimes> DataTemplateODSList = 
                (List<DataObjectSessionTimes>) HttpContext.Current.Cache[cacheName];

            if (DataTemplateODSList == null)
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                DataTemplateODSList = new List<DataObjectSessionTimes>();
                SqlDataReader reader = null;
                string sqlSelectString = "SELECT StartTime,StartTimeFriendly,EndTime,EndTimeFriendly,SessionMinutes,Description,id,TitleBeforeOnAgenda,TitleAfterOnAgenda FROM [dbo].[SessionTimes] ORDER BY StartTime ";
                SqlCommand cmd = new SqlCommand(sqlSelectString, conn);
                reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        DateTime starttime = reader.IsDBNull(0) ? DateTime.Now : reader.GetDateTime(0);
                        string starttimefriendly = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        DateTime endtime = reader.IsDBNull(2) ? DateTime.Now : reader.GetDateTime(2);
                        string endtimefriendly = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        int sessionminutes = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                        string description = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        int id = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
                        string titleBeforeOnAgenda = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        string titleAfterOnAgenda = reader.IsDBNull(8) ? "" : reader.GetString(8);
                        DataObjectSessionTimes td = new DataObjectSessionTimes(starttime, starttimefriendly, endtime, endtimefriendly, sessionminutes, description, id,titleBeforeOnAgenda,titleAfterOnAgenda);
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
                DataTemplateODSList = (List<DataObjectSessionTimes>)HttpContext.Current.Cache[cacheName];
            }

            return DataTemplateODSList;
        }
    }
}





