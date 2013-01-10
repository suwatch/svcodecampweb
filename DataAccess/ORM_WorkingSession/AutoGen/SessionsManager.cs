//  This is the Manager class used for data operations.  It is meant to have another Partial
//  class associated with it.
//  C 3PLogic, Inc.
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

using CodeCampSV;
using System.Reflection;


namespace CodeCampSV
{
    //  Here are the 2 methods that needs to be auto genearted. 
    //  First is a one to one maping to the database columns. 
    //  Since we auto generate the results class too, we can guarantee the columns are all there
    [DataObject(true)]
    public partial class SessionsManager : ManagerBase<SessionsManager, SessionsResult, Sessions, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(Sessions record, SessionsResult result)
        {
            record.SessionLevel_id = result.SessionLevel_id ?? record.SessionLevel_id;
            record.Username = result.Username ?? record.Username;
            record.Title = result.Title ?? record.Title;
            record.Description = result.Description ?? record.Description;
            record.Approved = result.Approved ?? record.Approved;
            record.Createdate = result.Createdate ?? record.Createdate;
            record.Updatedate = result.Updatedate ?? record.Updatedate;
            record.AdminComments = result.AdminComments ?? record.AdminComments;
            record.InterentAccessRequired = result.InterentAccessRequired ?? record.InterentAccessRequired;
            record.LectureRoomsId = result.LectureRoomsId ?? record.LectureRoomsId;
            record.SessionTimesId = result.SessionTimesId ?? record.SessionTimesId;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        public List<SessionsResult> GetJustBaseTableColumns(SessionsQuery query)
        {
            
            foreach (var info in typeof (SessionsQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        throw new ApplicationException("attribute illegal here: " + info.Name);
                    }
                }
            }

            var meta = new CodeCampDataContext();
            IQueryable<Sessions> baseQuery = from myData in meta.Sessions select myData;
            baseQuery = BaseQueryAutoGen(baseQuery, query,true);
            IQueryable<SessionsResult> results = (from myData in baseQuery
                                                  orderby myData.Id
                                                  select new SessionsResult
                                                  {
                                                      Id = myData.Id,
                                                      SessionLevel_id = myData.SessionLevel_id,
                                                      Username = myData.Username,
                                                      Title = myData.Title,
                                                      Description = myData.Description,
                                                      Approved = myData.Approved,
                                                      Createdate = myData.Createdate == null ? null : (DateTime?)new DateTime(myData.Createdate.Value.Ticks, DateTimeKind.Utc),
                                                      Updatedate = myData.Updatedate == null ? null : (DateTime?)new DateTime(myData.Updatedate.Value.Ticks, DateTimeKind.Utc),
                                                      AdminComments = myData.AdminComments,
                                                      InterentAccessRequired = myData.InterentAccessRequired,
                                                      LectureRoomsId = myData.LectureRoomsId,
                                                      SessionTimesId = myData.SessionTimesId
                                                  });

            List<SessionsResult> resultList = GetFinalResults(results, query);
            return resultList;
        }

        protected override Sessions GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.Sessions where r.Id == id select r).FirstOrDefault();
        }
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<Sessions> BaseQueryAutoGen(IQueryable<Sessions> baseQuery, SessionsQuery query,bool checkQueryAttribute)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.SessionLevel_id != null)
            {
                //if (checkQueryAttribute && !HasAttribe(typeof (SessionsQuery), "SessionLevel_id"))
                //{
                //    throw new ApplicationException("Attribute " + "SessionLevel_id" );
                //}

                //if ((checkQueryAttribute && HasAttribe(typeof (SessionsQuery), "SessionLevel_id")) ||
                //    !checkQueryAttribute)
                //{
                    baseQuery = baseQuery.Where(a => a.SessionLevel_id == query.SessionLevel_id);
                //}
            }
            if (query.Username != null) baseQuery = baseQuery.Where(a => a.Username.ToLower().Equals(query.Username.ToLower()));
            if (query.title != null) baseQuery = baseQuery.Where(a => a.Title.ToLower().Equals(query.title.ToLower()));
            if (query.description != null) baseQuery = baseQuery.Where(a => a.Description.ToLower().Equals(query.description.ToLower()));
            if (query.approved != null) baseQuery = baseQuery.Where(a => a.Approved == query.approved);
            if (query.createdate != null) baseQuery = baseQuery.Where(a => a.Createdate.Value.CompareTo(query.createdate.Value) == 0);
            if (query.updatedate != null) baseQuery = baseQuery.Where(a => a.Updatedate.Value.CompareTo(query.updatedate.Value) == 0);
            if (query.AdminComments != null) baseQuery = baseQuery.Where(a => a.AdminComments.ToLower().Equals(query.AdminComments.ToLower()));
            if (query.InterentAccessRequired != null) baseQuery = baseQuery.Where(a => a.InterentAccessRequired == query.InterentAccessRequired);
            if (query.LectureRoomsId != null) baseQuery = baseQuery.Where(a => a.LectureRoomsId == query.LectureRoomsId);
            if (query.SessionTimesId != null) baseQuery = baseQuery.Where(a => a.SessionTimesId == query.SessionTimesId);

            return baseQuery;
        }

        private static bool HasAttribe(Type type, string propertyName)
        {
            var attributes = type.GetProperty(propertyName).GetCustomAttributes(typeof(AutoGenColumnAttribute), true);
            return attributes.Length > 0;
        }
        
    }
}
