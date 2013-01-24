using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CodeCampSV;
using System.ComponentModel;
using System.Web;

namespace CodeCampSV
{
    public partial class SessionsManager
    {

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void UpdateHashTag(SessionsResult sessionResult)
        {
            var rec = SessionsManager.I.Get(new SessionsQuery() {Id = sessionResult.Id}).FirstOrDefault();
            if (rec != null)
            {
                rec.TwitterHashTags = sessionResult.TwitterHashTags;
            }
            SessionsManager.I.Update(rec);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SessionsResult> GetByCodeCampYearId(int codeCampYearId)
        {
            var sessionResults = Get(new SessionsQuery()
                                         {
                                             CodeCampYearId = codeCampYearId,
                                             WithSpeakers = true
                                         }).OrderBy(a => a.Title.ToLower()).ToList();

            foreach (var rec in sessionResults)
            {
                if (rec.SpeakersList.Count > 0)
                {
                    rec.PresenterName = rec.SpeakersList[0].UserFirstName + " " + rec.SpeakersList[0].UserLastName;
                }
            }

            return sessionResults;
        }



        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SessionsResult> GetByStringIds(string listOfIds,int codeCampYearId)
        {
            List<SessionsResult> results = new List<SessionsResult>();
            if (!String.IsNullOrEmpty(listOfIds))
            {
                List<String> idsListString = listOfIds.Split(',').ToList();
                List<int> ids = new List<int>();
                foreach (var idString in idsListString)
                {
                    int id = Convert.ToInt32(idString);
                    ids.Add(id);
                }

                results = Get(new SessionsQuery()
                {
                    CodeCampYearId = codeCampYearId,
                    Ids = ids,
                    IsMaterializeResult = true
                });
            }
            return results;
        }


        public List<SessionsResult> Get(SessionsQuery query)
        {
            if (query.CodeCampYearId.HasValue)
            {
                query.CodeCampYearIds = new List<int>() { query.CodeCampYearId.Value};
            }

            var meta = new CodeCampDataContext();

            IQueryable<Sessions> baseQuery = from myData in meta.Sessions  
                                             select myData;

            // .Where(a => a.Id == 594).ToList();
            baseQuery = BaseQueryAutoGen(baseQuery, query);

            if (query.SkipRoomIds != null && query.SkipRoomIds.Any())
            {
                baseQuery = from data in baseQuery
                            where data.LectureRoomsId.HasValue && !query.SkipRoomIds.Contains(data.LectureRoomsId.Value)
                            select data;
            }

            if (query.RoomIds != null && query.RoomIds.Any())
            {
                baseQuery = from data in baseQuery
                            where data.LectureRoomsId.HasValue && query.RoomIds.Contains(data.LectureRoomsId.Value)
                            select data;
            }


            var sessionIdsPlanToAttend = new List<int>();
            var sessionIdsInterested = new List<int>();
            if (query.WithInterestOrPlanToAttend != null &&  HttpContext.Current != null && HttpContext.Current.User.Identity.IsAuthenticated )
            {
                string username = HttpContext.Current.User.Identity.Name;

                Guid? PKID = (from data in meta.Attendees
                            where data.Username.Equals(username)
                            select data.PKID).FirstOrDefault();
                if (PKID != null)
                {
                    sessionIdsPlanToAttend = (from data in meta.SessionAttendee
                                              where data.Interestlevel == 3 && data.Attendees_username == PKID.Value
                                              select data.Sessions_id).ToList();

                    sessionIdsInterested = (from data in meta.SessionAttendee
                                              where data.Interestlevel == 2 && data.Attendees_username == PKID.Value
                                              select data.Sessions_id).ToList();
                }
            }




            if (query.CodeCampYearIds != null && query.CodeCampYearIds.Count > 0)
            {
                baseQuery = baseQuery.Where(data => query.CodeCampYearIds.Contains(data.CodeCampYearId));
            }

            List<int> sponsorIdsWithActiveJobs;
            if (query.JustActiveJobListings != null && query.JustActiveJobListings.Value)
            {
                sponsorIdsWithActiveJobs = (from data in meta.SponsorListJobListing
                                            where data.StartRunDate.HasValue && data.EndRunDate.HasValue &&
                                                  data.StartRunDate.Value.CompareTo(DateTime.Now) < 0 &&
                                                  data.EndRunDate.Value.CompareTo(DateTime.Now) > 0 &&
                                                  data.HideListing == false
                                            select data.SponsorListId).ToList();
                baseQuery =
                    baseQuery.Where(
                        data => data.SponsorId != null && sponsorIdsWithActiveJobs.Contains(data.SponsorId.Value));
            }

            var speakerResults = new List<AttendeesResult>();
            var sessionPresenterResults =new List<SessionPresenterResult>();
            if (query.WithSpeakers != null && query.WithSpeakers.Value)
            {
                //var attendeesQuery = new AttendeesQuery
                //                         {
                //                             PresentersOnly = true
                //                         };
                var sessionPresenterQuery = new SessionPresenterQuery();

                

                if (query.CodeCampYearIds != null && query.CodeCampYearIds.Count > 0)
                {
                    //attendeesQuery.CodeCampYearIds = query.CodeCampYearIds;
                    sessionPresenterQuery.CodeCampYearIds = query.CodeCampYearIds;
                    //attendeesQuery.CodeCampYearIds = query.CodeCampYearIds;
                }

                sessionPresenterResults = SessionPresenterManager.I.Get(sessionPresenterQuery);

                List<int> speakerIdsAll = sessionPresenterResults.Select(a => a.AttendeeId).ToList();




                speakerResults = AttendeesManager.I.Get(new AttendeesQuery()
                                                            {
                                                                Ids = speakerIdsAll
                                                            });
            }

           
                   


            IQueryable<SessionsResult> results = GetBaseResultIQueryable(baseQuery);

            List<SessionsResult> resultList = GetFinalResults(results, query);

            var tagsResults = new List<TagsResult>();
             var sessionTagsManagers = new List<SessionTagsResult>();

            if (query.WithTags != null && query.WithTags.Value)
            {
                tagsResults = TagsManager.I.GetAll(); // could be smarter, but not that many tags
                sessionTagsManagers = SessionTagsManager.I.GetAll();
            }




            //// next several lines are just for the single speaker that is associated with the session. not the multiple speaker option
            //List<int> speakerIds = (resultList.Select(data => data.Attendeesid)).ToList();
            //var presentersQuery = from attend in meta.Attendees
            //                      where speakerIds.Contains(attend.Id)
            //                      select new
            //                      {
            //                          attend.Id,
            //                          SpeakerName = attend.UserFirstName + " " + attend.UserLastName,
            //                          attend.UserWebsite,
            //                          attend.PKID
            //                      };
            //var speakerDict = 
            //    presentersQuery.ToDictionary(presenter => presenter.Id, presenter => presenter.SpeakerName);
            //var speakerUrlDict = new Dictionary<int, string>();
            //var speakerImageUrl = new Dictionary<int, string>();
            //foreach (var presenter in presentersQuery)
            //{
            //    speakerUrlDict.Add(presenter.Id, presenter.UserWebsite);
            //    speakerImageUrl.Add(presenter.Id,presenter.PKID.ToString());
            //}



            var lectureRoomsDict = (from data in meta.LectureRooms select data).ToDictionary(k => k.Id, v => v.Number);
            var sessionTimesDict = (from data in meta.SessionTimes select data).ToDictionary(k => k.Id, v => v.StartTimeFriendly);

            var planCountsDict = (from data in meta.SessionAttendee
                                  where data.Interestlevel == 3
                                  group data by data.Sessions_id
                                      into g
                                      orderby g.Key ascending
                                      select new { cnt = g.Count(), id = g.Key }).ToDictionary(k => k.id, v => v.cnt);

            var interestCountsDict = (from data in meta.SessionAttendee
                                  where data.Interestlevel == 2
                                  group data by data.Sessions_id
                                      into g
                                      orderby g.Key ascending
                                      select new { cnt = g.Count(), id = g.Key }).ToDictionary(k => k.id, v => v.cnt);


            var sessionTimesFullDict = new Dictionary<int, SessionTimes>();
            if (query.WithSchedule != null)
            {
                sessionTimesFullDict =
                    (from data in meta.SessionTimes
                     select data).ToDictionary(k => k.Id, v => v);
            }

            foreach (var session in resultList)
            {
                if (query.WithSchedule != null && session.SessionTimesId.HasValue)
                {
                    if (sessionTimesFullDict.ContainsKey(session.SessionTimesId.Value))
                    {
                        var st = sessionTimesFullDict[session.SessionTimesId.Value];
                        session.SessionTimesResult = new SessionTimesResult()
                        {
                            // no doing all the parameters, not necessary
                            CodeCampYearId = st.CodeCampYearId,
                            StartTime = st.StartTime,
                            EndTime = st.EndTime,
                            Id = st.Id,
                            Description = st.Description
                        };
                    }
                }

                if (query.WithInterestOrPlanToAttend != null)
                {
                    if (sessionIdsInterested.Contains(session.Id))
                    {
                        session.LoggedInUserInterested = true;
                    }

                    if (sessionIdsPlanToAttend.Contains(session.Id))
                    {
                        session.LoggedInUserPlanToAttend = true;
                    }
                }

                if (session.Createdate != null)
                {
                    var ts = DateTime.Now.Subtract(session.Createdate.Value);
                    session.SessionPosted = (ts.Days + 1).ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    session.SessionPosted = "Up";
                }

                // http://localhost:5691/Web/DisplayImage.ashx?sizex=100&PKID=33a199dd-1154-45cb-bc94-40ffc5a99391
                // (the next 3 lines maybe overwritten if speaker is set to be removed)

                // GOING FORWARD WE DON'T WANT TO USE SINGLE PICTURE FOR SESSIONS SPEAKER SINCE CAN HAVE MULTIPLE SPEAKERS
                //session.SpeakerPictureUrl = String.Format("DisplayImage.ashx?sizex=150&PKID={0}", speakerImageUrl[session.Attendeesid]);
                //session.PresenterName = speakerDict[session.Attendeesid];
                //session.PresenterURL = speakerUrlDict[session.Attendeesid];
                session.SpeakerPictureUrl = "DO NOT USE, USE SpeakersList instead";
                session.PresenterName = "DO NOT USE, USE SpeakersList instead";
                session.PresenterURL = "DO NOT USE, USE SpeakersList instead";


                if (query.WithSpeakers != null && query.WithSpeakers.Value)
                {
                    SessionsResult session1 = session;
                    var speakerIdsForList =
                        sessionPresenterResults.Where(a => a.SessionId == session1.Id).Select(a => a.AttendeeId).ToList();

                    // quick and dirty cleansing of speaker data so just public data will be shown
                    var tempSpeakerResults =
                        speakerResults.Where(a => speakerIdsForList.Contains(a.Id))
                                      .OrderBy(a => a.UserLastName.ToUpper());
                    session.SpeakersList = new List<AttendeesResult>();
                    foreach (var rec in tempSpeakerResults)
                    {
                        //// need to figure out if removing primary speaker is necessary
                        //if (session.DoNotShowPrimarySpeaker)
                        //{
                        //    string userNameOfSpeakerFromSession = session.Username;
                        //    string userNameOfSpeakerFromSessionSpeaker = rec.Username;
                        //    if (!userNameOfSpeakerFromSession.Equals(userNameOfSpeakerFromSessionSpeaker))
                        //    {
                        //        session.SpeakersList.Add(new AttendeesResult()
                        //        {
                        //            Id = rec.Id,
                        //            Email = rec.Email,
                        //            TwitterHandle = rec.TwitterHandle,
                        //            Username = rec.Username,
                        //            City = rec.City,
                        //            State = rec.State,
                        //            UserBio = rec.UserBio,
                        //            UserFirstName = rec.UserFirstName,
                        //            UserLastName = rec.UserLastName,
                        //            UserZipCode = rec.UserZipCode,
                        //            UserWebsite = rec.UserWebsite,
                        //            SpeakerPictureUrl =
                        //                String.Format(
                        //                    String.Format("attendeeimage/{0}.jpg", rec.Id),
                        //                    rec.Id)
                        //        });

                        //        // kind of klugy if there are more than 2 speakers, but for now, we should push someone who is not the primary speaker
                        //        // into the primary slot.
                        //        session.Username = "DO NOT USE THIS. USE SpeakersList instead";
                        //        session.PresenterName = "DO NOT USE THIS. USE SpeakersList instead";
                        //        session.PresenterURL = rec.UserWebsite;
                        //        session.SpeakerPictureUrl = "DO NOT USE THIS. USE SpeakersList instead";
                        //    }
                        //}
                        //else
                        //{
                            var attendeeResult = new AttendeesResult()
                                                          {
                                                              Id = rec.Id,
                                                              Email = rec.Email,
                                                              TwitterHandle = rec.TwitterHandle,
                                                              Username = rec.Username,
                                                              City = rec.City,
                                                              State = rec.State,
                                                              UserBio = rec.UserBio,
                                                              UserFirstName = rec.UserFirstName,
                                                              UserLastName = rec.UserLastName,
                                                              UserZipCode = rec.UserZipCode,
                                                              UserWebsite = rec.UserWebsite,
                                                              SpeakerPictureUrl =
                                                                  String.Format(
                                                                      String.Format("attendeeimage/{0}.jpg", rec.Id),
                                                                      rec.PKID)
                                                          };

                            session.SpeakersList.Add(attendeeResult);
                        //}
                    }

                    // need to update speakersshort
                    
                    var sbSpeakersShort = new StringBuilder();
                    if (session.SpeakersList.Count > 0 && session.SpeakersList.Count <= 1)
                    {
                        session.SpeakersShort = session.SpeakersList[0].UserFirstName + " " +
                                                session.SpeakersList[0].UserLastName;
                    }
                    else if (session.SpeakersList.Count > 1)
                    {
                        foreach (var speaker in session.SpeakersList)
                        {
                            sbSpeakersShort.Append(speaker.UserLastName);
                            sbSpeakersShort.Append(", ");
                        }
                        session.SpeakersShort = sbSpeakersShort.ToString().Trim();
                        if (session.SpeakersShort.Length > 2)
                        {
                            session.SpeakersShort.Remove(session.SpeakersShort.Length - 2);
                        }
                    }
                    else
                    {
                        session.SpeakersShort = "Unknown Speaker";
                    }
                    //session.SpeakersList = speakerResults.Where(a => speakerIdsForList.Contains(a.Id)).ToList();
                }




                session.RoomNumber = session.LectureRoomsId != null &&
                                     lectureRoomsDict.ContainsKey(session.LectureRoomsId.Value)
                                         ? lectureRoomsDict[session.LectureRoomsId.Value]
                                         : "ROOM NOT FOUND/PROBLEM!";
                session.RoomNumberNew = session.RoomNumber;

                session.SessionTime = session.SessionTimesId != null &&
                                      sessionTimesDict.ContainsKey(session.SessionTimesId.Value)
                                          ? sessionTimesDict[session.SessionTimesId.Value]
                                          : "TIME NOT FOUND/PROBLEM!";


                session.TitleWithPlanAttend = planCountsDict.ContainsKey(session.Id)
                                                  ? string.Format("PA: {0} {1}  ", planCountsDict[session.Id],
                                                                  session.Title)
                                                  : "PS: 0   " + session.Title;
                if (planCountsDict.ContainsKey(session.Id))
                {
                    session.PlanAheadCount = planCountsDict[session.Id].ToString();
                    session.PlanAheadCountInt = planCountsDict[session.Id];
                }
                else
                {
                    session.PlanAheadCount = "SessionId: " + session.Id + " Not Found";
                    session.PlanAheadCountInt = 0;
                }

                if (interestCountsDict.ContainsKey(session.Id))
                {
                    session.InterestCount = interestCountsDict[session.Id].ToString();
                    session.InterestCountInt = interestCountsDict[session.Id];
                }
                else
                {
                    session.PlanAheadCount = "SessionId: " + session.Id + " Not Found";
                    session.PlanAheadCountInt = 0;
                }

                if (query.WithTags != null && query.WithTags.Value)
                {
                    List<int> tagIds = sessionTagsManagers.Where(a=>a.SessionId == session.Id).Select(a => a.TagId).ToList();
                    session.TagsResults = (tagsResults.Where(data => tagIds.Contains(data.Id))).ToList();
                }

                session.SessionSlug = GenerateSlug(session.Title); // ORM has no access to this function so need to do it here
                session.TitleEllipsized = GetTitleEllipsized(session.Title, 48, "...");
            }

            return resultList;
        }

        private string GenerateSlug(string phrase)
        {
            string str = RemoveAccent(phrase).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        private static string RemoveAccent(string txt)
        {
            byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return Encoding.ASCII.GetString(bytes);
        }

        private string GetTitleEllipsized(string text, int characterCount, string ellipsis)
        {
            var cleanTailRegex = new Regex(@"\s+\S*$");

            if (string.IsNullOrEmpty(text) || characterCount < 0 || text.Length <= characterCount)
                return text;

            return cleanTailRegex.Replace(text.Substring(0, characterCount + 1), "") + ellipsis;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<SessionsResult> GetAll()
        {
            return Get(new SessionsQuery { IsMaterializeResult = true });
        }
    }
}
