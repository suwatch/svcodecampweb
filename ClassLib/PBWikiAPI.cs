using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// An API for calling the PBWiki's API.  This is all C# and .NET 2.0
/// </summary>
public class PBWikiAPI
{
    public static string AddPage(string wikiURL, string apiKey, string page, string email,
                                 string name, string postData, out string responseReturn)
    {
        string wikiPageName = string.Empty;
        responseReturn = string.Empty;

        try
        {
            wikiPageName = Regex.Replace(page, @"[^\w@-]", "");
            if (!String.IsNullOrEmpty(wikiPageName))
            {
                string url =
                    string.Format("{0}/api/AddPage?apikey_v1={1}&page={2}&name={3}&email={4}",
                                  wikiURL, apiKey, wikiPageName, name, email);

                //var ourUri = new Uri(url);

                // Create a 'WebRequest' object with the specified url. 
                WebRequest myWebRequest = WebRequest.Create(url);
                myWebRequest.Method = "POST";

                byte[] bytes = Encoding.ASCII.GetBytes(postData);
                myWebRequest.ContentLength = bytes.Length;

                Stream os = myWebRequest.GetRequestStream();
                os.Write(bytes, 0, bytes.Length);
                os.Close();

                // Send the 'WebRequest' and wait for response.
                WebResponse myWebResponse = myWebRequest.GetResponse();
                if (myWebResponse != null)
                {
                    var sr1 = new StreamReader(myWebResponse.GetResponseStream());
                    responseReturn = sr1.ReadToEnd().Trim();
                }

                // Release resources of response object.
                myWebResponse.Close();
            }
            else
            {
                throw new ApplicationException("PBWikiAPI: no Page Name Specified");
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException("PBWikiAPI: " + ex);
        }

        return wikiPageName;
    }
}