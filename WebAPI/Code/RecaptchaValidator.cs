// Type: Recaptcha.RecaptchaValidator
// Assembly: Recaptcha, Version=1.0.5.0, Culture=neutral, PublicKeyToken=9afc4d65b28c38c2
// Assembly location: C:\temp\WebApplication1\WebApplication1\bin\Recaptcha.dll

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace WebAPI.Code
{
    public class RecaptchaValidator
    {
        private const string VerifyUrl = "http://www.google.com/recaptcha/api/verify";
        private string privateKey;
        private string remoteIp;
        private string challenge;
        private string response;
        private IWebProxy proxy;

        public string PrivateKey
        {
            get
            {
                return this.privateKey;
            }
            set
            {
                this.privateKey = value;
            }
        }

        public string RemoteIP
        {
            get
            {
                return this.remoteIp;
            }
            set
            {
                IPAddress ipAddress = IPAddress.Parse(value);
                if (ipAddress == null || ipAddress.AddressFamily != AddressFamily.InterNetwork && ipAddress.AddressFamily != AddressFamily.InterNetworkV6)
                    throw new ArgumentException("Expecting an IP address, got " + (object)ipAddress);
                this.remoteIp = ipAddress.ToString();
            }
        }

        public string Challenge
        {
            get
            {
                return this.challenge;
            }
            set
            {
                this.challenge = value;
            }
        }

        public string Response
        {
            get
            {
                return this.response;
            }
            set
            {
                this.response = value;
            }
        }

        public IWebProxy Proxy
        {
            get
            {
                return this.proxy;
            }
            set
            {
                this.proxy = value;
            }
        }

        private void CheckNotNull(object obj, string name)
        {
            if (obj == null)
                throw new ArgumentNullException(name);
        }

        public RecaptchaResponse Validate()
        {
            this.CheckNotNull((object)this.PrivateKey, "PrivateKey");
            this.CheckNotNull((object)this.RemoteIP, "RemoteIp");
            this.CheckNotNull((object)this.Challenge, "Challenge");
            this.CheckNotNull((object)this.Response, "Response");
            if (this.challenge == string.Empty || this.response == string.Empty)
                return RecaptchaResponse.InvalidSolution;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.google.com/recaptcha/api/verify");
            httpWebRequest.ProtocolVersion = HttpVersion.Version10;
            httpWebRequest.Timeout = 30000;
            httpWebRequest.Method = "POST";
            httpWebRequest.UserAgent = "reCAPTCHA/ASP.NET";
            if (this.proxy != null)
                httpWebRequest.Proxy = this.proxy;
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            byte[] bytes = Encoding.ASCII.GetBytes(string.Format("privatekey={0}&remoteip={1}&challenge={2}&response={3}", (object)HttpUtility.UrlEncode(this.PrivateKey), (object)HttpUtility.UrlEncode(this.RemoteIP), (object)HttpUtility.UrlEncode(this.Challenge), (object)HttpUtility.UrlEncode(this.Response)));
            using (Stream requestStream = ((WebRequest)httpWebRequest).GetRequestStream())
                requestStream.Write(bytes, 0, bytes.Length);
            string[] strArray;
            try
            {
                using (WebResponse response = httpWebRequest.GetResponse())
                {
                    using (TextReader textReader = (TextReader)new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        strArray = textReader.ReadToEnd().Split(new string[2]
            {
              "\n",
              "\\n"
            }, StringSplitOptions.RemoveEmptyEntries);
                }
            }
            catch (WebException ex)
            {
                EventLog.WriteEntry("Application", ex.Message, EventLogEntryType.Error);
                return RecaptchaResponse.RecaptchaNotReachable;
            }
            switch (strArray[0])
            {
                case "true":
                    return RecaptchaResponse.Valid;
                case "false":
                    return new RecaptchaResponse(0 != 0, strArray[1].Trim(new char[1]
          {
            '\''
          }));
                default:
                    throw new InvalidProgramException("Unknown status response.");
            }
        }
    }
}
