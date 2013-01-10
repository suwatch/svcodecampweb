//   Regenerated Code
//   C 3PLogic, Inc.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlTypes;
using System.ComponentModel;


namespace CodeCampSV
{
    [Serializable]
    public partial class SessionsQuery : QueryBase
    {
        //   Retrieving by one Id or a list of Id's is always supported.
        public int? Id { get; set; }
        public List<int> Ids { get; set; }
        // 
        //  Generate query for all columns in table
        [AutoGenColumnAttribute]
        public int? SessionLevel_id { get; set; }
        [AutoGenColumnAttribute]
        public string Username { get; set; }
        [AutoGenColumnAttribute]
        public string title { get; set; }
        [AutoGenColumnAttribute]
        public string description { get; set; }
        [AutoGenColumnAttribute]
        public bool? approved { get; set; }
        [AutoGenColumnAttribute]
        public DateTime? createdate { get; set; }
        [AutoGenColumnAttribute]
        public DateTime? updatedate { get; set; }
        [AutoGenColumnAttribute]
        public string AdminComments { get; set; }
        [AutoGenColumnAttribute]
        public bool? InterentAccessRequired { get; set; }
        [AutoGenColumnAttribute]
        public int? LectureRoomsId { get; set; }
        [AutoGenColumnAttribute]
        public int? SessionTimesId { get; set; }
    }


}
