using BL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DotNetTemplate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {
        public BaseController()
        {
        }

        // Function to generate dynamic response
        protected IActionResult GenerateResponse(ResultViewModel response)
        {
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}