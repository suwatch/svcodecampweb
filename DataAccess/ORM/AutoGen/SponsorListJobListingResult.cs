//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;

namespace CodeCampSV
{
    public partial class SponsorListJobListingResult : ResultBase
    {
        [DataMember] public int SponsorListId { get; set; }
        [DataMember] public string JobName { get; set; }
        [DataMember] public string JobLocation { get; set; }
        [DataMember] public string JobURL { get; set; }
        [DataMember] public string JobBrief { get; set; }
        [DataMember] public string JobTagline { get; set; }
        [DataMember] public string JobButtonToolTip { get; set; }
        [DataMember] public DateTime EnteredDate { get; set; }
        [DataMember] public string JobCompanyName { get; set; }
        [DataMember] public DateTime? StartRunDate { get; set; }
        [DataMember] public DateTime? EndRunDate { get; set; }
        [DataMember] public bool HideListing { get; set; }
        [DataMember] public string Notes { get; set; }
        
        //  
        //  Do not put Id here since it is in ResultBase already
        //  
        //  Might include other classes here such as:
        //  public List<PhoneResult> AssociatedPhoneResult { get; set; }
        //  or
        //  public List<SegmentResult> Segments { get; set; }
    }
}
