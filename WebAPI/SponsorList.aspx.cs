using System;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.UI;
using CodeCampSV;

public partial class SponsorList : BaseContentPage
{
//    protected void Page_Init(object sender, EventArgs e)
//    {
//        LabelCodeCampYearId.Text = Utils.GetCurrentCodeCampYear().ToString();
//    }


//    protected void Page_PreInit(object sender, EventArgs e)
//    {
//        //FormsAuthentication.SetAuthCookie("pkellner",true);
//    }

//    protected void Page_PreRender(object sender, EventArgs e)
//    {
//        string str = SqlDataSource1.SelectCommand;
//        if (DDLIDSponsorType.SelectedValue.Equals("Community"))
//        {
//            str += " WHERE CodeCampYearId=@CodeCampYearId  AND DonationAmount = 0.0 ORDER BY [HoverOverText] ";
//        }
//        else if (DDLIDSponsorType.SelectedValue.Equals("Platinum"))
//        {
//            str += string.Format(" WHERE CodeCampYearId=@CodeCampYearId  AND DonationAmount >= {0} ORDER BY [HoverOverText] ", Utils.MinSponsorLevelPlatinum);
//        }
//        else if (DDLIDSponsorType.SelectedValue.Equals("Gold"))
//        {
//            str += string.Format(" WHERE CodeCampYearId=@CodeCampYearId  AND DonationAmount >=  {0} AND DonationAmount < {1} ORDER BY [HoverOverText] ", Utils.MinSponsorLevelGold, Utils.MinSponsorLevelPlatinum);
//        }
//        else if (DDLIDSponsorType.SelectedValue.Equals("Silver"))
//        {
//            str += string.Format(" WHERE    CodeCampYearId=@CodeCampYearId  AND DonationAmount >= {0} AND DonationAmount < {1}  ORDER BY [HoverOverText] ", Utils.MinSponsorLevelSilver, Utils.MinSponsorLevelGold);
//        }
//        else if (DDLIDSponsorType.SelectedValue.Equals("Bronze"))
//        {
//            str += string.Format(" WHERE    CodeCampYearId=@CodeCampYearId  AND DonationAmount >= {0} AND DonationAmount < {1}  ORDER BY [HoverOverText] ", Utils.MinSponsorLevelBronze, Utils.MinSponsorLevelSilver);
//        }
//        else if (DDLIDSponsorType.SelectedValue.Equals("Community"))
//        {
//            str += " WHERE DonationAmount = 0.0 AND  CodeCampYearId=@CodeCampYearId ORDER BY [HoverOverText] ";
//        }
//        else if (DDLIDSponsorType.SelectedValue.Equals("Raffle"))
//        {
//            str += " WHERE DonationAmount < 0.0 AND  CodeCampYearId=@CodeCampYearId ORDER BY [HoverOverText] ";
//        }


//        SqlDataSource1.SelectCommand = str;
//        GridView1.DataBind();
//    }


//    private void InsertARecord(string sponsorType)
//    {
//        string connectionString = WebConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString;
//        using (var conn = new SqlConnection(connectionString))
//        {
//            conn.Open();

//            string amount = string.Empty;
//            if (sponsorType.Equals("Gold"))
//            {
//                amount = Utils.MinSponsorLevelGold.ToString();
//            }
//            else if (sponsorType.Equals("Silver"))
//            {
//                amount = Utils.MinSponsorLevelSilver.ToString();
//            }
//            else if (sponsorType.Equals("Bronze"))
//            {
//                amount = Utils.MinSponsorLevelBronze.ToString();
//            }
//            else if (sponsorType.Equals("Platinum"))
//            {
//                amount = Utils.MinSponsorLevelPlatinum.ToString();
//            }
//            else if (sponsorType.Equals("Community"))
//            {
//                amount = "0.0";
//            }
//            else if (sponsorType.Equals("Raffle"))
//            {
//                amount = "-1.0";
//            }


//            // insert as invisible
//            string sqlInsertString = String.Format(
//                @"
//                    INSERT INTO [SponsorList] ([ImageURL], [NavigateURL], 
//                    [HoverOverText], [DonationAmount], [Comment], [SortIndex], [Visible], [CodeCampYearId]) 
//                    VALUES ('--','--','--',{0},'--',0,0,{1})",
//                amount, Utils.CurrentCodeCampYear);

//            using (var cmd = new SqlCommand(sqlInsertString, conn))
//            {
//                try
//                {
//                    int numInserted = cmd.ExecuteNonQuery();
//                }
//                catch (Exception eeeee)
//                {
//                    throw new ApplicationException(eeeee.ToString());
//                }
//            }
//            conn.Close();
//        }
//    }

//    protected void ButtonAddSilver_Click(object sender, EventArgs e)
//    {
//        InsertARecord("Silver");
//    }

//    protected void ButtonAddBronze_Click(object sender, EventArgs e)
//    {
//        InsertARecord("Bronze");
//    }

//    protected void ButtonAddGold_Click(object sender, EventArgs e)
//    {
//        InsertARecord("Gold");
//    }

//    protected void ButtonAddPlatinum_Click(object sender, EventArgs e)
//    {
//        InsertARecord("Platinum");
//    }

//    protected void ButtonAddCommunity_Click(object sender, EventArgs e)
//    {
//        InsertARecord("Community");
//    }

//    protected void ButtonAddRaffle_Click(object sender, EventArgs e)
//    {
//        InsertARecord("Community");
//    }
}