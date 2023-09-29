using BL.Auth;
using BL.FormModels;
using BL.ViewModels;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace DotNetTemplate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginForm loginForm)
        {
            return GenerateResponse(await _authService.Authentication(loginForm));
        }

        [Authorize]
        [HttpPost]
        [Route("getclaim")]
        public IActionResult GetClaimInformation()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            IEnumerable<Claim> claims = identity.Claims;

            return GenerateResponse(new ResultViewModel() { HttpStatusCode = (int)HttpStatusCode.OK, Message = "OK", Success = true, Data = new User() { UserName = claims.First(x => x.Type == ClaimTypes.Sid).Value } });
        }
    }
}