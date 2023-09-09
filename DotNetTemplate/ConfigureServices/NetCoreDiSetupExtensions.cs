using NetCore.AutoRegisterDi;
using System.Reflection;

namespace DotNetTemplate.ConfigureServices
{
    public static class NetCoreDiSetupExtensions
    {
        public static void RegisterServiceLayerDi(this IServiceCollection services)
        {
            var refer = Assembly.GetEntryAssembly()?.GetReferencedAssemblies().First(x => x.Name == "BL");

            if (refer == null)
                throw new ArgumentException(nameof(refer));

            var assembliesToScan = new[]
            {
              Assembly.GetExecutingAssembly(),
              Assembly.Load(refer),
            };

            // Auto Register Implemented Interfaces with Name End with "Service"
            services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
            .Where(c => c.Name.EndsWith("Service"))
            .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);
        }
    }
}