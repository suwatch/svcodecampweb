<%@ WebHandler Language="C#" Class="Login" %>

using System;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;

public class Login : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        var username = context.Request["username"];
        var password = context.Request["password"];
        var rememberMeIn = context.Request["rememberme"];

        bool rememberMe = !String.IsNullOrEmpty(rememberMeIn) && rememberMeIn.ToLower().Equals("true");

        string json;

        if (context.Request.HttpMethod == "POST")
        {

            if (!String.IsNullOrEmpty(username) &&
                !String.IsNullOrEmpty(password))
            {
                if (Membership.ValidateUser(username, password))
                {
                    FormsAuthentication.SetAuthCookie(username, rememberMe);
                    json = JsonConvert.SerializeObject(new Result {Success = true, Message = ""});

                }
                else
                {
                    json =
                        JsonConvert.SerializeObject(new Result
                                                        {Success = false, Message = "Invalid Username or Password"});
                }
            }
            else
            {
                json =
                    JsonConvert.SerializeObject(new Result
                                                    {
                                                        Success = false,
                                                        Message = "Both password and username must be specified"
                                                    });
            }
        }
        else
        {
            json =
                    JsonConvert.SerializeObject(new Result
                    {
                        Success = false,
                        Message = "only POST allowed"
                    });
        }
        context.Response.ContentType = "application/json";
        context.Response.Write(json);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
    
    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

}

        //[HttpPost]
        //public JsonResult Login(string username, string password, bool? rememberMe)
        //{   

        //    if (String.IsNullOrEmpty(username) ||  username == "x")
        //    {
        //        username = "ekellner99";
        //    }
        //    if (String.IsNullOrEmpty(password) || password == "x")
        //    {
        //        password = "password";
        //    }

        //    rememberMe = rememberMe.HasValue && rememberMe.Value;

        //    var logMessage = String.Format("Login: username:^{0}^  password:^{1}^    rememberMe: {2}", username, password,rememberMe);
        //    Utils.Logger(Utils.LogLevel.Trace, logMessage, LoggerClassName.ToString());

        //    if (Membership.ValidateUser(username, password))
        //    {
               
        //         FormsAuthentication.SetAuthCookie(username, rememberMe.Value);
                

        //        string errorString;
        //        FullUserInfo fullUserInfo;
        //        using (var db = new SiteDB())
        //        {
        //            fullUserInfo = WebUtils.FullUserInfoLocal(out errorString, username, db);
        //            if (fullUserInfo != null)
        //            {
        //                fullUserInfo.AEPassword = string.Empty;
        //            }
        //        }

        //        Utils.Logger(Utils.LogLevel.Info, "Login: succeeded for username passedin:" + username, LoggerClassName.ToString());

        //        return Json(new
        //                        {
        //                            success = true,
        //                            data = fullUserInfo,
        //                            message = errorString
        //                        }, JsonRequestBehavior.DenyGet);
        //    }

        //    Utils.Logger(Utils.LogLevel.Info, "Login: failed for username passedin: " + username, LoggerClassName.ToString());

           

        //    return Json(new
        //                    {
        //                        success = false,
        //                        message = "The user name or password provided is incorrect."
        //                    }, JsonRequestBehavior.DenyGet);
        //}