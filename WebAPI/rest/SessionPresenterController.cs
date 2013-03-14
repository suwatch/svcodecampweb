using CodeCampSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.rest
{
    public class SessionPresenterController : ApiController
    {
       
        // GET rest/sessionpresenter/
        public HttpResponseMessage Get(int? codeCampYearId,int? sessionId,int? attendeesId)
        {
            var sessionPresenterQuery = new SessionPresenterQuery()
                                            {
                                                WithTitle = true
                                            };
            if (!codeCampYearId.HasValue || codeCampYearId == -1)
            {
                sessionPresenterQuery.CodeCampYearId = Utils.CurrentCodeCampYear;
            }
            else
            {
                sessionPresenterQuery.CodeCampYearId = codeCampYearId.Value;
            }
            if (sessionId.HasValue && sessionId.Value != -1)
            {
                sessionPresenterQuery.SessionId = sessionId.Value;
            }
            if (attendeesId.HasValue && attendeesId.Value != -1)
            {
                sessionPresenterQuery.AttendeeId = attendeesId.Value;
            }

            var sessions =
                SessionPresenterManager.I.Get(sessionPresenterQuery);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, sessions);
            return response;
            
        }

        // GET api/sessionpresenter/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/sessionpresenter
        public void Post([FromBody]string value)
        {
        }

        // PUT api/sessionpresenter/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/sessionpresenter/5
        public void Delete(int id)
        {
        }
    }
}
