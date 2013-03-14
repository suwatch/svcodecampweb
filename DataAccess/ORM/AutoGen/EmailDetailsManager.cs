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
    public partial class EmailDetailsManager : ManagerBase<EmailDetailsManager, EmailDetailsResult, EmailDetails, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(EmailDetails record, EmailDetailsResult result)
        {
            record.AttendeesId = result.AttendeesId;
            record.EmailDetailsTopicId = result.EmailDetailsTopicId;
            record.EmailDetailsGuid = result.EmailDetailsGuid;
            record.EmailReadCount = result.EmailReadCount;
            record.EmailReadDate = result.EmailReadDate;
            record.MessageUniqueId = result.MessageUniqueId;
            record.EmailSendStatus = result.EmailSendStatus;
            record.EmailSendStartTime = result.EmailSendStartTime;
            record.EmailSendFinishTime = result.EmailSendFinishTime;
            record.EmailSendLogMessage = result.EmailSendLogMessage;
            record.Subject = result.Subject;
            record.BodyText = result.BodyText;
            record.SentDateTime = result.SentDateTime;
            record.EmailFrom = result.EmailFrom;
            record.EmailTo = result.EmailTo;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override EmailDetails GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.EmailDetails where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<EmailDetailsResult> GetBaseResultIQueryable(IQueryable<EmailDetails> baseQuery)
        {
      IQueryable<EmailDetailsResult> results = (from myData in baseQuery orderby myData.Id select new EmailDetailsResult { Id= myData.Id,
            AttendeesId = myData.AttendeesId,
            EmailDetailsTopicId = myData.EmailDetailsTopicId,
            EmailDetailsGuid = myData.EmailDetailsGuid,
            EmailReadCount = myData.EmailReadCount,
            EmailReadDate = myData.EmailReadDate == null ? null :  (DateTime?) new DateTime(myData.EmailReadDate.Value.Ticks,DateTimeKind.Utc),
            MessageUniqueId = myData.MessageUniqueId,
            EmailSendStatus = myData.EmailSendStatus,
            EmailSendStartTime = myData.EmailSendStartTime == null ? null :  (DateTime?) new DateTime(myData.EmailSendStartTime.Value.Ticks,DateTimeKind.Utc),
            EmailSendFinishTime = myData.EmailSendFinishTime == null ? null :  (DateTime?) new DateTime(myData.EmailSendFinishTime.Value.Ticks,DateTimeKind.Utc),
            EmailSendLogMessage = myData.EmailSendLogMessage,
            Subject = myData.Subject,
            BodyText = myData.BodyText,
            SentDateTime = myData.SentDateTime == null ? null :  (DateTime?) new DateTime(myData.SentDateTime.Value.Ticks,DateTimeKind.Utc),
            EmailFrom = myData.EmailFrom,
            EmailTo = myData.EmailTo
      });
		    return results;
        }
        
        public List<EmailDetailsResult> GetJustBaseTableColumns(EmailDetailsQuery query)
        {
            foreach (var info in typeof (EmailDetailsQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: EmailDetails QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<EmailDetails> baseQuery = from myData in meta.EmailDetails select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<EmailDetailsResult> results = (from myData in baseQuery orderby myData.Id select new EmailDetailsResult { Id= myData.Id,
                        AttendeesId = myData.AttendeesId,
                        EmailDetailsTopicId = myData.EmailDetailsTopicId,
                        EmailDetailsGuid = myData.EmailDetailsGuid,
                        EmailReadCount = myData.EmailReadCount,
                        EmailReadDate = myData.EmailReadDate == null ? null :  (DateTime?) new DateTime(myData.EmailReadDate.Value.Ticks,DateTimeKind.Utc),
                        MessageUniqueId = myData.MessageUniqueId,
                        EmailSendStatus = myData.EmailSendStatus,
                        EmailSendStartTime = myData.EmailSendStartTime == null ? null :  (DateTime?) new DateTime(myData.EmailSendStartTime.Value.Ticks,DateTimeKind.Utc),
                        EmailSendFinishTime = myData.EmailSendFinishTime == null ? null :  (DateTime?) new DateTime(myData.EmailSendFinishTime.Value.Ticks,DateTimeKind.Utc),
                        EmailSendLogMessage = myData.EmailSendLogMessage,
                        Subject = myData.Subject,
                        BodyText = myData.BodyText,
                        SentDateTime = myData.SentDateTime == null ? null :  (DateTime?) new DateTime(myData.SentDateTime.Value.Ticks,DateTimeKind.Utc),
                        EmailFrom = myData.EmailFrom,
                        EmailTo = myData.EmailTo
            });
            
            List<EmailDetailsResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<EmailDetails> BaseQueryAutoGen(IQueryable<EmailDetails> baseQuery, EmailDetailsQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.AttendeesId != null) baseQuery = baseQuery.Where(a => a.AttendeesId == query.AttendeesId);
            if (query.EmailDetailsTopicId != null) baseQuery = baseQuery.Where(a => a.EmailDetailsTopicId == query.EmailDetailsTopicId);
            if (query.EmailReadCount != null) baseQuery = baseQuery.Where(a => a.EmailReadCount == query.EmailReadCount);
            if (query.EmailReadDate != null) baseQuery = baseQuery.Where(a => a.EmailReadDate.Value.CompareTo(query.EmailReadDate.Value) == 0);
            if (query.MessageUniqueId != null) baseQuery = baseQuery.Where(a => a.MessageUniqueId.ToLower().Equals(query.MessageUniqueId.ToLower()));
            if (query.EmailSendStatus != null) baseQuery = baseQuery.Where(a => a.EmailSendStatus.ToLower().Equals(query.EmailSendStatus.ToLower()));
            if (query.EmailSendStartTime != null) baseQuery = baseQuery.Where(a => a.EmailSendStartTime.Value.CompareTo(query.EmailSendStartTime.Value) == 0);
            if (query.EmailSendFinishTime != null) baseQuery = baseQuery.Where(a => a.EmailSendFinishTime.Value.CompareTo(query.EmailSendFinishTime.Value) == 0);
            if (query.EmailSendLogMessage != null) baseQuery = baseQuery.Where(a => a.EmailSendLogMessage.ToLower().Equals(query.EmailSendLogMessage.ToLower()));
            if (query.Subject != null) baseQuery = baseQuery.Where(a => a.Subject.ToLower().Equals(query.Subject.ToLower()));
            if (query.BodyText != null) baseQuery = baseQuery.Where(a => a.BodyText.ToLower().Equals(query.BodyText.ToLower()));
            if (query.SentDateTime != null) baseQuery = baseQuery.Where(a => a.SentDateTime.Value.CompareTo(query.SentDateTime.Value) == 0);
            if (query.EmailFrom != null) baseQuery = baseQuery.Where(a => a.EmailFrom.ToLower().Equals(query.EmailFrom.ToLower()));
            if (query.EmailTo != null) baseQuery = baseQuery.Where(a => a.EmailTo.ToLower().Equals(query.EmailTo.ToLower()));

            return baseQuery;
        }
        
    }
}
