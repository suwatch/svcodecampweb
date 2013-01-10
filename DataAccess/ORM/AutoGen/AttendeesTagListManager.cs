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
    public partial class AttendeesTagListManager : ManagerBase<AttendeesTagListManager, AttendeesTagListResult, AttendeesTagList, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(AttendeesTagList record, AttendeesTagListResult result)
        {
            record.AttendeesId = result.AttendeesId;
            record.TagListName = result.TagListName;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override AttendeesTagList GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.AttendeesTagList where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<AttendeesTagListResult> GetBaseResultIQueryable(IQueryable<AttendeesTagList> baseQuery)
        {
      IQueryable<AttendeesTagListResult> results = (from myData in baseQuery orderby myData.Id select new AttendeesTagListResult { Id= myData.Id,
            AttendeesId = myData.AttendeesId,
            TagListName = myData.TagListName
      });
		    return results;
        }
        
        public List<AttendeesTagListResult> GetJustBaseTableColumns(AttendeesTagListQuery query)
        {
            foreach (var info in typeof (AttendeesTagListQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: AttendeesTagList QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<AttendeesTagList> baseQuery = from myData in meta.AttendeesTagList select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<AttendeesTagListResult> results = (from myData in baseQuery orderby myData.Id select new AttendeesTagListResult { Id= myData.Id,
                        AttendeesId = myData.AttendeesId,
                        TagListName = myData.TagListName
            });
            
            List<AttendeesTagListResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<AttendeesTagList> BaseQueryAutoGen(IQueryable<AttendeesTagList> baseQuery, AttendeesTagListQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.AttendeesId != null) baseQuery = baseQuery.Where(a => a.AttendeesId == query.AttendeesId);
            if (query.TagListName != null) baseQuery = baseQuery.Where(a => a.TagListName.ToLower().Equals(query.TagListName.ToLower()));

            return baseQuery;
        }
        
    }
}
