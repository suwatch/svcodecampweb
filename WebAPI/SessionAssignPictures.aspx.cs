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
using System.IO;
using System.Data.SqlClient;
using CodeCampSV;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;

public partial class SessionAssignPictures : BaseContentPage
{
    int sessionId = -1;
    protected Dictionary<int, string> pictureDescriptionDictionary;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        

        if (Request.QueryString["sessionid"] != null)
        {
            string str = Request.QueryString["sessionid"];
            bool good = Int32.TryParse(str, out sessionId);
            ViewState["sessionid"] = sessionId;
            HyperLinkReturn.NavigateUrl = "~/Sessions.aspx?OnlyOne=true&id=" + sessionId;
        }
        else
        {
            Response.Redirect("~/Sessions.aspx");
        }

        if (CodeCampSV.Utils.CheckUserIsAdmin())
        {
            FileUploadImages.Visible = true;
            Button1.Visible = true;
        }
        else
        {
            FileUploadImages.Visible = false;
            Button1.Visible = false;
        }


        if (!Context.User.Identity.IsAuthenticated)
        {
            DivAvailablePictureList.Visible = false;
            DropDownListPerPageCount.Visible = false;
        }

        if (Cache[CodeCampSV.Utils.CachePictureDescriptions] == null)
        {
            const string sqlSelect = @"SELECT 
                  PictureId,Description
                FROM
                  SessionPictures
                WHERE
                  (SessionPictures.SessionId = @sessionid)";

            pictureDescriptionDictionary = new Dictionary<int, string>();

            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();
                SqlDataReader reader = null;
                try
                {
                    SqlCommand command = new SqlCommand(sqlSelect, sqlConnection);
                    command.Parameters.Add("@sessionid", SqlDbType.Int).Value = sessionId;
                    reader = command.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        int pictureId = reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
                        string description = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        if (pictureId >= 0 && description.Length > 0)
                        {
                            pictureDescriptionDictionary.Add(pictureId, description);
                        }
                    }
                    Cache.Insert(CodeCampSV.Utils.CachePictureDescriptions,
                        pictureDescriptionDictionary, null,
                        DateTime.Now.Add(new TimeSpan(0, 0, Utils.RetrieveSecondsForSessionCacheTimeout())), new TimeSpan(0));
                }
                catch (Exception eee1)
                {
                    throw new ApplicationException(eee1.ToString());
                }
                finally
                {
                    if (reader != null) reader.Dispose();
                }
            }
        }
        else
        {
            pictureDescriptionDictionary = (Dictionary<int, string>)Cache[CodeCampSV.Utils.CachePictureDescriptions];
        }
    }
    protected void GridViewAvailablePictures_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName.Equals("Assign"))
        {
            var idPictureStr = (string)e.CommandArgument;
            int pictureId = -1;
            Int32.TryParse(idPictureStr, out pictureId);


            var PKID = new Guid(CodeCampSV.Utils.GetAttendeePKIDByUsername(Context.User.Identity.Name));

            using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();

                const string sqlInsert = @"INSERT INTO SessionPictures(PictureId, SessionId, AttendeePKID,
                     AssignedDate)
                    VALUES (@PictureId, @SessionId, @AttendeePKID, @AssignedDate)";


                if (ViewState["sessionid"] != null)
                {
                    using (var sqlCommand = new SqlCommand(sqlInsert, sqlConnection))
                    {
                        sqlCommand.Parameters.Add("@PictureId", SqlDbType.Int).Value = pictureId;
                        sqlCommand.Parameters.Add("@SessionId", SqlDbType.Int).Value = (int)ViewState["sessionid"];
                        sqlCommand.Parameters.Add("@AssignedDate", SqlDbType.DateTime).Value = DateTime.Now;
                        sqlCommand.Parameters.Add("@AttendeePKID", SqlDbType.UniqueIdentifier).Value = PKID;
                        sqlCommand.ExecuteNonQuery();
                    }
                }


                sqlConnection.Close();
                sqlConnection.Dispose();
            }

            GridViewAvailablePictures.DataBind();
            GridViewPicturesAssigned.DataBind();
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {


        if (FileUploadImages.HasFile)
        {
            Guid attendeeGuid = new Guid(CodeCampSV.Utils.GetAttendeePKIDByUsername(Context.User.Identity.Name));

            Stream stream = FileUploadImages.FileContent;
            string fileName = FileUploadImages.FileName;

            if (fileName.ToUpper().EndsWith("ZIP"))
            {
                SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString);
                sqlConnection.Open();


                int iLen = Convert.ToInt32(stream.Length);
                byte[] uploadedByteArray = new byte[iLen];
                stream.Read(uploadedByteArray, 0, iLen);

                // figure out how many hrm's are here.  This happens again when the file is uploaded.  If number is
                // greater than some constant (hardwired to 25 now, but change to parameter later so the number does not
                // have to be in same place, see UploadFileAndProcessWorkItem for number to set.
                int totalFiles = 0;

                MemoryStream ms = new MemoryStream(uploadedByteArray);
                ZipInputStream zipInputStream = new ZipInputStream(ms);
                ZipEntry theEntry1;
                while ((theEntry1 = zipInputStream.GetNextEntry()) != null)
                {
                    string strFileName = theEntry1.Name;
                    if (strFileName.ToUpper().EndsWith("JPG"))
                    {
                        totalFiles++;
                        string strx = theEntry1.Name;
                        char[] delim = new char[3];
                        delim[0] = Convert.ToChar(".");
                        delim[1] = Convert.ToChar("/");
                        delim[2] = Convert.ToChar("\\");
                        string[] fileParts = strx.Split(delim, 10);
                        string fileType = fileParts[fileParts.Length - 1];
                        string fileName1 = fileParts[fileParts.Length - 2];
                        string fullFileName = fileName1 + "." + fileType;

                        if (fileType.ToUpper().EndsWith("JPG"))
                        {
                            byte[] uploadedByteArrayFile = new byte[theEntry1.Size];

                            int size = 2048;
                            byte[] data = new byte[2048];

                            int iPos = 0;
                            {
                                while (true)
                                {
                                    size = zipInputStream.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        for (int i = 0; i < size; i++)
                                        {
                                            uploadedByteArrayFile[iPos] = data[i];
                                            iPos++;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }

                            // insert the record
                            string sqlInsert =
                                @"INSERT INTO
                                  Pictures
                                (
                                  AttendeePKID,
                                  DateCreated,
                                  DateUpdated,
                                  PictureBytes,
                                  FileName)
                                VALUES(
                                  @AttendeePKID,
                                  @DateCreated,
                                  @DateUpdated,
                                  @PictureBytes,
                                  @FileName)";

                            SqlCommand sqlCommand = new SqlCommand(sqlInsert, sqlConnection);
                            sqlCommand.Parameters.Add("@AttendeePKID", SqlDbType.UniqueIdentifier).Value = attendeeGuid;
                            sqlCommand.Parameters.Add("@DateCreated", SqlDbType.DateTime).Value = DateTime.Now;
                            sqlCommand.Parameters.Add("@DateUpdated", SqlDbType.DateTime).Value = DateTime.Now;
                            sqlCommand.Parameters.Add("@PictureBytes", SqlDbType.Image).Value = uploadedByteArrayFile;
                            sqlCommand.Parameters.Add("@FileName", SqlDbType.VarChar).Value = strFileName;

                            int cntUpdate = sqlCommand.ExecuteNonQuery();


                        }


                    }
                }

                sqlConnection.Close();
            }
        }
        GridViewAvailablePictures.DataBind();
    }
    protected void GridViewPicturesAssigned_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        // CommandArgument='<%# "makedefault^" + Eval("id") %>'
        int iPosHat = e.CommandArgument.ToString().IndexOf("^");
        int idPicture = Convert.ToInt32(e.CommandArgument.ToString().Substring(iPosHat + 1));
        
        if (e.CommandArgument.ToString().StartsWith("makedefault"))
        {
            // Need to unassign all other defaults for this and make this one the default
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();
                string sqlSelect =
                @"UPDATE 
                  dbo.SessionPictures
                SET
                  DefaultPicture = 0
                WHERE
                  dbo.SessionPictures.SessionId = @SessionId;
                UPDATE 
                  dbo.SessionPictures
                SET
                  DefaultPicture = 1
                WHERE
                  (dbo.SessionPictures.PictureId = @PictureId)
                ";

                try
                {
                    SqlCommand command = new SqlCommand(sqlSelect, sqlConnection);
                    command.Parameters.Add("@PictureId", SqlDbType.Int).Value = idPicture;
                    command.Parameters.Add("@SessionId", SqlDbType.Int).Value = (int)ViewState["sessionid"];
                    command.ExecuteNonQuery();
                }
                catch (Exception eee)
                {
                    throw new ApplicationException(eee.ToString());
                }

            }
        }
        else if (e.CommandArgument.ToString().StartsWith("unassign"))
        {
            // Need to unassign all other defaults for this and make this one the default
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString))
            {
                sqlConnection.Open();
                string sqlSelect =
                @"DELETE FROM SessionPictures
                WHERE
                  SessionPictures.SessionId = @SessionId AND
                  SessionPictures.PictureId = @PictureId";

                try
                {
                    SqlCommand command = new SqlCommand(sqlSelect, sqlConnection);
                    command.Parameters.Add("@PictureId", SqlDbType.Int).Value = idPicture;
                    command.Parameters.Add("@SessionId", SqlDbType.Int).Value = (int)ViewState["sessionid"];
                    command.ExecuteNonQuery();
                }
                catch (Exception eee)
                {
                    throw new ApplicationException(eee.ToString());
                }
            }
            GridViewAvailablePictures.DataBind();
            GridViewPicturesAssigned.DataBind();
        }
        else if (e.CommandArgument.ToString().StartsWith("editdescription"))
        {
            int idSession = (int)ViewState["sessionid"];
            Response.Redirect("~/SessionPicturesDescription.aspx?idpicture=" + idPicture + "&idsession=" + idSession);
        }
    }

    protected string GetDescriptionFromPictureId(int pictureId)
    {
        string descr = "No Description";
        if (pictureDescriptionDictionary.ContainsKey(pictureId))
        {
            descr = pictureDescriptionDictionary[pictureId];
        }
        return descr;
    }

    protected string GetUpperName(string inName)
    {
        return inName.ToUpper();
    }

    protected void DropDownListPerPageCount_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewAvailablePictures.PageSize = Convert.ToInt32(DropDownListPerPageCount.SelectedValue);
        GridViewAvailablePictures.DataBind();
    }

    protected bool CheckForAuthenticated()
    {
        //return true;

        if (Context.User.Identity.IsAuthenticated)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected string UpperCaseName(string inName)
    {
        return inName.ToUpper();
    }
}
