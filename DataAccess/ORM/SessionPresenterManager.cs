using System;
using System.Collections.Generic;
using System.Linq;
using CodeCampSV;
using System.ComponentModel;

namespace CodeCampSV
{
    public partial class SessionPresenterManager
    {
        public List<SessionPresenterResult> Get(SessionPresenterQuery query)
        {
            var meta = new CodeCampDataContext();

            // add to codecampyearids
            if (query.CodeCampYearId.HasValue)
            {
                if (query.CodeCampYearIds != null && query.CodeCampYearIds.Count > 0)
                {
                    if (!query.CodeCampYearIds.Contains(query.CodeCampYearId.Value))
                    {
                        query.CodeCampYearIds.Add(query.CodeCampYearId.Value);
                    }
                }
                else
                {
                    query.CodeCampYearIds = new List<int> { query.CodeCampYearId.Value };
                }
            }

            IQueryable<SessionPresenter> baseQuery = from myData in meta.SessionPresenter select myData;

            if (query.CodeCampYearIds != null && query.CodeCampYearIds.Count > 0)
            {
                // sessionpresenter Ids for this year codecamp
                var sessionPresenterIds = (from data in meta.SessionPresenter
                                           join data1 in meta.Sessions on data.SessionId equals data1.Id
                                           where query.CodeCampYearIds.Contains(data1.CodeCampYearId)
                                           select data.Id).ToList();
                baseQuery = baseQuery.Where(data => sessionPresenterIds.Contains(data.Id));
            }

            //  next is automated query formation from AutoGen Shared Class 
            //  (do not remove next line or filters will go away)
            baseQuery = BaseQueryAutoGen(baseQuery, query);

            IQueryable<SessionPresenterResult> results = GetBaseResultIQueryable(baseQuery);
            List<SessionPresenterResult> resultList = GetFinalResults(results, query);
            if (query.WithTitle.HasValue && query.WithTitle.Value)
            {
                var sessions = (from data in meta.Sessions
                                where resultList.Select(a => a.SessionId).Contains(data.Id)
                                select data);
                foreach (var sessionPresenter in resultList)
                {
                    var session = sessions.FirstOrDefault(a => a.Id == sessionPresenter.SessionId);
                    if (session != null)
                    {
                        sessionPresenter.Title = session.Title;
                        sessionPresenter.Description = session.Description;
                    }
                }
            }

            if (query.WithSpeaker.HasValue && query.WithSpeaker.Value)
            {
                var speakerDict = (AttendeesManager.I.Get(new AttendeesQuery()
                                                              {
                                                                  Ids = results.Select(a => a.AttendeeId).ToList()
                                                              })).ToDictionary(k => k.Id, v => v);
                foreach (var result in results)
                {
                    result.Presenter = speakerDict.ContainsKey(result.AttendeeId)
                                           ? speakerDict[result.AttendeeId]
                                           : new AttendeesResult();
                }
            }


            return resultList;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<SessionPresenterResult> GetAll()
        {
            return Get(new SessionPresenterQuery {IsMaterializeResult = true});
        }
    }
}
