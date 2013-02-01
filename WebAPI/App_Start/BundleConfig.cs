using System.Web;
using System.Web.Optimization;

namespace WebAPI
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            // this allows the css to render for some reason I don't understand yet.
            // on azure, the EnableOptimizations seems to be true even though release does not set that
            BundleTable.EnableOptimizations = false;

      //      bundles.Add(new StyleBundle("~/Content/Styles").Include("~/Content/Styles/svcc.css"));

      //      bundles.Add(new StyleBundle("~/Content/Styles").Include("~/Content/kendo/2012.3.1114/kendo.common.min.css"));


       
          //  bundles.Add(new StyleBundle("~/Content/kendocommon").Include("~/Content/kendo/2012.3.1114/kendo.common.min.css"));


            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //      "~/Scripts/jquery-1.8.2.js"));


            //bundles.Add(new StyleBundle("~/Content/kendodefault").Include("~/Content/kendo/2012.3.1114/kendo.default.min.css"));

           

            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //    "~/Scripts/kendo/2012.3.1114/jquery.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/kendouiweb").Include(
            //   "~/Scripts/kendo/2012.3.1114/kendo.web.min.js"));

           

         


        }
    }
}