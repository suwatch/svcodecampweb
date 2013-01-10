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
    public partial class SessionsManager : ManagerBase<SessionsManager, SessionsResult, Sessions, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(Sessions record, SessionsResult result)
        {
            record.CodeCampYearId = result.CodeCampYearId;
            record.Attendeesid = result.Attendeesid;
            record.SessionLevel_id = result.SessionLevel_id;
            record.SponsorId = result.SponsorId;
            record.Username = result.Username;
            record.Title = result.Title;
            record.Description = result.Description;
            record.Approved = result.Approved;
            record.Createdate = result.Createdate;
            record.Updatedate = result.Updatedate;
            record.AdminComments = result.AdminComments;
            record.DoNotShowPrimarySpeaker = result.DoNotShowPrimarySpeaker;
            record.InterentAccessRequired = result.InterentAccessRequired;
            record.LectureRoomsId = result.LectureRoomsId;
            record.SessionTimesId = result.SessionTimesId;
            record.TweetLine = result.TweetLine;
            record.TweetLineTweetedDate = result.TweetLineTweetedDate;
            record.TweetLineTweeted = result.TweetLineTweeted;
            record.SessionsMaterialUrl = result.SessionsMaterialUrl;
            record.SessionsMaterialQueueToSend = result.SessionsMaterialQueueToSend;
            record.SessionMaterialUrlDateSent = result.SessionMaterialUrlDateSent;
            record.SessionMaterialUrlMessage = result.SessionMaterialUrlMessage;
            record.TwitterHashTags = result.TwitterHashTags;
            record.TweetLinePreCamp = result.TweetLinePreCamp;
            record.TweetLineTweetedDatePreCamp = result.TweetLineTweetedDatePreCamp;
            record.TweetLineTweetedPreCamp = result.TweetLineTweetedPreCamp;
            record.ShortUrl = result.ShortUrl;
            record.BoxFolderIdString = result.BoxFolderIdString;
            record.BoxFolderEmailInAddress = result.BoxFolderEmailInAddress;
            record.BoxFolderPublicUrl = result.BoxFolderPublicUrl;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override Sessions GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.Sessions where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<SessionsResult> GetBaseResultIQueryable(IQueryable<Sessions> baseQuery)
        {
      IQueryable<SessionsResult> results = (from myData in baseQuery orderby myData.Id select new SessionsResult { Id= myData.Id,
            CodeCampYearId = myData.CodeCampYearId,
            Attendeesid = myData.Attendeesid,
            SessionLevel_id = myData.SessionLevel_id,
            SponsorId = myData.SponsorId,
            Username = myData.Username,
            Title = myData.Title,
            Description = myData.Description,
            Approved = myData.Approved,
            Createdate = myData.Createdate == null ? null :  (DateTime?) new DateTime(myData.Createdate.Value.Ticks,DateTimeKind.Utc),
            Updatedate = myData.Updatedate == null ? null :  (DateTime?) new DateTime(myData.Updatedate.Value.Ticks,DateTimeKind.Utc),
            AdminComments = myData.AdminComments,
            DoNotShowPrimarySpeaker = myData.DoNotShowPrimarySpeaker,
            InterentAccessRequired = myData.InterentAccessRequired,
            LectureRoomsId = myData.LectureRoomsId,
            SessionTimesId = myData.SessionTimesId,
            TweetLine = myData.TweetLine,
            TweetLineTweetedDate = myData.TweetLineTweetedDate == null ? null :  (DateTime?) new DateTime(myData.TweetLineTweetedDate.Value.Ticks,DateTimeKind.Utc),
            TweetLineTweeted = myData.TweetLineTweeted,
            SessionsMaterialUrl = myData.SessionsMaterialUrl,
            SessionsMaterialQueueToSend = myData.SessionsMaterialQueueToSend,
            SessionMaterialUrlDateSent = myData.SessionMaterialUrlDateSent == null ? null :  (DateTime?) new DateTime(myData.SessionMaterialUrlDateSent.Value.Ticks,DateTimeKind.Utc),
            SessionMaterialUrlMessage = myData.SessionMaterialUrlMessage,
            TwitterHashTags = myData.TwitterHashTags,
            TweetLinePreCamp = myData.TweetLinePreCamp,
            TweetLineTweetedDatePreCamp = myData.TweetLineTweetedDatePreCamp == null ? null :  (DateTime?) new DateTime(myData.TweetLineTweetedDatePreCamp.Value.Ticks,DateTimeKind.Utc),
            TweetLineTweetedPreCamp = myData.TweetLineTweetedPreCamp,
            ShortUrl = myData.ShortUrl,
            BoxFolderIdString = myData.BoxFolderIdString,
            BoxFolderEmailInAddress = myData.BoxFolderEmailInAddress,
            BoxFolderPublicUrl = myData.BoxFolderPublicUrl
      });
		    return results;
        }
        
        public List<SessionsResult> GetJustBaseTableColumns(SessionsQuery query)
        {
            foreach (var info in typeof (SessionsQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: Sessions QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<Sessions> baseQuery = from myData in meta.Sessions select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<SessionsResult> results = (from myData in baseQuery orderby myData.Id select new SessionsResult { Id= myData.Id,
                        CodeCampYearId = myData.CodeCampYearId,
                        Attendeesid = myData.Attendeesid,
                        SessionLevel_id = myData.SessionLevel_id,
                        SponsorId = myData.SponsorId,
                        Username = myData.Username,
                        Title = myData.Title,
                        Description = myData.Description,
                        Approved = myData.Approved,
                        Createdate = myData.Createdate == null ? null :  (DateTime?) new DateTime(myData.Createdate.Value.Ticks,DateTimeKind.Utc),
                        Updatedate = myData.Updatedate == null ? null :  (DateTime?) new DateTime(myData.Updatedate.Value.Ticks,DateTimeKind.Utc),
                        AdminComments = myData.AdminComments,
                        DoNotShowPrimarySpeaker = myData.DoNotShowPrimarySpeaker,
                        InterentAccessRequired = myData.InterentAccessRequired,
                        LectureRoomsId = myData.LectureRoomsId,
                        SessionTimesId = myData.SessionTimesId,
                        TweetLine = myData.TweetLine,
                        TweetLineTweetedDate = myData.TweetLineTweetedDate == null ? null :  (DateTime?) new DateTime(myData.TweetLineTweetedDate.Value.Ticks,DateTimeKind.Utc),
                        TweetLineTweeted = myData.TweetLineTweeted,
                        SessionsMaterialUrl = myData.SessionsMaterialUrl,
                        SessionsMaterialQueueToSend = myData.SessionsMaterialQueueToSend,
                        SessionMaterialUrlDateSent = myData.SessionMaterialUrlDateSent == null ? null :  (DateTime?) new DateTime(myData.SessionMaterialUrlDateSent.Value.Ticks,DateTimeKind.Utc),
                        SessionMaterialUrlMessage = myData.SessionMaterialUrlMessage,
                        TwitterHashTags = myData.TwitterHashTags,
                        TweetLinePreCamp = myData.TweetLinePreCamp,
                        TweetLineTweetedDatePreCamp = myData.TweetLineTweetedDatePreCamp == null ? null :  (DateTime?) new DateTime(myData.TweetLineTweetedDatePreCamp.Value.Ticks,DateTimeKind.Utc),
                        TweetLineTweetedPreCamp = myData.TweetLineTweetedPreCamp,
                        ShortUrl = myData.ShortUrl,
                        BoxFolderIdString = myData.BoxFolderIdString,
                        BoxFolderEmailInAddress = myData.BoxFolderEmailInAddress,
                        BoxFolderPublicUrl = myData.BoxFolderPublicUrl
            });
            
            List<SessionsResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<Sessions> BaseQueryAutoGen(IQueryable<Sessions> baseQuery, SessionsQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.CodeCampYearId != null) baseQuery = baseQuery.Where(a => a.CodeCampYearId == query.CodeCampYearId);
            if (query.Attendeesid != null) baseQuery = baseQuery.Where(a => a.Attendeesid == query.Attendeesid);
            if (query.SessionLevel_id != null) baseQuery = baseQuery.Where(a => a.SessionLevel_id == query.SessionLevel_id);
            if (query.SponsorId != null) baseQuery = baseQuery.Where(a => a.SponsorId == query.SponsorId);
            if (query.Username != null) baseQuery = baseQuery.Where(a => a.Username.ToLower().Equals(query.Username.ToLower()));
            if (query.Title != null) baseQuery = baseQuery.Where(a => a.Title.ToLower().Equals(query.Title.ToLower()));
            if (query.Description != null) baseQuery = baseQuery.Where(a => a.Description.ToLower().Equals(query.Description.ToLower()));
            if (query.Approved != null) baseQuery = baseQuery.Where(a => a.Approved == query.Approved);
            if (query.Createdate != null) baseQuery = baseQuery.Where(a => a.Createdate.Value.CompareTo(query.Createdate.Value) == 0);
            if (query.Updatedate != null) baseQuery = baseQuery.Where(a => a.Updatedate.Value.CompareTo(query.Updatedate.Value) == 0);
            if (query.AdminComments != null) baseQuery = baseQuery.Where(a => a.AdminComments.ToLower().Equals(query.AdminComments.ToLower()));
            if (query.DoNotShowPrimarySpeaker != null) baseQuery = baseQuery.Where(a => a.DoNotShowPrimarySpeaker == query.DoNotShowPrimarySpeaker);
            if (query.InterentAccessRequired != null) baseQuery = baseQuery.Where(a => a.InterentAccessRequired == query.InterentAccessRequired);
            if (query.LectureRoomsId != null) baseQuery = baseQuery.Where(a => a.LectureRoomsId == query.LectureRoomsId);
            if (query.SessionTimesId != null) baseQuery = baseQuery.Where(a => a.SessionTimesId == query.SessionTimesId);
            if (query.TweetLine != null) baseQuery = baseQuery.Where(a => a.TweetLine.ToLower().Equals(query.TweetLine.ToLower()));
            if (query.TweetLineTweetedDate != null) baseQuery = baseQuery.Where(a => a.TweetLineTweetedDate.Value.CompareTo(query.TweetLineTweetedDate.Value) == 0);
            if (query.TweetLineTweeted != null) baseQuery = baseQuery.Where(a => a.TweetLineTweeted == query.TweetLineTweeted);
            if (query.SessionsMaterialUrl != null) baseQuery = baseQuery.Where(a => a.SessionsMaterialUrl.ToLower().Equals(query.SessionsMaterialUrl.ToLower()));
            if (query.SessionsMaterialQueueToSend != null) baseQuery = baseQuery.Where(a => a.SessionsMaterialQueueToSend == query.SessionsMaterialQueueToSend);
            if (query.SessionMaterialUrlDateSent != null) baseQuery = baseQuery.Where(a => a.SessionMaterialUrlDateSent.Value.CompareTo(query.SessionMaterialUrlDateSent.Value) == 0);
            if (query.SessionMaterialUrlMessage != null) baseQuery = baseQuery.Where(a => a.SessionMaterialUrlMessage.ToLower().Equals(query.SessionMaterialUrlMessage.ToLower()));
            if (query.TwitterHashTags != null) baseQuery = baseQuery.Where(a => a.TwitterHashTags.ToLower().Equals(query.TwitterHashTags.ToLower()));
            if (query.TweetLinePreCamp != null) baseQuery = baseQuery.Where(a => a.TweetLinePreCamp.ToLower().Equals(query.TweetLinePreCamp.ToLower()));
            if (query.TweetLineTweetedDatePreCamp != null) baseQuery = baseQuery.Where(a => a.TweetLineTweetedDatePreCamp.Value.CompareTo(query.TweetLineTweetedDatePreCamp.Value) == 0);
            if (query.TweetLineTweetedPreCamp != null) baseQuery = baseQuery.Where(a => a.TweetLineTweetedPreCamp == query.TweetLineTweetedPreCamp);
            if (query.ShortUrl != null) baseQuery = baseQuery.Where(a => a.ShortUrl.ToLower().Equals(query.ShortUrl.ToLower()));
            if (query.BoxFolderIdString != null) baseQuery = baseQuery.Where(a => a.BoxFolderIdString.ToLower().Equals(query.BoxFolderIdString.ToLower()));
            if (query.BoxFolderEmailInAddress != null) baseQuery = baseQuery.Where(a => a.BoxFolderEmailInAddress.ToLower().Equals(query.BoxFolderEmailInAddress.ToLower()));
            if (query.BoxFolderPublicUrl != null) baseQuery = baseQuery.Where(a => a.BoxFolderPublicUrl.ToLower().Equals(query.BoxFolderPublicUrl.ToLower()));

            return baseQuery;
        }
        
    }
}
