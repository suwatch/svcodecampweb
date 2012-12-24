using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using App_Code;
using CodeCampSV;
using System.Linq;


public enum RegistrationPageMode
{
    NotRegistered = 1,
    UpdateNotRegisteredThisYear = 2,
    UpdateRegisteredThisYear = 3
}


public partial class Register : BaseContentPage
{

    protected bool DuplicateUserName;
    protected bool DuplicateEmail;
    protected bool RegisteredForCurrentYear;
    protected AttendeesResult Attendee;
    protected RegistrationPageMode PageMode;
    protected bool CaptchaInvalid;
    protected bool DoValidation;

    //private bool _inOptOutList;


   

    protected void ServerValidate(object source, ServerValidateEventArgs value)
    {
        // verify that something is checked on this page
        bool pageValid = (CheckBoxSaturday.Checked || CheckBoxSunday.Checked || CheckBoxUnableToAttend.Checked) 
                         || RadioButtonListEmailSubcription.SelectedValue.Equals("1") ||
                         RadioButtonListEmailSubcription.SelectedValue.Equals("2");


        value.IsValid = pageValid;
    }

    protected void ServerValidateDuplicateUserName(object source, ServerValidateEventArgs value)
    {
        if (DuplicateUserName)
        {
            value.IsValid = false;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        //LabelShowJobsNotReadyMessage.Visible = true;
        //if (CheckBoxVolunteer.Checked)
        //{
        //    if (ConfigurationManager.AppSettings["ShowVolunteerJobsToAttendees"].ToLower().Equals("true")
        //        || Utils.CheckUserIsAdmin() || Utils.CheckUserIsVolunteerCoordinator())
        //    {
        //        ButtonVolunteerSignup.Enabled = true;
        //        LabelShowJobsNotReadyMessage.Visible = false;
        //    }
        //    else
        //    {
        //        LabelShowJobsNotReadyMessage.Visible = true;
        //    }
        //}

        if (DoValidation)
        {
            if (DuplicateUserName)
            {
                ValidationSummary1.AddMessage("Username Exists.  Choose another username please");
            }

            if (DuplicateEmail)
            {
                ValidationSummary1.AddMessage("Email Exists.  Choose another Email please");
            }

            if (CheckBoxVolunteer.Checked && String.IsNullOrEmpty(TextBoxPhoneNumber.Text))
            {
                ValidationSummary1.AddMessage("If you check Volunteer, you must provide a contact phone number");
            }

            if (CaptchaInvalid)
            {
                ValidationSummary1.AddMessage("CAPTCHA Failed. Retype Special Characters");
            }

            if (Context.User.Identity.IsAuthenticated)
            {

                if (PageMode != RegistrationPageMode.NotRegistered)
                {
                    //if (!IsPostBack)
                    //{
                        CaptchaUltimateControl1.DataBind();
                        this.DataBind();
                    //}
                }
            }
        }
    }

    protected void Page_PreInit(object sender, EventArgs e)
    {
        DuplicateUserName = false;
        DuplicateEmail = false;
        RegisteredForCurrentYear = false;
        CaptchaInvalid = false;
        DoValidation = false;
      

        PageMode = RegistrationPageMode.NotRegistered;
        if (Context.User.Identity.IsAuthenticated)
        {
            RegisteredForCurrentYear =
                Utils.IsRegisteredForCurrentCodeCampYear(Context.User.Identity.Name, Utils.CurrentCodeCampYear);
            if (RegisteredForCurrentYear)
            {
                PageMode = RegistrationPageMode.UpdateRegisteredThisYear;
            }
            else
            {
                PageMode = RegistrationPageMode.UpdateNotRegisteredThisYear;
            }

           
        }
           
    }

    // 
    protected bool GetAttendeeBoolIfAuthenticated(string parameterName)
    {
        bool retBool = false;
        if (Context.User.Identity.IsAuthenticated)
        {
            
            if (parameterName.Equals("QREmailAllow"))
            {
                retBool = Attendee.QREmailAllow ?? false;
            }
            else if (parameterName.Equals("QRWebSiteAllow"))
            {
                retBool = Attendee.QRWebSiteAllow ?? false;
            }
            else if (parameterName.Equals("QRAddressLine1Allow"))
            {
                retBool = Attendee.QRAddressLine1Allow ?? false;
            }
            else if (parameterName.Equals("QRZipCodeAllow"))
            {
                retBool = Attendee.QRZipCodeAllow ?? false;
            }
        }
        return retBool;
    }

    protected string GetAttendeeInfoIfAuthenticated(string parameterName)
    {
        if (Context.User.Identity.IsAuthenticated)
        {
            var retStr = string.Empty;

            if (parameterName.Equals("FirstName"))
            {
                retStr = Attendee.UserFirstName;
            }
            else if (parameterName.Equals("LastName"))
            {
                retStr = Attendee.UserLastName;
            }
            else if (parameterName.Equals("Email"))
            {
                retStr = Attendee.Email;
            }
            else if (parameterName.Equals("WebSite"))
            {
                retStr = Attendee.UserWebsite;
            }
            else if (parameterName.Equals("ZipCode"))
            {
                retStr = Attendee.UserZipCode;
            }
            else if (parameterName.Equals("AddressLine1"))
            {
                retStr = Attendee.AddressLine1;
            }
            else if (parameterName.Equals("UserBio"))
            {
                retStr = Attendee.UserBio;
            }       

            else if (parameterName.Equals("ShareInfo"))
            {
                retStr = Attendee.UserShareInfo ?? false ? "true" : "false";
            }
            else if (parameterName.Equals("TwitterHandle"))
            {
                retStr = Attendee.TwitterHandle;
            }
                                  
           
            return retStr;
        }
        return string.Empty;
    }

    protected string GetButtonRegisterOrUpdateText()
    {
        string retText = "Register";
        switch (PageMode)
        {
            case RegistrationPageMode.UpdateNotRegisteredThisYear:
                retText = "Update Registration";
                break;
            case RegistrationPageMode.UpdateRegisteredThisYear:
                retText = "Update Information";
                break;
        }
        return retText;
    }

    protected bool ShowUsernamePasswordFields()
    {
        if (PageMode == RegistrationPageMode.NotRegistered)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        int maxRegistration = Convert.ToInt32(ConfigurationManager.AppSettings["MaxRegistration"] ?? "99999999");
        int numberRegisteredCurrentYear = Utils.GetNumberRegistered();
        bool registrationClosed = numberRegisteredCurrentYear > maxRegistration;

        // if not logged in, don't let them go to registratoin page if full
        if (!Context.User.Identity.IsAuthenticated && registrationClosed)
        {
            Response.Redirect("~/CodeCampClosedForRegistration.aspx");
        }

        if (!IsPostBack)
        {
            int sponsorListId = Utils.GetSponsorIdBasedOnUsername(Context.User.Identity.Name);
            if (sponsorListId > 0)
            {
                var rec = SponsorListManager.I.Get(new SponsorListQuery() { Id = sponsorListId }).FirstOrDefault();
                HyperLinkSponsorInformation.Text = String.Format(
                    "Sponsorship Information For {0}", rec.SponsorName);
                HyperLinkSponsorInformation.Visible = true;
            }
        }

        // if logged in, but currently not registered for saturday or sunday, don't let them login in unless special role set

        if (Context.User.Identity.IsAuthenticated && registrationClosed)
        {
            if (!Utils.CheckUserIsAllowRegistration() 
                && !Utils.CheckUserIsVolunteerCoordinator() && !Utils.CheckUserIsPresenterOrAdmin())
            {
                int attendeeId = Utils.GetAttendeesIdFromUsername(Context.User.Identity.Name);
                var attendeeRec = AttendeesManager.I.Get(new AttendeesQuery() {Id = attendeeId}).FirstOrDefault();
                if (attendeeRec != null)
                {
                     
                    if (!Utils.IsRegisteredForCurrentCodeCampYear(Context.User.Identity.Name,
                                                                 Utils.CurrentCodeCampYear))
                    {
                        Response.Redirect("~/CodeCampClosedForRegistration.aspx");
                    }
                }

            }

           
        }





       
        // could optimize below if statement)
        if ((ConfigurationManager.AppSettings["SubmitSessionsOpen"] != null &&
            ConfigurationManager.AppSettings["SubmitSessionsOpen"].Equals("true")) || Utils.CheckUserIsSubmitSession() ||
            Utils.CheckUserIsAdmin())
        {
            CheckBoxSpeakerDDL.Enabled = true;
            LabelSessionClosedMessage.Text = String.Empty;
        }
        else
        {
            CheckBoxSpeakerDDL.Enabled = false;
            LabelSessionClosedMessage.Text = "Sessions are closed to new submissions";
        }

        if (Request.QueryString["PKID"] != null)
        {
            string guidString = Request.QueryString["PKID"];
            string username = Utils.GetAttendeeUsernameByGUID(guidString);
            if (!String.IsNullOrEmpty(username))
            {
                if (!Utils.GetIgnoreAutoSignOnGuid(username))
                {
                    if (!string.IsNullOrEmpty(username))
                    {
                        if (User.Identity.IsAuthenticated)
                        {
                            FormsAuthentication.SignOut();
                        }
                        FormsAuthentication.SetAuthCookie(username, true);
                        Response.Redirect("~/Register.aspx", true);
                    }
                }
            }
        }

        if (PageMode == RegistrationPageMode.NotRegistered)
        {
            if (!IsPostBack)
            {
                RegisteringEasyFreeId.Text = "Registering is easy and FREE!";
                LoggedInButNotRegisteredThisYearID.Visible = false;
                ButtonUpdateOrRegister.Text = "Register";
                ButtonRegisterOrUpdate1.Text = "Register";
                //ButtonCancelRegistration.Visible = false;
                //ButtonUnsubscribe.Visible = false;
            }
        }

        if (Context.User.Identity.IsAuthenticated)
        {
            int attendeeId = Utils.GetAttendeesIdFromUsername(Context.User.Identity.Name);
            bool isSpeakingThisYear = Utils.CheckAttendeeIdIsSpeaker(attendeeId);

            if (isSpeakingThisYear)
            {
                if (ConfigurationManager.AppSettings["SpeakerShirtSizes"] != null)
                {
                    string list = ConfigurationManager.AppSettings["SpeakerShirtSizes"];
                    char[] splitchar = { ',' };
                    List<string> newList = list.Split(splitchar).ToList();
                    DropDownListSpeakerShirtSize.Items.Add("--Not Selected");
                    foreach (var item in newList)
                    {
                        DropDownListSpeakerShirtSize.Items.Add(new ListItem(item.Trim(), item.Trim()));
                    }
                }
                SpeakerShirtSizeDiv.Visible = true;
            }
           
            IDRegistrationInfo.Visible = true;
            IDAuthenticated.Visible = true;

            Attendee =
                AttendeesManager.I.Get(new AttendeesQuery
                                           {
                                               Id = attendeeId
                                           }).FirstOrDefault();

            if (Attendee != null && !IsPostBack)
            {
                ButtonUpdateOrRegister.Text = GetButtonRegisterOrUpdateText();
                ButtonRegisterOrUpdate1.Text = GetButtonRegisterOrUpdateText();
                TextBoxPhoneNumber.Text = Attendee.PhoneNumber;
                TextBoxFalafelEventBoardEmail.Text = Attendee.EmailEventBoard;



                ListItem listItem = DropDownListSpeakerShirtSize.Items.FindByValue(Attendee.ShirtSize);
                if (listItem != null)
                {
                    DropDownListSpeakerShirtSize.SelectedValue = listItem.Value;
                }



                CheckBoxAllowEmailFromSpeakerInterested.Checked = Attendee.AllowEmailToSpeakerInterested != null ? Attendee.AllowEmailToSpeakerInterested.Value : false;
                CheckBoxAllowEmailFromSpeakerPlanToAttend.Checked = Attendee.AllowEmailToSpeakerPlanToAttend != null ? Attendee.AllowEmailToSpeakerPlanToAttend.Value : false;
                CheckBoxQRPhoneNumber.Checked = Attendee.QRPhoneAllow != null ? Attendee.QRPhoneAllow.Value : false;

                if (Attendee.EmailSubscription != null)
                {
                    RadioButtonListEmailSubcription.SelectedIndex = Attendee.EmailSubscription.Value;
                }
                else
                {
                    Attendee.EmailSubscription = (int) EmailSubscriptionEnum.AllEmails;
                    RadioButtonListEmailSubcription.SelectedIndex = 0;
                }
                LabelEmailStatusMessage.Text = Attendee.EmailSubscriptionStatus;

                if (Attendee.EmailSubscription == 2)
                {
                    LabelEmailBouncing.Visible = true;
                }

            }




            // Need to see if person already registered for this year.  If so, then take them to profile page
            var attendeesCodeCampYearResult =
                AttendeesCodeCampYearManager.I.Get(new AttendeesCodeCampYearQuery()
                                                       {
                                                           AttendeesId = attendeeId,
                                                           CodeCampYearId = Utils.CurrentCodeCampYear
                                                       }).FirstOrDefault();


            // keep value from being overwritten when autopostback true happens.
            if (attendeesCodeCampYearResult != null && !IsPostBack)
            {
                CheckBoxSaturday.Checked = attendeesCodeCampYearResult.AttendSaturday;
                CheckBoxSunday.Checked = attendeesCodeCampYearResult.AttendSunday;
                CheckBoxVolunteer.Checked = attendeesCodeCampYearResult.Volunteer ?? false;
            }
            else if (CheckBoxUnableToAttend.Checked)
            {
                CheckBoxSaturday.Checked = false;
                CheckBoxSunday.Checked = false;
                var rec = AttendeesCodeCampYearManager.I.Get(new AttendeesCodeCampYearQuery()
                {
                    AttendeesId =
                        attendeeId,
                    CodeCampYearId =
                        Utils.CurrentCodeCampYear,
                }).FirstOrDefault();
                if (rec != null)
                {
                    AttendeesCodeCampYearManager.I.Delete(rec.Id);
                }
                PageMode = RegistrationPageMode.UpdateNotRegisteredThisYear;
            }

            if (PageMode == RegistrationPageMode.UpdateNotRegisteredThisYear)
            {
                RegisteringEasyFreeId.Text = "Update Your Profile For This Year Now!";
                LoggedInButNotRegisteredThisYearID.Visible = true;
                //ButtonCancelRegistration.Visible = false;
                //ButtonUnsubscribe.Visible = true;
            }
            else if (PageMode == RegistrationPageMode.UpdateRegisteredThisYear)
            {
                RegisteringEasyFreeId.Text = "Update Your Profile!";
                LoggedInButNotRegisteredThisYearID.Visible = false;
                //ButtonCancelRegistration.Visible = true;
                //ButtonUnsubscribe.Visible = true;
            }

            //// check if person has sessions.  If so, then make CheckBoxSpeakerDDL default ot Yes
            //List<int> allSessionIdsByAttendeeId =
            //    SessionPresenterManager.I.Get(new SessionPresenterQuery()
            //                                      {
            //                                          AttendeeId = attendeeId
            //                                      }).Select(a => a.SessionId).ToList();
            //int sessionsThisYearCnt = SessionsManager.I.Get(
            //    new SessionsQuery
            //        {
            //            Ids = allSessionIdsByAttendeeId
            //        }).Count;

            if (isSpeakingThisYear)
            {
                CheckBoxSpeakerDDL.SelectedIndex = 0; // first choice is YES
            }

            if (!IsPostBack)
            {
                CaptchaUltimateControl1.DataBind();
            }
        }
        else
        {
            IDRegistrationInfo.Visible = true;
            IDAuthenticated.Visible = false;
            //CaptchaUltimateControl1.Visible = true;
            //LoginStatus1.Visible = false;
            //ShowMessage.Visible = true;
        }



        // check and see if a referral URL is coming in.  If so, keep it for adding
        // to user account
        try
        {
            if (Request.QueryString["Referral"] != null)
            {
                string referralPKIDString = Request.QueryString["Referral"];
                var referralGuid = new Guid(referralPKIDString);
                Items["ReferralGuid"] = referralGuid;
            }
            else if (Session["ReferralGuid"] != null)
            {
                Items["ReferralGuid"] = (Guid) Session["ReferralGuid"];
            }
        }
        catch (Exception ee)
        {
            throw new ApplicationException(ee.ToString());
        }


        //FileUpload fileUpload = (FileUpload) CaptchaUltimateControl1.FindControl("FileUpload1");
        //string str = fileUpload.FileName;
        //string str1 = fileUpload.

        //if (Context.User.Identity.IsAuthenticated)
        //{
        //    int attendeeId = Utils.GetAttendeesIdFromUsername(Context.User.Identity.Name);
        //    AttendeesResult attendeeResultRec =
        //        AttendeesManager.I.Get(new AttendeesQuery() {Id = attendeeId}).FirstOrDefault();
        //    if (attendeeResultRec != null)
        //    {
        //        int totalRecs = (EmailOptOutManager.I.Get(
        //            new EmailOptOutQuery()
        //                {
        //                    Email = attendeeResultRec.Email
        //                })).Count;
        //        if (totalRecs > 0)
        //        {
        //            ButtonReSubscribe.Visible = true;
        //            ButtonUnsubscribe.Visible = false;
        //        }
        //        else
        //        {
        //            ButtonReSubscribe.Visible = false;
        //            ButtonUnsubscribe.Visible = true;
        //        }
        //    }
        //}


    }


    private string GetFromCaptchaControl(string fieldName)
    {
        string retString = string.Empty;
        var textBox = (TextBox) CaptchaUltimateControl1.FindControl(fieldName);
        if (textBox != null)
        {
            retString = textBox.Text;
        }
        return retString;
    }

    private bool GetFromCaptchaControlCheckBox(string fieldName)
    {
        bool ret = false;
        var checkBox = (CheckBox)CaptchaUltimateControl1.FindControl(fieldName);
        if (checkBox != null)
        {
            ret = checkBox.Checked;
        }
        return ret;
    }

    protected void CaptchaUltimateControl1_Verifying(object sender, VerifyingEventArgs e)
    {
        bool authenticated = Context.User.Identity.IsAuthenticated;

        if (ConfigurationManager.AppSettings["OverrideCaptcha"].ToLower().Equals("true")
            || authenticated)
        {
            e.ForceVerify = true;
        }    
    }

    // 
    protected void CaptchaUltimateControl1_FailedVerify(object sender, EventArgs e)
    {
        CaptchaInvalid = true;
    }
  
    protected void CaptchaUltimateControl1_Verified(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            var sqlConnection =
                   new SqlConnection(ConfigurationManager.ConnectionStrings["CodeCampSV06"].ConnectionString);
            sqlConnection.Open();


            bool completionStatus = false;
            string completionMessage = string.Empty;
            string userName = GetFromCaptchaControl("TextBoxUserName");
            string password = GetFromCaptchaControl("TextBoxPassword");
            if (!CheckBoxVolunteer.Checked || !String.IsNullOrEmpty(TextBoxPhoneNumber.Text))
            {
                CheckBox checkBoxSpeaker = (CheckBox)CaptchaUltimateControl1.FindControl("CheckBoxSpeaker");

                bool speaking = CheckBoxSpeakerDDL.SelectedValue.ToLower().Equals("yes") ? true : false;

                // I think transactionscope is failing because in the membership provider,
                // an open and close of the connection is done.  I'll need to mess with that later.
                // for now, this is not transaction safe.  bummer.
                //using (TransactionScope scope = new TransactionScope())
                //{
                // $$$ Broken scope, will not work.  See SessionsEdit.aspx for correct way


                if (PageMode == RegistrationPageMode.NotRegistered)
                {
                    MembershipCreateStatus mStatus;
                    Membership.CreateUser(
                        userName,
                        password,
                        GetFromCaptchaControl("TextBoxEmail"),
                        "Question", "Answer",
                        true,
                        out mStatus);

                    if (mStatus.Equals(MembershipCreateStatus.Success))
                    {
                        completionStatus = true; // default, but just to be safe
                        Utils.ClearDisplayAdCache();
                    }
                    else
                    {
                        completionStatus = false;
                        completionMessage = mStatus.ToString();

                        if (mStatus == MembershipCreateStatus.DuplicateUserName)
                        {
                            DuplicateUserName = true;
                        }
                        if (mStatus == MembershipCreateStatus.DuplicateEmail)
                        {
                            DuplicateEmail = true;
                        }

                    }
                }
                else
                {
                    // this means user created and we are just updating
                    completionStatus = true;
                    userName = Context.User.Identity.Name;
                }
            }

            if (completionStatus)
            {
                // Grab all the other fields hoping that the image will upload OK.
                string firstName = GetFromCaptchaControl("TextBoxFirstName");
                string lastName = GetFromCaptchaControl("TextBoxLastName");
                string zipcode = GetFromCaptchaControl("TextBoxZipCode");
                string webSite = GetFromCaptchaControl("TextBoxWebsite");
                string bio = GetFromCaptchaControl("TextBoxBio");
                bool checkBoxShare = GetFromCaptchaControlCheckBox("CheckBoxShareInfo");
                string email = GetFromCaptchaControl("TextBoxEmail");
                string phoneNumber = TextBoxPhoneNumber.Text;
                bool volunteering = CheckBoxVolunteer.Checked;
                string emailEventBoard = TextBoxFalafelEventBoardEmail.Text;
                bool allowEmailToSpeakerPlanToAttend = CheckBoxAllowEmailFromSpeakerPlanToAttend.Checked;
                bool allowEmailToSpeakerInterested = CheckBoxAllowEmailFromSpeakerInterested.Checked;

                string addressLine1 = GetFromCaptchaControl("TextBoxAddressLine1");
                string twitterHandle = GetFromCaptchaControl("TextBoxTwitterHandle");
                bool qREmailAllow = GetFromCaptchaControlCheckBox("CheckBoxQREmail");
                bool qRWebSiteAllow = GetFromCaptchaControlCheckBox("CheckBoxQRWebSite");
                bool qRAddressLine1Allow = GetFromCaptchaControlCheckBox("CheckBoxQRAddressLine1");
                bool qRZipCodeAllow = GetFromCaptchaControlCheckBox("CheckBoxQRZip");

                string shirtSize = DropDownListSpeakerShirtSize.SelectedValue;
                int emailSubscription = Convert.ToInt32(RadioButtonListEmailSubcription.SelectedItem.Value);


                if (shirtSize.Equals("--Not Selected"))
                {
                    shirtSize = string.Empty;
                }
                
             
                // try grabbing the image specified
                var fileUpload = (FileUpload) CaptchaUltimateControl1.FindControl("FileUpload1");
                Byte[] smallerImageBytes = null;
                Byte[] imageBytes;

                try
                {
                    if (fileUpload != null && fileUpload.HasFile)
                    {
                        imageBytes = fileUpload.FileBytes;
                        if (imageBytes.Length > 1000000)
                        {
                            completionStatus = false;
                            completionMessage = "Image May not be over 1 Meg, try again";
                        }
                        else
                        {
                            var ms = new MemoryStream(imageBytes);
                            string fName = string.Empty;
                            if (!string.IsNullOrEmpty(fileUpload.FileName))
                            {
                                fName = fileUpload.FileName;
                            }
                            smallerImageBytes = Utils.ResizeFromStream(fName, Utils.ThumbSize, ms);
                        }
                    }
                }
                catch (Exception eee)
                {
                    throw new ApplicationException(eee.ToString());
                }

                Guid referralGuid = Items["ReferralGuid"] == null ? Guid.Empty : (Guid) Items["ReferralGuid"];

                // upload the image to the database
                string sqlUpdate =
                    "UPDATE dbo.Attendees SET " +
                    "  ShirtSize = @ShirtSize, " +
                    "  EmailSubscription = @EmailSubscription, " +
                    "  AddressLine1 = @AddressLine1,  " +
                    "  QREmailAllow = @QREmailAllow, " +
                    "  QRWebSiteAllow = @QRWebSiteAllow, " +
                    "  QRAddressLine1Allow = @QRAddressLine1Allow, " +
                    "  TwitterHandle = @TwitterHandle, " +
                    "  QRZipCodeAllow = @QRZipCodeAllow, " +
                    "  QRPhoneAllow = @QRPhoneAllow, " +
                    "  AllowEmailToSpeakerPlanToAttend = @AllowEmailToSpeakerPlanToAttend, " +
                    "  AllowEmailToSpeakerInterested = @AllowEmailToSpeakerInterested, " +
                    "  UserWebsite = @UserWebsite, " +
                    "  VistaOnly = 1," +
                    "  Email = @Email," +
                    "  PhoneNumber = @PhoneNumber," +
                    "  EmailEventBoard = @EmailEventBoard," +
                    "  UserFirstName = @UserFirstName, " +
                    "  UserLastName = @UserLastName, " +
                    "  UserZipCode = @UserZipCode, " +
                    "  UserBio = @UserBio, ";

                if (!referralGuid.Equals(Guid.Empty))
                {
                    sqlUpdate += "  ReferralGuid = @ReferralGuid, ";
                }
                if (smallerImageBytes != null)
                {
                    sqlUpdate += "  UserImage = @UserImage, ";
                }
                sqlUpdate +=
                    "  UserShareInfo = @UserShareInfo " +
                    "WHERE " +
                    " Username = @Username";

                try
                {
                    var sqlCommand = new SqlCommand(sqlUpdate, sqlConnection);

                    sqlCommand.Parameters.Add("@UserWebsite", SqlDbType.VarChar, 256).Value = webSite;
                    sqlCommand.Parameters.Add("@UserFirstName", SqlDbType.VarChar, 128).Value = firstName;
                    sqlCommand.Parameters.Add("@UserLastName", SqlDbType.VarChar, 128).Value = lastName;
                    sqlCommand.Parameters.Add("@UserZipCode", SqlDbType.VarChar, 128).Value = zipcode;
                    sqlCommand.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = userName;
                    sqlCommand.Parameters.Add("@Email", SqlDbType.VarChar, 255).Value = email;
                    sqlCommand.Parameters.Add("@EmailEventBoard", SqlDbType.VarChar, 255).Value = emailEventBoard;
                    sqlCommand.Parameters.Add("@UserShareInfo", SqlDbType.Bit).Value = checkBoxShare;
                    sqlCommand.Parameters.Add("@UserBio", SqlDbType.Text).Value = bio;
                    sqlCommand.Parameters.Add("@PhoneNumber", SqlDbType.Text).Value = phoneNumber;

                    sqlCommand.Parameters.Add("@AllowEmailToSpeakerPlanToAttend", SqlDbType.Bit).Value =
                        allowEmailToSpeakerPlanToAttend;

                    sqlCommand.Parameters.Add("@AllowEmailToSpeakerInterested", SqlDbType.Bit).Value =
                        allowEmailToSpeakerInterested;

                    sqlCommand.Parameters.Add("@AddressLine1", SqlDbType.NVarChar).Value = addressLine1;
                    sqlCommand.Parameters.Add("@TwitterHandle", SqlDbType.NVarChar).Value = twitterHandle;

                    sqlCommand.Parameters.Add("@QREmailAllow", SqlDbType.Bit).Value = qREmailAllow;
                    sqlCommand.Parameters.Add("@QRWebSiteAllow", SqlDbType.Bit).Value = qRWebSiteAllow;
                    sqlCommand.Parameters.Add("@QRAddressLine1Allow", SqlDbType.Bit).Value = qRAddressLine1Allow;
                    sqlCommand.Parameters.Add("@QRZipCodeAllow", SqlDbType.Bit).Value = qRZipCodeAllow;

                    sqlCommand.Parameters.Add("@ShirtSize", SqlDbType.NVarChar).Value = shirtSize;
                    sqlCommand.Parameters.Add("@EmailSubscription", SqlDbType.Int).Value = emailSubscription;



                    sqlCommand.Parameters.Add("@QRPhoneAllow", SqlDbType.Bit).Value = CheckBoxQRPhoneNumber.Checked;

                    if (!referralGuid.Equals(Guid.Empty))
                    {
                        sqlCommand.Parameters.Add("@ReferralGuid", SqlDbType.UniqueIdentifier).Value = referralGuid;
                    }
                    if (smallerImageBytes != null)
                    {
                        sqlCommand.Parameters.Add("@UserImage", SqlDbType.Image, smallerImageBytes.Length).Value =
                            smallerImageBytes;
                    }

                    int rowsUpdated = sqlCommand.ExecuteNonQuery();
                    if (rowsUpdated != 1)
                    {
                        completionStatus = false;
                        completionMessage = "SqlUpdate did not return 1 row." + rowsUpdated;
                    }
                    else
                    {



                        // Add the cross reference for code camp
                        if (PageMode != RegistrationPageMode.UpdateRegisteredThisYear)
                        {
                            if (CheckBoxSunday.Checked || CheckBoxSaturday.Checked)
                            {
                                AttendeesCodeCampYearManager.I.Insert(new AttendeesCodeCampYearResult
                                                                          {
                                                                              AttendeesId =
                                                                                  Utils.GetAttendeesIdFromUsername(
                                                                                      userName),
                                                                              CodeCampYearId = Utils.CurrentCodeCampYear,
                                                                              AttendSaturday = CheckBoxSaturday.Checked,
                                                                              AttendSunday = CheckBoxSunday.Checked,
                                                                              Volunteer = CheckBoxVolunteer.Checked,
                                                                              CreateDate = DateTime.Now
                                                                          });
                            }
                        }
                        else
                        {
                            var rec = AttendeesCodeCampYearManager.I.Get(new AttendeesCodeCampYearQuery()
                                                                             {
                                                                                 AttendeesId =
                                                                                     Utils.
                                                                                     GetAttendeesIdFromUsername(
                                                                                         userName),
                                                                                 CodeCampYearId =
                                                                                     Utils.CurrentCodeCampYear,
                                                                             }).FirstOrDefault();
                            if (rec != null)
                            {
                                rec.AttendSaturday = CheckBoxSaturday.Checked;
                                rec.AttendSunday = CheckBoxSunday.Checked;
                                rec.Volunteer = volunteering;
                                AttendeesCodeCampYearManager.I.Update(rec);
                            }

                        }
                    }
                }
                catch (Exception updateException)
                {
                    completionStatus = false;
                    completionMessage = updateException.ToString();
                }
            }



            if (completionStatus)
            {
                //scope.Complete();

                // likely, next line will never show.
                completionMessage = "Username: " + userName + " added successfully.";
                FormsAuthentication.SetAuthCookie(userName, true);
                Session["Username"] = userName;


                if (IsUserSpeaker())
                {
                    Response.Redirect("~/RegistrationConfirmation.aspx?speaker=yes");
                }
                else
                {
                    Response.Redirect("~/RegistrationConfirmation.aspx");
                }

            }


            //}

            LabelCreateStatus.Text = completionMessage;

            sqlConnection.Close();
            sqlConnection.Dispose();
        }
    }

