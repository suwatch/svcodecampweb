using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using CodeCampSV;
using System.ComponentModel;
using DataAccess.Code;

namespace CodeCampSV
{
    public class SpeakerResult
    {
        [DataMember]
        public int AttendeeId { get; set; }

        [DataMember]
        public Guid PKID { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string UserWebsite { get; set; }

        [DataMember]
        public string UserLocation { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public string UserFirstName { get; set; }

        [DataMember]
        public string UserLastName { get; set; }

        [DataMember]
        public string UserZipCode { get; set; }

        [DataMember]
        public string UserBio { get; set; }

        [DataMember]
        public string UserBioEllipsized { get; set; }

        [DataMember]
        public bool? SaturdayClasses { get; set; }

        [DataMember]
        public bool? SundayClasses { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string AddressLine1 { get; set; }

        [DataMember]
        public string ShirtSize { get; set; }

        [DataMember]
        public int? EmailSubscription { get; set; }

        [DataMember]
        public string TwitterHandle { get; set; }

        [DataMember]
        public string ImageUrl { get; set; }

        [DataMember]
        public string SpeakerLocalUrl { get; set; }

        [DataMember]
        public List<SessionPresentResultSmall> Sessions { get; set; }
    }

    public class SessionPresentResultSmall
    {
        public int SessionId { get; set; }
        public int CodeCampYearId { get; set; }
        public int AttendeeId { get; set; }
        public int PrimarySpeakerId { get; set; }
        public bool DoNotShowPrimarySpeaker { get; set; }

        public string Title { get; set; }
        public string TitleEllipsized { get; set; }
        public string RoomNumber { get; set; }
        public string SessionTime { get; set; }
        public string Description { get; set; }
        public string DescriptionEllipsized { get; set; }
        public string SessionUrl { get; set; }
    }

    public partial class AttendeesManager
    {
        public List<SpeakerResult> GetSpeakerResults(AttendeesQuery query)
        {
            List<AttendeesResult> recs = Get(query);
            var speakers = new List<SpeakerResult>();
            recs.ForEach(a => speakers.Add(new SpeakerResult()
                                               {
                                                   AttendeeId = a.Id,
                                                   PKID = a.PKID,
                                                   Username = a.Username,
                                                   UserWebsite = a.UserWebsite,
                                                   UserLocation = a.UserLocation,
                                                   UserFirstName = a.UserFirstName,
                                                   UserLastName = a.UserLastName,
                                                   UserZipCode = a.UserZipCode,
                                                   UserBio = a.UserBio,
                                                   UserBioEllipsized = Utils.GetEllipsized(a.UserBio, 90, "..."),
                                                   SaturdayClasses = a.SaturdayClasses,
                                                   SundayClasses = a.SundayClasses,
                                                   PhoneNumber = a.PhoneNumber,
                                                   AddressLine1 = a.AddressLine1,
                                                   EmailSubscription = a.EmailSubscription,
                                                   TwitterHandle = a.TwitterHandle,
                                                   ImageUrl = String.Format("/attendeeimage/{0}.jpg", a.Id),
                                                   Sessions = a.Sessions,


                                                   SpeakerLocalUrl =
                                                       String.Format("/Speaker/Detail/{0}-{1}-{2}", a.UserFirstName,
                                                                     a.UserLastName, a.Id)
                                               }));
            return speakers;
        }

        public List<AttendeesResult> Get(AttendeesQuery query)
        {

            var meta = new CodeCampDataContext();


            // add to codecampyearids (make sure List is always populated)
            if (query.CodeCampYearId.HasValue)
            {
                if (query.CodeCampYearIds != null && query.CodeCampYearIds.Count > 0)
                {
                    if (!query.CodeCampYearIds.Contains(query.CodeCampYearId.Value))
                    {
                        query.CodeCampYearIds.Add(query.CodeCampYearId.Value);
                    }
                }
                else
                {
                    query.CodeCampYearIds = new List<int> {query.CodeCampYearId.Value};
                }
            }
            // query.CodeCampYearId should not be used for hear on out, just the array

            IQueryable<Attendees> baseQuery = from myData in meta.Attendees select myData;

            if (query.Emails != null && query.Emails.Count > 0)
            {
                baseQuery = baseQuery.Where(data => query.Emails.Contains(data.Email));
            }

            if (!String.IsNullOrEmpty(query.SpeakerNameWithId))
            {
                // looking for "Peter-Kellner-903"
                List<string> parts = query.SpeakerNameWithId.Split(new[] {'-'}).ToList();
                if (parts.Count == 3)
                {
                    int attendeeId;
                    if (Int32.TryParse(parts[2], out attendeeId))
                    {
                        if (parts[0].Length > 0 && parts[1].Length > 0)
                        {
                            // paydirt!
                            baseQuery = baseQuery.Where(a => a.UserFirstName.ToLower() == parts[0].ToLower() &&
                                                             a.UserLastName.ToLower() == parts[1].ToLower() &&
                                                             a.Id == attendeeId);

                        }
                    }
                }
            }


            if (query.CodeCampYearIds != null && query.CodeCampYearIds.Count > 0)
            {
                if (query.PresentersOnly != null && query.PresentersOnly.Value)
                {
                    var speakerIds = (from sessionPresenter in meta.SessionPresenter
                                      join session in meta.Sessions on sessionPresenter.SessionId equals session.Id
                                      where query.CodeCampYearIds.Contains(session.CodeCampYearId)
                                      select sessionPresenter.AttendeeId).ToList();
                    baseQuery = baseQuery.Where(data => speakerIds.Contains(data.Id));
                }
                else
                {
                    // this may blow up with two many attendees in contains (which translates to sql IN)
                    var speakerIds = (from data in meta.AttendeesCodeCampYear
                                      where query.CodeCampYearIds.Contains(data.CodeCampYearId)
                                      select data.AttendeesId).ToList();
                    baseQuery = baseQuery.Where(data => speakerIds.Contains(data.Id));
                }
            }
            else
            {
                // if codecampyear not specified, thenw we need to deal with presentersonly separately
                if (query.PresentersOnly != null)
                {
                    var presenterAttendeeIds = from data in meta.SessionPresenter
                                               select data.AttendeeId;
                    baseQuery = baseQuery.Where(a => presenterAttendeeIds.Contains(a.Id));
                }
            }


            //  next is automated query formation from AutoGen Shared Class 
            //  (do not remove next line or filters will go away)
            baseQuery = BaseQueryAutoGen(baseQuery, query);

            if (query.EmailContains != null)
            {
                baseQuery = baseQuery.Where(a => a.Email.Contains(query.EmailContains));
            }



            if (query.AttendeesOnlyNoPresenters != null)
            {
                var presenterAttendeIds = from data in meta.SessionPresenter
                                          select data.AttendeeId;
                baseQuery = baseQuery.Where(a => !presenterAttendeIds.Contains(a.Id));
            }

            IQueryable<AttendeesResult> results = GetBaseResultIQueryable(baseQuery);


            var resultList = new List<AttendeesResult>();

            // NOT NEESSARY BECAUSE CODECAMPYEARS FILTERED ABOVE
            //if (query.CodeCampYearIds != null && query.CodeCampYearIds.Count > 0)
            //{
            //    var attendeeIds = (from data in meta.AttendeesCodeCampYear
            //                       where query.CodeCampYearIds.Contains(data.CodeCampYearId)
            //                       select data.AttendeesId).ToList();

            //    // can't use contains because list will be to long. need to do it one by one sadly
            //    var resultListTemp = GetFinalResults(results, query);
            //    resultList.AddRange(resultListTemp.Where(rec => attendeeIds.Contains(rec.Id)));
            //}
            //else
            //{
            resultList = GetFinalResults(results, query);
            //}

            var sessionsBySpeakerdict = new Dictionary<int, List<SessionPresentResultSmall>>();
            if (query.IncludeSessions.HasValue && query.IncludeSessions.Value)
            {
                IQueryable<SessionPresentResultSmall> sessionPresentResultSmalls =
                    from sessionPresenter in meta.SessionPresenter
                    join session in meta.Sessions on sessionPresenter.SessionId equals session.Id
                    join roomdata in meta.LectureRooms on session.LectureRoomsId equals roomdata.Id
                    join timedata in meta.SessionTimes on session.SessionTimesId equals timedata.Id
                    select new SessionPresentResultSmall
                               {
                                   SessionId = session.Id,
                                   CodeCampYearId = session.CodeCampYearId,
                                   AttendeeId = sessionPresenter.AttendeeId,
                                   PrimarySpeakerId = session.Attendeesid,
                                   DoNotShowPrimarySpeaker = session.DoNotShowPrimarySpeaker,
                                   Title = session.Title,
                                   TitleEllipsized = Utils.GetEllipsized(session.Title, 45, "..."),
                                   RoomNumber = roomdata.Number,
                                   SessionTime = timedata.StartTimeFriendly,
                                   Description = session.Description,
                                   DescriptionEllipsized = Utils.GetEllipsized(session.Description, 75, "..."),
                                   SessionUrl =
                                       String.Format("/Session/{0}/{1}",
                                                     Utils.ConvertCodeCampYearToActualYear(
                                                         session.CodeCampYearId.ToString(CultureInfo.InvariantCulture)),
                                                     Utils.GenerateSlug(session.Title)),
                               };

                if (query.CodeCampYearIds != null && query.CodeCampYearIds.Count > 0)
                {
                    sessionPresentResultSmalls =
                        sessionPresentResultSmalls.Where(a => query.CodeCampYearIds.Contains(a.CodeCampYearId));
                }


                foreach (var rec in sessionPresentResultSmalls)
                {
                    if (!sessionsBySpeakerdict.ContainsKey(rec.AttendeeId))
                    {
                        // no speakers in session dictionary so add it from scarch
                        sessionsBySpeakerdict.Add(rec.AttendeeId, new List<SessionPresentResultSmall>
                                                                      {
                                                                          rec
                                                                      });
                    }
                    else
                    {
                        // add one more speaker to this session
                        List<SessionPresentResultSmall> sessionPresentResultSmallsTemp =
                            sessionsBySpeakerdict[rec.AttendeeId];
                        sessionPresentResultSmallsTemp.Add(rec);
                        sessionsBySpeakerdict[rec.AttendeeId] = sessionPresentResultSmallsTemp;

                    }
                }

            }

            if (query.IncludeSessions.HasValue && query.IncludeSessions.Value)
            {
                foreach (var attendee in resultList)
                {
                    attendee.SessionIds = new List<int>();
                    attendee.Sessions = new List<SessionPresentResultSmall>();

                    List<SessionPresentResultSmall> sessionPresentResultSmallsTemp = sessionsBySpeakerdict[attendee.Id];
                    if (sessionPresentResultSmallsTemp != null && sessionPresentResultSmallsTemp.Count > 0)
                    {
                        // check and see if this person owns the session AND there name is to be supressed

                        var sessionPresentResultSmallsFiltered = new List<SessionPresentResultSmall>();
                        foreach (var recc in sessionPresentResultSmallsTemp)
                        {
                            bool supressSpeaker = recc.DoNotShowPrimarySpeaker &&
                                                  recc.PrimarySpeakerId == recc.AttendeeId;
                            if (!supressSpeaker)
                            {
                                sessionPresentResultSmallsFiltered.Add(recc);
                            }
                        }
                        attendee.SessionIds = sessionPresentResultSmallsTemp.Select(a => a.SessionId).ToList();
                        attendee.Sessions = sessionPresentResultSmallsTemp.ToList();
                    }
                }
            }

            // blank out info not to be shown
            if (query.RespectQRCodes != null && query.RespectQRCodes.Value)
            {
                foreach (var rec in resultList)
                {
                    rec.SpeakerPictureUrl =
                        String.Format("http://www.siliconvalley-codecamp.com/DisplayImage.ashx?PKID={0}", rec.PKID);

                    if (!(rec.QRAddressLine1Allow != null && rec.QRAddressLine1Allow.Value))
                    {
                        rec.AddressLine1 = string.Empty;
                    }

                    if (!(rec.QREmailAllow != null && rec.QREmailAllow.Value))
                    {
                        rec.Email = string.Empty;
                    }

                    if (!(rec.QRPhoneAllow != null && rec.QRPhoneAllow.Value))
                    {
                        rec.PhoneNumber = string.Empty;
                    }

                    if (!(rec.QRWebSiteAllow != null && rec.QRWebSiteAllow.Value))
                    {
                        rec.UserWebsite = string.Empty;
                    }

                    if (!(rec.QRZipCodeAllow != null && rec.QRZipCodeAllow.Value))
                    {
                        rec.UserZipCode = string.Empty;
                    }
                    else
                    {
                        // Grab up city state from zip
                        rec.City = "CityToSet";
                        rec.State = "StateToSet";
                    }



                }
            }


            //  Put Stuff Here if you want to load another result
            //  The following is done AFTER GetFinalResults so that we don't waste machine cycles sucking in all the
            //  addresses for all results returned, just the ones that are actually being returned.
            //  if (query.WithAddress != null && query.WithAddress == true)
            //  {
            //     foreach (var r in companyResultList)
            //     {
            //         r.CompanyAddressResultList =
            //             CompanyAddressManager.I.Get(new CompanyAddressQuery { CompanyIds = query.Ids, WithAddress = true });
            //     }
            //  }
            //             
            return resultList;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<AttendeesResult> GetAll()
        {
            return Get(new AttendeesQuery {IsMaterializeResult = true});
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<AttendeesResult> GetByStringIds(string listOfAttendeeIds)
        {
            List<AttendeesResult> results = new List<AttendeesResult>();
            if (!String.IsNullOrEmpty(listOfAttendeeIds))
            {
                List<String> idsListString = listOfAttendeeIds.Split(',').ToList();
                List<int> ids = new List<int>();
                foreach (var idString in idsListString)
                {
                    int id = Convert.ToInt32(idString);
                    ids.Add(id);
                }

                results = Get(new AttendeesQuery
                                  {
                                      Ids = ids,
                                      IsMaterializeResult = true
                                  });
            }
            return results;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<AttendeesResult> GetByMisc(string misc)
        {
            var recsOri = Get(new AttendeesQuery {IsMaterializeResult = true});
            var recs = new List<AttendeesResult>();

            if (!String.IsNullOrEmpty(misc))
            {

                foreach (var rec in recsOri)
                {
                    if (!String.IsNullOrEmpty(rec.UserFirstName) && rec.UserFirstName.ToLower().Equals(misc.ToLower()))
                    {
                        recs.Add(rec);
                    }
                    else if (!String.IsNullOrEmpty(rec.UserLastName) && rec.UserLastName.ToLower().Equals(misc.ToLower()))
                    {
                        recs.Add(rec);
                    }
                    else if (!String.IsNullOrEmpty(rec.Username) && rec.Username.ToLower().Equals(misc.ToLower()))
                    {
                        recs.Add(rec);
                    }
                    else if (!String.IsNullOrEmpty(rec.Email) && rec.Email.ToLower().Equals(misc.ToLower()))
                    {
                        recs.Add(rec);
                    }
                    else if (!String.IsNullOrEmpty(rec.UserFirstName) && !String.IsNullOrEmpty(rec.UserLastName))
                    {
                        string firstLast = rec.UserFirstName + " " + rec.UserLastName;
                        if (firstLast.ToLower().Equals(misc.ToLower()))
                        {
                            recs.Add(rec);
                        }
                    }

                }
            }

            return recs;
        }

    }
}
