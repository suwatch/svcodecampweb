<%@ WebHandler Language="C#" Class="AttendeeZipCode" %>

using System;
using System.Collections.Generic;
using System.Web;
using CodeCampSV;
using Newtonsoft.Json;

public class AttendeeZipCode : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        //List<WS_LatLon> wsLatLon = new List<WS_LatLon>();
        //context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //context.Response.ContentType = "text/plain";
        //if (context.Request.QueryString["AddPaulKing"] != null)
        //{
        //    wsLatLon = Utils.GetAllLatLonAddPaulKing();
        //}
        //else
        //{
        //    wsLatLon = Utils.GetAllLatLon();
        //}
        //string json = JavaScriptConvert.SerializeObject(wsLatLon);
        //context.Response.Write(json);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}