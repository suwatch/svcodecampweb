using System;
using System.Collections.Generic;
using System.Linq;
using CodeCampSV;
using System.ComponentModel;
using Gurock.SmartInspect.LinqToSql;

namespace CodeCampSV
{
    public partial class AttendeesCodeCampYearManager
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

        public void Upsert(AttendeesCodeCampYearResult result)
        {
            var meta = new CodeCampDataContext();

            var attendeesCodeCampYear = (from data in meta.AttendeesCodeCampYear
                                         where data.CodeCampYearId == result.CodeCampYearId && data.AttendeesId == result.AttendeesId
                                         select data).SingleOrDefault();

            if (attendeesCodeCampYear != null)
            {
                attendeesCodeCampYear.AttendSaturday = result.AttendSaturday;
                attendeesCodeCampYear.AttendSunday = result.AttendSunday;
            }
            else
            {
                attendeesCodeCampYear = new AttendeesCodeCampYear()
                {
                    AttendeesId = result.AttendeesId,
                    CodeCampYearId = result.CodeCampYearId,
                    AttendSaturday = result.AttendSaturday,
                    AttendSunday = result.AttendSunday
                };
                meta.AttendeesCodeCampYear.InsertOnSubmit(attendeesCodeCampYear);
            }
            meta.SubmitChanges();
        }

        public List<AttendeesCodeCampYearResult> Get(AttendeesCodeCampYearQuery query)
        {

            var meta = new CodeCampDataContext();

            //meta.Log = new SmartInspectLinqToSqlAdapter();

            IQueryable<AttendeesCodeCampYear> baseQuery = from myData in meta.AttendeesCodeCampYear select myData;
            
           
            //  next is automated query formation from AutoGen Shared Class 
            //  (do not remove next line or filters will go away)
            baseQuery = BaseQueryAutoGen(baseQuery,query);

            if (query.RegisterDateStart != null)
            {
                baseQuery = baseQuery.Where(a => a.CreateDate.Value.CompareTo(query.RegisterDateStart.Value) >= 0);
            }

            IQueryable<AttendeesCodeCampYearResult> results = GetBaseResultIQueryable(baseQuery);


            
            
            List<AttendeesCodeCampYearResult> resultList = GetFinalResults(results, query);
            
            //  Put Stuff Here if you want to load another result
            //  The following is done AFTER GetFinalResults so that we don't waste machine cycles sucking in all the
            //  addresses for all results returned, just the ones that are actually being returned.
            //  if (query.WithAddress != null && query.WithAddress == true)
            //  {
            //     foreach (var r in companyResultList)
            //     {
            //         r.CompanyAddressResultList =
            //             CompanyAddressManager.I.Get(new CompanyAddressQuery { CompanyIds = query.Ids, WithAddress = true });
            //     }
            //  }
            //             
            return resultList;  
        }

        public List<string> GetUsernameListByCodeCampYearId(int codeCampYearId)
        {
            var meta = new CodeCampDataContext();



            List<string> userNameList = (from data in meta.Attendees
                                         join attendeeccy in meta.AttendeesCodeCampYear on data.Id equals attendeeccy.AttendeesId
                                         where attendeeccy.CodeCampYearId == codeCampYearId
                                         select data.Username).ToList();

            return userNameList;

        }
        
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<AttendeesCodeCampYearResult> GetAll()
        {
            return Get(new AttendeesCodeCampYearQuery {IsMaterializeResult = true});
        }
    }
}
