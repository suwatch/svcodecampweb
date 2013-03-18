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
        public HttpResponseMessage Post(SessionPresenterResult sessionPresenterResult)
        {
            SessionPresenterResult spr = new SessionPresenterResult()
            {
                AttendeeId = sessionPresenterResult.AttendeeId,
                SessionId = sessionPresenterResult.SessionId
            };
            SessionPresenterManager.I.Insert(spr);

            var rec = SessionPresenterManager.I.Get(new SessionPresenterQuery()
            {
                Id = spr.Id,
                WithTitle = true
            });


            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, rec);
            return response;
        }

        // PUT api/sessionpresenter/5
        public HttpResponseMessage Put(SessionPresenterResult sessionPresenterResult)
        {
            var spr = new SessionPresenterResult()
            {
                SessionId = sessionPresenterResult.SessionId,
                AttendeeId = sessionPresenterResult.AttendeeId
            };
            SessionPresenterManager.I.Insert(spr);


            HttpResponseMessage response;
            response = Request.CreateResponse(HttpStatusCode.OK, spr);
            return response;


        }

        // DELETE api/sessionpresenter/5
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage response;
            response = Request.CreateResponse(HttpStatusCode.NotImplemented);
            return response;
        }
    }
}
