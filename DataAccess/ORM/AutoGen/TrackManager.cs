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
    public partial class TrackManager : ManagerBase<TrackManager, TrackResult, Track, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(Track record, TrackResult result)
        {
            record.CodeCampYearId = result.CodeCampYearId;
            record.OwnerAttendeeId = result.OwnerAttendeeId;
            record.Named = result.Named;
            record.Description = result.Description;
            record.Visible = result.Visible;
            record.AlternateURL = result.AlternateURL;
            record.TrackImage = result.TrackImage;
            record.CreationDate = result.CreationDate;
            record.ModifiedDate = result.ModifiedDate;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override Track GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.Track where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<TrackResult> GetBaseResultIQueryable(IQueryable<Track> baseQuery)
        {
      IQueryable<TrackResult> results = (from myData in baseQuery orderby myData.Id select new TrackResult { Id= myData.Id,
            CodeCampYearId = myData.CodeCampYearId,
            OwnerAttendeeId = myData.OwnerAttendeeId,
            Named = myData.Named,
            Description = myData.Description,
            Visible = myData.Visible,
            AlternateURL = myData.AlternateURL,
            TrackImage = myData.TrackImage,
            CreationDate = myData.CreationDate == null ? null :  (DateTime?) new DateTime(myData.CreationDate.Value.Ticks,DateTimeKind.Utc),
            ModifiedDate = myData.ModifiedDate == null ? null :  (DateTime?) new DateTime(myData.ModifiedDate.Value.Ticks,DateTimeKind.Utc)
      });
		    return results;
        }
        
        public List<TrackResult> GetJustBaseTableColumns(TrackQuery query)
        {
            foreach (var info in typeof (TrackQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: Track QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<Track> baseQuery = from myData in meta.Track select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<TrackResult> results = (from myData in baseQuery orderby myData.Id select new TrackResult { Id= myData.Id,
                        CodeCampYearId = myData.CodeCampYearId,
                        OwnerAttendeeId = myData.OwnerAttendeeId,
                        Named = myData.Named,
                        Description = myData.Description,
                        Visible = myData.Visible,
                        AlternateURL = myData.AlternateURL,
                        TrackImage = myData.TrackImage,
                        CreationDate = myData.CreationDate == null ? null :  (DateTime?) new DateTime(myData.CreationDate.Value.Ticks,DateTimeKind.Utc),
                        ModifiedDate = myData.ModifiedDate == null ? null :  (DateTime?) new DateTime(myData.ModifiedDate.Value.Ticks,DateTimeKind.Utc)
            });
            
            List<TrackResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<Track> BaseQueryAutoGen(IQueryable<Track> baseQuery, TrackQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.CodeCampYearId != null) baseQuery = baseQuery.Where(a => a.CodeCampYearId == query.CodeCampYearId);
            if (query.OwnerAttendeeId != null) baseQuery = baseQuery.Where(a => a.OwnerAttendeeId == query.OwnerAttendeeId);
            if (query.Named != null) baseQuery = baseQuery.Where(a => a.Named.ToLower().Equals(query.Named.ToLower()));
            if (query.Description != null) baseQuery = baseQuery.Where(a => a.Description.ToLower().Equals(query.Description.ToLower()));
            if (query.Visible != null) baseQuery = baseQuery.Where(a => a.Visible == query.Visible);
            if (query.AlternateURL != null) baseQuery = baseQuery.Where(a => a.AlternateURL.ToLower().Equals(query.AlternateURL.ToLower()));
            if (query.CreationDate != null) baseQuery = baseQuery.Where(a => a.CreationDate.Value.CompareTo(query.CreationDate.Value) == 0);
            if (query.ModifiedDate != null) baseQuery = baseQuery.Where(a => a.ModifiedDate.Value.CompareTo(query.ModifiedDate.Value) == 0);

            return baseQuery;
        }
        
    }
}
