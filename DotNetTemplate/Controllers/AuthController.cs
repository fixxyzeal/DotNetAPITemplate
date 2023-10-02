using BL.Auth;
using BL.FormModels;
using BL.ViewModels;
using DotNetTemplate.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
            return GenerateResponse(new ResultViewModel() { HttpStatusCode = (int)HttpStatusCode.OK, Message = "OK", Success = true, Data = GetAuthenticatedClaimHelper.GetAuthenticatedClaim(this.HttpContext) });
        }
    }
}