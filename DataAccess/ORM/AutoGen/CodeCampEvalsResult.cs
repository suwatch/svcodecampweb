//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CodeCampSV
{
    public partial class CodeCampEvalsResult : ResultBase
    {
        [DataMember] public Guid AttendeePKID { get; set; }
        [DataMember] public int? CodeCampYearId { get; set; }
        [DataMember] public int? MetExpectations { get; set; }
        [DataMember] public int? PlanToAttendAgain { get; set; }
        [DataMember] public int? EnjoyedFreeFood { get; set; }
        [DataMember] public int? SessionsVariedEnough { get; set; }
        [DataMember] public int? EnoughSessionsAtMyLevel { get; set; }
        [DataMember] public int? FoothillGoodVenue { get; set; }
        [DataMember] public int? WishToldMoreFriends { get; set; }
        [DataMember] public int? EventWellPlanned { get; set; }
        [DataMember] public int? WirelessAccessImportant { get; set; }
        [DataMember] public int? WiredAccessImportant { get; set; }
        [DataMember] public int? LikeReceivingUpdateByEmail { get; set; }
        [DataMember] public int? LikeReceivingUpdateByByRSSFeed { get; set; }
        [DataMember] public int? RatherNoSponsorAndNoFreeFood { get; set; }
        [DataMember] public bool? AttendedVistaFairOnly { get; set; }
        [DataMember] public bool? AttendedVistaFairAndCC { get; set; }
        [DataMember] public bool? AttendedCCOnly { get; set; }
        [DataMember] public string BestPartOfEvent { get; set; }
        [DataMember] public string WhatWouldYouChange { get; set; }
        [DataMember] public string NotSatisfiedWhy { get; set; }
        [DataMember] public string WhatFoothillClassesToAdd { get; set; }
        [DataMember] public bool? InteresteInLongTermPlanning { get; set; }
        [DataMember] public bool? InteresteInWebBackEnd { get; set; }
        [DataMember] public bool? InterestedInWebFrontEnd { get; set; }
        [DataMember] public bool? InteresteInLongSessionReviewPanel { get; set; }
        [DataMember] public bool? InteresteInContributorSolicitation { get; set; }
        [DataMember] public bool? InteresteInBeingContributor { get; set; }
        [DataMember] public bool? InteresteInBeforeEvent { get; set; }
        [DataMember] public bool? InteresteInDayOfEvent { get; set; }
        [DataMember] public bool? InteresteInEventTearDown { get; set; }
        [DataMember] public bool? InteresteInAfterEvent { get; set; }
        [DataMember] public string ForVolunteeringBestWayToContactEmail { get; set; }
        [DataMember] public string ForVolunteeringBestWayToContactPhone { get; set; }
        [DataMember] public DateTime? DateSubmitted { get; set; }
        [DataMember] public DateTime? DateUpdated { get; set; }
        
        //  
        //  Do not put Id here since it is in ResultBase already
        //  
        //  Might include other classes here such as:
        //  public List<PhoneResult> AssociatedPhoneResult { get; set; }
        //  or
        //  public List<SegmentResult> Segments { get; set; }
    }
}
