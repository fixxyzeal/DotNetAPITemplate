using NetCore.AutoRegisterDi;
using System.Reflection;

namespace DotNetTemplate.ConfigureServices
{
    public static class NetCoreDiSetupExtensions
    {
        public static void RegisterServiceLayerDi(this IServiceCollection services)
        {
            // Get Business Logic Library Assembly
            var blRefer = Assembly.GetEntryAssembly()?.GetReferencedAssemblies().First(x => x.Name == "BL");

            if (blRefer == null)
                throw new ArgumentException(nameof(blRefer));

            var assembliesToScan = new[]
            {
              Assembly.GetExecutingAssembly(),
              Assembly.Load(blRefer)
            };

            // Auto Register Implemented Interfaces with Name End with "Service"
            services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
            .Where(c => c.Name.EndsWith("Service") || c.Name.EndsWith("Repository"))
            .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);
        }
    }
}