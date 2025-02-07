using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DietFitter_backend.Database;
using DietFitter_backend.DTO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DietFitter_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserDataController :ControllerBase
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public UserDataController(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _signInManager = signInManager;
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
            FirstName = user.FirstName,
            LastName = user.LastName,
            Weight = user.Weight,
            Height = user.Height

        });
    }

    [HttpPost("change-data")]
    public async Task<IActionResult> UpdateUserData([FromBody]ChangeUserDataDto Request)
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
        
        user.FirstName = Request.FirstName;
        user.LastName = Request.LastName;
        user.Weight = Request.Weight;
        user.Height = Request.Height;
        var isSuccess = await _userManager.UpdateAsync(user);
        if (!isSuccess.Succeeded)
        {
            return BadRequest(isSuccess.Errors);
        }

        return Ok(new { Messaage = "Twoje dane zostały zmienione" });

    }
    
}