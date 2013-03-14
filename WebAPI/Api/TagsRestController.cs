//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using CodeCampSV;

////public class TagItem
////{
////    public  int Id { get; set; }
////    public  int SessionId { get; set; }
////    public  string TagName { get; set; }
////    public  bool TaggedInSession { get; set; }
////}

//namespace WebAPI.Api
//{
//    public class TagsRestController : ApiController
//    {
//        // GET api/tagsrest
//        public HttpResponseMessage Get(int sessionId)
//        {
//            var tags =
//                TagsManager.I.Get(new TagsQuery
//                {
//                    SessionId = sessionId
//                });

//            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, tags);
//            return response;


//            //return 
//            //    new List<TagItem>()
//            //        {
//            //            new TagItem() {Id=101,SessionId = 7,TagName = "C#",TaggedInSession = true},
//            //            new TagItem() {Id=102,SessionId = 7,TagName = "JavaScript",TaggedInSession = true},
//            //            new TagItem() {Id=103,SessionId = 7,TagName = "WebAPI",TaggedInSession = true},
//            //            new TagItem() {Id=104,SessionId = 7,TagName = "C++",TaggedInSession = false},
//            //        };
//        }

//        //// GET api/tagsrest/5
//        //public string Get(int sessionId)
//        //{
//        //    return "value";
//        //}

//        // POST api/tagsrest
//        public HttpResponseMessage Post(TagsResult tagItem)
//        {
//            TagsManager.I.Insert(tagItem);
//            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, tagItem);
//            return response;
//        }

//        // PUT api/tagsrest/5
//        public HttpResponseMessage Put(TagsResult tagItem)
//        {
//            TagsManager.I.Update(tagItem);
//            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, tagItem);
//            return response;
//        }

//        // DELETE api/tagsrest/5
//        public HttpResponseMessage Delete(TagsResult tagItem)
//        {
//            TagsManager.I.Delete(tagItem);
//            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, tagItem);
//            return response;
//        }
//    }
//}
