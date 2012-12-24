using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;

public partial class ReferralUrls : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //if (!IsPostBack)
        //{
            PopulateDropDownListWithUnclaimedUrls();
            if (Context.User.Identity.IsAuthenticated)
            {
                Label1.Text = Utils.GetAttendeesIdFromUsername(Context.User.Identity.Name).ToString();
            }
            else
            {
                ButtonClaimReferringUrl.Enabled = false;
            }
        //}


    }

    private void PopulateDropDownListWithUnclaimedUrls()
    {
        var dict = new Dictionary<string, int>();
        if (Context.User.Identity.IsAuthenticated)
        {
            dict = Utils.GetReferrerUrls(false,3);
        }

        DropDownListUnClaimedUrls.Items.Clear();
        foreach (var rec in dict)
        {
            string searchString = FilterSearchId.Text;
            if (!String.IsNullOrEmpty(searchString))
            {
                if (rec.Key.ToString(CultureInfo.InvariantCulture).ToLower().Contains(searchString.ToLower()))
                {
                    string str = string.Format("{0}       (Count: {1})", rec.Key, rec.Value.ToString());
                    DropDownListUnClaimedUrls.Items.Add(new ListItem(str, rec.Key));
                }
            }
            else
            {
                string str = string.Format("{0}       (Count: {1})", rec.Key, rec.Value.ToString());
                DropDownListUnClaimedUrls.Items.Add(new ListItem(str, rec.Key));
            }

           
        }
    }

    protected void ButtonClaimReferringUrl_Click(object sender, EventArgs e)
    {
        string url = DropDownListUnClaimedUrls.SelectedValue;
        ReferringUrlGroupManager.I.Insert(
            new ReferringUrlGroupResult()
                {
                    AttendeesId = Utils.GetAttendeesIdFromUsername(Context.User.Identity.Name),
                    DeletedCount = 0,
                    ArticleName = "",
                    ReferringUrlName = url,
                    UserGroup = "",
                    Visible = true
                }
            );
        PopulateDropDownListWithUnclaimedUrls();
        GridViewMyReferringUrls.DataBind();

    }
    protected void GridViewMyReferringUrls_RowUpdated(object sender, GridViewUpdatedEventArgs e)
    {
        PopulateDropDownListWithUnclaimedUrls();
    }
    protected void GridViewMyReferringUrls_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        PopulateDropDownListWithUnclaimedUrls();
    }
}

