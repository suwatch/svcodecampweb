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

public partial class ClassRoomsForPictures : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void DetailsView1_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        int id = Convert.ToInt32(DetailsView1.SelectedValue);

        // invalidate cache DisplayImageWithParams_roomid=6
        string cacheString = "DisplayImageWithParams_roomid=" + id.ToString();
        HttpContext.Current.Cache.Remove(cacheString);

        // try grabbing the image specified
        Byte[] smallerImageBytes = null;
        Byte[] imageBytes = null;
        bool completionStatus = true;
        string completionMessage = string.Empty;

        FileUpload fileUploadImage = (FileUpload)Page.FindControl("ctl00$ctl00$blankContent$parentContent$DetailsView1$FileUploadImage");

        if (fileUploadImage != null && fileUploadImage.HasFile)
        {
            imageBytes = fileUploadImage.FileBytes;
            if (imageBytes.Length > 1000000)
            {
                completionStatus = false;
                completionMessage = "Image May not be over 1Meg, try again";
            }
            else
            {
                MemoryStream ms = new MemoryStream(imageBytes);
                string fName = string.Empty;
                if (!string.IsNullOrEmpty(fileUploadImage.FileName))
                {
                    fName = fileUploadImage.FileName;
                }
                smallerImageBytes = CodeCampSV.Utils.ResizeFromStream(fName, CodeCampSV.Utils.MediumSize, ms);
                completionStatus = true;
            }

            string sqlUpdate =
              "UPDATE LectureRooms SET picture=@picture WHERE id=@id";
            try
            {
                SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString);
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand(sqlUpdate, sqlConnection);

                sqlCommand.Parameters.Add("@id", SqlDbType.Int).Value = id;
                sqlCommand.Parameters.Add("@picture", SqlDbType.Image).Value = smallerImageBytes;

                int rowsUpdated = sqlCommand.ExecuteNonQuery();
                if (rowsUpdated != 1)
                {
                    completionStatus = false;
                    completionMessage = "SqlUpdate did not return 1 row." + rowsUpdated.ToString();
                }

                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            catch (Exception updateException)
            {
                completionStatus = false;
                completionMessage = updateException.ToString();
            }
        }

        GridView1.DataBind();
        Label1.Text = completionMessage + " " + completionStatus.ToString();
    }
    protected void DetailsView1_ModeChanged(object sender, EventArgs e)
    {
       

    }
    protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        GridView1.DataBind();
    }
    protected void DetailsView1_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
    {
        GridView1.DataBind();
    }
    protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        
    }
}
