using System.Net;
using System.Net.Http;
using System.Web.Http;
using CodeCampSV;
using System.Linq;

//public class TagItem
//{
//    public  int Id { get; set; }
//    public  int SessionId { get; set; }
//    public  string TagName { get; set; }
//    public  bool TaggedInSession { get; set; }
//}

namespace WebAPI.rest
{

    /// <summary>
    /// this really acts more like a TagsSession controller.  :(
    /// </summary>
    public class TagsController : ApiController
    {
        /// <summary>
        /// if passed in with sessionId then set true or false along with tag. always get all tags
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public HttpResponseMessage Get(int? sessionId)
        {
            var tags =
                TagsManager.I.Get(new TagsQuery
                {
                    SessionId = sessionId
                });

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, tags);
            return response;


            //return 
            //    new List<TagItem>()
            //        {
            //            new TagItem() {Id=101,SessionId = 7,TagName = "C#",TaggedInSession = true},
            //            new TagItem() {Id=102,SessionId = 7,TagName = "JavaScript",TaggedInSession = true},
            //            new TagItem() {Id=103,SessionId = 7,TagName = "WebAPI",TaggedInSession = true},
            //            new TagItem() {Id=104,SessionId = 7,TagName = "C++",TaggedInSession = false},
            //        };
        }

        //// GET api/tagsrest/5
        //public string Get(int sessionId)
        //{
        //    return "value";
        //}

        // POST api/tagsrest
        public HttpResponseMessage Post(TagsResult tagItem)
        {
            HttpResponseMessage response;




            // should really check to see if you own session here also

            if (tagItem.SessionId.HasValue && Utils.IsSessionIdCurrentYear(tagItem.SessionId.Value))
            {
                var tags =
                    TagsManager.I.Get(new TagsQuery
                        {
                            SessionId = tagItem.SessionId
                        });
                if (tags.Count <= 7)
                {
                    TagsManager.I.Insert(tagItem);
                    response = Request.CreateResponse(HttpStatusCode.OK, tagItem);
                }
                else
                {
                    response = Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                           "no more than 7 tags allowed per session");
                }
            }
            else
            {
                response = Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                       "sessionId passed in no good or not this year");

            }
            return response;
        }

       /// <summary>
       /// this only updates the tag itself when sessionId is null or -1
       /// otherwise, it updates the session only and assume the tag did not change
       /// </summary>
       /// <param name="tagItem"></param>
       /// <returns></returns>
        public HttpResponseMessage Put(TagsResult tagItem)
       {
           //return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "error here!!! in tags");


            if (tagItem.SessionId.HasValue && tagItem.SessionId != -1)
            {
                SessionTagsResult sessionTagsResult =
                    SessionTagsManager.I.Get(new SessionTagsQuery
                                                 {
                                                     SessionId = tagItem.SessionId,
                                                     TagId = tagItem.Id
                                                 }).FirstOrDefault();



                if (sessionTagsResult == null && tagItem.TaggedInSession)
                {
                    SessionTagsManager.I.Insert(new SessionTagsResult
                                                    {
                                                        SessionId = tagItem.SessionId,
                                                        TagId = tagItem.Id,
                                                        AssignedToSession = tagItem.TaggedInSession
                                                    });
                }
                else if (sessionTagsResult != null && !tagItem.TaggedInSession)
                {
                    SessionTagsManager.I.Delete(sessionTagsResult.Id);
                }
            }
            else
            {
                TagsManager.I.Update(tagItem);
            }




            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, tagItem);
            return response;
        }

        // DELETE api/tagsrest/5
        public HttpResponseMessage Delete(TagsResult tagItem)
        {
            TagsManager.I.Delete(tagItem);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, tagItem);
            return response;
        }
    }
}
