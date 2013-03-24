using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using CodeCampSV;
using System.Linq;


namespace WebAPI.rest
{

    /// <summary>
    /// this really acts more like a TagsSession controller.  :(
    /// </summary>
    public class SponsorRequestController : ApiController
    {


        //  [Authorize(Users = "pkellner")]
        public HttpResponseMessage Get()
        {
            HttpResponseMessage response = null;

            var recs =
                SponsorRequestManager.I.Get(new SponsorRequestQuery()).OrderByDescending(a => a.CreateDate).ToList();

            response = Request.CreateResponse(HttpStatusCode.OK, recs);
          

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
            sponsorRequestResult.CreateDate = DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0));  // PST
       

            SponsorRequestManager.I.Insert(sponsorRequestResult);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, sponsorRequestResult);
            return response;
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

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, sponsorRequestResult);
            return response;
        }

        // DELETE api/tagsrest/5
       // [Authorize(RoleName = "Administrator")]
          [Authorize(Users = "pkellner")]
        public HttpResponseMessage Delete(TagsResult tagItem)
        {
            TagsManager.I.Delete(tagItem);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, tagItem);
            return response;
        }
    }
}
