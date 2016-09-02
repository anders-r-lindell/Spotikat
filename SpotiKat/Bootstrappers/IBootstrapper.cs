using Autofac;

namespace SpotiKat.Bootstrappers {
    public interface IBootstrapper {
        void RegisterDependencies(ContainerBuilder container);
    }
}