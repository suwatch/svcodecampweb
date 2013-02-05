//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CodeCampSV
{
    public partial class SessionTimesResult : ResultBase
    {
        [DataMember] public DateTime? StartTime { get; set; }
        [DataMember] public string StartTimeFriendly { get; set; }
        [DataMember] public DateTime? EndTime { get; set; }
        [DataMember] public string EndTimeFriendly { get; set; }
        [DataMember] public int? SessionMinutes { get; set; }
        [DataMember] public string Description { get; set; }
        [DataMember] public string TitleBeforeOnAgenda { get; set; }
        [DataMember] public string TitleAfterOnAgenda { get; set; }
        [DataMember] public int? CodeCampYearId { get; set; }
        
        //  
        //  Do not put Id here since it is in ResultBase already
        //  
        //  Might include other classes here such as:
        //  public List<PhoneResult> AssociatedPhoneResult { get; set; }
        //  or
        //  public List<SegmentResult> Segments { get; set; }
    }
}
