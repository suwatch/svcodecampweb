using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.REST
{
    public class SessionController : ApiController
    {
        // GET api/session
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/session/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/session
        public void Post([FromBody]string value)
        {
        }

        // PUT api/session/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/session/5
        public void Delete(int id)
        {
        }
    }
}
