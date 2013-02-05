//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CodeCampSV
{
    public partial class VolunteerJobResult : ResultBase
    {
        [DataMember] public int CodeCampYearId { get; set; }
        [DataMember] public string Description { get; set; }
        [DataMember] public DateTime JobStartTime { get; set; }
        [DataMember] public DateTime JobEndTime { get; set; }
        [DataMember] public int NumberNeeded { get; set; }
        
        //  
        //  Do not put Id here since it is in ResultBase already
        //  
        //  Might include other classes here such as:
        //  public List<PhoneResult> AssociatedPhoneResult { get; set; }
        //  or
        //  public List<SegmentResult> Segments { get; set; }
    }
}
