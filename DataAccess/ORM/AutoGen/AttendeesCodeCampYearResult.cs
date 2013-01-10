//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;

namespace CodeCampSV
{
    public partial class AttendeesCodeCampYearResult : ResultBase
    {
        [DataMember] public int AttendeesId { get; set; }
        [DataMember] public int CodeCampYearId { get; set; }
        [DataMember] public bool AttendSaturday { get; set; }
        [DataMember] public bool AttendSunday { get; set; }
        [DataMember] public bool? Volunteer { get; set; }
        [DataMember] public DateTime? CreateDate { get; set; }
        
        //  
        //  Do not put Id here since it is in ResultBase already
        //  
        //  Might include other classes here such as:
        //  public List<PhoneResult> AssociatedPhoneResult { get; set; }
        //  or
        //  public List<SegmentResult> Segments { get; set; }
    }
}
