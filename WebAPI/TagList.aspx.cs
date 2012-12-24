using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;

public partial class TagList : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //if (Utils.CheckUserIsAdmin())
            //{
            //    LabelAttendeesId.Visible = true;
            //    LabelCodeCampYearId.Visible = true;
            //}

            LabelAttendeesId.Text = Utils.GetAttendeesIdFromUsername(Context.User.Identity.Name).ToString();
            LabelCodeCampYearId.Text = Utils.GetCurrentCodeCampYear().ToString(); // gets from dropdownlist on top of page
        }

    }

    
    protected void ButtonCreateNewTagListName_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(TextBox1.Text))
        {
            int totalFound = AttendeesTagListManager.I.Get(new AttendeesTagListQuery
                                              {
                                                  AttendeesId =
                                                      Utils.GetAttendeesIdFromUsername(Context.User.Identity.Name),
                                                  TagListName = TextBox1.Text
                                              }).Count();

            // don't add dups (no error for now, just don't add it)
            if (totalFound == 0)
            {
                AttendeesTagListManager.I.Insert(new AttendeesTagListResult()
                                                     {
                                                         AttendeesId =
                                                             Utils.GetAttendeesIdFromUsername(Context.User.Identity.Name),
                                                         TagListName = TextBox1.Text
                                                     });
                GridViewTagList.DataBind();
            }
        }
        
    }

    protected void GridViewNotSelected_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (GridViewTagList.SelectedIndex != -1)
        {
            int tagsId = Convert.ToInt32(e.CommandArgument);
            int attendeesId = Convert.ToInt32(LabelAttendeesId.Text);

            AttendeesTagListDetailManager.I.Insert(new AttendeesTagListDetailResult()
                                                       {
                                                           AttendeesId = attendeesId,
                                                           TagsId = tagsId,
                                                           AttendeesTagListId =
                                                               Convert.ToInt32(GridViewTagList.SelectedValue)
                                                       });
            GridViewNotSelected.DataBind();
            GridViewSelected.DataBind();
        }

    }

    /// <summary>
    /// this is GridViewTagList
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridViewSelectedRowCommand(object sender, GridViewCommandEventArgs e)
    {
        LabelError1.Visible = false; // just in case
        int tagsId = Convert.ToInt32(e.CommandArgument);
        int attendeesId = Convert.ToInt32(LabelAttendeesId.Text);

        var rec = AttendeesTagListDetailManager.I.Get(new AttendeesTagListDetailQuery
                                                          {
                                                              AttendeesId = attendeesId,
                                                              TagsId = tagsId,
                                                              AttendeesTagListId =
                                                                  Convert.ToInt32(GridViewTagList.SelectedValue)
                                                          }).SingleOrDefault();
        if (rec != null)
        {
            AttendeesTagListDetailManager.I.Delete(rec);
        }
        GridViewNotSelected.DataBind();
        GridViewSelected.DataBind();
    }

    protected void GridViewTagListSelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = GridViewTagList.SelectedRow;
        LabelTagGroupName.Text = row.Cells[4].Text;
    }
    protected void GridViewTagListRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string newName = e.NewValues[0].ToString();
        int totalFound = AttendeesTagListManager.I.Get(new AttendeesTagListQuery
        {
            AttendeesId =
                Utils.GetAttendeesIdFromUsername(Context.User.Identity.Name),
            TagListName = newName
        }).Count();
        if (totalFound > 0)
        {
            e.Cancel = true;
            LabelError1.Text = "Can Not Rename to Name That Already Exists";
            LabelError1.BackColor = System.Drawing.Color.Red;
            LabelError1.Visible = true;
        }
        else
        {
            LabelError1.Visible = false;
        }

       
    }

    protected void GridViewTagList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Delete"))
        {
            var recs = AttendeesTagListDetailManager.I.Get(new AttendeesTagListDetailQuery()
                                                    {
                                                        AttendeesId = Convert.ToInt32(LabelAttendeesId.Text),
                                                        AttendeesTagListId = Convert.ToInt32(e.CommandArgument)
                                                    });
            AttendeesTagListDetailManager.I.Delete(recs);
            GridViewTagList.DataBind(); // must be first so others don't retrieve
            GridViewTagList.SelectedIndex = -1;
            GridViewSelected.DataBind();
            GridViewNotSelected.DataBind();
            
            LabelTagGroupName.Text = "";
        }
    }
}