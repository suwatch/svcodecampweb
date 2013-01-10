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
    public partial class SponsorListNoteQuery : QueryBase
    {
        //   Retrieving by one Id or a list of Id's is always supported.
        [AutoGenColumn]
        public int? Id { get; set; }
        [AutoGenColumn]
        public List<int> Ids { get; set; }
        // 
        //  Generate query for all columns in table
        [AutoGenColumn]
        public int? SponsorListId { get; set; }
        [AutoGenColumn]
        public DateTime? TimeStampOfNote { get; set; }
        [AutoGenColumn]
        public string NoteAuthor { get; set; }
        [AutoGenColumn]
        public string Note { get; set; }
    }
}
