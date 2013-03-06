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
    public partial class SponsorListResult
    {
        public int SponsorSupportLevelOrder { get; set; }
        public string SponsorSupportLevel { get; set; }

        public bool? HasCurrentCodeCampYear { get; set; } // Used to determine if this sponsor has current code camp year sponsored

        public bool? ShowQuestionMark { get; set; }
        public bool? ShowFeatured { get; set; }

        public List<SponsorListCodeCampYearResult> SponsorListCodeCampYearResults { get; set; }

        public List<SponsorListContactResult> SponsorListContactResults { get; set; }

        public List<SponsorListNoteResult> SponsorListNoteResults { get; set; }

        public List<SponsorListJobListingResult> SponsorListJobListingResults { get; set; }



    }
}
