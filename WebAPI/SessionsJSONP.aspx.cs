using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;

public partial class SessionsJSONP : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["jsonp"]))
            this.JsonPCallback();
    }

    public void JsonPCallback()
    {
        string Callback = Request.QueryString["jsonp"];
        if (!string.IsNullOrEmpty(Callback))
        {
            var sessionsQuery = new SessionsQuery();
            if (HttpContext.Current.Request["query"] != null)
            {
                sessionsQuery = HttpContext.Current.Request["query"].FromJson<SessionsQuery>();
            }
            sessionsQuery.CodeCampYearId = Utils.CurrentCodeCampYear;

            sessionsQuery.WithTags = true;


            if (HttpContext.Current.Request["start"] != null && HttpContext.Current.Request["limit"] != null)
            {
                sessionsQuery.Start = Convert.ToInt32(HttpContext.Current.Request["start"]);
                sessionsQuery.Limit = Convert.ToInt32(HttpContext.Current.Request["limit"]);
            }

            var listData1 = new List<SessionsResult>();
            var sessionsManager = new SessionsManager();
            //if (HttpContext.Current.User.Identity.IsAuthenticated
            // && Utils.CheckUserIsAdmin())
            {
                listData1 = sessionsManager.Get(sessionsQuery);
                for (int i = 0; i < listData1.Count; i++)
                {
                    listData1[i].AdminComments = null;
                    listData1[i].InterestCount = "";
                    listData1[i].InterestedCount = 0;
                    listData1[i].NotInterestedCount = 0;
                    listData1[i].PlanAheadCount = "";
                    listData1[i].PlanAheadCountInt = 0;
                    listData1[i].SessionEvalsResults = null;
                    listData1[i].SpeakerPictureUrl = "http://www.siliconvalley-codecamp.com/" + listData1[i].SpeakerPictureUrl;
                    listData1[i].TitleWithPlanAttend = "";
                    listData1[i].Username = "";
                    listData1[i].WillAttendCount = 0;
                    listData1[i].WikiURL = "";
                    listData1[i].SpeakersList = null;


                }
            }

            var listData2 = (from data in listData1 orderby data.SessionTime, data.Title.ToUpper() select data).ToList();

            var ret = new { success = true, rows = listData2, total = sessionsQuery.OutputTotal };
            //HttpContext.Current.Response.ContentType = "text/plain";
            //HttpContext.Current.Response.Write(ret.ToJson());




            // *** Do whatever you need
            //Response.Write(Callback + "( {\"x\":10 , \"y\":100} );");

            Response.Write(Callback + "( " + ret.ToJson() + " );");
        }

        Response.End();
    }
}