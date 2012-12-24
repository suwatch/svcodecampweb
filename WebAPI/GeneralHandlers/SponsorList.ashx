<%@ WebHandler Language="C#" Class="SponsorList" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CodeCampSV;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class SponsorList : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        int codeCampYearId = Utils.CurrentCodeCampYear;
       
        if (HttpContext.Current.Request["codecampyear"] != null)
        {
            Int32.TryParse(HttpContext.Current.Request["codecampyear"], out codeCampYearId);
        }


        var sponsors = SponsorListManager.I.Get(new SponsorListQuery()
                                                    {
                                                        CodeCampYearId = codeCampYearId,
                                                        IncludeSponsorLevel = true,
                                                        PlatinumLevel = Utils.MinSponsorLevelPlatinum,
                                                        GoldLevel = Utils.MinSponsorLevelGold,
                                                        SilverLevel = Utils.MinSponsorLevelSilver,
                                                        BronzeLevel = Utils.MinSponsorLevelBronze,
                                                        Visible = true
                                                    });




        var listDataResults = sponsors.Select(a => new
                                                       {
                                                           a.Id,
                                                           a.NavigateURL,
                                                           a.SponsorName,
                                                           a.SponsorSupportLevel,
                                                           a.SponsorSupportLevelOrder,
                                                           a.CompanyCity,
                                                           a.CompanyState,
                                                           a.CompanyZip,
                                                           a.CompanyDescriptionLong,
                                                           a.CompanyDescriptionShort,
                                                       }).OrderBy(a => a.SponsorSupportLevelOrder).ToList();


        var ret = new { success = true, rows = listDataResults, total = listDataResults.Count };
        context.Response.ContentType = "application/json";
        HttpContext.Current.Response.Write(ret.ToJson());
    }

  

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

   
    
 

  
}
