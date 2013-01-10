using System;
using System.Collections.Generic;
using System.Linq;
using CodeCampSV;
using System.ComponentModel;
using System.Web;

namespace CodeCampSV
{
    public partial class SessionAttendeeManager
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

        //public List<SessionAttendeeResult> GetByAttendeePKIDCached(Guid userGuid,int codeCampYearId,int cacheTimeOutSeconds)
        //{
        //    string cacheName = "SessionAttendeeResult-GetByAttendeePKID-" + " codeCampYearId.ToString();
        //    var sessionAttendeeResults = HttpContext.Current.Cache[cacheName] as List<SessionAttendeeResult>;
        //    if (sessionAttendeeResults == null)
        //    {



                
        //        HttpContext.Current.Cache.Insert(cacheName, numberRegistered ?? 0,
        //                                        null,
        //                                        DateTime.Now.Add(new TimeSpan(0, 0, cacheTimeOutSeconds)),
        //                                        TimeSpan.Zero);
        //    }


        //    return sessionAttendeeResults;
        //}


        public List<SessionAttendeeResult> Get(SessionAttendeeQuery query)
        {

            var meta = new CodeCampDataContext();

            IQueryable<SessionAttendee> baseQuery = from myData in meta.SessionAttendee select myData;
            
           
            //  next is automated query formation from AutoGen Shared Class 
            //  (do not remove next line or filters will go away)
            baseQuery = BaseQueryAutoGen(baseQuery,query);


            if (query.Attendees_username != null)
            {
                baseQuery = baseQuery.Where(a => a.Attendees_username == query.Attendees_username.Value);
            }


            if (!String.IsNullOrEmpty(query.EventBoardEmail))
            {
                // might be better to do this in one step
                var attendeeUsernamePKID = (from data in meta.Attendees
                                            where data.EmailEventBoard.Equals(query.EventBoardEmail)
                                            select data.PKID).FirstOrDefault();
                baseQuery = baseQuery.Where(a => a.Attendees_username == attendeeUsernamePKID);
            }

            if (query.CodeCampYearId != null)
            {
                var sessionIds = from data in meta.Sessions
                                 where data.CodeCampYearId == query.CodeCampYearId.Value
                                 select data.Id;
                baseQuery = baseQuery.Where(a => sessionIds.Contains(a.Sessions_id));
            }


            IQueryable<SessionAttendeeResult> results = GetBaseResultIQueryable(baseQuery);

            
            List<SessionAttendeeResult> resultList = GetFinalResults(results, query);
            
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
        
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public List<SessionAttendeeResult> GetAll()
        {
            return Get(new SessionAttendeeQuery {IsMaterializeResult = true});
        }
    }
}
