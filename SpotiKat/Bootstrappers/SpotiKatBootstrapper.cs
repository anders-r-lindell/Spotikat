using Autofac;
using SpotiKat.Configuration;
using SpotiKat.Interfaces.Configuration;
using SpotiKat.Interfaces.Logging;
using SpotiKat.Interfaces.Net.Http;
using SpotiKat.Logging;
using SpotiKat.Net.Http;

//ncrunch: no coverage start

namespace SpotiKat.Bootstrappers {
    public class SpotiKatBootstrapper : IBootstrapper {
        public void RegisterDependencies(ContainerBuilder builder) {
            builder.RegisterType<JsonServiceClient>().As<IJsonServiceClient>().InstancePerDependency();
            builder.RegisterType<WebClient>().As<IWebClient>().InstancePerDependency();
            builder.RegisterType<JsonServiceClientConfiguration>()
                .As<IJsonServiceClientConfiguration>().SingleInstance();
            builder.RegisterType<GenresConfiguration>().As<IGenresConfiguration>().SingleInstance();
            builder.RegisterType<NullLogFactory>().As<ILogFactory>().SingleInstance();
        }
    }
}

//ncrunch: no coverage end