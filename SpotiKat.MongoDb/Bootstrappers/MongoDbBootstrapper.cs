using Autofac;
using SpotiKat.Bootstrappers;
using SpotiKat.MongoDb.Configuration;
using SpotiKat.MongoDb.Interfaces;
using SpotiKat.MongoDb.Interfaces.Configuration;

//ncrunch: no coverage start

namespace SpotiKat.MongoDb.Bootstrappers {
    public class MongoDbBootstrapper : IBootstrapper {
        public void RegisterDependencies(ContainerBuilder builder) {
            builder.RegisterType<MongoDbFactory>().As<IMongoDbFactory>().SingleInstance();
            builder.RegisterType<MongoDbConfiguration>().As<IMongoDbConfiguration>().SingleInstance();
        }
    }
}

//ncrunch: no coverage end