using Microsoft.AspNetCore.Mvc;
using DietFitter_backend.Services;
using DietFitter_backend.Repositories;
using DietFitter_backend.DTO;
using DietFitter_backend.Database;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DietFitter_backend.Controllers
{
    [ApiController]
    [Route("api/DietRecommendation")]
    public class DietRecommendationController : ControllerBase
    {
        private readonly DietRecommendationService _dietRecommendationService;
        private readonly UserDietRecommendationRepository _userDietRecommendationRepository;

        public DietRecommendationController(
            DietRecommendationService dietRecommendationService,
            UserDietRecommendationRepository userDietRecommendationRepository)
        {
            _dietRecommendationService = dietRecommendationService;
            _userDietRecommendationRepository = userDietRecommendationRepository;
        }

        
        [HttpPost("recommend-diet")]
        public async Task<IActionResult> CreateDietRecommendation([FromBody] DietRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.SelectedCondition))
            {
                return BadRequest("Nieprawidłowe dane wejściowe");
            }

            var recommendation = await _dietRecommendationService.FitDietForProblem(request);

            if (!recommendation.Any())
            {
                return BadRequest("Nie znaleziono odpowiednich produktów");
            }

            var formatRecommendation = new UserDietRecommendation
            {
                UserId = request.UserId,
                Problem = request.SelectedCondition,
                SelectedCategory = request.PreferredCategory ?? "Wszystkie",
                Date = DateTime.UtcNow,
                Meals = recommendation.Select(m => new Meal
                {
                    Name = m.Name,
                    Items = m.Items.Select(i => new MealItem
                    {
                        Food = i.Food,
                        Grams = i.Grams,
                        ProvidedValue = i.ProvidedValue,
                        MealId = 0 
                    }).ToList()
                }).ToList()
            };

            await _userDietRecommendationRepository.SaveRecommendation(formatRecommendation);

            return Ok(recommendation);
        }

        
        [HttpGet("user-recommendations/{userId}")]
        public async Task<IActionResult> GetUserRecommendations(string userId, [FromQuery] int limit = 1)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Brak identyfikatora użytkownika");
            }

            var recommendations = await _userDietRecommendationRepository.GetUserRecommendations(userId, limit);

            if (!recommendations.Any())
            {
                return NotFound("Nie znaleziono rekomendacji dla tego użytkownika.");
            }

            return Ok(recommendations);
        }
    }
}
