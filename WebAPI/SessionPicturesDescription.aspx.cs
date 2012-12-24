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
using CodeCampSV;
using System.Collections.Generic;
using System.Data.SqlClient;

public partial class SessionPicturesDescription : BaseContentPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int sessionId = -1;
            int pictureid = -1;

            GetParams(ref sessionId, ref pictureid);

            HyperLinkReturnToPictures.NavigateUrl = "~/SessionAssignPictures.aspx?sessionid=" + sessionId;

            

            LabelPresenterName.Text = Utils.GetUserNameOfSession(sessionId);
            SessionsODS sessionsODS = new SessionsODS();
            List<SessionsODS.DataObjectSessions> li =
                sessionsODS.GetByPrimaryKeySessions(sessionId);
            LabelSessionName.Text = li[0].Title;

            Image1.ImageUrl = "~/DisplayImage.ashx?sizex=0&pictureid=" + pictureid.ToString();
            string fileName = string.Empty;
            // Get the description
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();
                string sqlSelect =
                @"SELECT Pictures.FileName,
                       SessionPictures.Description
                FROM Pictures
                     INNER JOIN SessionPictures ON (Pictures.id = SessionPictures.PictureId)
                WHERE (SessionPictures.PictureId = pictures.id) AND
                      SessionPictures.SessionId = @SessionId AND
                      SessionPictures.PictureId = @PictureId";
                SqlDataReader reader = null;
                try
                {
                    SqlCommand command = new SqlCommand(sqlSelect, sqlConnection);
                    command.Parameters.Add("@PictureId", SqlDbType.Int).Value = pictureid;
                    command.Parameters.Add("@SessionId", SqlDbType.Int).Value = sessionId;

                    
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                         if (!reader.IsDBNull(0))
                        {
                            fileName = CodeCampSV.Utils.ConvertEncodedHTMLToRealHTML(reader.GetString(0));
                        }        
                        if (!reader.IsDBNull(1))
                        {
                            TextBoxDescription.Text = CodeCampSV.Utils.ConvertEncodedHTMLToRealHTML(reader.GetString(1));
                        }        
     
                    }
                }
                catch (Exception eee)
                {
                    throw new ApplicationException(eee.ToString());
                }
                finally
                {
                    if (reader != null) reader.Close();
                }

            }

            HyperLinkFullRes.NavigateUrl = "http://peterkellner.net/images/CodeCampSV06/" + fileName.ToUpper();
          

        }
    }

    private void GetParams(ref int sessionId, ref int pictureid)
    {
        string requestIdStr = Request.QueryString["idsession"];
        bool good = Int32.TryParse(requestIdStr, out sessionId);
        if (!good)
        {
            Response.Redirect("~/Sessions.aspx");
        }

        requestIdStr = Request.QueryString["idpicture"];

        good = Int32.TryParse(requestIdStr, out pictureid);
        if (!good)
        {
            Response.Redirect("~/Sessions.aspx");
        }
    }
    protected void ButtonUpdate_Click(object sender, EventArgs e)
    {
        Cache.Remove(CodeCampSV.Utils.CachePictureDescriptions);

        int sessionId = -1;
        int pictureid = -1;

        GetParams(ref sessionId, ref pictureid);
        //string description = Context.Server.HtmlEncode(TextBoxDescription.Text);

        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
        {
            sqlConnection.Open();
            string sqlSelect =
            @"UPDATE 
                  SessionPictures
                SET
                  Description = @description
                WHERE
                  SessionId = @SessionId AND
                  PictureId = @PictureId";
            try
            {
                SqlCommand command = new SqlCommand(sqlSelect, sqlConnection);
                command.Parameters.Add("@PictureId", SqlDbType.Int).Value = pictureid;
                command.Parameters.Add("@SessionId", SqlDbType.Int).Value = sessionId;
                command.Parameters.Add("@description", SqlDbType.VarChar).Value = Context.Server.HtmlEncode(TextBoxDescription.Text);
                
                command.ExecuteNonQuery();
            }
            catch (Exception eee)
            {
                throw new ApplicationException(eee.ToString());
            }

        }
    }
}
