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
    public partial class RolesInMembershipManager : ManagerBase<RolesInMembershipManager, RolesInMembershipResult, RolesInMembership, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(RolesInMembership record, RolesInMembershipResult result)
        {
            record.RoleName = result.RoleName;
            record.ApplicationName = result.ApplicationName;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override RolesInMembership GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.RolesInMembership where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<RolesInMembershipResult> GetBaseResultIQueryable(IQueryable<RolesInMembership> baseQuery)
        {
      IQueryable<RolesInMembershipResult> results = (from myData in baseQuery orderby myData.Id select new RolesInMembershipResult { Id= myData.Id,
            RoleName = myData.RoleName,
            ApplicationName = myData.ApplicationName
      });
		    return results;
        }
        
        public List<RolesInMembershipResult> GetJustBaseTableColumns(RolesInMembershipQuery query)
        {
            foreach (var info in typeof (RolesInMembershipQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: RolesInMembership QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<RolesInMembership> baseQuery = from myData in meta.RolesInMembership select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<RolesInMembershipResult> results = (from myData in baseQuery orderby myData.Id select new RolesInMembershipResult { Id= myData.Id,
                        RoleName = myData.RoleName,
                        ApplicationName = myData.ApplicationName
            });
            
            List<RolesInMembershipResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<RolesInMembership> BaseQueryAutoGen(IQueryable<RolesInMembership> baseQuery, RolesInMembershipQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.RoleName != null) baseQuery = baseQuery.Where(a => a.RoleName.ToLower().Equals(query.RoleName.ToLower()));
            if (query.ApplicationName != null) baseQuery = baseQuery.Where(a => a.ApplicationName.ToLower().Equals(query.ApplicationName.ToLower()));

            return baseQuery;
        }
        
    }
}
