//  This file is meant as a starting point only.  it should be copied into working source tree only if there is not
//  an existing file with this name in it already.
//  C 3PLogic, Inc.
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace CodeCampSV
{
    [Serializable]
    public partial class SponsorListJobListingResult
    {
        //public List<int> SessionIds { get; set; } 
        public string JobDateFriendly { get; set; }
    }
}
