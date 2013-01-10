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
using System.Configuration;
using System.Transactions;
using System.Web;

namespace CodeCampSV
{
    [DataObject(true)]  // This attribute allows the ObjectDataSource wizard to see this class
    public class ProfileDataODS
    {
        string connectionString;
        public ProfileDataODS()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString;
        }
        public class DataObjectProfileData
        {
            public DataObjectProfileData() { }
            public DataObjectProfileData(Guid pkid, string keyname, string data, int id)
            {
                this.pkid = pkid;
                this.keyname = keyname;
                this.data = data;
                this.id = id;
            }

            private Guid pkid;
            [DataObjectField(false, false, false)]
            public Guid Pkid
            {
                get { return pkid; }
                set { pkid = value; }
            }

            private string keyname;
            [DataObjectField(false, false, true)]
            public string Keyname
            {
                get { return keyname; }
                set { keyname = value; }
            }

            private string data;
            [DataObjectField(false, false, true)]
            public string Data
            {
                get { return data; }
                set { data = value; }
            }

            private int id;
            [DataObjectField(true, true, false)]
            public int Id
            {
                get { return id; }
                set { id = value; }
            }
        }


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectProfileData> GetByUsername(string username)
        {
            string cacheName = CodeCampSV.Utils.CacheProfileDataByUsername + "_" + username;
            List<DataObjectProfileData> DataTemplateODSList = null;

            if (HttpContext.Current.Cache[cacheName] == null)
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                DataTemplateODSList = new List<DataObjectProfileData>();
                SqlDataReader reader = null;
                string sqlSelectString = "SELECT PKID,keyname,data,id FROM [dbo].[ProfileData] WHERE PKID=(select PKID FROM Attendees WHERE Username=@Username)";
                SqlCommand cmd = new SqlCommand(sqlSelectString, conn);
                cmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = username; ;
                reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        Guid pkid = reader.IsDBNull(0) ? Guid.NewGuid() : reader.GetGuid(0);
                        string keyname = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        string data = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        int id = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                        DataObjectProfileData td = new DataObjectProfileData(pkid, keyname, data, id);
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
                DataTemplateODSList = (List<DataObjectProfileData>) HttpContext.Current.Cache[cacheName];
            }
            return DataTemplateODSList;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<DataObjectProfileData> GetAllProfileData(string sortData)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            List<DataObjectProfileData> DataTemplateODSList = new List<DataObjectProfileData>();
            SqlDataReader reader = null;
            string sqlSelectString = "SELECT PKID,keyname,data,id FROM [dbo].[ProfileData] ";
            SqlCommand cmd = new SqlCommand(sqlSelectString, conn);
            reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    Guid pkid = reader.IsDBNull(0) ? Guid.NewGuid() : reader.GetGuid(0);
                    string keyname = reader.IsDBNull(1) ? "" : reader.GetString(1);
                    string data = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    int id = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                    DataObjectProfileData td = new DataObjectProfileData(pkid, keyname, data, id);
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
            Comparison<DataObjectProfileData> comparison = null;
            switch (sortDataBase)
            {
                case "Keyname":
                    comparison = new Comparison<DataObjectProfileData>(
                       delegate(DataObjectProfileData lhs, DataObjectProfileData rhs)
                       {
                           return lhs.Keyname.CompareTo(rhs.Keyname);
                       }
                     );
                    break;
                case "Data":
                    comparison = new Comparison<DataObjectProfileData>(
                       delegate(DataObjectProfileData lhs, DataObjectProfileData rhs)
                       {
                           return lhs.Data.CompareTo(rhs.Data);
                       }
                     );
                    break;
                case "Id":
                    comparison = new Comparison<DataObjectProfileData>(
                       delegate(DataObjectProfileData lhs, DataObjectProfileData rhs)
                       {
                           return lhs.Id.CompareTo(rhs.Id);
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

       

       
       

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public void Delete(int id)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string deleteString = "DELETE FROM [dbo].[ProfileData] WHERE id = @id";
            SqlCommand cmd = new SqlCommand(deleteString, connection);

            cmd.Parameters.Add("@original_id", SqlDbType.Int, 4).Value = id;
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException err)
            {
                throw new ApplicationException("TestODS Delete Error." + err.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        public void UpdateProfileData(string username,string keyname,string data)
        {
            // clear the cache
            // ProfileDataByUsername_pkellner
            HttpContext.Current.Cache.Remove(CodeCampSV.Utils.CacheProfileDataByUsername + "_" + username);


            //// add or update.  First, try to update and if that fails, then add.
            //

            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            // $$$ Broken scope, will not work.  See SessionsEdit.aspx for correct way
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    string sqlUpdate = "UPDATE ProfileData SET data=@data WHERE PKID = (select PKID FROM Attendees WHERE Username=@username) AND keyname=@keyname";

                    SqlCommand sqlCommand = new SqlCommand(sqlUpdate, sqlConnection);
                    sqlCommand.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
                    sqlCommand.Parameters.Add("@keyname", SqlDbType.VarChar).Value = keyname;
                    sqlCommand.Parameters.Add("@data", SqlDbType.VarChar).Value = data;

                    int rowsUpdated = sqlCommand.ExecuteNonQuery();

                    // must not have existed, so add it.
                    if (rowsUpdated == 0)
                    {
                        string userPKID = CodeCampSV.Utils.GetAttendeePKIDByUsername(username);
                        string sqlInsert = "INSERT INTO ProfileData (PKID,keyname,data) VALUES " +
                            "(@PKID,@keyname,@data)";
                        SqlCommand sqlCommand1 = new SqlCommand(sqlInsert, sqlConnection);
                        sqlCommand1.Parameters.Add("@PKID", SqlDbType.UniqueIdentifier).Value = new Guid(userPKID);
                        sqlCommand1.Parameters.Add("@keyname", SqlDbType.VarChar).Value = keyname;
                        sqlCommand1.Parameters.Add("@data", SqlDbType.VarChar).Value = data;
                        rowsUpdated = sqlCommand1.ExecuteNonQuery();
                    }
                    scope.Complete();
                }
                catch (Exception ee)
                {
                    throw new ApplicationException(ee.ToString());
                }
            }

            sqlConnection.Close();
            sqlConnection.Dispose();

            
        }
    }
}





