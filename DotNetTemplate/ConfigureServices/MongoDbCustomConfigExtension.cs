using DAL.Repositories;
using MongoDB.Driver;

namespace DotNetTemplate.ConfigureServices
{
    public static class MongoDbCustomConfigExtension
    {
        public static void ConfigMongoDb(this IServiceCollection services)
        {
            // Add MongoDB Connection with Singleton Life time

            services.AddSingleton<IMongoClient>(c =>
            {
                return new MongoClient(Environment.GetEnvironmentVariable("MongoConnection") ?? string.Empty);
            });

            // Add Repositories
            services.AddSingleton<IUserRepository, UserRepository>();
        }
    }
}