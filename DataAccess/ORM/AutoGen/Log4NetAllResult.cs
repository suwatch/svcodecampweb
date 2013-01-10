//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;

namespace CodeCampSV
{
    public partial class Log4NetAllResult : ResultBase
    {
        [DataMember] public DateTime Date { get; set; }
        [DataMember] public string Thread { get; set; }
        [DataMember] public string Level { get; set; }
        [DataMember] public string Logger { get; set; }
        [DataMember] public string Message { get; set; }
        [DataMember] public string ExceptionMessage { get; set; }
        [DataMember] public string ExceptionStackTrace { get; set; }
        [DataMember] public string UserName { get; set; }
        [DataMember] public int? EllapsedTime { get; set; }
        [DataMember] public string MessageLine1 { get; set; }
        [DataMember] public string MessageLine2 { get; set; }
        
        //  
        //  Do not put Id here since it is in ResultBase already
        //  
        //  Might include other classes here such as:
        //  public List<PhoneResult> AssociatedPhoneResult { get; set; }
        //  or
        //  public List<SegmentResult> Segments { get; set; }
    }
}
