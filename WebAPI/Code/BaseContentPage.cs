using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using CodeCampSV;
//using Gurock.SmartInspect;

/// <summary>
/// Summary description for BaseContentPage
/// </summary>
public class BaseContentPage : Page
{
    private readonly bool _smartInspectEnabled = (ConfigurationManager.AppSettings["SmartInspectEnabled"] ?? "").Equals("true");

    //protected Gurock.SmartInspect.Session LogSession
    //{
    //    get
    //    {
    //        return SiAuto.Si[Session.SessionID];
    //    }
    //}

    //protected override void OnSaveStateComplete(EventArgs e)
    //{
    //    base.OnSaveStateComplete(e);
    //    if (_smartInspectEnabled)
    //    {
    //        LogSession.LeaveMethod("BaseContentPage:" + 
    //            HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath);
    //    }


    //}

    protected override void OnPreInit(EventArgs e)
    {
        //if (_smartInspectEnabled)
        //{
        //    LogSession.EnterMethod("BaseContentPage:" +
        //        HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath);
        //}

        base.OnPreInit(e);

        Page.Theme = ConfigurationManager.AppSettings["TestingDataOnly"] != null &&
                     ConfigurationManager.AppSettings["TestingDataOnly"].ToLower().Equals("true")
                         ? String.Format("Gray{0}", "2012")
                         : String.Format("Gray{0}", Utils.GetCurrentCodeCampYearStartDate().Year);

        //int codeCampYear = Utils.GetCurrentCodeCampYear();
        //string dateString = Utils.GetCodeCampDateStringByCodeCampYearId(codeCampYear);


        //if (codeCampYear == 3)
        //{
        //    Page.Theme = "Gray2008";
        //}
        //else if (codeCampYear == 4)
        //{
        //    Page.Theme = "Gray2009";
        //}

        //const string cacheName = "TrackRedirectsCache";
        //const int cacheTimeoutSeconds = 0;

        //var alternateUrlsDictionary = (Dictionary<int,string>)Cache[cacheName];
        //if (alternateUrlsDictionary == null)
        //{
        //    alternateUrlsDictionary = new Dictionary<int,string>();
        //    // Grab all the track custom url's
        //    var trackResults = TrackManager.I.Get(new TrackQuery());
        //    foreach (TrackResult trackResult in trackResults)
        //    {
        //        if (!String.IsNullOrEmpty(trackResult.AlternateURL))
        //        {
        //            alternateUrlsDictionary.Add(trackResult.Id,trackResult.AlternateURL.Trim());
        //        }
        //    }
        //    DateTime cacheExpire = DateTime.Now.AddSeconds(cacheTimeoutSeconds);
        //    Cache.Insert(cacheName, alternateUrlsDictionary, null, cacheExpire, new TimeSpan(0));
        //}

        //foreach (var alternateUrl in alternateUrlsDictionary)
        //{
        //    if (Context.Request.RawUrl.ToLower().EndsWith(alternateUrl.Value.ToLower()))
        //    {
        //        Response.Redirect(String.Format("~/Sessions.aspx?track={0}", alternateUrl.Key), true);
        //    }
        //}
    }
}
