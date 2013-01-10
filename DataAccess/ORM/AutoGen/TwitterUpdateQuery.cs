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
    public partial class TwitterUpdateQuery : QueryBase
    {
        //   Retrieving by one Id or a list of Id's is always supported.
        [AutoGenColumn]
        public int? Id { get; set; }
        [AutoGenColumn]
        public List<int> Ids { get; set; }
        // 
        //  Generate query for all columns in table
        [AutoGenColumn]
        public string Title { get; set; }
        [AutoGenColumn]
        public DateTime? Published { get; set; }
        [AutoGenColumn]
        public string AuthorImageUrl { get; set; }
        [AutoGenColumn]
        public string AuthorUrl { get; set; }
        [AutoGenColumn]
        public string AuthorName { get; set; }
        [AutoGenColumn]
        public string AuthorHandle { get; set; }
        [AutoGenColumn]
        public string ContentTweet { get; set; }
        [AutoGenColumn]
        public string AlternateTweet { get; set; }
        [AutoGenColumn]
        public string CodeCampSpeakerUrl { get; set; }
        [AutoGenColumn]
        public string CodeCampSessionsUrl { get; set; }
        [AutoGenColumn]
        public bool? TweetNotRelevant { get; set; }
        [AutoGenColumn]
        public DateTime? TweetInserted { get; set; }
        [AutoGenColumn]
        public DateTime? TweetUpdate { get; set; }
    }
}
