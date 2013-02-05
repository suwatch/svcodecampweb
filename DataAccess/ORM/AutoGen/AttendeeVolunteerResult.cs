//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CodeCampSV
{
    public partial class AttendeeVolunteerResult : ResultBase
    {
        [DataMember] public int AttendeeId { get; set; }
        [DataMember] public int VolunteerJobId { get; set; }
        [DataMember] public string Notes { get; set; }
        [DataMember] public DateTime? CreatedDate { get; set; }
        [DataMember] public DateTime? LastUpdated { get; set; }
        
        //  
        //  Do not put Id here since it is in ResultBase already
        //  
        //  Might include other classes here such as:
        //  public List<PhoneResult> AssociatedPhoneResult { get; set; }
        //  or
        //  public List<SegmentResult> Segments { get; set; }
    }
}
