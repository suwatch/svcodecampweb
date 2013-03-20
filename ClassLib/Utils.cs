using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using CodeCampSV;
using Encoder = System.Drawing.Imaging.Encoder;
using Image = System.Drawing.Image;

namespace CodeCampSV
{
    /// <summary>
    /// Summary description for Utils
    /// </summary>
    public class Utils
    {
        private const string STR_SessionCacheTimeoutSeconds = "SessionCacheTimeoutSeconds";
        // Session Level Interest

        #region InterestLevel enum

        public enum InterestLevel
        {
            NotInterested = 1,
            Interested = 2,
            WillAttend = 3
        }

        #endregion



        public static string AdminRoleName = "admin";
        public static string SponsorManagerRoleName = "SponsorManager";
        public static string VideoEditorRoleName = "VideoEditor";
        public static string TrackViewRoleName = "trackview";
        public static string RemovePrimarySpeakerRoleName = "removeprimaryspeaker";
        public static string AddMoreThanTwoSessionsRoleName = "AddMoreThanTwoSessions";
        public static string AddTwoSessionsRoleName = "AddTwoSessions";
        public static string AddThreeSessionsRoleName = "AddThreeSessions";
        public static string AddFourSessionsRoleName = "AddFourSessions";
        public static string VolunteerCoordinatorRoleName = "VolunteerCoordinator";
        public static string SubmitSessionRoleName = "SubmitSession";
        public static string NoAutoLoginForGUIDRoleName = "NoAutoLoginForGUID";
        public static string TagGroupGraphViewerRoleName = "TagGroupGraphViewer";
        public static string AllowRegistrationRoleName = "AllowRegistration";
        public static string SessionHashTaggerRoleName = "SessionHashTagger";
        public static string SpeakerAssignOwnMaterialsUrl = "SpeakerAssignOwnMaterialsUrl";


        public static string baseURL = "http://www.SiliconValley-CodeCamp.com/";
        public static int BigSize = 300;

        public static string CacheAgendaUpdateInfo = "AgendaUpdateInfo";

        public const int CurrentCodeCampYear = 8; // this is current year and never changes.  hence const
        public static string CacheAttendeeByUserName = "DisplayImageWithParams";
        public static string CacheAttendeeBySessionId = "DisplayImageWithParamsBySessionId";
        public static string CacheBySessionTimesId = "SessionTimesId";
        public static string CacheDisplayImage = "AttendeeByUserName";
        public static string CacheDisplayImageNoPic = "DisplayImageNoPic";
        public static string CacheEvaluationCount = "EvaluationCount";
        public static string CacheLectureRooms = "LectureRoomsAll";
        public static string CacheMailCancelFlag = "MailCancelFlag";
        public static string CacheMailSentStatusName = "MailSentStatusName";
        public static string CacheNumberRegisteredBeSeen = "NumberRegisteredBeSeen";
        public static string CachePictureDescriptions = "PictureDescriptions";
        public static string CacheProfileDataByUsername = "ProfileDataByUsername";
        public static string CacheSessionAttendeeBySessionId = "SessionAttendeeBySessionId";
        public static string CacheSessionAttendeeByUsername = "SessionAttendeeByUsername";
        public static string CacheSessionsAllByPresenterLastName = "SessionsAllByPresenterLastName";
        public static string CacheSessionsAllByStartTime = "SessionsAllByStartTime";
        public static string CacheSessionsAllByStartTimeOnlyAssigned = "SessionsAllByStartTimeOnlyAssigned";
        public static string CacheSessionsAllBySubmissionDate = "SessionsAllBySubmissionDate";
        public static string CacheSessionsAllByTitle = "SessionsAllByTitle";
        public static string CacheSessionsByUserGuid = "SessionsByUserGuid";
        public static string CacheSessionsByUsername = "SessionsByUsername";
        public static string CacheSessionsGetByTagWithParams = "SessionsGetByTagWithParams";
        public static string CacheSessionsGetByTrackWithParams = "SessionsGetByTrackWithParams";
        public static string CacheSessionTags = "SessionTags";
        public static string CacheSessionTimes = "SessionTimes";
        public static string CacheTagNameBySession = "TagNameBySession";
        public static string CacheTagsGetBySession = "TagsGetBySessionWithParams";
        public static string CacheRSSFeed = "CacheRSSFeed";

        public static int DefaultCloudTagsToShow = 10;
        public static object MailLocker = new object();
        public static int MaxSessionTagsToShow = 8;
        public static int MediumSize = 150;

        public static double MinSponsorLevelGold = 2500.00;
        public static double MinSponsorLevelPlatinum = 4500.00;
        public static double MinSponsorLevelSilver = 1000.00;
        public static double MinSponsorLevelBronze = 500.00;

        public static int SessionSlugLengthMax = 50;


        //public static double MinSponsorLevel1 = 1500.00;
        //public static double MinSponsorLevel2 = 500.00;
        //public static double MinSponsorLevel3 = 0.01;

        public static string ProfileDataSessionsHideDescriptions = "ShowSessionsHideDescriptions";
        public static string ProfileDataSessionsSortBy = "ShowSessionsSortBy";
        public static string ProfileDataShowOnlyAssignedSessions = "ShowOnlyAssignedSessions";
        public static int RoomNotAssigned = 19; // LectureRooms Table
        public static string Session_Interested = "Interested";
        public static string Session_NotInterested = "Not Interested";
        public static string Session_WillAttend = "Will Attend";
        public static string SurveyViewerRoleName = "surveyviewer";
        public static int ThumbSize = 600;
        public static int TimeSessionUnassigned = 10; // TimeSession table

        public static T SerializeFromString<T>(string xml)
        {
            var serializer = new XmlSerializer(typeof (T));

            using (var reader = new StringReader(xml))
            {
                return (T) serializer.Deserialize(reader);
            }
        }

        public static string SerializeToString(object obj)
        {
            var serializer = new XmlSerializer(obj.GetType());

            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);

