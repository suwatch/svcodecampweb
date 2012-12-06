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
            List<RSSItem> rssItems = (List<RSSItem>)HttpContext.Current.Cache[cacheName];

            if (rssItems == null)
            {
                rssItems = new List<RSSItem>();
                try
                {
                    XDocument feedXML = XDocument.Load("http://blog.siliconvalley-codecamp.com/feed/");

                    var feeds = feedXML.Descendants("item").Select(feed => new
                                                                               {
                                                                                   PostTitle = feed.Element("title").Value,
                                                                                   PostURL = feed.Element("link").Value
                                                                               });

                    int id = 0;
                    foreach (var rec in feeds)
                    {
                        rssItems.Add(new RSSItem(id, rec.PostTitle, rec.PostURL));
                        id++;
                        if (id >= numberToGet)
                        {
                            break;
                        }
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

        public RSSItem(int id, string postTitle, string postURL)
        {
            Id = id;
            PostTitle = postTitle;
            PostURL = postURL;
        }
    }
}