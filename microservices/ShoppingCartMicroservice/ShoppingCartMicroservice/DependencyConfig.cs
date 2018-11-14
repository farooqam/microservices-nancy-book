using Autofac;
using AzureFunctions.Autofac.Configuration;
using Techniqly.Microservices.Abstractions;

namespace Techniqly.Microservices
{
    public class DependencyConfig
    {
        public DependencyConfig(string functionName)
        {
            DependencyInjection.Initialize(builder =>
                {
                    builder.RegisterType<InMemoryCartRepository>().As<ICartRepository>().SingleInstance();
                    builder.RegisterType<StringArrayRequestConverter>().As<IRequestConverter<string[]>>();

                },
                functionName);
        }
    }
}
