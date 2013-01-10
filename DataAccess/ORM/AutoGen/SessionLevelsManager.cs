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
    public partial class SessionLevelsManager : ManagerBase<SessionLevelsManager, SessionLevelsResult, SessionLevels, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(SessionLevels record, SessionLevelsResult result)
        {
            record.Description = result.Description;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override SessionLevels GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.SessionLevels where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<SessionLevelsResult> GetBaseResultIQueryable(IQueryable<SessionLevels> baseQuery)
        {
      IQueryable<SessionLevelsResult> results = (from myData in baseQuery orderby myData.Id select new SessionLevelsResult { Id= myData.Id,
            Description = myData.Description
      });
		    return results;
        }
        
        public List<SessionLevelsResult> GetJustBaseTableColumns(SessionLevelsQuery query)
        {
            foreach (var info in typeof (SessionLevelsQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: SessionLevels QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<SessionLevels> baseQuery = from myData in meta.SessionLevels select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<SessionLevelsResult> results = (from myData in baseQuery orderby myData.Id select new SessionLevelsResult { Id= myData.Id,
                        Description = myData.Description
            });
            
            List<SessionLevelsResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<SessionLevels> BaseQueryAutoGen(IQueryable<SessionLevels> baseQuery, SessionLevelsQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.Description != null) baseQuery = baseQuery.Where(a => a.Description.ToLower().Equals(query.Description.ToLower()));

            return baseQuery;
        }
        
    }
}
