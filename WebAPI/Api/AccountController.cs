using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CodeCampSV;
using System.Net.Http.Formatting;

namespace WebAPI.Api
{
    public class AccountController : ApiController
    {

        public class LoginCredentials
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public bool RememberMe { get; set; }
        }

        public class LoginReturnStatus
        {
            public string Status { get; set; }
            public string Message { get; set; }
            public AttendeesResult Data { get; set; }
        }


        [HttpPost]
        [ActionName("Login")]
        public HttpResponseMessage PostLogin(LoginCredentials login)
        {
            // Create a 201 response.
            //{
            //    //Content = new StringContent(update.Status)
            //};
            //response.Headers.Location =
            //    new Uri(Url.Link("DefaultApi", new { action = "status", id = id }));

            var attendeesResult = new AttendeesResult
                                      {
                                          Username = "testuser",
                                          PKID = Guid.NewGuid(),
                                          UserFirstName = "peter"
                                      };

            var loginReturnStatus =
                new LoginReturnStatus
                    {
                        Status = "Success",
                        Message = "",
                        Data = attendeesResult
                    };

            HttpResponseMessage response =
                Request.CreateResponse(HttpStatusCode.OK, loginReturnStatus);


            return response;



        }



        [HttpPost]
        public HttpResponseMessage Loginxxx(FormDataCollection formDataCollection)
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

