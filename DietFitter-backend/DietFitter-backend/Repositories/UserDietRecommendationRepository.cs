using DietFitter_backend.Database;
using DietFitter_backend.DTO;
using DietFitter_backend.Database;

using Microsoft.EntityFrameworkCore;

namespace DietFitter_backend.Repositories
{
    public class UserDietRecommendationRepository
    {
        private readonly ApplicationDbContext _context;

        public UserDietRecommendationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveRecommendation(UserDietRecommendation recommendation)
        {
            _context.UserDietRecommendations.Add(recommendation);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserDietRecommendation>> GetUserRecommendations(string userId, int limit = 5)
        {
            return await _context.UserDietRecommendations
                .Where(r => r.UserId == userId)
                .Include(r => r.Meals)
                    .ThenInclude(m => m.Items)
                .OrderByDescending(r => r.Date)
                .Take(limit)
                .ToListAsync();
        }
    }
}