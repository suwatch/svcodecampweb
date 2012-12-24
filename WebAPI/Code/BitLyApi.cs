using System;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Runtime.InteropServices;

namespace App_Code
{
    [ComVisible(true)]
    public class API
    {
        [ComVisible(true)]
        public static String Bit(string username, string apiKey, string input, string type)
        {
            String Btype;
            String Xtype;
            String Itype;
            switch (type)
            {
                case "Shorten":
                    Btype = "shorten";
                    Xtype = "shortUrl";
                    Itype = "longUrl";
                    break;
                case "MetaH":
                    Btype = "info";
                    Xtype = "htmlMetaDescription";
                    Itype = "hash";
                    break;
                case "MetaU":
                    Btype = "info";
                    Xtype = "htmlMetaDescription";
                    Itype = "shortUrl";
                    break;
                case "ExpandH":
                    Btype = "expand";
                    Xtype = "longUrl";
                    Itype = "hash";
                    break;
                case "ExpandU":
                    Btype = "expand";
                    Xtype = "longUrl";
                    Itype = "shortUrl";
                    break;
                case "ClicksU":
                    Btype = "stats";
                    Xtype = "clicks";
                    Itype = "shortUrl";
                    break;
                case "ClicksH":
                    Btype = "stats";
                    Xtype = "clicks";
                    Itype = "hash";
                    break;
                case "UserU":
                    Btype = "info";
                    Xtype = "shortenedByUser";
                    Itype = "shortUrl";
                    break;
                case "UserH":
                    Btype = "info";
                    Xtype = "shortenedByUser";
                    Itype = "hash";
                    break;
                default:
                    return "";
            }


            StringBuilder url = new StringBuilder();  //Build a new string
            url.Append("http://api.bit.ly/");   //Add base URL
            url.Append(Btype);
            url.Append("?version=2.0.1");             //Add Version
            url.Append("&format=xml");
            url.Append("&");
            url.Append(Itype);
            url.Append("=");
            url.Append(input);                         //Append longUrl from input
            url.Append("&login=");                    //Add login "Key"
            url.Append(username);                     //Append login from input
            url.Append("&apiKey=");                   //Add ApiKey "Key"
            url.Append(apiKey);                       //Append ApiKey from input
            WebRequest request = WebRequest.Create(url.ToString()); //prepare web request
            StreamReader responseStream = new StreamReader(request.GetResponse().GetResponseStream()); //prepare responese holder
            String response = responseStream.ReadToEnd(); //fill up response
            responseStream.Close(); //Close stream

            string data = response.ToString(); //Turn it into a string
            string newdata = XmlParse_general(data, Xtype); //parse the XML
            if (newdata == "Error")
            {
                return "";
            }
            else
            {
                return newdata;
            }
        }

        private static void ComVisible(bool p)
        {
            throw new NotImplementedException();
        }

        private static string XmlParse_general(string Url, string type)    //XML parse Function
        {

            System.Xml.XmlTextReader xmlrt1 = new XmlTextReader(new StringReader(Url));
            while (xmlrt1.Read())
            {
                //Trace.Write("Node Type", xmlrt1.NodeType.ToString());
                string strNodeType = xmlrt1.NodeType.ToString();
                string strName = xmlrt1.Name;

                if (strNodeType == "Element" && strName == type) //get the clicks
                {
                    xmlrt1.Read();
                    return xmlrt1.Value; //Return output
                } // end if
            } return "";// end while
        }
    }
}

