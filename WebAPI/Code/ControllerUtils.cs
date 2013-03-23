using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Serialization;
using CodeCampSV;
using WebAPI.Controllers;
using WebAPI.ViewModels;

namespace WebAPI.Code
{
    public class ControllerUtils
    {
        public static List<SponsorListResult> AllSponsors(int codeCampYearId)
        {
            List<SponsorListResult> sponsors =
                ManagerBase<SponsorListManager, SponsorListResult, CodeCampSV.SponsorList, CodeCampDataContext>.I.Get(new SponsorListQuery
                {
                    CodeCampYearId = codeCampYearId,
                    IncludeSponsorLevel = true,
                    PlatinumLevel = Utils.MinSponsorLevelPlatinum,
                    GoldLevel = Utils.MinSponsorLevelGold,
                    SilverLevel = Utils.MinSponsorLevelSilver,
                    BronzeLevel = Utils.MinSponsorLevelBronze
                });
            return sponsors;
        }

        public static CommonViewModel GetCommonViewModelOneSession(string session, CommonViewModel commonViewModel)
        {
            List<SessionsResult> sessionsTemp = commonViewModel.Sessions;
            var sessionSlugsDict = new Dictionary<string, int>();
            foreach (SessionsResult result in sessionsTemp)
            {
                if (!sessionSlugsDict.ContainsKey(result.SessionSlug))
                {
                    sessionSlugsDict.Add(result.SessionSlug, result.Id);
                }
            }
            var sessions = new List<SessionsResult>();
            if (sessionSlugsDict.ContainsKey(session))
            {
                SessionsResult sessionsResult = sessionsTemp.FirstOrDefault(a => a.Id == sessionSlugsDict[session]);
                if (sessionsResult != null)
                {
                    sessions = commonViewModel.Sessions.Where(a => a.Id == sessionsResult.Id).ToList();
                }
            }
            commonViewModel.Sessions = sessions;
            return commonViewModel;
        }


        /// <summary>
        /// take in a list of all sessions you are interested in and divide them up into buckets by session time.  
        /// return the list of sessions in that order and also return unassigned session to end of list so that
        /// all sessions are accounted for.
        /// </summary>
        /// <param name="codeCampYearId"></param>
        /// <param name="sessions"></param>
        /// <returns></returns>
        public static List<SessionTimesResult> SessionTimesResultsWithSessionInfo(int codeCampYearId, List<SessionsResult> sessions)
        {
            // string retStr = "";
            List<SessionTimesResult> sessionTimesResults;
            if (IsTestMode)
            {
                var commonViewModel = CommonViewModelTestData();
                sessionTimesResults = commonViewModel.SessionTimeResults;
            }
            else
            {
                sessionTimesResults =
                    ManagerBase<SessionTimesManager, SessionTimesResult, SessionTimes, CodeCampDataContext>.I.Get
                        (new SessionTimesQuery
                             {
                                 CodeCampYearId
                                     =
                                     codeCampYearId
                             });
            }

            var sessionsAssignedToTime = new List<int>();

                // need to go through all times and sort. need to create extra record for unassigned times
                foreach (var sessionTimeResult in sessionTimesResults)
                {
                    sessionTimeResult.SessionsResults =
                        sessions.Where(a => a.SessionTimesId == sessionTimeResult.Id)
                                .OrderBy(a => a.Title.ToUpper())
                                .ToList();
                    sessionsAssignedToTime.AddRange(sessionTimeResult.SessionsResults.Select(a => a.Id).ToList());
                }

                var allSessionIds = sessions.Select(a => a.Id).ToList();

                var unAssignedIds = allSessionIds.Except(sessionsAssignedToTime);
                if (unAssignedIds.Any() && sessionTimesResults.Count > 0)
                {
                    var sessionTimeResultUnassigned = new SessionTimesResult
                                                          {
                                                              Id = sessionTimesResults.Max(a => a.Id) + 1,
                                                              CodeCampYearId = codeCampYearId,
                                                              SessionsResults = sessions,
                                                              StartTimeFriendly = "Unassigned"
                                                          };
                    sessionTimesResults.Add(sessionTimeResultUnassigned);
                }
            

            return sessionTimesResults;
        }

     
        /// <summary>
        /// Returns last 5 job postings regardless of when
        /// </summary>
        /// <returns></returns>
        public static List<SponsorListJobListingResult> JobsTop()
        {
            return ManagerBase<SponsorListJobListingManager, SponsorListJobListingResult, SponsorListJobListing, CodeCampDataContext>.I.Get(new SponsorListJobListingQuery()
                                                          {
                                                              Top5ForTesting = true
                                                          });
        }

