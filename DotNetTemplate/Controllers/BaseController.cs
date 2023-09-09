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
        protected IActionResult GenerateResponse(object data, bool isSuccess = true, string message = "")
        {
            var response = new
            {
                Success = isSuccess,
                Message = message,
                Data = data
            };

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