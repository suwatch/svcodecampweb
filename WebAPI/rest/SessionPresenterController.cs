using System.Globalization;
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
        public HttpResponseMessage Get(int? codeCampYearId, int? sessionId, int? attendeesId)
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


        /// <summary>
        /// POST (insert) 
        /// </summary>
        /// <param name="sessionPresenterResult"></param>
        /// <returns></returns>
        public HttpResponseMessage Post(SessionPresenterResult sessionPresenterResult)
        {
            HttpResponseMessage response;

            if (!User.Identity.IsAuthenticated || String.IsNullOrEmpty(User.Identity.Name))
            {
                response = Request.CreateErrorResponse(HttpStatusCode.Forbidden,
                                                       "Can not create sessionPresenterResult without being logged in");
            }
            else if (sessionPresenterResult == null)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                       "no sessionPresenterResult values passed in");
            }
            else
            {
                // make sure person logged in is same as in sessionPresenterResult
                var attendeeRec =
                    AttendeesManager.I.Get(new AttendeesQuery {Id = sessionPresenterResult.AttendeeId})
                                    .FirstOrDefault();
                if (attendeeRec == null || !attendeeRec.Username.Equals(User.Identity.Name))
                {
                    response = Request.CreateErrorResponse(HttpStatusCode.Forbidden,
                                                           "user logged in must be user assigned to session presenter attendeeId");
                }
                else
                {
                    string message;
                    bool canPresent = Utils.GetSpeakerCanPresent(sessionPresenterResult.AttendeeId, out message);
                    if (canPresent)
                    {
                        var spr = new SessionPresenterResult
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
                        response = Request.CreateErrorResponse(HttpStatusCode.NotAcceptable,
                                                               "Over Session Submit Limit or Session Submission closed");
                    }
                }
            }
            return response;
        }

        // DELETE api/sessionpresenter/5
        public HttpResponseMessage Delete(int id)
        {
            HttpResponseMessage response;

            if (!User.Identity.IsAuthenticated || String.IsNullOrEmpty(User.Identity.Name))
            {
                response = Request.CreateErrorResponse(HttpStatusCode.Forbidden,
                                                       "Can not delete sessionPresenterResult without being logged in");
            }
            else
            {
                var rec = SessionPresenterManager.I.Get(new SessionPresenterQuery() {Id = id});
                if (rec != null)
                {
                    var attendeeRec =
                        AttendeesManager.I.Get(new AttendeesQuery {Id = id})
                                        .FirstOrDefault();
                    if (attendeeRec == null || !attendeeRec.Username.Equals(User.Identity.Name))
                    {
                        response = Request.CreateErrorResponse(HttpStatusCode.Forbidden,
                                                               "user logged in must be user assigned to session presenter attendeeId");
                    }
                    else
                    {
                        SessionPresenterManager.I.Delete(id);
                        response = Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
                else
                {
                    response = Request.CreateErrorResponse(HttpStatusCode.Forbidden,
                                                       "no valid sessionPresenterRecord found for passed in id " + id.ToString(CultureInfo.InvariantCulture));
           
                }
            }
            return response;
        }
    }
}
