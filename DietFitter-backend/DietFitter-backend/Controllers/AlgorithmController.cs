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
    
    public AlgorithmController(AlgorithmService algorithmService, UserDietRecommendationRepository userDietRecommendationRepository)
    {
        _algorithmService = algorithmService;
        _userDietRecommendationRepository = userDietRecommendationRepository;
    }
    
    
  /*  [HttpPost("recommend-diet")]
    public async Task<IActionResult> CreateDietAlgorithResult([FromBody] DietRequest request)
    {
        if (request == null)
        {
            return BadRequest("Nieprawidłowe dane wejściowe");
        }

        if (string.IsNullOrWhiteSpace(request.SelectedCondition))
        {
            return BadRequest("Nieprawidłowe dane wejściowe");
        }
        
        
        var recommendation = await _algorithmService.FitDietForProblem(request);
        
        if(!recommendation.Any())
        {
            return BadRequest("Nie znaleziono odpowiednich produktów");
        }
        
        var formatRecomendation = new UserDietRecommendation()
        {
            UserId = request.UserId,
            Problem = request.SelectedCondition,
            SelectedCategory = request.PreferredCategory ?? "Wszystkie",
            Date = DateTime.UtcNow,
            Diet = recommendation
            
        };
        await _userDietRecommendationRepository.SaveRecommendation(formatRecomendation);
        
        return Ok(recommendation);
    }*/
/*
    [HttpGet("last-recommendation/{userId}")]
    public async Task<IActionResult> GetLastUserRecommendation(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return BadRequest("Nieprawidłowe dane wejściowe");
        }

        var recommendation = await _userDietRecommendationRepository.GetLastRecommendation(userId);

        if (recommendation == null)
        {
            return NotFound("Nie znaleziono rekomendacji");
        }

        return Ok(recommendation);
    }*/
}