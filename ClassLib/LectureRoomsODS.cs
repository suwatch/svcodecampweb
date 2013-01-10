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
    public class LectureRoomsODS
    {
        string connectionString;
        public LectureRoomsODS()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString;
        }
        public class DataObjectLectureRooms
        {
            public DataObjectLectureRooms() { }
            public DataObjectLectureRooms(string number, string description, string style, int capacity, bool projector, bool screen, SqlBytes picture, int id)
            {
                this.number = number;
                this.description = description;
                this.style = style;
                this.capacity = capacity;
                this.projector = projector;
                this.screen = screen;
                this.picture = picture;
                this.id = id;
            }

            private string number;
            [DataObjectField(false, false, true)]
            public string Number
            {
                get { return number; }
                set { number = value; }
            }

            private string description;
            [DataObjectField(false, false, true)]
            public string Description
            {
                get { return description; }
                set { description = value; }
            }

            private string style;
            [DataObjectField(false, false, true)]
            public string Style
            {
                get { return style; }
                set { style = value; }
            }

            private int capacity;
            [DataObjectField(false, false, true)]
            public int Capacity
            {
                get { return capacity; }
                set { capacity = value; }
            }

            private bool projector;
            [DataObjectField(false, false, true)]
            public bool Projector
            {
                get { return projector; }
                set { projector = value; }
            }

            private bool screen;
            [DataObjectField(false, false, true)]
            public bool Screen
            {
                get { return screen; }
                set { screen = value; }
            }

            private SqlBytes picture;
            [DataObjectField(false, false, true)]
            public SqlBytes Picture
            {
                get { return picture; }
                set { picture = value; }
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
        public List<DataObjectLectureRooms> GetAllLectureRooms()
        {
            string cacheName = CodeCampSV.Utils.CacheLectureRooms;
            List<DataObjectLectureRooms> DataTemplateODSList = null;

            if (HttpContext.Current.Cache[cacheName] == null)
            {

                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                DataTemplateODSList = new List<DataObjectLectureRooms>();
                SqlDataReader reader = null;
                string sqlSelectString = "SELECT Number,Description,Style,Capacity,Projector,Screen,picture,id FROM [dbo].[LectureRooms] ";
                SqlCommand cmd = new SqlCommand(sqlSelectString, conn);
                reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        string number = reader.IsDBNull(0) ? "" : reader.GetString(0);
                        string description = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        string style = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        int capacity = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                        bool projector = reader.IsDBNull(4) ? false : reader.GetBoolean(4);
                        bool screen = reader.IsDBNull(5) ? false : reader.GetBoolean(5);
                        SqlBytes picture = reader.IsDBNull(6) ? new SqlBytes() : reader.GetSqlBytes(6);
                        int id = reader.IsDBNull(7) ? 0 : reader.GetInt32(7);
                        DataObjectLectureRooms td = new DataObjectLectureRooms(number, description, style, capacity, projector, screen, picture, id);
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
                DataTemplateODSList = (List<DataObjectLectureRooms>)HttpContext.Current.Cache[cacheName];
            }

            return DataTemplateODSList;
        }
    }
}





