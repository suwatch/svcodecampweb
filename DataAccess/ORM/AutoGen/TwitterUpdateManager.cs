//  This is the Manager class used for data operations.  It is meant to have another Partial
//  class associated with it.
//  C 3PLogic, Inc.
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

using CodeCampSV;


namespace CodeCampSV
{
    //  Here are the 2 methods that needs to be auto genearted. 
    //  First is a one to one maping to the database columns. 
    //  Since we auto generate the results class too, we can guarantee the columns are all there
    [DataObject(true)]
    public partial class TwitterUpdateManager : ManagerBase<TwitterUpdateManager, TwitterUpdateResult, TwitterUpdate, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(TwitterUpdate record, TwitterUpdateResult result)
        {
            record.Title = result.Title;
            record.Published = result.Published;
            record.AuthorImageUrl = result.AuthorImageUrl;
            record.AuthorUrl = result.AuthorUrl;
            record.AuthorName = result.AuthorName;
            record.AuthorHandle = result.AuthorHandle;
            record.ContentTweet = result.ContentTweet;
            record.AlternateTweet = result.AlternateTweet;
            record.CodeCampSpeakerUrl = result.CodeCampSpeakerUrl;
            record.CodeCampSessionsUrl = result.CodeCampSessionsUrl;
            record.TweetNotRelevant = result.TweetNotRelevant;
            record.TweetInserted = result.TweetInserted;
            record.TweetUpdate = result.TweetUpdate;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override TwitterUpdate GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.TwitterUpdate where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<TwitterUpdateResult> GetBaseResultIQueryable(IQueryable<TwitterUpdate> baseQuery)
        {
      IQueryable<TwitterUpdateResult> results = (from myData in baseQuery orderby myData.Id select new TwitterUpdateResult { Id= myData.Id,
            Title = myData.Title,
            Published = myData.Published == null ? null :  (DateTime?) new DateTime(myData.Published.Value.Ticks,DateTimeKind.Utc),
            AuthorImageUrl = myData.AuthorImageUrl,
            AuthorUrl = myData.AuthorUrl,
            AuthorName = myData.AuthorName,
            AuthorHandle = myData.AuthorHandle,
            ContentTweet = myData.ContentTweet,
            AlternateTweet = myData.AlternateTweet,
            CodeCampSpeakerUrl = myData.CodeCampSpeakerUrl,
            CodeCampSessionsUrl = myData.CodeCampSessionsUrl,
            TweetNotRelevant = myData.TweetNotRelevant,
            TweetInserted = myData.TweetInserted == null ? null :  (DateTime?) new DateTime(myData.TweetInserted.Value.Ticks,DateTimeKind.Utc),
            TweetUpdate = myData.TweetUpdate == null ? null :  (DateTime?) new DateTime(myData.TweetUpdate.Value.Ticks,DateTimeKind.Utc)
      });
		    return results;
        }
        
        public List<TwitterUpdateResult> GetJustBaseTableColumns(TwitterUpdateQuery query)
        {
            foreach (var info in typeof (TwitterUpdateQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: TwitterUpdate QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<TwitterUpdate> baseQuery = from myData in meta.TwitterUpdate select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<TwitterUpdateResult> results = (from myData in baseQuery orderby myData.Id select new TwitterUpdateResult { Id= myData.Id,
                        Title = myData.Title,
                        Published = myData.Published == null ? null :  (DateTime?) new DateTime(myData.Published.Value.Ticks,DateTimeKind.Utc),
                        AuthorImageUrl = myData.AuthorImageUrl,
                        AuthorUrl = myData.AuthorUrl,
                        AuthorName = myData.AuthorName,
                        AuthorHandle = myData.AuthorHandle,
                        ContentTweet = myData.ContentTweet,
                        AlternateTweet = myData.AlternateTweet,
                        CodeCampSpeakerUrl = myData.CodeCampSpeakerUrl,
                        CodeCampSessionsUrl = myData.CodeCampSessionsUrl,
                        TweetNotRelevant = myData.TweetNotRelevant,
                        TweetInserted = myData.TweetInserted == null ? null :  (DateTime?) new DateTime(myData.TweetInserted.Value.Ticks,DateTimeKind.Utc),
                        TweetUpdate = myData.TweetUpdate == null ? null :  (DateTime?) new DateTime(myData.TweetUpdate.Value.Ticks,DateTimeKind.Utc)
            });
            
            List<TwitterUpdateResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<TwitterUpdate> BaseQueryAutoGen(IQueryable<TwitterUpdate> baseQuery, TwitterUpdateQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.Title != null) baseQuery = baseQuery.Where(a => a.Title.ToLower().Equals(query.Title.ToLower()));
            if (query.Published != null) baseQuery = baseQuery.Where(a => a.Published.Value.CompareTo(query.Published.Value) == 0);
            if (query.AuthorImageUrl != null) baseQuery = baseQuery.Where(a => a.AuthorImageUrl.ToLower().Equals(query.AuthorImageUrl.ToLower()));
            if (query.AuthorUrl != null) baseQuery = baseQuery.Where(a => a.AuthorUrl.ToLower().Equals(query.AuthorUrl.ToLower()));
            if (query.AuthorName != null) baseQuery = baseQuery.Where(a => a.AuthorName.ToLower().Equals(query.AuthorName.ToLower()));
            if (query.AuthorHandle != null) baseQuery = baseQuery.Where(a => a.AuthorHandle.ToLower().Equals(query.AuthorHandle.ToLower()));
            if (query.ContentTweet != null) baseQuery = baseQuery.Where(a => a.ContentTweet.ToLower().Equals(query.ContentTweet.ToLower()));
            if (query.AlternateTweet != null) baseQuery = baseQuery.Where(a => a.AlternateTweet.ToLower().Equals(query.AlternateTweet.ToLower()));
            if (query.CodeCampSpeakerUrl != null) baseQuery = baseQuery.Where(a => a.CodeCampSpeakerUrl.ToLower().Equals(query.CodeCampSpeakerUrl.ToLower()));
            if (query.CodeCampSessionsUrl != null) baseQuery = baseQuery.Where(a => a.CodeCampSessionsUrl.ToLower().Equals(query.CodeCampSessionsUrl.ToLower()));
            if (query.TweetNotRelevant != null) baseQuery = baseQuery.Where(a => a.TweetNotRelevant == query.TweetNotRelevant);
            if (query.TweetInserted != null) baseQuery = baseQuery.Where(a => a.TweetInserted.Value.CompareTo(query.TweetInserted.Value) == 0);
            if (query.TweetUpdate != null) baseQuery = baseQuery.Where(a => a.TweetUpdate.Value.CompareTo(query.TweetUpdate.Value) == 0);

            return baseQuery;
        }
        
    }
}
