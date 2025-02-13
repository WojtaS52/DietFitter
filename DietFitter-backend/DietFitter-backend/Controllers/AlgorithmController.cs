using DietFitter_backend.Database;
using Microsoft.AspNetCore.Mvc;
using DietFitter_backend.Services;
using DietFitter_backend.DTO;
using DietFitter_backend.Repositories;


namespace DietFitter_backend.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AlgorithmController : ControllerBase
{
    private readonly AlgorithmService _algorithmService;
    private readonly UserDietRecommendationRepository _userDietRecommendationRepository;
    
    
    [HttpGet("last-recommendation/{userId}")]
    public async Task<IActionResult> GetLastUserRecommendation(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return BadRequest("Nieprawidłowe dane wejściowe");
        }

        var recommendation = await _userDietRecommendationRepository.GetUserRecommendations(userId, 1);

        if (recommendation == null)
        {
            return NotFound("Nie znaleziono rekomendacji");
        }

        return Ok(recommendation);
    }
}