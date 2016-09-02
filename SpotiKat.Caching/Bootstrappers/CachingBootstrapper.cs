using Autofac;
using SpotiKat.Boomkat;
using SpotiKat.Boomkat.Interfaces;
using SpotiKat.Boomkat.Interfaces.Configuration;
using SpotiKat.Boomkat.Interfaces.HtmlParser;
using SpotiKat.Bootstrappers;
using SpotiKat.Caching.Configuration;
using SpotiKat.Caching.Interfaces;
using SpotiKat.Caching.Interfaces.Configuration;
using SpotiKat.Caching.MongoDb;
using SpotiKat.Caching.Proxy;
using SpotiKat.Caching.Proxy.Boomkat;
using SpotiKat.Caching.Proxy.Spotify;
using SpotiKat.Interfaces.Caching;
using SpotiKat.Interfaces.Logging;
using SpotiKat.Interfaces.Net.Http;
using SpotiKat.MongoDb.Interfaces;
using SpotiKat.NewRelic.Interfaces;
using SpotiKat.Sbwr.interfaces.Configuration;
using SpotiKat.Sbwr.Interfaces;
using SpotiKat.Services;
using SpotiKat.Services.Interfaces;
using SpotiKat.Spotify;
using SpotiKat.Spotify.Interfaces;
using IUrlBuilder = SpotiKat.Spotify.Interfaces.IUrlBuilder;

//ncrunch: no coverage start

namespace SpotiKat.Caching.Bootstrappers {
    public class CachingBootstrapper : IBootstrapper {
        public void RegisterDependencies(ContainerBuilder builder) {
            builder.RegisterType<CacheService>().As<ICacheService>().InstancePerDependency();

            builder.RegisterType<SpotifyCacheConfiguration>().As<ISpotifyCacheConfiguration>().SingleInstance();

            builder.Register(c => new SearchServiceCacheProxy(
                new SearchService(
                    c.Resolve<IUrlBuilder>(),
                    c.Resolve<IJsonServiceClient>(),
                    c.Resolve<ILogFactory>()
                    ),
                c.Resolve<ISpotifyCacheConfiguration>(),
                c.Resolve<ICacheService>(new TypedParameter(typeof (ICache), new HttpRuntimeCache()))
                )).As<ISearchService>().InstancePerDependency();

            builder.RegisterType<BoomkatCacheConfiguration>().As<IBoomkatCacheConfiguration>().SingleInstance();

            builder.Register(c => new BoomkatFeedItemServiceCacheProxy(
                new BoomkatFeedItemService(
                    c.Resolve<ILastAlbumsFeedItemHtmlParser>(),
                    c.Resolve<IAlbumsFeedItemHtmlParser>(),
                    c.Resolve<Boomkat.Interfaces.IUrlBuilder>(),
                    c.Resolve<ILogFactory>(),
                    c.Resolve<IWebClient>(),
                    c.Resolve<IBoomkatConfiguration>()
                    ),
                c.Resolve<IBoomkatCacheConfiguration>(),
                c.Resolve<ICacheService>(new TypedParameter(typeof (ICache), new HttpRuntimeCache()))
                )).As<IBoomkatFeedItemService>().InstancePerDependency();

            builder.RegisterType<SpotiKatCacheConfiguration>().As<ISpotiKatCacheConfiguration>().SingleInstance();

            builder.Register(c => new LastAlbumsServiceCacheProxy(
                new LastAlbumService(
                    c.Resolve<IBoomkatFeedItemService>(),
                    c.Resolve<ISbwrFeedItemService>(),
                    c.Resolve<ISpotifyService>(),
                    c.Resolve<ISbwrConfiguration>(),
                    c.Resolve<INewRelicTransactionManager>()
                    ),
                c.Resolve<ISpotiKatCacheConfiguration>(),
                c.Resolve<ICacheService>(new TypedParameter(typeof (ICache),
                    new MongoDbCache(c.Resolve<IMongoDbFactory>())))
                )).As<ILastAlbumService>().InstancePerDependency();
        }
    }
}

//ncrunch: no coverage end