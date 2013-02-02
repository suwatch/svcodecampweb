using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using CodeCampSV;

namespace WebAPI.Code
{
    [DataObject(true)]
    public class RSSFeedObject
    {

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<RSSItem> Get(int numberToGet)
        {
            string cacheName = String.Format("{0}_{1}", Utils.CacheRSSFeed, numberToGet);
            var rssItems = (List<RSSItem>)HttpContext.Current.Cache[cacheName];

            if (rssItems == null)
            {
                rssItems = new List<RSSItem>();
                try
                {
                    XDocument feedXML = XDocument.Load("http://blog.siliconvalley-codecamp.com/feed/");

                    var feeds = feedXML.Descendants("item").Select(feed =>
                                                                       {
                                                                           var xElement = feed.Element("title");
                                                                           var element = feed.Element("link");
                                                                           var pubDate = feed.Element("pubDate");
                                                                           if (element != null)
                                                                               if (pubDate != null)
                                                                                   return xElement != null
                                                                                              ? new
                                                                                                    {
                                                                                                        PostTitle =
                                                                                                    xElement.Value,
                                                                                                        PostURL =
                                                                                                    element.Value,
                                                                                                        pubDate =
                                                                                                    pubDate.Value
                                                                                                    }
                                                                                              : null;
                                                                           return null;
                                                                       }).Take(numberToGet);

                    int id = 0;
                    foreach (var rec in feeds)
                    {
                        rssItems.Add(new RSSItem(id, rec.PostTitle, rec.PostURL,rec.pubDate));
                        id++;
                    }
                }
                catch (Exception)
                {
                    // do nothing if can't hit blog
                }

                HttpContext.Current.Cache.Insert(cacheName, rssItems, null, 
                    DateTime.Now.Add(new TimeSpan(0, 0, Utils.RetrieveSecondsForSessionCacheTimeout())), 
                    TimeSpan.Zero);

            }
            return rssItems;                    
        }

    }

    public class RSSItem
    {
        [DataObjectField(true)]
        public int Id { get; set; }

        [DataObjectField(false)]
        public string PostTitle { get; set; }

        [DataObjectField(false)]
        public string PostURL { get; set; }

        [DataObjectField(false)]
        public DateTime PubDate { get; set; }

        [DataObjectField(false)]
        public String PubDateMonthYearOnly { get; set; }



        public RSSItem(int id, string postTitle, string postURL, string pubDate)
        {
            DateTime pubDateLocal = ConvertToDateTime(pubDate);

            Id = id;
            PostTitle = postTitle;
            PubDate = pubDateLocal;
            PostURL = postURL;
            PubDateMonthYearOnly = String.Format("{0:mm/d}", pubDateLocal);
        }

        private DateTime ConvertToDateTime(string pubDate)
        {
            string newstring = String.Format("{0:MM/dd/yyyy hh:mm tt}", DateTime.Now);
            if (pubDate.Length > pubDate.IndexOf(" +", StringComparison.Ordinal) && pubDate.IndexOf("+", StringComparison.Ordinal) > 0)
            {
                newstring = String.Format("{0:MM/dd/yyyy hh:mm tt}", DateTime.Parse(pubDate.Remove(pubDate.IndexOf(" +", StringComparison.Ordinal))));
             
            }
            return Convert.ToDateTime(newstring);
        }
    }
}