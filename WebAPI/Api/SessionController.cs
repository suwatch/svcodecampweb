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
        [HttpGet]
        // http://localhost:17138/api/Session/GetAll
        public IEnumerable<SessionsResult> GetAll()
        {


            var sessions = SessionsManager.I.Get(new SessionsQuery()
            {
                CodeCampYearId = 7
            });

            return sessions.ToList();
        }

        public SessionsResult Get(int year,string session)
        {


            var sessionInstance = SessionsManager.I.Get(new SessionsQuery()
                                                     {
                                                         CodeCampYearId = 7
                                                     }).FirstOrDefault();

            return sessionInstance;


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
