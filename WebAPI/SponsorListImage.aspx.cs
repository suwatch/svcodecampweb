using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;

public partial class SponsorListImage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int row = GridView1.EditIndex;
        FileUpload editImage = (FileUpload)GridView1.Rows[row].FindControl("FileUploadImageId");

       

        Label labelIdField = GridView1.Rows[row].FindControl("LabelId") as Label;
        if (labelIdField != null)
        {
            int sponsorId = Convert.ToInt32(labelIdField.Text);
            if (editImage.HasFile)
            {
                var bytes = editImage.FileBytes;
                Utils.UpdateSponsorImage(sponsorId, bytes,editImage.FileName);


            }
        }
    }
}