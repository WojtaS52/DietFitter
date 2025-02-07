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

        public async Task<UserDietRecommendation?> GetLastRecommendation(string userId)
        {
            return await _context.UserDietRecommendations
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.Date)
                .FirstOrDefaultAsync();
        }
    }
}