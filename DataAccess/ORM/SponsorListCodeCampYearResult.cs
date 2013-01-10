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
    public partial class SponsorListCodeCampYearResult
    {
        [DataMember] 
        public string SponsorName { get; set; }
        [DataMember]
        public string Comment { get; set; }

        [DataMember] 
        public int CurrentCodeCampYearId { get; set; }

    }
}
