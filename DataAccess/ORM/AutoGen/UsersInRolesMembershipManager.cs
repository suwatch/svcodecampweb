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
    public partial class UsersInRolesMembershipManager : ManagerBase<UsersInRolesMembershipManager, UsersInRolesMembershipResult, UsersInRolesMembership, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(UsersInRolesMembership record, UsersInRolesMembershipResult result)
        {
            record.Username = result.Username;
            record.Rolename = result.Rolename;
            record.ApplicationName = result.ApplicationName;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override UsersInRolesMembership GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.UsersInRolesMembership where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<UsersInRolesMembershipResult> GetBaseResultIQueryable(IQueryable<UsersInRolesMembership> baseQuery)
        {
      IQueryable<UsersInRolesMembershipResult> results = (from myData in baseQuery orderby myData.Id select new UsersInRolesMembershipResult { Id= myData.Id,
            Username = myData.Username,
            Rolename = myData.Rolename,
            ApplicationName = myData.ApplicationName
      });
		    return results;
        }
        
        public List<UsersInRolesMembershipResult> GetJustBaseTableColumns(UsersInRolesMembershipQuery query)
        {
            foreach (var info in typeof (UsersInRolesMembershipQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: UsersInRolesMembership QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<UsersInRolesMembership> baseQuery = from myData in meta.UsersInRolesMembership select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<UsersInRolesMembershipResult> results = (from myData in baseQuery orderby myData.Id select new UsersInRolesMembershipResult { Id= myData.Id,
                        Username = myData.Username,
                        Rolename = myData.Rolename,
                        ApplicationName = myData.ApplicationName
            });
            
            List<UsersInRolesMembershipResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<UsersInRolesMembership> BaseQueryAutoGen(IQueryable<UsersInRolesMembership> baseQuery, UsersInRolesMembershipQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.Username != null) baseQuery = baseQuery.Where(a => a.Username.ToLower().Equals(query.Username.ToLower()));
            if (query.Rolename != null) baseQuery = baseQuery.Where(a => a.Rolename.ToLower().Equals(query.Rolename.ToLower()));
            if (query.ApplicationName != null) baseQuery = baseQuery.Where(a => a.ApplicationName.ToLower().Equals(query.ApplicationName.ToLower()));

            return baseQuery;
        }
        
    }
}
