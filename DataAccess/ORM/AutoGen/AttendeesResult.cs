//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CodeCampSV
{
    public partial class AttendeesResult : ResultBase
    {
        [DataMember] public Guid PKID { get; set; }
        [DataMember] public string Username { get; set; }
        [DataMember] public string ApplicationName { get; set; }
        [DataMember] public string Email { get; set; }
        [DataMember] public string EmailEventBoard { get; set; }
        [DataMember] public string Comment { get; set; }
        [DataMember] public string Password { get; set; }
        [DataMember] public string PasswordQuestion { get; set; }
        [DataMember] public string PasswordAnswer { get; set; }
        [DataMember] public bool? IsApproved { get; set; }
        [DataMember] public DateTime? LastActivityDate { get; set; }
        [DataMember] public DateTime? LastLoginDate { get; set; }
        [DataMember] public DateTime? CreationDate { get; set; }
        [DataMember] public bool? IsOnLine { get; set; }
        [DataMember] public bool? IsLockedOut { get; set; }
        [DataMember] public DateTime? LastLockedOutDate { get; set; }
        [DataMember] public int? FailedPasswordAttemptCount { get; set; }
        [DataMember] public DateTime? FailedPasswordAttemptWindowStart { get; set; }
        [DataMember] public int? FailedPasswordAnswerAttemptCount { get; set; }
        [DataMember] public DateTime? FailedPasswordAnswerAttemptWindowStart { get; set; }
        [DataMember] public DateTime? LastPasswordChangedDate { get; set; }
        [DataMember] public string UserWebsite { get; set; }
        [DataMember] public string UserLocation { get; set; }
        [DataMember][XmlIgnore()] public System.Data.Linq.Binary UserImage { get; set; }
        [DataMember] public string UserFirstName { get; set; }
        [DataMember] public string UserLastName { get; set; }
        [DataMember] public string UserZipCode { get; set; }
        [DataMember] public string UserBio { get; set; }
        [DataMember] public bool? UserShareInfo { get; set; }
        [DataMember] public Guid? ReferralGUID { get; set; }
        [DataMember] public DateTime? ConfirmedDate { get; set; }
        [DataMember] public int? VistaSlotsId { get; set; }
        [DataMember] public string FullNameUsernameZipcode { get; set; }
        [DataMember] public bool? VistaOnly { get; set; }
        [DataMember] public bool? SaturdayClasses { get; set; }
        [DataMember] public bool? SundayClasses { get; set; }
        [DataMember] public string DesktopOrLaptopForVista { get; set; }
        [DataMember] public bool? SaturdayDinner { get; set; }
        [DataMember] public string PhoneNumber { get; set; }
        [DataMember] public string AddressLine1 { get; set; }
        [DataMember] public bool? AllowEmailToSpeakerPlanToAttend { get; set; }
        [DataMember] public bool? AllowEmailToSpeakerInterested { get; set; }
        [DataMember] public bool? QREmailAllow { get; set; }
        [DataMember] public bool? QRWebSiteAllow { get; set; }
        [DataMember] public bool? QRAddressLine1Allow { get; set; }
        [DataMember] public bool? QRZipCodeAllow { get; set; }
        [DataMember] public bool? QRPhoneAllow { get; set; }
        [DataMember] public string ShirtSize { get; set; }
        [DataMember] public int? EmailSubscription { get; set; }
        [DataMember] public string EmailSubscriptionStatus { get; set; }
        [DataMember] public int? EmailBounces { get; set; }
        [DataMember] public int? VolunteerMeetingStatus { get; set; }
        [DataMember] public DateTime? VolunteerMeetingInterestDate { get; set; }
        [DataMember] public string TwitterHandle { get; set; }
        [DataMember] public string FacebookId { get; set; }
        [DataMember] public string LinkedInId { get; set; }
        [DataMember] public string GooglePlusId { get; set; }
        [DataMember] public int? OptInSponsoredMailingsLevel { get; set; }
        [DataMember] public int? OptInSponsorSpecialsLevel { get; set; }
        [DataMember] public string City { get; set; }
        [DataMember] public string State { get; set; }
        [DataMember] public int? PresentationLimit { get; set; }
        [DataMember] public bool? PresentationApprovalRequired { get; set; }
        
        //  
        //  Do not put Id here since it is in ResultBase already
        //  
        //  Might include other classes here such as:
        //  public List<PhoneResult> AssociatedPhoneResult { get; set; }
        //  or
        //  public List<SegmentResult> Segments { get; set; }
    }
}
