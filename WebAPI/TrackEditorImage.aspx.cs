using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;

public partial class TrackEditorImage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        FileUpload editImage = (FileUpload)DetailsView1.FindControl("FileUploadImageId");

        Label labelIdField = DetailsView1.FindControl("LabelIdField") as Label;
        if (labelIdField != null)
        {
            int trackId = Convert.ToInt32(labelIdField.Text);
            if (editImage.HasFile)
            {
                var bytes = editImage.FileBytes;
                Utils.UpdateTrackImage(trackId, bytes);


            }
        }
    }

    protected string GetImageUrl(int id)
    {
        return string.Format("~/trackimage/{0}.jpg",id);
    }
}