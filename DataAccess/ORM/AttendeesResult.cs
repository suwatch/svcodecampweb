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
    public partial class AttendeesResult
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
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember] 
        public string SpeakerPictureUrl { get; set; }
        [DataMember] 
        public List<int> SessionIds { get; set; } // does not include if marked DoNotShowPrimarySpeaker in session record

        [DataMember]
        public string UserBioEllipsized { get; set; }

    }
}
