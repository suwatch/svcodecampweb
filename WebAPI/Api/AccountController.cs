using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CodeCampSV;
using System.Net.Http.Formatting;

namespace WebAPI.Api
{
    public class AccountController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Login(string username,string password)
        {
            //var dict = formDataCollection.ToDictionary(k => k.Key, v => v.Value);

            //// check and see if speaker record
            //if (dict.ContainsKey("SpeakerCanSpeak"))
            //{

            //}
            //else
            //{

            //}



            ////return Request.CreateResponse(HttpStatusCode.BadRequest);

            //Contact contact = new Contact()
            //{
            //    Name = "myname"
            //};


            //var response = Request.CreateResponse<Contact>(HttpStatusCode.Created, contact);
            //response.Headers.Location = new Uri("http://localhost/");
            //return response;
            return Request.CreateResponse(HttpStatusCode.OK);

        }
   
      
        //[HttpPost]
        //public HttpResponseMessage SpeakerReg(FormDataCollection formDataCollection)
        //{
        //    var dict = formDataCollection.ToDictionary(k => k.Key, v => v.Value);

        //    // check and see if speaker record
        //    if (dict.ContainsKey("SpeakerCanSpeak"))
        //    {

        //    }
        //    else
        //    {

        //    }



        //    //return Request.CreateResponse(HttpStatusCode.BadRequest);

        //    Contact contact = new Contact()
        //    {
        //        Name = "myname"
        //    };


        //    var response = Request.CreateResponse<Contact>(HttpStatusCode.Created, contact);
        //    response.Headers.Location = new Uri("http://localhost/");
        //    return response;


        //}

        //// POST api/session
        //public HttpResponseMessage Post(FormDataCollection formDataCollection)
        //{
        //    var dict = formDataCollection.ToDictionary(k => k.Key, v => v.Value);

        //    // check and see if speaker record
        //    if (dict.ContainsKey("SpeakerCanSpeak"))
        //    {

        //    }
        //    else
        //    {

        //    }



        //    //return Request.CreateResponse(HttpStatusCode.BadRequest);

        //    Contact contact = new Contact()
        //    {
        //        Name = "myname"
        //    };


        //    var response = Request.CreateResponse<Contact>(HttpStatusCode.Created, contact);
        //    response.Headers.Location = new Uri("http://localhost/");
        //    return response;


        //}

        //// PUT api/session/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/session/5
        //public void Delete(int id)
        //{
        //}
    }

    public class Contact
    {
       
        public int ContactId { get; set; }
   
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Email { get; set; }
        public string Twitter { get; set; }
       
        public string Self { get; set; }
    }
}
//// GET api/session
//public IEnumerable<string> Get()
//{
//    return new string[] { "value1", "value2" };
//}

//// GET api/session/5
//public string Get(int id)
//{
//    return "value";
//}


//[HttpPost]
//public HttpResponseMessage RegisterSpeaker(FormDataCollection form)
//{

//    var resp = Request.CreateResponse(HttpStatusCode.OK, string.Empty);
//    return resp;

//}

