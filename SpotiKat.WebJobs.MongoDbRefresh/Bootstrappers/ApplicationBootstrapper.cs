using Autofac;
using Bootstrap.Autofac;
using SpotiKat.Boomkat.Bootstrappers;
using SpotiKat.Bootstrappers;
using SpotiKat.Caching.Bootstrappers;
using SpotiKat.MongoDb;
using SpotiKat.MongoDb.Bootstrappers;
using SpotiKat.NewRelic.Bootstrappers;
using SpotiKat.Sbwr.Bootstrappers;
using SpotiKat.Services.Bootstrappers;
using SpotiKat.Spotify.Bootstrappers;

//ncrunch: no coverage start

namespace SpotiKat.WebJobs.MongoDbRefresh.Bootstrappers {
    public class ApplicationBootstrapper : IAutofacRegistration {
        public void Register(ContainerBuilder builder) {
            new SpotiKatBootstrapper().RegisterDependencies(builder);
            new SpotifyBootstrapper().RegisterDependencies(builder);
            new BoomkatBootstrapper().RegisterDependencies(builder);
            new SbwrBootstrapper().RegisterDependencies(builder);
            new ServicesBootstrapper().RegisterDependencies(builder);
            new MongoDbBootstrapper().RegisterDependencies(builder);
            new NewRelicBootstrapper().RegisterDependencies(builder);

            new CachingBootstrapper().RegisterDependencies(builder);

            new BsonClassMap().Register();
        }
    }
}

//ncrunch: no coverage end