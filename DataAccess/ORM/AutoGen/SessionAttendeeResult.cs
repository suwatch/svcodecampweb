//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;

namespace CodeCampSV
{
    public partial class SessionAttendeeResult : ResultBase
    {
        [DataMember] public int Sessions_id { get; set; }
        [DataMember] public Guid Attendees_username { get; set; }
        [DataMember] public int Interestlevel { get; set; }
        [DataMember] public DateTime? LastUpdatedDate { get; set; }
        [DataMember] public string UpdateByProgram { get; set; }
        
        //  
        //  Do not put Id here since it is in ResultBase already
        //  
        //  Might include other classes here such as:
        //  public List<PhoneResult> AssociatedPhoneResult { get; set; }
        //  or
        //  public List<SegmentResult> Segments { get; set; }
    }
}
