using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CodeCampSV;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

public partial class SessionsEdit : BaseContentPage
{
    int sessionsId = -1;
    int attendeeId = -1;

    private int sponsorId = 0;

    private SessionsResult _sessionsResults;

    public int SessionId
    {
        get
        {
            if (ViewState["MySessionId"] != null)
            {
                return (int) ViewState["MySessionId"];
            }

            return -1;
        }
        set
        {
            ViewState["MySessionId"] = value;
        }
    }




    protected void Page_PreRender(object senter,EventArgs e)
    {

        if (Utils.CheckUserIsSpeakerAssignOwnMaterialsUrl() || Utils.CheckUserIsAdmin())
        {
            LabelMaterialUrl.Visible = true;
            TextBoxMaterialUrl.Visible = true;
        }


        DivAssignedSponsor.Visible = false;
        if (Utils.CheckUserIsAdmin())
        {
            DivAssignedSponsor.Visible = true;
            DropDownListSponsors.DataBind();
            if (sponsorId > 0)
            {
                ListItem listItem = DropDownListSponsors.Items.FindByValue(sponsorId.ToString());
                if (listItem != null)
                {
                    listItem.Selected = true;
                }
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //FormsAuthentication.SetAuthCookie("pkellner", true);
        bool keepGoing = true;

       
        if (Request.QueryString["id"] != null)
        {
            Int32.TryParse((string)Request.QueryString["id"], out sessionsId);
            SessionId = sessionsId;
        }
        else
        {
            keepGoing = false;
            //keepGoing = true;
            //sessionsId = 5;
        }

        if (!IsPostBack)
        {
            if (Utils.CheckUserIsAdmin() || Utils.CheckUserIsRemovePrimarySpeaker())
            {
                DivRemovePrimarySpeakerFromSessionDisplay.Visible = true;
            }
            else
            {
                DivRemovePrimarySpeakerFromSessionDisplay.Visible = false;
            }

            _sessionsResults = SessionsManager.I.Get(new SessionsQuery()
                                                            {
                                                                Id = sessionsId,
                                                                WithSpeakers = true
                                                            }).FirstOrDefault();

            //SessionsODS sessionsODS = new SessionsODS();
            //List<SessionsODS.DataObjectSessions> sessionsLi =
            //    sessionsODS.GetByPrimaryKeySessions(sessionsId);

            //SessionsODS.DataObjectSessions sessionInfo = null;


            if (_sessionsResults != null && keepGoing)
            {
                if (Context.User.Identity.IsAuthenticated)
                {
                    if (!_sessionsResults.Username.Equals(Context.User.Identity.Name) && !CodeCampSV.Utils.CheckUserIsAdmin())
                    {
                        keepGoing = false;
                    }
                }
                else
                {
                    keepGoing = false;
                }
            }

            if (keepGoing)
            {
                if (_sessionsResults != null)
                {
                    DisplayData(_sessionsResults);
                }
            }
            else
            {
                ButtonUpdate1.Enabled = false;
                ButtonUpdate2.Enabled = false;
            }
        }
    }

    private void DisplayData(SessionsResult sessionInfo)
    {
        if (sessionInfo == null)
        {
            throw new ApplicationException("sesionInfo is null in DisplayData");
        }


        if (sessionInfo.SponsorId.HasValue)
        {
            sponsorId = sessionInfo.SponsorId.Value;
        }

        if (!String.IsNullOrEmpty(sessionInfo.BoxFolderIdString))
        {
            UploadToBoxMailToHrefId.Visible = true;
            UploadToBoxMailToHrefId.HRef = String.Format("mailto:{0}?subject=Attach Slides and Code Files For Upload To Your Session And Send",sessionInfo.BoxFolderEmailInAddress);
            UploadToBoxMailToHrefId.InnerText = "mailto: Custom Mailer That Attaches To Your Session For All Attendees";
            

            ButtonCreateMailInId.Visible = false;
            ButtonDeleteSlidesFolderId.Visible = true;
        }
        else
        {
            UploadToBoxMailToHrefId.Visible = false;
            ButtonCreateMailInId.Visible = true;
            ButtonDeleteSlidesFolderId.Visible = false;
        }


        TextBoxMaterialUrl.Text = sessionInfo.SessionsMaterialUrl ?? "";

        LabelPresenter.Text = Context.Server.HtmlDecode(CodeCampSV.Utils.GetAttendeeNameByUsername(sessionInfo.Username));
        LabelPresenterAdditional.Text = Context.Server.HtmlDecode(CodeCampSV.Utils.GetSecondaryPresentersBySessionIdString(sessionInfo.Id,sessionInfo.Username));
        TextBoxSummary.Text = Context.Server.HtmlDecode(sessionInfo.Description);
        TextBoxTitle.Text = Context.Server.HtmlDecode(sessionInfo.Title);
        TextBoxHashTags.Text = Context.Server.HtmlDecode(sessionInfo.TwitterHashTags);
        CheckBoxDoNotShowPrimarySpeaker.Checked = sessionInfo.DoNotShowPrimarySpeaker;
        CheckBoxQueueEmailNotification.Checked = sessionInfo.SessionsMaterialQueueToSend.HasValue && sessionInfo.SessionsMaterialQueueToSend.Value;
 //       TextBoxSlides.Text = sessionInfo.SessionsMaterialUrl;

       
        if (sessionInfo.SessionLevel_id == 1)
        {
            DropDownListLevels.SelectedIndex = 0;
        }
        else if (sessionInfo.SessionLevel_id == 2)
        {
            DropDownListLevels.SelectedIndex = 1;
        }
        else if (sessionInfo.SessionLevel_id == 3)
        {
            DropDownListLevels.SelectedIndex = 2;
        }

        TagsODS tagsODS = new TagsODS();
        List<TagsODS.DataObjectTags> tagsODSli =
            tagsODS.GetAllBySession(string.Empty, sessionsId,false);

        CheckBoxListTags.DataBind();

        foreach (ListItem li in CheckBoxListTags.Items)
        {
            foreach (TagsODS.DataObjectTags dot in tagsODSli)
            {
                if (dot.Tagname.Equals(li.Text))
                {
                    li.Selected = true;
                }
            }
        }

        if (sessionInfo.SpeakersList != null && sessionInfo.SpeakersList.Count == 1)
        {
            SpeakerHandle2Tr.Visible = false;
            SpeakerHandle3Tr.Visible = false;
        }

        if (sessionInfo.SpeakersList != null && sessionInfo.SpeakersList.Count == 2)
        {
            SpeakerHandle3Tr.Visible = false;
        }

        if (SpeakerHandle1Tr.Visible && sessionInfo.SpeakersList != null && sessionInfo.SpeakersList.Count > 0)
        {
            SpeakerName1.Text = sessionInfo.SpeakersList[0].UserFirstName  + " " +
                                  sessionInfo.SpeakersList[0].UserLastName;
            TextBoxSpeakerHandle1.Text = sessionInfo.SpeakersList[0].TwitterHandle ?? "";
            SpeakerHandle1Id.Text = sessionInfo.SpeakersList[0].AttendeeId.ToString(CultureInfo.InvariantCulture);
        }

        if (SpeakerHandle2Tr.Visible && sessionInfo.SpeakersList != null && sessionInfo.SpeakersList.Count > 1)
        {
            SpeakerName2.Text = sessionInfo.SpeakersList[1].UserFirstName + " " +
                                  sessionInfo.SpeakersList[1].UserLastName;
            TextBoxSpeakerHandle2.Text = sessionInfo.SpeakersList[1].TwitterHandle ?? "";
            SpeakerHandle2Id.Text = sessionInfo.SpeakersList[1].AttendeeId.ToString(CultureInfo.InvariantCulture);
        }

        if (SpeakerHandle3Tr.Visible && sessionInfo.SpeakersList != null && sessionInfo.SpeakersList.Count > 2)
        {
            SpeakerName3.Text = sessionInfo.SpeakersList[2].UserFirstName + " " +
                                  sessionInfo.SpeakersList[2].UserLastName;
            TextBoxSpeakerHandle3.Text = sessionInfo.SpeakersList[2].TwitterHandle ?? "";
            SpeakerHandle3Id.Text = sessionInfo.SpeakersList[2].AttendeeId.ToString(CultureInfo.InvariantCulture);
        }





    }
    protected void ButtonUpdate1_Click(object sender, EventArgs e)
    {
        UpdateSessionInfo();
    }

    protected void ButtonUpdate2_Click(object sender, EventArgs e)
    {
        UpdateSessionInfo();
    }

    private void UpdateSessionInfo()
    {
       

        //SessionsODS sessionsODS = new SessionsODS();

        //List<SessionsODS.DataObjectSessions> sessionsLi =
        //    sessionsODS.sessionsResult(sessionsId);

        var sessionsResults = SessionsManager.I.Get(new SessionsQuery() { Id = sessionsId }).FirstOrDefault();


        if (sessionsResults != null)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                if (!sessionsResults.Username.Equals(Context.User.Identity.Name) && !Utils.CheckUserIsAdmin())
                {
                    return; // kluky, but just fast check
                }
            }
            else
            {
                return; // kluky, but just fast check
            }

            attendeeId = Utils.GetAttendeesIdFromUsername(sessionsResults.Username);
        }


        // remove cache
        // break existing cache
        string keyToRemove = String.Format("SessionsAllByStartTime--1-{0}", Utils.GetCurrentCodeCampYear().ToString());
        HttpContext.Current.Cache.Remove(keyToRemove);

        keyToRemove = String.Format("CacheGetSecondaryPresentersBySessionId-{0}-{1}-{2}", sessionsResults.Id, sessionsResults.Username, "True");
        HttpContext.Current.Cache.Remove(keyToRemove);

        keyToRemove = String.Format("CacheGetSecondaryPresentersBySessionId-{0}-{1}-{2}", sessionsResults.Id, sessionsResults.Username, "False");
        HttpContext.Current.Cache.Remove(keyToRemove);


        // first, if new session requested, add it
        if (!String.IsNullOrEmpty(TextBoxAdditionalSpeakerLoginName.Text))
        {
            // check and see if this is a real username
            int attendeeIdFromAddSpeakerTextBox = Utils.GetAttendeesIdFromUsername(TextBoxAdditionalSpeakerLoginName.Text.Trim());
            if (attendeeIdFromAddSpeakerTextBox > 0)
            {
                // verify person not in session already
                List<int> excludeAttendeeIdList =
                    (from data in Utils.GetSecondaryPresentersBySessionId(sessionsId, string.Empty)
                     // by leaving second parameter empty, will get all
                     select data.Id).ToList();
                if (!excludeAttendeeIdList.Contains(attendeeIdFromAddSpeakerTextBox) && !CheckBoxRemoveAttendee.Checked)
                {
                    Utils.AddPresenterToSession(sessionsId, attendeeIdFromAddSpeakerTextBox);
                }

                // don't allow person to remove themselves from session
                if (CheckBoxRemoveAttendee.Checked && attendeeIdFromAddSpeakerTextBox != attendeeId)
                {
                    Utils.RemovePresenterToSession(sessionsId, attendeeIdFromAddSpeakerTextBox);
                }
            }
        }


        List<int> updateTags = UpdateNewTags();

        foreach (ListItem li in CheckBoxListTags.Items)
        {
            if (li.Selected)
            {
                int tagId = Convert.ToInt32(li.Value);
                updateTags.Add(tagId);
            }
        }


        SessionsODS sessionsODS = new SessionsODS();
        sessionsODS.UpdateSession(sessionsId,
                                  //TextBoxSlides.Text,
                                  "",
                                  CheckBoxQueueEmailNotification.Checked,
                                  CheckBoxDoNotShowPrimarySpeaker.Checked,
                                  Context.Server.HtmlEncode(TextBoxTitle.Text),
                                  Context.Server.HtmlEncode(TextBoxHashTags.Text),
                                  Context.Server.HtmlEncode(TextBoxSummary.Text),
                                  DropDownListLevels.SelectedIndex + 1,
                                  updateTags,Convert.ToInt32(DropDownListSponsors.SelectedValue));

        if (SpeakerHandle1Tr.Visible)
        {
            Utils.UpdateTwitterHandleForAttendee(SpeakerHandle1Id.Text,
                                                 TextBoxSpeakerHandle1.Text);                    
        }

        if (SpeakerHandle2Tr.Visible)
        {
            Utils.UpdateTwitterHandleForAttendee(SpeakerHandle2Id.Text,
                                                 TextBoxSpeakerHandle2.Text);
        }

        if (SpeakerHandle3Tr.Visible)
        {
            Utils.UpdateTwitterHandleForAttendee(SpeakerHandle3Id.Text,
                                                 TextBoxSpeakerHandle3.Text);
        }

        if ((Utils.CheckUserIsAdmin() || Utils.CheckUserIsSpeakerAssignOwnMaterialsUrl())  && !String.IsNullOrEmpty(TextBoxMaterialUrl.Text))
        {
            Utils.UpdateSessionMaterialUrl(sessionsId,TextBoxMaterialUrl.Text);
        }

        



        DisplayData(sessionsResults);

        CodeCampSV.Utils.CacheClear("sessions");
        String tagsCache = CodeCampSV.Utils.CacheTagsGetBySession + "_" + sessionsId.ToString(CultureInfo.InvariantCulture);
        HttpContext.Current.Cache.Remove(tagsCache);

        Response.Redirect("~/Sessions.aspx?id=");

    }

