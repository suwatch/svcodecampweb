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
    public partial class EmailDetailsTopicManager : ManagerBase<EmailDetailsTopicManager, EmailDetailsTopicResult, EmailDetailsTopic, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(EmailDetailsTopic record, EmailDetailsTopicResult result)
        {
            record.CreateDate = result.CreateDate;
            record.Title = result.Title;
            record.FirstRunDate = result.FirstRunDate;
            record.Notes = result.Notes;
            record.EmailSubject = result.EmailSubject;
            record.EmailMime = result.EmailMime;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override EmailDetailsTopic GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.EmailDetailsTopic where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<EmailDetailsTopicResult> GetBaseResultIQueryable(IQueryable<EmailDetailsTopic> baseQuery)
        {
      IQueryable<EmailDetailsTopicResult> results = (from myData in baseQuery orderby myData.Id select new EmailDetailsTopicResult { Id= myData.Id,
            CreateDate = new DateTime(myData.CreateDate.Ticks,DateTimeKind.Utc),
            Title = myData.Title,
            FirstRunDate = myData.FirstRunDate == null ? null :  (DateTime?) new DateTime(myData.FirstRunDate.Value.Ticks,DateTimeKind.Utc),
            Notes = myData.Notes,
            EmailSubject = myData.EmailSubject,
            EmailMime = myData.EmailMime
      });
		    return results;
        }
        
        public List<EmailDetailsTopicResult> GetJustBaseTableColumns(EmailDetailsTopicQuery query)
        {
            foreach (var info in typeof (EmailDetailsTopicQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: EmailDetailsTopic QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<EmailDetailsTopic> baseQuery = from myData in meta.EmailDetailsTopic select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<EmailDetailsTopicResult> results = (from myData in baseQuery orderby myData.Id select new EmailDetailsTopicResult { Id= myData.Id,
                        CreateDate = new DateTime(myData.CreateDate.Ticks,DateTimeKind.Utc),
                        Title = myData.Title,
                        FirstRunDate = myData.FirstRunDate == null ? null :  (DateTime?) new DateTime(myData.FirstRunDate.Value.Ticks,DateTimeKind.Utc),
                        Notes = myData.Notes,
                        EmailSubject = myData.EmailSubject,
                        EmailMime = myData.EmailMime
            });
            
            List<EmailDetailsTopicResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<EmailDetailsTopic> BaseQueryAutoGen(IQueryable<EmailDetailsTopic> baseQuery, EmailDetailsTopicQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.CreateDate != null) baseQuery = baseQuery.Where(a => a.CreateDate.CompareTo(query.CreateDate) == 0);
            if (query.Title != null) baseQuery = baseQuery.Where(a => a.Title.ToLower().Equals(query.Title.ToLower()));
            if (query.FirstRunDate != null) baseQuery = baseQuery.Where(a => a.FirstRunDate.Value.CompareTo(query.FirstRunDate.Value) == 0);
            if (query.Notes != null) baseQuery = baseQuery.Where(a => a.Notes.ToLower().Equals(query.Notes.ToLower()));
            if (query.EmailSubject != null) baseQuery = baseQuery.Where(a => a.EmailSubject.ToLower().Equals(query.EmailSubject.ToLower()));
            if (query.EmailMime != null) baseQuery = baseQuery.Where(a => a.EmailMime.ToLower().Equals(query.EmailMime.ToLower()));

            return baseQuery;
        }
        
    }
}
