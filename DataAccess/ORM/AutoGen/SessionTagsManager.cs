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
    public partial class SessionTagsManager : ManagerBase<SessionTagsManager, SessionTagsResult, SessionTags, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(SessionTags record, SessionTagsResult result)
        {
            record.TagId = result.TagId;
            record.SessionId = result.SessionId;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override SessionTags GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.SessionTags where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<SessionTagsResult> GetBaseResultIQueryable(IQueryable<SessionTags> baseQuery)
        {
      IQueryable<SessionTagsResult> results = (from myData in baseQuery orderby myData.Id select new SessionTagsResult { Id= myData.Id,
            TagId = myData.TagId,
            SessionId = myData.SessionId
      });
		    return results;
        }
        
        public List<SessionTagsResult> GetJustBaseTableColumns(SessionTagsQuery query)
        {
            foreach (var info in typeof (SessionTagsQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: SessionTags QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<SessionTags> baseQuery = from myData in meta.SessionTags select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<SessionTagsResult> results = (from myData in baseQuery orderby myData.Id select new SessionTagsResult { Id= myData.Id,
                        TagId = myData.TagId,
                        SessionId = myData.SessionId
            });
            
            List<SessionTagsResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<SessionTags> BaseQueryAutoGen(IQueryable<SessionTags> baseQuery, SessionTagsQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.TagId != null) baseQuery = baseQuery.Where(a => a.TagId == query.TagId);
            if (query.SessionId != null) baseQuery = baseQuery.Where(a => a.SessionId == query.SessionId);

            return baseQuery;
        }
        
    }
}
