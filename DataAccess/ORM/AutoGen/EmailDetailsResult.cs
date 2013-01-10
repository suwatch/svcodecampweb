//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;

namespace CodeCampSV
{
    public partial class EmailDetailsResult : ResultBase
    {
        [DataMember] public int AttendeesId { get; set; }
        [DataMember] public string MessageUniqueId { get; set; }
        [DataMember] public string EmailSendStatus { get; set; }
        [DataMember] public DateTime? EmailSendStartTime { get; set; }
        [DataMember] public DateTime? EmailSendFinishTime { get; set; }
        [DataMember] public string EmailSendLogMessage { get; set; }
        [DataMember] public string Subject { get; set; }
        [DataMember] public string BodyText { get; set; }
        [DataMember] public DateTime? SentDateTime { get; set; }
        [DataMember] public string EmailFrom { get; set; }
        [DataMember] public string EmailTo { get; set; }
        
        //  
        //  Do not put Id here since it is in ResultBase already
        //  
        //  Might include other classes here such as:
        //  public List<PhoneResult> AssociatedPhoneResult { get; set; }
        //  or
        //  public List<SegmentResult> Segments { get; set; }
    }
}
