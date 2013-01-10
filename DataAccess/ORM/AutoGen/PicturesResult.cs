//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;

namespace CodeCampSV
{
    public partial class PicturesResult : ResultBase
    {
        [DataMember] public Guid? AttendeePKID { get; set; }
        [DataMember] public DateTime? DateCreated { get; set; }
        [DataMember] public DateTime? DateUpdated { get; set; }
        [DataMember] public System.Data.Linq.Binary PictureBytes { get; set; }
        [DataMember] public string FileName { get; set; }
        [DataMember] public string Description { get; set; }
        
        //  
        //  Do not put Id here since it is in ResultBase already
        //  
        //  Might include other classes here such as:
        //  public List<PhoneResult> AssociatedPhoneResult { get; set; }
        //  or
        //  public List<SegmentResult> Segments { get; set; }
    }
}
