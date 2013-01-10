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
    public partial class SessionEvalsManager : ManagerBase<SessionEvalsManager, SessionEvalsResult, SessionEvals, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(SessionEvals record, SessionEvalsResult result)
        {
            record.SessionId = result.SessionId;
            record.PKID = result.PKID;
            record.CreateDate = result.CreateDate;
            record.UpdateDate = result.UpdateDate;
            record.CourseAsWhole = result.CourseAsWhole;
            record.CourseContent = result.CourseContent;
            record.InstructorEff = result.InstructorEff;
            record.InstructorAbilityExplain = result.InstructorAbilityExplain;
            record.InstructorEffective = result.InstructorEffective;
            record.InstructorKnowledge = result.InstructorKnowledge;
            record.QualityOfFacility = result.QualityOfFacility;
            record.OverallCodeCamp = result.OverallCodeCamp;
            record.ContentLevel = result.ContentLevel;
            record.Favorite = result.Favorite;
            record.Improved = result.Improved;
            record.GeneralComments = result.GeneralComments;
            record.DiscloseEval = result.DiscloseEval;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override SessionEvals GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.SessionEvals where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<SessionEvalsResult> GetBaseResultIQueryable(IQueryable<SessionEvals> baseQuery)
        {
      IQueryable<SessionEvalsResult> results = (from myData in baseQuery orderby myData.Id select new SessionEvalsResult { Id= myData.Id,
            SessionId = myData.SessionId,
            PKID = myData.PKID,
            CreateDate = myData.CreateDate == null ? null :  (DateTime?) new DateTime(myData.CreateDate.Value.Ticks,DateTimeKind.Utc),
            UpdateDate = myData.UpdateDate == null ? null :  (DateTime?) new DateTime(myData.UpdateDate.Value.Ticks,DateTimeKind.Utc),
            CourseAsWhole = myData.CourseAsWhole,
            CourseContent = myData.CourseContent,
            InstructorEff = myData.InstructorEff,
            InstructorAbilityExplain = myData.InstructorAbilityExplain,
            InstructorEffective = myData.InstructorEffective,
            InstructorKnowledge = myData.InstructorKnowledge,
            QualityOfFacility = myData.QualityOfFacility,
            OverallCodeCamp = myData.OverallCodeCamp,
            ContentLevel = myData.ContentLevel,
            Favorite = myData.Favorite,
            Improved = myData.Improved,
            GeneralComments = myData.GeneralComments,
            DiscloseEval = myData.DiscloseEval
      });
		    return results;
        }
        
        public List<SessionEvalsResult> GetJustBaseTableColumns(SessionEvalsQuery query)
        {
            foreach (var info in typeof (SessionEvalsQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: SessionEvals QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<SessionEvals> baseQuery = from myData in meta.SessionEvals select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<SessionEvalsResult> results = (from myData in baseQuery orderby myData.Id select new SessionEvalsResult { Id= myData.Id,
                        SessionId = myData.SessionId,
                        PKID = myData.PKID,
                        CreateDate = myData.CreateDate == null ? null :  (DateTime?) new DateTime(myData.CreateDate.Value.Ticks,DateTimeKind.Utc),
                        UpdateDate = myData.UpdateDate == null ? null :  (DateTime?) new DateTime(myData.UpdateDate.Value.Ticks,DateTimeKind.Utc),
                        CourseAsWhole = myData.CourseAsWhole,
                        CourseContent = myData.CourseContent,
                        InstructorEff = myData.InstructorEff,
                        InstructorAbilityExplain = myData.InstructorAbilityExplain,
                        InstructorEffective = myData.InstructorEffective,
                        InstructorKnowledge = myData.InstructorKnowledge,
                        QualityOfFacility = myData.QualityOfFacility,
                        OverallCodeCamp = myData.OverallCodeCamp,
                        ContentLevel = myData.ContentLevel,
                        Favorite = myData.Favorite,
                        Improved = myData.Improved,
                        GeneralComments = myData.GeneralComments,
                        DiscloseEval = myData.DiscloseEval
            });
            
            List<SessionEvalsResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<SessionEvals> BaseQueryAutoGen(IQueryable<SessionEvals> baseQuery, SessionEvalsQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.SessionId != null) baseQuery = baseQuery.Where(a => a.SessionId == query.SessionId);
            if (query.CreateDate != null) baseQuery = baseQuery.Where(a => a.CreateDate.Value.CompareTo(query.CreateDate.Value) == 0);
            if (query.UpdateDate != null) baseQuery = baseQuery.Where(a => a.UpdateDate.Value.CompareTo(query.UpdateDate.Value) == 0);
            if (query.CourseAsWhole != null) baseQuery = baseQuery.Where(a => a.CourseAsWhole == query.CourseAsWhole);
            if (query.CourseContent != null) baseQuery = baseQuery.Where(a => a.CourseContent == query.CourseContent);
            if (query.InstructorEff != null) baseQuery = baseQuery.Where(a => a.InstructorEff == query.InstructorEff);
            if (query.InstructorAbilityExplain != null) baseQuery = baseQuery.Where(a => a.InstructorAbilityExplain == query.InstructorAbilityExplain);
            if (query.InstructorEffective != null) baseQuery = baseQuery.Where(a => a.InstructorEffective == query.InstructorEffective);
            if (query.InstructorKnowledge != null) baseQuery = baseQuery.Where(a => a.InstructorKnowledge == query.InstructorKnowledge);
            if (query.QualityOfFacility != null) baseQuery = baseQuery.Where(a => a.QualityOfFacility == query.QualityOfFacility);
            if (query.OverallCodeCamp != null) baseQuery = baseQuery.Where(a => a.OverallCodeCamp == query.OverallCodeCamp);
            if (query.ContentLevel != null) baseQuery = baseQuery.Where(a => a.ContentLevel == query.ContentLevel);
            if (query.Favorite != null) baseQuery = baseQuery.Where(a => a.Favorite.ToLower().Equals(query.Favorite.ToLower()));
            if (query.Improved != null) baseQuery = baseQuery.Where(a => a.Improved.ToLower().Equals(query.Improved.ToLower()));
            if (query.GeneralComments != null) baseQuery = baseQuery.Where(a => a.GeneralComments.ToLower().Equals(query.GeneralComments.ToLower()));
            if (query.DiscloseEval != null) baseQuery = baseQuery.Where(a => a.DiscloseEval == query.DiscloseEval);

            return baseQuery;
        }
        
    }
}
