using Finances.Facade;
using Finances.Module;
using Finances.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Finances
{
    public static class Dependencies
    {
        public static IServiceCollection GetDependencies()
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();

            return new ServiceCollection()
                    .AddTransient<IConstant, Constant>(c => new Constant(configuration))

                    // Module
                    .AddTransient<IBillModule, BillModule>()
                    .AddTransient<IScheduleModule, ScheduleModule>()

                    // Facade
                    .AddTransient<IBillFacade, BillFacade>()
                    .AddTransient<IScheduleFacade, ScheduleFacade>()

                    // Service
                    .AddTransient<ISqlService, SqlService>()

                    // Windows
                    .AddTransient<IBillManager, BillManager>()
                    .AddTransient<IScheduleManager, ScheduleManager>()
                    .AddTransient<IScheduleBill, ScheduleBill>()
            ;
        }
    }
}