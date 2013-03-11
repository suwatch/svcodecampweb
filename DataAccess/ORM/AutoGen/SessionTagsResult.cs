//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CodeCampSV
{
    public partial class SessionTagsResult : ResultBase
    {
        [DataMember] public int TagId { get; set; }
        [DataMember] public int? SessionId { get; set; }
    }
}
