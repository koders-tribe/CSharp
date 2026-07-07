using Microsoft.AspNetCore.Mvc;

namespace StudentManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetWeather()
        {
            var response = new
            {
                message = "Hello from ASP.NET Core!",
                temperature = 25,
                condition = "Sunny"
            };
            
            return Ok(response);
        }
    }
}