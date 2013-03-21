using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Transactions;
using System.Data.SqlClient;
using System.Collections.Generic;
using CodeCampSV;

public partial class SubmitSession : BaseContentPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // could optimize below if statement
        if (ConfigurationManager.AppSettings["SubmitSessionsOpen"] != null &&
            ConfigurationManager.AppSettings["SubmitSessionsOpen"].Equals("true"))
        {
          
        }
        else
        {
            if (!Utils.CheckUserIsAdmin() && !Utils.CheckUserIsSubmitSession())
            {
                Response.Redirect("~/SubmitSessionClosed.aspx", true);
            }
        }


        int currentCodeCampYear = Utils.GetCurrentCodeCampYear(); // year selected in drop down
        if (currentCodeCampYear != Utils.CurrentCodeCampYear)     // Utils.CurrentCodeCampYear is the actual year of the site
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Context.User.Identity.IsAuthenticated)
        {
            DIVMain.Visible = false;
            DIVNOTLOGGEDIN.Visible = true;
        }
        else
        {

            DIVMain.Visible = true;
            DIVNOTLOGGEDIN.Visible = false;
            LabelMaxCategories.Text = CodeCampSV.Utils.MaxSessionTagsToShow.ToString();

            int numberSessionsCanAdd = Utils.CheckUserIsSessionRestrictedOrAdmin();

            int numberSessionsThisUser = Utils.GetNumberSessionsThisYearForCurrentLoggedInUser();
            if (numberSessionsThisUser >= numberSessionsCanAdd)
            {
                DIVMain.Visible = false;
                DIVOverSessionLimit.Visible = true;
            }
            else
            {
                DIVMain.Visible = true;
                DIVOverSessionLimit.Visible = false;
            }
        }
    }
    protected void CaptchaUltimateControl1_Verified(object sender, EventArgs e)
    {
        bool success = false;
        int idSessionNew = -1;
        Dictionary<string, int> tagsDictionaryByName = new Dictionary<string, int>();

        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
        {
            sqlConnection.Open();
            // get list of existing categories to make sure doesn't exist already
            string sqlSelectAllCategories = "SELECT TagName,id FROM Tags";
            using (SqlCommand sqlCommandTagList = new SqlCommand(sqlSelectAllCategories, sqlConnection))
            {
                using (SqlDataReader sqlReaderTagListAll = sqlCommandTagList.ExecuteReader())
                {
                    while (sqlReaderTagListAll.Read())
                    {
                        String tagName = sqlReaderTagListAll.GetString(0).ToLower().Trim();
                        int tagId = sqlReaderTagListAll.GetInt32(1);
                        if (!tagsDictionaryByName.ContainsKey(tagName) && !String.IsNullOrEmpty(tagName))
                        {
                            tagsDictionaryByName.Add(tagName, tagId);
                        }
                    }
                }
            }
            // first see if new categories to add.

            List<string> categoriesToAdd = new List<string>();
            if (!String.IsNullOrEmpty(TextBoxAddCategories.Text))
            {
                if (TextBoxAddCategories.Text.Length < 200)
                {  // sanity check
                    char[] delimiterChars = { ',', '.', ':', ';' };
                    string[] categoriesPass1 = TextBoxAddCategories.Text.Split(delimiterChars);
                    foreach (string category in categoriesPass1)
                    {
                        if (!tagsDictionaryByName.ContainsKey(category.ToLower().Trim()) && !String.IsNullOrEmpty(category))
                        {
                            categoriesToAdd.Add(category);
                        }
                    }
                }
            }

            MembershipUser mu = Membership.GetUser();
            List<int> listTagIDs = null;

            // $$$ Broken scope, will not work.  See SessionsEdit.aspx for correct way
            // $$$ currently broken when adding sessionspeaker table, not sure why. 4/2010 PGK
            //using (TransactionScope scope = new TransactionScope())
            //{

            try
            {
                // this is the list of all tagid's we will be inserting into session
                // (need to create now so if person has new ones to insert we can do that now)
                //
                listTagIDs = new List<int>();
                if (categoriesToAdd.Count > 0)
                {
                    string sqlInsertTag =
                       "INSERT INTO  dbo.Tags(TagName) VALUES (@TagName);SELECT @@identity";
                    using (SqlCommand sqlCommandInsertTag = new SqlCommand(sqlInsertTag, sqlConnection))
                    {
                        sqlCommandInsertTag.Parameters.Add("TagName", SqlDbType.VarChar).Value = string.Empty;
                        foreach (string newCategory in categoriesToAdd)
                            if (!String.IsNullOrEmpty(newCategory))
                            {
                                sqlCommandInsertTag.Parameters["TagName"].Value = newCategory;
                                int idTag = Convert.ToInt32((Decimal)sqlCommandInsertTag.ExecuteScalar());
                                listTagIDs.Add(idTag);
                            }
                    }

                }

                int attendeeId =
                    Utils.GetAttendeesIdFromUsername(HttpContext.Current.User.Identity.Name);

                var sessionResult = new SessionsResult
                                                   {
                                                       TweetLineTweetedPreCamp = false,
                                                       TweetLineTweeted = false,
                                                       TweetLine = string.Empty,
                                                       TweetLineTweetedDate = new DateTime(1960,1,1),
                                                       Approved = true,
                                                       CodeCampYearId = Utils.GetCurrentCodeCampYear(),
                                                       //Attendeesid = attendeeId,
                                                       Createdate = DateTime.Now,
                                                       Description = TextBoxDescription.Text.Trim(),
                                                       InterentAccessRequired = CheckBoxInternetAccess.Checked,
                                                       
                                                       SessionLevel_id =
                                                           Convert.ToInt32(DropDownListLevel.SelectedValue),
                                                       LectureRoomsId = Utils.RoomNotAssigned,
                                                       SessionTimesId = Utils.TimeSessionUnassigned,
                                                       Title = TextBoxTitle.Text.Trim(),
                                                       Username = HttpContext.Current.User.Identity.Name,
                                                       DoNotShowPrimarySpeaker = false,
                                                       TwitterHashTags = TextBoxHashTags.Text
                                                   };
                SessionsManager.I.Insert(sessionResult);
                idSessionNew = sessionResult.Id;

                // THIS SHOULD NOT BE USED BECAUSE IT DOES NOT CONTAIN DEFAULTS FOR NEW SESSIONPRESENTER TABLE

                const string sqlInsertSessionPresenter = "INSERT INTO  SessionPresenter (AttendeeId,SessionId) VALUES (@AttendeeId,@SessionId)";
                using (var sqlCommandInsertSessionPresenter = new SqlCommand(sqlInsertSessionPresenter, sqlConnection))
                {
                    sqlCommandInsertSessionPresenter.Parameters.Add("AttendeeId", SqlDbType.Int).Value = attendeeId;
                    sqlCommandInsertSessionPresenter.Parameters.Add("SessionId", SqlDbType.Int).Value = idSessionNew;
                    sqlCommandInsertSessionPresenter.ExecuteScalar();
                }


                foreach (ListItem item in CheckBoxListTags.Items)
                {

                    if (item.Selected)
                    {
                        String itemText = item.Text.Trim().ToLower();
                        if (tagsDictionaryByName.ContainsKey(itemText))
                        {
                            int tagId = tagsDictionaryByName[itemText];
                            listTagIDs.Add(tagId);
                        }
                        else
                        {
                            throw new ApplicationException(String.Format("problem with tag not in dictionary: {0} {1}", itemText, tagsDictionaryByName.Count));
                        }
                    }
                }

                foreach (int idTag in listTagIDs)
                {
                    SessionTagsManager.I.Insert(new SessionTagsResult
                    {
                        SessionId = idSessionNew,
                        TagId = idTag
                    });
                }
                //scope.Complete();
                success = true;
            }
            catch (Exception ee)
            {
                throw new ApplicationException(String.Format("{0} Items in dictionary: {1}", ee, tagsDictionaryByName.Count));
            }
        }
        //} // end of transaction

        CodeCampSV.Utils.CacheClear("sessions");

        if (success)
        {
            string str = "~/Sessions.aspx?id=" + idSessionNew.ToString();
            // invalidate cache for adds when we add a new user
            CodeCampSV.Utils.ClearDisplayAdCache();
            CodeCampSV.Utils.ClearCacheForSessionTags(idSessionNew);
            Response.Redirect(str);
        }
        else
        {
            LabelStatus.Text = "Error Occurred.  Session Not Submitted.";
        }
    }
    protected void CaptchaUltimateControl1_Verifying(object sender, VerifyingEventArgs e)
    {
        if (ConfigurationManager.AppSettings["OverrideCaptcha"].ToLower().Equals("true")
            || CodeCampSV.Utils.CheckUserIsAdmin())
        {
            e.ForceVerify = true;
        }
    }
}
