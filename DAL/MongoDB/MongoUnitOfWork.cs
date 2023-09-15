using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL.MongoDB
{
    public class MongoUnitOfWork<T> where T : class
    {
        private readonly IMongoCollection<T> mongo;

        public MongoUnitOfWork(IMongoClient mongoClient, string database, string collection)
        {
            var db = mongoClient.GetDatabase(database);
            var filter = new BsonDocument("name", collection);
            //filter by collection name
            var collections = db.ListCollections(new ListCollectionsOptions { Filter = filter });
            //check for existence
            if (!collections.Any())
            {
                db.CreateCollection(collection);
            }
            mongo = db.GetCollection<T>(collection);
        }

        public async Task<IEnumerable<T>> GetAllAsync(FilterDefinition<T> filterDefinition)
        {
            return await mongo.Find<T>(filterDefinition).ToListAsync();
        }

        public Task<T> GetAsync(FilterDefinition<T> filterDefinition)
        {
            return mongo.Find<T>(filterDefinition).FirstOrDefaultAsync();
        }

        public Task InsertOneAsync(T model)
        {
            return mongo.InsertOneAsync(model);
        }

        public Task InsertManyAsync(IEnumerable<T> model)
        {
            return mongo.InsertManyAsync(model);
        }

        public Task UpdateOneAsync(FilterDefinition<T> filterDefinition, T entity)
        {
            return mongo.ReplaceOneAsync(filterDefinition, entity);
        }

        public Task UpdateManyAsync(FilterDefinition<T> filterDefinition, UpdateDefinition<T> updateDefinition)
        {
            return mongo.UpdateManyAsync(filterDefinition, updateDefinition);
        }

        public Task DeleteOneAsync(FilterDefinition<T> filterDefinition)
        {
            return mongo.DeleteOneAsync(filterDefinition);
        }

        public Task DeleteManyAsync(FilterDefinition<T> filterDefinition)
        {
            return mongo.DeleteManyAsync(filterDefinition);
        }
    }
}