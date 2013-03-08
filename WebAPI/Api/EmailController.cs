using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
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
        public HttpResponseMessage GetUsersBySql(string sqlFilter)
        {
            List<AttendeesShortForEmail> attendeesShorts = Utils.GetAttendeesShortBySql(sqlFilter);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, attendeesShorts);
            return response;
        }


        private static readonly Encoding LocalEncoding = Encoding.UTF8;

        [HttpPost]
        [ActionName("EmailGenerate")]
        public HttpResponseMessage PostEmailGenerate(EmailSendDetail emailSendDetail)
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
                        CssOption = CssOption.EmbedLinkedSheets
                    };

            utility.LoadUrl(emailSendDetail.EmailUrl ?? "http://pkellner.site44.com/");

            utility.SetUrlContentBase = true;
            utility.SetHtmlBaseTag = true;
            utility.EmbedImageOption = EmbedImageOption.ConvertToAbsoluteUrl;


            utility.Render();

            var emailFinal = utility.ToEmailMessage();

            var memoryStream = new MemoryStream();
            emailFinal.SaveToStream(memoryStream);
            string emailString = LocalEncoding.GetString(memoryStream.ToArray());

            var emailDetailTopic = new EmailDetailsTopicResult()
                                       {
                                           Title = emailSendDetail.MailBatchLabel ?? DateTime.Now.ToString(),
                                           CreateDate = DateTime.UtcNow,
                                           EmailMime = emailString,
                                           EmailSubject = !String.IsNullOrEmpty(emailSendDetail.SubjectHtml)
                                                              ? emailSendDetail.SubjectHtml
                                                              : ConvertStringToHtml(emailSendDetail.Subject,
                                                                                    27)

                                       };
           EmailDetailsTopicManager.I.Insert(emailDetailTopic);

           List<AttendeesShortForEmail> attendeesShorts = Utils.GetAttendeesShortBySql(emailSendDetail.SqlStatement);
            foreach (var rec in attendeesShorts)
            {
                var emailDetails = new EmailDetailsResult()
                {
                    EmailDetailTopicId = emailDetailTopic.Id,
                    AttendeesId = rec.Id,
                    EmailDetailsGuid = Guid.NewGuid(),
                    EmailTo = rec.Email
                };
                EmailDetailsManager.I.Insert(emailDetails);
            }


           


            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);

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

        [HttpPost]
        [ActionName("EmailPreview")]
        public HttpResponseMessage PostEmailPreview(EmailSendDetail emailSendDetail)
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
                    CssOption = CssOption.EmbedLinkedSheets
                };

            string siteUrl = emailSendDetail.EmailUrl ??
                "http://pkellner.site44.com/";
            utility.LoadUrl(siteUrl);

            utility.SetUrlContentBase = true;
            utility.SetHtmlBaseTag = true;
            utility.EmbedImageOption = EmbedImageOption.ConvertToAbsoluteUrl;

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


            utility.Render();
            var emailFinal = utility.ToEmailMessage();


  

            //emailFinal.SaveToFile("e:\\temp\\mailSave.txt", true);

            var emailMergeField =
                new EmailMergeField
                    {
                        SubjectHtml = !String.IsNullOrEmpty(emailSendDetail.SubjectHtml)
                                          ? emailSendDetail.SubjectHtml
                                          : ConvertStringToHtml(emailSendDetail.Subject, 27),
                        EmailHtml = emailSendDetail.EmailHtml,
                        PreviousYearsStatusHtml = ""
                    };


            var emailMergeFields =
                new List<EmailMergeField>
                    {
                        emailMergeField
                    };

            //emailFinal.Logging = true;
            //emailFinal.LogInMemory = true;

          


            HttpResponseMessage httpResponseMessage =
               emailFinal.SendMailMerge(emailMergeFields)
                   ? new HttpResponseMessage(HttpStatusCode.OK)
                   : Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 emailFinal.LastException().Message);

           // var str = emailFinal.Log.ToString();

            //System.IO.File.AppendAllText("e:\\temp\\_log.txt", str);

            //HttpResponseMessage httpResponseMessage =
            //    emailFinal.Send()
            //        ? new HttpResponseMessage(HttpStatusCode.OK)
            //        : Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
            //                                      emailFinal.LastException().Message);

            return httpResponseMessage;


        }

        protected const string Newline = "<br/>";

        /// <summary>
        /// convert the string to html with multiple lines if necessary
        /// http://www.softcircuits.com/Blog/post/2010/01/10/Implementing-Word-Wrap-in-C.aspx
        /// </summary>
        /// <param name="text"></param>
        /// <param name="margin"></param>
        /// <returns></returns>
        private string ConvertStringToHtml(string text,int margin)
        {
            //text = text.Replace(" ", "&nbsp;");
            int start = 0, end;
            var lines = new List<string>();
            text = Regex.Replace(text, @"\s", " ").Trim();

            while ((end = start + margin) < text.Length)
            {
                while (text[end] != ' ' && end > start)
                    end -= 1;

                if (end == start)
                    end = start + margin;

                lines.Add(text.Substring(start, end - start));
                start = end + 1;
            }

            if (start < text.Length)
            {
                lines.Add(text.Substring(start));
            }

            /*
                <!--<span class="style4">SV Code Camp Version 8!</span><br>
                <span class="style5">October 5th and 6th, 2013</span>-->
             */
            var sb = new StringBuilder();
            for (int index = 0; index < lines.Count; index++)
            {
                var rec = lines[index];
                if (index == 0)
                {
                    // rec = string.Format("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{0}", rec);
                    // first line, always style4 and always <br>
                    rec = String.Format("<span class=\"style4\">{0}</span><br/>", rec);
                }
                else
                {
                    rec = String.Format("<span class=\"style5\">{0}</span>", rec);
                    if (index != lines.Count - 1) // if not last line, add <br/>
                    {
                        rec = rec + "<br/>";
                    }
                }

                

                //sb.Append(rec.Replace(" ", "%nbsp;"));
                sb.Append(rec);
            }

            return sb.ToString();
        }
    }




}
