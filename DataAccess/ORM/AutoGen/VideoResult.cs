//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;

namespace CodeCampSV
{
    public partial class VideoResult : ResultBase
    {
        [DataMember] public string YouTubeURL { get; set; }
        [DataMember] public string DescriptionText { get; set; }
        [DataMember] public System.Data.Linq.Binary PictureBytes { get; set; }
        [DataMember] public DateTime? CreatedDate { get; set; }
        
        //  
        //  Do not put Id here since it is in ResultBase already
        //  
        //  Might include other classes here such as:
        //  public List<PhoneResult> AssociatedPhoneResult { get; set; }
        //  or
        //  public List<SegmentResult> Segments { get; set; }
    }
}
