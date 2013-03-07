//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CodeCampSV
{
    public partial class EmailDetailsTopicResult : ResultBase
    {
        [DataMember] public DateTime CreateDate { get; set; }
        [DataMember] public string Title { get; set; }
        [DataMember] public DateTime? FirstRunDate { get; set; }
        [DataMember] public string Notes { get; set; }
        [DataMember] public string EmailSubject { get; set; }
        [DataMember] public string EmailMime { get; set; }
        
        //  
        //  Do not put Id here since it is in ResultBase already
        //  
        //  Might include other classes here such as:
        //  public List<PhoneResult> AssociatedPhoneResult { get; set; }
        //  or
        //  public List<SegmentResult> Segments { get; set; }
    }
}