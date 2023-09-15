using DAL.Models;

namespace DAL.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByUserNameAsync(string username);
    }
}