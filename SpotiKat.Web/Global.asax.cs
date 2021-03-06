﻿using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Bootstrap;
using Bootstrap.Autofac;
using SpotiKat.Web.Configuration;

namespace SpotiKat.Web {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.ico");
            routes.IgnoreRoute("robots.txt");

            GlobalConfiguration.Configure(c => c.MapHttpAttributeRoutes());

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}", // URL with parameters
                new {controller = "Default", action = "Index"} // Parameter defaults
                );
        }

        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            BundleConfiguration.RegisterBundles(BundleTable.Bundles);

            Bootstrapper.With.Autofac().Start();
        }
    }
}