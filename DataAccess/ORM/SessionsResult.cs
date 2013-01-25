//  This file is meant as a starting point only.  it should be copied into working source tree only if there is not
//  an existing file with this name in it already.
//  C 3PLogic, Inc.
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace CodeCampSV
{
    [Serializable]
    public partial class SessionsResult
    {
        [DataMember] public string SessionSlug { get; set; }
        [DataMember] public string SessionUrl { get; set; }

        [DataMember]
        public string DescriptionEllipsized { get; set; }

        [DataMember] public int InterestedCount { get; set; }
        [DataMember] public int NotInterestedCount { get; set; }
        [DataMember] public int WillAttendCount { get; set; }

        [DataMember]
        public bool InterestedBool { get; set; }
        [DataMember]
        public bool NotInterestedBool { get; set; }
        [DataMember]
        public bool WillAttendBool { get; set; }

        public string WikiURL { get; set; }
        [DataMember]
        public string SessionLevel { get; set; }
        [DataMember]
        public string PresenterName { get; set; }
        [DataMember]
        public string PresenterURL { get; set; }

        [DataMember]
        public string SpeakersShort { get; set; } // either "Douglas Crockford" or "Smith,Jones,..." for multiple


        [DataMember]
        public string RoomNumber { get; set; }
        [DataMember]
        public string RoomNumberNew { get; set; }
        [DataMember]
        public string SessionTime { get; set; }
        [DataMember]
        public string TitleWithPlanAttend { get; set; }
        [DataMember]
        public string PlanAheadCount { get; set; }
        [DataMember]
        public int PlanAheadCountInt { get; set; }
        [DataMember]
        public string InterestCount { get; set; }
        [DataMember]
        public int InterestCountInt { get; set; }

        [DataMember]
        public string SessionPosted { get; set; } // something like "39 Days Ago"
        [DataMember]
        public bool LoggedInUserInterested { get; set; }
        [DataMember]
        public bool LoggedInUserPlanToAttend { get; set; }

        [DataMember]
        public string SpeakerPictureUrl { get; set; }
        [DataMember] 
        public List<TagsResult> TagsResults { get; set; }
        //[DataMember] 
        //public List<AttendeesResult> SpeakersList { get; set; }
        [DataMember] 
        public LectureRoomsResult LectureRoomsResult { get; set; }
        [DataMember] 
        public SessionTimesResult SessionTimesResult { get; set; }
        [DataMember] 
        public List<SessionEvalsResult> SessionEvalsResults { get; set; }
       
        [DataMember]
        public SessionLevelsResult SessionLevelsResult { get; set; }

        [DataMember]
        public List<SpeakerResult> SpeakersList { get; set; }


        public string TitleEllipsized { get; set; }
       
    }
}
