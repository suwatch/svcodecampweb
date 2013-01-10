using System;
using System.Collections.Generic;
using System.Linq;
using CodeCampSV;
using System.ComponentModel;

namespace CodeCampSV
{
    public partial class TwitterUpdateManager
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<TwitterUpdateResult> GetByParams(string svCodeCamp, string svcc, string allSpeakers, 
            string planPlusInterest, string plan, string loggedInUsername, int codeCampYearId,int maxTweetsToShow)
        {

            // get list of all speaker twitter handles for this year.
            List<string> speakerTwitterHandlesAll = new List<string>();
            var sessionsList = SessionsManager.I.Get(new SessionsQuery() 
            { CodeCampYearId = codeCampYearId, WithSpeakers = true, WithInterestOrPlanToAttendAttributeSet = true });

            foreach (var session in sessionsList)
            {
                foreach (var speaker in session.SpeakersList)
                {
                    if (!String.IsNullOrEmpty(speaker.TwitterHandle) && !speakerTwitterHandlesAll.Contains(CleanTwitterHandle(speaker.TwitterHandle)))
                    {
                        speakerTwitterHandlesAll.Add(CleanTwitterHandle(speaker.TwitterHandle));
                    }
                }
            }


            List<string> speakerTwitterHandlesInterestedIn = new List<string>();
            List<string> speakerTwitterHandlesPlanToAttend = new List<string>();

            if (!String.IsNullOrEmpty(loggedInUsername))
            {
                var sessionIdsInterestedIn = new List<int>();
                var sessionIdsPlanToAttend = new List<int>();

                Guid attendeePKID = AttendeesManager.I.Get(new AttendeesQuery() { Username = loggedInUsername }).FirstOrDefault().PKID;
                var recsx = SessionAttendeeManager.I.Get(new SessionAttendeeQuery() { Attendees_username = attendeePKID, CodeCampYearId = codeCampYearId });

                sessionIdsInterestedIn =
                    recsx.Where(a => a.Interestlevel == 2).Select(a => a.Sessions_id).ToList();
                sessionIdsPlanToAttend =
                   recsx.Where(a => a.Interestlevel == 3).Select(a => a.Sessions_id).ToList();

                foreach (var session in sessionsList)
                {
                    if (sessionIdsInterestedIn.Contains(session.Id))
                    {
                        foreach (var speaker in session.SpeakersList)
                        {
                            if (!String.IsNullOrEmpty(speaker.TwitterHandle) && !speakerTwitterHandlesInterestedIn.Contains(CleanTwitterHandle(speaker.TwitterHandle)))
                            {
                                speakerTwitterHandlesInterestedIn.Add(CleanTwitterHandle(speaker.TwitterHandle));
                            }
                        }
                    }

                    if (sessionIdsPlanToAttend.Contains(session.Id))
                    {
                        foreach (var speaker in session.SpeakersList)
                        {
                            if (!String.IsNullOrEmpty(speaker.TwitterHandle) && !speakerTwitterHandlesPlanToAttend.Contains(CleanTwitterHandle(speaker.TwitterHandle)))
                            {
                                speakerTwitterHandlesPlanToAttend.Add(CleanTwitterHandle(speaker.TwitterHandle));
                            }
                        }
                    }
                }
            }

            var twitterUpdateResults = GetAll();
            List<TwitterUpdateResult> results = new List<TwitterUpdateResult>();


            foreach (var twitterUpdateResult in twitterUpdateResults)
            {
                bool addTweet = false;
                if (twitterUpdateResult.TweetNotRelevant != null && twitterUpdateResult.TweetNotRelevant.HasValue &&
                    twitterUpdateResult.TweetNotRelevant.Value == true)
                {

                }
                else
                {
                    if (svCodeCamp.Equals("1"))
                    {
                        if (twitterUpdateResult.AuthorHandle.Equals("sv_code_camp"))
                        {
                            addTweet = true;
                        }
                    }

                    if (svcc.Equals("1"))
                    {

                        if (twitterUpdateResult.ContentTweet.Contains("#svcc") ||
                            twitterUpdateResult.ContentTweet.Contains("@sv_code_camp") ||
                            twitterUpdateResult.AuthorHandle.Equals("sv_code_camp"))
                        {
                            addTweet = true;
                        }
                    }

                    // if all speakers, then don't need to worry about ones person is interested in
                    if (allSpeakers.Equals("1"))
                    {
                        if (speakerTwitterHandlesAll.Contains(twitterUpdateResult.AuthorHandle))
                        {
                            addTweet = true;
                        }
                    }
                    else
                    {
                        if (plan.Equals("1"))
                        {
                            if (speakerTwitterHandlesPlanToAttend.Contains(twitterUpdateResult.AuthorHandle))
                            {
                                addTweet = true;
                            }
                        }

                        if (planPlusInterest.Equals("1"))
                        {
                            if (speakerTwitterHandlesPlanToAttend.Contains(twitterUpdateResult.AuthorHandle) || speakerTwitterHandlesInterestedIn.Contains(twitterUpdateResult.AuthorHandle))
                            {
                                addTweet = true;
                            }
                        }
                    }

                    if (addTweet)
                    {
                        if (loggedInUsername != null && loggedInUsername.Equals("pkellner"))
                        {
                            twitterUpdateResult.ContentTweet += " ADMIN: " + DateTime.Now.AddHours(-3).ToString();
                        }
                        results.Add(twitterUpdateResult);
                    }
                }
            }
            return results.OrderByDescending(a => a.Published).Take(maxTweetsToShow).ToList();


        }