    private bool IsUserSpeaker()
    {
        return CheckBoxSpeakerDDL.SelectedValue.ToLower().Equals("true") ? true : false;
    }

   
    protected void ButtonUpdateProfile_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/ProfileInfoAccount.aspx");
    }

    protected void ButtonUpdateOrRegister_Click(object sender, EventArgs e)
    {
        DoValidation = true;
        CustomValidator1.Validate();
    }


    //protected void ButtonCancelRegistration_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("~/ProfileInfoAccountCancel.aspx", true);
    //}


    //protected void ButtonUnsubscribe_Click(object sender, EventArgs e)
    //{
    //    var attendeesODS = new AttendeesODS();
    //    List<AttendeesODS.DataObjectAttendees> listAttendees = attendeesODS.GetByUsername(string.Empty,
    //                                                                                      Context.User.Identity.Name,
    //                                                                                      false);
    //    AttendeesODS.DataObjectAttendees attendee = listAttendees[0];

    //    string email = attendee.Email;

    //    var optoutUser = (EmailOptOutManager.I.Get(new EmailOptOutQuery() { Email = email })).SingleOrDefault();
    //    if (optoutUser == null)
    //    {
    //        LabelUnsubscribe.Visible = true;
    //        LabelUnsubscribe.Text = email + " Has been unsubscribed. To resubscribe, contact service@siliconvalley-codecamp.com.";
    //        LabelUnsubscribe.BackColor = Color.Blue;
    //        LabelUnsubscribe.ForeColor = Color.Yellow;
    //        ButtonUnsubscribe.Enabled = false;

    //        EmailOptOutManager.I.Insert(new EmailOptOutResult
    //        {
    //            Comment = "Opted Out By Profile",
    //            DateAdded = DateTime.Now,
    //            Email = email
    //        });
    //    }
    //    else
    //    {
    //        LabelUnsubscribe.Visible = true;
    //        LabelUnsubscribe.Text = email + " has previously been opted out.";
    //        LabelUnsubscribe.BackColor = Color.Red;
    //        LabelUnsubscribe.ForeColor = Color.Yellow;
    //        ButtonUnsubscribe.Enabled = false;
    //    }
    //}

    //protected void ButtonReSubscribe_Click(object sender, EventArgs e)
    //{
    //    // Delete email from optout
    //    if (Context.User.Identity.IsAuthenticated)
    //    {
    //        int attendeeId = Utils.GetAttendeesIdFromUsername(Context.User.Identity.Name);
    //        AttendeesResult attendeeResultRec =
    //            AttendeesManager.I.Get(new AttendeesQuery() { Id = attendeeId }).FirstOrDefault();
    //        if (attendeeResultRec != null)
    //        {
    //            string emailToRemove = attendeeResultRec.Email;

    //            if (!String.IsNullOrEmpty(emailToRemove))
    //            {
    //                var recs = EmailOptOutManager.I.Get(new EmailOptOutQuery() { Email = emailToRemove });
    //                foreach (var rec in recs)
    //                {
    //                    EmailOptOutManager.I.Delete(rec.Id);
    //                }
    //            }
    //        }
    //    }
    //}
    protected void Button1_Click(object sender, EventArgs e)
    {

    }
}