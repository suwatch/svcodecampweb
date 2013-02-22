﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using CodeCampSV;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Api
{
    public class AccountController : ApiController
    {

        public class LoginCredentials
        {
            [StringLength(15)]
            public string Username { get; set; }
            public string Password { get; set; }
            public bool RememberMe { get; set; }
        }

        public class LoginReturnStatus
        {
            public string Status { get; set; }
            public string Message { get; set; }
            public AttendeesResult Data { get; set; }
            public string File { get; set; }

            public bool Success { get; set; }

            public int AttendeeId { get; set; }
           
        }

        public class ShirtSizeRec
        {
            public string ShirtSize { get; set; }
        }

        public class ShirtSizeReturn
        {
            
            public string Message { get; set; }
            public List<ShirtSizeRec> Data { get; set; }  
            
            public bool Success { get; set; }
        }

        /// <summary>
        /// http://www.enterpriseframework.com/post/2012/12/19/Full-Web-Api-Controller-Code-to-Receive-a-Posted-File-Async.aspx
        /// 
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("FormData")]
        public async Task<HttpResponseMessage> PostFormData()
        {
            int attendeesId = -1;

            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            //Write to File
            //string root = HttpContext.Current.Server.MapPath("~/App_Data");
            //var provider = new MultipartFormDataStreamProvider(root);

            //Write to Memory
            var provider = new MultipartMemoryStreamProvider();

            try
            {
                // Read the form data and return an async task.
                await Request.Content.ReadAsMultipartAsync(provider);

                /* THIS WORKS WITH MultipartMemoryStreamProvider UNCOMMENTED ABOVE */

                using (var memoryStream = new MemoryStream())
                {
                    //// You could also just use StreamWriter to do "writer.Write(bytes)"
                    //ms.Write(bytes, 0, bytes.Length);

                    //using (StreamWriter writer = new StreamWriter(ms))
                    //{
                    //    writer.Write("Some Data");
                    //}

                    //File.WriteAllBytes(outFilePath, ms.ToArray());


                    foreach (var item in provider.Contents)
                    {
                        using (Stream stream = item.ReadAsStreamAsync().Result)
                        {
                            if (stream != null)
                            {
                                //Convert Stream to Bytes or something
                                var bytes = new byte[stream.Length];
                                stream.Read(bytes, 0, (int) stream.Length);
                                memoryStream.Write(bytes, 0, (int) stream.Length);
                            }
                        }
                    }

                    //create new Bite Array
                    var byteArray = new byte[memoryStream.Length];

                    //Set pointer to the beginning of the stream
                    memoryStream.Position = 0;

                    //Read the entire stream
                    memoryStream.Read(byteArray, 0, (int)memoryStream.Length);

                    if (User.Identity.IsAuthenticated)
                    {
                        var attendeesResult =
                            AttendeesManager.I.Get(new AttendeesQuery
                                                       {
                                                           Username = User.Identity.Name
                                                       }).FirstOrDefault();
                        
                        if (attendeesResult != null)
                        {
                            attendeesId = attendeesResult.Id;
                            attendeesResult.UserImage = new System.Data.Linq.Binary(byteArray);

                            AttendeesManager.I.Update(attendeesResult);
                        }

                    }
                }

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new LoginReturnStatus()
                                                                                             {
                                                                                                 AttendeeId = attendeesId,
                                                                                                 Success = true,
                                                                                                 Status = "success",
                                                                                                 File="speaker.jpg"
                                                                                             });
                return response;
            }
            catch (System.Exception e)
            {

                var ret = new LoginReturnStatus()
                              {
                                  Success = false,
                                  Status = "Failure",
                                  File = "speaker.jpg",
                                  Message = e.ToString()
                              };

                return Request.CreateResponse(HttpStatusCode.Forbidden,ret);
            }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("ShirtSizes")]
        public HttpResponseMessage PostShirtSizes()
        {
            var shirtSizes = new List<string>();
            shirtSizes.Add("--Please Select From Below");
            shirtSizes.Add("--No Shirt Please");

            if (ConfigurationManager.AppSettings["SpeakerShirtSizes"] != null)
            {
                string list = ConfigurationManager.AppSettings["SpeakerShirtSizes"];
                char[] splitchar = { ',' };
                shirtSizes.AddRange(list.Split(splitchar).ToList());
                foreach (var item in shirtSizes.ToList())
                {
                    shirtSizes.Add(item.Trim());
                }
            }

            List<ShirtSizeRec> shirtSizeRecs = shirtSizes.Select(recx => new ShirtSizeRec() {ShirtSize = recx}).ToList();

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new ShirtSizeReturn()
            {
                Success = true,
                Data = shirtSizeRecs
            });
            return response;
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("CheckUsernameEmailExists")]
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

        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("UpdateSpeaker")]
        public HttpResponseMessage PostUpdateSpeaker(AttendeesResult attendeeRecord)
        {
            return UpdateAttendeeRecordParts(attendeeRecord, "speaker");
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("UpdateOptIn")]
        public HttpResponseMessage PostUpdateOptIn(AttendeesResult attendeeRecord)
        {
            return UpdateAttendeeRecordParts(attendeeRecord, "optin");
        }

        //[HttpGet]
        //[ActionName("RedirectSessions")]
        //public HttpResponseMessage GetRedirectSessions()
        //{
        //    //HTTP/1.1 301 Moved Permanently
        //    //Location: http://www.example.org/

        //    string urlBase = HttpContext.Current.Request.Url.Authority;
        //    string urlProtocol = HttpContext.Current.Request.IsSecureConnection ? "https" : "http";
        //    string redirectUrl = String.Format("{0}://{1}/{2}", urlProtocol, urlBase, "Session#");
        //    var response = Request.CreateResponse(HttpStatusCode.Redirect);
        //    response.Headers.Add("Location",redirectUrl);
        //    return response;

        //}

        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("SponsorRequest")]
        public HttpResponseMessage PostSponsorRequest(FormDataCollection formItems)
        {
            return Request.CreateErrorResponse(HttpStatusCode.OK,
                                               "");
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("UpdateAttendee")]
        public HttpResponseMessage PostUpdateAttendee(AttendeesResult attendeeRecord)
        {
            return UpdateAttendeeRecordParts(attendeeRecord,"attendee");
        }

        private HttpResponseMessage UpdateAttendeeRecordParts(AttendeesResult attendeeRecord,string attendeeSaveOption)
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
                    if (attendeeSaveOption.ToLower().Equals("optin"))
                    {
                        attendeesResult.OptInSponsorSpecialsLevel = attendeeRecord.OptInSponsorSpecialsLevel;
                        attendeesResult.OptInSponsoredMailingsLevel = attendeeRecord.OptInSponsoredMailingsLevel;
                    }
                    else
                    {
                        // These are the only fields that can get updated by the caller (security)
                        // (for both speaker and attendee)
                        attendeesResult.Email = attendeeRecord.Email;
                        attendeesResult.UserFirstName = attendeeRecord.UserFirstName;
                        attendeesResult.UserLastName = attendeeRecord.UserLastName;
                        attendeesResult.City = attendeeRecord.City;
                        attendeesResult.State = attendeeRecord.State;
                        attendeesResult.UserZipCode = attendeeRecord.UserZipCode;
                        attendeesResult.TwitterHandle = attendeeRecord.TwitterHandle;
                        attendeesResult.AttendingDaysChoiceCurrentYear = attendeeRecord.AttendingDaysChoiceCurrentYear;
                        attendeesResult.RegisteredCurrentYear = attendeeRecord.RegisteredCurrentYear;
                        attendeesResult.PhoneNumber = attendeeRecord.PhoneNumber;
                        attendeesResult.EmailEventBoard = attendeeRecord.EmailEventBoard;
                        attendeesResult.VolunteeredCurrentYear = attendeeRecord.VolunteeredCurrentYear;


                        // speaker stuff below
                        if (attendeeSaveOption.ToLower().Equals("speaker"))
                        {
                            attendeesResult.FacebookId = attendeeRecord.FacebookId;
                            attendeesResult.GooglePlusId = attendeeRecord.GooglePlusId;
                            attendeesResult.LinkedInId = attendeeRecord.LinkedInId;
                            attendeesResult.ShirtSize = attendeeRecord.ShirtSize;
                            attendeesResult.UserBio = attendeeRecord.UserBio;
                        }
                    }


                    attendeesResult.CurrentCodeCampYear = Utils.CurrentCodeCampYear;
                    AttendeesManager.I.UpdateWithAttendeeCCY(attendeesResult);

                    response = Request.CreateResponse(HttpStatusCode.OK, MakeSafeAttendee(attendeesResult));
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

        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("CreateUser")]
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
                        FormsAuthentication.SetAuthCookie(attendee.Username,true);
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

        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("Login")]
        public HttpResponseMessage PostLogin(LoginCredentials login)
        {
            if (!ModelState.IsValid)
            {
                // throw error  (ivalidateable object)
                // throw httpresponse exce.
                // webdev blog
                // webapi pipeline
                // tracing system?  nuget webapi system diagnostics trace
                // attribute routing.org
            }
            



            var loginReturnStatus =
                new LoginReturnStatus();

           HttpResponseMessage response;
           if (!String.IsNullOrEmpty(login.Username) && !String.IsNullOrEmpty(login.Password))
           {
               var loginSuccess = Membership.ValidateUser(login.Username, login.Password);
               if (loginSuccess)
               {
                   FormsAuthentication.SetAuthCookie(login.Username, login.RememberMe);

                   AttendeesResult attendeesResultFull =
                       AttendeesManager.I.Get(new AttendeesQuery()
                                                  {
                                                      CodeCampYearId = Utils.CurrentCodeCampYear,
                                                      IncludeAttendeesCodeCampYearResult = true,
                                                      Username = login.Username
                                                  }).FirstOrDefault();
                   if (attendeesResultFull != null)
                   {
                       response = Request.CreateResponse(HttpStatusCode.OK, MakeSafeAttendee(attendeesResultFull));
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

        private AttendeesResult MakeSafeAttendee(AttendeesResult attendeesResultFull)
        {
            attendeesResultFull.ApplicationName = "";
            attendeesResultFull.FullNameUsernameZipcode = "";
            attendeesResultFull.PKID = Guid.Empty;
            attendeesResultFull.Password = "";
            attendeesResultFull.PasswordAnswer = "";
            attendeesResultFull.PasswordQuestion = "";
            attendeesResultFull.UserImage = null;
            return attendeesResultFull;
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("IsLoggedIn")]
        public HttpResponseMessage PostIsLoggedIn(LoginCredentials login)
        {
            var loginReturnStatus =
                new LoginReturnStatus();

            HttpResponseMessage response;

            if (User.Identity.IsAuthenticated)
            {
                AttendeesResult attendeesResultFull =
                    AttendeesManager.I.Get(new AttendeesQuery
                                               {
                                                   Username = User.Identity.Name,
                                                   CodeCampYearId = Utils.CurrentCodeCampYear,
                                                   IncludeAttendeesCodeCampYearResult = true
                                               }).FirstOrDefault();

                if (attendeesResultFull != null)
                {
                    //var attendeesResult = AttendeesResultStripped(attendeesResultFull);
                    loginReturnStatus.Data = attendeesResultFull;
                    response = Request.CreateResponse(HttpStatusCode.OK, MakeSafeAttendee(attendeesResultFull));
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

        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("LogOut")]
        public HttpResponseMessage PostLogOut(LoginCredentials login)
        {
            if (User.Identity.IsAuthenticated)
            {
               FormsAuthentication.SignOut();
            }
            return Request.CreateResponse(HttpStatusCode.OK, "");
        }

    }
}
