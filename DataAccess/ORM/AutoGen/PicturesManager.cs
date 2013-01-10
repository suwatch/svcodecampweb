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
    public partial class PicturesManager : ManagerBase<PicturesManager, PicturesResult, Pictures, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(Pictures record, PicturesResult result)
        {
            record.AttendeePKID = result.AttendeePKID;
            record.DateCreated = result.DateCreated;
            record.DateUpdated = result.DateUpdated;
            record.PictureBytes = result.PictureBytes;
            record.FileName = result.FileName;
            record.Description = result.Description;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override Pictures GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.Pictures where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<PicturesResult> GetBaseResultIQueryable(IQueryable<Pictures> baseQuery)
        {
      IQueryable<PicturesResult> results = (from myData in baseQuery orderby myData.Id select new PicturesResult { Id= myData.Id,
            AttendeePKID = myData.AttendeePKID,
            DateCreated = myData.DateCreated == null ? null :  (DateTime?) new DateTime(myData.DateCreated.Value.Ticks,DateTimeKind.Utc),
            DateUpdated = myData.DateUpdated == null ? null :  (DateTime?) new DateTime(myData.DateUpdated.Value.Ticks,DateTimeKind.Utc),
            PictureBytes = myData.PictureBytes,
            FileName = myData.FileName,
            Description = myData.Description
      });
		    return results;
        }
        
        public List<PicturesResult> GetJustBaseTableColumns(PicturesQuery query)
        {
            foreach (var info in typeof (PicturesQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: Pictures QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<Pictures> baseQuery = from myData in meta.Pictures select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<PicturesResult> results = (from myData in baseQuery orderby myData.Id select new PicturesResult { Id= myData.Id,
                        AttendeePKID = myData.AttendeePKID,
                        DateCreated = myData.DateCreated == null ? null :  (DateTime?) new DateTime(myData.DateCreated.Value.Ticks,DateTimeKind.Utc),
                        DateUpdated = myData.DateUpdated == null ? null :  (DateTime?) new DateTime(myData.DateUpdated.Value.Ticks,DateTimeKind.Utc),
                        PictureBytes = myData.PictureBytes,
                        FileName = myData.FileName,
                        Description = myData.Description
            });
            
            List<PicturesResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<Pictures> BaseQueryAutoGen(IQueryable<Pictures> baseQuery, PicturesQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.DateCreated != null) baseQuery = baseQuery.Where(a => a.DateCreated.Value.CompareTo(query.DateCreated.Value) == 0);
            if (query.DateUpdated != null) baseQuery = baseQuery.Where(a => a.DateUpdated.Value.CompareTo(query.DateUpdated.Value) == 0);
            if (query.FileName != null) baseQuery = baseQuery.Where(a => a.FileName.ToLower().Equals(query.FileName.ToLower()));
            if (query.Description != null) baseQuery = baseQuery.Where(a => a.Description.ToLower().Equals(query.Description.ToLower()));

            return baseQuery;
        }
        
    }
}
