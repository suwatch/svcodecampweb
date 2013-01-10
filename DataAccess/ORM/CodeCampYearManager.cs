using System;
using System.Collections.Generic;
using System.Linq;
using CodeCampSV;
using System.ComponentModel;

namespace CodeCampSV
{
    public partial class CodeCampYearManager
    {
     
        public List<CodeCampYearResult> Get(CodeCampYearQuery query)
        {
            var meta = new CodeCampDataContext();

            IQueryable<CodeCampYear> baseQuery = from myData in meta.CodeCampYear select myData;

            //  (do not remove next line or filters will go away)
            baseQuery = BaseQueryAutoGen(baseQuery, query);

            if (query.OnlyYearsWithSessions != null && query.OnlyYearsWithSessions.Value)
            {
                var codeCampYearsWithSessions = (from data in meta.Sessions
                                                 select data.CodeCampYearId).Distinct().ToList();
                baseQuery = baseQuery.Where(a => codeCampYearsWithSessions.Contains(a.Id));
            }

            IQueryable<CodeCampYearResult> results = GetBaseResultIQueryable(baseQuery);
            List<CodeCampYearResult> resultList = GetFinalResults(results, query);

            if (query.WithSessionDetail != null && query.WithSessionDetail.Value)
            {
                // could optimize this based on what years are selected
                var codeCampYearIds = results.Select(a => a.Id).ToList();
                var sessionResults = SessionsManager.I.Get(new SessionsQuery
                                                               {
                                                                   CodeCampYearIds = codeCampYearIds,
                                                                   WithTags = query.WithTags,
                                                                   WithEvaluations = query.WithEvaluations,
                                                                   WithLectureRoom = query.WithLectureRoom,
                                                                   WithLevel = query.WithLevel,
                                                                   WithSchedule = query.WithSchedule,
                                                                   WithSpeakers = query.WithSpeakers
                                                               });


                foreach (var rec in resultList)
                {
                    rec.SessionResults = sessionResults.Where(data => data.CodeCampYearId == rec.Id).ToList();
                }
            }


            //if (query.WithSpeakers != null && query.WithSpeakers.Value)
            //{
            //    // could optimize this based on what years are selected
            //    var codeCampYearIds = results.Select(a => a.Id).ToList();
            //    var attendeesResults = AttendeesManager.I.Get(new AttendeesQuery
            //    {
            //        CodeCampYearIds = codeCampYearIds,
            //        PresentersOnly = true
            //    });


            //    foreach (var rec in resultList)
            //    {
            //        rec.SpeakerResults = attendeesResults.Where(data => data.Id == rec.Id).ToList();
            //    }
            //}

            //if (query.WithSchedule != null && query.WithSchedule.Value)
            //{
            //    //// could optimize this based on what years are selected
            //    //var codeCampYearIds = results.Select(a => a.Id).ToList();
            //    //var sessionResults = SessionsManager.I.Get(new SessionsQuery
            //    //{
            //    //    CodeCampYearIds = codeCampYearIds,
            //    //    WithTags = true
            //    //});


            //    //foreach (var rec in resultList)
            //    //{
            //    //    rec.SessionResults = sessionResults.Where(data => data.CodeCampYearId == rec.Id).ToList();
            //    //}
            //}







            return resultList;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<CodeCampYearResult> GetAll()
        {
            return Get(new CodeCampYearQuery {IsMaterializeResult = true});
        }
    }
}
