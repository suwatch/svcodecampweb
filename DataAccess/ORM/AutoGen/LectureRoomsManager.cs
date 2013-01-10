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
    public partial class LectureRoomsManager : ManagerBase<LectureRoomsManager, LectureRoomsResult, LectureRooms, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(LectureRooms record, LectureRoomsResult result)
        {
            record.Number = result.Number;
            record.Description = result.Description;
            record.Style = result.Style;
            record.Capacity = result.Capacity;
            record.Projector = result.Projector;
            record.Screen = result.Screen;
            record.Picture = result.Picture;
            record.Available = result.Available;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override LectureRooms GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.LectureRooms where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<LectureRoomsResult> GetBaseResultIQueryable(IQueryable<LectureRooms> baseQuery)
        {
      IQueryable<LectureRoomsResult> results = (from myData in baseQuery orderby myData.Id select new LectureRoomsResult { Id= myData.Id,
            Number = myData.Number,
            Description = myData.Description,
            Style = myData.Style,
            Capacity = myData.Capacity,
            Projector = myData.Projector,
            Screen = myData.Screen,
            Picture = myData.Picture,
            Available = myData.Available
      });
		    return results;
        }
        
        public List<LectureRoomsResult> GetJustBaseTableColumns(LectureRoomsQuery query)
        {
            foreach (var info in typeof (LectureRoomsQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: LectureRooms QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<LectureRooms> baseQuery = from myData in meta.LectureRooms select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<LectureRoomsResult> results = (from myData in baseQuery orderby myData.Id select new LectureRoomsResult { Id= myData.Id,
                        Number = myData.Number,
                        Description = myData.Description,
                        Style = myData.Style,
                        Capacity = myData.Capacity,
                        Projector = myData.Projector,
                        Screen = myData.Screen,
                        Picture = myData.Picture,
                        Available = myData.Available
            });
            
            List<LectureRoomsResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<LectureRooms> BaseQueryAutoGen(IQueryable<LectureRooms> baseQuery, LectureRoomsQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.Number != null) baseQuery = baseQuery.Where(a => a.Number.ToLower().Equals(query.Number.ToLower()));
            if (query.Description != null) baseQuery = baseQuery.Where(a => a.Description.ToLower().Equals(query.Description.ToLower()));
            if (query.Style != null) baseQuery = baseQuery.Where(a => a.Style.ToLower().Equals(query.Style.ToLower()));
            if (query.Capacity != null) baseQuery = baseQuery.Where(a => a.Capacity == query.Capacity);
            if (query.Projector != null) baseQuery = baseQuery.Where(a => a.Projector == query.Projector);
            if (query.Screen != null) baseQuery = baseQuery.Where(a => a.Screen == query.Screen);
            if (query.Available != null) baseQuery = baseQuery.Where(a => a.Available == query.Available);

            return baseQuery;
        }
        
    }
}
