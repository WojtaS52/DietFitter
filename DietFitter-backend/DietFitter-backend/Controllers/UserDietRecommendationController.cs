using DietFitter_backend.Database;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DietFitter_backend.Services;
using DietFitter_backend.DTO;
using DietFitter_backend.Models;
using DietFitter_backend.Repositories;

namespace DietFitter_backend.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UserDietRecommendationController : ControllerBase
{
     private readonly ApplicationDbContext _context;
    
        public UserDietRecommendationController(ApplicationDbContext context)
        {
            _context = context;
        }
    
        
        [HttpPost("like/{recommendationId}")]
        public async Task<IActionResult> LikeRecommendation(int recommendationId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (userId == null)
            {
                return Unauthorized();
            }
            
            var existingLike = await _context.UserLikedRecommendations
                .FirstOrDefaultAsync(l => l.UserId == userId && l.RecommendationId == recommendationId);
            
            if (existingLike != null)
            {
               return BadRequest("Polubiłeś już tę rekomendację");
            }
            
            var like = new UserLikedRecommendation
            {
                UserId = userId,
                RecommendationId = recommendationId
            };
    
            _context.UserLikedRecommendations.Add(like);
            await _context.SaveChangesAsync();
    
            return Ok();
        }
        
        
        [HttpGet("liked")]
        public async Task<IActionResult> GetLikedRecommendations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (userId == null)
            {
                return Unauthorized();
            }
            
            var likedUserRecommendations = await _context.UserLikedRecommendations
                    .Where(l => l.UserId == userId)
                    .Include(l => l.Recommendation) 
                    .ThenInclude(r => r.Meals) 
                    .ThenInclude(m => m.Items) 
                    .ToListAsync();
            
            return Ok(likedUserRecommendations);
        }
        
       [HttpGet("user-last-recommendation/{userId}")]
       public async Task<IActionResult> GetLastUserRecommendation(string userId)
       {
           var lastRecommendation = await _context.UserDietRecommendations
               .Where(r => r.UserId == userId) 
               .OrderByDescending(r => r.Id)
               .FirstOrDefaultAsync();
       
           if (lastRecommendation == null)
           {
               return NotFound("Nie znaleziono rekomendacji.");
           }
       
           return Ok(new { id = lastRecommendation.Id });
       }




        
}        

   