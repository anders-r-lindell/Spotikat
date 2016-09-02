using Autofac;
using SpotiKat.Bootstrappers;
using SpotiKat.Spotify.Configuration;
using SpotiKat.Spotify.Interfaces;
using SpotiKat.Spotify.Interfaces.Configuration;

//ncrunch: no coverage start

namespace SpotiKat.Spotify.Bootstrappers {
    public class SpotifyBootstrapper : IBootstrapper {
        public void RegisterDependencies(ContainerBuilder builder) {
            builder.RegisterType<SearchService>().As<ISearchService>().InstancePerDependency();
            builder.RegisterType<UrlBuilder>().As<IUrlBuilder>().InstancePerDependency();
            builder.RegisterType<SearchQueryEncoder>().As<ISearchQueryEncoder>().InstancePerDependency();
            builder.RegisterType<SpotifyConfiguration>().As<ISpotifyConfiguration>().SingleInstance();
        }
    }
}

//ncrunch: no coverage end