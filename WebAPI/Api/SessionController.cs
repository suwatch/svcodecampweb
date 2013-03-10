using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CodeCampSV;

namespace WebAPI.Api
{
    public class SessionController : ApiController
    {

        [HttpPut]
        public void PutSession(SessionsResult sessionResult)
        {
            var rec = SessionsManager.I.Get(new SessionsQuery()
                                                {
                                                    Id = sessionResult.Id
                                                }).FirstOrDefault();
            if (rec != null)
            {
                rec.Title = sessionResult.Title;
                SessionsManager.I.Update(rec);
            }
        }


        [HttpGet]
        // http://localhost:17138/api/Session/GetAll
        public IEnumerable<SessionsResult> GetAll()
        {


            var sessions = SessionsManager.I.Get(new SessionsQuery()
            {
                CodeCampYearId = Utils.CurrentCodeCampYear,
                WithSpeakers = true,
                WithInterestOrPlanToAttend = true
            });

            return sessions.ToList();
        }

        public SessionsResult Get(int year,string session)
        {
          

            var sessionInstance = SessionsManager.I.Get(new SessionsQuery()
                                                     {
                                                         CodeCampYearId = year
                                                     }).FirstOrDefault();

            return sessionInstance;


        }

        /// <summary>
        /// get speakers based on code camp year and attendeesId. No reason to protect this, sessions by speaker is public info
        /// </summary>
        /// <param name="codeCampYearId"></param>
        /// <param name="attendeesId"></param>
        /// <returns></returns>
        public HttpResponseMessage GetBySpeaker(int codeCampYearId, int attendeesId)
        {

            if (codeCampYearId == -1)
            {
                codeCampYearId = Utils.CurrentCodeCampYear;
            }

            var sessions =
                SessionsManager.I.Get(new SessionsQuery
                                          {
                                              CodeCampYearId =codeCampYearId,
                                              Attendeesid = attendeesId
                                          });

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, sessions);
            return response;
        }



        //// GET api/session
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/session/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/session
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/session/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/session/5
        //public void Delete(int id)
        //{
        //}
    }
}
