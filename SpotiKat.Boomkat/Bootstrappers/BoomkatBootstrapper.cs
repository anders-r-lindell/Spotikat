using Autofac;
using SpotiKat.Boomkat.Configuration;
using SpotiKat.Boomkat.HtmlParser;
using SpotiKat.Boomkat.Interfaces;
using SpotiKat.Boomkat.Interfaces.Configuration;
using SpotiKat.Boomkat.Interfaces.HtmlParser;
using SpotiKat.Bootstrappers;

//ncrunch: no coverage start

namespace SpotiKat.Boomkat.Bootstrappers {
    public class BoomkatBootstrapper : IBootstrapper {
        public void RegisterDependencies(ContainerBuilder builder) {
            builder.RegisterType<BoomkatFeedItemService>().As<IBoomkatFeedItemService>().InstancePerDependency();
            builder.RegisterType<LastAlbumsFeedItemHtmlParser>()
                .As<ILastAlbumsFeedItemHtmlParser>()
                .InstancePerDependency();
            builder.RegisterType<AlbumsFeedItemHtmlParser>().As<IAlbumsFeedItemHtmlParser>().InstancePerDependency();
            builder.RegisterType<UrlBuilder>().As<IUrlBuilder>().InstancePerDependency();
            builder.RegisterType<BoomkatConfiguration>().As<IBoomkatConfiguration>().SingleInstance();
        }
    }
}

//ncrunch: no coverage end