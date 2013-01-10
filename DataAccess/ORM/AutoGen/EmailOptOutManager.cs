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
    public partial class EmailOptOutManager : ManagerBase<EmailOptOutManager, EmailOptOutResult, EmailOptOut, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(EmailOptOut record, EmailOptOutResult result)
        {
            record.Email = result.Email;
            record.DateAdded = result.DateAdded;
            record.Comment = result.Comment;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override EmailOptOut GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.EmailOptOut where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<EmailOptOutResult> GetBaseResultIQueryable(IQueryable<EmailOptOut> baseQuery)
        {
      IQueryable<EmailOptOutResult> results = (from myData in baseQuery orderby myData.Id select new EmailOptOutResult { Id= myData.Id,
            Email = myData.Email,
            DateAdded = new DateTime(myData.DateAdded.Ticks,DateTimeKind.Utc),
            Comment = myData.Comment
      });
		    return results;
        }
        
        public List<EmailOptOutResult> GetJustBaseTableColumns(EmailOptOutQuery query)
        {
            foreach (var info in typeof (EmailOptOutQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: EmailOptOut QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<EmailOptOut> baseQuery = from myData in meta.EmailOptOut select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<EmailOptOutResult> results = (from myData in baseQuery orderby myData.Id select new EmailOptOutResult { Id= myData.Id,
                        Email = myData.Email,
                        DateAdded = new DateTime(myData.DateAdded.Ticks,DateTimeKind.Utc),
                        Comment = myData.Comment
            });
            
            List<EmailOptOutResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<EmailOptOut> BaseQueryAutoGen(IQueryable<EmailOptOut> baseQuery, EmailOptOutQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.Email != null) baseQuery = baseQuery.Where(a => a.Email.ToLower().Equals(query.Email.ToLower()));
            if (query.DateAdded != null) baseQuery = baseQuery.Where(a => a.DateAdded.CompareTo(query.DateAdded) == 0);
            if (query.Comment != null) baseQuery = baseQuery.Where(a => a.Comment.ToLower().Equals(query.Comment.ToLower()));

            return baseQuery;
        }
        
    }
}
