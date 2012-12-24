<%@ WebHandler Language="C#" Class="Version" %>

using System;
using System.Web;

public class Version : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        context.Response.Write("Version May 5th 2009");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}