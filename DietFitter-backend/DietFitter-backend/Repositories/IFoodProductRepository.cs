using System.Collections.Generic;
using System.Threading.Tasks;
using DietFitter_backend.Database;

namespace DietFitter_backend.Repositories
{
    public interface IFoodProductRepository
    {
        Task<List<FoodProduct>> GetAllFoodProducts();
    }
}