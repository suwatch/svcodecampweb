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
    public partial class ReferringUrlGroupManager : ManagerBase<ReferringUrlGroupManager, ReferringUrlGroupResult, ReferringUrlGroup, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(ReferringUrlGroup record, ReferringUrlGroupResult result)
        {
            record.ReferringUrlName = result.ReferringUrlName;
            record.AttendeesId = result.AttendeesId;
            record.ArticleName = result.ArticleName;
            record.UserGroup = result.UserGroup;
            record.DeletedCount = result.DeletedCount;
            record.Visible = result.Visible;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override ReferringUrlGroup GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.ReferringUrlGroup where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<ReferringUrlGroupResult> GetBaseResultIQueryable(IQueryable<ReferringUrlGroup> baseQuery)
        {
      IQueryable<ReferringUrlGroupResult> results = (from myData in baseQuery orderby myData.Id select new ReferringUrlGroupResult { Id= myData.Id,
            ReferringUrlName = myData.ReferringUrlName,
            AttendeesId = myData.AttendeesId,
            ArticleName = myData.ArticleName,
            UserGroup = myData.UserGroup,
            DeletedCount = myData.DeletedCount,
            Visible = myData.Visible
      });
		    return results;
        }
        
        public List<ReferringUrlGroupResult> GetJustBaseTableColumns(ReferringUrlGroupQuery query)
        {
            foreach (var info in typeof (ReferringUrlGroupQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: ReferringUrlGroup QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<ReferringUrlGroup> baseQuery = from myData in meta.ReferringUrlGroup select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<ReferringUrlGroupResult> results = (from myData in baseQuery orderby myData.Id select new ReferringUrlGroupResult { Id= myData.Id,
                        ReferringUrlName = myData.ReferringUrlName,
                        AttendeesId = myData.AttendeesId,
                        ArticleName = myData.ArticleName,
                        UserGroup = myData.UserGroup,
                        DeletedCount = myData.DeletedCount,
                        Visible = myData.Visible
            });
            
            List<ReferringUrlGroupResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<ReferringUrlGroup> BaseQueryAutoGen(IQueryable<ReferringUrlGroup> baseQuery, ReferringUrlGroupQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.ReferringUrlName != null) baseQuery = baseQuery.Where(a => a.ReferringUrlName.ToLower().Equals(query.ReferringUrlName.ToLower()));
            if (query.AttendeesId != null) baseQuery = baseQuery.Where(a => a.AttendeesId == query.AttendeesId);
            if (query.ArticleName != null) baseQuery = baseQuery.Where(a => a.ArticleName.ToLower().Equals(query.ArticleName.ToLower()));
            if (query.UserGroup != null) baseQuery = baseQuery.Where(a => a.UserGroup.ToLower().Equals(query.UserGroup.ToLower()));
            if (query.DeletedCount != null) baseQuery = baseQuery.Where(a => a.DeletedCount == query.DeletedCount);
            if (query.Visible != null) baseQuery = baseQuery.Where(a => a.Visible == query.Visible);

            return baseQuery;
        }
        
    }
}
