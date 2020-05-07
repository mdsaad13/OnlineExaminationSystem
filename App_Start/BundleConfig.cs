using System.Web;
using System.Web.Optimization;

namespace OnlineExaminationSystem
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/js").Include(
                        "~/Scripts/vendor.bundle.base.js",
                        "~/Scripts/main.js"));

            bundles.Add(new StyleBundle("~/css").Include(
                      "~/Content/vendor.bundle.base.css",
                      "~/Content/style.css"));
        }
    }
}