        /// <summary>
        /// returns last 5 blog feed items
        /// </summary>
        /// <returns></returns>
        public static List<RSSItem> FeedItems()
        {
            return new List<RSSItem>
                       {
                           new RSSItem(1, "title", "http://peterkellner.net", ""),
                           new RSSItem(1, "title", "http://siliconvalley-codecamp.com", ""),

                       };
           // return new RSSFeedObject().Get(5);
        }

        /// <summary>
        /// Adds Jobs,RSSFeed,Sponsors to CommonViewModel
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="codeCampYearId"></param>
        /// <returns></returns>
        public static CommonViewModel UpdateViewModel(CommonViewModel viewModel, int codeCampYearId)
        {
            viewModel.JobListings = JobsTop();
            viewModel.FeedItems = FeedItems();
            viewModel.Sponsors = AllSponsors(codeCampYearId);
            return viewModel;
        }

        /// <summary>
        /// takes in int codeCampYearId (presumably as method call) and returns it for later use
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="codeCampYearId"></param>
        /// <param name="codeCampYearIdOut"></param>
        /// <returns></returns>
        public static CommonViewModel UpdateViewModel
            (CommonViewModel viewModel, int codeCampYearId, out int codeCampYearIdOut)
        {
            codeCampYearIdOut = codeCampYearId;
            return UpdateViewModel(viewModel, codeCampYearId);
        }

        /// <summary>
        /// Returns CodeCampYearId or throws 404 if not found (not sure if this is right error code for this)
        /// </summary>
        /// <param name="year"></param>
        public static int GetCodeCampYearId(string year)
        {
            var codeCampYearId = CodeCampYearId(year);
            if (codeCampYearId < 0)
            {
                throw new HttpException(404, "CCYearNotFound");
            }
            return codeCampYearId;
        }

        private static int CodeCampYearId(string year)
        {
            var codeCampYears = Utils.GetListCodeCampYear();
            var dateDict = codeCampYears.ToDictionary(k => k.CampStartDate.Year.ToString(CultureInfo.InvariantCulture),
                                                      v => v.Id);
            int codeCampYearId = -1;
            if (dateDict.ContainsKey(year))
            {
                codeCampYearId = dateDict[year];
            }
            return codeCampYearId;
        }

        public static bool IsTestMode
        {
            get
            {
                return ConfigurationManager.AppSettings["TestingDataOnly"] != null &&
                       (ConfigurationManager.AppSettings["TestingDataOnly"].ToLower().Equals("true"));
            } 
        }

        public static CommonViewModel CommonViewModelTestData()
        {
            CommonViewModel commonViewModel = null;
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var textStreamReader =
                    new StreamReader(assembly.GetManifestResourceStream("WebAPI.App_Data.CommonViewModel.xml"));
                //string data = _textStreamReader.ReadToEnd();
                var ser = new XmlSerializer(typeof(CommonViewModel));
                commonViewModel = (CommonViewModel)ser.Deserialize(textStreamReader);
            }
            catch
            {
                throw new ApplicationException("deserializing test data failed");
            }
            return commonViewModel;
        }
    }
}