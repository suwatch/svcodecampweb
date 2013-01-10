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
    public partial class ProfileDataManager : ManagerBase<ProfileDataManager, ProfileDataResult, ProfileData, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(ProfileData record, ProfileDataResult result)
        {
            record.PKID = result.PKID;
            record.Keyname = result.Keyname;
            record.Data = result.Data;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override ProfileData GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.ProfileData where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<ProfileDataResult> GetBaseResultIQueryable(IQueryable<ProfileData> baseQuery)
        {
      IQueryable<ProfileDataResult> results = (from myData in baseQuery orderby myData.Id select new ProfileDataResult { Id= myData.Id,
            PKID = myData.PKID,
            Keyname = myData.Keyname,
            Data = myData.Data
      });
		    return results;
        }
        
        public List<ProfileDataResult> GetJustBaseTableColumns(ProfileDataQuery query)
        {
            foreach (var info in typeof (ProfileDataQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: ProfileData QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<ProfileData> baseQuery = from myData in meta.ProfileData select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<ProfileDataResult> results = (from myData in baseQuery orderby myData.Id select new ProfileDataResult { Id= myData.Id,
                        PKID = myData.PKID,
                        Keyname = myData.Keyname,
                        Data = myData.Data
            });
            
            List<ProfileDataResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<ProfileData> BaseQueryAutoGen(IQueryable<ProfileData> baseQuery, ProfileDataQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.Keyname != null) baseQuery = baseQuery.Where(a => a.Keyname.ToLower().Equals(query.Keyname.ToLower()));
            if (query.Data != null) baseQuery = baseQuery.Where(a => a.Data.ToLower().Equals(query.Data.ToLower()));

            return baseQuery;
        }
        
    }
}
