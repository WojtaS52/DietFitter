using DietFitter_backend.Database;

namespace DietFitter_backend.Repositories
{
    public interface IUserDietRecommendationRepository
    {
        Task SaveRecommendation(UserDietRecommendation recommendation);
        Task<List<UserDietRecommendation>> GetUserRecommendations(string userId, int limit = 5);
    }
}