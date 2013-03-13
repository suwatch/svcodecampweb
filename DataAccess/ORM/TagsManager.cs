using System;
using System.Collections.Generic;
using System.Linq;
using CodeCampSV;
using System.ComponentModel;

namespace CodeCampSV
{
    public partial class TagsManager
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

        public List<TagsResult> Get(TagsQuery query)
        {

            var meta = new CodeCampDataContext();

            IQueryable<Tags> baseQuery = from myData in meta.Tags select myData;


            //  next is automated query formation from AutoGen Shared Class 
            //  (do not remove next line or filters will go away)
            baseQuery = BaseQueryAutoGen(baseQuery, query);

              if (query.CodeCampYearId.HasValue)
              {
                  var tagsIds =
                      meta.SessionTagsView.Where(data => data.CodeCampYearId == query.CodeCampYearId)
                          .Select(data => data.TagId);
                  baseQuery = baseQuery.Where(data => tagsIds.Contains(data.Id));
              }

            List<int> tagIdsTagged = null;
            if (query.SessionId.HasValue)
            {
                tagIdsTagged = (from data in meta.SessionTags
                                        where data.SessionId == query.SessionId.Value && data.SessionId.HasValue
                                        select data.TagId).ToList();
            }
            IQueryable<TagsResult> results = GetBaseResultIQueryable(baseQuery);
            List<TagsResult> resultList = GetFinalResults(results, query);


            // query.SessionId may be true but if not tags for session, this will not execute
            if (tagIdsTagged != null && tagIdsTagged.Count > 0)
            {
                foreach (var rec in resultList)
                {
                    rec.TaggedInSession = tagIdsTagged.Contains(rec.Id);
                }
            }

            if (query.SessionId.HasValue)
            {
                foreach (var rec in resultList)
                {
                    rec.SessionId = query.SessionId.Value;
                }
            }
            return resultList;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<TagsResult> GetAll()
        {
            return Get(new TagsQuery {IsMaterializeResult = true});
        }
    }
}
