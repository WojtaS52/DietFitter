using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DietFitter_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DietFitter_backend.Database; 
using DietFitter_backend.DTO;
namespace DietFitter_backend.Controllers
{
    [ApiController]
    [Route("api/userstats")]
    [Authorize]
    public class UserStatsController : ControllerBase
    {
        private readonly UserStatsService _userStatsService;

        public UserStatsController(UserStatsService userStatsService)
        {
            _userStatsService = userStatsService;
        }
        
       [HttpPost]
       public async Task<IActionResult> AddUserStats([FromBody] UserStatsDto dto)
       {
           var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
           if (userId == null) return Unauthorized();
       
           var result = await _userStatsService.AddUserStats(dto, userId);
           return Ok(result);
       }



        [HttpGet]
        public async Task<IActionResult> GetUserStats()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var result = await _userStatsService.GetUserStats(userId);
            return Ok(result);
        }

        [HttpGet("range")]
        public async Task<IActionResult> GetUserStatsInRange([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var result = await _userStatsService.GetUserStatsInRange(userId, from, to);
            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUserStats(Guid id, [FromBody] Dictionary<string, object> updates)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var result = await _userStatsService.UpdateUserStats(id, userId, updates);
            if (result == null) return BadRequest("Niepoprawne pole lub wartość.");

            return Ok(result);
        }
        
        [HttpGet("{fieldName}")]
        public async Task<IActionResult> GetSpecificStat(string fieldName)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();
        
            var result = await _userStatsService.GetSpecificStat(userId, fieldName);
            if (result == null) return BadRequest($"Nieznane pole: {fieldName}");
        
            return Ok(result);
        }

        
    }
}
