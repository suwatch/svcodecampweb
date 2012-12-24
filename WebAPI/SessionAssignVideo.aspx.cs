using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;
using ICSharpCode.SharpZipLib.Zip;

public partial class SessionAssignVideo : System.Web.UI.Page
{
    private bool _successfulUpoad;

    protected int? SessionId
    {
        get { return (int?) ViewState["SessionAssignVideoSessionId"] ?? 0; }
        set
        {
            ViewState["SessionAssignVideoSessionId"] = value;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["SessionId"] != null && !IsPostBack)
        {
            SessionId = Convert.ToInt32(Request.QueryString["SessionId"]);
           
        }

        HyperLinkSession.NavigateUrl = string.Format("~/Sessions.aspx?OnlyOne=true&id={0}", SessionId);

        if (SessionId > 0 && !IsPostBack)
        {
            var sessionVideoResult = SessionVideoManager.I.Get(new SessionVideoQuery() {SessionId = SessionId}).FirstOrDefault();
            if (sessionVideoResult != null)
            {
                var videoResult =
                           VideoManager.I.Get(new VideoQuery() { Id = sessionVideoResult.VideoId }).FirstOrDefault();

                TextBoxYoutubeURL.Text = videoResult.YouTubeURL;
                TextBoxDescrText.Text = videoResult.DescriptionText;

            }
        }

        _successfulUpoad = false;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        ImageStaticVideo.ImageUrl = string.Format("~/DisplayImage.ashx?sizex=80&SessionIdVideo={0}&Cache=false", SessionId);

        if (_successfulUpoad)
        {
            Response.Redirect(@"~\SessionAssignVideo.aspx?SessionId=" + SessionId,true);
        }


    }

    protected void ButtonUploadClick(object sender, EventArgs e)
    {
        // first, try and load original record if it is there
        var sessionVideoResult = SessionVideoManager.I.Get(new SessionVideoQuery {SessionId = SessionId}).FirstOrDefault();
        var videoResult = new VideoResult();
        if (sessionVideoResult != null)
        {
            videoResult = VideoManager.I.Get(new VideoQuery() {Id = sessionVideoResult.VideoId}).FirstOrDefault();
        }

        // either there is a new file ... or there was a file previously loaded
        if ((FileUploadStaticImage.HasFile && SessionId > 0) || (videoResult != null && videoResult.PictureBytes.Length > 0))
        {
            Stream stream = FileUploadStaticImage.FileContent;
            string fileName = FileUploadStaticImage.FileName;

            // first delete what might have been there before

            if (sessionVideoResult != null)
            {
                SessionVideoManager.I.Delete(sessionVideoResult.Id);
                VideoManager.I.Delete(videoResult.Id);
            }


            if (sessionVideoResult == null)
            {
                videoResult = new VideoResult()
                                  {
                                      CreatedDate = DateTime.Now
                                  };

                sessionVideoResult = new SessionVideoResult
                                         {
                                             SessionId = SessionId.Value
                                         };
            }



            if ((fileName.ToLower().EndsWith("jpg") || fileName.ToLower().EndsWith("png")) && FileUploadStaticImage.HasFile)
            {
                int iLen = Convert.ToInt32(stream.Length);
                var uploadedByteArray = new byte[iLen];
                stream.Read(uploadedByteArray, 0, iLen);
                videoResult.PictureBytes = uploadedByteArray;
            }

            if (!String.IsNullOrEmpty(TextBoxDescrText.Text))
            {
                videoResult.DescriptionText = TextBoxDescrText.Text;
            }

            if (!String.IsNullOrEmpty(TextBoxYoutubeURL.Text))
            {
                videoResult.YouTubeURL = TextBoxYoutubeURL.Text;
            }

            VideoManager.I.Insert(videoResult);
            sessionVideoResult.VideoId = videoResult.Id;
            SessionVideoManager.I.Insert(sessionVideoResult);

            DeleteCache();
        }
    }

    private void DeleteCache()
    {
        string cacheString1 = String.Format("GetSessionVideoURL-{0}", SessionId);
        string cacheString2 = String.Format("AttendeeByUserName?SessionId={0}&sizex=80", SessionId);
        if (HttpContext.Current.Cache[cacheString1] != null)
        {
            try
            {
                if (Cache[cacheString1] != null)
                {
                    Cache.Remove(cacheString1);
                }
            }
            catch
            {
                // possible cache could expire after check causing thrown error
                //Console.WriteLine(exception);
            }
        }

        if (HttpContext.Current.Cache[cacheString2] != null)
        {
            try
            {
                if (Cache[cacheString2] != null)
                {
                    Cache.Remove(cacheString2);
                }

                //HttpContext.Current.Cache.Remove((string) HttpContext.Current.Cache[cacheString]);
            }
            catch
            {
                // possible cache could expire after check causing thrown error
                //Console.WriteLine(exception);
            }
        }

    }

    protected void ButtonDeleteClick(object sender, EventArgs e)
    {
        // first delete what might have been there before
        var recs = SessionVideoManager.I.Get(new SessionVideoQuery() { SessionId = SessionId });
        foreach (var rec in recs)
        {
            SessionVideoManager.I.Delete(rec.Id);
        }
        DeleteCache();
        Response.Redirect(@"~\SessionAssignVideo.aspx?SessionId=" + SessionId, true);
    }
}