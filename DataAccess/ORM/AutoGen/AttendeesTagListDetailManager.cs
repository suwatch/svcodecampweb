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
    public partial class AttendeesTagListDetailManager : ManagerBase<AttendeesTagListDetailManager, AttendeesTagListDetailResult, AttendeesTagListDetail, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(AttendeesTagListDetail record, AttendeesTagListDetailResult result)
        {
            record.AttendeesId = result.AttendeesId;
            record.AttendeesTagListId = result.AttendeesTagListId;
            record.TagsId = result.TagsId;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override AttendeesTagListDetail GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.AttendeesTagListDetail where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<AttendeesTagListDetailResult> GetBaseResultIQueryable(IQueryable<AttendeesTagListDetail> baseQuery)
        {
      IQueryable<AttendeesTagListDetailResult> results = (from myData in baseQuery orderby myData.Id select new AttendeesTagListDetailResult { Id= myData.Id,
            AttendeesId = myData.AttendeesId,
            AttendeesTagListId = myData.AttendeesTagListId,
            TagsId = myData.TagsId
      });
		    return results;
        }
        
        public List<AttendeesTagListDetailResult> GetJustBaseTableColumns(AttendeesTagListDetailQuery query)
        {
            foreach (var info in typeof (AttendeesTagListDetailQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: AttendeesTagListDetail QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<AttendeesTagListDetail> baseQuery = from myData in meta.AttendeesTagListDetail select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<AttendeesTagListDetailResult> results = (from myData in baseQuery orderby myData.Id select new AttendeesTagListDetailResult { Id= myData.Id,
                        AttendeesId = myData.AttendeesId,
                        AttendeesTagListId = myData.AttendeesTagListId,
                        TagsId = myData.TagsId
            });
            
            List<AttendeesTagListDetailResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<AttendeesTagListDetail> BaseQueryAutoGen(IQueryable<AttendeesTagListDetail> baseQuery, AttendeesTagListDetailQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.AttendeesId != null) baseQuery = baseQuery.Where(a => a.AttendeesId == query.AttendeesId);
            if (query.AttendeesTagListId != null) baseQuery = baseQuery.Where(a => a.AttendeesTagListId == query.AttendeesTagListId);
            if (query.TagsId != null) baseQuery = baseQuery.Where(a => a.TagsId == query.TagsId);

            return baseQuery;
        }
        
    }
}
