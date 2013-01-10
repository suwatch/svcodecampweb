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
    public partial class TagsManager : ManagerBase<TagsManager, TagsResult, Tags, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(Tags record, TagsResult result)
        {
            record.TagName = result.TagName;
            record.TagDescription = result.TagDescription;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override Tags GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.Tags where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<TagsResult> GetBaseResultIQueryable(IQueryable<Tags> baseQuery)
        {
      IQueryable<TagsResult> results = (from myData in baseQuery orderby myData.Id select new TagsResult { Id= myData.Id,
            TagName = myData.TagName,
            TagDescription = myData.TagDescription
      });
		    return results;
        }
        
        public List<TagsResult> GetJustBaseTableColumns(TagsQuery query)
        {
            foreach (var info in typeof (TagsQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: Tags QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<Tags> baseQuery = from myData in meta.Tags select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<TagsResult> results = (from myData in baseQuery orderby myData.Id select new TagsResult { Id= myData.Id,
                        TagName = myData.TagName,
                        TagDescription = myData.TagDescription
            });
            
            List<TagsResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<Tags> BaseQueryAutoGen(IQueryable<Tags> baseQuery, TagsQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.TagName != null) baseQuery = baseQuery.Where(a => a.TagName.ToLower().Equals(query.TagName.ToLower()));
            if (query.TagDescription != null) baseQuery = baseQuery.Where(a => a.TagDescription.ToLower().Equals(query.TagDescription.ToLower()));

            return baseQuery;
        }
        
    }
}
