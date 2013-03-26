using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using CodeCampSV;
using System.Linq;
using System.Text;
using aspNetEmail;


namespace WebAPI.rest
{

   /// <summary>
   /// 
   /// </summary>
    public class SponsorRequestController : ApiController
    {

        [Authorize(Users = "pkellner")]
        public HttpResponseMessage Get(int id)
        {
            HttpResponseMessage response = null;

            var recs =
                SponsorRequestManager.I.Get(new SponsorRequestQuery(){Id = id}).OrderByDescending(a => a.CreateDate).ToList();

            //response = Request.CreateResponse(HttpStatusCode.OK, recs);

            response = Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = true,
                    data = recs
                });


            return response;
        }




        [Authorize(Users = "pkellner")]
        public HttpResponseMessage Get()
        {
            HttpResponseMessage response = null;

            var recs =
                SponsorRequestManager.I.Get(new SponsorRequestQuery()).OrderByDescending(a => a.CreateDate).ToList();

            //response = Request.CreateResponse(HttpStatusCode.OK, recs);

            response = Request.CreateResponse(HttpStatusCode.OK, new
            {
                success = true,
                data = recs
            });


            return response;
        }

        /// <summary>
        /// insert (anyone)
        /// </summary>
        /// <param name="sponsorRequestResult"></param>
        /// <returns></returns>
        //
        public HttpResponseMessage Post(SponsorRequestResult sponsorRequestResult)
        {

            sponsorRequestResult.SvccEnteredInSystem = false;
            sponsorRequestResult.SvccRespondedTo = false;
            sponsorRequestResult.SvccNotes = "";
            sponsorRequestResult.CreateDate = DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0)); // PST

            SponsorRequestManager.I.Insert(sponsorRequestResult);

            SendMailConfirmation(sponsorRequestResult.ContactEmail, sponsorRequestResult.Company,
                                 sponsorRequestResult.Id);


            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new
            {
                success = true,
                data = new List<SponsorRequestResult> { sponsorRequestResult }
            });

            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new
            //{
            //    message = "problem here!",
            //    success = false
            //   // data = new List<SponsorRequestResult> { sponsorRequestResult }
            //});


            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, sponsorRequestResult);
            return response;
        }

       private void SendMailConfirmation(string contactEmail, string company, int id)
       {
           var sb = new StringBuilder();
           sb.AppendLine("email: " + contactEmail);
           sb.AppendLine("company:" + company);
           sb.AppendLine("url: " + "http://siliconvalley-codecamp.com/rest/SponsorRequest/" + id.ToString(CultureInfo.InvariantCulture));

           var msg = new EmailMessage(true, false);
           int portNumber = Convert.ToInt32(ConfigurationManager.AppSettings["EmailMessage.Port"]);
           msg.Port = portNumber;

           if (msg.Server.Equals("smtp.gmail.com"))
           {
               var ssl = new AdvancedIntellect.Ssl.SslSocket();
               msg.LoadSslSocket(ssl);
               msg.Port = 587;
           }

           msg.CharSet = "iso-8859-1";
           msg.Logging = true;
           msg.LogInMemory = true;
           msg.LogOverwrite = false;
           //msg.LogPath = logFile;
           msg.FromAddress = "sponsorship@siliconvalley-codecamp.com";
           msg.To = "pkellner99@gmail.com";
           msg.Subject = "Sponsorship Email From: " + company + " " + contactEmail;
           msg.Body = sb.ToString();

           try
           {
               msg.Send(true, false);
           }
           catch (Exception)
           {
               
              // throw;
           }



           var str = msg.Log;
       }

       /// <summary>
        /// this only updates the tag itself when sessionId is null or -1
        /// otherwise, it updates the session only and assume the tag did not change
        /// </summary>
        /// <returns></returns>
        [Authorize(Users = "pkellner")]
        public HttpResponseMessage Put(SponsorRequestResult sponsorRequestResult)
        {
            if (sponsorRequestResult == null) throw new ArgumentNullException("sponsorRequestResult");
            //return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "error here!!! in tags");

            var rec = SponsorRequestManager.I.Get(new SponsorRequestQuery()
                {
                    Id = sponsorRequestResult.Id
                }).FirstOrDefault();

            if (rec != null)
            {
                SponsorRequestManager.I.Update(sponsorRequestResult);
            }


            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new
            {
                success = true,
                data = new List<SponsorRequestResult> { sponsorRequestResult }
            });

            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, sponsorRequestResult);
            return response;
        }

        // DELETE api/tagsrest/5
        // [Authorize(RoleName = "Administrator")]
        [Authorize(Users = "pkellner")]
        public HttpResponseMessage Delete(int id)
        {
            TagsManager.I.Delete(id);
            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, tagItem);


            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new
            {
                success = true
               // data = new List<SponsorRequestResult> { sponsorRequestResult }
            });

         
            return response;
        }
    }
}
