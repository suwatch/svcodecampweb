using CodeCampSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.rest
{
    public class ReturnMessage
    {
        public string Message { get; set; }

        
    }



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

     
        // POST api/sessionpresenter
        /// <summary>
        /// insert
        /// </summary>
        /// <param name="sessionPresenterResult"></param>
        /// <returns></returns>
        public HttpResponseMessage Post(SessionPresenterResult sessionPresenterResult)
        {
            HttpResponseMessage response;
            
            // block sessions that are over limit
            var numberSessionsAllowed = AttendeesManager.I.Get(new AttendeesQuery()
                {
                    Id = sessionPresenterResult.AttendeeId
                }).Count;

            var numberSessionsApprovedThisYear = SessionPresenterManager.I.Get(new SessionPresenterQuery()
                {
                    AttendeeId = sessionPresenterResult.AttendeeId,
                    CodeCampYearId = Utils.GetCurrentCodeCampYear()
                }).Count();

            if (numberSessionsApprovedThisYear < numberSessionsAllowed && !Utils.CheckUserIsAdmin())
            {
                var spr = new SessionPresenterResult()
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
                response = Request.CreateResponse(HttpStatusCode.OK, rec);
            }
            else
            {

                response = Request.CreateResponse(HttpStatusCode.ExpectationFailed,
                                                  "Over Session Submit Limit or Session Submission closed");
            }
            return response;
        }


        // THIS TABLE WE DO NOT UPDATE THAT I KNOW OF
        //// PUT api/sessionpresenter/5
        ///// <summary>
        ///// update
        ///// </summary>
        ///// <param name="sessionPresenterResult"></param>
        ///// <returns></returns>
        //public HttpResponseMessage Put(SessionPresenterResult sessionPresenterResult)
        //{
        //    // block sessions that are over limit
        //    var numberSessionsAllowed =  AttendeesManager.I.Get(new AttendeesQuery()
        //        {
        //            Id = sessionPresenterResult.AttendeeId
        //        }).Count;

        //    var rec = SessionPresenterManager.I.Get(new SessionPresenterQuery()
        //    {

        //       // Id = spr.Id
        //    });


        //    var spr = new SessionPresenterResult
        //        {
        //            SessionId = sessionPresenterResult.SessionId,
        //            AttendeeId = sessionPresenterResult.AttendeeId
        //        };
        //    SessionPresenterManager.I.Insert(spr);


        //    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, spr);
        //    return response;


        //}

        // DELETE api/sessionpresenter/5
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage response;
            response = Request.CreateResponse(HttpStatusCode.NotImplemented);
            return response;
        }
    }
}
