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
    public partial class GuidRouterManager : ManagerBase<GuidRouterManager, GuidRouterResult, GuidRouter, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(GuidRouter record, GuidRouterResult result)
        {
            record.RouterType = result.RouterType;
            record.GuidItself = result.GuidItself;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override GuidRouter GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.GuidRouter where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<GuidRouterResult> GetBaseResultIQueryable(IQueryable<GuidRouter> baseQuery)
        {
      IQueryable<GuidRouterResult> results = (from myData in baseQuery orderby myData.Id select new GuidRouterResult { Id= myData.Id,
            RouterType = myData.RouterType,
            GuidItself = myData.GuidItself
      });
		    return results;
        }
        
        public List<GuidRouterResult> GetJustBaseTableColumns(GuidRouterQuery query)
        {
            foreach (var info in typeof (GuidRouterQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: GuidRouter QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<GuidRouter> baseQuery = from myData in meta.GuidRouter select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<GuidRouterResult> results = (from myData in baseQuery orderby myData.Id select new GuidRouterResult { Id= myData.Id,
                        RouterType = myData.RouterType,
                        GuidItself = myData.GuidItself
            });
            
            List<GuidRouterResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<GuidRouter> BaseQueryAutoGen(IQueryable<GuidRouter> baseQuery, GuidRouterQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.RouterType != null) baseQuery = baseQuery.Where(a => a.RouterType.ToLower().Equals(query.RouterType.ToLower()));

            return baseQuery;
        }
        
    }
}
