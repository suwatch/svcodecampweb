using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Newtonsoft.Json.Linq;

namespace WebAPI.App_Start
{
    public class SenchaError
    {
        public bool success { get; set; }
        public string message { get; set; }
    }


    public class ValidationActionFilter : ActionFilterAttribute 
    { 
        public override void OnActionExecuting(HttpActionContext context)
        {
            string errorString = "";
            var modelState = context.ModelState; 
            if (!modelState.IsValid) 
            { 
                var errors = new JObject(); 
                foreach (var key in modelState.Keys) 
                { 
                    var state = modelState[key]; 
                    if (state.Errors.Any()) 
                    { 
                        errors[key] = state.Errors.First().ErrorMessage; 
                        errorString = state.Errors.First().ErrorMessage;
                    } 
                } 
 
                //context.Response = 
                //    context.Request.CreateResponse<JObject>(HttpStatusCode.BadRequest, errors); 

                context.Response =
                    context.Request.CreateResponse<SenchaError>(HttpStatusCode.NotAcceptable, new SenchaError()
                                                                                                  {
                                                                                                     message = errorString,
                                                                                                     success = false
                                                                                                  });


            } 
        } 
    }
}