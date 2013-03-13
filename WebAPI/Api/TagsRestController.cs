using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

public class TagItem
{
    public  int Id { get; set; }
    public  int SessionId { get; set; }
    public  string TagName { get; set; }
    public  bool TaggedInSession { get; set; }
}

namespace WebAPI.Api
{
    public class TagsRestController : ApiController
    {
        // GET api/tagsrest
        public IEnumerable<TagItem> Get()
        {
            return 
                new List<TagItem>()
                    {
                        new TagItem() {Id=101,SessionId = 7,TagName = "C#",TaggedInSession = true},
                        new TagItem() {Id=102,SessionId = 7,TagName = "JavaScript",TaggedInSession = true},
                        new TagItem() {Id=103,SessionId = 7,TagName = "WebAPI",TaggedInSession = true},
                        new TagItem() {Id=104,SessionId = 7,TagName = "C++",TaggedInSession = false},
                    };
        }

        // GET api/tagsrest/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/tagsrest
        public void Post([FromBody]string value)
        {
        }

        // PUT api/tagsrest/5
        public void Put(TagItem tagItem)
        {

        }

        // DELETE api/tagsrest/5
        public void Delete(int id)
        {
        }
    }
}
