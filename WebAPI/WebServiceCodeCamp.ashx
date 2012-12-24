<%@ WebHandler Language="C#" Class="WebServiceCodeCamp" %>

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.SessionState;
using CodeCampSV;
using Jayrock.JsonRpc;
using Jayrock.JsonRpc.Web;

public class WebServiceCodeCamp : JsonRpcHandler, IRequiresSessionState
{
    [JsonRpcMethod("RetrieveAllData", Idempotent = true)]
    [JsonRpcHelp("Returns information about All Sessions and Logged In User")]
    public WS_Detail RetrieveAllCodeCamp()
    {
        return Utils.RetrieveSessionsAllCodeCamp();
    }


}

   