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
using System.Web;
using System.Web.Configuration;

namespace CodeCampSV
{
    [DataObject(true)] // This attribute allows the ObjectDataSource wizard to see this class
    public class TagsODS
    {
        private readonly string connectionString;

        public TagsODS()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectTags> GetAllBySession(string sortData, int sessionid)
        {
            return GetAllBySession(sortData, sessionid, true); // by default use cache
        }


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectTags> GetAllBySession(string sortData, int sessionid,bool useCache)
        {
            string cacheName = string.Format("{0}-{1}", Utils.CacheTagNameBySession, sessionid);
            List<DataObjectTags> DataTemplateODSList =
                (List<DataObjectTags>)HttpContext.Current.Cache[cacheName];

            if (DataTemplateODSList == null || !useCache) // force thru if not wanting cache
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    DataTemplateODSList = new List<DataObjectTags>();
                    conn.Open();
                    List<int> listOfTags = new List<int>();
                    // make this a two stepper.  first, get the assigned values, and stuff
                    // in a List, then get the full list and populate on return
                    string sqlSelectString = "SELECT tagid from SessionTags WHERE sessionid=@sessionid ";
                    using (SqlCommand cmda = new SqlCommand(sqlSelectString, conn))
                    {
                        cmda.Parameters.Add("@sessionid", SqlDbType.Int).Value = sessionid;
                        using (SqlDataReader readera = cmda.ExecuteReader())
                        {

                            try
                            {
                                while (readera.Read())
                                {
                                    listOfTags.Add(readera.GetInt32(0));
                                }
                            }
                            finally
                            {
                                if (readera != null) readera.Close();
                            }
                        }
                    }

                    sqlSelectString =
                        "SELECT TagName,TagDescription,id FROM [dbo].[Tags] ORDER BY UPPER(LTRIM(TagName)) ";
                    using (SqlCommand cmd = new SqlCommand(sqlSelectString, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            try
                            {
                                while (reader.Read())
                                {
                                    string tagname = reader.IsDBNull(0) ? "" : reader.GetString(0);
                                    string tagdescription = reader.IsDBNull(1) ? "" : reader.GetString(1);
                                    int id = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                                    Boolean sessionassigned = false;
                                    if (listOfTags.Contains(id))
                                    {
                                        var td = new DataObjectTags(tagname, tagdescription, sessionid, sessionassigned,
                                                                    id);
                                        DataTemplateODSList.Add(td);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new ApplicationException("Error InsufficientMemoryException TagsODS: " + ex);
                            }
                        }
                    }
                }
                if (useCache)
                {
                    HttpContext.Current.Cache.Insert(cacheName, DataTemplateODSList,
                                                     null,
                                                     DateTime.Now.Add(new TimeSpan(0, 0,
                                                                                   Utils.
                                                                                       RetrieveSecondsForSessionCacheTimeout
                                                                                       ())), TimeSpan.Zero);
                }
            }
            else
            {
                DataTemplateODSList = (List<DataObjectTags>)HttpContext.Current.Cache[cacheName];
            }
            return DataTemplateODSList;
        }

        /// <summary>
        /// no matter what is passed in, this returns all tags.  Go figure.
        /// </summary>
        /// <param name="sortData"></param>
        /// <param name="searchsessionid"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<DataObjectTags> GetAllTags(string sortData, int searchsessionid)
        {
            var conn = new SqlConnection(connectionString);
            conn.Open();
            var DataTemplateODSList = new List<DataObjectTags>();
            SqlDataReader reader = null;
            const string sqlSelectString = "SELECT TagName,TagDescription,id FROM [dbo].[Tags] ORDER BY UPPER(LTRIM(TagName))";
            var cmd = new SqlCommand(sqlSelectString, conn);
            reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    string tagname = reader.IsDBNull(0) ? "" : reader.GetString(0);
                    string tagdescription = reader.IsDBNull(1) ? "" : reader.GetString(1);
                    int sessionid = 0;
                    Boolean sessionassigned = false;
                    int id = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                    var td = new DataObjectTags(tagname, tagdescription, sessionid, sessionassigned, id);
                    DataTemplateODSList.Add(td);
                }
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            conn.Close();

            //// do a quick case insensative sort
            //Comparison<DataObjectTags> comparison =
            //    delegate(DataObjectTags lhs, DataObjectTags rhs) { return lhs.Tagname.ToLower().CompareTo(rhs.Tagname.ToLower()); };
            //DataTemplateODSList.Sort(comparison);


