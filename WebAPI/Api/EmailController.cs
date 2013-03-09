using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;
using CodeCampSV;
using aspNetEmail;

namespace WebAPI.Api
{
    public class EmailSendDetail
    {
        public string PreviewEmailSend { get; set; }
        public string EmailUrl { get; set; }
        public string Subject { get; set; }
        public string SubjectHtml { get; set; }
        public string MailBatchLabel { get; set; }
        public string SqlStatement { get; set; }
        public string EmailHtml { get; set; }
    }

    public class EmailMergeField
    {
        public string SubjectHtml { get; set; }
        public string EmailHtml { get; set; }
        public string PreviousYearsStatusHtml { get; set; }
        public string ToEmailAddress { get; set; }
        public string FromEmailAddress { get; set; }
        public string BaseUrlEmailPage { get; set; }
        public string BaseUrlSvcc { get; set; }
        public string EmailTrackingId { get; set; }
    }

    public class MailCriteria
    {
        public string SqlFilter { get; set; }
    }

    public class MailReturn
    {
        public List<AttendeesShortForEmail> Data { get; set; }
        public bool Success { get; set; }
    }

    public class EmailController : ApiController
    {
        [HttpGet]
        [ActionName("UsersBySql")]
        [Authorize(Roles = "admin")]
        public HttpResponseMessage GetUsersBySql(string sqlStatement)
        {
            List<AttendeesShortForEmail> attendeesShorts = Utils.GetAttendeesShortBySql(sqlStatement);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, attendeesShorts);
            return response;
        }

        private static readonly Encoding LocalEncoding = Encoding.UTF8;

        [HttpPost]
        [ActionName("EmailGenerate")]
        public HttpResponseMessage PostEmailGenerate(EmailSendDetail emailSendDetail)
        {
            HtmlUtility utility = HtmlUtilityReturn(emailSendDetail);

            var emailFinal = utility.ToEmailMessage();

            var memoryStream = new MemoryStream();
            emailFinal.SaveToStream(memoryStream);
            string emailString = LocalEncoding.GetString(memoryStream.ToArray());

            var emailDetailsTopicResult = new EmailDetailsTopicResult()
                                              {
                                                  Title =
                                                      emailSendDetail.MailBatchLabel ??
                                                      DateTime.Now.ToString(CultureInfo.InvariantCulture),
                                                  CreateDate = DateTime.UtcNow,
                                                  EmailMime = emailString,
                                                  EmailSubject = !String.IsNullOrEmpty(emailSendDetail.SubjectHtml)
                                                                     ? emailSendDetail.SubjectHtml
                                                                     : ConvertStringToHtml(emailSendDetail.Subject)

                                              };
           EmailDetailsTopicManager.I.Insert(emailDetailsTopicResult);

           List<AttendeesShortForEmail> attendeesShorts = Utils.GetAttendeesShortBySql(emailSendDetail.SqlStatement);
            foreach (var rec in attendeesShorts)
            {
                var emailDetails = new EmailDetailsResult()
                {
                    EmailDetailsTopicId = emailDetailsTopicResult.Id,
                    AttendeesId = rec.Id,
                    EmailDetailsGuid = Guid.NewGuid(),
                    EmailTo = rec.Email
                };
                EmailDetailsManager.I.Insert(emailDetails);
            }


          
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);

            //emailFinal.SaveToFile("e:\\temp\\mailSave.txt", true);

            //var emailMergeField =
            //    new EmailMergeField
            //        {
            //            SubjectHtml = !String.IsNullOrEmpty(emailSendDetail.SubjectHtml)
            //                              ? emailSendDetail.SubjectHtml
            //                              : ConvertStringToHtml(emailSendDetail.Subject, 27),
            //            EmailHtml = emailSendDetail.EmailHtml,
            //            PreviousYearsStatusHtml = ""
            //        };


            //var emailMergeFields =
            //    new List<EmailMergeField>
            //        {
            //            emailMergeField
            //        };

            //HttpResponseMessage httpResponseMessage =
            //   emailFinal.SendMailMerge(emailMergeFields)
            //       ? new HttpResponseMessage(HttpStatusCode.OK)
            //       : Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
            //                                     emailFinal.LastException().Message);

