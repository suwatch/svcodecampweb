using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;

public partial class SponsorInformationEdit : System.Web.UI.Page
{
    int sponsorId;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Context.User.Identity.IsAuthenticated)
        {
            Response.Redirect("~/login.aspx");
        }

        if (!IsPostBack)
        {


            if (Context.Request["sponsorid"] != null && Utils.CheckUserIsSponsorManagerOrAdmin())
            {
                sponsorId = Convert.ToInt32(Context.Request["sponsorid"]);
                CodeCampNoteId.Visible = true;
                HyperLinkSponsorInformation.NavigateUrl = "~/SponsorInformation.aspx?sponsorid=" + sponsorId;
            }
            else
            {
                sponsorId = Utils.GetSponsorIdBasedOnUsername(Context.User.Identity.Name);
                CodeCampNoteId.Visible = false;
            }

            if (sponsorId <= 0)
            {
                Response.Redirect("~/LoginCamp.aspx");
            }
            else
            {
                var sponsorListRec =
                    SponsorListManager.I.Get(new SponsorListQuery
                                                 {
                                                     Id = sponsorId,
                                                     IncludeSponsorLevel = true,
                                                     CodeCampYearId = Utils.GetCurrentCodeCampYear(),
                                                     PlatinumLevel = Utils.MinSponsorLevelPlatinum,
                                                     GoldLevel = Utils.MinSponsorLevelGold,
                                                     SilverLevel = Utils.MinSponsorLevelSilver,
                                                     BronzeLevel = Utils.MinSponsorLevelBronze
                                                 }).FirstOrDefault();
                if (sponsorListRec != null)
                {
                    TextBoxFullDescription.Text = sponsorListRec.CompanyDescriptionLong;
                    TextBoxShortDescription.Text = sponsorListRec.CompanyDescriptionShort;

                    TextBoxAddressLine1.Text = sponsorListRec.CompanyAddressLine1;
                    TextBoxAddressLine2.Text = sponsorListRec.CompanyAddressLine2;
                    TextBoxCity.Text = sponsorListRec.CompanyCity;
                    TextBoxState.Text = sponsorListRec.CompanyState;
                    TextBoxZip.Text = sponsorListRec.CompanyZip;
                    TextBoxPhone.Text = sponsorListRec.CompanyPhoneNumber;

                    LabelSponsorId.Text = sponsorId.ToString(CultureInfo.InvariantCulture);


                    if (sponsorListRec.SponsorSupportLevel != null &&   !sponsorListRec.SponsorSupportLevel.Equals("Platinum"))
                    {
                        CheckBoxIDPlatinumTable.Enabled = false;
                        CheckBoxIdPlatinumFlier.Enabled = false;
                    }
                }

                var rec1 =
                    SponsorListCodeCampYearManager.I.Get(new SponsorListCodeCampYearQuery()
                                                             {
                                                                 CodeCampYearId = Utils.CurrentCodeCampYear,
                                                                 SponsorListId = sponsorId
                                                             }).FirstOrDefault();
                if (rec1 != null)
                {
                    CheckBoxIDPlatinumTable.Checked = rec1.TableRequired.HasValue && rec1.TableRequired.Value;
                    CheckBoxIdPlatinumFlier.Checked = rec1.AttendeeBagItem.HasValue && rec1.AttendeeBagItem.Value;
                    CheckBoxIdShippingItemstoCodeCamp.Checked = rec1.ItemSentToFoothill.HasValue &&
                                                                rec1.ItemSentToFoothill.Value;
                    TextBoxShippingWhatToCodeCamp.Text = rec1.ItemsShippingDescription;

                    if (CodeCampNoteId.Visible)
                    {
                        TextBoxCodeCampNote.Text = rec1.NoteFromCodeCamp;
                    }
                }
            }
        }
        else
        {
            Int32.TryParse(LabelSponsorId.Text, out sponsorId);
        }
    }


    protected void ButtonUpdate_Click(object sender, EventArgs e)
    {
        var rec = SponsorListManager.I.Get(new SponsorListQuery() { Id = sponsorId }).FirstOrDefault();
        if (rec != null)
        {
            rec.CompanyDescriptionShort = TextBoxShortDescription.Text;
            rec.CompanyDescriptionLong = TextBoxFullDescription.Text;
            rec.CompanyAddressLine1 = TextBoxAddressLine1.Text;
            rec.CompanyAddressLine2 = TextBoxAddressLine2.Text;
            rec.CompanyCity = TextBoxCity.Text;
            rec.CompanyState = TextBoxState.Text;
            rec.CompanyZip = TextBoxZip.Text;
            rec.CompanyPhoneNumber = TextBoxPhone.Text;
            
            SponsorListManager.I.Update(rec);
        }

         var rec1 =
                        SponsorListCodeCampYearManager.I.Get(new SponsorListCodeCampYearQuery()
                                                                 {
                                                                     CodeCampYearId = Utils.CurrentCodeCampYear,
                                                                     SponsorListId = sponsorId
                                                                 }).FirstOrDefault();
         if (rec1 != null)
         {
             rec1.TableRequired = CheckBoxIDPlatinumTable.Checked;
             rec1.AttendeeBagItem = CheckBoxIdPlatinumFlier.Checked;
             rec1.ItemSentToFoothill = CheckBoxIdShippingItemstoCodeCamp.Checked;
             rec1.ItemsShippingDescription = TextBoxShippingWhatToCodeCamp.Text;

             if (CodeCampNoteId.Visible)
             {
                 rec1.NoteFromCodeCamp = TextBoxCodeCampNote.Text;
             }


             SponsorListCodeCampYearManager.I.Update(rec1);
         }

         LabelStatus.Text = "Updated";





    }
}