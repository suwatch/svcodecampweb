//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;

namespace CodeCampSV
{
    public partial class SessionPicturesResult : ResultBase
    {
        [DataMember] public int PictureId { get; set; }
        [DataMember] public int SessionId { get; set; }
        [DataMember] public Guid AttendeePKID { get; set; }
        [DataMember] public DateTime? AssignedDate { get; set; }
        [DataMember] public string Description { get; set; }
        [DataMember] public bool? DefaultPicture { get; set; }
        
        //  
        //  Do not put Id here since it is in ResultBase already
        //  
        //  Might include other classes here such as:
        //  public List<PhoneResult> AssociatedPhoneResult { get; set; }
        //  or
        //  public List<SegmentResult> Segments { get; set; }
    }
}
