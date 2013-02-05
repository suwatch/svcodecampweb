//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CodeCampSV
{
    public partial class SponsorListResult : ResultBase
    {
        [DataMember] public string SponsorName { get; set; }
        [DataMember] public DateTime? NextActionDate { get; set; }
        [DataMember] public string ImageURL { get; set; }
        [DataMember] public string NavigateURL { get; set; }
        [DataMember] public string HoverOverText { get; set; }
        [DataMember] public string UnderLogoText { get; set; }
        [DataMember] public string Comment { get; set; }
        [DataMember] public string CompanyDescriptionShort { get; set; }
        [DataMember] public string CompanyDescriptionLong { get; set; }
        [DataMember] public string CompanyAddressLine1 { get; set; }
        [DataMember] public string CompanyAddressLine2 { get; set; }
        [DataMember] public string CompanyCity { get; set; }
        [DataMember] public string CompanyState { get; set; }
        [DataMember] public string CompanyZip { get; set; }
        [DataMember] public string CompanyPhoneNumber { get; set; }
        [DataMember][XmlIgnore()] public System.Data.Linq.Binary CompanyImage { get; set; }
        [DataMember] public DateTime? CreationDate { get; set; }
        [DataMember] public DateTime? ModifiedDate { get; set; }
        [DataMember] public string CompanyImageType { get; set; }
        
        //  
        //  Do not put Id here since it is in ResultBase already
        //  
        //  Might include other classes here such as:
        //  public List<PhoneResult> AssociatedPhoneResult { get; set; }
        //  or
        //  public List<SegmentResult> Segments { get; set; }
    }
}
