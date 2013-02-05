//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CodeCampSV
{
    public partial class SponsorListContactResult : ResultBase
    {
        [DataMember] public int? SponsorListId { get; set; }
        [DataMember] public int? SponsorListContactTypeId { get; set; }
        [DataMember] public string ContactType { get; set; }
        [DataMember] public string EmailAddress { get; set; }
        [DataMember] public string FirstName { get; set; }
        [DataMember] public string LastName { get; set; }
        [DataMember] public string PhoneNumberOffice { get; set; }
        [DataMember] public string PhoneNumberCell { get; set; }
        [DataMember] public string Comment { get; set; }
        [DataMember] public DateTime DateCreated { get; set; }
        [DataMember] public bool? GeneralCCMailings { get; set; }
        [DataMember] public bool? SponsorCCMailings { get; set; }
        
        //  
        //  Do not put Id here since it is in ResultBase already
        //  
        //  Might include other classes here such as:
        //  public List<PhoneResult> AssociatedPhoneResult { get; set; }
        //  or
        //  public List<SegmentResult> Segments { get; set; }
    }
}