    private List<int> UpdateNewTags()
    {
        List<int> newTags = new List<int>();
        Dictionary<string, int> tagsDictionaryByName = new Dictionary<string, int>();

        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
        {
            sqlConnection.Open();
            SqlDataReader sqlReaderTagListAll = null;
            try
            {
                // get list of existing categories to make sure doesn't exist already
                string sqlSelectAllCategories = "SELECT TagName,id FROM Tags";
                SqlCommand sqlCommandTagList = new SqlCommand(sqlSelectAllCategories, sqlConnection);
                sqlReaderTagListAll = sqlCommandTagList.ExecuteReader();
                while (sqlReaderTagListAll.Read())
                {
                    String tagName = sqlReaderTagListAll.GetString(0).ToLower().Trim();
                    int tagId = sqlReaderTagListAll.GetInt32(1);
                    if (!tagsDictionaryByName.ContainsKey(tagName))
                    {
                        tagsDictionaryByName.Add(tagName, tagId);
                    }
                }
            }
            finally
            {
                sqlReaderTagListAll.Close();
                sqlReaderTagListAll.Dispose();
            }


            // first see if new categories to add.

            List<string> categoriesToAdd = new List<string>();
            if (!String.IsNullOrEmpty(TextBoxAddCategories.Text))
            {
                try
                {
                    if (TextBoxAddCategories.Text.Length < 200)
                    {  // sanity check
                        char[] delimiterChars = { ',', '.', ':', ';' };
                        string[] categoriesPass1 = TextBoxAddCategories.Text.Split(delimiterChars);
                        foreach (string category in categoriesPass1)
                        {
                            if (!tagsDictionaryByName.ContainsKey(category.ToLower().Trim()))
                            {
                                categoriesToAdd.Add(category);
                            }
                        }
                    }
                }
                finally
                {
                    sqlReaderTagListAll.Close();
                    sqlReaderTagListAll.Dispose();
                }
            }

            // finally, add the new categories.
            if (categoriesToAdd.Count > 0)
            {

                string sqlInsertTag =
                   "INSERT INTO  dbo.Tags(TagName) VALUES (@TagName);SELECT @@identity";
                SqlCommand sqlCommandInsertTag = new SqlCommand(sqlInsertTag, sqlConnection);
                sqlCommandInsertTag.Parameters.Add("TagName", SqlDbType.VarChar).Value = string.Empty;
                foreach (string newCategory in categoriesToAdd)
                {
                    sqlCommandInsertTag.Parameters["TagName"].Value = newCategory;
                    int idTag = Convert.ToInt32((Decimal)sqlCommandInsertTag.ExecuteScalar());
                    newTags.Add(idTag);
                }
            }
        }
        return newTags;
    }
    protected void ButtonCreateMailInId_Click(object sender, EventArgs e)
    {
        var session = SessionsManager.I.Get(new SessionsQuery { Id = SessionId,WithSpeakers = true}).FirstOrDefault();
        if (session != null)
        {
            var boxSessionManager = new BoxSessionManager();

            

            var speakers = new StringBuilder();
            foreach (var rec in session.SpeakersList)
            {
                string userWebSite = rec.UserWebsite ?? "";
                if (!userWebSite.ToLower().StartsWith("http://"))
                {
                    userWebSite = "http://" + userWebSite;
                }

                speakers.Append(Utils.ClearSpecialCharacters(rec.UserFirstName) + " " + Utils.ClearSpecialCharacters(rec.UserLastName) + "\n " + userWebSite + " \n");
            }
            string sessionUrl = "http://siliconvalley-codecamp.com/Sessions.aspx?id=" +
                                session.Id.ToString(CultureInfo.InvariantCulture) + " \n";

           string str = SessionPageContent(Utils.ClearSpecialCharacters(session.Title), speakers.ToString(), 
               Utils.ClearSpecialCharacters(session.Description), sessionUrl);

            string error;
            string folderIdStr = boxSessionManager.CreateFolder(Utils.ClearSpecialCharacters(session.Title), str, out error);
            string emailIn = boxSessionManager.AssociatedUploadEmail(folderIdStr);
            string url = boxSessionManager.GetPublicUrl(folderIdStr);

            

            session.BoxFolderIdString = folderIdStr;
            session.BoxFolderEmailInAddress = emailIn;
            session.BoxFolderPublicUrl = url;
            SessionsManager.I.Update(session);

            Response.Redirect("~/SessionsEdit.aspx?id=" + session.Id,true);
        }

    }
    protected void ButtonDeleteSlidesFolderId_Click(object sender, EventArgs e)
    {
        var session = SessionsManager.I.Get(new SessionsQuery { Id = SessionId }).FirstOrDefault();
        if (session != null)
        {
            var boxSessionManager = new BoxSessionManager();
            string errorStr;
            boxSessionManager.DeleteFolder(session.BoxFolderIdString,out errorStr);

            session.BoxFolderIdString = "";
            session.BoxFolderPublicUrl = "";
            session.BoxFolderEmailInAddress = "";
            SessionsManager.I.Update(session);
            Response.Redirect("~/SessionsEdit.aspx?id=" + session.Id, true);
        }
    }

    private string SessionPageContent(string sessionName, string speakerName, string description, string sessionURL)
    {
        String retString = "\n\n\n" + sessionName + " \n"
                           + "\n" + sessionURL + " \n"
                           + "\n" + speakerName + " \n\n"
                           + "\n http://www.siliconvalley-codecamp.com/Sessions.aspx All Sessions (Code Camp Website)"
                           + "\n http://www.siliconvalley-codecamp.com/ Home (Code Camp Website)"
                           + "\n\n " + description;
       

        return retString;
    }
}
