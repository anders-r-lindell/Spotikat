using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using SpotiKat.Bootstrappers;

namespace SpotiKat.Api.ServiceInterface.Bootstrappers {
    public class ServiceInterfaceBootstrapper : IBootstrapper {
        public void RegisterDependencies(ContainerBuilder builder) {
            builder.RegisterApiControllers(Assembly.GetAssembly(typeof (ServiceInterfaceBootstrapper)));
        }
    }
}