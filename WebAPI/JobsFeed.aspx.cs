using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using CodeCampSV;

public partial class JobsFeed : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Build the list
        const int maxItemsInFeed = 5;
        var jobDatas = Utils.GetLastPostedJobs(maxItemsInFeed);

        // Determine the maximum number of items to show in the feed
       

        // Determine whether we're outputting an Atom or RSS feed
        bool outputRss = (Request.QueryString["Type"] == "RSS");
        bool outputAtom = !outputRss;

        // Output the appropriate ContentType
        if (outputRss)
            Response.ContentType = "application/rss+xml";
        else if (outputAtom)
            Response.ContentType = "application/atom+xml";

        // Create the feed and specify the feed's attributes
        var myFeed = new SyndicationFeed
                         {
                             Title = TextSyndicationContent.CreatePlaintextContent("Jobs At Silicon Valley Code Camp"),
                             Description =
                                 TextSyndicationContent.CreatePlaintextContent(
                                     "A syndication of the most recently published Jobs At Silicon Valley Code Camp")
                         };
        myFeed.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(GetFullyQualifiedUrl("~/Default.aspx"))));
        myFeed.Links.Add(SyndicationLink.CreateSelfLink(new Uri(GetFullyQualifiedUrl(Request.RawUrl))));
        myFeed.Copyright = TextSyndicationContent.CreatePlaintextContent("Copyright Silicon Valley Code Camp");
        myFeed.Language = "en-us";

        // Creat and populate a list of SyndicationItems
        var feedItems = new List<SyndicationItem>();

        foreach (JobData t in jobDatas.Take(maxItemsInFeed))
        {
            //// Atom items MUST have an author, so if there are no authors for this content item then go to next item in loop
            //if (outputAtom && t.TitleAuthors.Count == 0)
            //    continue;

            var item = new SyndicationItem
                           {
                               Title = TextSyndicationContent.CreatePlaintextContent(t.JobTitle)
                           };

            item.Links.Add(SyndicationLink.CreateAlternateLink(new Uri(GetFullyQualifiedUrl("~/Jobs.aspx"))));
            item.Summary = TextSyndicationContent.CreatePlaintextContent(t.Company);
            //item.Categories.Add(new SyndicationCategory(t.type));
            item.PublishDate = t.JobDatePosted;
           
            

            var authInfo = new SyndicationPerson
                               {
                                   Email = "jobads@siliconvalley-codecamp.com",
                                   Name = "JobAds"
                               };
            item.Authors.Add(authInfo);

            //foreach (TitleAuthor ta in t.TitleAuthors)
            //{
            //    SyndicationPerson authInfo = new SyndicationPerson();
            //    authInfo.Email = ta.Author.au_lname + "@example.com";
            //    authInfo.Name = ta.Author.au_fullname;
            //    item.Authors.Add(authInfo);

            //    // RSS feeds can only have one author, so quit loop after first author has been added
            //    if (outputRss)
            //        break;
            //}

            // Add the item to the feed
            feedItems.Add(item);
        }

        myFeed.Items = feedItems;

        // Return the feed's XML content as the response
        XmlWriterSettings outputSettings = new XmlWriterSettings {Indent = true};
        var feedWriter = XmlWriter.Create(Response.OutputStream, outputSettings);

        if (outputAtom)
        {
            // Use Atom 1.0        
            var atomFormatter = new Atom10FeedFormatter(myFeed);
            atomFormatter.WriteTo(feedWriter);
        }
        else if (outputRss)
        {
            // Emit RSS 2.0
            var rssFormatter = new Rss20FeedFormatter(myFeed);
            rssFormatter.WriteTo(feedWriter);
        }

        feedWriter.Close();

    }

    private string GetFullyQualifiedUrl(string url)
    {
        return string.Concat(Request.Url.GetLeftPart(UriPartial.Authority), ResolveUrl(url));
    }
}

