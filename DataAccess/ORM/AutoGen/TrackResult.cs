//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CodeCampSV
{
    public partial class TrackResult : ResultBase
    {
        [DataMember] public int? CodeCampYearId { get; set; }
        [DataMember] public int? OwnerAttendeeId { get; set; }
        [DataMember] public string Named { get; set; }
        [DataMember] public string Description { get; set; }
        [DataMember] public bool? Visible { get; set; }
        [DataMember] public string AlternateURL { get; set; }
        [DataMember][XmlIgnore()] public System.Data.Linq.Binary TrackImage { get; set; }
        [DataMember] public DateTime? CreationDate { get; set; }
        [DataMember] public DateTime? ModifiedDate { get; set; }
        
        //  
        //  Do not put Id here since it is in ResultBase already
        //  
        //  Might include other classes here such as:
        //  public List<PhoneResult> AssociatedPhoneResult { get; set; }
        //  or
        //  public List<SegmentResult> Segments { get; set; }
    }
}
