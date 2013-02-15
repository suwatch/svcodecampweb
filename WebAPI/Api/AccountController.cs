using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
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

        //public class UpdateAttendeeRecord
        //{
        //    public string AttendeeGroup { get; set; } // radio button, attend which days. AttendingSaturdaySundayRb;
        //    public string FirstName { get; set; }
        //    public string LastName { get; set; }
        //    public string Email { get; set; }
        //    public string TwitterHandle { get; set; }
        //    public string City { get; set; }
        //    public string State { get; set; }
        //    public string Zip { get; set; }

        //    public bool? QrAllowEmail { get; set; }
        //    public bool? QrWebSiteAllow { get; set; }
        //    public bool? QrAddressLineAllow { get; set; }
        //    public bool? QrZipCodeAllow { get; set; }
        //    public bool? QrTwitterAllow { get; set; }
           

        //}

        [HttpPost]
        [ActionName("CheckUsernameExists")]
        public HttpResponseMessage PostCheckUsernameExists(AttendeesResult attendeeRecord)
        {
            Thread.Sleep(300); // try to defend a little against denial of service attack or username searching attack
            bool attendeeUsernameExists = AttendeesManager.I.CheckAttendeeExists(attendeeRecord.Username);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK,
                                                                  attendeeUsernameExists ? "Exists" : "NotFound");
            return response;
        }

        [HttpPost]
        [ActionName("UpdateAttendee")]
        public HttpResponseMessage PostUpdateAttendee(AttendeesResult attendeeRecord)
        {
            HttpResponseMessage response;

            if (!User.Identity.IsAuthenticated)
            {
                response =
                    Request.CreateErrorResponse(HttpStatusCode.Forbidden,
                                                "User Not Logged In So Update Forbidden");
            }
            else
            {
                var attendeesResult =
                    AttendeesManager.I.Get(new AttendeesQuery
                                               {
                                                   Username = User.Identity.Name,
                                                   CodeCampYearId = Utils.CurrentCodeCampYear,
                                                   IncludeAttendeesCodeCampYearResult = true
                                               }).FirstOrDefault();
                if (attendeesResult != null)
                {
                    attendeesResult.Email = attendeeRecord.Email;
                    attendeesResult.UserFirstName = attendeeRecord.UserFirstName;
                    attendeesResult.UserLastName = attendeeRecord.UserLastName;
                    attendeesResult.City = attendeeRecord.City;
                    attendeesResult.State = attendeeRecord.State;
                    attendeesResult.UserZipCode = attendeeRecord.UserZipCode;
                    attendeesResult.TwitterHandle = attendeeRecord.TwitterHandle;
                    attendeesResult.FacebookId = attendeeRecord.FacebookId;
                    attendeesResult.GooglePlusId = attendeeRecord.GooglePlusId;
                    attendeesResult.LinkedInId = attendeeRecord.LinkedInId;
                    attendeesResult.AttendingDaysChoiceCurrentYear = attendeeRecord.AttendingDaysChoiceCurrentYear;
                    attendeesResult.RegisteredCurrentYear = attendeeRecord.RegisteredCurrentYear;

                    attendeesResult.CurrentCodeCampYear = Utils.CurrentCodeCampYear;
                    AttendeesManager.I.UpdateWithAttendeeCCY(attendeesResult);

                    response = Request.CreateResponse(HttpStatusCode.OK, attendeesResult);


                }
                else
                {
                    response =
                        Request.CreateErrorResponse(HttpStatusCode.Forbidden,
                                                    "User Authenticated but no base attendee record found");
                }



            }
            return response;
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
                   FormsAuthentication.SetAuthCookie(login.Username, login.RememberMe);

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
                 Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Username and Password are not valid.  Please Try again");
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
                var attendeesResultFull =
                    AttendeesManager.I.Get(new AttendeesQuery
                                               {
                                                   Username = User.Identity.Name,
                                                   CodeCampYearId = Utils.CurrentCodeCampYear,
                                                   IncludeAttendeesCodeCampYearResult = true
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

        [HttpPost]
        [ActionName("LogOut")]
        public HttpResponseMessage PostLogOut(LoginCredentials login)
        {
            if (User.Identity.IsAuthenticated)
            {
               FormsAuthentication.SignOut();
            }
            return Request.CreateResponse(HttpStatusCode.OK, "");
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
                        Email = attendeesResultFull.Email,
                        City = attendeesResultFull.City,
                        State = attendeesResultFull.State,
                        UserZipCode = attendeesResultFull.UserZipCode,
                        TwitterHandle = attendeesResultFull.TwitterHandle,
                        FacebookId = attendeesResultFull.FacebookId,
                        GooglePlusId = attendeesResultFull.GooglePlusId,
                        LinkedInId = attendeesResultFull.LinkedInId,
                        EmailEventBoard = attendeesResultFull.EmailEventBoard,
                        OptInSponsorSpecialsLevel = attendeesResultFull.OptInSponsorSpecialsLevel,
                        OptInSponsoredMailingsLevel = attendeesResultFull.OptInSponsorSpecialsLevel,
                        ShirtSize = attendeesResultFull.ShirtSize,
                        UserBio = attendeesResultFull.UserBio,
                        UserWebsite = attendeesResultFull.UserWebsite,
                            

                        QREmailAllow = attendeesResultFull.QREmailAllow,
                        QRAddressLine1Allow = attendeesResultFull.QRAddressLine1Allow,
                        QRPhoneAllow = attendeesResultFull.QRPhoneAllow,
                        QRWebSiteAllow = attendeesResultFull.QRWebSiteAllow,
                        QRZipCodeAllow = attendeesResultFull.QRZipCodeAllow,
                        
                        AttendeesCodeCampYearResult = attendeesResultFull.AttendeesCodeCampYearResult,
                        HasSessionsCurrentYear = attendeesResultFull.HasSessionsCurrentYear,
                        AttendingDaysChoiceCurrentYear = attendeesResultFull.AttendingDaysChoiceCurrentYear,
                        RegisteredCurrentYear = attendeesResultFull.RegisteredCurrentYear,
                        VolunteeredCurrentYear = attendeesResultFull.VolunteeredCurrentYear
                    };
            return attendeesResult;
        }
    }
}
