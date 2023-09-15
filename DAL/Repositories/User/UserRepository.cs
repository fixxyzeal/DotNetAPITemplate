using DAL.Models;
using DAL.MongoDB;
using MongoDB.Driver;

namespace DAL.Repositories
{
    public class UserRepository : MongoUnitOfWork<User>, IUserRepository
    {
        public UserRepository(IMongoClient client) : base(client, "fsdatabase", "user")
        {
        }

        public Task<User> GetUserByUserNameAsync(string username)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(p => p.UserName, username);

            return GetAsync(filter);
        }
    }
}