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

public partial class BadgeList : BaseContentPage
{

    //protected void Page_PreRender(object sender, EventArgs e)
    //{
    //    LabelCount.Text = GridView1.Rows.Count.ToString();  
    //}

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    if (!CodeCampSV.Utils.CheckUserIsAdmin())
    //    {
    //        Response.Redirect("~/Default.aspx");
    //    }

    //    if (!IsPostBack)
    //    {
    //        TextBoxCreateDateStart.Text = DateTime.MinValue.ToString();
    //        TextBoxCreateDateStart.Text = new DateTime(2001, 1, 1,0,0, 0).ToString();
    //        TextBoxCreateDateEnd.Text = DateTime.MaxValue.ToString();
    //        RadioButtonList1.SelectedIndex = 0;
    //        RadioButtonList1.Items.Add(new ListItem("GetAllAttendeesBetweenDatesNotPresenter", "GetAllAttendeesBetweenDatesNotPresenter",true));
    //        RadioButtonList1.Items.Add(new ListItem("GetAllAttendeesBetweenDatesJustPresenter", "GetAllAttendeesBetweenDatesJustPresenter"));
    //        RadioButtonList1.Items.Add(new ListItem("GetAllAttendeesBetweenDatesBarbeque", "GetAllAttendeesBetweenDatesBarbeque"));

    //        //RadioButtonList1.Items.Add(new ListItem("GetDataByNotVistaOnlyNotPresenter", "GetDataByNotVistaOnlyNotPresenter"));
        

        
    //    }

    //    if (!String.IsNullOrEmpty(RadioButtonList1.SelectedValue))
    //    {
    //        ObjectDataSource1.SelectMethod = RadioButtonList1.SelectedValue;
    //        GridView1.DataBind();
    //        Label1.Text = GridView1.Rows.Count.ToString();
    //    }
    

    //}
    //protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    ObjectDataSource1.SelectMethod = RadioButtonList1.SelectedValue;
    //    GridView1.DataBind();
    //}
    //protected void Button1_Click(object sender, EventArgs e)
    //{
    //    GridView1.DataBind();
    //}
}
