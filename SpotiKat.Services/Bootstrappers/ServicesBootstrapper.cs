using Autofac;
using SpotiKat.Bootstrappers;
using SpotiKat.Services.Interfaces;

//ncrunch: no coverage start

namespace SpotiKat.Services.Bootstrappers {
    public class ServicesBootstrapper : IBootstrapper {
        public void RegisterDependencies(ContainerBuilder builder) {
            builder.RegisterType<LastAlbumService>().As<ILastAlbumService>().InstancePerDependency();
            builder.RegisterType<AlbumService>().As<IAlbumService>().InstancePerDependency();
            builder.RegisterType<SpotifyAlbumService>().As<ISpotifyAlbumService>().InstancePerDependency();
            builder.RegisterType<SpotifyService>().As<ISpotifyService>().InstancePerDependency();
            builder.RegisterType<GenreService>().As<IGenreService>().SingleInstance();
        }
    }
}

//ncrunch: no coverage end