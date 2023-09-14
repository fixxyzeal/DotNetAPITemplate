using BL.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetTemplate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : BaseController
    {
        private readonly IAesCryptographyService _aesCryptographyService;

        public AuthController(IAesCryptographyService aesCryptographyService)
        {
            _aesCryptographyService = aesCryptographyService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login()
        {
            return GenerateResponse(await _aesCryptographyService.EncryptAsync("test"), true, "test");
        }

        [Authorize]
        [HttpPost]
        [Route("getclaim")]
        public async Task<IActionResult> GetClaimInformation()
        {
            return GenerateResponse(await _aesCryptographyService.DecryptAsync(new byte[] { }), true, "test");
        }
    }
}