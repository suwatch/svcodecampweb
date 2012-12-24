using System.Globalization;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using Newtonsoft.Json.Linq;

public class BoxSessionManager
{
    private string ApplicationKey
    {
        get { return ConfigurationManager.AppSettings["BOXapplicationKey"]; }
    }

    private string Authtoken
    {
        get { return ConfigurationManager.AppSettings["BOXauthtoken"]; }
    }

    private string SessionBaseFolderId
    {
        get { return ConfigurationManager.AppSettings["BOXSessionDataFolderId"]; }
    }

    public string GetTicket()
    {
        string myXmlString =
            HttpGet(String.Format("https://www.box.com/api/1.0/rest?action=get_ticket&api_key={0}", ApplicationKey));

        var xml = new XmlDocument();
        xml.LoadXml(myXmlString); // suppose that myXmlString contains "<Names>...</Names>"

        XmlNodeList xnList = xml.SelectNodes("/response");
        string status = "";
        string ticket = "";
        if (xnList != null)
        {
            foreach (XmlNode xn in xnList)
            {
                var element = xn["status"];
                if (element != null) status = element.InnerText;
                var xmlElement = xn["ticket"];
                if (xmlElement != null) ticket = xmlElement.InnerText;
            }
        }
        return status.Equals("get_ticket_ok") ? ticket : "";

    }

    public static string HttpPost(string uri, string parameters, List<string> headers, out string errorDescription)
    {
        return HttpGeneral(uri, "POST", parameters, headers, out errorDescription);
    }

    public static string HttpGeneral(string uri, string verb, string parameters, List<string> headers, out string errorDescription)
    {
        errorDescription = "";
        var req = WebRequest.Create(uri);
        foreach (var header in headers)
        {
            req.Headers.Add(header);
        }
        //Add these, as we're doing a POST
        req.ContentType = "application/x-www-form-urlencoded";
        req.Method = verb;
        byte[] bytes = Encoding.ASCII.GetBytes(parameters);
        req.ContentLength = bytes.Length;

        using (var os = req.GetRequestStream())
        {
            os.Write(bytes, 0, bytes.Length); //Push it out there
            os.Close();
        }
        string retStr = "";
        try
        {
            var resp = req.GetResponse();
            var sr = new System.IO.StreamReader(resp.GetResponseStream());
            retStr = sr.ReadToEnd().Trim();
        }
        catch (Exception e)
        {
            errorDescription = e.ToString();
        }
        return retStr;

    }

    public static string HttpGet(string uri, List<string> headers)
    {
        var req = WebRequest.Create(uri);
        foreach (var header in headers)
        {
            req.Headers.Add(header);
        }
        var resp = req.GetResponse();
        var sr = new System.IO.StreamReader(resp.GetResponseStream());
        return sr.ReadToEnd().Trim();
    }

    private string HttpGet(string uri)
    {
        return HttpGet(uri, new List<string>());
    }


    public string CreateFolder(string folderName, string folderDescription, out string errorDescription)
    {
        string folderIdString = "";
        string rootFolderId = String.IsNullOrEmpty(SessionBaseFolderId) ? "0" : SessionBaseFolderId;

        // https://api.box.com/2.0/folders/FOLDER_ID
        string header = String.Format("Authorization: BoxAuth api_key={0}&auth_token={1}", ApplicationKey, Authtoken);
        string data = JsonConvert.SerializeObject(new
                                                      {
                                                          name = folderName
                                                      });

        string error;
        string retData = HttpPost(string.Format("https://api.box.com/2.0/folders/{0}", rootFolderId), data, new List<string> { header }, out error);
        if (!String.IsNullOrEmpty(retData))
        {
            //JToken token = JObject.Parse(retData);
            //folderIdString = token.SelectToken("id").ToString();

            //Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(retData);
            //folderIdString = values["id"].ToString();

            var rootObject1 = new JavaScriptSerializer().Deserialize<RootObject1>(retData);
            folderIdString = rootObject1.id;

            errorDescription = "";

            // set the description
            // http://developers.box.net/w/page/12923949/ApiFunction_set_description
            string urlSetDescr =
                String.Format(
                    "https://www.box.net/api/1.0/rest?action=set_description&api_key={0}&auth_token={1}&target=folder&target_id={2}&description={3}",
                    ApplicationKey, Authtoken, folderIdString, folderDescription);
            string resp = HttpGet(urlSetDescr);
        }
        else
        {
            errorDescription = error;
        }

        return folderIdString;
    }

