//   Regenerated Code
//   C 3PLogic, Inc.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlTypes;
using System.ComponentModel;

namespace CodeCampSV
{
    [Serializable]
    public partial class AttendeesQuery : QueryBase
    {
        //   Retrieving by one Id or a list of Id's is always supported.
        [AutoGenColumn]
        public int? Id { get; set; }
        [AutoGenColumn]
        public List<int> Ids { get; set; }
        // 
        //  Generate query for all columns in table
        [AutoGenColumn]
        public Guid? PKID { get; set; }
        [AutoGenColumn]
        public string Username { get; set; }
        [AutoGenColumn]
        public string ApplicationName { get; set; }
        [AutoGenColumn]
        public string Email { get; set; }
        [AutoGenColumn]
        public string EmailEventBoard { get; set; }
        [AutoGenColumn]
        public string Comment { get; set; }
        [AutoGenColumn]
        public string Password { get; set; }
        [AutoGenColumn]
        public string PasswordQuestion { get; set; }
        [AutoGenColumn]
        public string PasswordAnswer { get; set; }
        [AutoGenColumn]
        public bool? IsApproved { get; set; }
        [AutoGenColumn]
        public DateTime? LastActivityDate { get; set; }
        [AutoGenColumn]
        public DateTime? LastLoginDate { get; set; }
        [AutoGenColumn]
        public DateTime? CreationDate { get; set; }
        [AutoGenColumn]
        public bool? IsOnLine { get; set; }
        [AutoGenColumn]
        public bool? IsLockedOut { get; set; }
        [AutoGenColumn]
        public DateTime? LastLockedOutDate { get; set; }
        [AutoGenColumn]
        public int? FailedPasswordAttemptCount { get; set; }
        [AutoGenColumn]
        public DateTime? FailedPasswordAttemptWindowStart { get; set; }
        [AutoGenColumn]
        public int? FailedPasswordAnswerAttemptCount { get; set; }
        [AutoGenColumn]
        public DateTime? FailedPasswordAnswerAttemptWindowStart { get; set; }
        [AutoGenColumn]
        public DateTime? LastPasswordChangedDate { get; set; }
        [AutoGenColumn]
        public string UserWebsite { get; set; }
        [AutoGenColumn]
        public string UserLocation { get; set; }
        [AutoGenColumn]
        public System.Data.Linq.Binary UserImage { get; set; }
        [AutoGenColumn]
        public string UserFirstName { get; set; }
        [AutoGenColumn]
        public string UserLastName { get; set; }
        [AutoGenColumn]
        public string UserZipCode { get; set; }
        [AutoGenColumn]
        public string UserBio { get; set; }
        [AutoGenColumn]
        public bool? UserShareInfo { get; set; }
        [AutoGenColumn]
        public Guid? ReferralGUID { get; set; }
        [AutoGenColumn]
        public DateTime? ConfirmedDate { get; set; }
        [AutoGenColumn]
        public int? VistaSlotsId { get; set; }
        [AutoGenColumn]
        public string FullNameUsernameZipcode { get; set; }
        [AutoGenColumn]
        public bool? VistaOnly { get; set; }
        [AutoGenColumn]
        public bool? SaturdayClasses { get; set; }
        [AutoGenColumn]
        public bool? SundayClasses { get; set; }
        [AutoGenColumn]
        public string DesktopOrLaptopForVista { get; set; }
        [AutoGenColumn]
        public bool? SaturdayDinner { get; set; }
        [AutoGenColumn]
        public string PhoneNumber { get; set; }
        [AutoGenColumn]
        public string AddressLine1 { get; set; }
        [AutoGenColumn]
        public bool? AllowEmailToSpeakerPlanToAttend { get; set; }
        [AutoGenColumn]
        public bool? AllowEmailToSpeakerInterested { get; set; }
        [AutoGenColumn]
        public bool? QREmailAllow { get; set; }
        [AutoGenColumn]
        public bool? QRWebSiteAllow { get; set; }
        [AutoGenColumn]
        public bool? QRAddressLine1Allow { get; set; }
        [AutoGenColumn]
        public bool? QRZipCodeAllow { get; set; }
        [AutoGenColumn]
        public bool? QRPhoneAllow { get; set; }
        [AutoGenColumn]
        public string ShirtSize { get; set; }
        [AutoGenColumn]
        public int? EmailSubscription { get; set; }
        [AutoGenColumn]
        public string EmailSubscriptionStatus { get; set; }
        [AutoGenColumn]
        public int? EmailBounces { get; set; }
        [AutoGenColumn]
        public int? VolunteerMeetingStatus { get; set; }
        [AutoGenColumn]
        public DateTime? VolunteerMeetingInterestDate { get; set; }
        [AutoGenColumn]
        public string TwitterHandle { get; set; }
        [AutoGenColumn]
        public string FacebookId { get; set; }
        [AutoGenColumn]
        public string LinkedInId { get; set; }
        [AutoGenColumn]
        public string GooglePlusId { get; set; }
        [AutoGenColumn]
        public int? OptInSponsoredMailingsLevel { get; set; }
        [AutoGenColumn]
        public int? OptInSponsorSpecialsLevel { get; set; }
        [AutoGenColumn]
        public string City { get; set; }
        [AutoGenColumn]
        public string State { get; set; }
        [AutoGenColumn]
        public int? PresentationLimit { get; set; }
        [AutoGenColumn]
        public bool? PresentationApprovalRequired { get; set; }
        [AutoGenColumn]
        public string OptInTechJobKeyWords { get; set; }
        [AutoGenColumn]
        public string Company { get; set; }
        [AutoGenColumn]
        public string PrincipleJob { get; set; }
    }
}
