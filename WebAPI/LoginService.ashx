<%@ WebHandler Language="C#" Class="LoginService"   %>

using System;
using System.Web;
using Newtonsoft.Json;

public class LoginService : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        object retJSON = null;

        var usernamePost = context.Request["username"];
        var passwordPost = context.Request["password"];

        if (!String.IsNullOrEmpty(usernamePost) &&
            !String.IsNullOrEmpty(passwordPost))
        {
            if (System.Web.Security.Membership.ValidateUser(usernamePost, passwordPost))
            {
                retJSON = new
                              {
                                  success = true
                              };
                System.Web.Security.FormsAuthentication.SetAuthCookie(usernamePost, true);
            }
            else
            {
                retJSON = new
                              {
                                  success = false,
                                  msg = "Username/Password Not Valid"
                              };
            }
        }
        else
        {
            retJSON = new
                          {
                              success = false,
                              msg = "Failure! " + "Post Variables username and password required"
                          };
        }

        string s = JsonConvert.SerializeObject(retJSON);
        context.Response.Write(s);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}