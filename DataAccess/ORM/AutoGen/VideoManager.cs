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
    public partial class VideoManager : ManagerBase<VideoManager, VideoResult, Video, CodeCampDataContext>
    {
        protected override void ApplyToDataModel(Video record, VideoResult result)
        {
            record.YouTubeURL = result.YouTubeURL;
            record.DescriptionText = result.DescriptionText;
            record.PictureBytes = result.PictureBytes;
            record.CreatedDate = result.CreatedDate;
            // 
            //  Used by Default in Update and Insert Methods.
        }

        protected override Video GetEntityById(CodeCampDataContext meta, int id)
        {
            return (from r in meta.Video where r.Id == id select r).FirstOrDefault();
        }

  public IQueryable<VideoResult> GetBaseResultIQueryable(IQueryable<Video> baseQuery)
        {
      IQueryable<VideoResult> results = (from myData in baseQuery orderby myData.Id select new VideoResult { Id= myData.Id,
            YouTubeURL = myData.YouTubeURL,
            DescriptionText = myData.DescriptionText,
            PictureBytes = myData.PictureBytes,
            CreatedDate = myData.CreatedDate == null ? null :  (DateTime?) new DateTime(myData.CreatedDate.Value.Ticks,DateTimeKind.Utc)
      });
		    return results;
        }
        
        public List<VideoResult> GetJustBaseTableColumns(VideoQuery query)
        {
            foreach (var info in typeof (VideoQuery).GetProperties())
            {
                object value = info.GetValue(query, null);
                if (value != null)
                {
                    object[] attributes = info.GetCustomAttributes(typeof (AutoGenColumnAttribute), true);
                    if (attributes.Length == 0)
                    {
                        string errorMessage = String.Format("Attribute Illegal Here, Use Normal Get(..), not GetJustBaseTableColumns(..)  Table: Video QueryColumnProblem: {0}",info.Name);
                        throw new ApplicationException(errorMessage);
                    }
                }
            }
        

            var meta = new CodeCampDataContext();

            IQueryable<Video> baseQuery = from myData in meta.Video select myData;
            
            baseQuery = BaseQueryAutoGen(baseQuery,query);
            
            IQueryable<VideoResult> results = (from myData in baseQuery orderby myData.Id select new VideoResult { Id= myData.Id,
                        YouTubeURL = myData.YouTubeURL,
                        DescriptionText = myData.DescriptionText,
                        PictureBytes = myData.PictureBytes,
                        CreatedDate = myData.CreatedDate == null ? null :  (DateTime?) new DateTime(myData.CreatedDate.Value.Ticks,DateTimeKind.Utc)
            });
            
            List<VideoResult> resultList = GetFinalResults(results, query);
            // 
            return resultList;  
        }
        
        
        //  This is called from partial class which can be modified and not regenerated
        //  This class is expected to be regenerated as new columns are added
        private static IQueryable<Video> BaseQueryAutoGen(IQueryable<Video> baseQuery, VideoQuery query)
        {
			//  This assumes all tables have an Id column
            if (query.Id != null) baseQuery = baseQuery.Where(a => a.Id == query.Id);
            if (query.Ids != null) baseQuery = baseQuery.Where(a => query.Ids.Contains(a.Id));
            
            //  Generate Queries for Each type of data
            if (query.YouTubeURL != null) baseQuery = baseQuery.Where(a => a.YouTubeURL.ToLower().Equals(query.YouTubeURL.ToLower()));
            if (query.DescriptionText != null) baseQuery = baseQuery.Where(a => a.DescriptionText.ToLower().Equals(query.DescriptionText.ToLower()));
            if (query.CreatedDate != null) baseQuery = baseQuery.Where(a => a.CreatedDate.Value.CompareTo(query.CreatedDate.Value) == 0);

            return baseQuery;
        }
        
    }
}
