using BL.FormModels;
using BL.ViewModels;

namespace BL.Auth
{
    public interface IAuthService
    {
        Task<ResultViewModel> Authentication(LoginForm login);
    }
}