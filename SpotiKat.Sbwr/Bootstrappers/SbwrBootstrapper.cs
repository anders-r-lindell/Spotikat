using Autofac;
using SpotiKat.Bootstrappers;
using SpotiKat.Sbwr.Configuration;
using SpotiKat.Sbwr.HtmlParser;
using SpotiKat.Sbwr.interfaces.Configuration;
using SpotiKat.Sbwr.Interfaces;
using SpotiKat.Sbwr.Interfaces.HtmlParser;

//ncrunch: no coverage start

namespace SpotiKat.Sbwr.Bootstrappers {
    public class SbwrBootstrapper : IBootstrapper {
        public void RegisterDependencies(ContainerBuilder builder) {
            builder.RegisterType<SbwrFeedItemService>().As<ISbwrFeedItemService>().InstancePerDependency();
            builder.RegisterType<FeedItemHtmlParser>().As<IFeedItemHtmlParser>().InstancePerDependency();
            builder.RegisterType<UrlBuilder>().As<IUrlBuilder>().InstancePerDependency();
            builder.RegisterType<SbwrConfiguration>().As<ISbwrConfiguration>().SingleInstance();
        }
    }
}

//ncrunch: no coverage end