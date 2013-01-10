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
    public class SessionTagsODS
    {
        private readonly string connectionString;

        public SessionTagsODS()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString;
        }

        public int GetCloudCountByTagsToShow(int numTagsToShow)
        {
            // Get highest tag count
            List<DataObjectSessionTags> allTags = GetAllTags(0,Utils.GetCurrentCodeCampYear());
            int maxVal = 0;
            foreach (DataObjectSessionTags dost in allTags)
            {
                if (dost.TagCount > maxVal)
                {
                    maxVal = dost.TagCount;
                }
            }

            int answer = 0;
            for (int cnt = maxVal;cnt > 0;cnt --)
            {
                int howMany = 0;
                foreach (DataObjectSessionTags dost in allTags)
                {
                    if (dost.TagCount >= cnt)
                    {
                        howMany++;
                    }
                }
                if (howMany >= numTagsToShow)
                {
                    answer = cnt;
                    break;
                }
            }
            return answer;
        }


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectSessionTags> GetAllTagsShorten()
        {
            return GetAllTagsShorten(0,Utils.GetCurrentCodeCampYear());
        }


        /// <summary>
        /// shorten tagName
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectSessionTags> GetAllTagsShorten(int minCount,int codeCampYearId)
        {
            var listNew = new List<DataObjectSessionTags>();
            List<DataObjectSessionTags> listOri = GetAllTags(minCount, codeCampYearId);
            const int maxLen = 25;
            foreach (DataObjectSessionTags dost in listOri)
            {
                if (!listNew.Contains(dost))
                {
                    dost.TagName = dost.TagName.Length > maxLen ? dost.TagName.Remove(maxLen) : dost.TagName;
                    listNew.Add(dost);
                }

                //if (dost.TagName.Length > maxLen)
                //{
                //    dost.TagName = dost.TagName.Substring(0, maxLen) + "...";
                //}
                //if (!listNew.Contains(dost))
                //{
                //    listNew.Add(dost);
                //}
            }
            return listNew;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<DataObjectSessionTags> GetAllTags()
        {
            return GetAllTags(0,Utils.GetCurrentCodeCampYear());
        }


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectSessionTags> GetAllTags(int minCount,int codeCampYearId)
        {
            string cacheName = Utils.CacheSessionTags + "-" + codeCampYearId.ToString() + "_" + minCount.ToString();
            List<DataObjectSessionTags> DataTemplateODSList;

            if (HttpContext.Current.Cache[cacheName] == null)
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // First, build a dictionary of tagid's by name
                    var tagIdDictionary = new Dictionary<string, string>();
                    // todo: might want to restrict this to just this years tags 
                    const string sqlSelectTags = "SELECT TagName,id FROM Tags";
                    var cmdTags = new SqlCommand(sqlSelectTags, conn);
                    using (SqlDataReader readerTags = cmdTags.ExecuteReader())
                    {
                        try
                        {
                            while (readerTags.Read())
                            {
                                string tagName = readerTags.IsDBNull(0) ? String.Empty : readerTags.GetString(0);
                                int tagId = readerTags.IsDBNull(1) ? 0 : readerTags.GetInt32(1);
                                if (!String.IsNullOrEmpty(tagName) && !tagIdDictionary.ContainsKey(tagName))
                                    tagIdDictionary.Add(tagName, tagId.ToString());
                            }
                        }
                        finally
                        {
                            if (readerTags != null)
                                readerTags.Close();
                        }
                    }
                    DataTemplateODSList = new List<DataObjectSessionTags>();
                    string sqlSelectString = String.Format(@"
                                    SELECT dbo.Tags.TagName,
                                           COUNT(dbo.Tags.TagName)
                                    FROM dbo.SessionTags
                                         INNER JOIN dbo.Tags ON (dbo.SessionTags.tagid = dbo.Tags.id)
                                         INNER JOIN dbo.Sessions ON (dbo.SessionTags.sessionId = dbo.Sessions.id AND dbo.[Sessions].CodeCampYearId = @CodeCampYearId)
                                    GROUP BY dbo.Tags.TagName
                                    Having COUNT(dbo.Tags.TagName) > {0}", minCount);
                    var cmd = new SqlCommand(sqlSelectString, conn);
                    cmd.Parameters.Add("@CodeCampYearId", SqlDbType.Int, 4).Value = Utils.GetCurrentCodeCampYear();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        try
                        {
                            if (reader != null)
                                while (reader.Read())
                                {
                                    string tagName = reader.IsDBNull(0) ? String.Empty : reader.GetString(0).Trim();
                                    if (!String.IsNullOrEmpty(tagName))
                                    {
                                        int tagCount = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                                        string tagIdString = string.Empty;
                                        if (tagIdDictionary.ContainsKey(tagName))
                                            tagIdString = tagIdDictionary[tagName];
                                        if (!String.IsNullOrEmpty(tagIdString))
                                        {
                                            var dost = new DataObjectSessionTags(Convert.ToInt32(tagIdString), tagName, tagCount);
                                            if (!DataTemplateODSList.Contains(dost))
                                                DataTemplateODSList.Add(dost);
                                        }
                                    }
                                }
                        }
                        finally
                        {
                            if (reader != null)
                                reader.Close();
                        }
                    }
                    conn.Close();
                    // Sort by lowercase name
                    Comparison<DataObjectSessionTags> comparison = null;
                    comparison = delegate(DataObjectSessionTags lhs, DataObjectSessionTags rhs)
                    {
                        return lhs.TagName.Trim().ToLower().CompareTo(rhs.TagName.Trim().ToLower());
                    };

                    DataTemplateODSList.Sort(comparison);
                    HttpContext.Current.Cache.Insert(cacheName, DataTemplateODSList, null, DateTime.Now.Add(new TimeSpan(0, 0, Utils.RetrieveSecondsForSessionCacheTimeout())), TimeSpan.Zero);
                }
            }
            else
            {
                DataTemplateODSList = (List<DataObjectSessionTags>) HttpContext.Current.Cache[cacheName];
            }

            return DataTemplateODSList;
        }

        #region Nested type: DataObjectSessionTags

        public class DataObjectSessionTags
        {
            public DataObjectSessionTags()
            {
            }

            public DataObjectSessionTags(int tagId, String tagName, int tagCount)
            {
                TagCount = tagCount;
                TagId = tagId;
                TagName = tagName;
            }

            [DataObjectField(false, false, false)]
            public string TagName { get; set; }

            [DataObjectField(false, false, false)]
            public int TagId { get; set; }

            [DataObjectField(false, false, false)]
            public int TagCount { get; set; }
        }

        #endregion
    }
}