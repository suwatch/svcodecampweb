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
using ICSharpCode.SharpZipLib.Zip;
using System.Data.SqlClient;

public partial class SessionPictures : BaseContentPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Context.User.Identity.IsAuthenticated)
        {
            ButtonUpload.Enabled = true;
        }
        else
        {
            ButtonUpload.Enabled = false;
        }
    }

    protected void ButtonUpload_Click(object sender, EventArgs e)
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
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }
}
