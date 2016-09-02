using Autofac;
using SpotiKat.Bootstrappers;
using SpotiKat.Lastfm.Configuration;
using SpotiKat.Lastfm.Interfaces;

//ncrunch: no coverage start

namespace SpotiKat.Lastfm.Bootstrappers {
    public class LastfmBootstrapper : IBootstrapper {
        public void RegisterDependencies(ContainerBuilder builder) {
            builder.RegisterType<ArtistService>().As<IArtistService>().InstancePerDependency();
            builder.RegisterType<UrlBuilder>().As<IUrlBuilder>().InstancePerDependency();
            builder.RegisterType<LastfmConfiguration>().As<ILastfmConfiguration>().SingleInstance();
        }
    }
}

//ncrunch: no coverage end