        private string CleanTwitterHandle(string twitterHandle)
        {
            string ret = twitterHandle.Trim();
            if (  (twitterHandle.StartsWith("@") || twitterHandle.StartsWith("#")) && twitterHandle.Length > 1)
            {
                ret = twitterHandle.Substring(1).Trim();
            }
            return ret;
        }

        public List<TwitterUpdateResult> Get(TwitterUpdateQuery query)
        {

            var meta = new CodeCampDataContext();

            IQueryable<TwitterUpdate> baseQuery = from myData in meta.TwitterUpdate select myData;


            //  next is automated query formation from AutoGen Shared Class 
            //  (do not remove next line or filters will go away)
            baseQuery = BaseQueryAutoGen(baseQuery, query);

            IQueryable<TwitterUpdateResult> results = GetBaseResultIQueryable(baseQuery);


            List<TwitterUpdateResult> resultList = GetFinalResults(results, query);

            foreach (var r in resultList)
            {
                r.PrettyDateDifferenceFromNow = GetPrettyDate(r.Published.Value);
            }

            return resultList;
        }

        private string GetPrettyDate(DateTime dateUTC)
        {
            // this is a kluge for how many hours ORCSWeb server is from GMT
            DateTime d = dateUTC.Subtract(TimeSpan.FromHours(4));

            // 1.
            // Get time span elapsed since the date.
            TimeSpan s = DateTime.Now.Subtract(d);

            // 2.
            // Get total number of days elapsed.
            int dayDiff = (int)s.TotalDays;

            // 3.
            // Get total number of seconds elapsed.
            int secDiff = (int)s.TotalSeconds;

            // 4.
            // Don't allow out of range values.
            if (dayDiff < 0 || dayDiff >= 31)
            {
                return null;
            }

            // 5.
            // Handle same-day times.
            if (dayDiff == 0)
            {
                // A.
                // Less than one minute ago.
                if (secDiff < 60)
                {
                    return "within the last minute";
                }
                // B.
                // Less than 2 minutes ago.
                if (secDiff < 120)
                {
                    return "1 minute ago";
                }
                // C.
                // Less than one hour ago.
                if (secDiff < 3600)
                {
                    return string.Format("{0} minutes ago",
                        Math.Floor((double)secDiff / 60));
                }
                // D.
                // Less than 2 hours ago.
                if (secDiff < 7200)
                {
                    return "1 hour ago";
                }
                // E.
                // Less than one day ago.
                if (secDiff < 86400)
                {
                    return string.Format("{0} hours ago",
                        Math.Floor((double)secDiff / 3600));
                }
            }
            // 6.
            // Handle previous days.
            if (dayDiff == 1)
            {
                return "yesterday";
            }
            if (dayDiff < 7)
            {
                return string.Format("{0} days ago",
                dayDiff);
            }
            if (dayDiff < 31)
            {
                return string.Format("{0} weeks ago",
                Math.Ceiling((double)dayDiff / 7));
            }
            return null;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<TwitterUpdateResult> GetAll()
        {
            return Get(new TwitterUpdateQuery { IsMaterializeResult = true });
        }
    }


}
