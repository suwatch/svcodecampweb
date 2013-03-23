using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CodeCampSV;

namespace WebAPI.REST
{
    public class SessionController : ApiController
    {

        /// <summary>
        /// route like implementation of Get.  If no parameters then get all sessions.  
        /// option = BySpeaker with param1 = "#" gives all sessions for given speaker
        /// </summary>
        /// <param name="option"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <returns></returns>
        public HttpResponseMessage Get(string option,string param1,string param2,string param3)
        {
            HttpResponseMessage response = null;

            SessionsQuery sessionQuery = null;
           

            if (String.IsNullOrEmpty(option))
            {
                sessionQuery = new SessionsQuery();
                response = Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Did not pass in option variable");
            }
          
            else if (option.ToLower().Equals("byspeaker") && !String.IsNullOrEmpty(param1))
            {
                int attendeesId;
                if (!Int32.TryParse(param1, out attendeesId))
                {
                    attendeesId = -2;
                }

                int codeCampYearId; // current year
                if (!Int32.TryParse(param2, out codeCampYearId))
                {
                    codeCampYearId = -1;
                }

                if (codeCampYearId == -1)
                {
                    codeCampYearId = Utils.GetCurrentCodeCampYear();
                }


                List<int> sessionIds = SessionPresenterManager.I.Get(new SessionPresenterQuery
                                                                                {
                                                                                    AttendeeId = attendeesId,
                                                                                    CodeCampYearId = codeCampYearId
                                                                                }).Select(a => a.SessionId).ToList();
                sessionQuery = new SessionsQuery
                                   {
                                      Ids = sessionIds,
                                      WithLevel = true
                                   };

               

                List<SessionsResult> session = SessionsManager.I.Get(sessionQuery);
                foreach (var rec in session)
                {
                    rec.Title = rec.Title.Trim(); // just making sure because we use this for unique stuff
                }
                response = Request.CreateResponse(HttpStatusCode.OK, session);
            }
            else if (option.ToLower().Equals("justlowercasetitle"))
            {
                List<SessionsResult> sessionsFull = SessionsManager.I.Get(new SessionsQuery()
                    {
                        CodeCampYearId = Utils.CurrentCodeCampYear
                    });

                List<SessionsResult>  sessionTitles = sessionsFull.Select(rec => new SessionsResult()
                    {
                        Id = rec.Id,
                        Title = rec.Title.ToLower().Trim()
                    }).ToList();


                response = Request.CreateResponse(HttpStatusCode.OK, sessionTitles);
            }

         
            return response;
        }
       

        public HttpResponseMessage Get(int id)
        {
            var session =
                SessionsManager.I.Get(new SessionsQuery
                                               {
                                                   Id = id
                                               }).FirstOrDefault();

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, session);
            return response;
        }

        // POST api/session
        public HttpResponseMessage Post(SessionsResult sessionsResult)
        {
            if (sessionsResult != null)
            {
                sessionsResult.Title = sessionsResult.Title.Trim();
            }

            HttpResponseMessage response;
            bool sessionAccepted = false;
            if (sessionsResult.LoggedInUserAttendeeId.HasValue)
            {
                var attendeeRec =
                    AttendeesManager.I.Get(new AttendeesQuery() {Id = sessionsResult.LoggedInUserAttendeeId})
                                    .FirstOrDefault();
                if (attendeeRec != null)
                {
                    if ((attendeeRec.PresentationApprovalRequired.HasValue &&
                         !attendeeRec.PresentationApprovalRequired.Value) || Utils.CheckUserIsAdmin())
                    {
                        sessionAccepted = true;
                    }
                }
            }


            string message;
            bool canPresent = Utils.GetSpeakerCanPresent(sessionsResult.LoggedInUserAttendeeId ?? 0, out message);
            if (canPresent)
            {
                UpdateSessionResultForSessionLevel(sessionsResult);
                var session = new SessionsResult()
                    {
                        Createdate = DateTime.UtcNow,
                        CodeCampYearId = Utils.CurrentCodeCampYear,
                        Title = sessionsResult.Title,
                        Description = sessionsResult.Description,
                        SessionLevel_id = sessionsResult.SessionLevel_id,
                        TwitterHashTags = sessionsResult.TwitterHashTags,
                        Approved = sessionAccepted,
                        LectureRoomsId = Utils.RoomNotAssigned,
                        SessionTimesId = Utils.TimeSessionUnassigned,
                    };
                SessionsManager.I.Insert(session);
                response = Request.CreateResponse(HttpStatusCode.OK, session);
            }
            else
            {
                response = Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "session submission not allowed");
            }
            return response;
        }

        private static void UpdateSessionResultForSessionLevel(SessionsResult sessionsResult)
        {
            if (sessionsResult.SessionLevel.ToLower().StartsWith("beg"))
            {
                sessionsResult.SessionLevel_id = 1;
            }
            else if (sessionsResult.SessionLevel.ToLower().StartsWith("int"))
            {
                sessionsResult.SessionLevel_id = 2;
            }
            else if (sessionsResult.SessionLevel.ToLower().StartsWith("adv"))
            {
                sessionsResult.SessionLevel_id = 3;
            }
            else
            {
                sessionsResult.SessionLevel_id = 1;
            }
        }

        /// <summary>
        ///  only let this update some safe fields.
        /// </summary>
        /// <param name="sessionsResult"></param>
        /// <returns></returns>
        public HttpResponseMessage Put(SessionsResult sessionsResult)
        {
            HttpResponseMessage response;
            if (sessionsResult != null)
            {
                sessionsResult.Title = sessionsResult.Title.Trim();

                //if (sessionsResult.Title.StartsWith("bad"))
                //{
                //    response = Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "session started with bad so erroring!");
                //    return response;
                //}

                var session =
                    SessionsManager.I.Get(new SessionsQuery
                        {
                            Id = sessionsResult.Id
                        }).FirstOrDefault();

                if (session != null)
                {
                    UpdateSessionResultForSessionLevel(sessionsResult);
                    session.Title = sessionsResult.Title;
                    session.Description = sessionsResult.Description;
                    session.TwitterHashTags = sessionsResult.TwitterHashTags;
                    session.SessionLevel_id = sessionsResult.SessionLevel_id;

                    SessionsManager.I.Update(session);

                    response = Request.CreateResponse(HttpStatusCode.OK, sessionsResult);
                }
                else
                {
                    response = Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                           "No SessionId Found For Update");
                }
            }
            else
            {
                response = Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "SessionResult passed in is null.  Not continuing");
                //    return response;
            }

            return response;
        }

        // DELETE api/session/5
        public HttpResponseMessage Delete(int id)
        {
            // need to make sure session is not accepted and/or admin rights
            var session =
             SessionsManager.I.Get(new SessionsQuery
             {
                 WithTags = true,
                 WithEvaluations = true,
                 WithInterestOrPlanToAttend = true,
                 WithLectureRoom = true,
                 Id = id
             }).FirstOrDefault();

            var sessionPresenters = SessionPresenterManager.I.Get(new SessionPresenterQuery() {SessionId = id});
            List<int> spIds = sessionPresenters.Select(a => a.Id).ToList();
            foreach (var spid in spIds)
            {
                SessionPresenterManager.I.Delete(spid);
            }

            if (session != null)
            {
                List<int> tagIds = session.TagsResults.Select(a => a.Id).ToList();
                foreach (var tagId in tagIds)
                {
                    SessionTagsManager.I.Delete(tagId);
                }
            }

            SessionsManager.I.Delete(id);

            return Request.CreateResponse(HttpStatusCode.OK);


        }
    }
}
