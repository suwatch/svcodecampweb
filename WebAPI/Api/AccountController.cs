using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        /// <summary>
        /// http://www.asp.net/web-api/overview/working-with-http/sending-html-form-data,-part-2
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("FormData")]
        public Task<HttpResponseMessage> PostFormData()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            // Read the form data and return an async task.
            var task = Request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<HttpResponseMessage>(t =>
                {
                    if (t.IsFaulted || t.IsCanceled)
                    {
                        Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);
                    }

                    // This illustrates how to get the file names.
                    foreach (MultipartFileData file in provider.FileData)
                    {
                        Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                        Trace.WriteLine("Server file path: " + file.LocalFileName);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK);
                });

            return task;
        }
        [HttpPost]
        [ActionName("CheckUsernameEmailExists")]
        public HttpResponseMessage PostCheckUsernameEmailExists(AttendeesResult attendeeRecord)
        {
            Thread.Sleep(300); // try to defend a little against denial of service attack or username searching attack

            var errorMessage = new StringBuilder();

            if (String.IsNullOrEmpty(attendeeRecord.Username) && String.IsNullOrEmpty(attendeeRecord.Email))
            {
                errorMessage.Append("Failure: Must specify username, email or both in incoming parameters.");
            }
            else if (!String.IsNullOrEmpty(attendeeRecord.Username) && String.IsNullOrEmpty(attendeeRecord.Email))
            {
                bool attendeeUsernameExists = AttendeesManager.I.CheckAttendeeUsernameExists(attendeeRecord.Username);
                if (attendeeUsernameExists)
                {
                    errorMessage.Append("Failure: Username Exists.");
                }
            }
            else if (String.IsNullOrEmpty(attendeeRecord.Username) && !String.IsNullOrEmpty(attendeeRecord.Email))
            {
                bool attendeeEmailExists = AttendeesManager.I.CheckAttendeeEmailExists(attendeeRecord.Username);
                if (attendeeEmailExists)
                {
                    errorMessage.Append("Failure: Email Exists.");
                }
            }
            else if (!String.IsNullOrEmpty(attendeeRecord.Username) && !String.IsNullOrEmpty(attendeeRecord.Email))
            {
                bool attendeeUsernameExists = AttendeesManager.I.CheckAttendeeUsernameExists(attendeeRecord.Username);
                bool attendeeEmailExists = AttendeesManager.I.CheckAttendeeEmailExists(attendeeRecord.Username);
                if (attendeeEmailExists || attendeeUsernameExists)
                {
                    if (attendeeEmailExists)
                    {
                        errorMessage.Append("Failure: Email Exists.");
                    }
                    if (attendeeUsernameExists)
                    {
                        errorMessage.Append("Failure: Username Exists");
                    }
                }
            }

            return errorMessage.Length == 0
                       ? Request.CreateResponse(HttpStatusCode.OK, "")
                       : Request.CreateErrorResponse(HttpStatusCode.Forbidden,
                                                     errorMessage.ToString());
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
        [ActionName("CreateUser")]
        public HttpResponseMessage PostCreateUser(AttendeesResult attendee)
        {
            HttpResponseMessage response;
            if (String.IsNullOrEmpty(attendee.Username) ||
                String.IsNullOrEmpty(attendee.Password) ||
                String.IsNullOrEmpty(attendee.Email))
            {
                response =
                           Request.CreateErrorResponse(HttpStatusCode.Forbidden,
                                                       "CreateUser requires non blank Username,Password and Email");
            }
            else if (attendee.Password.Length < 4)
            {
                response =
                    Request.CreateErrorResponse(HttpStatusCode.Forbidden,
                                                "CreateUser requires password at least 4 characters long");
            }
            else
            {
                // now we have username, password and email
                bool emailExists = AttendeesManager.I.CheckAttendeeEmailExists(attendee.Email);
                bool usernameExists = AttendeesManager.I.CheckAttendeeEmailExists(attendee.Username);
                if (emailExists || usernameExists)
                {
                    response =
                        Request.CreateErrorResponse(HttpStatusCode.Forbidden,
                                                    string.Format(
                                                        "CreateUser email or username exists so can not create {0}:{1}",
                                                        emailExists.ToString(), usernameExists.ToString()));
                }
                else
                {
                    // create the user! everything should be good
                    MembershipCreateStatus mStatus;
                    Membership.CreateUser(
                        attendee.Username,
                        attendee.Password,
                       attendee.Email,
                        "Question", "Answer",
                        true,
                        out mStatus);

                    if (mStatus.Equals(MembershipCreateStatus.Success))
                    {

                        response = Request.CreateResponse(HttpStatusCode.OK, "");

                    }
                    else
                    {
                        response =
                            Request.CreateErrorResponse(HttpStatusCode.Forbidden,
                                                        "CreateUser membership failed " + mStatus.ToString());
                    }
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
