using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.ServiceModel.Channels;
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
using WebAPI.Code;
using aspNetEmail;

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

        //[System.Web.Http.HttpPost]
        //[System.Web.Http.ActionName("Login")]
        //public HttpResponseMessage PostLogin(LoginCredentials loginCredentials)
        //{

        //    return new HttpResponseMessage(HttpStatusCode.OK);
        //}


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
                int bytesUpoaded = -1;
                using (var memoryStream = new MemoryStream())
                {
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

                    Log4NetAllManager.I.Insert(new Log4NetAllResult()
                    {
                        Date = DateTime.Now.AddHours(-3),
                        EllapsedTime = 0,
                        ExceptionMessage = "",
                        ExceptionStackTrace = "",
                        Level = "",
                        Logger = "",
                        Message = "AccountController:PostFormData length: " + memoryStream.Length,
                        MessageLine1 = "",
                        Thread = "",
                        
                    });

                    var byteArray = new byte[memoryStream.Length];

                    //Set pointer to the beginning of the stream
                    memoryStream.Position = 0;

                    //Read the entire stream
                    memoryStream.Read(byteArray, 0, (int) memoryStream.Length);

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
                            bytesUpoaded = byteArray.Count();

                            AttendeesManager.I.Update(attendeesResult);
                        }

                    }
                }

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new LoginReturnStatus()
                    {
                        AttendeeId = attendeesId,
                        Success = true,
                        Status = "success",
                        File = "speaker.jpg"                    ,
                        Message = "bytes uploaded: " +  bytesUpoaded.ToString()
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

                return Request.CreateResponse(HttpStatusCode.Forbidden, ret);
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
                char[] splitchar = {','};
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
        [System.Web.Http.ActionName("CheckPictureExists")]
        public HttpResponseMessage PostCheckPictureExists(AttendeesResult attendeeRecord)
        {
            Thread.Sleep(300); // try to defend a little against denial of service attack or username searching attack

            var errorMessage = new StringBuilder();

            if (attendeeRecord == null || attendeeRecord.Id <= 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden,
                                                   "PostCheckPictureExists: attendeeRecord no passed in populated");

            }

            var pictureLen = Utils.GetPictureLengthByAttendee(attendeeRecord.Id);

            return pictureLen > 0
                       ? Request.CreateResponse(HttpStatusCode.OK, "")
                       : Request.CreateErrorResponse(HttpStatusCode.Forbidden,
                                                     "picture length for attendee zero");
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

        //[System.Web.Http.HttpPost]
        //[System.Web.Http.ActionName("SponsorRequest")]
        //public HttpResponseMessage PostSponsorRequest(FormDataCollection formItems)
        //{


        //    return Request.CreateResponse(HttpStatusCode.OK,
        //                                       "");
        //}

        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("UpdateAttendee")]
        public HttpResponseMessage PostUpdateAttendee(AttendeesResult attendeeRecord)
        {
            //return  Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,"test bad responose from PostUpdateAttendee");
            return UpdateAttendeeRecordParts(attendeeRecord, "attendee");
        }

        private HttpResponseMessage UpdateAttendeeRecordParts(AttendeesResult attendeeRecord, string attendeeSaveOption)
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
                        attendeesResult.OptInTechJobKeyWords = attendeeRecord.OptInTechJobKeyWords;
                        attendeesResult.OptInSvccKids = attendeeRecord.OptInSvccKids;
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
                        attendeesResult.PrincipleJob = attendeeRecord.PrincipleJob;
                        attendeesResult.Company = attendeeRecord.Company;


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



        /// <summary>
        /// http://stackoverflow.com/questions/9565889/get-the-ip-address-of-the-remote-host
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private string GetClientIp(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper) request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop;
                prop = (RemoteEndpointMessageProperty) this.Request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            else
            {
                return null;
            }
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("CreateUser")]
        public HttpResponseMessage PostCreateUser(AttendeesResult attendee)
        {
            HttpResponseMessage response;
            if (!ConfigurationManager.AppSettings["OverrideCaptcha"].ToLower().Equals("true") &&
                !HttpContext.Current.Request.IsAuthenticated)
            {
                // First, verif captcha
                string clientAddress = HttpContext.Current.Request.UserHostAddress;
                string message;
                if (
                    !VerifyRecaptcha(attendee.RecaptchaChallengeField, attendee.RecaptchaResponseField, clientAddress,
                                     out message))
                {
                    response =
                        Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                    message);
                    return response;

                }
            }

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
                        FormsAuthentication.SetAuthCookie(attendee.Username, true);
                        // for now update counts
                        var attendeeNew = AttendeesManager.I.Get(new AttendeesQuery()
                            {
                                Username = attendee.Username
                            }).FirstOrDefault();

                        if (attendeeNew != null)
                        {
                            attendeeNew.PresentationLimit = 3;
                            attendeeNew.PresentationApprovalRequired = false;
                            AttendeesManager.I.Update(attendeeNew);
                        }
                        response = Request.CreateResponse(HttpStatusCode.OK, attendeeNew);


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

        ///// <summary>
        ///// http://localhost:17138/rpc/Account/Get?challenge=5VJ6BUWbaWse_UJUhN8qFpNbSderffhOwXtrZQOIJFskAOoNJadD8zm1NclkxZppipMuSPysE7ydUM&response=Saltat Horster
        ///// </summary>
        ///// <param name="challenge"></param>
        ///// <param name="response"></param>
        ///// <returns></returns>
        //[System.Web.Http.HttpGet]
        //public HttpResponseMessage Get(string challenge, string response)
        //{
        //    string message;
        //    bool verified = VerifyRecaptcha(challenge, response, HttpContext.Current.Request.UserHostAddress,out message);

        //    return verified
        //               ? Request.CreateResponse(HttpStatusCode.OK)
        //               : Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, message);
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="recaptchaChallengeField"></param>
        /// <param name="recaptchaResponseField"></param>
        /// <param name="clientIp"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool VerifyRecaptcha(string recaptchaChallengeField, string recaptchaResponseField, string clientIp,out string message)
        {
          
            var recaptchaValidator = new RecaptchaValidator
                {
                    PrivateKey = "6LcrXN4SAAAAALfMoSWjxx5GGH1B8koKXRj4czvA",
                    Challenge = recaptchaChallengeField,
                    //RemoteIP =  clientIp
                    RemoteIP = "71.202.162.255",
                    Response = recaptchaResponseField
                };
            var recaptchaResponse = recaptchaValidator.Validate();

            message = recaptchaResponse.ErrorMessage;
            return recaptchaResponse.IsValid;
        }

        private void EncodeAndAddItem(ref StringBuilder baseRequest, string key, string dataItem)
        {
            if (baseRequest == null)
            {
                baseRequest = new StringBuilder();
            }
            if (baseRequest.Length != 0)
            {
                baseRequest.Append("&");
            }
            baseRequest.Append(key);
            baseRequest.Append("=");
            baseRequest.Append(System.Web.HttpUtility.UrlEncode(dataItem));
        }



        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("ForgotPassword")]
        public HttpResponseMessage PostForgotPassword(AttendeesResult attendeesResult)
        {
            HttpResponseMessage response;

            string usernameOrEmail = attendeesResult.Username;


            string username = Utils.GetUsernameFromEmail(usernameOrEmail);
            if (String.IsNullOrEmpty(username))
            {
                string goodEmail = Utils.GetEmailFromUsername(usernameOrEmail);
                username = Utils.GetUsernameFromEmail(goodEmail);
            }

            if (String.IsNullOrEmpty(username))
            {
                response =
                    Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                "Name not found as either username or email.  Please register as new attendee");
            }
            else
            {
                var attendeeRec = AttendeesManager.I.Get(new AttendeesQuery {Username = username}).FirstOrDefault();
                if (attendeeRec == null)
                {
                    throw new ApplicationException("attendeeRec could not be loaded");
                }
                MembershipUser mu = Membership.GetUser(username);
                if (mu == null)
                {
                    throw new ApplicationException("MembershipUser mu not found");
                }
                var newPassword = mu.ResetPassword();
                var msg = new EmailMessage(true, false)
                    {
                        Logging = false,
                        LogOverwrite = false,
                        //LogPath = Context.Server.MapPath(String.Empty) + "\\App_Data\\EmailPasswordRecovery.log",
                        FromAddress = Utils.GetServiceEmailAddress(),
                        To = attendeeRec.Email,
                        Subject = "Your New Password For http://www.siliconvalley-codecamp.com"
                    };

                if (msg.Server.Equals("smtp.gmail.com"))
                {
                    var ssl = new AdvancedIntellect.Ssl.SslSocket();
                    msg.LoadSslSocket(ssl);
                    msg.Port = 587;
                }

                var sb = new StringBuilder();
                sb.AppendLine(
                    String.Format("Please log in to your codecamp account ({0}) with the new password: {1}",
                                  username, newPassword));
                sb.AppendLine(" ");
                sb.AppendLine("We suggest that after you log in, you change your password.");
                sb.AppendLine("We store your password in an encrypted format which is");
                sb.AppendLine("why we are unable to send you your original password.");
                sb.AppendLine(" ");
                sb.AppendLine("We are looking forward to seeing you at camp!");
                sb.AppendLine(" ");
                sb.AppendLine("Best Regards,");
                sb.AppendLine("");
                sb.AppendLine("http://www.siliconvalley-codecamp.com");

                msg.Body = sb.ToString();

                try
                {
                    msg.Send();
                    response =
                   Request.CreateResponse(HttpStatusCode.OK, new AttendeesResult()
                   {
                       Email = attendeesResult.Email,
                       Username = attendeesResult.Username,
                       Id = attendeesResult.Id
                   });
                }
                catch (Exception e)
                {
                    response =
                        Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                    "We found your account but email could not be delivered to " +
                                                    attendeeRec.Email + " for account "+ attendeeRec.Username +
                                                    ".  Please Make a new account or contact info@siliconvalley-codecamp.com and we will reset the password for you.");
                }
               
            }
            return response;

        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("Login")]
        public HttpResponseMessage PostLogin(LoginCredentials login)
        {
            //if (!ModelState.IsValid)
            //{
            //    // throw error  (ivalidateable object)
            //    // throw httpresponse exce.
            //    // webdev blog
            //    // webapi pipeline
            //    // tracing system?  nuget webapi system diagnostics trace
            //    // attribute routing.org
            //}




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
