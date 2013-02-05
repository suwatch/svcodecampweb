//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CodeCampSV
{
    public partial class CodeCampYearResult : ResultBase
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public DateTime CampStartDate { get; set; }
        [DataMember] public DateTime CampEndDate { get; set; }
        [DataMember] public bool ReadOnly { get; set; }
        [DataMember] public string CodeCampDateString { get; set; }
        [DataMember] public string CodeCampSaturdayString { get; set; }
        [DataMember] public string CodeCampSundayString { get; set; }
        
        //  
        //  Do not put Id here since it is in ResultBase already
        //  
        //  Might include other classes here such as:
        //  public List<PhoneResult> AssociatedPhoneResult { get; set; }
        //  or
        //  public List<SegmentResult> Segments { get; set; }
    }
}
