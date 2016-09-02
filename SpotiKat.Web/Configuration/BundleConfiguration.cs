using System.Web.Optimization;

namespace SpotiKat.Web.Configuration {
    public class BundleConfiguration {
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new ScriptBundle("~/Scripts/js").Include(
                "~/assets/plugins/jquery/jquery.min.js",
                "~/assets/plugins/bootstrap/js/bootstrap.min.js",
                "~/assets/plugins/back-to-top.js",
                "~/assets/plugins/smoothScroll.js",
                "~/assets/plugins/cube-portfolio/cubeportfolio/js/jquery.cubeportfolio.min.js",
                "~/assets/js/app.js")
                );

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/assets/plugins/bootstrap/css/bootstrap.min.css",
                "~/assets/css/ie8.css",
                "~/assets/css/blocks.css",
                "~/assets/css/plugins.css",
                "~/assets/css/app.css",
                "~/assets/css/style.css",
                "~/assets/css/headers/header-default.css",
                "~/assets/css/footers/footer-v1.css",
                "~/assets/plugins/font-awesome/css/font-awesome.min.css",
                "~/assets/plugins/cube-portfolio/cubeportfolio/css/cubeportfolio.min.css",
                "~/assets/plugins/cube-portfolio/cubeportfolio/custom/custom-cubeportfolio.css",
                "~/assets/css/custom.css"));
        }
    }
}