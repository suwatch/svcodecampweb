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
            SessionsQuery sessionQuery = null;
           

            if (String.IsNullOrEmpty(option))
            {
                sessionQuery = new SessionsQuery();
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
                                      Ids = sessionIds
                                   };


            }

            var session = new List<SessionsResult>();
            if (sessionQuery != null)
            {
                session = SessionsManager.I.Get(sessionQuery);
            }

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, session);
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
        public void Post(SessionsResult sessionsResult)
        {
        }

        /// <summary>
        ///  only let this update some safe fields.
        /// </summary>
        /// <param name="sessionsResult"></param>
        /// <returns></returns>
        public HttpResponseMessage Put(SessionsResult sessionsResult)
        {
            var session =
              SessionsManager.I.Get(new SessionsQuery
              {
                  Id = sessionsResult.Id
              }).FirstOrDefault();

            HttpResponseMessage response;
            if (session != null)
            {
                session.Title = sessionsResult.Title;
                session.Description = sessionsResult.Description;
                SessionsManager.I.Update(session);
               
                response = Request.CreateResponse(HttpStatusCode.OK, sessionsResult);
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.ExpectationFailed, "No SessionId Found For Update");
            }

            return response;
        }

        //// DELETE api/session/5
        //public void Delete(int id)
        //{
        //}
    }
}