            return httpResponseMessage;


        }

        private static HtmlUtility HtmlUtilityReturn(EmailSendDetail emailSendDetail)
        {
            var email =
                new EmailMessage(true, false)
                    {
                        To = emailSendDetail.PreviewEmailSend,
                        Subject = emailSendDetail.Subject
                    };


            var utility =
                new HtmlUtility(email)
                    {
                        CssOption = CssOption.None
                        
                    };

            utility.LoadUrl(emailSendDetail.EmailUrl ?? "http://pkellner.site44.com/");

            utility.SetUrlContentBase = false;
            utility.SetHtmlBaseTag = false;
            utility.EmbedImageOption = EmbedImageOption.None;
            

            utility.Render();
            return utility;

            // note from dave on removing objects embedded image collection
            //If you are using utility.ToEmailMessage(), it should be created for you, automatically. If not, then I need to do some quick testing.

            //As far as the embedded images, there is a msg.EmbeddedObjects collection.

            //You can loop through that collection, check image names, and simply remove them. Off the top of my head:

            //If( msg.EmbeddedObjects != null ) && ( msg.EmbeddedObjects.Count > 0) ){
            //For( int i=msg.EmbeddedObjects.Count -1;i>=0;i--)
            //{
            //EmbeddedObject eo = msg.EmbeddedObjects[i] as EmbeddedObject.
            //If( eo.Name == “Whatever”)
            //{
            //  //remove the object
            //Msg.EmbeddedObjects.RemoveAt(i);
            //}
            //}
            //}
        }

        [HttpPost]
        [ActionName("EmailPreview")]
        public HttpResponseMessage PostEmailPreview(EmailSendDetail emailSendDetail)
        {
           

          


            HtmlUtility utility = HtmlUtilityReturn(emailSendDetail);

            var emailFinal = utility.ToEmailMessage();
            //emailFinal.SaveToFile("e:\\temp\\mailSave.txt", true);





            //string baseUnSubscribeUrl = "http://localhost:17138/";
            //string baseUnSubscribeUrl = "http://svcodecamp.azurewebsites.net/";

            List<EmailDetailsTopicResult> emailDetailsTopicResult =
                EmailDetailsTopicManager.I.GetAll().OrderByDescending(a => a.Id).ToList();
           var emailDetails = new EmailDetailsResult()
           {
               EmailDetailsTopicId = 
               emailDetailsTopicResult.Count > 0 ? emailDetailsTopicResult[0].Id : -1,
               AttendeesId = 99999,
               EmailDetailsGuid = Guid.NewGuid(),
               EmailTo = emailSendDetail.PreviewEmailSend,
               EmailFrom = emailFinal.FromAddress,
               SentDateTime = DateTime.UtcNow,
               EmailReadCount = 0
           };
           EmailDetailsManager.I.Insert(emailDetails);

            // make sure these do not end in /
            const string baseUrlEmailPage = "http://pkellner.site44.com";
            const string baseUrlSvcc = "http://svcodecamp.azurewebsites.net";

            var emailMergeField =
                new EmailMergeField
                    {
                        SubjectHtml = !String.IsNullOrEmpty(emailSendDetail.SubjectHtml)
                                          ? emailSendDetail.SubjectHtml
                                          : ConvertStringToHtml(emailSendDetail.Subject),
                        //UnsubscribeLink =
                        //    String.Format("u/{0}", emailDetails.EmailDetailsGuid.ToString()),
                        EmailHtml = emailSendDetail.
                            EmailHtml,
                        PreviousYearsStatusHtml = "",
                        ToEmailAddress = emailSendDetail.PreviewEmailSend,
                        FromEmailAddress = emailFinal.FromAddress,
                        EmailTrackingId = emailDetails.EmailDetailsGuid.ToString(),
                        BaseUrlEmailPage = baseUrlEmailPage,
                        BaseUrlSvcc = baseUrlSvcc
                    };


            var emailMergeFields =
                new List<EmailMergeField>
                    {
                        emailMergeField
                    };

            emailFinal.Logging = true;
            emailFinal.LogInMemory = true;

            HttpResponseMessage httpResponseMessage =
               emailFinal.SendMailMerge(emailMergeFields)
                   ? new HttpResponseMessage(HttpStatusCode.OK)
                   : Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 emailFinal.LastException().Message);

            // var str = emailFinal.Log.ToString();
            //System.IO.File.AppendAllText("e:\\temp\\_log.txt", str);

            return httpResponseMessage;


        }

        private string ConvertStringToHtml(string subject)
        {
            return String.IsNullOrEmpty(subject)
                       ? ""
                       : String.Format("<span>{0}</span>", subject);
        }
    }




}
