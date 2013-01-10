//  This file is meant as a starting point only.  it should be copied into working source tree only if there is not
//  an existing file with this name in it already.
//  C 3PLogic, Inc.
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace CodeCampSV
{
    [Serializable]
    public partial class CodeCampYearResult
    {
        //  Put things here that may not come from the table directly
        //  For example, you may have StatusTypeId in your Result, however
        //  you want to return the StatusTypeName so you would be the
        //  following here:
        //  public string StatusTypeName { get; set; }
        // 
        //  You can also put computed type columns like
        //  public bool? IsStar { get; set; }
        // 
        //  Or even related datasets like:
        //  public List<CargoResult> Cargos { get; set; }
        // 
        [DataMember] 
        public List<SessionsResult> SessionResults { get; set; }
        [DataMember] 
        public List<AttendeesResult> SpeakerResults { get; set; }
        [DataMember] 
        public LectureRoomsResult LectureRoomsResult { get; set; }
        [DataMember] 
        public SessionTimesResult SessionTimesResult { get; set; }
        [DataMember] 
        public SessionLevelsResult SessionLevelsResult { get; set; }
        [DataMember] 
        public List<SessionEvalsResult> SessionEvalsResults { get; set; } 
    }
}
