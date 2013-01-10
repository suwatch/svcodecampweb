//  This is the Manager class used for data operations.  It is meant to have another Partial
//  class associated with it.
//  C 3PLogic, Inc.
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

using CodeCampSV;


namespace CodeCampSV
{
    //  Here are the 2 methods that needs to be auto genearted. 
    //  First is a one to one maping to the database columns. 
    //  Since we auto generate the results class too, we can guarantee the columns are all there
    [DataObject(true)]
    public partial class AttendeesManager : ManagerBase<AttendeesManager, AttendeesResult, Attendees, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(Attendees record, AttendeesResult result)
        {
            record.PKID = result.PKID;
            record.Username = result.Username;
            record.ApplicationName = result.ApplicationName;
            record.Email = result.Email;
            record.EmailEventBoard = result.EmailEventBoard;
            record.Comment = result.Comment;
            record.Password = result.Password;
            record.PasswordQuestion = result.PasswordQuestion;
            record.PasswordAnswer = result.PasswordAnswer;
            record.IsApproved = result.IsApproved;
            record.LastActivityDate = result.LastActivityDate;
            record.LastLoginDate = result.LastLoginDate;
            record.CreationDate = result.CreationDate;
            record.IsOnLine = result.IsOnLine;
            record.IsLockedOut = result.IsLockedOut;
            record.LastLockedOutDate = result.LastLockedOutDate;
            record.FailedPasswordAttemptCount = result.FailedPasswordAttemptCount;
            record.FailedPasswordAttemptWindowStart = result.FailedPasswordAttemptWindowStart;
            record.FailedPasswordAnswerAttemptCount = result.FailedPasswordAnswerAttemptCount;
            record.FailedPasswordAnswerAttemptWindowStart = result.FailedPasswordAnswerAttemptWindowStart;
            record.LastPasswordChangedDate = result.LastPasswordChangedDate;
            record.UserWebsite = result.UserWebsite;
            record.UserLocation = result.UserLocation;
            record.UserImage = result.UserImage;
            record.UserFirstName = result.UserFirstName;
            record.UserLastName = result.UserLastName;
            record.UserZipCode = result.UserZipCode;
            record.UserBio = result.UserBio;
            record.UserShareInfo = result.UserShareInfo;
            record.ReferralGUID = result.ReferralGUID;
            record.ConfirmedDate = result.ConfirmedDate;
            record.VistaSlotsId = result.VistaSlotsId;
            record.FullNameUsernameZipcode = result.FullNameUsernameZipcode;
            record.VistaOnly = result.VistaOnly;
            record.SaturdayClasses = result.SaturdayClasses;
            record.SundayClasses = result.SundayClasses;
            record.DesktopOrLaptopForVista = result.DesktopOrLaptopForVista;
            record.SaturdayDinner = result.SaturdayDinner;
            record.PhoneNumber = result.PhoneNumber;
            record.AddressLine1 = result.AddressLine1;
            record.AllowEmailToSpeakerPlanToAttend = result.AllowEmailToSpeakerPlanToAttend;
            record.AllowEmailToSpeakerInterested = result.AllowEmailToSpeakerInterested;
            record.QREmailAllow = result.QREmailAllow;
            record.QRWebSiteAllow = result.QRWebSiteAllow;
            record.QRAddressLine1Allow = result.QRAddressLine1Allow;
            record.QRZipCodeAllow = result.QRZipCodeAllow;
            record.QRPhoneAllow = result.QRPhoneAllow;
            record.ShirtSize = result.ShirtSize;
            record.EmailSubscription = result.EmailSubscription;
            record.EmailSubscriptionStatus = result.EmailSubscriptionStatus;
            record.EmailBounces = result.EmailBounces;
            record.VolunteerMeetingStatus = result.VolunteerMeetingStatus;
            record.VolunteerMeetingInterestDate = result.VolunteerMeetingInterestDate;
            record.TwitterHandle = result.TwitterHandle;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override Attendees GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.Attendees where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<AttendeesResult> GetBaseResultIQueryable(IQueryable<Attendees> baseQuery)
        {
      IQueryable<AttendeesResult> results = (from myData in baseQuery orderby myData.Id select new AttendeesResult { Id= myData.Id,
            PKID = myData.PKID,
            Username = myData.Username,
            ApplicationName = myData.ApplicationName,
            Email = myData.Email,
            EmailEventBoard = myData.EmailEventBoard,
            Comment = myData.Comment,
            Password = myData.Password,
            PasswordQuestion = myData.PasswordQuestion,
            PasswordAnswer = myData.PasswordAnswer,
            IsApproved = myData.IsApproved,
            LastActivityDate = myData.LastActivityDate == null ? null :  (DateTime?) new DateTime(myData.LastActivityDate.Value.Ticks,DateTimeKind.Utc),
            LastLoginDate = myData.LastLoginDate == null ? null :  (DateTime?) new DateTime(myData.LastLoginDate.Value.Ticks,DateTimeKind.Utc),
            CreationDate = myData.CreationDate == null ? null :  (DateTime?) new DateTime(myData.CreationDate.Value.Ticks,DateTimeKind.Utc),
            IsOnLine = myData.IsOnLine,
            IsLockedOut = myData.IsLockedOut,
            LastLockedOutDate = myData.LastLockedOutDate == null ? null :  (DateTime?) new DateTime(myData.LastLockedOutDate.Value.Ticks,DateTimeKind.Utc),
            FailedPasswordAttemptCount = myData.FailedPasswordAttemptCount,
            FailedPasswordAttemptWindowStart = myData.FailedPasswordAttemptWindowStart == null ? null :  (DateTime?) new DateTime(myData.FailedPasswordAttemptWindowStart.Value.Ticks,DateTimeKind.Utc),
            FailedPasswordAnswerAttemptCount = myData.FailedPasswordAnswerAttemptCount,
            FailedPasswordAnswerAttemptWindowStart = myData.FailedPasswordAnswerAttemptWindowStart == null ? null :  (DateTime?) new DateTime(myData.FailedPasswordAnswerAttemptWindowStart.Value.Ticks,DateTimeKind.Utc),
            LastPasswordChangedDate = myData.LastPasswordChangedDate == null ? null :  (DateTime?) new DateTime(myData.LastPasswordChangedDate.Value.Ticks,DateTimeKind.Utc),
            UserWebsite = myData.UserWebsite,
            UserLocation = myData.UserLocation,
            UserImage = myData.UserImage,
            UserFirstName = myData.UserFirstName,
            UserLastName = myData.UserLastName,
            UserZipCode = myData.UserZipCode,
            UserBio = myData.UserBio,
            UserShareInfo = myData.UserShareInfo,
            ReferralGUID = myData.ReferralGUID,
            ConfirmedDate = myData.ConfirmedDate == null ? null :  (DateTime?) new DateTime(myData.ConfirmedDate.Value.Ticks,DateTimeKind.Utc),
            VistaSlotsId = myData.VistaSlotsId,
            FullNameUsernameZipcode = myData.FullNameUsernameZipcode,
            VistaOnly = myData.VistaOnly,
            SaturdayClasses = myData.SaturdayClasses,
            SundayClasses = myData.SundayClasses,
            DesktopOrLaptopForVista = myData.DesktopOrLaptopForVista,
            SaturdayDinner = myData.SaturdayDinner,
            PhoneNumber = myData.PhoneNumber,
            AddressLine1 = myData.AddressLine1,
            AllowEmailToSpeakerPlanToAttend = myData.AllowEmailToSpeakerPlanToAttend,
            AllowEmailToSpeakerInterested = myData.AllowEmailToSpeakerInterested,
            QREmailAllow = myData.QREmailAllow,
            QRWebSiteAllow = myData.QRWebSiteAllow,
            QRAddressLine1Allow = myData.QRAddressLine1Allow,
            QRZipCodeAllow = myData.QRZipCodeAllow,
            QRPhoneAllow = myData.QRPhoneAllow,
            ShirtSize = myData.ShirtSize,
            EmailSubscription = myData.EmailSubscription,
            EmailSubscriptionStatus = myData.EmailSubscriptionStatus,
            EmailBounces = myData.EmailBounces,
            VolunteerMeetingStatus = myData.VolunteerMeetingStatus,
            VolunteerMeetingInterestDate = myData.VolunteerMeetingInterestDate == null ? null :  (DateTime?) new DateTime(myData.VolunteerMeetingInterestDate.Value.Ticks,DateTimeKind.Utc),
            TwitterHandle = myData.TwitterHandle
      });
		    return results;
        }
        
