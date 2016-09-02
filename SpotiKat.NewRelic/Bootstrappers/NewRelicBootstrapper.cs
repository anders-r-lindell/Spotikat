using Autofac;
using SpotiKat.Bootstrappers;
using SpotiKat.NewRelic.Interfaces;

//ncrunch: no coverage start

namespace SpotiKat.NewRelic.Bootstrappers {
    public class NewRelicBootstrapper : IBootstrapper {
        public void RegisterDependencies(ContainerBuilder builder) {
            builder.RegisterType<NewRelicTransactionManager>().As<INewRelicTransactionManager>().InstancePerDependency();
        }
    }
}

//ncrunch: no coverage end