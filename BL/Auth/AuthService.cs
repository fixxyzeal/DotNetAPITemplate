using BL.FormModels;
using BL.ViewModels;
using DAL.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace BL.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ResultViewModel> Authentication(LoginForm login)
        {
            var result = new ResultViewModel();

            // Authenticate Logic
            if (string.IsNullOrWhiteSpace(login.Username) || string.IsNullOrEmpty(login.Password))
            {
                result.HttpStatusCode = (int)HttpStatusCode.BadRequest;
                result.Message = "Username or Password is Empty";
                return result;
            }

            // Validate Email & Password
            var user = await _userRepository.GetUserByUserNameAsync(login.Username);

            if (user != null)
            {
                if (login.Password != user.PassWord)
                {
                    result.HttpStatusCode = (int)HttpStatusCode.Unauthorized;
                    result.Message = "Password Not Match";
                    return result;
                }
                // Create JWT Token
                AuthResponse authResponse = new();

                var tokenHandler = new JwtSecurityTokenHandler();
                byte[] key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWTKEY") ?? string.Empty);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Sid, user.UserName ?? string.Empty),
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
                    Issuer = "DotNetAPITemplate",
                };

                result.Success = true;
                result.HttpStatusCode = (int)HttpStatusCode.OK;
                authResponse.UserName = user.UserName;
                authResponse.Email = user.Email;
                authResponse.Token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

                result.Data = authResponse;
            }
            else
            {
                result.HttpStatusCode = (int)HttpStatusCode.Unauthorized;
                result.Message = "Cannot Get UserData From Database";
                return result;
            }

            return result;
        }
    }
}