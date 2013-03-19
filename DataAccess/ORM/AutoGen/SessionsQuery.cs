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
        [AutoGenColumn]
        public int? Id { get; set; }
        [AutoGenColumn]
        public List<int> Ids { get; set; }
        // 
        //  Generate query for all columns in table
        [AutoGenColumn]
        public int? CodeCampYearId { get; set; }
        [AutoGenColumn]
        public int? SessionLevel_id { get; set; }
        [AutoGenColumn]
        public int? SponsorId { get; set; }
        [AutoGenColumn]
        public string Username { get; set; }
        [AutoGenColumn]
        public string Title { get; set; }
        [AutoGenColumn]
        public string Description { get; set; }
        [AutoGenColumn]
        public bool? Approved { get; set; }
        [AutoGenColumn]
        public DateTime? Createdate { get; set; }
        [AutoGenColumn]
        public DateTime? Updatedate { get; set; }
        [AutoGenColumn]
        public string AdminComments { get; set; }
        [AutoGenColumn]
        public bool? DoNotShowPrimarySpeaker { get; set; }
        [AutoGenColumn]
        public bool? InterentAccessRequired { get; set; }
        [AutoGenColumn]
        public int? LectureRoomsId { get; set; }
        [AutoGenColumn]
        public int? SessionTimesId { get; set; }
        [AutoGenColumn]
        public string TweetLine { get; set; }
        [AutoGenColumn]
        public DateTime? TweetLineTweetedDate { get; set; }
        [AutoGenColumn]
        public bool? TweetLineTweeted { get; set; }
        [AutoGenColumn]
        public string SessionsMaterialUrl { get; set; }
        [AutoGenColumn]
        public bool? SessionsMaterialQueueToSend { get; set; }
        [AutoGenColumn]
        public DateTime? SessionMaterialUrlDateSent { get; set; }
        [AutoGenColumn]
        public string SessionMaterialUrlMessage { get; set; }
        [AutoGenColumn]
        public string TwitterHashTags { get; set; }
        [AutoGenColumn]
        public string TweetLinePreCamp { get; set; }
        [AutoGenColumn]
        public DateTime? TweetLineTweetedDatePreCamp { get; set; }
        [AutoGenColumn]
        public bool? TweetLineTweetedPreCamp { get; set; }
        [AutoGenColumn]
        public string ShortUrl { get; set; }
        [AutoGenColumn]
        public string BoxFolderIdString { get; set; }
        [AutoGenColumn]
        public string BoxFolderEmailInAddress { get; set; }
        [AutoGenColumn]
        public string BoxFolderPublicUrl { get; set; }
        [AutoGenColumn]
        public string OptInTechJobKeyWords { get; set; }
    }
}
