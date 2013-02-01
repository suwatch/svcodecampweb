using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeCampSV;
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

     
        public static List<SponsorListJobListingResult> JobsTop()
        {
            return SponsorListJobListingManager.I.Get(new SponsorListJobListingQuery()
                                                          {
                                                              Top5ForTesting = true
                                                          });
        }

        public static List<RSSItem> FeedItems()
        {
            return new RSSFeedObject().Get(5);
        }

        public static void UpdateViewModel(CommonViewModel viewModel)
        {
            viewModel.JobListings = JobsTop();
            viewModel.FeedItems = FeedItems();
        }
    }
}