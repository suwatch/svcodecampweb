//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;

namespace CodeCampSV
{
    public partial class SponsorListCodeCampYearResult : ResultBase
    {
        [DataMember] public int CodeCampYearId { get; set; }
        [DataMember] public int SponsorListId { get; set; }
        [DataMember] public Double DonationAmount { get; set; }
        [DataMember] public int SortIndex { get; set; }
        [DataMember] public bool Visible { get; set; }
        [DataMember] public string Status { get; set; }
        [DataMember] public DateTime? NextActionDate { get; set; }
        [DataMember] public string WhoOwns { get; set; }
        [DataMember] public bool? TableRequired { get; set; }
        [DataMember] public bool? AttendeeBagItem { get; set; }
        [DataMember] public bool? ItemSentToUPS { get; set; }
        [DataMember] public bool? ItemSentToFoothill { get; set; }
        [DataMember] public string Comments { get; set; }
        [DataMember] public string ItemsShippingDescription { get; set; }
        [DataMember] public string NoteFromCodeCamp { get; set; }
        
        //  
        //  Do not put Id here since it is in ResultBase already
        //  
        //  Might include other classes here such as:
        //  public List<PhoneResult> AssociatedPhoneResult { get; set; }
        //  or
        //  public List<SegmentResult> Segments { get; set; }
    }
}
