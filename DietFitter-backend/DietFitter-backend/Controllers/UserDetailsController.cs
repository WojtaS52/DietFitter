using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DietFitter_backend.Database;
using DietFitter_backend.DTO;


namespace DietFitter_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserDetailsController :ControllerBase
{
    private readonly UserManager<User> _userManager;

    public UserDetailsController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    [HttpGet("current-user")]
    public async Task<IActionResult> GetUserData()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }
        return Ok(new
        {
            gender = user.Gender,
            dateOfBirth = user.DateOfBirth,
        });
    }

    [HttpPost("change-details")]
    public async Task<IActionResult> UpdateUserDetails([FromBody]ChangeUserDetailsDto Request)
    {
        if ( !ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }
        
        user.Gender = Request.Gender;
        user.DateOfBirth = Request.DateOfBirth;
        
        var isSuccess = await _userManager.UpdateAsync(user);
        if (!isSuccess.Succeeded)
        {
            return BadRequest(isSuccess.Errors);
        }

        return Ok(new { Messaage = "Twoje dane zostały zmienione" });

    }
}






