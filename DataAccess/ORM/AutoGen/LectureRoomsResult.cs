//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;

namespace CodeCampSV
{
    public partial class LectureRoomsResult : ResultBase
    {
        [DataMember] public string Number { get; set; }
        [DataMember] public string Description { get; set; }
        [DataMember] public string Style { get; set; }
        [DataMember] public int? Capacity { get; set; }
        [DataMember] public bool? Projector { get; set; }
        [DataMember] public bool? Screen { get; set; }
        [DataMember] public System.Data.Linq.Binary Picture { get; set; }
        [DataMember] public bool? Available { get; set; }
        
        //  
        //  Do not put Id here since it is in ResultBase already
        //  
        //  Might include other classes here such as:
        //  public List<PhoneResult> AssociatedPhoneResult { get; set; }
        //  or
        //  public List<SegmentResult> Segments { get; set; }
    }
}
