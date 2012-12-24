using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeCampSV;
using System.Diagnostics;

/// <summary>
/// Summary description for SessionInterestAnalysis
/// </summary>
public class SessionInterestAnalysis
{
    int _codeCampYearId;
    List<SessionTagsViewResultData> _sessionTags;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="codeCampYearId"></param>
    /// <param name="topTagsToRemove"></param>
	public SessionInterestAnalysis(int codeCampYearId,int topTagsToRemove)
	{
        //int codeCampYearId = Utils.GetCurrentCodeCampYear();
        _codeCampYearId = codeCampYearId;
       

        // all sessionTag records for sessions this year.
        var sessionTagsOri =
            Utils.GetSessionsTagViewCache().Where(a => a.CodeCampYearId == codeCampYearId).ToList();



        // get most popular tags
        var topTagIds = (from data in sessionTagsOri
                         group data by data.TagId into g
                         select new
                         {
                             TagId = g.Key,
                             MyCount = g.Count()
                         }).OrderByDescending(a => a.MyCount).Take(topTagsToRemove).ToList();

        List<int> topTagIdsReal = topTagIds.Select(a => a.TagId).ToList();

        //// get most popular tags
        //var topTagNames = (from data in sessionTagsOri
        //                   group data by data.TagName into g
        //                   select new
        //                   {
        //                       TagId = g.Key,
        //                       MyCount = g.Count()
        //                   }).OrderByDescending(a => a.MyCount).ToList();


        _sessionTags = new List<SessionTagsViewResultData>();
        foreach (var st in sessionTagsOri)
        {
            if (!topTagIdsReal.Contains(st.TagId))
            {
                _sessionTags.Add(st);
            }
        }
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="username"></param>
    /// <param name="interestLevel">"I","P","" for Interest,PlanToAttend or Both</param>
    /// <returns></returns>
    public List<int> GetSessionIds(string username,string interestLevel)
    {
        Stopwatch stopWatch = new Stopwatch();
        //var _StopWatch = new StopWatch();
        stopWatch.Start();
        //var username = Utils.GetAttendeeUsernameByPKID(rec.ToString());

        // all tags for sessions this year. (list of ints)
        var sessionTagsIds = _sessionTags.Select(a => a.TagId).Distinct().ToList();

        // all sessions this year
        var sessionIdsCurrentYear = _sessionTags.Select(a => a.SessionId).Distinct().ToList();

        // all tags peter ever has known (comes back as distinct)
        var tagIdsAttendeeHasKnown = Utils.GetAttendeeSessionsTags(username, interestLevel);

        // gives us all the sessionsIds we might be interested in
        var tagsOfInterestThisYear =
                    (from data in sessionTagsIds
                     where tagIdsAttendeeHasKnown.Contains(data)
                     select data).ToList();

        List<int> sessionIdsOfInterest = new List<int>();

        // go through each session and see if meets the criteria of having a tag the attendee might be interested
        foreach (var sessionId in sessionIdsCurrentYear)
        {
            var tagIdsForSession = _sessionTags.Where(a => a.SessionId == sessionId).Select(a => a.TagId).Distinct().ToList();

            var numFound = tagIdsForSession.Intersect(tagIdsAttendeeHasKnown).Count();

            if (numFound > 0)
            {
                sessionIdsOfInterest.Add(sessionId);
            }
        }
        stopWatch.Stop();

        return sessionIdsOfInterest.OrderBy(a => a).ToList();
        

        //strings.Add(sessionIdsOfInterest.Count() + " " + username + " " + stopWatch.ElapsedMilliseconds.ToString());

        //var sessions = SessionsManager.I.Get(new SessionsQuery() { Ids = recs });

    }

}