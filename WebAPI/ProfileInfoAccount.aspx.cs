using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlTypes;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Web.UI;
using CodeCampSV;
using Roles=System.Web.Security.Roles;
using System.Drawing;
using System.Linq;

public partial class ProfileInfoAccount : BaseContentPage
{
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Context.User.Identity.IsAuthenticated)
        {
            //int currentVistaStatusId = CodeCampSV.Utils.GetAttendeeVistaStatusByUsername(Context.User.Identity.Name.ToString());
            //RadioButtonListVista.SelectedValue = currentVistaStatusId.ToString();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // this page is obsolete now
        Response.RedirectPermanent("~/Register.aspx");


        LabelAttendingSaturdayLabel.Text = Utils.GetCodeCampCurrentString("saturday", Utils.CurrentCodeCampYear);
        LabelAttendingSundayLabel.Text = Utils.GetCodeCampCurrentString("sunday", Utils.CurrentCodeCampYear);
        ButtonRegisterCurrentYear.Text = string.Format("Register For Code Camp on {0}", Utils.GetCodeCampDateStringByCodeCampYearId(Utils.CurrentCodeCampYear));
        ButtonRegisterCurrentYear.BackColor = System.Drawing.Color.Orange;

        string guidString;
        if (Request.Params["id"] != null)
        {
            guidString = Request.Params["id"];
            string username = Utils.GetAttendeeUsernameByGUID(guidString);
            if (!Roles.IsUserInRole(username, Utils.GetAdminRoleName()))
            {
                if (!string.IsNullOrEmpty(username))
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        FormsAuthentication.SignOut();
                    }
                    FormsAuthentication.SetAuthCookie(username, true);

                    // check and see if person wants to override dinner setting
                    if (Request.QueryString["AttendDinner"] != null)
                    {
                        string answerAttendDinner = Request.QueryString["AttendDinner"];
                        if (answerAttendDinner.ToLower().Equals("true"))
                        {
                            Utils.UpdateAttendDinner(true, username);
                        }
                        else
                        {
                            Utils.UpdateAttendDinner(false, username);
                        }
                    }
                    Response.Redirect("~/ProfileInfoAccount.aspx", true);
                }
            }
        }
        else
        {
            guidString = Utils.GetAttendeePKIDByUsername(User.Identity.Name);
        }
        //else
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        guidString = CodeCampSV.Utils.GetAttendeePKIDByUsername(User.Identity.Name);
        //        string username = CodeCampSV.Utils.GetAttendeeUsernameByGUID(guidString);
        //        FormsAuthentication.SetAuthCookie(username, true);
        //    }
        //}

        if (Context.User.Identity.IsAuthenticated)
        {
            var attendeesCodeCampYearResults =
                AttendeesCodeCampYearManager.I.Get(new AttendeesCodeCampYearQuery()
                                                       {
                                                           AttendeesId =
                                                               Utils.GetAttendeesIdFromUsername(
                                                                   Context.User.Identity.Name),
                                                           CodeCampYearId = Utils.CurrentCodeCampYear
                                                       });
            if (attendeesCodeCampYearResults == null || attendeesCodeCampYearResults.Count <= 0)
            {
                ButtonCancelRegistration.Visible = false;
                ButtonRegisterCurrentYear.Visible = true;
            }
            else
            {
                ButtonCancelRegistration.Visible = true;
                ButtonRegisterCurrentYear.Visible = false;


                LabelTopMessage.Text = "Update Profile For: ";

                var attendeeRec = AttendeesManager.I.Get(new AttendeesQuery()
                                                             {
                                                                 Id =
                                                                     Utils.GetAttendeesIdFromUsername(
                                                                         Context.User.Identity.Name)
                                                             }).SingleOrDefault();


                TableRowSaturdayDinner.Visible =
                    ConfigurationManager.AppSettings["ShowDinnerConfirmation"].ToLower().Equals("true");

                // Always Add the Parameters to detailsview2 and radiobuttonlistvista
                if (!IsPostBack)
                {
                    LabelUsername.Text = attendeeRec.Username;
                    TextBoxFirstName.Text = attendeeRec.UserFirstName;
                    TextBoxLastName.Text = attendeeRec.UserLastName;
                    TextBoxBio.Text = attendeeRec.UserBio;
                    TextBoxWebBlog.Text = attendeeRec.UserWebsite;
                    TextBoxZipCode.Text = attendeeRec.UserZipCode;
                    TextBoxEmailAddress.Text = attendeeRec.Email;
                    CheckBoxSaturdayDinner.Checked = attendeeRec.SaturdayDinner ?? false;
                    //CheckBoxSaturday.Checked = attendee.Saturdayclasses;
                    //CheckBoxSunday.Checked = attendee.Sundayclasses;
                    CheckBoxAllowAttendeesToEmailMe.Checked = attendeeRec.VistaOnly ?? false;
                    // Using VistaOnly boolean to indicate weather it's OK to email speakers

                    //CheckBoxWetPaintWiki.Checked = attendee.vist == 1 ? true : false;
                    // use Vistaslotsid for whether to use wetpaint wiki or not

                    CheckBoxShareInfo.Checked = attendeeRec.UserShareInfo ?? false;

                    const bool cacheImage = false;
                    ImageUser.ImageUrl = "~/DisplayImage.ashx?PKID=" + guidString + "&Cache=" + cacheImage;

                    CheckBoxSaturday.Checked = attendeesCodeCampYearResults[0].AttendSaturday;  //Utils.GetWhetherAttendingCurrentCodeCamp("saturday", attendee.Username);
                    CheckBoxSunday.Checked = attendeesCodeCampYearResults[0].AttendSunday;  //Utils.GetWhetherAttendingCurrentCodeCamp("sunday", attendee.Username);
                    CheckBoxVolunteering.Checked = attendeesCodeCampYearResults[0].Volunteer ?? false;
                    TextBoxPhoneNumber.Text = attendeeRec.PhoneNumber;




                }
                TableData.Visible = true;
            }
        }
        //else
        //{
        //    // this means not current code camp year
        //    LabelTopMessage.Text =
        //        "Profile Only Available When Looking At Current Code Camp Year (See Upper Right Hand Corner DropDownList)";
        //    FullProfileId.Visible = false;
        //}
        else
        {
            LabelTopMessage.Text = "Please Login to Update Profile";
            TableData.Visible = false;
            //ObjectDataSourceVista.SelectParameters.Add("maxPerSlot", TypeCode.Int32,"99999");
            //ObjectDataSourceVista.SelectParameters.Add("showId", TypeCode.Int32, "-1");
        }
    }

    protected void ButtonUpdate_Click(object sender, EventArgs e)
    {
        bool updateSuccess = UpdateInfo();
    }

    private bool UpdateInfo()
    {
        bool updateSuccess = false;
        if (ValidateInfo())
        {
            try
            {
                SqlBytes sqlBytes = null;
                if (FileUploadImage.HasFile)
                {
                    Byte[] byteAray = FileUploadImage.FileBytes;
                    var ms = new MemoryStream(byteAray);
                    Byte[] smallerImageBytes =
                        Utils.ResizeFromStream(FileUploadImage.FileName, Utils.BigSize, ms);

                    sqlBytes = new SqlBytes(smallerImageBytes);

                    Utils.ClearCacheForAttendeeImage(LabelUsername.Text);
                }

                var attendeeODS = new AttendeesODS();
                attendeeODS.UpdateSpecial(LabelUsername.Text,
                                          CheckBoxWetPaintWiki.Checked ? 1 : 0,
                                          TextBoxEmailAddress.Text,
                                          TextBoxWebBlog.Text,
                                          TextBoxFirstName.Text,
                                          TextBoxLastName.Text,
                                          TextBoxZipCode.Text,
                                          TextBoxBio.Text,
                                          CheckBoxShareInfo.Checked,
                                          CheckBoxAllowAttendeesToEmailMe.Checked,
                                          CheckBoxSaturdayDinner.Checked,
                                          CheckBoxSaturday.Checked,
                                          CheckBoxSunday.Checked,
                                          "true"
                    );

                if (sqlBytes != null)
                {
                    attendeeODS.UpdateBlobsAttendees(sqlBytes, LabelUsername.Text);
                }

                // (ignore what is in attendees file for now on)
                Utils.UpdateAttendeeAttendance(Utils.CurrentCodeCampYear,LabelUsername.Text,CheckBoxSaturday.Checked, CheckBoxSunday.Checked);


                updateSuccess = true;
            }
            catch (Exception eee)
            {
                throw new ApplicationException("Update Profile Failed " + eee);
            }
            ErrorRow.Visible = false;
        }
        else
        {
            updateSuccess = false;
            ErrorRow.Visible = true;
        }
        if (updateSuccess)
        {
            Response.Redirect("~/ProfileInfoUpdateSuccess.aspx", true);
        }
        return updateSuccess;
    }

    private bool ValidateInfo()
    {
        var errorList = new List<string>();


        if (!CheckBoxSaturday.Checked && !CheckBoxSunday.Checked)
        {
            errorList.Add("Must chose Saturday or Sunday to register.");
        }

        // only check vista stuff if interested in vista
        //if (!RadioButtonListVista.SelectedValue.Equals("1"))
        //{
        //    if (RadioButtonListDesktopOrLaptop.SelectedValue.Equals("unknown") &&
        //        (RadioButtonListVista.SelectedValue.Equals("2") ||
        //            RadioButtonListVista.SelectedValue.Equals("3") ||
        //            RadioButtonListVista.SelectedValue.Equals("4") ||
        //            RadioButtonListVista.SelectedValue.Equals("5")))
        //    {
        //        errorList.Add("Must Choose Desktop or Laptop for Vista Upgrade.");
        //    }
        //}

        const string emailRE = @"^(\w+([-+.']\w+)*)@([\w-]+\.)+[\w-]+$";
        Match match = Regex.Match(TextBoxEmailAddress.Text, emailRE);
        if (!match.Success)
        {
            errorList.Add("Email Not In Valid Format.");
        }

        //if (CheckBoxVistaUpgradeOnly.Checked &&
        //    (CheckBoxClassesSaturday.Checked || CheckBoxClassesSunday.Checked))
        //{
        //    errorList.Add(@"Can Not Attend Classes if 'Only Coming to Vista Upgrade' Box is Checked.");
        //}


        if (errorList.Count > 0)
        {
            // errorList.Count.ToString();
            var sb = new StringBuilder();

            sb.Append("<ul type=square><li>Please Fix Problems Below, then press Update Again</li><li></li>");
            // square doesn't seem to work, no idea why.
            foreach (string errorStr in errorList)
            {
                sb.Append("<li >");
                sb.Append(errorStr);
                sb.Append("</li>");
            }
            sb.Append("</ul>");

            LabelErrors.Text = sb.ToString();
            LabelErrors.Visible = true;

            return false;
        }
        else
        {
            LabelErrors.Visible = false;
            return true;
        }
    }

    protected void ButtonCancelRegistration_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/ProfileInfoAccountCancel.aspx", true);
    }

    protected void ButtonRegister_Click(object sender, EventArgs e)
    {
        if (ValidateInfo())
        {
            bool updateSuccess = UpdateInfo();
            AttendeesCodeCampYearManager.I.Insert(new AttendeesCodeCampYearResult()
            {
                AttendeesId =
                    Utils.GetAttendeesIdFromUsername(Context.User.Identity.Name),
                CodeCampYearId = Utils.CurrentCodeCampYear
            });
            Response.Redirect("~/ProfileInfoAccount.aspx", true);
        }

        
    }

    //
    protected void ButtonUnsubscribe_Click(object sender, EventArgs e)
    {
         var attendeesODS = new AttendeesODS();
            List<AttendeesODS.DataObjectAttendees> listAttendees = attendeesODS.GetByUsername(string.Empty,
                                                                                              Context.User.Identity.Name,
                                                                                              false);
            AttendeesODS.DataObjectAttendees attendee = listAttendees[0];

        string email = attendee.Email;


     


        var optoutUser = (EmailOptOutManager.I.Get(new EmailOptOutQuery() {Email = email})).SingleOrDefault();
        if (optoutUser == null)
        {
            LabelUnsubscribe.Visible = true;
            LabelUnsubscribe.Text = email + " Has been unsubscribed. To resubscribe, contact " + Utils.GetServiceEmailAddress();
            LabelUnsubscribe.BackColor = Color.Blue;
            LabelUnsubscribe.ForeColor = Color.Yellow;
            ButtonUnsubscribe.Enabled = false;

            EmailOptOutManager.I.Insert(new EmailOptOutResult(){ Comment = "Opted Out By Profile",DateAdded = DateTime.Now,Email = email});

        }
        else
        {
            LabelUnsubscribe.Visible = true;
            LabelUnsubscribe.Text = email + " has previously been opted out.";
            LabelUnsubscribe.BackColor = Color.Red;
            LabelUnsubscribe.ForeColor = Color.Yellow;
            ButtonUnsubscribe.Enabled = false;
        }


    }
}