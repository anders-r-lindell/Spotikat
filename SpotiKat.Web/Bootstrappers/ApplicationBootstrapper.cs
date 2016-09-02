using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Bootstrap;
using Bootstrap.Autofac;
using SpotiKat.Api.ServiceInterface.Bootstrappers;
using SpotiKat.Boomkat.Bootstrappers;
using SpotiKat.Bootstrappers;
using SpotiKat.Caching.Bootstrappers;
using SpotiKat.Interfaces.Logging;
using SpotiKat.MongoDb;
using SpotiKat.MongoDb.Bootstrappers;
using SpotiKat.NewRelic.Bootstrappers;
using SpotiKat.Sbwr.Bootstrappers;
using SpotiKat.Services.Bootstrappers;
using SpotiKat.Spotify.Bootstrappers;
using SpotiKat.Web.Logging;

//ncrunch: no coverage start

namespace SpotiKat.Web.Bootstrappers {
    public class ApplicationBootstrapper : IAutofacRegistration {
        public void Register(ContainerBuilder builder) {
            new SpotiKatBootstrapper().RegisterDependencies(builder);
            new SpotifyBootstrapper().RegisterDependencies(builder);
            new BoomkatBootstrapper().RegisterDependencies(builder);
            new SbwrBootstrapper().RegisterDependencies(builder);
            new ServicesBootstrapper().RegisterDependencies(builder);
            new MongoDbBootstrapper().RegisterDependencies(builder);
            new ServiceInterfaceBootstrapper().RegisterDependencies(builder);
            new NewRelicBootstrapper().RegisterDependencies(builder);

            // This bootstrapper dependencies needs to be registred last
            new CachingBootstrapper().RegisterDependencies(builder);

            builder.RegisterType<NLogFactory>().As<ILogFactory>().SingleInstance();

            builder.RegisterControllers(typeof (MvcApplication).Assembly);

            var container = (IContainer) Bootstrapper.Container;

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            new BsonClassMap().Register();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
    }
}

//ncrunch: no coverage end