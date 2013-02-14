using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using CodeCampSV;
using System.Net.Http.Formatting;

namespace WebAPI.Api
{
    public class AccountController : ApiController
    {

        public class LoginCredentials
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public bool RememberMe { get; set; }
        }

        public class LoginReturnStatus
        {
            public string Status { get; set; }
            public string Message { get; set; }
            public AttendeesResult Data { get; set; }
        }


        [HttpPost]
        [ActionName("Login")]
        public HttpResponseMessage PostLogin(LoginCredentials login)
        {
            var loginReturnStatus =
                new LoginReturnStatus();

           HttpResponseMessage response;
           if (!String.IsNullOrEmpty(login.Username) && !String.IsNullOrEmpty(login.Password))
           {
               var loginSuccess = Membership.ValidateUser(login.Username, login.Password);
               if (loginSuccess)
               {
                   FormsAuthentication.SetAuthCookie(login.Username,login.RememberMe);
               }
               AttendeesResult attendeesResultFull = AttendeesManager.I.Get(new AttendeesQuery()
                                                                                {
                                                                                    Username = login.Username
                                                                                }).FirstOrDefault();
               if (attendeesResultFull != null)
               {
                   var attendeesResult = AttendeesResultStripped(attendeesResultFull);
                   loginReturnStatus.Data = attendeesResult;
                   response = Request.CreateResponse(HttpStatusCode.OK, attendeesResult);
               }
               else
               {
                   response =
                       Request.CreateErrorResponse(HttpStatusCode.Forbidden,
                                                   "User Authenticated, but no user record in database found.");
               }
           }
           else
           {
               response =
                  Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Username and Password must both have values");
               loginReturnStatus.Status = "Failed";
               loginReturnStatus.Message = "Username and Password must both have values";
           }

            return response;
        }


        [HttpPost]
        [ActionName("IsLoggedIn")]
        public HttpResponseMessage PostIsLoggedIn(LoginCredentials login)
        {
            var loginReturnStatus =
                new LoginReturnStatus();

            HttpResponseMessage response;

            if (User.Identity.IsAuthenticated)
            {
                var attendeesResultFull = AttendeesManager.I.Get(new AttendeesQuery()
                {
                    Username = login.Username
                }).FirstOrDefault();
                if (attendeesResultFull != null)
                {
                    var attendeesResult = AttendeesResultStripped(attendeesResultFull);
                    loginReturnStatus.Data = attendeesResult;
                    response = Request.CreateResponse(HttpStatusCode.OK, attendeesResult);
                }
                else
                {
                    response =
                        Request.CreateErrorResponse(HttpStatusCode.Forbidden,
                                                    "User Authenticated, but no user record in database found.");
                }
            }
            else
            {
                response =
                  Request.CreateErrorResponse(HttpStatusCode.Forbidden, "User Not Authenticated To Server");
                loginReturnStatus.Status = "Failed";
                loginReturnStatus.Message = "Not Authenticated";
            }
            return response;
        }

        private static AttendeesResult AttendeesResultStripped(AttendeesResult attendeesResultFull)
        {
            var attendeesResult =
                new AttendeesResult
                    {
                        Username = attendeesResultFull.Username,
                        UserFirstName = attendeesResultFull.UserFirstName,
                        UserLastName = attendeesResultFull.UserLastName,
                        PKID = attendeesResultFull.PKID,
                        Id = attendeesResultFull.Id,
                        Email = attendeesResultFull.Email
                    };
            return attendeesResult;
        }
    }
}
