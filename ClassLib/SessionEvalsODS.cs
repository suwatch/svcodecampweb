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
    public class SessionEvalsODS
    {
        string connectionString;
        public SessionEvalsODS()
        {
            connectionString = WebConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString;
        }

        public class DataObjectSessionEvalQuestions
        {
            public DataObjectSessionEvalQuestions() { }

            private string question;

            private int numberResponses;

            double averageResponse;

            double overallAverageResponse;

            public DataObjectSessionEvalQuestions(string question, int numberResponses, double averageResponse, double overallAverageResponse)
            {
                this.question = question;
                this.numberResponses = numberResponses;
                this.averageResponse = averageResponse;
                this.overallAverageResponse = overallAverageResponse;
            }

            public string Question
            {
                get { return question; }
                set { question = value; }
            }

            public int NumberResponses
            {
                get { return numberResponses; }
                set { numberResponses = value; }
            }

            public double AverageResponse
            {
                get { return averageResponse; }
                set { averageResponse = value; }
            }

            public double OverallAverageResponse
            {
                get { return overallAverageResponse; }
                set { overallAverageResponse = value; }
            }

        }


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectSessionEvalQuestions> GetSessionResponses(int sessionId)
        {
            

            List<DataObjectSessionEvalQuestions> sessionEvalQuestionsList = new List<DataObjectSessionEvalQuestions>();

            // first, load everything up
            List<DataObjectSessionEvals> sessionEvalsList = GetAllSessionEvals(string.Empty);
            int numberResponsesTotal = CountResponsesForSession(sessionId, sessionEvalsList);
            foreach (string question in questions)
            {
                DataObjectSessionEvalQuestions q = new DataObjectSessionEvalQuestions();
                q.Question = question;
                int numberResponses = -1;
                double avgResp = 0.0;
                double overallAvgResp = 0.0;
                ProcessResponsesForSession(sessionId, question, sessionEvalsList, 
                    ref  numberResponses, ref  avgResp, ref  overallAvgResp);
                q.OverallAverageResponse = overallAvgResp;
                q.NumberResponses = numberResponses;
                q.AverageResponse = avgResp;
                sessionEvalQuestionsList.Add(q);
            }

            return sessionEvalQuestionsList;
        }

        string[] questions = new string[] {
                "Course as a Whole",
                "Course Content",
                "Instructors Ability to Explain Material",
                "Effectiveness of instructor",
                "Instructors Knowledge of Material",
                "Quality of Facility",
                "Was Content Level as Advertised",
                "Overall Feeling How Code Camp is going"
            };

        private void ProcessResponsesForSession(int sessionId, string question, List<DataObjectSessionEvals> sessionEvalsList,
            ref int numberResponses, ref double averageResponse, ref double overallAverageResponse)
        {
            double totalResponse = 0.0;
            int cntTotalResponse = 0;
            double totalOverAllResponse = 0.0;
            int cntTotalOverAllResponse = 0;



            foreach (DataObjectSessionEvals sessionEvals in sessionEvalsList)
            {
                //double responseSession = 0.0;
                //double responseSessionOverall = 0.0;

                int currentAnswer = 0;

                if (question.Equals(questions[0]))
                {
                    currentAnswer = sessionEvals.Courseaswhole;
                }
                else if (question.Equals(questions[1]))
                {
                    currentAnswer = sessionEvals.Coursecontent;
                }
                else if (question.Equals(questions[2]))
                {
                    currentAnswer = sessionEvals.Instructorabilityexplain;
                }
                else if (question.Equals(questions[3]))
                {
                    currentAnswer = sessionEvals.Instructoreffective;
                }
                else if (question.Equals(questions[4]))
                {
                    currentAnswer = sessionEvals.Instructorknowledge;
                }
                else if (question.Equals(questions[5]))
                {
                    currentAnswer = sessionEvals.Qualityoffacility;
                }
                else if (question.Equals(questions[6]))
                {
                    currentAnswer = sessionEvals.Contentlevel;
                }
                else if (question.Equals(questions[7]))
                {
                    currentAnswer = sessionEvals.Overallcodecamp;
                }


                if (currentAnswer >= 1 && currentAnswer <= 4)
                {
                    totalOverAllResponse += currentAnswer;
                    cntTotalOverAllResponse++;

                    if (sessionEvals.Sessionid == sessionId)
                    {
                        totalResponse += currentAnswer;
                        cntTotalResponse++;
                    }
                }
            }

            numberResponses = cntTotalResponse;

            if (cntTotalResponse > 0)
            {
                averageResponse = totalResponse / Convert.ToDouble(cntTotalResponse);
            }

            if (cntTotalOverAllResponse > 0)
            {
                overallAverageResponse = totalOverAllResponse / Convert.ToDouble(cntTotalOverAllResponse);
            }
        }

       

        private int CountResponsesForSession(int sessionId, List<DataObjectSessionEvals> sessionEvalsList)
        {
            int cnt = 0;
            foreach (DataObjectSessionEvals sessionEvals in sessionEvalsList)
            {
                if (sessionEvals.Sessionid == sessionId)
                {
                    cnt++;
                }
            }
            return cnt;
        }

       


        public class DataObjectSessionEvals
        {
            public DataObjectSessionEvals() { }
            public DataObjectSessionEvals(Guid pkid, DateTime createdate, DateTime updatedate, int courseaswhole, int coursecontent, int instructoreff, int instructorabilityexplain, int instructoreffective, int instructorknowledge, int qualityoffacility, int overallcodecamp, int contentlevel, string favorite, string improved, string generalcomments, bool discloseeval, int sessionid, int id)
            {
                this.pkid = pkid;
                this.createdate = createdate;
                this.updatedate = updatedate;
                this.courseaswhole = courseaswhole;
                this.coursecontent = coursecontent;
                this.instructoreff = instructoreff;
                this.instructorabilityexplain = instructorabilityexplain;
                this.instructoreffective = instructoreffective;
                this.instructorknowledge = instructorknowledge;
                this.qualityoffacility = qualityoffacility;
                this.overallcodecamp = overallcodecamp;
                this.contentlevel = contentlevel;
                this.favorite = favorite;
                this.improved = improved;
                this.generalcomments = generalcomments;
                this.discloseeval = discloseeval;
                this.sessionid = sessionid;
                this.id = id;
            }

            private Guid pkid;
            [DataObjectField(false, false, false)]
            public Guid Pkid
            {
                get { return pkid; }
                set { pkid = value; }
            }

            private DateTime createdate;
            [DataObjectField(false, false, true)]
            public DateTime Createdate
            {
                get { return createdate; }
                set { createdate = value; }
            }

            private DateTime updatedate;
            [DataObjectField(false, false, true)]
            public DateTime Updatedate
            {
                get { return updatedate; }
                set { updatedate = value; }
            }

            private int courseaswhole;
            [DataObjectField(false, false, true)]
            public int Courseaswhole
            {
                get { return courseaswhole; }
                set { courseaswhole = value; }
            }

            private int coursecontent;
            [DataObjectField(false, false, true)]
            public int Coursecontent
            {
                get { return coursecontent; }
                set { coursecontent = value; }
            }

            private int instructoreff;
            [DataObjectField(false, false, true)]
            public int Instructoreff
            {
                get { return instructoreff; }
                set { instructoreff = value; }
            }

            private int instructorabilityexplain;
            [DataObjectField(false, false, true)]
            public int Instructorabilityexplain
            {
                get { return instructorabilityexplain; }
                set { instructorabilityexplain = value; }
            }

            private int instructoreffective;
            [DataObjectField(false, false, true)]
            public int Instructoreffective
            {
                get { return instructoreffective; }
                set { instructoreffective = value; }
            }

            private int instructorknowledge;
            [DataObjectField(false, false, true)]
            public int Instructorknowledge
            {
                get { return instructorknowledge; }
                set { instructorknowledge = value; }
            }

            private int qualityoffacility;
            [DataObjectField(false, false, true)]
            public int Qualityoffacility
            {
                get { return qualityoffacility; }
                set { qualityoffacility = value; }
            }

            private int overallcodecamp;
            [DataObjectField(false, false, true)]
            public int Overallcodecamp
            {
                get { return overallcodecamp; }
                set { overallcodecamp = value; }
            }

            private int contentlevel;
            [DataObjectField(false, false, true)]
            public int Contentlevel
            {
                get { return contentlevel; }
                set { contentlevel = value; }
            }

            private string favorite;
            [DataObjectField(false, false, true)]
            public string Favorite
            {
                get { return favorite; }
                set { favorite = value; }
            }

            private string improved;
            [DataObjectField(false, false, true)]
            public string Improved
            {
                get { return improved; }
                set { improved = value; }
            }

            private string generalcomments;
            [DataObjectField(false, false, true)]
            public string Generalcomments
            {
                get { return generalcomments; }
                set { generalcomments = value; }
            }

            private bool discloseeval;
            [DataObjectField(false, false, true)]
            public bool Discloseeval
            {
                get { return discloseeval; }
                set { discloseeval = value; }
            }

            private int sessionid;
            [DataObjectField(false, false, false)]
            public int Sessionid
            {
                get { return sessionid; }
                set { sessionid = value; }
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
        public List<DataObjectSessionEvals> GetByUsername(string sortData, string username)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            List<DataObjectSessionEvals> DataTemplateODSList = new List<DataObjectSessionEvals>();
            SqlDataReader reader = null;

            string sqlSelectString =
               @"SELECT PKID,
                       CreateDate,
                       UpdateDate,
                       CourseAsWhole,
                       CourseContent,
                       InstructorEff,
                       InstructorAbilityExplain,
                       InstructorEffective,
                       InstructorKnowledge,
                       QualityOfFacility,
                       OverallCodeCamp,
                       ContentLevel,
                       Favorite,
                       Improved,
                       GeneralComments,
                       DiscloseEval,
                       sessionId,
                       id
                FROM [dbo].[SessionEvals]
                WHERE PKID =
                      (SELECT PKID FROM Attendees WHERE Username = @username)
                ORDER BY CreateDate";

            SqlCommand cmd = new SqlCommand(sqlSelectString, conn);
            cmd.Parameters.Add("@username", SqlDbType.VarChar, 4096).Value = username; ;
            reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    Guid pkid = reader.IsDBNull(0) ? Guid.NewGuid() : reader.GetGuid(0);
                    DateTime createdate = reader.IsDBNull(1) ? DateTime.Now : reader.GetDateTime(1);
                    DateTime updatedate = reader.IsDBNull(2) ? DateTime.Now : reader.GetDateTime(2);
                    int courseaswhole = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                    int coursecontent = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                    int instructoreff = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
                    int instructorabilityexplain = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
                    int instructoreffective = reader.IsDBNull(7) ? 0 : reader.GetInt32(7);
                    int instructorknowledge = reader.IsDBNull(8) ? 0 : reader.GetInt32(8);
                    int qualityoffacility = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                    int overallcodecamp = reader.IsDBNull(10) ? 0 : reader.GetInt32(10);
                    int contentlevel = reader.IsDBNull(11) ? 0 : reader.GetInt32(11);
                    string favorite = reader.IsDBNull(12) ? "" : reader.GetString(12);
                    string improved = reader.IsDBNull(13) ? "" : reader.GetString(13);
                    string generalcomments = reader.IsDBNull(14) ? "" : reader.GetString(14);
                    bool discloseeval = reader.IsDBNull(15) ? false : reader.GetBoolean(15);
                    int sessionid = reader.IsDBNull(16) ? 0 : reader.GetInt32(16);
                    int id = reader.IsDBNull(17) ? 0 : reader.GetInt32(17);
                    DataObjectSessionEvals td = new DataObjectSessionEvals(pkid, createdate, updatedate, courseaswhole, coursecontent, instructoreff, instructorabilityexplain, instructoreffective, instructorknowledge, qualityoffacility, overallcodecamp, contentlevel, favorite, improved, generalcomments, discloseeval, sessionid, id);
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
            Comparison<DataObjectSessionEvals> comparison = null;
            switch (sortDataBase)
            {
                case "Createdate":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Createdate.CompareTo(rhs.Createdate);
                       }
                     );
                    break;
                case "Updatedate":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Updatedate.CompareTo(rhs.Updatedate);
                       }
                     );
                    break;
                case "Courseaswhole":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Courseaswhole.CompareTo(rhs.Courseaswhole);
                       }
                     );
                    break;
                case "Coursecontent":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Coursecontent.CompareTo(rhs.Coursecontent);
                       }
                     );
                    break;
                case "Instructoreff":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Instructoreff.CompareTo(rhs.Instructoreff);
                       }
                     );
                    break;
                case "Instructorabilityexplain":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Instructorabilityexplain.CompareTo(rhs.Instructorabilityexplain);
                       }
                     );
                    break;
                case "Instructoreffective":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Instructoreffective.CompareTo(rhs.Instructoreffective);
                       }
                     );
                    break;
                case "Instructorknowledge":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Instructorknowledge.CompareTo(rhs.Instructorknowledge);
                       }
                     );
                    break;
                case "Qualityoffacility":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Qualityoffacility.CompareTo(rhs.Qualityoffacility);
                       }
                     );
                    break;
                case "Overallcodecamp":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Overallcodecamp.CompareTo(rhs.Overallcodecamp);
                       }
                     );
                    break;
                case "Contentlevel":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Contentlevel.CompareTo(rhs.Contentlevel);
                       }
                     );
                    break;
                case "Favorite":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Favorite.CompareTo(rhs.Favorite);
                       }
                     );
                    break;
                case "Improved":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Improved.CompareTo(rhs.Improved);
                       }
                     );
                    break;
                case "Generalcomments":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Generalcomments.CompareTo(rhs.Generalcomments);
                       }
                     );
                    break;
                case "Discloseeval":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Discloseeval.CompareTo(rhs.Discloseeval);
                       }
                     );
                    break;
                case "Sessionid":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Sessionid.CompareTo(rhs.Sessionid);
                       }
                     );
                    break;
                case "Id":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
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

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectSessionEvals> GetBySessionId(string sortData, int sessionId)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            List<DataObjectSessionEvals> DataTemplateODSList = new List<DataObjectSessionEvals>();
            SqlDataReader reader = null;

            string sqlSelectString =
            @"SELECT PKID,
                   CreateDate,
                   UpdateDate,
                   CourseAsWhole,
                   CourseContent,
                   InstructorEff,
                   InstructorAbilityExplain,
                   InstructorEffective,
                   InstructorKnowledge,
                   QualityOfFacility,
                   OverallCodeCamp,
                   ContentLevel,
                   Favorite,
                   Improved,
                   GeneralComments,
                   DiscloseEval,
                   sessionId,
                   id
            FROM [dbo].[SessionEvals]
            WHERE sessionId = @sessionId";
            
            SqlCommand cmd = new SqlCommand(sqlSelectString, conn);
            cmd.Parameters.Add("@sessionId", SqlDbType.Int, 4).Value = sessionId; ;
            reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    Guid pkid = reader.IsDBNull(0) ? Guid.NewGuid() : reader.GetGuid(0);
                    DateTime createdate = reader.IsDBNull(1) ? DateTime.Now : reader.GetDateTime(1);
                    DateTime updatedate = reader.IsDBNull(2) ? DateTime.Now : reader.GetDateTime(2);
                    int courseaswhole = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                    int coursecontent = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                    int instructoreff = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
                    int instructorabilityexplain = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
                    int instructoreffective = reader.IsDBNull(7) ? 0 : reader.GetInt32(7);
                    int instructorknowledge = reader.IsDBNull(8) ? 0 : reader.GetInt32(8);
                    int qualityoffacility = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                    int overallcodecamp = reader.IsDBNull(10) ? 0 : reader.GetInt32(10);
                    int contentlevel = reader.IsDBNull(11) ? 0 : reader.GetInt32(11);
                    string favorite = reader.IsDBNull(12) ? "" : reader.GetString(12);
                    string improved = reader.IsDBNull(13) ? "" : reader.GetString(13);
                    string generalcomments = reader.IsDBNull(14) ? "" : reader.GetString(14);
                    bool discloseeval = reader.IsDBNull(15) ? false : reader.GetBoolean(15);
                    int sessionid = reader.IsDBNull(16) ? 0 : reader.GetInt32(16);
                    int id = reader.IsDBNull(17) ? 0 : reader.GetInt32(17);
                    DataObjectSessionEvals td = new DataObjectSessionEvals(pkid, createdate, updatedate, courseaswhole, coursecontent, instructoreff, instructorabilityexplain, instructoreffective, instructorknowledge, qualityoffacility, overallcodecamp, contentlevel, favorite, improved, generalcomments, discloseeval, sessionid, id);
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
            Comparison<DataObjectSessionEvals> comparison = null;
            switch (sortDataBase)
            {
                case "Createdate":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Createdate.CompareTo(rhs.Createdate);
                       }
                     );
                    break;
                case "Updatedate":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Updatedate.CompareTo(rhs.Updatedate);
                       }
                     );
                    break;
                case "Courseaswhole":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Courseaswhole.CompareTo(rhs.Courseaswhole);
                       }
                     );
                    break;
                case "Coursecontent":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Coursecontent.CompareTo(rhs.Coursecontent);
                       }
                     );
                    break;
                case "Instructoreff":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Instructoreff.CompareTo(rhs.Instructoreff);
                       }
                     );
                    break;
                case "Instructorabilityexplain":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Instructorabilityexplain.CompareTo(rhs.Instructorabilityexplain);
                       }
                     );
                    break;
                case "Instructoreffective":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Instructoreffective.CompareTo(rhs.Instructoreffective);
                       }
                     );
                    break;
                case "Instructorknowledge":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Instructorknowledge.CompareTo(rhs.Instructorknowledge);
                       }
                     );
                    break;
                case "Qualityoffacility":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Qualityoffacility.CompareTo(rhs.Qualityoffacility);
                       }
                     );
                    break;
                case "Overallcodecamp":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Overallcodecamp.CompareTo(rhs.Overallcodecamp);
                       }
                     );
                    break;
                case "Contentlevel":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Contentlevel.CompareTo(rhs.Contentlevel);
                       }
                     );
                    break;
                case "Favorite":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Favorite.CompareTo(rhs.Favorite);
                       }
                     );
                    break;
                case "Improved":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Improved.CompareTo(rhs.Improved);
                       }
                     );
                    break;
                case "Generalcomments":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Generalcomments.CompareTo(rhs.Generalcomments);
                       }
                     );
                    break;
                case "Discloseeval":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Discloseeval.CompareTo(rhs.Discloseeval);
                       }
                     );
                    break;
                case "Sessionid":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Sessionid.CompareTo(rhs.Sessionid);
                       }
                     );
                    break;
                case "Id":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
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

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public int GetBySessionIdCount(int sessionId)
        {
            string cacheName = CodeCampSV.Utils.CacheEvaluationCount + "_" + sessionId.ToString(); ;
            int cnt = 0;
            if (HttpContext.Current.Cache[cacheName] == null)
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                string sqlSelectString =
                @"SELECT count(id)
                    FROM [dbo].[SessionEvals]
                    WHERE sessionId = @sessionId";

                SqlCommand cmd = new SqlCommand(sqlSelectString, conn);
                cmd.Parameters.Add("@sessionId", SqlDbType.Int, 4).Value = sessionId;
                cnt = (int)cmd.ExecuteScalar();
                conn.Close();
                HttpContext.Current.Cache.Insert(cacheName, cnt,
                           null, DateTime.Now.Add(new TimeSpan(0, 0, Utils.RetrieveSecondsForSessionCacheTimeout())), TimeSpan.Zero);
            }
            else
            {
                cnt = (int) HttpContext.Current.Cache[cacheName];
            }

            return cnt;
        }



        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DataObjectSessionEvals> GetByUsernameSessionId(string username, int sessionId)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            List<DataObjectSessionEvals> DataTemplateODSList = new List<DataObjectSessionEvals>();
            SqlDataReader reader = null;

            string sqlSelectString =
                @"SELECT PKID,
                       CreateDate,
                       UpdateDate,
                       CourseAsWhole,
                       CourseContent,
                       InstructorEff,
                       InstructorAbilityExplain,
                       InstructorEffective,
                       InstructorKnowledge,
                       QualityOfFacility,
                       OverallCodeCamp,
                       ContentLevel,
                       Favorite,
                       Improved,
                       GeneralComments,
                       DiscloseEval,
                       sessionId,
                       id
                FROM [dbo].[SessionEvals]
                WHERE PKID =
                      (SELECT PKID FROM Attendees WHERE Username = @username) AND
                      (sessionId = @sessionId)";

            SqlCommand cmd = new SqlCommand(sqlSelectString, conn);
            cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = username; ;
            cmd.Parameters.Add("@sessionId", SqlDbType.Int).Value = sessionId;
            reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    Guid pkid = reader.IsDBNull(0) ? Guid.NewGuid() : reader.GetGuid(0);
                    DateTime createdate = reader.IsDBNull(1) ? DateTime.Now : reader.GetDateTime(1);
                    DateTime updatedate = reader.IsDBNull(2) ? DateTime.Now : reader.GetDateTime(2);
                    int courseaswhole = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                    int coursecontent = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                    int instructoreff = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
                    int instructorabilityexplain = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
                    int instructoreffective = reader.IsDBNull(7) ? 0 : reader.GetInt32(7);
                    int instructorknowledge = reader.IsDBNull(8) ? 0 : reader.GetInt32(8);
                    int qualityoffacility = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                    int overallcodecamp = reader.IsDBNull(10) ? 0 : reader.GetInt32(10);
                    int contentlevel = reader.IsDBNull(11) ? 0 : reader.GetInt32(11);
                    string favorite = reader.IsDBNull(12) ? "" : reader.GetString(12);
                    string improved = reader.IsDBNull(13) ? "" : reader.GetString(13);
                    string generalcomments = reader.IsDBNull(14) ? "" : reader.GetString(14);
                    bool discloseeval = reader.IsDBNull(15) ? false : reader.GetBoolean(15);
                    int sessionid = reader.IsDBNull(16) ? 0 : reader.GetInt32(16);
                    int id = reader.IsDBNull(17) ? 0 : reader.GetInt32(17);
                    DataObjectSessionEvals td = new DataObjectSessionEvals(pkid, createdate, updatedate, courseaswhole, coursecontent, instructoreff, instructorabilityexplain, instructoreffective, instructorknowledge, qualityoffacility, overallcodecamp, contentlevel, favorite, improved, generalcomments, discloseeval, sessionid, id);
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

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<DataObjectSessionEvals> GetAllSessionEvals(string sortData)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            List<DataObjectSessionEvals> DataTemplateODSList = new List<DataObjectSessionEvals>();
            SqlDataReader reader = null;
            string sqlSelectString = 
                @"SELECT PKID,
                       CreateDate,
                       UpdateDate,
                       CourseAsWhole,
                       CourseContent,
                       InstructorEff,
                       InstructorAbilityExplain,
                       InstructorEffective,
                       InstructorKnowledge,
                       QualityOfFacility,
                       OverallCodeCamp,
                       ContentLevel,
                       Favorite,
                       Improved,
                       GeneralComments,
                       DiscloseEval,
                       sessionId,
                       id
                FROM [dbo].[SessionEvals]
                ORDER BY sessionId";
            SqlCommand cmd = new SqlCommand(sqlSelectString, conn);
            reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    Guid pkid = reader.IsDBNull(0) ? Guid.NewGuid() : reader.GetGuid(0);
                    DateTime createdate = reader.IsDBNull(1) ? DateTime.Now : reader.GetDateTime(1);
                    DateTime updatedate = reader.IsDBNull(2) ? DateTime.Now : reader.GetDateTime(2);
                    int courseaswhole = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                    int coursecontent = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                    int instructoreff = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
                    int instructorabilityexplain = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
                    int instructoreffective = reader.IsDBNull(7) ? 0 : reader.GetInt32(7);
                    int instructorknowledge = reader.IsDBNull(8) ? 0 : reader.GetInt32(8);
                    int qualityoffacility = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                    int overallcodecamp = reader.IsDBNull(10) ? 0 : reader.GetInt32(10);
                    int contentlevel = reader.IsDBNull(11) ? 0 : reader.GetInt32(11);
                    string favorite = reader.IsDBNull(12) ? "" : reader.GetString(12);
                    string improved = reader.IsDBNull(13) ? "" : reader.GetString(13);
                    string generalcomments = reader.IsDBNull(14) ? "" : reader.GetString(14);
                    bool discloseeval = reader.IsDBNull(15) ? false : reader.GetBoolean(15);
                    int sessionid = reader.IsDBNull(16) ? 0 : reader.GetInt32(16);
                    int id = reader.IsDBNull(17) ? 0 : reader.GetInt32(17);
                    DataObjectSessionEvals td = new DataObjectSessionEvals(pkid, createdate, updatedate, courseaswhole, coursecontent, instructoreff, instructorabilityexplain, instructoreffective, instructorknowledge, qualityoffacility, overallcodecamp, contentlevel, favorite, improved, generalcomments, discloseeval, sessionid, id);
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
            Comparison<DataObjectSessionEvals> comparison = null;
            switch (sortDataBase)
            {
                case "Createdate":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Createdate.CompareTo(rhs.Createdate);
                       }
                     );
                    break;
                case "Updatedate":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Updatedate.CompareTo(rhs.Updatedate);
                       }
                     );
                    break;
                case "Courseaswhole":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Courseaswhole.CompareTo(rhs.Courseaswhole);
                       }
                     );
                    break;
                case "Coursecontent":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Coursecontent.CompareTo(rhs.Coursecontent);
                       }
                     );
                    break;
                case "Instructoreff":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Instructoreff.CompareTo(rhs.Instructoreff);
                       }
                     );
                    break;
                case "Instructorabilityexplain":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Instructorabilityexplain.CompareTo(rhs.Instructorabilityexplain);
                       }
                     );
                    break;
                case "Instructoreffective":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Instructoreffective.CompareTo(rhs.Instructoreffective);
                       }
                     );
                    break;
                case "Instructorknowledge":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Instructorknowledge.CompareTo(rhs.Instructorknowledge);
                       }
                     );
                    break;
                case "Qualityoffacility":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Qualityoffacility.CompareTo(rhs.Qualityoffacility);
                       }
                     );
                    break;
                case "Overallcodecamp":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Overallcodecamp.CompareTo(rhs.Overallcodecamp);
                       }
                     );
                    break;
                case "Contentlevel":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Contentlevel.CompareTo(rhs.Contentlevel);
                       }
                     );
                    break;
                case "Favorite":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Favorite.CompareTo(rhs.Favorite);
                       }
                     );
                    break;
                case "Improved":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Improved.CompareTo(rhs.Improved);
                       }
                     );
                    break;
                case "Generalcomments":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Generalcomments.CompareTo(rhs.Generalcomments);
                       }
                     );
                    break;
                case "Discloseeval":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Discloseeval.CompareTo(rhs.Discloseeval);
                       }
                     );
                    break;
                case "Sessionid":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
                       {
                           return lhs.Sessionid.CompareTo(rhs.Sessionid);
                       }
                     );
                    break;
                case "Id":
                    comparison = new Comparison<DataObjectSessionEvals>(
                       delegate(DataObjectSessionEvals lhs, DataObjectSessionEvals rhs)
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

       
        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public void InsertAllSessionEvals(string username, DateTime createdate, DateTime updatedate, int courseaswhole, int coursecontent, int instructoreff, int instructorabilityexplain, int instructoreffective, int instructorknowledge, int qualityoffacility, int overallcodecamp, int contentlevel, string favorite, string improved, string generalcomments, bool discloseeval, int sessionid)
        {
            Guid pkid = new Guid(CodeCampSV.Utils.GetAttendeePKIDByUsername(username));

            SqlConnection connection = new SqlConnection(connectionString);
            String insertString = "INSERT INTO [dbo].[SessionEvals] (";
            insertString += "PKID";
            insertString += ",CreateDate";
            insertString += ",UpdateDate";
            insertString += ",CourseAsWhole";
            insertString += ",CourseContent";
            insertString += ",InstructorEff";
            insertString += ",InstructorAbilityExplain";
            insertString += ",InstructorEffective";
            insertString += ",InstructorKnowledge";
            insertString += ",QualityOfFacility";
            insertString += ",OverallCodeCamp";
            insertString += ",ContentLevel";
            insertString += ",Favorite";
            insertString += ",Improved";
            insertString += ",GeneralComments";
            insertString += ",DiscloseEval";
            insertString += ",sessionId";
            insertString += ") VALUES (";
            insertString += "@pkid";
            insertString += ",@createdate";
            insertString += ",@updatedate";
            insertString += ",@courseaswhole";
            insertString += ",@coursecontent";
            insertString += ",@instructoreff";
            insertString += ",@instructorabilityexplain";
            insertString += ",@instructoreffective";
            insertString += ",@instructorknowledge";
            insertString += ",@qualityoffacility";
            insertString += ",@overallcodecamp";
            insertString += ",@contentlevel";
            insertString += ",@favorite";
            insertString += ",@improved";
            insertString += ",@generalcomments";
            insertString += ",@discloseeval";
            insertString += ",@sessionid";
            insertString += "); ";
            SqlCommand cmd = new SqlCommand(insertString, connection);
            cmd.Parameters.Add("@pkid", SqlDbType.UniqueIdentifier, 16).Value = pkid;
            cmd.Parameters.Add("@createdate", SqlDbType.DateTime, 8).Value = createdate.CompareTo(new DateTime(1800, 1, 1)) < 0 ? new DateTime(1800, 1, 1) : createdate;
            cmd.Parameters.Add("@updatedate", SqlDbType.DateTime, 8).Value = updatedate.CompareTo(new DateTime(1800, 1, 1)) < 0 ? new DateTime(1800, 1, 1) : updatedate;
            cmd.Parameters.Add("@courseaswhole", SqlDbType.Int, 4).Value = courseaswhole;
            cmd.Parameters.Add("@coursecontent", SqlDbType.Int, 4).Value = coursecontent;
            cmd.Parameters.Add("@instructoreff", SqlDbType.Int, 4).Value = instructoreff;
            cmd.Parameters.Add("@instructorabilityexplain", SqlDbType.Int, 4).Value = instructorabilityexplain;
            cmd.Parameters.Add("@instructoreffective", SqlDbType.Int, 4).Value = instructoreffective;
            cmd.Parameters.Add("@instructorknowledge", SqlDbType.Int, 4).Value = instructorknowledge;
            cmd.Parameters.Add("@qualityoffacility", SqlDbType.Int, 4).Value = qualityoffacility;
            cmd.Parameters.Add("@overallcodecamp", SqlDbType.Int, 4).Value = overallcodecamp;
            cmd.Parameters.Add("@contentlevel", SqlDbType.Int, 4).Value = contentlevel;
            cmd.Parameters.Add("@favorite", SqlDbType.VarChar, 4096).Value = favorite == null ? String.Empty : favorite;
            cmd.Parameters.Add("@improved", SqlDbType.VarChar, 4096).Value = improved == null ? String.Empty : improved;
            cmd.Parameters.Add("@generalcomments", SqlDbType.VarChar, 4096).Value = generalcomments == null ? String.Empty : generalcomments;
            cmd.Parameters.Add("@discloseeval", SqlDbType.Bit, 1).Value = discloseeval;
            cmd.Parameters.Add("@sessionid", SqlDbType.Int, 4).Value = sessionid;
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException err)
            {
                throw new ApplicationException("TestODS Insert Error." + err.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public void UpdateAllSessionEvals(string username,int sessionId, DateTime updatedate, int courseaswhole, int coursecontent, int instructoreff, int instructorabilityexplain, int instructoreffective, int instructorknowledge, int qualityoffacility, int overallcodecamp, int contentlevel, string favorite, string improved, string generalcomments, bool discloseeval)
        {
            Guid pkid = new Guid(CodeCampSV.Utils.GetAttendeePKIDByUsername(username));

            SqlConnection connection = new SqlConnection(connectionString);
            string updateString =
                @"UPDATE [dbo].[SessionEvals]
                    SET UpdateDate = @updatedate,
                        CourseAsWhole = @courseaswhole,
                        CourseContent = @coursecontent,
                        InstructorEff = @instructoreff,
                        InstructorAbilityExplain = @instructorabilityexplain,
                        InstructorEffective = @instructoreffective,
                        InstructorKnowledge = @instructorknowledge,
                        QualityOfFacility = @qualityoffacility,
                        OverallCodeCamp = @overallcodecamp,
                        ContentLevel = @contentlevel,
                        Favorite = @favorite,
                        Improved = @improved,
                        GeneralComments = @generalcomments,
                        DiscloseEval = @discloseeval
                    WHERE PKID =
                          (SELECT PKID FROM Attendees WHERE Username = @username) AND
                          (sessionId = @sessionId)";

                // "UPDATE [dbo].[SessionEvals] SET PKID = @pkid,CreateDate = @createdate,UpdateDate = @updatedate,CourseAsWhole = @courseaswhole,CourseContent = @coursecontent,InstructorEff = @instructoreff,InstructorAbilityExplain = @instructorabilityexplain,InstructorEffective = @instructoreffective,InstructorKnowledge = @instructorknowledge,QualityOfFacility = @qualityoffacility,OverallCodeCamp = @overallcodecamp,ContentLevel = @contentlevel,Favorite = @favorite,Improved = @improved,GeneralComments = @generalcomments,DiscloseEval = @discloseeval,sessionId = @sessionid WHERE id = @original_id";
            SqlCommand cmd = new SqlCommand(updateString, connection);
            //cmd.Parameters.Add("@pkid", SqlDbType.UniqueIdentifier, 16).Value = pkid;
            //cmd.Parameters.Add("@createdate", SqlDbType.DateTime, 8).Value = createdate.CompareTo(new DateTime(1800, 1, 1)) < 0 ? new DateTime(1800, 1, 1) : createdate;
            cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
            cmd.Parameters.Add("@updatedate", SqlDbType.DateTime, 8).Value = updatedate.CompareTo(new DateTime(1800, 1, 1)) < 0 ? new DateTime(1800, 1, 1) : updatedate;
            cmd.Parameters.Add("@courseaswhole", SqlDbType.Int, 4).Value = courseaswhole;
            cmd.Parameters.Add("@coursecontent", SqlDbType.Int, 4).Value = coursecontent;
            cmd.Parameters.Add("@instructoreff", SqlDbType.Int, 4).Value = instructoreff;
            cmd.Parameters.Add("@instructorabilityexplain", SqlDbType.Int, 4).Value = instructorabilityexplain;
            cmd.Parameters.Add("@instructoreffective", SqlDbType.Int, 4).Value = instructoreffective;
            cmd.Parameters.Add("@instructorknowledge", SqlDbType.Int, 4).Value = instructorknowledge;
            cmd.Parameters.Add("@qualityoffacility", SqlDbType.Int, 4).Value = qualityoffacility;
            cmd.Parameters.Add("@overallcodecamp", SqlDbType.Int, 4).Value = overallcodecamp;
            cmd.Parameters.Add("@contentlevel", SqlDbType.Int, 4).Value = contentlevel;
            cmd.Parameters.Add("@favorite", SqlDbType.VarChar, 4096).Value = favorite == null ? String.Empty : favorite;
            cmd.Parameters.Add("@improved", SqlDbType.VarChar, 4096).Value = improved == null ? String.Empty : improved;
            cmd.Parameters.Add("@generalcomments", SqlDbType.VarChar, 4096).Value = generalcomments == null ? String.Empty : generalcomments;
            cmd.Parameters.Add("@discloseeval", SqlDbType.Bit, 1).Value = discloseeval;
            cmd.Parameters.Add("@sessionid", SqlDbType.Int, 4).Value = sessionId;
            //cmd.Parameters.Add("@original_id", SqlDbType.Int, 4).Value = id == 0 ? original_id : id;
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException err)
            {
                throw new ApplicationException("TestODS Update Error." + err.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public void Delete(int id, int original_id)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string deleteString = "DELETE FROM [dbo].[SessionEvals] WHERE id = @original_id";
            SqlCommand cmd = new SqlCommand(deleteString, connection);

            cmd.Parameters.Add("@original_id", SqlDbType.Int, 4).Value = id == 0 ? original_id : id;
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
    }
}





