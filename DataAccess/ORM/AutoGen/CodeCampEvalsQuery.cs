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
    public partial class CodeCampEvalsQuery : QueryBase
    {
        //   Retrieving by one Id or a list of Id's is always supported.
        [AutoGenColumn]
        public int? Id { get; set; }
        [AutoGenColumn]
        public List<int> Ids { get; set; }
        // 
        //  Generate query for all columns in table
        [AutoGenColumn]
        public Guid? AttendeePKID { get; set; }
        [AutoGenColumn]
        public int? CodeCampYearId { get; set; }
        [AutoGenColumn]
        public int? MetExpectations { get; set; }
        [AutoGenColumn]
        public int? PlanToAttendAgain { get; set; }
        [AutoGenColumn]
        public int? EnjoyedFreeFood { get; set; }
        [AutoGenColumn]
        public int? SessionsVariedEnough { get; set; }
        [AutoGenColumn]
        public int? EnoughSessionsAtMyLevel { get; set; }
        [AutoGenColumn]
        public int? FoothillGoodVenue { get; set; }
        [AutoGenColumn]
        public int? WishToldMoreFriends { get; set; }
        [AutoGenColumn]
        public int? EventWellPlanned { get; set; }
        [AutoGenColumn]
        public int? WirelessAccessImportant { get; set; }
        [AutoGenColumn]
        public int? WiredAccessImportant { get; set; }
        [AutoGenColumn]
        public int? LikeReceivingUpdateByEmail { get; set; }
        [AutoGenColumn]
        public int? LikeReceivingUpdateByByRSSFeed { get; set; }
        [AutoGenColumn]
        public int? RatherNoSponsorAndNoFreeFood { get; set; }
        [AutoGenColumn]
        public bool? AttendedVistaFairOnly { get; set; }
        [AutoGenColumn]
        public bool? AttendedVistaFairAndCC { get; set; }
        [AutoGenColumn]
        public bool? AttendedCCOnly { get; set; }
        [AutoGenColumn]
        public string BestPartOfEvent { get; set; }
        [AutoGenColumn]
        public string WhatWouldYouChange { get; set; }
        [AutoGenColumn]
        public string NotSatisfiedWhy { get; set; }
        [AutoGenColumn]
        public string WhatFoothillClassesToAdd { get; set; }
        [AutoGenColumn]
        public bool? InteresteInLongTermPlanning { get; set; }
        [AutoGenColumn]
        public bool? InteresteInWebBackEnd { get; set; }
        [AutoGenColumn]
        public bool? InterestedInWebFrontEnd { get; set; }
        [AutoGenColumn]
        public bool? InteresteInLongSessionReviewPanel { get; set; }
        [AutoGenColumn]
        public bool? InteresteInContributorSolicitation { get; set; }
        [AutoGenColumn]
        public bool? InteresteInBeingContributor { get; set; }
        [AutoGenColumn]
        public bool? InteresteInBeforeEvent { get; set; }
        [AutoGenColumn]
        public bool? InteresteInDayOfEvent { get; set; }
        [AutoGenColumn]
        public bool? InteresteInEventTearDown { get; set; }
        [AutoGenColumn]
        public bool? InteresteInAfterEvent { get; set; }
        [AutoGenColumn]
        public string ForVolunteeringBestWayToContactEmail { get; set; }
        [AutoGenColumn]
        public string ForVolunteeringBestWayToContactPhone { get; set; }
        [AutoGenColumn]
        public DateTime? DateSubmitted { get; set; }
        [AutoGenColumn]
        public DateTime? DateUpdated { get; set; }
    }
}