                return writer.ToString();
            }
        }



        public static int GetNumberVolunteersNeededYear()
        {
            const string cache = "GetNumberNeededVolunteersThisYearCache";
            int total = 0;


            if (HttpContext.Current.Cache[cache] == null)
            {
                try
                {
                    using (
                        var sqlConnection =
                            new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
                    {
                        sqlConnection.Open();

                        string sqlSelect = "select SUM(numberneeded) from volunteerjob where codecampyearid=" +
                                           Utils.CurrentCodeCampYear.ToString(CultureInfo.InvariantCulture);
                        using (var sqlCommand = new SqlCommand(sqlSelect, sqlConnection))
                        {
                            total = (int) sqlCommand.ExecuteScalar();
                        }
                    }
                }
                catch (Exception)
                {
                }
                HttpContext.Current.Cache.Insert(cache, total,
                                                 null,
                                                 DateTime.Now.Add(new TimeSpan(0, 0,
                                                                               Utils.
                                                                                   RetrieveSecondsForSessionCacheTimeout
                                                                                   ())),
                                                 TimeSpan.Zero);
            }
            else
            {
                total = (int) HttpContext.Current.Cache[cache];
            }
            return total;
        }

        public static int GetNumberVolunteeredThisYear()
        {
            const string cache = "GetNumberVolunteeredThisYearCache";
            int total = 0;


            if (HttpContext.Current.Cache[cache] == null)
            {
                try
                {
                    using (
                        var sqlConnection =
                            new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
                    {
                        sqlConnection.Open();

                        string sqlSelect = string.Format(
                            @"SELECT DISTINCT count(*)_
                                                FROM AttendeeVolunteer
                                                WHERE VolunteerJobId  IN (
                                                                          select id
                                                                          from VolunteerJob
                                                                          where codecampyearid = {0})",
                            Utils.CurrentCodeCampYear.ToString(CultureInfo.InvariantCulture));
                        using (var sqlCommand = new SqlCommand(sqlSelect, sqlConnection))
                        {
                            total = (int) sqlCommand.ExecuteScalar();
                        }
                    }
                }
                catch (Exception)
                {
                }
                HttpContext.Current.Cache.Insert(cache, total,
                                                 null,
                                                 DateTime.Now.Add(new TimeSpan(0, 0,
                                                                               Utils.
                                                                                   RetrieveSecondsForSessionCacheTimeout
                                                                                   ())),
                                                 TimeSpan.Zero);
            }
            else
            {
                total = (int) HttpContext.Current.Cache[cache];
            }
            return total;
        }





        /// <summary>
        /// given a fully populated SessionResult, generate friendly lists of hashtags, twitterhandles and speakertime
        /// </summary>
        /// <param name="session"></param>
        /// <param name="hashTagsString"> </param>
        /// <param name="sessionTime"></param>
        /// <param name="sessionTimeFriendly"></param>
        /// <param name="speakerNames"> </param>
        /// <param name="speakerHandles"> </param>
        /// <returns></returns>
        public static void GetSessionHandlesHashTagsTime(SessionsResult session, out string speakerNames,
                                                         out string speakerHandles, out string hashTagsString,
                                                         out DateTime sessionTime,
                                                         out string sessionTimeFriendly)
        {
            int maxHandleCnt = 3;
            int maxHandleLength = 30;


            var sbspeakerHandles = new StringBuilder();
            var sbspeakerNames = new StringBuilder();
            var sbhashTags = new StringBuilder();

            if (session.SpeakersList != null && session.SpeakersList.Count > 0)
            {
                for (int index = 0; index < session.SpeakersList.Count; index++)
                {
                    var speaker = session.SpeakersList[index];
                    if (session.SpeakersList.Count == 1)
                    {
                        sbspeakerNames.Append(speaker.UserFirstName + " " + speaker.UserLastName);
                    }
                    else
                    {
                        sbspeakerNames.Append(speaker.UserLastName); // just include last name to keep it short
                    }

                    if (index < session.SpeakersList.Count - 1)
                    {
                        sbspeakerNames.Append(",");
                    }

                    if (!String.IsNullOrEmpty(speaker.TwitterHandle))
                    {
                        string speakerHandle = !speaker.TwitterHandle.StartsWith("@")
                                                   ? "@" + speaker.TwitterHandle
                                                   : speaker.TwitterHandle;
                        if (speakerHandle.Length > 1)
                        {
                            sbspeakerHandles.Append(speakerHandle + " ");
                        }
                    }
                }
            }

            if (!String.IsNullOrEmpty(session.TwitterHashTags))
            {
                char[] delimiterChars = {' ', ',', '.', ':', '\t'};
                var hashTags = session.TwitterHashTags.Split(delimiterChars).ToList();

                if (hashTags.Count > 0)
                {
                    for (int index = 0; index < Math.Min(hashTags.Count, maxHandleCnt); index++)
                    {
                        var hashTag = hashTags[index];
                        if (hashTag.Length > 1 && !hashTag.ToLower().Equals("svcc") &&
                            !hashTag.ToLower().Equals("#svcc"))
                        {
                            sbhashTags.Append(!hashTag.StartsWith("#") ? "#" + hashTag : hashTag);
                            sbhashTags.Append(" ");
                        }
                    }
                }
                if (sbhashTags.Length > maxHandleLength)
                {
                    sbhashTags = new StringBuilder();
                }
            }


            sessionTime = new DateTime(1999, 1, 1);
            sessionTimeFriendly = "";
            if (session.SessionTimesResult != null && session.SessionTimesResult.StartTime.HasValue)
            {
                sessionTime = session.SessionTimesResult.StartTime.Value;
                sessionTimeFriendly = String.Format("{0:g}", sessionTime);
            }

            int currentCodeCampYear = GetCurrentCodeCampYearStartDate().Year;
            if (sessionTime.Year < currentCodeCampYear)
            {
                sessionTimeFriendly = "Agenda Not Set";
            }

            hashTagsString = sbhashTags.ToString();
            speakerHandles = sbspeakerHandles.ToString();
            speakerNames = sbspeakerNames.ToString();

        }


        /// <summary>
        /// return 
        /// <a id="ctl00_ctl00_ctl00_blankContent_parentContent_MainContent_Repeater1_ctl01_HyperLink1" href="Speakers.aspx?id=345">Peter Kellner(SA:)</a>
        /// Build a horiz list of speakers links with speakerid's
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public static string GetAllSpeakersHtml(int sessionId)
        {

            List<Attendees> allSpeakers = GetSpeakersBySessionId(sessionId);


            var sb = new StringBuilder();

            int speakerCounter;

            for (speakerCounter = 0; speakerCounter < allSpeakers.Count; speakerCounter++)
            {
                var rec = allSpeakers.ElementAt(speakerCounter);
                sb.Append(FormatSpeakerHyperlink(rec.UserFirstName, rec.UserLastName, rec.Id, rec.SaturdayClasses,
                                                 rec.SundayClasses, true));
                if (speakerCounter < (allSpeakers.Count - 1))
                {
                    sb.Append(",&nbsp; ");
                }
            }
            sb.Append("&nbsp;&nbsp;&nbsp;");

            return sb.ToString();

        }

        public static object Olock = new Object();

        public static Dictionary<string, List<int>> GetSessionsBySearchWordsCached(int codeCampYearId)
        {
            string cacheString = String.Format("GetSessionsBySearchWordsCached-{0}", codeCampYearId);
            var o = (Dictionary<string, List<int>>) HttpContext.Current.Cache[cacheString];
            if (o == null)
            {
                lock (Olock)
                {
                    o = GetSessionsBySearchWords(codeCampYearId);
                    HttpContext.Current.Cache.Insert(cacheString, o,
                                                     null,
                                                     DateTime.Now.Add(new TimeSpan(0, 0,
                                                                                   RetrieveSecondsForSessionCacheTimeout
                                                                                       ())),
                                                     TimeSpan.Zero);
                }
            }
            return o;
        }


        /// <summary>
        /// Build a list of sessions associated with any keyword and cacheit
        /// </summary>
        /// <param name="codeCampYearId"></param>
        /// <returns></returns>
        public static Dictionary<string, List<int>> GetSessionsBySearchWords(int codeCampYearId)
        {



            int currentCodeCampYearId = GetCurrentCodeCampYear();

            var dict = new Dictionary<string, List<int>>();

            // build a list of tags for each session, then add to dict below

            // all tags for this year.
            var sessionTagsList =
                ManagerBase<SessionTagsManager, SessionTagsResult, SessionTags, CodeCampDataContext>.I.Get(new SessionTagsQuery
                    {
                        CodeCampYearId
                            =
                            currentCodeCampYearId

                    });

            var tagDict = ManagerBase<TagsManager, TagsResult, Tags, CodeCampDataContext>.I.Get(new TagsQuery()
                {

                }).ToDictionary(
                    k => k.Id,
                    v => v.TagName);

            var sessions =
                ManagerBase<SessionsManager, SessionsResult, Sessions, CodeCampDataContext>.I.Get(new SessionsQuery()
                    {
                        CodeCampYearId
                            =
                            codeCampYearId
                    });
            foreach (var session in sessions)
            {
                int sessionId = session.Id;

                List<string> titleWords = session.Title.Split(' ').ToList();
                foreach (var titleWord in titleWords)
                {
                    UpdateSessionKeyWord(dict, session.Id, titleWord);
                }

                List<string> presenterNames = session.PresenterName.Split(' ').ToList();
                foreach (var presenterName in presenterNames)
                {
                    UpdateSessionKeyWord(dict, session.Id, presenterName);
                }

                var sessionTags = (sessionTagsList.Where(data => data.SessionId == sessionId).Select(
                    data => data)).ToList();

                foreach (var sessionTag in sessionTags)
                {
                    if (tagDict.ContainsKey(sessionTag.TagId))
                    {
                        UpdateSessionKeyWord(dict, session.Id, tagDict[sessionTag.TagId]);
                    }
                }



            }


            return dict;
        }

        public class EventBoardSessionInterest
        {
            public string EventBoardEmail { get; set; }
            public int SessionId { get; set; }
            public int InterestLevelValue { get; set; }
        }

        public static List<EventBoardSessionInterest> GetEventBoardSessionInterest(string connectName,
                                                                                   string emailToFind,
                                                                                   int codeCampYearId,
                                                                                   string eventBoardId)
        {
            string eventBoardIdReal = ConfigurationManager.AppSettings["EventBoardId"] ?? String.Empty;

            if (codeCampYearId <= 0)
            {
                codeCampYearId = CurrentCodeCampYear;
            }
            var eventBoardSessionInterestList = new List<EventBoardSessionInterest>();

            if ((HttpContext.Current.Request.IsSecureConnection ||
                 HttpContext.Current.Request.Url.Host.ToLower().Equals("localhost")
                 || HttpContext.Current.Request.Url.Host.ToLower().Contains("dotnet4"))
                && eventBoardIdReal.Equals(eventBoardId))
            {


                using (
                    var sqlConnection =
                        new SqlConnection(ConfigurationManager.ConnectionStrings[connectName].ConnectionString))
                {
                    sqlConnection.Open();

                    string sqlSelect =
                        @"SELECT 
                          dbo.SessionInterestEBView.EmailEventBoard,
                          dbo.SessionInterestEBView.SessionId,
                          dbo.SessionInterestEBView.InterestLevel
                        FROM
                          dbo.SessionInterestEBView
                        WHERE
                          dbo.SessionInterestEBView.CodeCampYearId = @CodeCampYearId
                        ORDER BY
                          dbo.SessionInterestEBView.EmailEventBoard,dbo.SessionInterestEBView.SessionId";
                    if (!String.IsNullOrEmpty(emailToFind))
                    {
                        sqlSelect =
                            @"SELECT 
                              dbo.SessionInterestEBView.EmailEventBoard,
                              dbo.SessionInterestEBView.SessionId,
                              dbo.SessionInterestEBView.InterestLevel
                            FROM
                              dbo.SessionInterestEBView
                            WHERE
                              dbo.SessionInterestEBView.EmailEventBoard = @Email AND
                              dbo.SessionInterestEBView.CodeCampYearId = @CodeCampYearId";
                    }


                    try
                    {
                        var command = new SqlCommand(sqlSelect, sqlConnection);
                        command.Parameters.Add("@CodeCampYearId", SqlDbType.Int).Value = codeCampYearId;
                        if (!String.IsNullOrEmpty(emailToFind))
                        {
                            command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = emailToFind;
                        }

                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string emailEventBoard = reader.IsDBNull(0) ? String.Empty : reader.GetString(0);
                            int sessionId = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);
                            int interestlevel = reader.IsDBNull(2) ? -1 : reader.GetInt32(2);

                            eventBoardSessionInterestList.Add(
                                new EventBoardSessionInterest()
                                    {
                                        EventBoardEmail = emailEventBoard,
                                        InterestLevelValue = interestlevel,
                                        SessionId = sessionId
                                    }
                                );
                        }
                    }
                    catch (Exception eee)
                    {
                        throw new ApplicationException(eee.ToString());
                    }
                }
            }
            else
            {
                HttpContext.Current.Response.Write("https required and correct EventBoardId");
            }
            return eventBoardSessionInterestList;
        }


        private static void UpdateSessionKeyWord(Dictionary<string, List<int>> dict, int sessionId, string titleWord)
        {
            if (!dict.ContainsKey(titleWord.ToLower()))
            {
                dict.Add(titleWord.ToLower(), new List<int> {sessionId});
            }
            else
            {
                List<int> rec = dict[titleWord.ToLower()];
                if (!rec.Contains(sessionId))
                {
                    rec.Add(sessionId);
                    dict[titleWord.ToLower()] = rec;
                }
            }
        }




        public static List<Attendees> GetSpeakersBySessionId(int sessionId)
        {
            var allSpeakers = new List<Attendees>();

            string primarySpeakerUserName = GetUserNameFromSessionId(sessionId);
            bool removePrimarySpeaker = GetRemovePrimarySpeakerFromSessionId(sessionId);

            var primarySpeakerAttendeeList = GetSecondaryPresentersBySessionId(sessionId, primarySpeakerUserName, true);
            var secondarySpeakerAttendeeList = GetSecondaryPresentersBySessionId(sessionId, primarySpeakerUserName,
                                                                                 false);



            // only remove the primary speaker if there are secondary speakers.
            // never want to have a session with no speakers.
            if (!removePrimarySpeaker)
            {
                // if removePrimarySpeaker is not set, then always add primary
                allSpeakers.AddRange(primarySpeakerAttendeeList);
            }
            else
            {
                // remove primary speaker is true
                if (secondarySpeakerAttendeeList.Count == 0)
                {
                    // always add primary 8if no secondaries
                    allSpeakers.AddRange(primarySpeakerAttendeeList);
                }
            }
            allSpeakers.AddRange(secondarySpeakerAttendeeList);
            return allSpeakers;
        }

        /// <summary>
        /// get the speaker hyperlink in the pre SEO friendly way
        /// <a href=\"Speakers.aspx?AttendeeId={0}\">{1} {2} {3}</a>
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="speakerId"></param>
        /// <param name="saturdayClasses"></param>
        /// <param name="sundayClasses"></param>
        /// <param name="showSpeakerHyperLinks"></param>
        /// <returns></returns>
        public static string FormatSpeakerHyperlink(
            string firstName, string lastName, int speakerId,
            bool? saturdayClasses, bool? sundayClasses, bool showSpeakerHyperLinks)
        {
            var availability = new StringBuilder();
            if (CheckUserIsAdmin())
            {
                availability.Append("&nbsp;&nbsp;");
                if (saturdayClasses != null && saturdayClasses.Value)
                {
                    availability.Append("Sat");
                }
                availability.Append("-");
                if (sundayClasses != null && sundayClasses.Value)
                {
                    availability.Append("Sun");
                }
                availability.Append("");
            }

            string speakerTemplate = "<a href=\"Speakers.aspx?AttendeeId={0}\">{1} {2} {3}</a>";
            if (!showSpeakerHyperLinks)
            {
                speakerTemplate = "{1} {2} {3}";
            }

            string str = String.Format(speakerTemplate, speakerId, firstName, lastName, availability);



            return str;

        }



        public static void CacheClear(string typeOfCacheToClear)
        {
            if (typeOfCacheToClear.ToLower().Equals("sessions"))
            {
                HttpContext.Current.Cache.Remove(CacheSessionsAllByPresenterLastName);
                HttpContext.Current.Cache.Remove(CacheSessionsAllByTitle);
                HttpContext.Current.Cache.Remove(CacheSessionsAllBySubmissionDate);
                HttpContext.Current.Cache.Remove(CacheSessionsAllByStartTime);
                HttpContext.Current.Cache.Remove(CacheSessionsAllByStartTimeOnlyAssigned);
                HttpContext.Current.Cache.Remove(CacheSessionTags);
            }
        }

        public static string GetCodeCampDateStringByCodeCampYearId(int yearId)
        {
            string cacheListCodeCampYearResults = String.Format("CacheListCodeCampYearResults-{0}", yearId);
            var listCodeCampYear = (List<CodeCampYearResult>) HttpContext.Current.Cache[cacheListCodeCampYearResults];
            if (listCodeCampYear == null)
            {
                listCodeCampYear =
                    ManagerBase<CodeCampYearManager, CodeCampYearResult, CodeCampYear, CodeCampDataContext>.I.
                                                                                                            GetJustBaseTableColumns
                        (new CodeCampYearQuery
                            {
                                Id = yearId
                            });
                HttpContext.Current.Cache.Insert(cacheListCodeCampYearResults, listCodeCampYear,
                                                 null,
                                                 DateTime.Now.Add(new TimeSpan(0, 0,
                                                                               RetrieveSecondsForSessionCacheTimeout
                                                                                   ())),
                                                 TimeSpan.Zero);
            }

            return listCodeCampYear[0].CodeCampDateString;
        }

        /// <summary>
        /// This returns which year the person has said they want to view codecamp for.  
        /// That is, what is selected in upper right hand combobox on masterpage.
        /// (these numbers are 3,4,5,6,7,... for ...,2011,2012,...
        /// </summary>
        /// <returns></returns>
        public static int GetCurrentCodeCampYear()
        {
            // need to check because may be coming from handler with no session context.
            int currentCodeCampYear = CurrentCodeCampYear;
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    currentCodeCampYear = (int) HttpContext.Current.Session["CodeCampYear"];
                }
            }
            catch (Exception)
            {
            }
            return currentCodeCampYear;
        }



        //        /// <summary>
        //        /// thinking this is going to blow up because @attendee_username never set 
        //        /// </summary>
        //        /// <param name="username"></param>
        //        /// <returns></returns>
        //        public static Dictionary<int, int> GetDictionaryOfInterestLevelByAttendee(string username)
        //        {
        //            var dict = new Dictionary<int, int>();

        //            using (var sqlConnection =
        //                new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
        //            {
        //                // todo test
        //                sqlConnection.Open();
        //                var command =
        //                    new SqlCommand(
        //                        @"SELECT sessions_id,interestlevel 
        //                          FROM SessionAttendee 
        //                          WHERE attendee_username=@attendee_username",
        //                        sqlConnection);

        //                using (SqlDataReader reader = command.ExecuteReader())
        //                {
        //                    try
        //                    {
        //                        if (reader != null)
        //                        {
        //                            while (reader.Read())
        //                            {
        //                                int idSession = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
        //                                int interestLevel = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);
        //                                dict.Add(idSession, interestLevel);
        //                            }
        //                        }
        //                    }
        //                    catch (Exception eee1)
        //                    {
        //                        throw new ApplicationException(eee1.ToString());
        //                    }
        //                }
        //            }
        //            return dict;
        //        }

        //public static string GetRoomForScheduleId(int scheduleId)
        //{
        //    return string.Empty;
        //}

        public static void UpdateVistaStatus(string username, int vistaValue)
        {
            try
            {
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString);
                sqlConnection.Open();

                const string sqlSelect = "UPDATE Attendees SET VistaSlotsId = @VistaSlotsId WHERE Username = @Username";
                var sqlCommand = new SqlCommand(sqlSelect, sqlConnection);
                sqlCommand.Parameters.Add("@VistaSlotsId", SqlDbType.Int).Value = vistaValue;
                sqlCommand.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
            }
            catch (Exception eee)
            {
                throw new ApplicationException(eee.ToString());
            }
        }

        public static int GetNumberEvals()
        {
            int num;
            try
            {
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString);
                sqlConnection.Open();

                const string sqlSelect =
                    @"SELECT COUNT(*) FROM SessionEvals WHERE SessionId IN (
                        select sessionId
                        from sessionsoverview
                        where codecampyearid = {0})";
                var sqlCommand = new SqlCommand(String.Format(sqlSelect, GetCurrentCodeCampYear()), sqlConnection);

                num = (int) sqlCommand.ExecuteScalar();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            catch (Exception ee)
            {
                throw new ApplicationException(ee.ToString());
            }

            return num;
        }

        public static int GetNumberCodeCampEvals()
        {
            int num;
            try
            {
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString);
                sqlConnection.Open();

                const string sqlSelect = "SELECT COUNT(*) FROM CodeCampEvals WHERE CodeCampYearId = {0} ";
                var sqlCommand = new SqlCommand(String.Format(sqlSelect, GetCurrentCodeCampYear()), sqlConnection);
                num = (int) sqlCommand.ExecuteScalar();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            catch (Exception ee)
            {
                throw new ApplicationException(ee.ToString());
            }

            return num;
        }

        public static int GetNumberReferrals(Guid guidOfInterest)
        {
            int numberReferred = 0;
            try
            {
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString);
                sqlConnection.Open();

                const string sqlSelect = "SELECT COUNT(*) FROM Attendees WHERE ReferralGUID = @ReferralGUID";
                var sqlCommand = new SqlCommand(sqlSelect, sqlConnection);
                sqlCommand.Parameters.Add("@ReferralGUID", SqlDbType.UniqueIdentifier).Value = guidOfInterest;
                numberReferred = (int) sqlCommand.ExecuteScalar();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            catch (Exception)
            {
            }

            return numberReferred;
        }

        public static bool IsSqlServerOnLine()
        {
            var sqlConnection = new SqlConnection
                (ConfigurationManager.ConnectionStrings["CodeCampSV06FastTimeOut"].ConnectionString);

            bool goodConnection = true;

            try
            {
                sqlConnection.Open();
                sqlConnection.Close();
            }
            catch (Exception)
            {
                goodConnection = false;
            }

            return goodConnection;
        }

        public static int GetNumberRegisteredWithViewPermission()
        {
            int currentYear = GetCurrentCodeCampYear();

            string cacheName = "NumberRegisteredBeSeen";
            int numberRegisteredShareInfo = 0;

            if (HttpContext.Current.Cache[cacheName] == null)
            {
                try
                {
                    var sqlConnection =
                        new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString);
                    sqlConnection.Open();

                    string sqlSelect =
                        String.Format(
                            @"
                                SELECT count(*)
                                FROM dbo.AttendeesCodeCampYear,
                                     dbo.Attendees
                                WHERE dbo.AttendeesCodeCampYear.AttendeesId = dbo.Attendees.Id and
                                      Attendees.UserShareInfo = 1 and
                                      AttendeesCodeCampYear.CodeCampYearId = {0}",
                            currentYear);



                    var sqlCommand = new SqlCommand(sqlSelect, sqlConnection);
                    numberRegisteredShareInfo = (int) sqlCommand.ExecuteScalar();

                    sqlConnection.Close();
                    sqlConnection.Dispose();
                }
                catch (Exception)
                {
                }
                HttpContext.Current.Cache.Insert(cacheName, numberRegisteredShareInfo,
                                                 null,
                                                 DateTime.Now.Add(new TimeSpan(0, 0,
                                                                               RetrieveSecondsForSessionCacheTimeout())),
                                                 TimeSpan.Zero);
            }
            else
            {
                numberRegisteredShareInfo = (int) HttpContext.Current.Cache[cacheName];
            }

            return numberRegisteredShareInfo;
        }


        public static int GetNumberRegisteredForVista()
        {
            string cacheName = CacheNumberRegisteredBeSeen;
            int numberRegisteredShareInfo = 0;

            if (HttpContext.Current.Cache[cacheName] == null)
            {
                try
                {
                    var sqlConnection =
                        new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString);
                    sqlConnection.Open();

                    string sqlSelect = "SELECT COUNT(*) FROM Attendees WHERE VistaSlotsId <> 1 ";
                    var sqlCommand = new SqlCommand(sqlSelect, sqlConnection);
                    numberRegisteredShareInfo = (int) sqlCommand.ExecuteScalar();

                    sqlConnection.Close();
                    sqlConnection.Dispose();
                }
                catch (Exception)
                {
                }
                HttpContext.Current.Cache.Insert(cacheName, numberRegisteredShareInfo,
                                                 null,
                                                 DateTime.Now.Add(new TimeSpan(0, 0,
                                                                               RetrieveSecondsForSessionCacheTimeout())),
                                                 TimeSpan.Zero);
            }
            else
            {
                numberRegisteredShareInfo = (int) HttpContext.Current.Cache[cacheName];
            }

            return numberRegisteredShareInfo;
        }


        public static int GetNumberRegistered()
        {
            int codeCampYear = GetCurrentCodeCampYear();
            string cacheName = "NumberRegistered-" + codeCampYear.ToString();
            var numberRegistered = (int?) HttpContext.Current.Cache[cacheName];

            if (numberRegistered == null)
            {
                try
                {
                    var ccdc = new CodeCampDataContext();
                    numberRegistered = (from data in ccdc.AttendeesCodeCampYear
                                        where data.CodeCampYearId == GetCurrentCodeCampYear()
                                        select data.Id).Count();

                }
                catch (Exception)
                {
                }
                HttpContext.Current.Cache.Insert(cacheName, numberRegistered ?? 0,
                                                 null,
                                                 DateTime.Now.Add(new TimeSpan(0, 0,
                                                                               RetrieveSecondsForSessionCacheTimeout())),
                                                 TimeSpan.Zero);
            }

            return numberRegistered ?? 0;
        }

        public static int GetNumberSessions()
        {
            string cacheName = "NumberSessions" + GetCurrentCodeCampYear().ToString();

            var numberSessions = (int?) HttpContext.Current.Cache[cacheName];

            if (numberSessions == null)
            {
                try
                {
                    var ccdc = new CodeCampDataContext();
                    numberSessions = (from data in ccdc.Sessions
                                      where data.CodeCampYearId == GetCurrentCodeCampYear()
                                      select data.Id).Count();
                }
                catch (Exception)
                {
                }
                HttpContext.Current.Cache.Insert(cacheName, numberSessions.Value,
                                                 null,
                                                 DateTime.Now.Add(new TimeSpan(0, 0,
                                                                               RetrieveSecondsForSessionCacheTimeout())),
                                                 TimeSpan.Zero);
            }

            return numberSessions.Value;
        }

        /// <summary>
        /// given the attendee's ID, return the logged in username
        /// </summary>
        /// <param name="attendeeId"></param>
        /// <returns></returns>
        public static string GetUsernameFromAttendeeId(int attendeeId)
        {
            AttendeesResult attendeesResult =
                (ManagerBase<AttendeesManager, AttendeesResult, Attendees, CodeCampDataContext>.I.Get(new AttendeesQuery
                                                                                                          ()
                    {
                        Id =
                            attendeeId
                    })).
                    SingleOrDefault();
            string retUsername = String.Empty;
            if (attendeesResult != null)
            {
                retUsername = attendeesResult.Username;
            }
            return retUsername;
        }

        public static string GetFirstLastFromAttendeeId(int attendeeId)
        {
            AttendeesResult attendeesResult =
                (ManagerBase<AttendeesManager, AttendeesResult, Attendees, CodeCampDataContext>.I.Get(new AttendeesQuery
                                                                                                          ()
                    {
                        Id =
                            attendeeId
                    })).
                    SingleOrDefault();
            string retUsername = String.Empty;
            if (attendeesResult != null)
            {
                retUsername = String.Format("{0} {1}", attendeesResult.UserFirstName, attendeesResult.UserLastName);
            }
            return retUsername;
        }

        public static int GetAttendeeVistaStatusByUsername(string username)
        {
            int vistaIdStatus = -1;
            try
            {
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString);
                sqlConnection.Open();

                string sqlSelect = "select VistaSlotsId FROM attendees WHERE Username = @Username";
                var sqlCommand = new SqlCommand(sqlSelect, sqlConnection);
                sqlCommand.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
                vistaIdStatus = (int) sqlCommand.ExecuteScalar();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            catch (Exception ee)
            {
                throw new ApplicationException(ee.ToString());
            }

            return vistaIdStatus;
        }


        /// <summary>
        /// Vistaonly really means is speaker willing to let people email them
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static bool GetAttendeeVistaOnlyByUsername(string username)
        {
            string cacheGetAttendeeVistaOnlyByUsername = String.Format("CacheGetAttendeeVistaOnlyByUsername-{0}",
                                                                       username);
            bool? vistaOnly = (bool?) HttpContext.Current.Cache[cacheGetAttendeeVistaOnlyByUsername];
            if (vistaOnly == null)
            {
                vistaOnly = false;
                try
                {
                    using (
                        var sqlConnection =
                            new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
                    {
                        sqlConnection.Open();
                        const string sqlSelect = "select VistaOnly FROM attendees WHERE Username = @Username";
                        var sqlCommand = new SqlCommand(sqlSelect, sqlConnection);
                        sqlCommand.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
                        using (SqlDataReader reader1 = sqlCommand.ExecuteReader())
                        {
                            while (reader1.Read())
                                vistaOnly = reader1.IsDBNull(0) ? false : reader1.GetBoolean(0);
                        }
                        sqlCommand.Dispose();
                        sqlConnection.Close();
                    }
                }
                catch (Exception ee)
                {
                    throw new ApplicationException(ee.ToString());
                }
                HttpContext.Current.Cache.Insert(cacheGetAttendeeVistaOnlyByUsername, vistaOnly, null,
                                                 DateTime.Now.Add(new TimeSpan(0, 0,
                                                                               RetrieveSecondsForSessionCacheTimeout())),
                                                 TimeSpan.Zero);
            }

            return vistaOnly.Value;
        }

        public static string GetAttendeeNameByUsername(string userName)
        {
            return GetAttendeeNameByUsername(userName, false);
        }

        public static string GetAttendeeNameByUsername(string userName, bool tackOnSaturdaySundayAttendanceIfAdmin)
        {
            var attendeesODS = new AttendeesODS();
            List<AttendeesODS.DataObjectAttendees> listAttendees =
                attendeesODS.GetByUsername(String.Empty, userName);
            string retString = String.Empty;

            if (listAttendees.Count > 0)
            {
                retString = listAttendees[0].Userfirstname + " " + listAttendees[0].Userlastname;
                if (tackOnSaturdaySundayAttendanceIfAdmin)
                {
                    retString += "(" + (listAttendees[0].Saturdayclasses ? "SA:" : ":");
                    retString += (listAttendees[0].Sundayclasses ? "SU)" : ")");
                }
            }
            return retString;
        }


        public static string GetAttendeeUsernameByGUID(string guidString)
        {
            string username;
            using (
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
                try
                {
                    sqlConnection.Open();

                    const string sqlSelect = "select Username FROM attendees WHERE PKID=@PKID";
                    var sqlCommand = new SqlCommand(sqlSelect, sqlConnection);
                    var guid = new Guid(guidString);
                    sqlCommand.Parameters.Add("@PKID", SqlDbType.UniqueIdentifier).Value = guid;
                    username = (string) sqlCommand.ExecuteScalar();
                }
                catch (Exception ee)
                {
                    throw new ApplicationException(ee.ToString());
                }

            return username;
        }

        public static string GetAttendeeUsernameByPKID(string pkid)
        {
            //todo: add caching here possibly

            string username;
            using (
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
                try
                {
                    sqlConnection.Open();
                    const string sqlSelect = "select Username FROM attendees WHERE PKID=@PKID";
                    var sqlCommand = new SqlCommand(sqlSelect, sqlConnection);
                    sqlCommand.Parameters.Add("@PKID", SqlDbType.UniqueIdentifier).Value = new Guid(pkid);
                    username = (string) sqlCommand.ExecuteScalar();
                    sqlCommand.Dispose();
                }
                catch (Exception ee)
                {
                    throw new ApplicationException(ee.ToString());
                }

            return username;
        }

        public static string GetAttendeePKIDByUsername(string userName)
        {
            //todo: add caching here possibly

            Guid guid;
            using (
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
                try
                {
                    sqlConnection.Open();
                    const string sqlSelect = "select PKID FROM attendees WHERE Username=@Username";
                    var sqlCommand = new SqlCommand(sqlSelect, sqlConnection);
                    sqlCommand.Parameters.Add("@Username", SqlDbType.NVarChar).Value = userName;
                    guid = (Guid) sqlCommand.ExecuteScalar();
                    sqlCommand.Dispose();
                }
                catch (Exception ee)
                {
                    throw new ApplicationException(ee.ToString());
                }

            return guid.ToString();



            //var attendeesODS = new AttendeesODS();
            //List<AttendeesODS.DataObjectAttendees> listAttendees =
            //    attendeesODS.GetByUsername(string.Empty, userName);
            //string retString = string.Empty;

            //if (listAttendees.Count > 0)
            //{
            //    retString = listAttendees[0].Pkid.ToString();
            //}
            //return retString;
        }

        public static AttendeesODS.DataObjectAttendees GetAttendeeByUsername(string userName)
        {
            var attendeesODS = new AttendeesODS();
            List<AttendeesODS.DataObjectAttendees> listAttendees =
                attendeesODS.GetByUsername(String.Empty, userName);

            return listAttendees.Count == 0 ? null : listAttendees[0];
        }


        public static bool CheckUserIsAllowRegistration()
        {
            return Roles.IsUserInRole(AllowRegistrationRoleName);
        }

        public static bool CheckUserIsSpeakerAssignOwnMaterialsUrl()
        {
            return Roles.IsUserInRole(SpeakerAssignOwnMaterialsUrl);
        }

        public static bool CheckUserIsSponsorManagerOrAdmin()
        {
            return CheckUserIsAdmin() || Roles.IsUserInRole(SponsorManagerRoleName);
        }

        public static bool CheckUserIsNoAutoLoginForGUID()
        {
            return Roles.IsUserInRole(NoAutoLoginForGUIDRoleName);
        }

        public static bool CheckUserIsTagGroupGraphViewer()
        {
            return Roles.IsUserInRole(TagGroupGraphViewerRoleName);
        }

        public static bool CheckUserIsAdmin()
        {
            return Roles.IsUserInRole(AdminRoleName);
        }

        public static bool CheckUserIsVideoEditor()
        {
            return Roles.IsUserInRole(VideoEditorRoleName);
        }

        public static bool CheckUserIsSubmitSession()
        {
            return Roles.IsUserInRole(SubmitSessionRoleName);
        }

        public static bool CheckUserIsVolunteerCoordinator()
        {
            return Roles.IsUserInRole(VolunteerCoordinatorRoleName);
        }

        public static bool CheckUserIsRemovePrimarySpeaker()
        {

            return Roles.IsUserInRole(RemovePrimarySpeakerRoleName);
        }


        public static bool CheckUserIsSurveyViewer()
        {
            return Roles.IsUserInRole(SurveyViewerRoleName);
        }

        public static bool CheckUserIsScheduler()
        {
            return Roles.IsUserInRole("scheduler");
        }

        public static bool CheckUserIsPresenterOrAdmin()
        {
            return Roles.IsUserInRole(AdminRoleName) || Roles.IsUserInRole("presenter");
        }

        public static bool CheckUserIsTrackLeadOrAdmin()
        {
            return Roles.IsUserInRole(AdminRoleName) || Roles.IsUserInRole(TrackViewRoleName);
        }

        public static byte[] ResizeFromByteArray(string fileName, int MaxSideSize, Byte[] byteArrayIn)
        {
            byte[] byteArray; // really make this an error gif
            using (var memoryStream = new MemoryStream(byteArrayIn))
            {
                byteArray = ResizeFromStream(fileName, MaxSideSize, memoryStream);
                return byteArray;
            }
        }

        /// <summary>
        /// converts from input stream to output bytearray
        /// inspired from: http://www.eggheadcafe.com/articles/20030515.asp
        public static byte[] ResizeFromStream(string fileName, int MaxSideSize, Stream Buffer)
        {
            byte[] byteArray; // really make this an error gif

            try
            {
                using (var bitMap = new Bitmap(Buffer))
                {
                    int intOldWidth = bitMap.Width;
                    int intOldHeight = bitMap.Height;
                    int intNewWidth;
                    int intNewHeight;
                    int intMaxSide;
                    if (intOldWidth >= intOldHeight)
                        intMaxSide = intOldWidth;
                    else
                        intMaxSide = intOldHeight;
                    if (intMaxSide > MaxSideSize)
                    {
                        //set new width and height
                        double dblCoef = MaxSideSize/(double) intMaxSide;
                        intNewWidth = Convert.ToInt32(dblCoef*intOldWidth);
                        intNewHeight = Convert.ToInt32(dblCoef*intOldHeight);
                    }
                    else
                    {
                        intNewWidth = intOldWidth;
                        intNewHeight = intOldHeight;
                    }
                    var ThumbNailSize = new Size(intNewWidth, intNewHeight);
                    Image oImg = Image.FromStream(Buffer);
                    Image oThumbNail = new Bitmap(ThumbNailSize.Width, ThumbNailSize.Height);
                    Graphics oGraphic = Graphics.FromImage(oThumbNail);
                    oGraphic.CompositingQuality = CompositingQuality.HighQuality;
                    oGraphic.SmoothingMode = SmoothingMode.HighQuality;
                    oGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    var oRectangle = new Rectangle(0, 0, ThumbNailSize.Width, ThumbNailSize.Height);
                    oGraphic.DrawImage(oImg, oRectangle);
                    //string fileName = Context.Server.MapPath("~/App_Data/") + "test1.jpg";
                    //oThumbNail.Save(fileName, ImageFormat.Jpeg);
                    var ms = new MemoryStream();

                    ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
                    // Create an Encoder object based on the GUID
                    // for the Quality parameter category.
                    var myEncoder = Encoder.Quality;
                    var myEncoderParameters = new EncoderParameters(1);

                    // the second parameter is the percent quality.  10L is 10%   90L would be 90%
                    var myEncoderParameter = new EncoderParameter(myEncoder, 95L);

                    myEncoderParameters.Param[0] = myEncoderParameter;
                    oThumbNail.Save(ms, jgpEncoder, myEncoderParameters);




                    byteArray = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(byteArray, 0, Convert.ToInt32(ms.Length));
                    oGraphic.Dispose();
                    oImg.Dispose();
                    ms.Close();
                    ms.Dispose();
                }
            }
            catch (Exception)
            {
                int newSize = MaxSideSize - 20;
                var bitMap = new Bitmap(newSize, newSize);
                Graphics g = Graphics.FromImage(bitMap);
                g.FillRectangle(new SolidBrush(Color.Gray), new Rectangle(0, 0, newSize, newSize));

                var font = new Font("Courier", 8);
                var solidBrush = new SolidBrush(Color.Red);
                g.DrawString("Failed File", font, solidBrush, 10, 5);
                g.DrawString(fileName, font, solidBrush, 10, 50);

                var ms = new MemoryStream();
                bitMap.Save(ms, ImageFormat.Jpeg);
                byteArray = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(byteArray, 0, Convert.ToInt32(ms.Length));

                ms.Close();
                ms.Dispose();
                bitMap.Dispose();
                solidBrush.Dispose();
                g.Dispose();
                font.Dispose();
            }
            return byteArray;
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
        }

        public static void ClearDisplayAdCache()
        {
            // invalidate cache for adds when we add a new user
            HttpContext.Current.Cache.Remove("DisplayAd_1");
            HttpContext.Current.Cache.Remove("DisplayAd_2");
            HttpContext.Current.Cache.Remove("DisplayAd_3");
            HttpContext.Current.Cache.Remove("DisplayAd_4");
        }

        public static bool GetAttendeeOnOrBeforeSept11(string userName)
        {
            DateTime creationDate;
            try
            {
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString);
                sqlConnection.Open();

                const string sqlSelect = "select CreationDate FROM attendees WHERE Username = @Username";
                var sqlCommand = new SqlCommand(sqlSelect, sqlConnection);
                sqlCommand.Parameters.Add("@Username", SqlDbType.VarChar).Value = userName;
                creationDate = (DateTime) sqlCommand.ExecuteScalar();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            catch (Exception ee)
            {
                throw new ApplicationException(ee.ToString());
            }
            return creationDate.CompareTo(new DateTime(2006, 9, 11, 23, 59, 59)) <= 0 ? true : false;
        }

        public static int GetNumberAttendeesAttendingBothDays()
        {
            const string countBothDaysSqlTemplate = @"
                SELECT COUNT(*)
                FROM Attendees
                WHERE attendees.SaturdayClasses = 1 AND
                      attendees.SundayClasses = 1 and
                      Attendees.Id IN (
                                        SELECT AttendeesId
                                        FROM AttendeesCodeCampYear
                                        WHERE CodeCampYearId = {0} )";

            int cnt;
            using (
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();
                string sqlSelect = String.Format(countBothDaysSqlTemplate, GetCurrentCodeCampYear());


                using (var sqlCommand = new SqlCommand(sqlSelect, sqlConnection))
                {
                    cnt = (int) sqlCommand.ExecuteScalar();
                }
                sqlConnection.Close();
            }
            return cnt;
        }

        public static int GetNumberSpeakerCanEmailInterested()
        {
            const string count = @"
                 SELECT count(*)
                FROM dbo.AttendeesCodeCampYear
                     INNER JOIN dbo.Attendees ON (dbo.AttendeesCodeCampYear.AttendeesId =
                     dbo.Attendees.Id)
                WHERE dbo.Attendees.AllowEmailToSpeakerInterested = 1 AND
                dbo.AttendeesCodeCampYear.CodeCampYearId = {0}";

            int cnt;
            using (
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();
                string sqlSelect = String.Format(count, GetCurrentCodeCampYear());


                using (var sqlCommand = new SqlCommand(sqlSelect, sqlConnection))
                {
                    cnt = (int) sqlCommand.ExecuteScalar();
                }
                sqlConnection.Close();
            }
            return cnt;
        }

        public static int GetNumberSpeakerCanEmailPlanToAttend()
        {
            const string count = @"
                 SELECT count(*)
                FROM dbo.AttendeesCodeCampYear
                     INNER JOIN dbo.Attendees ON (dbo.AttendeesCodeCampYear.AttendeesId =
                     dbo.Attendees.Id)
                WHERE dbo.Attendees.AllowEmailToSpeakerPlanToAttend = 1 AND
                dbo.AttendeesCodeCampYear.CodeCampYearId = {0}";

            int cnt;
            using (
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();
                string sqlSelect = String.Format(count, GetCurrentCodeCampYear());


                using (var sqlCommand = new SqlCommand(sqlSelect, sqlConnection))
                {
                    cnt = (int) sqlCommand.ExecuteScalar();
                }
                sqlConnection.Close();
            }
            return cnt;
        }

        public static int GetNumberAttendeesByParam(string param)
        {
            using (
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();
                string sqlSelect = String.Empty;
                if (param.Equals("SaturdayClasses"))
                    sqlSelect =
                        String.Format(
                            "SELECT COUNT(*) FROM Attendees WHERE SaturdayClasses = 1 AND Attendees.Id IN (SELECT AttendeesId FROM AttendeesCodeCampYear WHERE CodeCampYearId = {0})",
                            GetCurrentCodeCampYear());
                else if (param.Equals("SundayClasses"))
                    sqlSelect =
                        String.Format(
                            "SELECT COUNT(*) FROM Attendees WHERE SundayClasses = 1 AND Attendees.Id IN (SELECT AttendeesId FROM AttendeesCodeCampYear WHERE CodeCampYearId = {0})",
                            GetCurrentCodeCampYear());
                else if (param.Equals("SessionsInterest"))
                {
                    string sqlSelectTemplate =
                        "SELECT COUNT(*) FROM SessionAttendee WHERE interestlevel = 2 AND sessions_id IN (SELECT id from sessions WHERE CodeCampYearId = {0}) ";
                    sqlSelect = String.Format(sqlSelectTemplate, GetCurrentCodeCampYear());
                }
                else if (param.Equals("SessionsPlanAttend"))
                {
                    string sqlSelectTemplate =
                        "SELECT COUNT(*) FROM SessionAttendee WHERE interestlevel = 3 AND sessions_id IN (SELECT id from sessions WHERE CodeCampYearId = {0}) ";
                    sqlSelect = String.Format(sqlSelectTemplate, GetCurrentCodeCampYear());
                }

                var sqlCommand = new SqlCommand(sqlSelect, sqlConnection);
                var cnt = (int) sqlCommand.ExecuteScalar();
                sqlConnection.Close();
                return cnt;
            }


            //int numberAttendingSaturday = CodeCampSV.Utils.GetNumberAttendeesByParam("SaturdayClasses");
            //LabelSaturdayClasses.Text = numberAttendingSaturday.ToString();

            //int numberAttendingSunday = CodeCampSV.Utils.GetNumberAttendeesByParam("SundayClasses");
            //LabelSundayClasses.Text = numberAttendingSunday.ToString();

            //int numberVistaOnly = CodeCampSV.Utils.GetNumberAttendeesByParam("VistaOnly");
            //LabelSaturdayClasses.Text = numberVistaOnly.ToString();

            //int numberVistaDesktop = CodeCampSV.Utils.GetNumberAttendeesByParam("VistaDesktop");
            //LabelVistaDesktop.Text = numberVistaDesktop.ToString();

            //int numberVistaLaptop = CodeCampSV.Utils.GetNumberAttendeesByParam("VistaLaptop");
            //LabelVistaDesktop.Text = numberVistaLaptop.ToString();
        }

        /// <summary>
        /// Update SessionId with roomid and SessionTimeId
        /// (unassign session from other places)
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="roomId"></param>
        /// <param name="sessionTimeId"></param>
        public static void AgendaUpdateSession(int sessionId, int roomId, int sessionTimeId)
        {
            // $$$ Broken scope, will not work.  See SessionsEdit.aspx for correct way
            using (var sqlConnection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();
                //using (var scope = new TransactionScope())
                //{
                try
                {
                    string sqlUpdate = "UPDATE Sessions SET LectureRoomsId = " +
                                       RoomNotAssigned + ",SessionTimesId = " +
                                       TimeSessionUnassigned +
                                       " WHERE LectureRoomsId = @roomid AND SessionTimesId = @sessiontimesid AND CodeCampYearid = @CodeCampYearId ";

                    var sqlCommand = new SqlCommand(sqlUpdate, sqlConnection);
                    sqlCommand.Parameters.Add("@CodeCampYearId", SqlDbType.Int).Value = CurrentCodeCampYear;
                    sqlCommand.Parameters.Add("@roomid", SqlDbType.Int).Value = roomId;
                    sqlCommand.Parameters.Add("@sessiontimesid", SqlDbType.Int).Value = sessionTimeId;
                    int rowsUpdated = sqlCommand.ExecuteNonQuery();


                    if (sessionId >= 0)
                    {
                        sqlUpdate = "UPDATE Sessions SET LectureRoomsId = " + roomId + "," +
                                    "SessionTimesId = " + sessionTimeId + " WHERE " +
                                    "  id=@id AND CodeCampYearId = @CodeCampYearId";
                        var sqlCommand1 = new SqlCommand(sqlUpdate, sqlConnection);
                        sqlCommand1.Parameters.Add("@id", SqlDbType.Int).Value = sessionId;
                        sqlCommand1.Parameters.Add("@CodeCampYearId", SqlDbType.Int).Value = CurrentCodeCampYear;
                        int rowsUpdated1 = sqlCommand1.ExecuteNonQuery();
                    }
                    // scope.Complete();
                }
                catch (Exception ee)
                {
                    throw new ApplicationException(ee.ToString());
                }
                //}
            }
        }

        /// <summary>
        /// unassign all rooms for all slots current code camp year.
        /// </summary>
        public static void UnAssignAllRooms()
        {
            var sqlConnection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString);
            sqlConnection.Open();

            try
            {
                string sqlUpdate = "UPDATE Sessions SET LectureRoomsId = " +
                                   RoomNotAssigned + " WHERE CodeCampYearId = " +
                                   CurrentCodeCampYear.ToString();

                var sqlCommand = new SqlCommand(sqlUpdate, sqlConnection);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ee1)
            {
                throw new ApplicationException(ee1.ToString());
            }
        }

        /// <summary>
        /// 
        /// unassign all times for all sessoinsgiven code camp year
        /// </summary>
        public static void UnAssignAllTimes()
        {
            var sqlConnection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString);
            sqlConnection.Open();

            try
            {
                string sqlUpdate = "UPDATE Sessions SET SessionTimesId = " +
                                   TimeSessionUnassigned + " WHERE CodeCampYearId = " +
                                   CurrentCodeCampYear.ToString();

                var sqlCommand = new SqlCommand(sqlUpdate, sqlConnection);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ee1)
            {
                throw new ApplicationException(ee1.ToString());
            }
        }

        public static List<DataObjectAgendaUpdateInfo> GetListAgendaUpdateInfo()
        {
            var liAgendaUpdateInfo = new List<DataObjectAgendaUpdateInfo>();

            if (HttpContext.Current.Cache[CacheAgendaUpdateInfo] == null)
            {
                var interestedDictionary = new Dictionary<int, int>();
                var willAttendDictionary = new Dictionary<int, int>();

                //string interestedString = Convert.ToInt32(CodeCampSV.Utils.InterestLevel.WillAttend).ToString();

                // First, make dictionary of interested
                string sqlSelectInterest = "select sessions_id,count(*) from sessionattendee " +
                                           "where interestlevel = " + Convert.ToInt32(InterestLevel.Interested) +
                                           " group by sessions_id ";

                string sqlSelectWillAttend = "select sessions_id,count(*) from sessionattendee " +
                                             "where interestlevel = " + Convert.ToInt32(InterestLevel.WillAttend) +
                                             " group by sessions_id ";

                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString);
                sqlConnection.Open();
                SqlDataReader reader1 = null;
                try
                {
                    var command1 = new SqlCommand(sqlSelectInterest, sqlConnection);
                    reader1 = command1.ExecuteReader();
                    while (reader1.Read())
                    {
                        int idSession = reader1.IsDBNull(0) ? -1 : reader1.GetInt32(0);
                        int interestLevel = reader1.IsDBNull(1) ? -1 : reader1.GetInt32(1);
                        interestedDictionary.Add(idSession, interestLevel);
                    }
                }
                catch (Exception eee1)
                {
                    throw new ApplicationException(eee1.ToString());
                }
                finally
                {
                    if (reader1 != null) reader1.Dispose();
                }

                SqlDataReader reader2 = null;
                try
                {
                    var command2 = new SqlCommand(sqlSelectWillAttend, sqlConnection);
                    reader2 = command2.ExecuteReader();
                    while (reader2.Read())
                    {
                        int idSession = reader2.IsDBNull(0) ? -1 : reader2.GetInt32(0);
                        int interestLevel = reader2.IsDBNull(1) ? -1 : reader2.GetInt32(1);
                        willAttendDictionary.Add(idSession, interestLevel);
                    }
                }
                catch (Exception eee2)
                {
                    throw new ApplicationException(eee2.ToString());
                }
                finally
                {
                    if (reader2 != null) reader2.Dispose();
                }


                liAgendaUpdateInfo = new List<DataObjectAgendaUpdateInfo>();
                string sqlSelect =
                    String.Format(@"SELECT 
                  Attendees.UserFirstName,
                  Attendees.UserLastName,
                  dbo.Sessions.title,
                  dbo.Sessions.id,
                  dbo.Sessions.LectureRoomsId,
                  dbo.Sessions.SessionTimesId
                FROM
                  Attendees
                  INNER JOIN dbo.Sessions ON (Attendees.Username = dbo.Sessions.Username)
                WHERE dbo.Sessions.CodeCampYearId = {0}", CurrentCodeCampYear);

                SqlDataReader reader3 = null;
                try
                {
                    var command3 = new SqlCommand(sqlSelect, sqlConnection);
                    reader3 = command3.ExecuteReader();
                    while (reader3.Read())
                    {
                        string firstName = reader3.IsDBNull(0) ? String.Empty : reader3.GetString(0);
                        string lastName = reader3.IsDBNull(1) ? String.Empty : reader3.GetString(1);
                        string title = reader3.IsDBNull(2) ? String.Empty : reader3.GetString(2);
                        int sessionId = reader3.IsDBNull(3) ? -1 : reader3.GetInt32(3);
                        int roomId = reader3.IsDBNull(4) ? -1 : reader3.GetInt32(4);
                        int sessionTimesId = reader3.IsDBNull(5) ? -1 : reader3.GetInt32(5);
                        var dataObject = new DataObjectAgendaUpdateInfo
                            {
                                SessionAuthor = (firstName + " " + lastName),
                                SessionTitle = title,
                                SessionId = sessionId,
                                LectureRoomId = roomId,
                                SessionTimesId = sessionTimesId
                            };
                        if (interestedDictionary.ContainsKey(sessionId))
                        {
                            dataObject.Interested = interestedDictionary[sessionId];
                        }
                        if (willAttendDictionary.ContainsKey(sessionId))
                        {
                            dataObject.WillAttend = willAttendDictionary[sessionId];
                        }
                        liAgendaUpdateInfo.Add(dataObject);
                    }
                }
                catch (Exception eee3)
                {
                    throw new ApplicationException(eee3.ToString());
                }
                finally
                {
                    if (reader3 != null) reader3.Dispose();
                }
                sqlConnection.Close();
                HttpContext.Current.Cache.Insert(CacheAgendaUpdateInfo, liAgendaUpdateInfo,
                                                 null,
                                                 DateTime.Now.Add(new TimeSpan(0, 0,
                                                                               RetrieveSecondsForSessionCacheTimeout())),
                                                 TimeSpan.Zero);
            }
            else
            {
                liAgendaUpdateInfo = (List<DataObjectAgendaUpdateInfo>) HttpContext.Current.Cache[CacheAgendaUpdateInfo];
            }
            return liAgendaUpdateInfo;
        }

        public static bool GetProfileDataBool(string keyName, bool defaultValue)
        {
            bool ret = defaultValue;
            var pdODS = new ProfileDataODS();
            List<ProfileDataODS.DataObjectProfileData> li =
                pdODS.GetByUsername(HttpContext.Current.User.Identity.Name);
            foreach (ProfileDataODS.DataObjectProfileData dopd in li)
            {
                if (dopd.Keyname.Equals(keyName))
                {
                    if (dopd.Data.ToLower().Equals("true"))
                    {
                        ret = true;
                    }
                    else
                    {
                        ret = false;
                    }
                    break;
                }
            }
            return ret;
        }

        public static string GetProfileDataString(string keyName, string defaultString)
        {
            string retString = defaultString;
            var pdODS = new ProfileDataODS();
            List<ProfileDataODS.DataObjectProfileData> li =
                pdODS.GetByUsername(HttpContext.Current.User.Identity.Name);
            foreach (ProfileDataODS.DataObjectProfileData dopd in li)
            {
                if (dopd.Keyname.Equals(keyName))
                {
                    retString = dopd.Data;
                    break;
                }
            }
            return retString;
        }

        public static void ClearCacheForSessionTags(int sessionId)
        {
            var sqlConnection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString);
            sqlConnection.Open();
            SqlDataReader reader1 = null;
            try
            {
                string sqlSelect = "SELECT tagid FROM SessionTags  WHERE sessionid=@sessionid";
                var command1 = new SqlCommand(sqlSelect, sqlConnection);
                command1.Parameters.Add("@sessionid", SqlDbType.Int).Value = sessionId;
                reader1 = command1.ExecuteReader();
                while (reader1.Read())
                {
                    int tagid = reader1.IsDBNull(0) ? -1 : reader1.GetInt32(0);
                    string cacheName = String.Format("{0}_{1}", CacheSessionsGetByTagWithParams, tagid);
                    HttpContext.Current.Cache.Remove(cacheName);
                }
            }
            catch (Exception eee1)
            {
                throw new ApplicationException(eee1.ToString());
            }
            finally
            {
                if (reader1 != null) reader1.Dispose();
            }
            sqlConnection.Close();
            sqlConnection.Dispose();
        }

        public static void ClearCacheForAttendeeImage(string username)
        {
            string pkidStr = GetAttendeePKIDByUsername(username);
            string cacheName = CacheDisplayImage + "?pkid=" + pkidStr;
            HttpContext.Current.Cache.Remove(cacheName);
        }

        /// <summary>
        /// gets speaker name "first last" of speaker
        /// </summary>
        /// <param name="sessionsId"></param>
        /// <returns></returns>
        public static string GetUserNameOfSession(int sessionsId)
        {
            string username = GetUserNameFromSessionId(sessionsId);
            return GetAttendeeNameByUsername(username);
        }

        /// <summary>
        /// gets username (logged in usernae)
        /// </summary>
        /// <param name="sessionsId"></param>
        /// <returns></returns>
        public static string GetUserNameFromSessionId(int sessionsId)
        {
            string cacheGetUserNameFromSessionId = String.Format("CacheGetUserNameFromSessionId-{0}", sessionsId);
            string username = (string) HttpContext.Current.Cache[cacheGetUserNameFromSessionId];
            if (username == null)
            {
                try
                {
                    using (
                        var sqlConnection =
                            new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
                    {
                        sqlConnection.Open();

                        const string sqlSelect = "select Username FROM Sessions WHERE id=@id";
                        using (var sqlCommand = new SqlCommand(sqlSelect, sqlConnection))
                        {
                            sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = sessionsId;
                            username = (string) sqlCommand.ExecuteScalar();
                        }
                    }
                }
                catch (Exception ee)
                {
                    throw new ApplicationException(ee.ToString());
                }
                HttpContext.Current.Cache.Insert(cacheGetUserNameFromSessionId, username,
                                                 null,
                                                 DateTime.Now.Add(new TimeSpan(0, 0,
                                                                               RetrieveSecondsForSessionCacheTimeout())),
                                                 TimeSpan.Zero);
            }


            return username;
        }

        public static bool GetRemovePrimarySpeakerFromSessionId(int sessionsId)
        {
            string cacheName = String.Format("CacheGetRemovePrimarySpeakerFromSessionId-{0}", sessionsId);
            var removePrimarySpeaker = (bool?) HttpContext.Current.Cache[cacheName];
            if (removePrimarySpeaker == null)
            {
                try
                {
                    using (
                        var sqlConnection =
                            new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
                    {
                        sqlConnection.Open();

                        const string sqlSelect = "select DoNotShowPrimarySpeaker FROM Sessions WHERE id=@id";
                        using (var sqlCommand = new SqlCommand(sqlSelect, sqlConnection))
                        {
                            sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = sessionsId;

                            SqlDataReader reader = sqlCommand.ExecuteReader();
                            while (reader.Read())
                            {
                                removePrimarySpeaker = reader.IsDBNull(0) ? false : reader.GetBoolean(0);
                            }
                        }
                    }
                }
                catch (Exception ee)
                {
                    throw new ApplicationException(ee.ToString());
                }
                HttpContext.Current.Cache.Insert(cacheName, removePrimarySpeaker,
                                                 null,
                                                 DateTime.Now.Add(new TimeSpan(0, 0,
                                                                               RetrieveSecondsForSessionCacheTimeout())),
                                                 TimeSpan.Zero);
            }


            return removePrimarySpeaker ?? false;
        }



        public static string GetUserNameMembershipOfSession(int sessionsId)
        {
            string username = String.Empty;
            try
            {
                using (
                    var sqlConnection =
                        new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
                {
                    sqlConnection.Open();
                    string sqlSelect = "select Username FROM Sessions WHERE id=@id";
                    var sqlCommand = new SqlCommand(sqlSelect, sqlConnection);
                    sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = sessionsId;
                    username = (string) sqlCommand.ExecuteScalar();
                    sqlConnection.Close();
                }
            }
            catch (Exception ee)
            {
                throw new ApplicationException(ee.ToString());
            }

            return username;
        }

        public static bool CheckUserIsPresenter()
        {
            string userName = HttpContext.Current.User.Identity.Name;
            if (Roles.IsUserInRole("presenter"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckUserIsSuperUser()
        {
            string userName = HttpContext.Current.User.Identity.Name;
            if (Roles.IsUserInRole("superuser"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool GetIgnoreAutoSignOnGuid(string userName)
        {
            bool ignoreIt = false;

            if (Roles.IsUserInRole(userName, AdminRoleName))
            {
                ignoreIt = true;
            }

            else if (Roles.IsUserInRole(userName, NoAutoLoginForGUIDRoleName))
            {
                ignoreIt = true;
            }

            return ignoreIt;
        }

        public static string GetAdminRoleName()
        {
            return AdminRoleName;
        }


        public static int GetDefaultPictureIdFromSession(int sessionId)
        {
            int pictureId = -1;
            using (
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();
                SqlDataReader reader = null;
                string sqlSelect =
                    @"SELECT 
                          dbo.SessionPictures.PictureId,
                          dbo.SessionPictures.DefaultPicture
                        FROM
                          dbo.SessionPictures
                        WHERE
                          (dbo.SessionPictures.SessionId = @SessionId)
                        ORDER BY
                          dbo.SessionPictures.DefaultPicture";
                try
                {
                    var command = new SqlCommand(sqlSelect, sqlConnection);
                    command.Parameters.Add("@SessionId", SqlDbType.Int).Value = sessionId;
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        pictureId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                    }
                }
                catch (Exception eee)
                {
                    throw new ApplicationException(eee.ToString());
                }
            }
            return pictureId;
        }

        public static string ConvertEncodedHTMLToRealHTML(string oldHTML)
        {
            var sb = new StringBuilder(oldHTML);
            // Selectively allow <b> and <i> and ...
            //sb.Replace("&lt;p&gt;", "<p>");
            //sb.Replace("&lt;/p&gt;", "</p>");
            //sb.Replace("&lt;b&gt;", "<b>");
            //sb.Replace("&lt;/b&gt;", "</b>");
            //sb.Replace("&lt;i&gt;", "<i>");
            //sb.Replace("&lt;/i&gt;", "</i>");

            //sb.Replace("&lt;u&gt;", "<u>");
            //sb.Replace("&lt;/u&gt;", "</u>");
            //sb.Replace("&lt;br&gt;", "<br>");
            //sb.Replace("&lt;/br&gt;", "</br>");
            //sb.Replace("&lt;br/&gt;", "</br>");
            //sb.Replace("&lt;ul&gt;", "<ul>");
            //sb.Replace("&lt;/ul&gt;", "</ul>");
            //sb.Replace("&lt;li&gt;", "<li>");
            //sb.Replace("&lt;/li&gt;", "</li>");


            return sb.ToString();

            //if (/^(?:[\s\w\?\!\,\.\'\"]*|(?:\<\/?(?:i|b|p|br|em|pre)\>))*$/i) {
            // # Cool, it's valid input!
            //    }
            //http://safari.oreilly.com/0735617228/IDAPO1T#X2ludGVybmFsX1RvYz94bWxpZD0wNzM1NjE3MjI4L0lEQTJIMVQ=

            //string fileName = Context.Server.MapPath("App_Data/RegExpForHTML.txt");
            //StreamReader re = File.OpenText(fileName);
            //string regExp = re.ReadLine();

            ////oldHTML = 

            //string newHTML = Regex.Replace(oldHTML, regExp, "");
            //return newHTML;
        }

        public static int GetNumberPicturesAssigned()
        {
            int num = 0;
            try
            {
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString);
                sqlConnection.Open();

                string sqlSelect = "SELECT COUNT(*) FROM SessionPictures";
                var sqlCommand = new SqlCommand(sqlSelect, sqlConnection);
                num = (int) sqlCommand.ExecuteScalar();

                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            catch (Exception ee)
            {
                throw new ApplicationException(ee.ToString());
            }

            return num;
        }

        public static int GetNumberVolunteerMeetingPlannedAttend()
        {
            int num;
            try
            {
                using (
                    var sqlConnection =
                        new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
                {
                    sqlConnection.Open();
                    const string sqlSelect = "SELECT count(*) FROM  attendees WHERE volunteerMeetingstatus = '4'";
                    using (var sqlCommand = new SqlCommand(sqlSelect, sqlConnection))
                    {
                        num = (int) sqlCommand.ExecuteScalar();
                    }
                }
            }
            catch (Exception ee)
            {
                throw new ApplicationException(ee.ToString());
            }

            return num;
        }

        /// <summary>
        /// shows all non registered users by going 
        /// through past years and making a
        /// unique query.
        /// </summary>
        /// <returns>List of all non-registered 
        /// code campers form previous years</returns>
        public static List<ListItem> LoadAllUnRegisteredCodeCampUsers()
        {
            List<string> codeCampersCurrentYear =
                GetEmailsFromPreviousYear("CodeCampSV06");

            List<string> emailOptOut2008 =
                GetDoNotRemoveList("CodeCampSV06");

            List<string> codeCampers2007 =
                GetEmailsFromPreviousYear("PASTSV07");

            List<string> codeCampers2006 =
                GetEmailsFromPreviousYear("PASTSV06");

            // Make Combined List of 06 and 07 and unique it 
            // (duplicates are removed in Union)
            IEnumerable<string> uniqueNamesQuery =
                codeCampers2006.Union(codeCampers2007).OrderBy(s => s);

            IEnumerable<string> emailListBeforeOptOut =
                uniqueNamesQuery.Except(codeCampersCurrentYear);

            IEnumerable<string> emaiListAfterOptOut =
                emailListBeforeOptOut.Except(emailOptOut2008);

            var finalList = new List<ListItem>();
            foreach (string s in emaiListAfterOptOut)
            {
                finalList.Add(new ListItem(s, s));
            }
            return finalList;
        }

        private static List<string> GetEmailsFromPreviousYear(string connectName)
        {
            var emailList = new List<string>();
            using (
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings[connectName].ConnectionString))
            {
                sqlConnection.Open();
                const string sqlSelect = @"SELECT 
                       Email
                       FROM Attendees";
                try
                {
                    var command = new SqlCommand(sqlSelect, sqlConnection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string email = reader.IsDBNull(0) ? String.Empty : reader.GetString(0);
                        emailList.Add(email);
                    }
                }
                catch (Exception eee)
                {
                    throw new ApplicationException(eee.ToString());
                }
            }
            return emailList;
        }

        public static List<string> GetDoNotRemoveList(string connectName)
        {
            var emailList = new List<string>();
            using (
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings[connectName].ConnectionString))
            {
                sqlConnection.Open();
                const string sqlSelect =
                    @"SELECT  
                       Email
                       FROM EmailOptOut";
                try
                {
                    var command = new SqlCommand(sqlSelect, sqlConnection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string email = reader.IsDBNull(0) ? String.Empty : reader.GetString(0);
                        emailList.Add(email);
                    }
                }
                catch (Exception eee)
                {
                    throw new ApplicationException(eee.ToString());
                }
            }
            return emailList;
        }

        public static string GetSessionNameOfSession(int outInt)
        {
            var sessionsODS = new SessionsODS();
            string ret = String.Empty;
            foreach (SessionsODS.DataObjectSessions session in sessionsODS.GetByPrimaryKeySessions(outInt))
            {
                ret = session.Title;
            }

            return ret;
        }

        public static string GetEmailOfSpeakerFromSession(int sessionIdTemp)
        {
            string username = GetUserNameFromSessionId(sessionIdTemp);
            return GetAttendeeEmailByUsername(username);
        }




        private static string GetAttendeeEmailByUsername(string username)
        {
            var attendeesODS = new AttendeesODS();
            List<AttendeesODS.DataObjectAttendees> listAttendees =
                attendeesODS.GetByUsername(String.Empty, username);
            string retString = String.Empty;

            if (listAttendees.Count > 0)
            {
                retString = listAttendees[0].Email ?? String.Empty;
            }
            return retString;
        }

        public static bool GetSessionInfo(int sessionId, out string firstName, out string lastName,
                                          out string description, out string sessionURL, out string speakerURL,
                                          out string sessionTitle,
                                          out string speakerBio, out string speakerPictureUrl,
                                          out string speakerZipCode, out string speakerPersonalUrl,
                                          out DateTime sessionStartTime)
        {
            bool success = false;
            using (var sqlConnection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();

                firstName = String.Empty;
                lastName = String.Empty;
                description = String.Empty;
                sessionTitle = String.Empty;
                sessionURL = String.Empty;
                speakerURL = String.Empty;
                speakerBio = String.Empty;
                speakerPictureUrl = String.Empty;
                speakerZipCode = String.Empty;
                speakerPersonalUrl = String.Empty;
                sessionStartTime = DateTime.MinValue;
                Guid speakerPKID = Guid.NewGuid();


                try
                {
                    const string sqlSelect =
                        @"
                            SELECT 
                              Attendees.UserFirstName,
                              Attendees.UserLastName,
                              dbo.Sessions.title,
                              dbo.Sessions.description,
                              Attendees.UserZipCode,
                              Attendees.UserBio,
                              Attendees.UserWebsite,
                              Attendees.PKID,
                              SessionTimes.StartTime
                            FROM
                              Attendees
                              INNER JOIN dbo.Sessions ON (Attendees.Username = dbo.Sessions.Username)
                              INNER JOIN dbo.SessionTimes ON (Sessions.SessionTimesId = SessionTimes.id)
                            WHERE
                              Sessions.id = @id";


                    using (var sqlCommand = new SqlCommand(sqlSelect, sqlConnection))
                    {
                        sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = sessionId;

                        var PKID = new Guid();
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            if (reader != null)
                                while (reader.Read())
                                {
                                    firstName = reader.IsDBNull(0) ? String.Empty : reader.GetString(0);
                                    lastName = reader.IsDBNull(1) ? String.Empty : reader.GetString(1);
                                    sessionTitle = reader.IsDBNull(2) ? String.Empty : reader.GetString(2);
                                    description = reader.IsDBNull(3) ? String.Empty : reader.GetString(3);
                                    speakerZipCode = reader.IsDBNull(4) ? String.Empty : reader.GetString(4);
                                    speakerBio = reader.IsDBNull(5) ? String.Empty : reader.GetString(5);
                                    speakerPersonalUrl = reader.IsDBNull(6) ? String.Empty : reader.GetString(6);
                                    PKID = reader.IsDBNull(7) ? Guid.NewGuid() : reader.GetGuid(7);
                                    //speakerZipCode = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                                    //speakerBio = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
                                    //speakerPersonalUrl = reader.IsDBNull(10) ? string.Empty : reader.GetString(10);
                                    //speakerPKID = reader.IsDBNull(11) ? Guid.NewGuid() : reader.GetGuid(11);
                                    sessionStartTime = reader.IsDBNull(8) ? DateTime.MinValue : reader.GetDateTime(8);
                                }
                        }
                        string baseUrl = ConfigurationManager.AppSettings["SVCCBasePage"].ToString();
                        //baseUrl = string.Empty; // use this for debugging, otherwise, likely will fail because pointing at wrong db
                        speakerPictureUrl = String.Format("{0}DisplayImage.ashx?PKID={1}&sizex=100",
                                                          baseUrl,
                                                          speakerPKID);

                        // always have at least one session here.
                        var sODS = new SessionsODS();
                        List<SessionsODS.DataObjectSessions> li = sODS.GetByPKID(PKID);
                        int sessionIdx = li[0].Id;

                        string basePageSVCC = ConfigurationManager.AppSettings["SVCCBasePage"];
                        string sessionURLTemplate = basePageSVCC + "Sessions.aspx?id={0}";
                        string speakerURLTemplate = basePageSVCC + "Speakers.aspx?id={0}";


                        sessionURL = String.Format(sessionURLTemplate, sessionIdx);
                        speakerURL = String.Format(speakerURLTemplate, sessionIdx);
                    }
                    success = true;
                }
                catch (Exception eee1)
                {
                    string str = eee1.ToString();
                    success = false;
                }
            }
            return success;
        }


        public static void ClearAllEventsFromHealthMonitoring(string connectName)
        {
            using (
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings[connectName].ConnectionString))
            {
                sqlConnection.Open();
                const string sqlSelect =
                    @"delete from aspnet_WebEvent_Events";
                //@"select count(*) from aspnet_WebEvent_Events";

                try
                {
                    var command = new SqlCommand(sqlSelect, sqlConnection);
                    command.ExecuteNonQuery();
                }
                catch (Exception eee)
                {
                    throw new ApplicationException(eee.ToString());
                }
            }
            return;
        }

        public static int CountEventsFromHealthMonitoring(string connectName)
        {
            int cntRet = 0;
            using (
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings[connectName].ConnectionString))
            {
                sqlConnection.Open();
                const string sqlSelect =
                    @"select count(*) from aspnet_WebEvent_Events";

                try
                {
                    var command = new SqlCommand(sqlSelect, sqlConnection);
                    cntRet = (int) command.ExecuteScalar();
                }
                catch (Exception eee)
                {
                    throw new ApplicationException(eee.ToString());
                }
            }
            return cntRet;
        }

        public static WS_Detail RetrieveSessionsAllCodeCamp()
        {
            var ws_Detail = new WS_Detail();
            ws_Detail.SessionDetails = RetrieveAllSessions();
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                AttendeesODS.DataObjectAttendees username = GetAttendeeByUsername(HttpContext.Current.User.Identity.Name);
                ws_Detail.loggedInFirstName = username.Userfirstname;
                ws_Detail.loggedInLastName = username.Userlastname;
                ws_Detail.loggedInUsername = HttpContext.Current.User.Identity.Name;
                ws_Detail.loggedInPictureUrl = username.Userfirstname;
                ws_Detail.loggedInPictureUrl = String.Format("{0}DisplayImage.ashx?username={1}",
                                                             ConfigurationManager.AppSettings["SVCCBasePage"].ToString(),
                                                             HttpContext.Current.User.Identity.Name);
            }
            else
            {
                ws_Detail.loggedInUsername = "Not Logged In";
            }
            return ws_Detail;
        }

        public static WS_Detail RetrieveSessionsOneCodeCamp(int sessionId)
        {
            var ws_Detail = new WS_Detail();
            ws_Detail.SessionDetails = RetrieveOneSessions(sessionId);
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                AttendeesODS.DataObjectAttendees username = GetAttendeeByUsername(HttpContext.Current.User.Identity.Name);
                ws_Detail.loggedInFirstName = username.Userfirstname;
                ws_Detail.loggedInLastName = username.Userlastname;
                ws_Detail.loggedInUsername = HttpContext.Current.User.Identity.Name;
                ws_Detail.loggedInPictureUrl = username.Userfirstname;
                ws_Detail.loggedInPictureUrl = String.Format("{0}DisplayImage.ashx?username={1}",
                                                             ConfigurationManager.AppSettings["SVCCBasePage"].ToString(),
                                                             HttpContext.Current.User.Identity.Name);
            }
            else
            {
                ws_Detail.loggedInUsername = "Not Logged In";
            }
            return ws_Detail;
        }

        public static List<WS_SessionDetail> RetrieveAllSessions()
        {
            var sessionsODS = new SessionsODS();
            List<SessionsODS.DataObjectSessions> sessions = sessionsODS.GetAllByStartTime();
            var sessionsComplete = new List<WS_SessionDetail>();
            foreach (SessionsODS.DataObjectSessions session in sessions)
            {
                string firstName = "Lynn";
                string lastName = "Langit";
                string description =
                    "Lynn will talk about SQL Server 2008 Data Mining. In a mostly demo-driven presentation, Lynn will explain the what, why and how of predictive analytics in SSAS. ";
                string sessionURL = "http://www.siliconvalley-codecamp.com/Sessions.aspx?id=11";
                string speakerURL = "http://www.siliconvalley-codecamp.com/Speakers.aspx?id=11";
                string sessionTitle = "Data Mining for .NET Developers";
                string speakerBio = "My Bio";
                string speakerPictureUrl = "http://peterkellner.net/mypict.jpg";
                string speakerZipCode = "95117";
                string speakerPersonalUrl = "http://peterkellner.net";
                DateTime sessionStartTime = DateTime.MinValue;

                bool success = GetSessionInfo(session.Id, out firstName,
                                              out lastName, out description, out sessionURL,
                                              out speakerURL, out sessionTitle,
                                              out speakerBio, out speakerPictureUrl, out speakerZipCode,
                                              out speakerPersonalUrl, out sessionStartTime);

                var sessionTags = new List<string>();
                var tagsODS = new TagsODS();
                foreach (TagsODS.DataObjectTags tags in tagsODS.GetAllBySession(String.Empty, session.Id))
                {
                    sessionTags.Add(tags.Tagname.Trim());
                }
                Comparison<string> comparison = (lhs, rhs) => rhs.ToLower().CompareTo(lhs.ToLower());
                sessionTags.Sort(new Comparison<string>(comparison));


                sessionsComplete.Add(new WS_SessionDetail(session, firstName, lastName,
                                                          sessionURL, speakerURL, speakerBio, speakerPictureUrl,
                                                          speakerZipCode,
                                                          speakerPersonalUrl,
                                                          sessionStartTime,
                                                          sessionTags));
            }


            return sessionsComplete;
        }


        public static List<WS_SessionDetail> RetrieveOneSessions(int sessionId)
        {
            var sessionsODS = new SessionsODS();
            List<SessionsODS.DataObjectSessions> sessions = sessionsODS.GetAllSessionsBySessionId(sessionId);
            var sessionsComplete = new List<WS_SessionDetail>();
            foreach (SessionsODS.DataObjectSessions session in sessions)
            {
                string firstName = "Lynn";
                string lastName = "Langit";
                string description =
                    "Lynn will talk about SQL Server 2008 Data Mining. In a mostly demo-driven presentation, Lynn will explain the what, why and how of predictive analytics in SSAS. ";
                string sessionURL = "http://www.siliconvalley-codecamp.com/Sessions.aspx?id=11";
                string speakerURL = "http://www.siliconvalley-codecamp.com/Speakers.aspx?id=11";
                string sessionTitle = "Data Mining for .NET Developers";
                string speakerBio = "My Bio";
                string speakerPictureUrl = "http://peterkellner.net/mypict.jpg";
                string speakerZipCode = "95117";
                string speakerPersonalUrl = "http://peterkellner.net";
                DateTime sessionStartTime = DateTime.MinValue;

                bool success = GetSessionInfo(session.Id, out firstName,
                                              out lastName, out description, out sessionURL,
                                              out speakerURL, out sessionTitle,
                                              out speakerBio, out speakerPictureUrl, out speakerZipCode,
                                              out speakerPersonalUrl, out sessionStartTime);

                var sessionTags = new List<string>();
                var tagsODS = new TagsODS();
                foreach (TagsODS.DataObjectTags tags in tagsODS.GetAllBySession(String.Empty, session.Id))
                {
                    sessionTags.Add(tags.Tagname.Trim());
                }
                Comparison<string> comparison = (lhs, rhs) => rhs.ToLower().CompareTo(lhs.ToLower());
                sessionTags.Sort(new Comparison<string>(comparison));


                sessionsComplete.Add(new WS_SessionDetail(session, firstName, lastName,
                                                          sessionURL, speakerURL, speakerBio, speakerPictureUrl,
                                                          speakerZipCode,
                                                          speakerPersonalUrl,
                                                          sessionStartTime,
                                                          sessionTags));
            }


            return sessionsComplete;
        }

        public static List<WS_LatLon> GetAllLatLonAddPaulKing()
        {
            List<WS_LatLon> latLonList = GetAllLatLon();
            latLonList.Add(new WS_LatLon(27.54929, 152.84793, true, "Paul", "King",
                                         "http://www.siliconvalley-codecamp.com/DisplayImage.ashx?sizex=100&PKID=46fce967-c7f4-4316-9b32-ce79d83267ab"));
            return latLonList;
        }

        public static List<WS_LatLon> GetAllLatLon()
        {
            var speakerUsernames = new List<string>();
            var listLatLon = new List<WS_LatLon>();

            try
            {
                using (var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
                {
                    sqlConnection.Open();
                    const string sqlSelect =
                        @"
                        SELECT DISTINCT
                          dbo.Attendees.Username
                        FROM
                          dbo.Attendees
                          INNER JOIN dbo.Sessions ON (dbo.Attendees.Username = dbo.Sessions.Username)
                        ";
                    using (var sqlCommand = new SqlCommand(sqlSelect, sqlConnection))
                    {
                        using (SqlDataReader reader1 = sqlCommand.ExecuteReader())
                        {
                            while (reader1.Read())
                            {
                                speakerUsernames.Add(reader1.IsDBNull(0) ? String.Empty : reader1.GetString(0));
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                throw new ApplicationException(ee.ToString());
            }


            try
            {
                using (var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
                {
                    //DisplayImage.ashx?sizex=100&amp;PKID=dfa6360e-8cb5-4c82-9572-1bd22444b25b
                    string pictureFormat = ConfigurationManager.AppSettings["SVCCBasePage"] +
                                           "DisplayImage.ashx?sizex={0}&PKID={1}";


                    sqlConnection.Open();
                    const string sqlSelect =
                        @"
                        SELECT 
                          dbo.ZIPCODEWORLDGOLD.LATITUDE,
                          dbo.ZIPCODEWORLDGOLD.LONGITUDE,
                          dbo.Attendees.Username,
                          dbo.Attendees.UserFirstName,
                          dbo.Attendees.UserLastName,
                          dbo.Attendees.PKID,
                          dbo.Attendees.UserZipCode
                        FROM
                          dbo.Attendees
                          INNER JOIN dbo.ZIPCODEWORLDGOLD ON (dbo.Attendees.UserZipCode = dbo.ZIPCODEWORLDGOLD.ZIP_CODE)
                        ORDER BY
                          dbo.Attendees.UserZipCode
                        ";
                    using (var sqlCommand = new SqlCommand(sqlSelect, sqlConnection))
                    {
                        using (SqlDataReader reader1 = sqlCommand.ExecuteReader())
                        {
                            while (reader1.Read())
                            {
                                double lattitude = Convert.ToDouble(reader1.IsDBNull(0) ? "0.0" : reader1.GetString(0));
                                double longitude = Convert.ToDouble(reader1.IsDBNull(1) ? "0.0" : reader1.GetString(1));
                                string username = reader1.IsDBNull(2) ? String.Empty : reader1.GetString(2);
                                string firstName = reader1.IsDBNull(3) ? String.Empty : reader1.GetString(3);
                                string lastName = reader1.IsDBNull(4) ? String.Empty : reader1.GetString(4);
                                Guid PKID = reader1.IsDBNull(5) ? Guid.NewGuid() : reader1.GetGuid(5);


                                bool isSpeaker = speakerUsernames.Contains(username) ? true : false;
                                listLatLon.Add(
                                    new WS_LatLon(lattitude, longitude, isSpeaker,
                                                  isSpeaker ? firstName : String.Empty,
                                                  isSpeaker ? lastName : String.Empty,
                                                  isSpeaker ? String.Format(pictureFormat, 100, PKID) : String.Empty
                                        ));
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                throw new ApplicationException(ee.ToString());
            }
            return listLatLon;
        }

        public static int RetrieveSecondsForSessionCacheTimeout()
        {
            int timeout = 120;
            if (ConfigurationManager.AppSettings[STR_SessionCacheTimeoutSeconds] != null)
            {
                timeout = Convert.ToInt32(ConfigurationManager.AppSettings[STR_SessionCacheTimeoutSeconds]);
            }
            return timeout;
        }

        public static bool ShowWetPaintWiki(int sessionId)
        {
            bool showWetPaintWiki = IsShowWetPaintWiki(sessionId);
            return showWetPaintWiki;
        }

        public static bool ShowPBWiki(int sessionId)
        {
            bool showWetPaintWiki = IsShowWetPaintWiki(sessionId);
            return !showWetPaintWiki;
        }

        private static bool IsShowWetPaintWiki(int sessionId)
        {
            return false; // not supporting wetpaint anymore
            //            bool showWetPaintWiki = false;
            //            string selectString =
            //                @"
            //                    SELECT 
            //                      dbo.Sessions.Username
            //                    FROM
            //                      dbo.Sessions           
            //                    WHERE
            //                      dbo.Sessions.Username IN (select username from Sessions where id=@id)
            //                    ";

            //            using (var sqlConnection =
            //                new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            //            {
            //                sqlConnection.Open();
            //                var command =
            //                    new SqlCommand(
            //                        selectString,
            //                        sqlConnection);
            //                command.Parameters.Add("@id", SqlDbType.Int).Value = sessionId;

            //                string username = string.Empty;

            //                using (SqlDataReader reader = command.ExecuteReader())
            //                {
            //                    try
            //                    {
            //                        if (reader != null)
            //                        {
            //                            while (reader.Read())
            //                            {
            //                                username = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
            //                            }
            //                        }
            //                    }
            //                    catch (Exception eee1)
            //                    {
            //                        throw new ApplicationException(eee1.ToString());
            //                    }
            //                }

            //                var attendeeODS = new AttendeesODS();
            //                List<AttendeesODS.DataObjectAttendees> usernameList = attendeeODS.GetByUsername(string.Empty, username);
            //                showWetPaintWiki = usernameList[0].Vistaslotsid == 1 ? true : false;
            //            }
            //            return showWetPaintWiki;
        }

        public static void UpdateAttendDinner(bool attendDinner, string userName)
        {
            using (var sqlConnection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();

                const string sqlSelect =
                    @"UPDATE Attendees SET SaturdayDinner = @SaturdayDinner WHERE Username = @Username";
                using (var sqlCommand = new SqlCommand(sqlSelect, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@SaturdayDinner", SqlDbType.Bit).Value = attendDinner;
                    sqlCommand.Parameters.Add("@Username", SqlDbType.VarChar).Value = userName;
                    try
                    {
                        sqlCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException(ex.ToString());
                    }
                }
            }
        }

        public static int RetrieveAttendeeActivityLastDays(int numDays)
        {
            string sqlSelectTempldate =
                @"
                SELECT 
                  count(*)
                FROM
                  dbo.Attendees
                WHERE
                dbo.Attendees.LastActivityDate > GetDate() - @daysback
                AND
                dbo.Attendees.Id IN (SELECT AttendeesId FROM AttendeesCodeCampYear WHERE CodeCampYearId = {0})
               ";
            string sqlSelect = String.Format(sqlSelectTempldate, GetCurrentCodeCampYear());


            int retCnt = 0;
            using (var sqlConnection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand(sqlSelect, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@daysback", SqlDbType.Int).Value = numDays;
                    try
                    {
                        retCnt = (int) sqlCommand.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException(ex.ToString());
                    }
                }
            }
            return retCnt;
        }


        public static int RetrieveSaturdayDinnerCount()
        {
            const string sqlSelectTemplate =
                @"
                SELECT 
                  count(*)
                FROM
                  dbo.Attendees
                WHERE
                dbo.Attendees.SaturdayDinner = 1 AND
                Attendees.Id IN (SELECT AttendeesId FROM AttendeesCodeCampYear WHERE CodeCampYearId = {0})
               ";

            string sqlSelect = String.Format(sqlSelectTemplate, GetCurrentCodeCampYear());

            int retCnt = 0;
            using (var sqlConnection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand(sqlSelect, sqlConnection))
                {
                    try
                    {
                        retCnt = (int) sqlCommand.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException(ex.ToString());
                    }
                }
            }
            return retCnt;
        }

        /// <summary>
        /// "2012" -> 7
        /// </summary>
        /// <param name="codeCampYearIn"></param>
        /// <returns></returns>
        public static int ConvertCodeCampYearToCodeCampYearId(string codeCampYearIn)
        {
            int codeCampYear;
            Int32.TryParse(codeCampYearIn, out codeCampYear);
            int codeCampYearId = codeCampYear - 2005;
            return codeCampYearId;

        }

        public static string ConvertCodeCampYearToActualYear(string codeCampYear)
        {
            int codeCampYearId;
            Int32.TryParse(codeCampYear, out codeCampYearId);
            int codeCampYearNumber = 2005 + codeCampYearId;
            string retString = codeCampYearNumber.ToString(CultureInfo.InvariantCulture);

            //if (codeCampYear.Equals("3"))
            //{
            //    retString = "2008";
            //}
            //else if (codeCampYear.Equals("4"))
            //{
            //    retString = "2009";
            //}
            //else if (codeCampYear.Equals("5"))
            //{
            //    retString = "2010";
            //}
            //else if (codeCampYear.Equals("6"))
            //{
            //    retString = "2011";
            //}
            //else if (codeCampYear.Equals("7"))
            //{
            //    retString = "2012";
            //}
            //else
            //{
            //    throw new ApplicationException("CodeCampYear Not Found");
            //}
            return retString;
        }

        public static int GetAttendeesIdFromUsername(string userName)
        {
            var ccdc = new CodeCampDataContext();
            int attendeesId = (from data in ccdc.Attendees
                               where data.Username.Equals(userName)
                               select data.Id).FirstOrDefault();
            return attendeesId;
        }

        public static string GetCodeCampCurrentString(string saturdayOrSunday, int codeCampYearId)
        {
            var ccdc = new CodeCampDataContext();
            string resultString = String.Empty;
            if (saturdayOrSunday.ToLower().Equals("saturday"))
            {
                resultString = (from data in ccdc.CodeCampYear
                                where data.Id == codeCampYearId
                                select data.CodeCampSaturdayString).FirstOrDefault();
            }
            else if (saturdayOrSunday.ToLower().Equals("sunday"))
            {
                resultString = (from data in ccdc.CodeCampYear
                                where data.Id == codeCampYearId
                                select data.CodeCampSundayString).FirstOrDefault();
            }
            return resultString;
        }



        /// <summary>
        /// Update the table AttendeesCodeCampYear for current year of code camp
        /// </summary>
        /// <param name="currentCodeCampYear"></param>
        /// <param name="userName"></param>
        /// <param name="attendSaturday"></param>
        /// <param name="attendSunday"></param>
        public static bool UpdateAttendeeAttendance(int currentCodeCampYear, string userName, bool attendSaturday,
                                                    bool attendSunday)
        {
            AttendeesCodeCampYearResult attendeesCodeCampYearResult = new AttendeesCodeCampYearResult()
                {
                    CodeCampYearId = currentCodeCampYear,
                    AttendeesId =
                        GetAttendeesIdFromUsername(
                            userName),
                    AttendSaturday = attendSaturday,
                    AttendSunday = attendSunday
                };
            ManagerBase
                <AttendeesCodeCampYearManager, AttendeesCodeCampYearResult, AttendeesCodeCampYear, CodeCampDataContext>.
                I.Upsert(attendeesCodeCampYearResult);
            return true;

        }

        public static bool GetWhetherAttendingCurrentCodeCamp(string saturdayOrSunday, string userName)
        {
            AttendeesCodeCampYear attendeesCodeCampYear;
            using (var ccdc = new CodeCampDataContext())
            {
                attendeesCodeCampYear = (from data in ccdc.AttendeesCodeCampYear
                                         where
                                             data.CodeCampYearId == CurrentCodeCampYear &&
                                             data.AttendeesId == GetAttendeesIdFromUsername(userName)
                                         select data).FirstOrDefault();
            }

            bool retVal = false;
            if (attendeesCodeCampYear != null)
            {
                if (saturdayOrSunday.ToLower().Equals("saturday"))
                {
                    retVal = attendeesCodeCampYear.AttendSaturday;
                }
                else if (saturdayOrSunday.ToLower().Equals("sunday"))
                {
                    retVal = attendeesCodeCampYear.AttendSunday;
                }
            }
            return retVal;
        }

        public static string GetUsernameFromEmail(string email)
        {
            string retUsername = String.Empty;
            if (!String.IsNullOrEmpty(email))
            {
                var attendee =
                    (ManagerBase<AttendeesManager, AttendeesResult, Attendees, CodeCampDataContext>.I.
                                                                                                    GetJustBaseTableColumns
                        (new AttendeesQuery()
                            {
                                Email = email
                            })).FirstOrDefault();
                if (attendee != null)
                {
                    retUsername = attendee.Username;
                }
            }
            return retUsername;
        }

        public static bool IsRegisteredForCurrentCodeCampYear(string name, int codeCampYearId)
        {
            int attendeesId = GetAttendeesIdFromUsername(name);
            var attendeesCodeCampYear =
                (ManagerBase
                    <AttendeesCodeCampYearManager, AttendeesCodeCampYearResult, AttendeesCodeCampYear,
                        CodeCampDataContext>.I.GetJustBaseTableColumns(new AttendeesCodeCampYearQuery()
                            {
                                CodeCampYearId = codeCampYearId,
                                AttendeesId = attendeesId
                            })).FirstOrDefault();
            return attendeesCodeCampYear != null;
        }

        public static bool CheckUserCanViewTrack()
        {
            return Roles.IsUserInRole("admin") || Roles.IsUserInRole("trackadmin") || Roles.IsUserInRole("trackview");
        }


        /// <summary>
        /// Returns a nicely formatted list of presenters for a give session, with the first one being the primary, followed
        /// by next ones in alpha order.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public static string GetHtmlForAllPresentersBySession(int sessionId)
        {
            var speakers = new List<Attendees>();

            string primarySpeakerUsername = GetUserNameFromSessionId(sessionId);
            return null;
        }

        /// <summary>
        /// this returns a nicely formatted string
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="speakerUserName"></param>
        /// <returns></returns>
        public static string GetSecondaryPresentersBySessionIdString(int sessionId, string speakerUserName)
        {
            var attendees = GetSecondaryPresentersBySessionId(sessionId, speakerUserName);
            var sb = new StringBuilder();
            foreach (Attendees attendee in attendees)
            {
                sb.Append(attendee.UserFirstName);
                sb.Append(" ");
                sb.Append(attendee.UserLastName);
                sb.Append(";");
            }
            return sb.ToString().TrimEnd(';');
        }

        public static List<Attendees> GetSecondaryPresentersBySessionId(int sessionId, string speakerUserName)
        {
            return GetSecondaryPresentersBySessionId(sessionId, speakerUserName, false);
        }

        /// <summary>
        /// if justPrimarySpeaker is true, then ignore speakerUserName and just get first record
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="speakerUserName"></param>
        /// <param name="justPrimarySpeaker"></param>
        /// <returns></returns>
        public static List<Attendees> GetSecondaryPresentersBySessionId(int sessionId, string speakerUserName,
                                                                        bool justPrimarySpeaker)
        {
            string cacheSessionsTrackSessionsResults =
                String.Format("CacheGetSecondaryPresentersBySessionId-{0}-{1}-{2}", sessionId, speakerUserName,
                              justPrimarySpeaker);
            var attendeesList = (List<Attendees>) HttpContext.Current.Cache[cacheSessionsTrackSessionsResults];
            if (attendeesList == null)
            {
                attendeesList = new List<Attendees>();
                int primarySpeakerAttendeeId = GetAttendeesIdFromUsername(speakerUserName);
                using (var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
                {
                    sqlConnection.Open();

                    string comparisonOperator = "<>";
                    if (justPrimarySpeaker)
                    {
                        comparisonOperator = "=";
                    }

                    string sqlSelect =
                        String.Format(
                            @"
                            SELECT userfirstname as FirstName,
                                   userlastname as LastName,
                                   id as AttendeeId,
                                   SaturdayClasses,
                                   SundayClasses,
                                   email as Email
                            FROM attendees
                            WHERE id IN (
                                          SELECT attendeeId
                                          FROM sessionpresenter
                                          WHERE SessionId = @SessionId AND
                                            AttendeeId {0} @NotAttendeeId
                                  )
                            ",
                            comparisonOperator);

                    var command = new SqlCommand(sqlSelect, sqlConnection);
                    command.Parameters.Add("@SessionId", SqlDbType.Int).Value = sessionId;
                    command.Parameters.Add("@NotAttendeeId", SqlDbType.Int).Value = primarySpeakerAttendeeId;


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        try
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    var attendees =
                                        new Attendees
                                            {
                                                UserFirstName = reader.IsDBNull(0) ? String.Empty : reader.GetString(0),
                                                UserLastName = reader.IsDBNull(1) ? String.Empty : reader.GetString(1),
                                                Id = reader.GetInt32(2),
                                                SaturdayClasses = !reader.IsDBNull(3) && reader.GetBoolean(3),
                                                SundayClasses = !reader.IsDBNull(4) && reader.GetBoolean(4),
                                                Email = reader.IsDBNull(5) ? String.Empty : reader.GetString(5)
                                            };
                                    attendeesList.Add(attendees);
                                }
                            }
                        }
                        catch (Exception eee12)
                        {
                            throw new ApplicationException(eee12.ToString());
                        }
                    }
                }
                // insert into cache
                HttpContext.Current.Cache.Insert(cacheSessionsTrackSessionsResults, attendeesList,
                                                 null,
                                                 DateTime.Now.Add(new TimeSpan(0, 0,
                                                                               RetrieveSecondsForSessionCacheTimeout())),
                                                 TimeSpan.Zero);
            }
            return attendeesList;
        }




        /// <summary>
        /// add the presenter to the session (assume presenter is not there already)
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="attendeeId"></param>
        public static void AddPresenterToSession(int sessionId, int attendeeId)
        {

            using (
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();

                const string sqlInsertSessionPresenter =
                    @"INSERT INTO  SessionPresenter (AttendeeId,SessionId) VALUES (@AttendeeId,@SessionId)";
                var sqlCommandInsertSessionPresenter = new SqlCommand(sqlInsertSessionPresenter, sqlConnection);
                sqlCommandInsertSessionPresenter.Parameters.Add("AttendeeId", SqlDbType.Int).Value = attendeeId;
                sqlCommandInsertSessionPresenter.Parameters.Add("SessionId", SqlDbType.Int).Value = sessionId;
                sqlCommandInsertSessionPresenter.ExecuteScalar();
            }
        }

        /// <summary>
        /// Remove a presenter from a session
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="attendeeId"></param>
        public static void RemovePresenterToSession(int sessionId, int attendeeId)
        {

            using (
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();

                const string sqlInsertSessionPresenter =
                    @"DELETE FROM  SessionPresenter WHERE  AttendeeId = @AttendeeId AND SessionId = @SessionId";
                var sqlCommandInsertSessionPresenter = new SqlCommand(sqlInsertSessionPresenter, sqlConnection);
                sqlCommandInsertSessionPresenter.Parameters.Add("AttendeeId", SqlDbType.Int).Value = attendeeId;
                sqlCommandInsertSessionPresenter.Parameters.Add("SessionId", SqlDbType.Int).Value = sessionId;
                sqlCommandInsertSessionPresenter.ExecuteScalar();
            }
        }

        public static bool CheckAttendeeUsernameIsSpeaker(string name)
        {
            return CheckAttendeeIdIsSpeaker(GetAttendeesIdFromUsername(name));
        }

        /// <summary>
        /// Verify that attendeeId is a speaker from some year
        /// </summary>
        /// <param name="attendeeId"></param>
        /// <returns></returns>
        public static bool CheckAttendeeIdIsSpeaker(int attendeeId)
        {
            bool foundOne = false;
            using (var sqlConnection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();

                const string sqlSelect =
                    @"
                        SELECT count(*)
                        FROM SessionPresenter
                             INNER JOIN dbo.Sessions ON (SessionPresenter.SessionId = dbo.Sessions.Id)
                        WHERE AttendeeId = @AttendeeId AND
                              CodeCampYearId = @CodeCampYearId
                    ";

                using (var command = new SqlCommand(sqlSelect, sqlConnection))
                {
                    command.Parameters.Add("@AttendeeId", SqlDbType.Int).Value = attendeeId;
                    command.Parameters.Add("@CodeCampYearId", SqlDbType.Int).Value = GetCurrentCodeCampYear();
                    try
                    {
                        var retCnt = (int) command.ExecuteScalar();
                        if (retCnt > 0)
                        {
                            foundOne = true;
                        }
                    }
                    catch (Exception eee12)
                    {
                        throw new ApplicationException(eee12.ToString());
                    }
                }
            }
            return foundOne;
        }

        public static List<CodeCampYearResult> GetListCodeCampYear()
        {
            const string defaultNoColumnMasterCacheCodeCampYearAllCache = "UtilsCacheCodeCampYearAll";
            var listCodeCampYear =
                (List<CodeCampYearResult>) HttpContext.Current.Cache[defaultNoColumnMasterCacheCodeCampYearAllCache];
            if (listCodeCampYear == null)
            {
                listCodeCampYear =
                    ManagerBase<CodeCampYearManager, CodeCampYearResult, CodeCampYear, CodeCampDataContext>.I.GetAll().
                                                                                                            OrderBy(
                                                                                                                a =>
                                                                                                                a.Id)
                                                                                                           .ToList();

                HttpContext.Current.Cache.Insert(defaultNoColumnMasterCacheCodeCampYearAllCache, listCodeCampYear,
                                                 null,
                                                 DateTime.Now.Add(new TimeSpan(0, 0,
                                                                               RetrieveSecondsForSessionCacheTimeout
                                                                                   ())),
                                                 TimeSpan.Zero);
            }
            return listCodeCampYear;
        }

        public static DateTime GetCurrentCodeCampYearStartDate()
        {
            var listCodeCampYear = GetListCodeCampYear();
            int currentCodeCampYear = GetCurrentCodeCampYear();

            DateTime startDate = (from data in listCodeCampYear
                                  where data.Id == currentCodeCampYear
                                  select data.CampStartDate).FirstOrDefault();

            return startDate;

            //            DateTime campStartDate = DateTime.Now;
            //            using (var sqlConnection =
            //                 new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            //            {
            //                sqlConnection.Open();
            //                var command =
            //                    new SqlCommand(
            //                        string.Format(
            //                            @"SELECT CampStartDate 
            //                          FROM CodeCampYear 
            //                          WHERE id = {0}",
            //                            GetCurrentCodeCampYear()),
            //                        sqlConnection);

            //                using (SqlDataReader reader = command.ExecuteReader())
            //                {
            //                    try
            //                    {
            //                        if (reader != null)
            //                        {
            //                            while (reader.Read())
            //                            {
            //                                campStartDate = reader.GetDateTime(0);
            //                            }
            //                        }
            //                    }
            //                    catch (Exception eee1)
            //                    {
            //                        throw new ApplicationException(eee1.ToString());
            //                    }
            //                }
            //            }
            //            return campStartDate;
        }

        /// <summary>
        /// return number of sessions a user can add
        /// </summary>
        /// <returns></returns>
        public static int CheckUserIsSessionRestrictedOrAdmin()
        {
            int numberSessionsCanAdd = 1;
            if (Roles.IsUserInRole(AdminRoleName))
            {
                numberSessionsCanAdd = 99;
            }
            else if (Roles.IsUserInRole(AddThreeSessionsRoleName))
            {
                numberSessionsCanAdd = 3;
            }
            else if (Roles.IsUserInRole(AddFourSessionsRoleName))
            {
                numberSessionsCanAdd = 4;
            }
            else if (Roles.IsUserInRole(AddMoreThanTwoSessionsRoleName))
            {
                numberSessionsCanAdd = 4;
            }
            else if (Roles.IsUserInRole(AddTwoSessionsRoleName))
            {
                numberSessionsCanAdd = 2;
            }
            return numberSessionsCanAdd;
        }



        public static int GetNumberSessionsThisYearForCurrentLoggedInUser()
        {
            int currentYear = GetCurrentCodeCampYear();
            int attendeeId = GetAttendeesIdFromUsername(HttpContext.Current.User.Identity.Name);

            int cnt = SessionPresenterManager.I.Get(new SessionPresenterQuery()
                {
                    CodeCampYearId = currentYear,
                    AttendeeId = attendeeId
                }).Count;



            //int cnt =
            //    (ManagerBase<SessionsManager, SessionsResult, Sessions, CodeCampDataContext>.I.Get(new SessionsQuery()
            //                                                                                           {
            //                                                                                               Attendeesid =
            //                                                                                                   attendeeId,
            //                                                                                               CodeCampYearId
            //                                                                                                   =
            //                                                                                                   currentYear
            //                                                                                           })).Count;
            return cnt;
        }

        public static bool GetDictionariesOfAllowEmailFromSpeaker(int codeCampYearId,
                                                                  out Dictionary<int, int>
                                                                      emailFromSpeakerInterestedDict,
                                                                  out Dictionary<int, int>
                                                                      emailFromSpeakerPlanToAttendDict)
        {
            const string sqlTemplate = @"
                    SELECT Sessions_id,
                           count(*)
                    FROM SessionAttendee
                    WHERE Sessions_id IN (
                                           SELECT Id
                                           FROM Sessions
                                           WHERE codecampyearid = {0}
                          ) AND
                          Attendees_username IN (
                                                  SELECT PKID
                                                  FROM Attendees
                                                  WHERE {1} = 1
                          )
                    GROUP BY Sessions_id";

            string sqlSelectInterested = String.Format(sqlTemplate, GetCurrentCodeCampYear(),
                                                       "AllowEmailToSpeakerInterested");
            string sqlSelectPlanToAttend = String.Format(sqlTemplate, GetCurrentCodeCampYear(),
                                                         "AllowEmailToSpeakerPlanToAttend");

            emailFromSpeakerPlanToAttendDict = new Dictionary<int, int>();
            emailFromSpeakerInterestedDict = new Dictionary<int, int>();



            using (var sqlConnection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();

                using (var sqlCommand = new SqlCommand(sqlSelectInterested, sqlConnection))
                {
                    using (var reader1 = sqlCommand.ExecuteReader())
                    {
                        if (reader1 != null)
                        {
                            while (reader1.Read())
                            {
                                var sessionId = reader1.IsDBNull(0) ? -1 : reader1.GetInt32(0);
                                var count = reader1.IsDBNull(1) ? -1 : reader1.GetInt32(1);
                                if (sessionId >= 0 && !emailFromSpeakerInterestedDict.ContainsKey(sessionId))
                                {
                                    emailFromSpeakerInterestedDict.Add(sessionId, count);
                                }
                            }
                        }
                    }
                }

                using (var sqlCommand = new SqlCommand(sqlSelectPlanToAttend, sqlConnection))
                {
                    using (var reader1 = sqlCommand.ExecuteReader())
                    {
                        if (reader1 != null)
                        {
                            while (reader1.Read())
                            {
                                var sessionId = reader1.IsDBNull(0) ? -1 : reader1.GetInt32(0);
                                var count = reader1.IsDBNull(1) ? -1 : reader1.GetInt32(1);
                                if (sessionId >= 0 && !emailFromSpeakerPlanToAttendDict.ContainsKey(sessionId))
                                {
                                    emailFromSpeakerPlanToAttendDict.Add(sessionId, count);
                                }
                            }
                        }
                    }
                }
            }
            // could put this in try catch and return false if error, or just let it throw like it does now and never getting to return below
            return true;
        }

        public static string GetSessionsJustWithVideoIdList()
        {
            var sessionIds =
                ManagerBase<SessionVideoManager, SessionVideoResult, SessionVideo, CodeCampDataContext>.I.Get(
                    new SessionVideoQuery()).Select(a => a.SessionId).ToList();
            var stringBuilder = GetStringIdListFromListInts(sessionIds);
            return stringBuilder.ToString();
        }

        /// <summary>
        /// return a list of comma separated sessionIds for saturday or sunday of current cc
        /// </summary>
        /// <returns></returns>
        public static string GetSessionsIdList(string saturdayOrSunday, string timeSlot)
        {
            var retSessionListIds = new List<int>();
            var sessionsByTitle = (new SessionsODS()).GetBySessionTitle();
            var sessionTimesList = (new SessionTimesODS()).GetAllSessionTimes();
            if (saturdayOrSunday.ToLower().Equals("all") && String.IsNullOrEmpty(timeSlot))
            {
                retSessionListIds = sessionsByTitle.Select(a => a.Id).ToList();
            }
            else
            {
                bool showAllDay = false;
                DateTime timeSlotDateTime = GetCurrentCodeCampYearStartDate();
                // get list of sessions times by sessiond
                if (!String.IsNullOrEmpty(timeSlot))
                {
                    timeSlotDateTime = Convert.ToDateTime(timeSlot);

                    if (timeSlotDateTime.Hour == 0 && timeSlotDateTime.Minute == 0 && timeSlotDateTime.Second == 0)
                    {
                        showAllDay = true;
                    }
                }
                else
                {
                    if (saturdayOrSunday.Equals("Saturday"))
                    {
                        timeSlotDateTime = GetCurrentCodeCampYearStartDate();
                    }
                    else if (saturdayOrSunday.Equals("Sunday"))
                    {
                        timeSlotDateTime = GetCurrentCodeCampYearStartDate().AddDays(1);
                    }
                }


                var sessionTimeOfInterestListIds = new List<int>();

                // all years, but small list so no worries and it's cached.
                foreach (var rec in sessionTimesList)
                {
                    var sessionStartDate = new DateTime(rec.Starttime.Year, rec.Starttime.Month, rec.Starttime.Day);
                    if (sessionStartDate.DayOfWeek.Equals(DayOfWeek.Saturday) &&
                        saturdayOrSunday.Equals("Saturday"))
                    {
                        if (showAllDay)
                        {
                            sessionTimeOfInterestListIds.Add(rec.Id);
                        }
                        else
                        {
                            if (rec.Starttime.Equals(timeSlotDateTime))
                            {
                                sessionTimeOfInterestListIds.Add(rec.Id);
                            }
                        }
                    }
                    else if (sessionStartDate.DayOfWeek.Equals(DayOfWeek.Sunday) &&
                             saturdayOrSunday.Equals("Sunday"))
                    {
                        if (showAllDay)
                        {
                            sessionTimeOfInterestListIds.Add(rec.Id);
                        }
                        else
                        {
                            if (rec.Starttime.Equals(timeSlotDateTime))
                            {
                                sessionTimeOfInterestListIds.Add(rec.Id);
                            }
                        }
                    }

                }




                foreach (var session in sessionsByTitle)
                {
                    if (sessionTimeOfInterestListIds.Contains(session.Sessiontimesid))
                    {
                        retSessionListIds.Add(session.Id);
                    }
                }
            }

            var stringBuilder = GetStringIdListFromListInts(retSessionListIds);
            return stringBuilder.ToString();
        }

        private static StringBuilder GetStringIdListFromListInts(List<int> retSessionListIds)
        {
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < retSessionListIds.Count; i++)
            {
                stringBuilder.Append(retSessionListIds[i]);
                if (i != retSessionListIds.Count - 1)
                {
                    stringBuilder.Append(",");
                }
            }
            return stringBuilder;
        }


        /// <summary>
        /// returns a dictionary with JobId is the key and the value is the count of number of jobs in that slot
        /// </summary>
        /// <param name="codeCampYearId"></param>
        /// <returns></returns>
        public static Dictionary<int, int> GetVolunteersNeededCounts(int codeCampYearId)
        {
            var dict = new Dictionary<int, int>();

            const string sqlTemplate =
                @"
                   SELECT dbo.VolunteerJob.Id,
                           Count(*) AS NumberVolunteers
                    FROM dbo.VolunteerJob
                         INNER JOIN dbo.AttendeeVolunteer ON (dbo.VolunteerJob.Id =
                         dbo.AttendeeVolunteer.VolunteerJobId)
                    WHERE dbo.VolunteerJob.CodeCampYearId = {0}
                    GROUP BY dbo.VolunteerJob.Id";

            using (var sqlConnection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();

                using (var sqlCommand = new SqlCommand(String.Format(sqlTemplate, codeCampYearId), sqlConnection))
                {
                    using (var reader1 = sqlCommand.ExecuteReader())
                    {
                        if (reader1 != null)
                        {
                            while (reader1.Read())
                            {

                                int jobId = reader1.IsDBNull(0) ? -1 : reader1.GetInt32(0);
                                int count = reader1.IsDBNull(1) ? -1 : reader1.GetInt32(1);
                                if (!dict.ContainsKey(jobId))
                                {
                                    dict.Add(jobId, count);
                                }
                            }
                        }
                    }
                }

            }
            return dict;
        }




        //public static object OlockGetJobListingDictionary = new Object();


        ///// <summary>
        ///// Get the job listings that are active for this year
        ///// </summary>
        ///// <returns></returns>
        //public static Dictionary<int, SponsorListJobListingResult> GetJobListingDictionary(bool cacheResult)
        //{
        //    // need to make this just active jobs at some point, now it does them all which could be a problem later TODO:
        //    string cacheString = String.Format("GetGetJobListingDictionary-{0}", "XXX");
        //    var o = (Dictionary<int, SponsorListJobListingResult>)HttpContext.Current.Cache[cacheString];
        //    if (o == null)
        //    {
        //        lock (OlockGetSessionJobDictionary)
        //        {
        //            var recs = SponsorListJobListingManager.I.Get(new SponsorListJobListingQuery());
        //            o = new Dictionary<int, SponsorListJobListingResult>();
        //            foreach (var rec in recs)
        //            {
        //                if (!o.ContainsKey(rec.Id))
        //                {
        //                    o.Add(rec.Id, rec);
        //                }
        //            }

        //            if (cacheResult)
        //            {
        //                HttpContext.Current.Cache.Insert(cacheString, o,
        //                                                 null,
        //                                                 DateTime.Now.Add(new TimeSpan(0, 0,
        //                                                                               Utils.
        //                                                                                   RetrieveSecondsForSessionCacheTimeout
        //                                                                                   ())),
        //                                                 TimeSpan.Zero);
        //            }
        //        }
        //    }
        //    return o;
        //}

        /// <summary>
        /// get something to stuff on the right side below the tags.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public static string GetSessionVideoURL(int sessionId)
        {
            string retString = String.Empty;
            if ((ConfigurationManager.AppSettings["ShowVideoOnSessionIfExists"] != null &&
                 ConfigurationManager.AppSettings["ShowVideoOnSessionIfExists"].ToLower().Equals("true")) ||
                CheckUserIsAdmin() || CheckUserIsVideoEditor())
            {


                string cacheString = String.Format("GetSessionVideoURL-{0}", sessionId);
                retString = (string) HttpContext.Current.Cache[cacheString];
                if (retString == null)
                {
                    retString = String.Empty; // force some value so we don't call database everytime to get null
                    var sessionVideoResult =
                        ManagerBase<SessionVideoManager, SessionVideoResult, SessionVideo, CodeCampDataContext>.I.Get(
                            new SessionVideoQuery {SessionId = sessionId}).FirstOrDefault();
                    if (sessionVideoResult != null)
                    {
                        var videoResult =
                            ManagerBase<VideoManager, VideoResult, Video, CodeCampDataContext>.I.Get(new VideoQuery()
                                {
                                    Id =
                                        sessionVideoResult
                                                                                                         .
                                                                                                         VideoId
                                }).
                                                                                               FirstOrDefault();

                        const string urlTemplate =
                            "<a href=\"{0}\" title=\"{1}\" rel=\"prettyPhoto\" class=\"video\"><img src=\"{2}\" /><br />{1}</a>";
                        string staticImageURL = String.Format("DisplayImage.ashx?SessionIdVideo={0}&sizex=80", sessionId);
                        retString = String.Format(urlTemplate, videoResult.YouTubeURL, videoResult.DescriptionText,
                                                  staticImageURL);
                    }
                    HttpContext.Current.Cache.Insert(cacheString, retString,
                                                     null,
                                                     DateTime.Now.Add(new TimeSpan(0, 0,
                                                                                   RetrieveSecondsForSessionCacheTimeout
                                                                                       ())),
                                                     TimeSpan.Zero);
                }
            }
            return retString;
        }




        public static bool CheckAttendeeDoneEval(string username)
        {
            bool foundEval = false;
            const string sqlTemplate =
                @"
                  SELECT COUNT(*)
                    FROM CodeCampEvals
                    WHERE CodeCampYearId = {0} AND
                          AttendeePKID =
                          (
                            SELECT PKID
                            FROM Attendees
                            WHERE Username = '{1}'
                          )
                ";

            using (var sqlConnection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();

                using (
                    var sqlCommand = new SqlCommand(String.Format(sqlTemplate, GetCurrentCodeCampYear(), username),
                                                    sqlConnection))
                {
                    int cnt = 0;
                    object o = sqlCommand.ExecuteScalar();
                    Int32.TryParse(o.ToString(), out cnt);
                    if (cnt > 0)
                    {
                        foundEval = true;
                    }
                }

            }
            return foundEval;
        }

        public static string GetEmailFromUsername(string username)
        {
            string email = String.Empty;
            var attendeesODS = new AttendeesODS();
            AttendeesODS.DataObjectAttendees attendee =
                attendeesODS.GetByUsername(String.Empty, username).FirstOrDefault();
            if (attendee != null)
            {
                email = attendee.Email;
            }
            return email;
        }

        public static bool CheckUserEmailIsBouncing(string username)
        {
            bool emailIsBouncing = false;
            int emailSubscription = 0;
            const string sqlSelect =
                @"
               SELECT EmailSubscription FROM Attendees WHERE Username = @Username";

            using (
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();

                try
                {
                    var command = new SqlCommand(sqlSelect, sqlConnection);
                    command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username;
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        emailSubscription = reader.GetInt32(0);
                    }
                }
                catch (Exception eee)
                {
                    throw new ApplicationException(eee.ToString());
                }
            }
            if (emailSubscription == 2)
            {
                emailIsBouncing = true;
            }
            return emailIsBouncing;
        }

        /// <summary>
        /// checks email status of user
        /// </summary>
        /// <param name="username"></param>
        /// <returns>returns 2 if bouncing, 1 if opted out</returns>
        public static int CheckUserEmailOptInStatus(string username)
        {
            int emailSubscription = -1;
            if (!String.IsNullOrEmpty(username))
            {
                emailSubscription = -1;
                const string sqlSelect =
                    @"SELECT EmailSubscription FROM Attendees WHERE Username = @Username";

                using (
                    var sqlConnection =
                        new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
                {
                    sqlConnection.Open();

                    try
                    {
                        var command = new SqlCommand(sqlSelect, sqlConnection);
                        command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = username;
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            emailSubscription = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                        }
                    }
                    catch (Exception eee)
                    {
                        throw new ApplicationException(eee.ToString());
                    }
                }
            }
            return emailSubscription;
        }

        public static List<JobData> GetLastPostedJobs(int numJobs)
        {
            var jobDatas = new List<JobData>();
            const string sqlSelect =
                @"
               SELECT TOP {0} Id,
                       JobName,
                       JobCompanyName,
                       JobLocation,
                       JobURL,
                       JobBrief,
                       StartRunDate,
                       EndRunDate,
                       HideListing
                FROM dbo.SponsorListJobListing
                WHERE GETDATE() >= StartRunDate AND
                      GETDATE() <= EndRunDate AND
                      HideListing = 1
                ORDER BY StartRunDate DESC";

            using (
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();

                try
                {
                    var command = new SqlCommand(String.Format(sqlSelect, numJobs), sqlConnection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var jobData = new JobData
                            {
                                JobTitle = reader.IsDBNull(1) ? String.Empty : reader.GetString(1),
                                Company = reader.IsDBNull(2) ? String.Empty : reader.GetString(2),
                                JobLocation = reader.IsDBNull(3) ? String.Empty : reader.GetString(3),
                                JobDescription = reader.IsDBNull(5) ? String.Empty : reader.GetString(5),
                                JobDatePosted = reader.IsDBNull(6) ? DateTime.Now : reader.GetDateTime(6)
                            };
                        jobDatas.Add(jobData);
                    }
                }
                catch (Exception eee)
                {
                    throw new ApplicationException(eee.ToString());
                }
            }
            return jobDatas;
        }

        /// <summary>
        /// ripe for injection attach, but should only be run by most trusted user
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<AttendeesShort> GetEmailWithSql(string sql)
        {
            var results = new List<AttendeesShort>();
            using (
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();

                try
                {
                    var command = new SqlCommand(sql, sqlConnection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string email = reader.IsDBNull(0) ? String.Empty : reader.GetString(0);
                        string firstName = reader.IsDBNull(1) ? String.Empty : reader.GetString(1);
                        string lastName = reader.IsDBNull(2) ? String.Empty : reader.GetString(2);

                        if (String.IsNullOrEmpty(email))
                        {
                            email = "";
                        }
                        if (String.IsNullOrEmpty(firstName))
                        {
                            firstName = "";
                        }
                        if (String.IsNullOrEmpty(lastName))
                        {
                            lastName = "";
                        }


                        results.Add(new AttendeesShort()
                            {
                                EmailAddress = email,
                                FirstName = firstName,
                                LastName = lastName
                            });
                    }
                }
                catch (Exception eee)
                {
                    throw new ApplicationException(eee.ToString());
                }
            }

            return results.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ThenBy(a => a.EmailAddress).ToList();
        }


        private static Regex specialCharactersRegex = new Regex(@"[^\w\s]+", RegexOptions.Compiled);
        private static Regex htmlEncodingsRegex = new Regex(@"&#\d{2,3};", RegexOptions.Compiled);

        public static string ClearSpecialCharacters(string str)
        {
            var result = htmlEncodingsRegex.Replace(str, "");
            return specialCharactersRegex.Replace(result, "");
        }


        /// <summary>
        /// sort by clean title without special characters
        /// </summary>
        /// <param name="dataTemplateOdsList"></param>
        /// <returns></returns>
        public static List<SessionsODS.DataObjectSessions> SortSessionsByCleanTitle(
            List<SessionsODS.DataObjectSessions> dataTemplateOdsList)
        {
            List<SessionsODS.DataObjectSessions> dataTemplateOdsListNew =
                dataTemplateOdsList.OrderBy(a => ClearSpecialCharacters(a.Title)).ToList();

            // nima special regular expression code dimaghani

            //using (var file = new StreamWriter(@"g:\tempjunk\sessions.txt"))
            //{
            //    foreach (var rec in dataTemplateOdsList)
            //    {
            //        file.WriteLine(rec.Title);

            //    }
            //}


            //return dataTemplateOdsListNew;
            return dataTemplateOdsList;
        }

        public static object OlockGetSessionListPbWkiShow = new Object();

        public static Dictionary<int, bool> GetSessionListPbWikiShow(int codeCampYearId, bool cacheResult)
        {
            string cacheString = String.Format("GetSessionListPbWikiShow-{0}", codeCampYearId);
            var o = (Dictionary<int, bool>) HttpContext.Current.Cache[cacheString];
            if (o == null)
            {
                lock (OlockGetSessionListPbWkiShow)
                {
                    var sessionResults =
                        ManagerBase<SessionsManager, SessionsResult, Sessions, CodeCampDataContext>.I.Get(new SessionsQuery
                            {
                                CodeCampYearId
                                    =
                                    codeCampYearId
                            });


                    o = new Dictionary<int, bool>();
                    foreach (var rec in sessionResults)
                    {
                        o.Add(rec.Id, String.IsNullOrEmpty(rec.SessionsMaterialUrl));
                    }
                    if (cacheResult)
                    {
                        HttpContext.Current.Cache.Insert(cacheString, o,
                                                         null,
                                                         DateTime.Now.Add(new TimeSpan(0, 0,
                                                                                       RetrieveSecondsForSessionCacheTimeout
                                                                                           ())),
                                                         TimeSpan.Zero);
                    }
                }
            }
            return o;
        }

        public static object OlockGetSessionJobDictionary = new Object();


        /// <summary>
        /// Get the dictionary of that shows the sponsor for every session (only non-zero sponsors that is)
        /// </summary>
        /// <param name="codeCampYearId"></param>
        /// <param name="cacheResult"></param>
        /// <returns></returns>
        public static Dictionary<int, SponsorListResult> GetSessionJobDictionary(int codeCampYearId, bool cacheResult)
        {
            string cacheString = String.Format("GetSessionJobDictionary-{0}", codeCampYearId);
            var o = (Dictionary<int, SponsorListResult>) HttpContext.Current.Cache[cacheString];
            if (o == null)
            {
                lock (OlockGetSessionJobDictionary)
                {
                    var sessionResults =
                        ManagerBase<SessionsManager, SessionsResult, Sessions, CodeCampDataContext>.I.Get(new SessionsQuery
                            {
                                CodeCampYearId
                                    =
                                    codeCampYearId,
                                JustActiveJobListings
                                    =
                                    true
                            });

                    // get them all (not that many)
                    var sponsorListResults =
                        ManagerBase<SponsorListManager, SponsorListResult, SponsorList, CodeCampDataContext>.I.Get(new SponsorListQuery
                                                                                                                       ()
                            {

                            });

                    o = new Dictionary<int, SponsorListResult>();
                    foreach (var rec in sessionResults)
                    {
                        if (rec.SponsorId.HasValue && rec.SponsorId > 0)
                        {
                            if (sponsorListResults.Select(a => a.Id).ToList().Contains(rec.SponsorId.Value))
                            {
                                o.Add(rec.Id,
                                      sponsorListResults.Where(a => a.Id == rec.SponsorId.Value).FirstOrDefault());
                            }
                        }
                    }
                    if (cacheResult)
                    {
                        HttpContext.Current.Cache.Insert(cacheString, o,
                                                         null,
                                                         DateTime.Now.Add(new TimeSpan(0, 0,
                                                                                       RetrieveSecondsForSessionCacheTimeout
                                                                                           ())),
                                                         TimeSpan.Zero);
                    }
                }
            }
            return o;
        }


        public static void LogReferrerUrl(string path, string referrer)
        {
            using (
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();

                const string sqlInsertSessionPresenter =
                    @"INSERT INTO  ReferringUrl (RequestUrl,ReferringUrlName,RequestDate) VALUES (@RequestUrl,@ReferringUrlName,@RequestDate)";
                var sqlCommandInsertSessionPresenter = new SqlCommand(sqlInsertSessionPresenter, sqlConnection);
                sqlCommandInsertSessionPresenter.Parameters.Add("@RequestUrl", SqlDbType.NVarChar).Value = path ??
                                                                                                           String.Empty;
                sqlCommandInsertSessionPresenter.Parameters.Add("@ReferringUrlName", SqlDbType.NVarChar).Value =
                    referrer ?? String.Empty;
                sqlCommandInsertSessionPresenter.Parameters.Add("@RequestDate", SqlDbType.DateTime).Value =
                    DateTime.UtcNow;
                sqlCommandInsertSessionPresenter.ExecuteScalar();
            }
        }

        /// <summary>
        /// makes a list of all ReferringUrlName from ReferringUrl while skipping all records in 
        /// 
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> GetReferrerUrls(bool used, int repeatCntMinimum)
        {
            string templateSql =
                @"SELECT ReferringUrlName,COUNT(*) As Counter
                            FROM ReferringUrl
                            WHERE RequestUrl NOT LIKE ('%DisplayAd.ashx%') AND 
                                       ReferringUrlName {0} IN (SELECT DISTINCT ReferringUrlName FROM ReferringUrlGroup)
                            GROUP BY ReferringUrlName
                            HAVING COUNT(*) >= @RepeatCntMinimum
                            ORDER BY count(*) desc";

            string sql = String.Format(templateSql, used ? "" : "NOT");

            var dict = new Dictionary<string, int>();
            using (var sqlConnection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();
                using (var command = new SqlCommand(sql, sqlConnection))
                {
                    command.Parameters.Add("@RepeatCntMinimum", SqlDbType.Int).Value = repeatCntMinimum;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        try
                        {
                            while (reader.Read())
                            {
                                string referringUrlName = reader.IsDBNull(0) ? String.Empty : reader.GetString(0);
                                int cnt = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);
                                if (!String.IsNullOrEmpty(referringUrlName))
                                {
                                    dict.Add(referringUrlName.Trim(), cnt);
                                }
                            }
                        }
                        catch (Exception eee1)
                        {
                            throw new ApplicationException(eee1.ToString());
                        }
                    }
                }
            }
            return dict;

            //var unusedUrls =
            //    ReferringUrlManager.I.Get(new ReferringUrlQuery() {SkipAllInReferringUrlGroup = true}).Select(
            //        a => a.ReferringUrlName).ToList();
        }

        public static List<ReferringUrlAttendeeInfo> GetAllReferralUrls()
        {
            var dict = GetReferrerUrls(true, 3);

            var recs = new List<ReferringUrlAttendeeInfo>();

            using (var sqlConnection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();
                using (var command = new SqlCommand(@"
                            SELECT dbo.Attendees.UserFirstName,
                                       dbo.Attendees.UserLastName,
                                       dbo.Attendees.UserWebsite,
                                       dbo.ReferringUrlGroup.ReferringUrlName,
                                       dbo.ReferringUrlGroup.ArticleName,
                                       dbo.ReferringUrlGroup.UserGroup
                                FROM dbo.Attendees
                                     INNER JOIN dbo.ReferringUrlGroup ON (dbo.Attendees.Id =
                                     dbo.ReferringUrlGroup.AttendeesId)
                                WHERE dbo.ReferringUrlGroup.Visible = 1
                     ", sqlConnection))
                {
                    //command.Parameters.Add("@RepeatCntMinimum", SqlDbType.Int).Value = repeatCntMinimum;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        try
                        {
                            while (reader.Read())
                            {
                                string firstName = reader.IsDBNull(0) ? String.Empty : reader.GetString(0);
                                string lastName = reader.IsDBNull(1) ? String.Empty : reader.GetString(1);
                                string webSite = reader.IsDBNull(2) ? String.Empty : reader.GetString(2);
                                string referringUrlName = reader.IsDBNull(3) ? String.Empty : reader.GetString(3);
                                string articleName = reader.IsDBNull(4) ? String.Empty : reader.GetString(4);
                                string userGroup = reader.IsDBNull(5) ? String.Empty : reader.GetString(5);
                                int cnt = dict.ContainsKey(referringUrlName) ? dict[referringUrlName] : 0;

                                recs.Add(new ReferringUrlAttendeeInfo()
                                    {
                                        FirstName = firstName,
                                        LastName = lastName,
                                        ArticleName = articleName,
                                        ReferringUrlName = referringUrlName,
                                        UserGroup = userGroup,
                                        ReferralCountAllTime = cnt,
                                        ReferralCountPast30Days = 0,
                                        UserWebSite = webSite
                                    });
                            }
                        }
                        catch (Exception eee1)
                        {
                            throw new ApplicationException(eee1.ToString());
                        }
                    }
                }
            }
            return recs;
        }



        /// <summary>
        /// get all tagids associated with a given attendee for all time
        /// (not by code camp year, this if for comparing to current year)
        /// 
        /// interestLevel = '2' Interested
        /// interestLevel = '3' Plan To Attend
        /// 
        /// pass in "P","I","A" {where A is all}
        /// 
        /// 
        /// </summary>
        /// <param name="attendeesId"></param>
        /// <param name="codeCampYearId"></param>
        /// <returns></returns>
        public static List<int> GetAttendeeSessionsTags(string userName, string interestLevel)
        {
            var tagIds = new List<int>();

            Guid userPKID = new Guid(GetAttendeePKIDByUsername(userName));

            string sql = @"
                         SELECT DISTINCT dbo.Tags.Id
                                                FROM dbo.Sessions
                                                     INNER JOIN SessionAttendee ON (dbo.Sessions.Id =
                                                     SessionAttendee.Sessions_id)
                                                     INNER JOIN dbo.SessionTags ON (dbo.Sessions.Id = dbo.SessionTags.SessionId)
                                                     INNER JOIN dbo.Tags ON (dbo.SessionTags.TagId = dbo.Tags.Id)
                                                WHERE Attendees_username = @AttendeesPKID   
                        ";
            if (interestLevel.Equals("P"))
            {
                sql += " AND SessionAttendee.InterestLevel = '3' ";
            }
            else if (interestLevel.Equals("I"))
            {
                sql += " AND SessionAttendee.InterestLevel = '2' ";
            }


            using (var sqlConnection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();
                using (var command = new SqlCommand(sql, sqlConnection))
                {
                    command.Parameters.Add("@AttendeesPKID", SqlDbType.UniqueIdentifier).Value = userPKID;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        try
                        {
                            while (reader.Read())
                            {
                                int tagId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                                tagIds.Add(tagId);
                            }
                        }
                        catch (Exception eee1)
                        {
                            throw new ApplicationException(eee1.ToString());
                        }
                    }
                }
            }
            return tagIds;
        }

        /// <summary>
        /// Key: Session      Vaue: AttendeeId
        /// Get List of SpeakerIds (really attendeesId's) where a speaker has multiple time slots
        /// Returning as dictionary so we can get two values per result (sessionId and attendeesId)
        /// </summary>
        /// <param name="codeCampYearId"></param>
        /// <returns></returns>
        public static Dictionary<int, int> GetSpeakerIdsWithMultipleSessions(int codeCampYearId)
        {
            string sql = String.Format(@"
                        SELECT  AttendeeId,SessionId
                        FROM SessionsOverview
                        WHERE attendeeid in (
                                                SELECT SessionsOverview.AttendeeId
                                                FROM SessionsOverview
                                                WHERE SessionsOverview.CodeCampYearId = {0}
                                                GROUP BY SessionsOverview.AttendeeId
                                                HAVING COUNT(*) > 1
                                ) AND
                                CodeCampYearId = {0}", codeCampYearId.ToString(CultureInfo.InvariantCulture));

            var dict = new Dictionary<int, int>();
            using (var sqlConnection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();
                using (var command = new SqlCommand(sql, sqlConnection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        try
                        {
                            while (reader.Read())
                            {
                                int attendeeId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                                int sessionId = reader.IsDBNull(1) ? -1 : reader.GetInt32(1);
                                if (!dict.ContainsKey(sessionId))
                                {
                                    dict.Add(sessionId, attendeeId);
                                }
                            }
                        }
                        catch (Exception eee1)
                        {
                            throw new ApplicationException(eee1.ToString());
                        }
                    }
                }
            }
            return dict;
        }

        public static List<SessionTagsViewResultData> GetSessionsTagViewCache()
        {
            string cacheSessionTagsViewResultData = "GetSessionsTagView";
            var o = (List<SessionTagsViewResultData>) HttpContext.Current.Cache[cacheSessionTagsViewResultData];
            if (o == null)
            {
                o = new List<SessionTagsViewResultData>();
                using (var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
                {
                    sqlConnection.Open();

                    string sqlSelect =
                        @"
                            SELECT 
                              dbo.SessionTagsView.Id,
                              dbo.SessionTagsView.TagId,
                              dbo.SessionTagsView.SessionId,
                              dbo.SessionTagsView.TagName,
                              dbo.SessionTagsView.CodeCampYearId
                            FROM
                              dbo.SessionTagsView
                            ";


                    var command = new SqlCommand(sqlSelect, sqlConnection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        try
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    var sessionTagsViewResultData =
                                        new SessionTagsViewResultData
                                            {
                                                Id = reader.GetInt32(0),
                                                TagId = reader.IsDBNull(1) ? -1 : reader.GetInt32(1),
                                                SessionId = reader.IsDBNull(2) ? -1 : reader.GetInt32(2),
                                                TagName = reader.IsDBNull(3) ? String.Empty : reader.GetString(3),
                                                CodeCampYearId = reader.IsDBNull(4) ? -1 : reader.GetInt32(4)
                                            };
                                    o.Add(sessionTagsViewResultData);
                                }
                            }
                        }
                        catch (Exception eee12)
                        {
                            throw new ApplicationException(eee12.ToString());
                        }
                    }
                }
                // insert into cache
                HttpContext.Current.Cache.Insert(cacheSessionTagsViewResultData, o,
                                                 null,
                                                 DateTime.Now.Add(new TimeSpan(0, 0,
                                                                               RetrieveSecondsForSessionCacheTimeout())),
                                                 TimeSpan.Zero);
            }
            return o;
        }



        public static int GetSponsorIdBasedOnUsername(string username)
        {
            int sponsorListId = -1;
            if (CheckUserIsAdmin())
            {
                // sponsorListId = 719; // K2, never coming back so can mess with
                sponsorListId = 718; // adobe
            }
            else
            {
                string email = GetEmailFromUsername(username);

                // ignore other sponsors this email may be associated with
                var rec =
                    ManagerBase
                        <SponsorListContactManager, SponsorListContactResult, SponsorListContact, CodeCampDataContext>.I
                                                                                                                      .Get
                        (new SponsorListContactQuery() {EmailAddress = email}).
                                                                                                                       FirstOrDefault
                        ();
                if (rec != null && rec.SponsorListId.HasValue)
                {
                    sponsorListId = rec.SponsorListId.Value;
                }
            }
            return sponsorListId;
        }

        public static string GetServiceEmailAddress()
        {
            string email = "info@siliconvalley-codecamp.com";
            if (ConfigurationManager.AppSettings["svccserviceemail"] != null)
            {
                email = ConfigurationManager.AppSettings["svccserviceemail"];
            }
            return email;
        }


        /// <summary>
        /// take text in and substitute using attendees id
        /// </summary>
        /// <param name="bodyText"></param>
        /// <param name="substituteWords"></param>
        /// <param name="attendeesId"> </param>
        /// <returns></returns>
        public static string SubstituteWordsInLetter(string bodyText, List<string> substituteWords, int attendeesId)
        {
            var attendeeRec =
                ManagerBase<AttendeesManager, AttendeesResult, Attendees, CodeCampDataContext>.I.Get(
                    new AttendeesQuery() {Id = attendeesId}).FirstOrDefault();
            if (attendeeRec != null)
            {
                var previousYears =
                    ManagerBase
                        <AttendeesCodeCampYearManager, AttendeesCodeCampYearResult, AttendeesCodeCampYear,
                            CodeCampDataContext>.I.Get(new AttendeesCodeCampYearQuery() {AttendeesId = attendeesId}).
                                                 Select(
                                                     a => a.CodeCampYearId).OrderBy(a => a).ToList();



                var dict = new Dictionary<string, string>();

                dict.Add("{PKID}", attendeeRec.PKID.ToString());
                dict.Add("{Username}", attendeeRec.Username);

                string registrationStatusThisYear = "Registered For 2012 Code Camp";
                if (!previousYears.Contains(CurrentCodeCampYear))
                {
                    registrationStatusThisYear =
                        "NOT Registered.  Please Login and Update your Registration at http://www.siliconvalley-codecamp.com/LoginCamp.aspx";
                }
                dict.Add("{RegistrationStatusThisYear}", registrationStatusThisYear);

                var registeredPreviousYears = "";
                foreach (var codeCampYearId in previousYears)
                {
                    if (codeCampYearId != CurrentCodeCampYear)
                    {
                        registeredPreviousYears += (codeCampYearId + 2005).ToString(CultureInfo.InvariantCulture) + ",";
                    }
                }
                if (registeredPreviousYears.EndsWith(","))
                {
                    registeredPreviousYears = registeredPreviousYears.Substring(0, registeredPreviousYears.Length - 1);
                }
                dict.Add("{RegisteredPreviousYears}", registeredPreviousYears);

                foreach (var rec in dict)
                {
                    bodyText = bodyText.Replace(rec.Key, rec.Value);
                }
            }

            return bodyText;
        }

        public static void ClearEmailDetailTable(string connectionName)
        {
            using (
                var sqlConnection =
                    new SqlConnection(connectionName))
            {
                sqlConnection.Open();

                const string sqlDelete = @"Truncate TABLE EmailDetails";

                try
                {
                    var command = new SqlCommand(sqlDelete, sqlConnection);
                    command.ExecuteNonQuery();
                }
                catch (Exception eee)
                {
                    throw new ApplicationException(eee.ToString());
                }
            }
        }

        public static void UpdateShirtSize(int attendeeId, string shirtSize)
        {
            try
            {
                using (
                    var sqlConnection =
                        new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
                {
                    sqlConnection.Open();

                    const string sqlSelect = "UPDATE Attendees SET ShirtSize = @ShirtSize WHERE Id = @Id";
                    using (var sqlCommand = new SqlCommand(sqlSelect, sqlConnection))
                    {
                        sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = attendeeId;
                        sqlCommand.Parameters.Add("@ShirtSize", SqlDbType.VarChar).Value = shirtSize;
                        sqlCommand.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception eee)
            {
                throw new ApplicationException(eee.ToString());
            }
        }

        public static bool CheckHasShirtSize(int attendeesId)
        {
            var attendee =
                ManagerBase<AttendeesManager, AttendeesResult, Attendees, CodeCampDataContext>.I.Get(
                    new AttendeesQuery() {Id = attendeesId}).FirstOrDefault();
            return attendee != null && !String.IsNullOrEmpty(attendee.ShirtSize);
        }


        public static void UpdateTrackImage(int trackId, byte[] bytes)
        {
            try
            {
                using (
                    var sqlConnection =
                        new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
                {
                    sqlConnection.Open();

                    const string sqlSelect = "UPDATE Track SET TrackImage = @TrackImage WHERE Id = @Id";
                    using (var sqlCommand = new SqlCommand(sqlSelect, sqlConnection))
                    {
                        sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = trackId;
                        sqlCommand.Parameters.Add("@TrackImage", SqlDbType.Image, bytes.Length).Value = bytes;
                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception eee)
            {
                throw new ApplicationException(eee.ToString());
            }
        }

        public static void UpdateSponsorImage(int sponsorId, byte[] bytes, string filename)
        {
            try
            {
                var extension = Path.GetExtension(filename);
                if (extension != null)
                {
                    string fileExtension = extension.Replace(".", "").ToLower();
                    using (
                        var sqlConnection =
                            new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
                    {
                        sqlConnection.Open();
                        const string sqlSelect =
                            "UPDATE SponsorList SET CompanyImage = @CompanyImage,CompanyImageType = @CompanyImageType WHERE Id = @Id";
                        using (var sqlCommand = new SqlCommand(sqlSelect, sqlConnection))
                        {
                            sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = sponsorId;
                            sqlCommand.Parameters.Add("@CompanyImage", SqlDbType.Image, bytes.Length).Value = bytes;
                            sqlCommand.Parameters.Add("@CompanyImageType", SqlDbType.NVarChar).Value =
                                !String.IsNullOrEmpty(fileExtension) ? fileExtension : "";
                            sqlCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception eee)
            {
                throw new ApplicationException(eee.ToString());
            }
        }

        public static void UpdateTwitterHandleForAttendee(string attendeeIdString, string twitterHandle)
        {
            int attendeeId;
            if (Int32.TryParse(attendeeIdString, out attendeeId))
            {
                if (!twitterHandle.StartsWith("@") && twitterHandle.Length > 1)
                {
                    twitterHandle = "@" + twitterHandle;
                    using (
                        var sqlConnection =
                            new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
                    {
                        sqlConnection.Open();
                        const string sqlSelect = "UPDATE Attendees SET TwitterHandle = @TwitterHandle WHERE Id = @Id";
                        using (var sqlCommand = new SqlCommand(sqlSelect, sqlConnection))
                        {
                            sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = attendeeId;
                            sqlCommand.Parameters.Add("@TwitterHandle", SqlDbType.NVarChar).Value = twitterHandle;
                            sqlCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public static void UpdateSessionMaterialUrl(int sessionsId, string url)
        {
            using (
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();
                const string sqlSelect = "UPDATE Sessions SET SessionsMaterialUrl = @Url WHERE Id = @Id";
                using (var sqlCommand = new SqlCommand(sqlSelect, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = sessionsId;
                    sqlCommand.Parameters.Add("@Url", SqlDbType.NVarChar).Value = url;
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateSessionShortUrl(int id, string sessionUrlShort)
        {
            using (
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();
                const string sqlSelect = "UPDATE Sessions SET ShortUrl = @ShortUrl WHERE Id = @Id";
                using (var sqlCommand = new SqlCommand(sqlSelect, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    sqlCommand.Parameters.Add("@ShortUrl", SqlDbType.NVarChar).Value = sessionUrlShort;
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }


        public static Dictionary<int, string> GetMaterialsUrlBySessionDict(int codeCampYearId)
        {
            const string cache = "GetMaterialsUrlBySessionDictCache";
            Dictionary<int, string> dict; // = new Dictionary<int, string>();

            if (HttpContext.Current.Cache[cache] == null)
            {

                dict = new Dictionary<int, string>();
                using (
                    var sqlConnection =
                        new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
                {
                    sqlConnection.Open();
                    const string sqlSelect = @"
                     SELECT dbo.Sessions.Id,
                            dbo.Sessions.SessionsMaterialUrl
                     FROM dbo.Sessions
                     WHERE dbo.Sessions.SessionsMaterialUrl IS NOT NULL AND
                           LEN(dbo.Sessions.SessionsMaterialUrl) > 0 AND
                           dbo.Sessions.CodeCampYearId = @CodeCampYearId";
                    using (var sqlCommand = new SqlCommand(sqlSelect, sqlConnection))
                    {
                        sqlCommand.Parameters.Add("@CodeCampYearId", SqlDbType.Int).Value = codeCampYearId;
                        using (var reader1 = sqlCommand.ExecuteReader())
                        {
                            while (reader1.Read())
                            {
                                int sessionId = reader1.IsDBNull(0) ? -1 : reader1.GetInt32(0);
                                string url = reader1.IsDBNull(1) ? "" : reader1.GetString(1);

                                if (dict.ContainsKey(sessionId))
                                {
                                    dict[sessionId] = url;
                                }
                                else
                                {
                                    dict.Add(sessionId, url);
                                }
                            }
                        }
                    }
                }
                HttpContext.Current.Cache.Insert(cache, dict,
                                                 null,
                                                 DateTime.Now.Add(new TimeSpan(0, 0,
                                                                               Utils.
                                                                                   RetrieveSecondsForSessionCacheTimeout
                                                                                   ())),
                                                 TimeSpan.Zero);

            }
            else
            {
                dict = (Dictionary<int, string>) HttpContext.Current.Cache[cache];
            }
            return dict;
        }

        /// <summary>
        /// get box urls but if overrides put those first.
        /// </summary>
        /// <param name="codeCampYearId"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetBoxFolderIdsBySessionDict(int codeCampYearId)
        {
            const string cache = "GetBoxFolderIdsBySessionDictCache";
            Dictionary<int, string> dict; // = new Dictionary<int, string>();

            if (HttpContext.Current.Cache[cache] == null)
            {

                dict = new Dictionary<int, string>();

                using (
                    var sqlConnection =
                        new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
                {
                    sqlConnection.Open();
                    const string sqlSelect = @"
                    SELECT 
                      dbo.Sessions.Id,
                      dbo.Sessions.BoxFolderIdString
                    FROM
                      dbo.Sessions
                    WHERE
                      dbo.Sessions.BoxFolderIdString IS NOT NULL AND
                      LEN(dbo.Sessions.BoxFolderIdString) > 0 AND
                      dbo.Sessions.CodeCampYearId = @CodeCampYearId";
                    using (var sqlCommand = new SqlCommand(sqlSelect, sqlConnection))
                    {
                        sqlCommand.Parameters.Add("@CodeCampYearId", SqlDbType.Int).Value = codeCampYearId;
                        using (var reader1 = sqlCommand.ExecuteReader())
                        {
                            while (reader1.Read())
                            {
                                int sessionId = reader1.IsDBNull(0) ? -1 : reader1.GetInt32(0);
                                string folderIdString = reader1.IsDBNull(1) ? "" : reader1.GetString(1);
                                dict.Add(sessionId, folderIdString);
                            }
                        }
                    }
                }
                HttpContext.Current.Cache.Insert(cache, dict,
                                                 null,
                                                 DateTime.Now.Add(new TimeSpan(0, 0,
                                                                               Utils.
                                                                                   RetrieveSecondsForSessionCacheTimeout
                                                                                   ())),
                                                 TimeSpan.Zero);
            }
            else
            {
                dict = (Dictionary<int, string>) HttpContext.Current.Cache[cache];
            }
            return dict;
        }

        public static void ClearLog4NetEntries(string connectionName)
        {
            using (
                var sqlConnection =
                    new SqlConnection(connectionName))
            {
                sqlConnection.Open();

                const string sqlDelete = @"Truncate TABLE Log4NetAll";

                try
                {
                    var command = new SqlCommand(sqlDelete, sqlConnection);
                    command.ExecuteNonQuery();
                }
                catch (Exception eee)
                {
                    throw new ApplicationException(eee.ToString());
                }
            }
        }


        /// <summary>
        /// http://stackoverflow.com/questions/2920744/url-slugify-alrogithm-in-c
        /// </summary>
        /// <param name="phrase"></param>
        /// <returns></returns>
        //public static string GenerateSlug(string phrase)
        //{
        //    string str = RemoveAccent(phrase).ToLower();
        //    // invalid chars           
        //    str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
        //    // convert multiple spaces into one space   
        //    str = Regex.Replace(str, @"\s+", " ").Trim();
        //    // cut and trim 
        //    str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
        //    str = Regex.Replace(str, @"\s", "-"); // hyphens   
        //    return str;
        //}

        //private static string RemoveAccent(string txt)
        //{
        //    byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(txt);
        //    return Encoding.ASCII.GetString(bytes);
        //}

        //public static string GetTitleEllipsized(string text,int characterCount,string ellipsis)
        //{
        //    var cleanTailRegex = new Regex(@"\s+\S*$");

        //    if (string.IsNullOrEmpty(text) || characterCount < 0 || text.Length <= characterCount)
        //        return text;

        //    return cleanTailRegex.Replace(text.Substring(0, characterCount + 1), "") + ellipsis;
        //}

        public static List<AttendeesShortForEmail> GetAttendeesShortBySql(string sqlFilter)
        {
            var recs = new List<AttendeesShortForEmail>();
            using (
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();
                string sqlSelect = String.Format(@"
                    SELECT 
                       attendees.Username,
                      attendees.UserFirstName,
                      attendees.UserLastName,
                      attendees.Email,
                      attendees.Id
                    FROM
                      attendees
                    WHERE
                      Id IN ({0}) ORDER BY attendees.Id", sqlFilter);





                using (var sqlCommand = new SqlCommand(sqlSelect, sqlConnection))
                {
                    using (var reader1 = sqlCommand.ExecuteReader())
                    {
                        while (reader1.Read())
                        {
                            AttendeesShortForEmail rec = new AttendeesShortForEmail();
                            rec.Username = reader1.IsDBNull(0) ? "" : reader1.GetString(0);
                            rec.UserFirstName = reader1.IsDBNull(1) ? "" : reader1.GetString(1);
                            rec.UserLastName = reader1.IsDBNull(2) ? "" : reader1.GetString(2);
                            rec.Email = reader1.IsDBNull(3) ? "" : reader1.GetString(3);
                            rec.Id = reader1.IsDBNull(4) ? -1 : reader1.GetInt32(4);
                            recs.Add(rec);
                        }
                    }
                }
            }

            return recs;

        }

        public static void UpdateEmailDetailsStatus(Guid guid)
        {
            //UPDATE TheTable SET RevisionId = RevisionId + 1 WHERE Id=@id
            using (
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();
                const string sqlSelect = @"
                   UPDATE EmailDetails SET EmailReadCount = EmailReadCount + 1,EmailReadDate=SYSUTCDATETIME() 
                   WHERE EmailDetailsGuid = @Guid";

                using (var sqlCommand = new SqlCommand(sqlSelect, sqlConnection))
                {
                    sqlCommand.Parameters.Add("@Guid", SqlDbType.UniqueIdentifier).Value = guid;
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// return true if speaker can present
        /// </summary>
        /// <param name="attendeesId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool GetSpeakerCanPresent(int attendeesId, out string message)
        {
            message = "";
            // block sessions that are over limit
            var attendeeRec = AttendeesManager.I.Get(new AttendeesQuery()
                {
                    Id = attendeesId
                }).FirstOrDefault();
            if (attendeeRec != null)
            {
                var numberSessionsAllowed = attendeeRec.PresentationLimit;
                var numberSessionsThisYear = SessionPresenterManager.I.Get(new SessionPresenterQuery()
                    {
                        AttendeeId = attendeesId,
                        CodeCampYearId = Utils.GetCurrentCodeCampYear()
                    }).Count();

                bool userIsAdmin = CheckUserIsAdmin();
                if (numberSessionsThisYear < numberSessionsAllowed || userIsAdmin)
                {
                    return true;
                }
            }
            message = "Attendee is over limit of sessions submitted for this year";
            return false;
        }

        public static int GetPictureLengthByAttendee(int id)
        {
            int pictureLength = 0;
            using (
                var sqlConnection =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();
                string sqlSelect = string.Format(@"select datalength(UserImage) from Attendees where id = {0}",
                                                 id.ToString(CultureInfo.InvariantCulture));

                using (var sqlCommand = new SqlCommand(sqlSelect, sqlConnection))
                {
                    pictureLength = (int) sqlCommand.ExecuteScalar();
                }
            }
            return pictureLength;
        }
    }

    public class AttendeesShortForEmail
    {
        public bool Selected { get; set; }
        public int Id { get; set; }
        public string Email { set; get; }
        public string Username { get; set; }
        public string UserFirstName { set; get; }
        public string UserLastName { set; get; }
    }


    public class AttendeesShort
    {
        public string EmailAddress { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
    }


    public class SessionTagsViewResultData
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public int SessionId { get; set; }
        public string TagName { get; set; }
        public int CodeCampYearId { get; set; }
    }


    public class JobData
    {
        public string JobTitle { get; set; }
        public DateTime JobDatePosted { get; set; }
        public string Company { get; set; }
        public string JobLink { get; set; }
        public string JobDescription { get; set; }
        public string JobLocation { get; set; }
    }


    /// <summary>
    /// skinny user class for JSON use primarily
    /// </summary>
    public class AttendeeShort
    {
        private DateTime createDate;
        private string firstName;
        private bool isApproved;
        private string lastName;
        private string pkid;
        private string userBio;
        private string userName;
        private string userWebSite;
        private string zipCode;

        public AttendeeShort(string userName, string firstName, string lastName, string zipCode, string pkid,
                             string userBio, bool isApproved, DateTime createDate, string userWebSite)
        {
            this.userName = userName;
            this.firstName = firstName;
            this.zipCode = zipCode;
            this.lastName = lastName;
            PKID1 = pkid;
            this.userBio = userBio;
            this.isApproved = isApproved;
            this.createDate = createDate;
            this.userWebSite = userWebSite;
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string ZipCode
        {
            get { return zipCode; }
            set { zipCode = value; }
        }

        public string PKID1
        {
            get { return pkid; }
            set { pkid = value; }
        }

        public string UserBio
        {
            get { return userBio; }
            set { userBio = value; }
        }

        public bool IsApproved
        {
            get { return isApproved; }
            set { isApproved = value; }
        }

        public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }

        public string UserWebSite
        {
            get { return userWebSite; }
            set { userWebSite = value; }
        }
    }


    public class WS_Detail
    {
        public string loggedInFirstName;
        public string loggedInLastName;
        public string loggedInPictureUrl;
        public string loggedInUsername;
        public List<WS_SessionDetail> SessionDetails;
    }

    public class WS_LatLon
    {
        public string firstName;
        public bool isSpeaker;
        public string lastName;
        public double lattitude;
        public double longitude;
        public string speakerPictureUrl;

        public WS_LatLon()
        {
        }

        public WS_LatLon(double lattitude, double longitude, bool isSpeaker, string firstName, string lastName,
                         string speakerPictureUrl)
        {
            this.lattitude = lattitude;
            this.longitude = longitude;
            this.isSpeaker = isSpeaker;
            this.firstName = firstName;
            this.lastName = lastName;
            this.speakerPictureUrl = speakerPictureUrl;
        }
    }


    public class WS_SessionDetail
    {
        public DateTime SessionDateTime;
        public SessionsODS.DataObjectSessions sessionInfo;
        public List<string> SessionTags;
        public string SessionURL;
        public string SpeakerBio;
        public string SpeakerFirstName;
        public string SpeakerLastName;
        public string SpeakerPersonalURL;
        public string SpeakerPictureUrl;
        public string SpeakerURL;
        public string SpeakerZipCode;

        public WS_SessionDetail()
        {
        }

        public WS_SessionDetail(SessionsODS.DataObjectSessions sessionInfo, string speakerFirstName,
                                string speakerLastName, string sessionURL, string speakerURL, string speakerBio,
                                string speakerPictureUrl, string speakerZipCode, string speakerPersonalURL,
                                DateTime sessionDateTime, List<string> sessionTags)
        {
            this.sessionInfo = sessionInfo;
            SpeakerFirstName = speakerFirstName;
            SpeakerLastName = speakerLastName;
            SessionURL = sessionURL;
            SpeakerURL = speakerURL;
            SpeakerBio = speakerBio;
            SpeakerPictureUrl = speakerPictureUrl;
            SpeakerZipCode = speakerZipCode;
            SpeakerPersonalURL = speakerPersonalURL;
            SessionDateTime = sessionDateTime;
            SessionTags = sessionTags;
        }
    }

    public class ReferralLogger
    {
        public string Path { get; set; }
        public string Referrer { get; set; }

        public ReferralLogger(string path, Uri urlReferrer)
        {
            Path = path.Substring(0, Math.Min(512, path.Length));
            if (urlReferrer != null)
            {
                Referrer = urlReferrer.ToString().Trim().Substring(0, Math.Min(512, urlReferrer.ToString().Length));
            }
        }

        public void Log()
        {
            if (!String.IsNullOrEmpty(Path) && !String.IsNullOrEmpty(Referrer))
            {
                Utils.LogReferrerUrl(Path, Referrer);
            }
        }
    }

    public class ReferringUrlAttendeeInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserWebSite { get; set; }
        public string ReferringUrlName { get; set; }
        public string ArticleName { get; set; }
        public string UserGroup { get; set; }
        public int ReferralCountPast30Days { get; set; }
        public int ReferralCountAllTime { get; set; }
    }

    [DataObject(true)] // This attribute allows the ObjectDataSource wizard to see this class
    public class ReferringUrlAttendeeInfoDo
    {
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<ReferringUrlAttendeeInfo> Get()
        {
            var recs = Utils.GetAllReferralUrls();

            foreach (ReferringUrlAttendeeInfo t in recs)
            {
                if (String.IsNullOrEmpty(t.ArticleName))
                {
                    t.ArticleName = t.ReferringUrlName;
                }
            }

            return recs.OrderByDescending(a => a.ReferralCountAllTime).ToList();
        }
    }



    public class CodeCampCount
    {
        public int daysBeforeCamp { get; set; }
        public int CountYear0 { get; set; }
        public int CountYearMinus1 { get; set; }
        public int CountYearMinus2 { get; set; }
        public int CountYearMinus3 { get; set; }
        public int CountYearMinus4 { get; set; }
        public int CountYearMinus5 { get; set; }
        public int CountYearMinus6 { get; set; }
    }

    [DataObject(true)] // This attribute allows the ObjectDataSource wizard to see this class
    public class CodeCampCountDb
    {
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CodeCampCount> Get(int numDaysToGoBack)
        {
            var results = new List<CodeCampCount>();

            var rt = new RegistrationTracker();

            // Grab Code Camp Years
            var ccYears = CodeCampYearManager.I.GetAll().OrderByDescending(a => a.CampStartDate).ToList();

            var year0recs = rt.GetNumberRegistered(ccYears[0].Id, numDaysToGoBack);
            var yearMinus1recs = rt.GetNumberRegistered(ccYears[1].Id, numDaysToGoBack);
            var yearMinus2recs = rt.GetNumberRegistered(ccYears[2].Id, numDaysToGoBack);
            var yearMinus3recs = rt.GetNumberRegistered(ccYears[3].Id, numDaysToGoBack);
            var yearMinus4recs = rt.GetNumberRegistered(ccYears[4].Id, numDaysToGoBack);
            var yearMinus5recs = rt.GetNumberRegistered(ccYears[5].Id, numDaysToGoBack);
            var yearMinus6recs = rt.GetNumberRegistered(ccYears[6].Id, numDaysToGoBack);



            for (int i = 0; i < numDaysToGoBack; i++)
            {
                var r = new CodeCampCount()
                    {
                        daysBeforeCamp = numDaysToGoBack - i,
                        CountYear0 = year0recs[i],
                        CountYearMinus1 = yearMinus1recs[i],
                        CountYearMinus2 = yearMinus2recs[i],
                        CountYearMinus3 = yearMinus3recs[i],
                        CountYearMinus4 = yearMinus4recs[i],
                        CountYearMinus5 = yearMinus5recs[i],
                        CountYearMinus6 = yearMinus6recs[i]
                    };
                results.Add(r);
            }
            return results;

        }


        public static int GetSessionSlugLength()
        {
            return Utils.SessionSlugLengthMax;
        }

    }



    public class RegistrationTracker
    {
        List<CodeCampYearResult> _codeCampYearResults;

        public RegistrationTracker()
        {
            _codeCampYearResults = CodeCampYearManager.I.GetAll();
        }

        public List<int> GetNumberRegistered(int codeCampYearId, int numberDaysToCheck)
        {
            var recs = AttendeesCodeCampYearManager.I.Get(new AttendeesCodeCampYearQuery() { CodeCampYearId = codeCampYearId });

            var results = new List<int>();
            DateTime codeCampDate = _codeCampYearResults.Where(a => a.Id == codeCampYearId).Select(a => a.CampStartDate).FirstOrDefault();
            //for (int i = 1; i <= numberDaysToCheck; i++)
            for (int i = numberDaysToCheck; i > 0; i--)
            {
                DateTime startDate = codeCampDate.Subtract(TimeSpan.FromDays(i - 1));
                int totalRegistered;
                if (startDate.CompareTo(DateTime.Now) < 0)
                {
                    totalRegistered = recs.Where(a => a.CreateDate <= startDate).Count();
                }
                else
                {
                    totalRegistered = 0;
                }
                results.Add(totalRegistered);
            }
            return results;
        }

        


    }

   
}