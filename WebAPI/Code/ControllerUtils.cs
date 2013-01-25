using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeCampSV;

namespace WebAPI.Code
{
    public class ControllerUtils
    {
        public static List<SponsorListResult> AllSponsors(int codeCampYearId)
        {
            List<SponsorListResult> sponsors =
                SponsorListManager.I.Get(new SponsorListQuery
                {
                    CodeCampYearId = codeCampYearId,
                    IncludeSponsorLevel = true,
                    PlatinumLevel = Utils.MinSponsorLevelPlatinum,
                    GoldLevel = Utils.MinSponsorLevelGold,
                    SilverLevel = Utils.MinSponsorLevelSilver,
                    BronzeLevel = Utils.MinSponsorLevelBronze
                });
            return sponsors;
        }
    }
}