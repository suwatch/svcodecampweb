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
    public partial class SessionPicturesManager : ManagerBase<SessionPicturesManager, SessionPicturesResult, SessionPictures, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(SessionPictures record, SessionPicturesResult result)
        {
            record.PictureId = result.PictureId;
            record.SessionId = result.SessionId;
            record.AttendeePKID = result.AttendeePKID;
            record.AssignedDate = result.AssignedDate;
            record.Description = result.Description;
            record.DefaultPicture = result.DefaultPicture;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override SessionPictures GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.SessionPictures where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<SessionPicturesResult> GetBaseResultIQueryable(IQueryable<SessionPictures> baseQuery)
        {
      IQueryable<SessionPicturesResult> results = (from myData in baseQuery orderby myData.Id select new SessionPicturesResult { Id= myData.Id,
            PictureId = myData.PictureId,
            SessionId = myData.SessionId,
            AttendeePKID = myData.AttendeePKID,
            AssignedDate = myData.AssignedDate == null ? null :  (DateTime?) new DateTime(myData.AssignedDate.Value.Ticks,DateTimeKind.Utc),
            Description = myData.Description,
            DefaultPicture = myData.DefaultPicture
      });
		    return results;
        }
        
        public List<SessionPicturesResult> GetJustBaseTableColumns(SessionPicturesQuery query)
        {
            foreach (var info in typeof (SessionPicturesQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: SessionPictures QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<SessionPictures> baseQuery = from myData in meta.SessionPictures select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<SessionPicturesResult> results = (from myData in baseQuery orderby myData.Id select new SessionPicturesResult { Id= myData.Id,
                        PictureId = myData.PictureId,
                        SessionId = myData.SessionId,
                        AttendeePKID = myData.AttendeePKID,
                        AssignedDate = myData.AssignedDate == null ? null :  (DateTime?) new DateTime(myData.AssignedDate.Value.Ticks,DateTimeKind.Utc),
                        Description = myData.Description,
                        DefaultPicture = myData.DefaultPicture
            });
            
            List<SessionPicturesResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<SessionPictures> BaseQueryAutoGen(IQueryable<SessionPictures> baseQuery, SessionPicturesQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.PictureId != null) baseQuery = baseQuery.Where(a => a.PictureId == query.PictureId);
            if (query.SessionId != null) baseQuery = baseQuery.Where(a => a.SessionId == query.SessionId);
            if (query.AssignedDate != null) baseQuery = baseQuery.Where(a => a.AssignedDate.Value.CompareTo(query.AssignedDate.Value) == 0);
            if (query.Description != null) baseQuery = baseQuery.Where(a => a.Description.ToLower().Equals(query.Description.ToLower()));
            if (query.DefaultPicture != null) baseQuery = baseQuery.Where(a => a.DefaultPicture == query.DefaultPicture);

            return baseQuery;
        }
        
    }
}
