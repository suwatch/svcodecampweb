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

        //// POST api/session
        //public void Post([FromBody]string value)
        //{
        //}

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

            session.Title = sessionsResult.Title;
            session.Description = sessionsResult.Description;



            SessionsManager.I.Update(session);



            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, sessionsResult);
            return response;



        }

        //// DELETE api/session/5
        //public void Delete(int id)
        //{
        //}
    }
}
