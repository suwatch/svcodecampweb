using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;

// ItemSentToUPS means flier received


public partial class SponsorInformation : System.Web.UI.Page
{
    int sponsorId;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Context.User.Identity.IsAuthenticated)
        {
            Response.Redirect("~/LoginCamp.aspx", true);
        }
        else
        {
            if (Context.Request["sponsorid"] != null && Utils.CheckUserIsSponsorManagerOrAdmin())
            {
                sponsorId = Convert.ToInt32(Context.Request["sponsorid"]);
            }
            else
            {
                sponsorId = Utils.GetSponsorIdBasedOnUsername(Context.User.Identity.Name);
            }

            if (sponsorId > 0)
            {
                LabelSponsorListId.Text = sponsorId.ToString(CultureInfo.InvariantCulture);

                HyperLinkEdit.NavigateUrl = String.Format("~/SponsorInformationEdit.aspx?sponsorid={0}", sponsorId);
                //HyperLinkEditShortDescription.NavigateUrl = String.Format("~/SponsorInformationEdit.aspx?SponsorId={0}", sponsorId);

                if (!IsPostBack)
                {
                    var rec = SponsorListManager.I.Get(new SponsorListQuery() { Id = sponsorId }).FirstOrDefault();
                    if (rec != null)
                    {
                      //  CheckBoxIDPlatinumTable.Checked = rec.


                        LabelShortDescription.Text = HttpUtility.HtmlEncode(rec.CompanyDescriptionShort);
                        LabelLongDescription.Text = HttpUtility.HtmlEncode(rec.CompanyDescriptionLong);
                        LabelAddressLine1.Text = HttpUtility.HtmlEncode(rec.CompanyAddressLine1);
                        LabelAddressLine2.Text = HttpUtility.HtmlEncode(rec.CompanyAddressLine2);
                        LabelCity.Text = HttpUtility.HtmlEncode(rec.CompanyCity);
                        LabelState.Text = HttpUtility.HtmlEncode(rec.CompanyState);
                        LabelZip.Text = HttpUtility.HtmlEncode(rec.CompanyZip);
                        LabelPhone.Text = HttpUtility.HtmlEncode(rec.CompanyPhoneNumber);



                        LabelSponsorName.Text = "Sponsorship Information:&nbsp;" + rec.SponsorName;
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
                        CheckBoxIdShippingItemstoCodeCamp.Checked = rec1.ItemSentToFoothill.HasValue && rec1.ItemSentToFoothill.Value;
                        LabelShippingWhatToCodeCamp.Text = rec1.ItemsShippingDescription;

                        LabelNoteFromCodeCamp.Text = rec1.NoteFromCodeCamp;


                    }


                }
            }
            else
            {
                LabelUnassigned.Visible = true;
                PageInfoId.Visible = false;
            }
        }

    }

    protected void lbInsert_Click(object sender, EventArgs e)
    {
        var email = ((TextBox)GridViewSponsorListContact.FooterRow.FindControl("EmailAddress")).Text;

        // need to fix uppercase on email
        var cnt = (SponsorListContactManager.I.Get(new SponsorListContactQuery() { EmailAddress = email, SponsorListId = sponsorId })).Count();
        if (cnt == 0 && !String.IsNullOrEmpty(email))
        {
            SponsorListContactManager.I.Insert(new SponsorListContactResult()
            {
                SponsorListId = sponsorId,
                EmailAddress = email,
                DateCreated = DateTime.Now,
                FirstName = "",
                LastName = "",
                PhoneNumberCell = "",
                PhoneNumberOffice = "",
                SponsorCCMailings = true,
                GeneralCCMailings = true,
                Comment = ""
            });
            GridViewSponsorListContact.DataBind();

        }


     
    }
    protected void GridViewSponsorListContact_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //if (e.CommandArgument.Equals("insert"))
        //{
        //    var email = ((TextBox)GridViewSponsorListContact.FooterRow.FindControl("EmailAddress")).Text;
        //}
    }
}