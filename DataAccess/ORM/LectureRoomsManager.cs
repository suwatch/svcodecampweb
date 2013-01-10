using System;
using System.Collections.Generic;
using System.Linq;
using CodeCampSV;
using System.ComponentModel;

namespace CodeCampSV
{
    public partial class LectureRoomsManager
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<LectureRoomsResult> GetAvailableRooms()
        {
            return Get(new LectureRoomsQuery() { Available = true }).OrderBy(a => a.Number).ToList();
        }
       
        public List<LectureRoomsResult> Get(LectureRoomsQuery query)
        {
            var meta = new CodeCampDataContext();

            IQueryable<LectureRooms> baseQuery = from myData in meta.LectureRooms select myData;
            
            //  next is automated query formation from AutoGen Shared Class 
            //  (do not remove next line or filters will go away)
            baseQuery = BaseQueryAutoGen(baseQuery,query);

            if (query.SkipIds != null && query.SkipIds.Count() > 0)
            {
                baseQuery = from data in baseQuery
                            where !query.SkipIds.Contains(data.Id)
                            select data;
            }

            IQueryable<LectureRoomsResult> results = GetBaseResultIQueryable(baseQuery);

            
            List<LectureRoomsResult> resultList = GetFinalResults(results, query);

            foreach (var rec in resultList)
            {
                if (!String.IsNullOrEmpty(rec.Number) && rec.Capacity.HasValue)
                {
                    rec.RoomNumberWithCapacity = String.Format("{0}, Holds {1}", rec.Number, rec.Capacity.Value.ToString());
                }
                else
                {
                    rec.RoomNumberWithCapacity = rec.Number ?? "";
                }
            }

           
            return resultList;  
        }
        
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<LectureRoomsResult> GetAll()
        {
            return Get(new LectureRoomsQuery { IsMaterializeResult = true }).OrderBy(a => a.Number).ToList();
        }
    }
}
