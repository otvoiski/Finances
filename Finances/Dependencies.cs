using Finances.Facade;
using Finances.Module;
using Finances.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finances
{
    public static class Dependencies
    {
        public static ServiceProvider GetServiceProvider()
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();

            return new ServiceCollection()
                    .AddTransient<IConstant, Constant>(c => new Constant(configuration))
                    .AddTransient<IBillFacade, BillFacade>()
                    .AddTransient<ISqlService, SqlService>()
                    .AddTransient<IBillModule, BillModule>()
                    .AddTransient<IBillManager, BillManager>()
                .BuildServiceProvider();
        }
    }
}