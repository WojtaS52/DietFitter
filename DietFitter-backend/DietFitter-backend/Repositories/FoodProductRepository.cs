using DietFitter_backend.Database;
using Microsoft.EntityFrameworkCore;

namespace DietFitter_backend.Repositories
{
    public class FoodProductRepository : IFoodProductRepository
    {
        
        private readonly ApplicationDbContext _context;
        public FoodProductRepository(ApplicationDbContext context)
        { 
            _context = context;
        }
            
        public async Task<List<FoodProduct>> GetAllFoodProducts()
        {
            return await _context.FoodProducts.ToListAsync();
        }
            
        
    }
}
