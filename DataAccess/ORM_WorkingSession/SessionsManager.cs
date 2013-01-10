using System;
using System.Collections.Generic;
using System.Linq;
using CodeCampSV;
using System.ComponentModel;

namespace CodeCampSV
{
    public partial class SessionsManager
    {
        //  public override void Insert(LoadResult result)
        //  {
        //     base.Insert(result);
        //     if (result.Cargos != null && result.Cargos.Count > 0)
        //     {
        //        foreach (CargoResult c in result.Cargos)
        //        {
        //            c.LoadId = result.Id;
        //            CargoManager.I.Insert(c);
        //         }
        //      }
        //  }
        // 
        //  public override void Update(LoadResult result)
        //  {
        //      base.Update(result);
        //      if (result.Cargos != null && result.Cargos.Count > 0)
        //      {
        //          CargoManager.I.Update(result.Cargos);
        //      }
        //  }

        public List<SessionsResult> Get(SessionsQuery query)
        {
            var meta = new CodeCampDataContext();
            IQueryable<Sessions> baseQuery = from myData in meta.Sessions select myData;
            baseQuery = BaseQueryAutoGen(baseQuery,query,false);
            IQueryable<SessionsResult> results = (from myData in baseQuery orderby myData.Id select new SessionsResult { Id= myData.Id,
                        SessionLevel_id = myData.SessionLevel_id,
                        Username = myData.Username,
                        Title = myData.Title,
                        Description = myData.Description,
                        Approved = myData.Approved,
                        Createdate = myData.Createdate == null ? null :  (DateTime?) new DateTime(myData.Createdate.Value.Ticks,DateTimeKind.Utc),
                        Updatedate = myData.Updatedate == null ? null :  (DateTime?) new DateTime(myData.Updatedate.Value.Ticks,DateTimeKind.Utc),
                        AdminComments = myData.AdminComments,
                        InterentAccessRequired = myData.InterentAccessRequired,
                        LectureRoomsId = myData.LectureRoomsId,
                        SessionTimesId = myData.SessionTimesId
            });
            
            List<SessionsResult> resultList = GetFinalResults(results, query);
            return resultList;  
        }
        
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<SessionsResult> GetAll()
        {
            return Get(new SessionsQuery {IsMaterializeResult = true});
        }
    }
}
