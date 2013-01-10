//  This is the Class that is used by the Manager class for data operations.
//  C 3PLogic, Inc.

using System;
using System.Data.SqlTypes;

namespace CodeCampSV
{
    public partial class SessionsResult : ResultBase
    {
        public int? SessionLevel_id { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? Approved { get; set; }
        public DateTime? Createdate { get; set; }
        public DateTime? Updatedate { get; set; }
        public string AdminComments { get; set; }
        public bool? InterentAccessRequired { get; set; }
        public int? LectureRoomsId { get; set; }
        public int? SessionTimesId { get; set; }
        
        //  
        //  Do not put Id here since it is in ResultBase already
        //  
        //  Might include other classes here such as:
        //  public List<PhoneResult> AssociatedPhoneResult { get; set; }
        //  or
        //  public List<SegmentResult> Segments { get; set; }
    }
}