    public bool DeleteFolder(string folderId,out string errorString)
    {
        errorString = "";
        string url = String.Format("https://api.box.com/2.0/folders/{0}?recursive=true", folderId);
        string header = String.Format("Authorization: BoxAuth api_key={0}&auth_token={1}", ApplicationKey, Authtoken);

        try
        {
            string errorD1;
            HttpGeneral(url, "DELETE", "", new List<string>() { header }, out errorD1);
            errorString = errorD1;
        }
        catch (Exception e)
        {
            errorString = e.ToString();
        }

        return String.IsNullOrEmpty(errorString);
    }



    /// <summary>
    /// http://developers.box.com/docs/?#folders-create-a-shared-link-for-a-folder
    /// 
    /// 
    /// </summary>
    /// <param name="folderId"></param>
    public string GetPublicUrl(string folderId)
    {
        string urlReturn = "";
        //string urlReturn;
        string url = string.Format("https://api.box.com/2.0/folders/{0}", folderId);
        string header = String.Format("Authorization: BoxAuth api_key={0}&auth_token={1}", ApplicationKey, Authtoken);
        string data = JsonConvert.SerializeObject(new
                                                      {
                                                          shared_link = new
                                                                            {
                                                                                access = "Open"
                                                                            }
                                                      });
        string errorD1;
        var retData = HttpGeneral(url, "PUT", data, new List<string>() { header }, out errorD1);
        if (String.IsNullOrEmpty(errorD1))
        {
            //JToken token = JObject.Parse(retData);
            //var shareLink = token.SelectToken("shared_link");

            //Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(retData);
            //var shareLink = values["shared_link"].ToString();
            //Dictionary<string, object> values1 = JsonConvert.DeserializeObject<Dictionary<string, object>>(shareLink);
            //urlReturn = values1["url"].ToString();

            var rootObject2 = new JavaScriptSerializer().Deserialize<RootObject2>(retData);
            urlReturn = rootObject2.shared_link.url;
        }

        return urlReturn;



    }


    /// <summary>
    /// http://developers.box.net/w/page/35640290/APiFunction_toggle_folder_email
    /// 
    /// </summary>
    /// <param name="folderId"></param>
    /// <returns></returns>
    public string AssociatedUploadEmail(string folderId)
    {

        //       https://www.box.net/api/1.0/rest?action=toggle_folder_email&api_key=rrc1d3ntb53tt6b2vhail6rdtrsxov3v&auth_token=19x43ykyo3bnefz75yyuepxgm6o4rf7a&folder_id=739&enable=1

        //string downloadUrlReturn = "";
        //string urlReturn;
        string url = string.Format("https://www.box.net/api/1.0/rest?action=toggle_folder_email&api_key={0}&auth_token={1}&folder_id={2}&enable=1", ApplicationKey, Authtoken, folderId);
        //string header = String.Format("Authorization: BoxAuth api_key={0}&auth_token={1}", ApplicationKey, Authtoken);


        var myXmlString = HttpGet(url);

        var xml = new XmlDocument();
        xml.LoadXml(myXmlString); // suppose that myXmlString contains "<Names>...</Names>"

        XmlNodeList xnList = xml.SelectNodes("/response");
        string email = "";
        string status = "";
        foreach (XmlNode xn in xnList)
        {
            var element1 = xn["upload_email"];
            if (element1 != null) email = element1.InnerText;
            var element2 = xn["status"];
            if (element2 != null) status = element2.InnerText;
        }
        return status.Equals("s_toggle_folder_email") ? email : "";
    }

}

public class JSONResponse
{
    public string id { get; set; }
}

public class RootObject1
{
    public string type { get; set; }
    public string id { get; set; }
    public string sequence_id { get; set; }
    public string name { get; set; }
    public string created_at { get; set; }
    public string modified_at { get; set; }
    public string description { get; set; }
    public int size { get; set; }
    public string item_status { get; set; }
}

public class RootObject2
{
    public string type { get; set; }
    public string id { get; set; }
    public string sequence_id { get; set; }
    public string name { get; set; }
    public string created_at { get; set; }
    public string modified_at { get; set; }
    public string description { get; set; }
    public int size { get; set; }
    public SharedLink shared_link { get; set; }
}

public class SharedLink
{
    public string url { get; set; }
    public string download_url { get; set; }
    public bool password_enabled { get; set; }
    public object unshared_at { get; set; }
    public int download_count { get; set; }
    public int preview_count { get; set; }
    public string access { get; set; }
}