using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeCampSV;


namespace WebAPI
{
    public partial class SponsorsList : System.Web.UI.UserControl
    {
        public bool ShowPictures { get; set; }

        protected void Page_Init(object sender, EventArgs e)
        {
            string sql = @"
            SELECT  COALESCE (ImageURL, '') AS ImageURL,
                    COALESCE (NavigateURL, '') AS NavigateURL,
                    COALESCE (HoverOverText, '') AS HoverOverText,
                    COALESCE (UnderLogoText, '') AS UnderLogoText,
                   dbo.SponsorList.Comment,
                   dbo.SponsorList.Id,
                   COALESCE (CompanyImageType, '') AS CompanyImageType
            FROM dbo.SponsorListCodeCampYear
                 INNER JOIN dbo.SponsorList ON (dbo.SponsorListCodeCampYear.SponsorListId =
                 dbo.SponsorList.Id)
            where dbo.SponsorListCodeCampYear.Visible = 1 AND
                  (DonationAmount BETWEEN {0} AND
                  {1}) AND
                  CodeCampYearId = @CodeCampYearId
            ORDER BY dbo.SponsorListCodeCampYear.SortIndex,
                     dbo.SponsorList.HoverOverText,
                     dbo.SponsorList.id
        ";

            LabelCodeCampYearId.Text = Utils.GetCurrentCodeCampYear().ToString(CultureInfo.InvariantCulture);

            int cacheTimeSeconds = Utils.RetrieveSecondsForSessionCacheTimeout();

            SqlDataSourceSponsorsBronze.CacheDuration = cacheTimeSeconds;
            SqlDataSourceSponsorsCommunity.CacheDuration = cacheTimeSeconds;
            SqlDataSourceSponsorsGold.CacheDuration = cacheTimeSeconds;
            SqlDataSourceSponsorsPlatinum.CacheDuration = cacheTimeSeconds;
            SqlDataSourceSponsorsSilver.CacheDuration = cacheTimeSeconds;




            SqlDataSourceSponsorsPlatinum.SelectCommand = String.Format(sql, Utils.MinSponsorLevelPlatinum, 99999.00);
            SqlDataSourceSponsorsGold.SelectCommand = String.Format(sql, Utils.MinSponsorLevelGold,
                                                                    Utils.MinSponsorLevelPlatinum - .01);
            SqlDataSourceSponsorsSilver.SelectCommand = String.Format(sql, Utils.MinSponsorLevelSilver,
                                                                      Utils.MinSponsorLevelGold - .01);
            SqlDataSourceSponsorsBronze.SelectCommand = String.Format(sql, Utils.MinSponsorLevelBronze,
                                                                      Utils.MinSponsorLevelSilver - .01);
            SqlDataSourceSponsorsCommunity.SelectCommand = String.Format(sql, 0.00, 1.00);


        }

        protected bool ShowImageIfGood(string url)
        {
            bool showImage = false;
            if (Utils.GetCurrentCodeCampYear() >= 5)
            {
                if (ShowPictures)
                {
                    if (url.StartsWith("~") || String.IsNullOrEmpty(url))
                    {
                        showImage = true;
                    }
                }
            }
            return showImage;
        }

        protected string GetJobURLonclick(string hoverOverName)
        {
            const string outboundLinkTemplate = "javascript:pageTracker._trackPageview ('/outbound/ADS-{0}-{1}');";
            string outboundLink = String.Format(outboundLinkTemplate, "SponsorAd", hoverOverName);

            return outboundLink;
        }

        protected string GetSpriteLocation(string oldLocation, int sponsorId, string companyImageType,
                                           string sponsorType) //,string companyImageType)
        {
            string retUrl;

            int imageSize = 130;
            if (sponsorType.Equals("Platinum"))
            {
                imageSize = 160;
            }

            if (String.IsNullOrEmpty(oldLocation))
            {
                if (companyImageType == "psd")
                {
                    const string template =
                        "/sponsorimage/{0}.psd?format=gif&layerColors=&layerVisibility=&layerRedraw=&layerText=&renderer=psdplugin&w={1}&h={2}&scale=both&mode=pad&bgcolor=white";
                    retUrl = String.Format(template, sponsorId, imageSize, imageSize);
                }
                else
                {
                    const string template =
                        "/sponsorimage/{0}.jpg?format=gif&w={1}&h={2}&scale=both&mode=pad&bgcolor=white";
                    retUrl = String.Format(template, sponsorId, imageSize, imageSize);
                }

            }
            else
            {
                // old path:  "~/Images/Sponsors/apprenda-logo.jpg"
                // new path:  "~/App_Sprites/Images/Sponsors/apprenda-logo.jpg"
                retUrl = oldLocation;
                if (oldLocation.StartsWith("~/Images/Sponsors"))
                {
                    retUrl = oldLocation.Replace("~/Images/Sponsors", "~/App_Sprites/Sponsors");
                }
            }

            return retUrl;
        }
    }
}