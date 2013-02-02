using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
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
            List<SessionTimesResult> sessionTimesResults =
                ManagerBase<SessionTimesManager, SessionTimesResult, SessionTimes, CodeCampDataContext>.I.Get(new SessionTimesQuery
                                                                                                                  {
                                                                                                                      CodeCampYearId = codeCampYearId
                                                                                                                  });

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
            if (unAssignedIds.Any())
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
    }
}