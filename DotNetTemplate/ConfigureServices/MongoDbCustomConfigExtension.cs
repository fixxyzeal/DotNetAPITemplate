using MongoDB.Driver;

namespace DotNetTemplate.ConfigureServices
{
    public static class MongoDbCustomConfigExtension
    {
        public static void ConfigMongoDb(this IServiceCollection services)
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MongoConnection")))
            {
                throw new ArgumentNullException(Environment.GetEnvironmentVariable("MongoConnection"), "Missing MongoDB ConnectionString Please Set in .ENV File or Environment Variable");
            }

            // Add MongoDB Connection with Singleton Life time

            services.AddSingleton<IMongoClient>(c =>
            {
                return new MongoClient(Environment.GetEnvironmentVariable("MongoConnection") ?? string.Empty);
            });
        }
    }
}