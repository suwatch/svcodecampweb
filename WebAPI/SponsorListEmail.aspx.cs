using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;

public partial class SponsorListEmail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TextBoxCodeCampYearId.Text = Utils.GetCurrentCodeCampYear().ToString();
            TextBoxDollarStart.Text = "3900.00";
            TextBoxDollarEndAmount.Text = "999999.99";
        }


    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (CheckBoxCurrentYear.Checked)
        {
            SqlDataSource1.SelectCommand = @"
                   SELECT 
                      dbo.SponsorListContact.EmailAddress,
                      CAST(dbo.SponsorList.SponsorName as varchar(10)) + ':'+ CAST(dbo.SponsorListContact.EmailAddress as varchar(40)) AS INFO
                    FROM
                      dbo.SponsorList
                      INNER JOIN dbo.SponsorListCodeCampYear ON (dbo.SponsorList.Id = dbo.SponsorListCodeCampYear.SponsorListId)
                      INNER JOIN dbo.SponsorListContact ON (dbo.SponsorList.Id = dbo.SponsorListContact.SponsorListId)
                    WHERE
                      dbo.SponsorListCodeCampYear.CodeCampYearId = @CodeCampYearId AND 
                      dbo.SponsorListCodeCampYear.DonationAmount >= @AmountStart AND 
                      dbo.SponsorListCodeCampYear.DonationAmount <= @AmountStop
                    GROUP BY dbo.SponsorListContact.EmailAddress, CAST (dbo.SponsorList.SponsorName as varchar (10)) + ':' + CAST (
                            dbo.SponsorListContact.EmailAddress as varchar (40)) 
                    ORDER BY
                      INFO
                    ";
        }
        else
        {
            {
                SqlDataSource1.SelectCommand = @"
                     SELECT 
                      dbo.SponsorListContact.EmailAddress,
                      CAST(dbo.SponsorList.SponsorName as varchar(10)) + ':'+ CAST(dbo.SponsorListContact.EmailAddress as varchar(40)) AS INFO
                    FROM
                      dbo.SponsorList
                      INNER JOIN dbo.SponsorListCodeCampYear ON (dbo.SponsorList.Id = dbo.SponsorListCodeCampYear.SponsorListId)
                      INNER JOIN dbo.SponsorListContact ON (dbo.SponsorList.Id = dbo.SponsorListContact.SponsorListId)
                    WHERE
                      dbo.SponsorListCodeCampYear.SponsorListId NOT IN 
                        (SELECT SponsorListId FROM SponsorListCodeCampYear WHERE CodeCampYearId = @CodeCampYearId) AND
                      dbo.SponsorListCodeCampYear.DonationAmount >=  @AmountStart AND 
                      dbo.SponsorListCodeCampYear.DonationAmount <= @AmountStop
                    GROUP BY dbo.SponsorListContact.EmailAddress, CAST (dbo.SponsorList.SponsorName as varchar (10)) + ':' + CAST (
                            dbo.SponsorListContact.EmailAddress as varchar (40)) 
                    ORDER BY
                       INFO
                    ";
            }
        }


    }
    protected void ButtonCopySelected_Click(object sender, EventArgs e)
    {
        var sb = new StringBuilder();
        foreach (ListItem rec in CheckBoxList1.Items)
        {
            if (!rec.Selected)
            {
                if (!String.IsNullOrEmpty(rec.Value) && !rec.Value.ToLower().Equals("emailaddress"))
                {
                    sb.Append(rec.Value);
                    sb.Append(";");
                }
            }
        }
        string text = sb.ToString();
        TextBox1.Text = text.Substring(0, text.Length - 1);
    }
   
    protected void Button1_Click(object sender, EventArgs e)
    {

    }
   
}