            return DataTemplateODSList;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectTags> GetByPrimaryKeyTags(string sortData, int searchid)
        {
            var conn = new SqlConnection(connectionString);
            conn.Open();
            var DataTemplateODSList = new List<DataObjectTags>();
            SqlDataReader reader = null;
            const string sqlSelectString = "SELECT TagName,TagDescription,id FROM [dbo].[Tags]  WHERE id = @searchid ";
            var cmd = new SqlCommand(sqlSelectString, conn);
            cmd.Parameters.Add("@searchid", SqlDbType.Int, 4).Value = searchid;
            ;
            reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    string tagname = reader.IsDBNull(0) ? "" : reader.GetString(0);
                    string tagdescription = reader.IsDBNull(1) ? "" : reader.GetString(1);
                    int sessionid = 0;
                    Boolean sessionassigned = false;
                    int id = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                    var td = new DataObjectTags(tagname, tagdescription, sessionid, sessionassigned, id);
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
                sortData = "Id";
            }
            if (sortData.Length == 0)
            {
                sortData = "Id";
            }
            string sortDataBase = sortData;
            string descString = " DESC";
            if (sortData.EndsWith(descString))
            {
                sortDataBase = sortData.Substring(0, sortData.Length - descString.Length);
            }
            Comparison<DataObjectTags> comparison = null;
            switch (sortDataBase)
            {
                case "Tagname":
                    comparison =
                        delegate(DataObjectTags lhs, DataObjectTags rhs) { return lhs.Tagname.ToLower().Trim().CompareTo(rhs.Tagname.ToLower().Trim()); };
                    break;
                case "Tagdescription":
                    comparison =
                        delegate(DataObjectTags lhs, DataObjectTags rhs) { return lhs.Tagdescription.CompareTo(rhs.Tagdescription); };
                    break;
                case "Sessionassigned":
                    comparison =
                        delegate(DataObjectTags lhs, DataObjectTags rhs) { return lhs.Sessionassigned.CompareTo(rhs.Sessionassigned); };
                    break;
                case "Id":
                    comparison = delegate(DataObjectTags lhs, DataObjectTags rhs) { return lhs.Id.CompareTo(rhs.Id); };
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


        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public void UpdateChecked(int idUpdated, bool sessionAssigned)
        {
            // Need to check and see if assigned to session or not, then toggle


            //SqlConnection connection = new SqlConnection(connectionString);
            //string updateString = "UPDATE [dbo].[Tags] SET  WHERE id = @original_id";
            //SqlCommand cmd = new SqlCommand(updateString, connection);
            //cmd.Parameters.Add("@original_id", SqlDbType.Int, 4).Value = idUpdated;
            //try
            //{
            //    connection.Open();
            //    cmd.ExecuteNonQuery();
            //}
            //catch (SqlException err)
            //{
            //    throw new ApplicationException("TestODS Update Error." + err.ToString());
            //}
            //finally
            //{
            //    connection.Close();
            //}
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public void Delete(int id, int original_id)
        {
            var connection = new SqlConnection(connectionString);
            string deleteString = "DELETE FROM [dbo].[Tags] WHERE id = @original_id";
            var cmd = new SqlCommand(deleteString, connection);


            cmd.Parameters.Add("@original_id", SqlDbType.Int, 4).Value = id == 0 ? original_id : id;
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException err)
            {
                throw new ApplicationException("TestODS Delete Error." + err);
            }
            finally
            {
                connection.Close();
            }
        }

        #region Nested type: DataObjectTags

        public class DataObjectTags
        {
            public DataObjectTags()
            {
            }

            public DataObjectTags(string tagname, string tagdescription, int sessionid, Boolean sessionassigned, int id)
            {
                this.Tagname = tagname;
                this.Tagdescription = tagdescription;
                this.Sessionid = sessionid;
                this.Sessionassigned = sessionassigned;
                this.Id = id;
            }

            [DataObjectField(false, false, true)]
            public string Tagname { get; set; }

            [DataObjectField(false, false, true)]
            public string Tagdescription { get; set; }

            public int Sessionid { get; set; }

            public Boolean Sessionassigned { get; set; }

            [DataObjectField(true, true, false)]
            public int Id { get; set; }
        }

        #endregion
    }
}