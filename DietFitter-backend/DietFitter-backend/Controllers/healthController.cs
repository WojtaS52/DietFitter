using Microsoft.AspNetCore.Mvc;

namespace DietFitter_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet("calculate")]
        public IActionResult CalculateBMI(double weight, double height)
        {
            if (height <= 0)
            {
                return BadRequest(new { Error = "Height must be greater than 0" });
            }

            double bmi = weight / (height * height);

            string result = bmi switch
            {
                < 18.5 => "Underweight",
                >= 18.5 and < 25 => "Normal",
                >= 25 and < 30 => "Overweight",
                _ => "Obese"
            };

            return Ok(new
            {
                Bmi = Math.Round(bmi, 2),
                Result = result
            });
        }
    }
}