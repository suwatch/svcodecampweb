//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CodeCampSV
{
    public partial class SponsorRequestResult : ResultBase
    {
        [DataMember] public string ContactEmail { get; set; }
        [DataMember] public string Company { get; set; }
        [DataMember] public string PhoneNumber { get; set; }
        [DataMember] public bool? EmailMe { get; set; }
        [DataMember] public bool? ContactMeByPhone { get; set; }
        [DataMember] public bool? AlsoAttending { get; set; }
        [DataMember] public bool? PastSponsor { get; set; }
        [DataMember] public string SponsorSpecialNotes { get; set; }
        [DataMember] public string SvccNotes { get; set; }
        [DataMember] public bool? SvccRespondedTo { get; set; }
        [DataMember] public bool? SvccEnteredInSystem { get; set; }
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
