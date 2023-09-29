using NetCore.AutoRegisterDi;
using System.Reflection;

namespace DotNetTemplate.ConfigureServices
{
    public static class NetCoreDiSetupExtensions
    {
        public static void RegisterServiceLayerDi(this IServiceCollection services)
        {
            var loadedAssemblies = Assembly.GetEntryAssembly()?.GetReferencedAssemblies().ToList();

            // Get Business Logic Library Assembly
            var blRefer = loadedAssemblies?.First(x => x.Name == "BL");

            // Get Data Access Layer Assembly
            var dalRefer = loadedAssemblies?.First(x => x.Name == "DAL");

            if (blRefer == null)
                throw new ArgumentException(nameof(blRefer));
            if (dalRefer == null)
                throw new ArgumentException(nameof(dalRefer));

            var assembliesToScan = new[]
            {
              Assembly.GetExecutingAssembly(),
              Assembly.Load(blRefer),
              Assembly.Load(dalRefer)
            };

            // Auto Register Scoped Implemented Interfaces with Name End with "Service"
            services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
                .Where(c => c.Name.EndsWith("Service"))
                .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

            // Auto Register Singleton Implemented Interfaces with Name End with "Respository"
            services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
              .Where(c => c.Name.EndsWith("Repository"))
              .AsPublicImplementedInterfaces(ServiceLifetime.Singleton);
        }
    }
}