        public List<AttendeesResult> GetJustBaseTableColumns(AttendeesQuery query)
        {
            foreach (var info in typeof (AttendeesQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: Attendees QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<Attendees> baseQuery = from myData in meta.Attendees select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<AttendeesResult> results = (from myData in baseQuery orderby myData.Id select new AttendeesResult { Id= myData.Id,
                        PKID = myData.PKID,
                        Username = myData.Username,
                        ApplicationName = myData.ApplicationName,
                        Email = myData.Email,
                        EmailEventBoard = myData.EmailEventBoard,
                        Comment = myData.Comment,
                        Password = myData.Password,
                        PasswordQuestion = myData.PasswordQuestion,
                        PasswordAnswer = myData.PasswordAnswer,
                        IsApproved = myData.IsApproved,
                        LastActivityDate = myData.LastActivityDate == null ? null :  (DateTime?) new DateTime(myData.LastActivityDate.Value.Ticks,DateTimeKind.Utc),
                        LastLoginDate = myData.LastLoginDate == null ? null :  (DateTime?) new DateTime(myData.LastLoginDate.Value.Ticks,DateTimeKind.Utc),
                        CreationDate = myData.CreationDate == null ? null :  (DateTime?) new DateTime(myData.CreationDate.Value.Ticks,DateTimeKind.Utc),
                        IsOnLine = myData.IsOnLine,
                        IsLockedOut = myData.IsLockedOut,
                        LastLockedOutDate = myData.LastLockedOutDate == null ? null :  (DateTime?) new DateTime(myData.LastLockedOutDate.Value.Ticks,DateTimeKind.Utc),
                        FailedPasswordAttemptCount = myData.FailedPasswordAttemptCount,
                        FailedPasswordAttemptWindowStart = myData.FailedPasswordAttemptWindowStart == null ? null :  (DateTime?) new DateTime(myData.FailedPasswordAttemptWindowStart.Value.Ticks,DateTimeKind.Utc),
                        FailedPasswordAnswerAttemptCount = myData.FailedPasswordAnswerAttemptCount,
                        FailedPasswordAnswerAttemptWindowStart = myData.FailedPasswordAnswerAttemptWindowStart == null ? null :  (DateTime?) new DateTime(myData.FailedPasswordAnswerAttemptWindowStart.Value.Ticks,DateTimeKind.Utc),
                        LastPasswordChangedDate = myData.LastPasswordChangedDate == null ? null :  (DateTime?) new DateTime(myData.LastPasswordChangedDate.Value.Ticks,DateTimeKind.Utc),
                        UserWebsite = myData.UserWebsite,
                        UserLocation = myData.UserLocation,
                        UserImage = myData.UserImage,
                        UserFirstName = myData.UserFirstName,
                        UserLastName = myData.UserLastName,
                        UserZipCode = myData.UserZipCode,
                        UserBio = myData.UserBio,
                        UserShareInfo = myData.UserShareInfo,
                        ReferralGUID = myData.ReferralGUID,
                        ConfirmedDate = myData.ConfirmedDate == null ? null :  (DateTime?) new DateTime(myData.ConfirmedDate.Value.Ticks,DateTimeKind.Utc),
                        VistaSlotsId = myData.VistaSlotsId,
                        FullNameUsernameZipcode = myData.FullNameUsernameZipcode,
                        VistaOnly = myData.VistaOnly,
                        SaturdayClasses = myData.SaturdayClasses,
                        SundayClasses = myData.SundayClasses,
                        DesktopOrLaptopForVista = myData.DesktopOrLaptopForVista,
                        SaturdayDinner = myData.SaturdayDinner,
                        PhoneNumber = myData.PhoneNumber,
                        AddressLine1 = myData.AddressLine1,
                        AllowEmailToSpeakerPlanToAttend = myData.AllowEmailToSpeakerPlanToAttend,
                        AllowEmailToSpeakerInterested = myData.AllowEmailToSpeakerInterested,
                        QREmailAllow = myData.QREmailAllow,
                        QRWebSiteAllow = myData.QRWebSiteAllow,
                        QRAddressLine1Allow = myData.QRAddressLine1Allow,
                        QRZipCodeAllow = myData.QRZipCodeAllow,
                        QRPhoneAllow = myData.QRPhoneAllow,
                        ShirtSize = myData.ShirtSize,
                        EmailSubscription = myData.EmailSubscription,
                        EmailSubscriptionStatus = myData.EmailSubscriptionStatus,
                        EmailBounces = myData.EmailBounces,
                        VolunteerMeetingStatus = myData.VolunteerMeetingStatus,
                        VolunteerMeetingInterestDate = myData.VolunteerMeetingInterestDate == null ? null :  (DateTime?) new DateTime(myData.VolunteerMeetingInterestDate.Value.Ticks,DateTimeKind.Utc),
                        TwitterHandle = myData.TwitterHandle
            });
            
            List<AttendeesResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<Attendees> BaseQueryAutoGen(IQueryable<Attendees> baseQuery, AttendeesQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.Username != null) baseQuery = baseQuery.Where(a => a.Username.ToLower().Equals(query.Username.ToLower()));
            if (query.ApplicationName != null) baseQuery = baseQuery.Where(a => a.ApplicationName.ToLower().Equals(query.ApplicationName.ToLower()));
            if (query.Email != null) baseQuery = baseQuery.Where(a => a.Email.ToLower().Equals(query.Email.ToLower()));
            if (query.EmailEventBoard != null) baseQuery = baseQuery.Where(a => a.EmailEventBoard.ToLower().Equals(query.EmailEventBoard.ToLower()));
            if (query.Comment != null) baseQuery = baseQuery.Where(a => a.Comment.ToLower().Equals(query.Comment.ToLower()));
            if (query.Password != null) baseQuery = baseQuery.Where(a => a.Password.ToLower().Equals(query.Password.ToLower()));
            if (query.PasswordQuestion != null) baseQuery = baseQuery.Where(a => a.PasswordQuestion.ToLower().Equals(query.PasswordQuestion.ToLower()));
            if (query.PasswordAnswer != null) baseQuery = baseQuery.Where(a => a.PasswordAnswer.ToLower().Equals(query.PasswordAnswer.ToLower()));
            if (query.IsApproved != null) baseQuery = baseQuery.Where(a => a.IsApproved == query.IsApproved);
            if (query.LastActivityDate != null) baseQuery = baseQuery.Where(a => a.LastActivityDate.Value.CompareTo(query.LastActivityDate.Value) == 0);
            if (query.LastLoginDate != null) baseQuery = baseQuery.Where(a => a.LastLoginDate.Value.CompareTo(query.LastLoginDate.Value) == 0);
            if (query.CreationDate != null) baseQuery = baseQuery.Where(a => a.CreationDate.Value.CompareTo(query.CreationDate.Value) == 0);
            if (query.IsOnLine != null) baseQuery = baseQuery.Where(a => a.IsOnLine == query.IsOnLine);
            if (query.IsLockedOut != null) baseQuery = baseQuery.Where(a => a.IsLockedOut == query.IsLockedOut);
            if (query.LastLockedOutDate != null) baseQuery = baseQuery.Where(a => a.LastLockedOutDate.Value.CompareTo(query.LastLockedOutDate.Value) == 0);
            if (query.FailedPasswordAttemptCount != null) baseQuery = baseQuery.Where(a => a.FailedPasswordAttemptCount == query.FailedPasswordAttemptCount);
            if (query.FailedPasswordAttemptWindowStart != null) baseQuery = baseQuery.Where(a => a.FailedPasswordAttemptWindowStart.Value.CompareTo(query.FailedPasswordAttemptWindowStart.Value) == 0);
            if (query.FailedPasswordAnswerAttemptCount != null) baseQuery = baseQuery.Where(a => a.FailedPasswordAnswerAttemptCount == query.FailedPasswordAnswerAttemptCount);
            if (query.FailedPasswordAnswerAttemptWindowStart != null) baseQuery = baseQuery.Where(a => a.FailedPasswordAnswerAttemptWindowStart.Value.CompareTo(query.FailedPasswordAnswerAttemptWindowStart.Value) == 0);
            if (query.LastPasswordChangedDate != null) baseQuery = baseQuery.Where(a => a.LastPasswordChangedDate.Value.CompareTo(query.LastPasswordChangedDate.Value) == 0);
            if (query.UserWebsite != null) baseQuery = baseQuery.Where(a => a.UserWebsite.ToLower().Equals(query.UserWebsite.ToLower()));
            if (query.UserLocation != null) baseQuery = baseQuery.Where(a => a.UserLocation.ToLower().Equals(query.UserLocation.ToLower()));
            if (query.UserFirstName != null) baseQuery = baseQuery.Where(a => a.UserFirstName.ToLower().Equals(query.UserFirstName.ToLower()));
            if (query.UserLastName != null) baseQuery = baseQuery.Where(a => a.UserLastName.ToLower().Equals(query.UserLastName.ToLower()));
            if (query.UserZipCode != null) baseQuery = baseQuery.Where(a => a.UserZipCode.ToLower().Equals(query.UserZipCode.ToLower()));
            if (query.UserBio != null) baseQuery = baseQuery.Where(a => a.UserBio.ToLower().Equals(query.UserBio.ToLower()));
            if (query.UserShareInfo != null) baseQuery = baseQuery.Where(a => a.UserShareInfo == query.UserShareInfo);
            if (query.ConfirmedDate != null) baseQuery = baseQuery.Where(a => a.ConfirmedDate.Value.CompareTo(query.ConfirmedDate.Value) == 0);
            if (query.VistaSlotsId != null) baseQuery = baseQuery.Where(a => a.VistaSlotsId == query.VistaSlotsId);
            if (query.FullNameUsernameZipcode != null) baseQuery = baseQuery.Where(a => a.FullNameUsernameZipcode.ToLower().Equals(query.FullNameUsernameZipcode.ToLower()));
            if (query.VistaOnly != null) baseQuery = baseQuery.Where(a => a.VistaOnly == query.VistaOnly);
            if (query.SaturdayClasses != null) baseQuery = baseQuery.Where(a => a.SaturdayClasses == query.SaturdayClasses);
            if (query.SundayClasses != null) baseQuery = baseQuery.Where(a => a.SundayClasses == query.SundayClasses);
            if (query.DesktopOrLaptopForVista != null) baseQuery = baseQuery.Where(a => a.DesktopOrLaptopForVista.ToLower().Equals(query.DesktopOrLaptopForVista.ToLower()));
            if (query.SaturdayDinner != null) baseQuery = baseQuery.Where(a => a.SaturdayDinner == query.SaturdayDinner);
            if (query.PhoneNumber != null) baseQuery = baseQuery.Where(a => a.PhoneNumber.ToLower().Equals(query.PhoneNumber.ToLower()));
            if (query.AddressLine1 != null) baseQuery = baseQuery.Where(a => a.AddressLine1.ToLower().Equals(query.AddressLine1.ToLower()));
            if (query.AllowEmailToSpeakerPlanToAttend != null) baseQuery = baseQuery.Where(a => a.AllowEmailToSpeakerPlanToAttend == query.AllowEmailToSpeakerPlanToAttend);
            if (query.AllowEmailToSpeakerInterested != null) baseQuery = baseQuery.Where(a => a.AllowEmailToSpeakerInterested == query.AllowEmailToSpeakerInterested);
            if (query.QREmailAllow != null) baseQuery = baseQuery.Where(a => a.QREmailAllow == query.QREmailAllow);
            if (query.QRWebSiteAllow != null) baseQuery = baseQuery.Where(a => a.QRWebSiteAllow == query.QRWebSiteAllow);
            if (query.QRAddressLine1Allow != null) baseQuery = baseQuery.Where(a => a.QRAddressLine1Allow == query.QRAddressLine1Allow);
            if (query.QRZipCodeAllow != null) baseQuery = baseQuery.Where(a => a.QRZipCodeAllow == query.QRZipCodeAllow);
            if (query.QRPhoneAllow != null) baseQuery = baseQuery.Where(a => a.QRPhoneAllow == query.QRPhoneAllow);
            if (query.ShirtSize != null) baseQuery = baseQuery.Where(a => a.ShirtSize.ToLower().Equals(query.ShirtSize.ToLower()));
            if (query.EmailSubscription != null) baseQuery = baseQuery.Where(a => a.EmailSubscription == query.EmailSubscription);
            if (query.EmailSubscriptionStatus != null) baseQuery = baseQuery.Where(a => a.EmailSubscriptionStatus.ToLower().Equals(query.EmailSubscriptionStatus.ToLower()));
            if (query.EmailBounces != null) baseQuery = baseQuery.Where(a => a.EmailBounces == query.EmailBounces);
            if (query.VolunteerMeetingStatus != null) baseQuery = baseQuery.Where(a => a.VolunteerMeetingStatus == query.VolunteerMeetingStatus);
            if (query.VolunteerMeetingInterestDate != null) baseQuery = baseQuery.Where(a => a.VolunteerMeetingInterestDate.Value.CompareTo(query.VolunteerMeetingInterestDate.Value) == 0);
            if (query.TwitterHandle != null) baseQuery = baseQuery.Where(a => a.TwitterHandle.ToLower().Equals(query.TwitterHandle.ToLower()));

            return baseQuery;
        }
        
    }
}
