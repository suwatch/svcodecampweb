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
    public class SessionLevelController : ApiController
    {
        /// <summary>
        /// if passed in with sessionId then set true or false along with tag. always get all tags
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {
            var sessionLevels =
                SessionLevelsManager.I.GetAll();

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, sessionLevels);
            return response;
        }

        
    }
}
