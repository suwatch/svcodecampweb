using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;

public partial class ShirtSizeSet : System.Web.UI.Page
{
    int _attendeeId;

    protected void Page_Load(object sender, EventArgs e)
    {
        

        if (Context.User.Identity.IsAuthenticated)
        {
            _attendeeId = Utils.GetAttendeesIdFromUsername(Context.User.Identity.Name);
            bool hasShirtSize = Utils.CheckHasShirtSize(_attendeeId);
            if (hasShirtSize)
            {
                Response.Redirect("~");
            }
            bool isSpeakingThisYear = Utils.CheckAttendeeIdIsSpeaker(_attendeeId);
            SpeakerShirtSizeDiv.Visible = false;
            HaveShirtSizeId.Visible = true;

            if (isSpeakingThisYear)
            {
                if (ConfigurationManager.AppSettings["SpeakerShirtSizes"] != null)
                {
                    string list = ConfigurationManager.AppSettings["SpeakerShirtSizes"];
                    char[] splitchar = {','};
                    List<string> newList = list.Split(splitchar).ToList();
                    DropDownListSpeakerShirtSize.Items.Add("--Not Selected");
                    foreach (var item in newList)
                    {
                        DropDownListSpeakerShirtSize.Items.Add(new ListItem(item.Trim(), item.Trim()));
                    }
                }
                SpeakerShirtSizeDiv.Visible = true;
                HaveShirtSizeId.Visible = false;
            }
        }


    }
    protected void submitbuttonId_Click(object sender, EventArgs e)
    {
        if (DropDownListSpeakerShirtSize.SelectedValue != "--Not Selected")
        {
            Utils.UpdateShirtSize(_attendeeId, DropDownListSpeakerShirtSize.SelectedValue);
            Response.Redirect("~");
        }
        else
        {
            
        }


       
    }
}