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
    public partial class PicturesQuery : QueryBase
    {
        //   Retrieving by one Id or a list of Id's is always supported.
        [AutoGenColumn]
        public int? Id { get; set; }
        [AutoGenColumn]
        public List<int> Ids { get; set; }
        // 
        //  Generate query for all columns in table
        [AutoGenColumn]
        public Guid? AttendeePKID { get; set; }
        [AutoGenColumn]
        public DateTime? DateCreated { get; set; }
        [AutoGenColumn]
        public DateTime? DateUpdated { get; set; }
        [AutoGenColumn]
        public System.Data.Linq.Binary PictureBytes { get; set; }
        [AutoGenColumn]
        public string FileName { get; set; }
        [AutoGenColumn]
        public string Description { get; set; }
    }
}
