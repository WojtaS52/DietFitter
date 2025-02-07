using DietFitter_backend.Database;
using DietFitter_backend.Repositories;
using DietFitter_backend.DTO;
using DietFitter_backend.Utils;

namespace DietFitter_backend.Services
{
    
    public class AlgorithmService
    {
        private readonly FoodProductRepository _foodProductRepository;
        private readonly UserDietRecommendationRepository _userDietRecommendationRepository;
        private double ingredientPortion;

        public AlgorithmService(FoodProductRepository foodProductRepository, UserDietRecommendationRepository userDietRecommendationRepository)
        {
            _foodProductRepository = foodProductRepository;
            _userDietRecommendationRepository = userDietRecommendationRepository;
        }

        public async Task<List<UserDietDto>> FitDietForProblem(DietRequest request)
        {
            var foodDb = await _foodProductRepository.GetAllFoodProducts();
            double dailyPortion=0;
            var filtratedFood = string.IsNullOrWhiteSpace(request.PreferredCategory) ? foodDb : foodDb.Where(f => f.Category == request.PreferredCategory).ToList();
            
            var algorithmResult = new List<UserDietDto>();
            string temp = request.SelectedCondition.ToLower();
            string help = request.SelectedCondition;
            if (temp == "nadwaga")
            {
                filtratedFood = filtratedFood.Where(f => f.Calories < 500 && f.Protein > 10).OrderByDescending(f => f.Protein ).ThenBy(f => f.Calories).ToList();
            }
            else if (temp == "niedowaga")
            {
                filtratedFood = filtratedFood.Where(f => f.Calories > 400 && f.Protein > 14).OrderByDescending(f => f.Protein ).ThenBy(f => f.Calories).ToList();
            }
            else if (temp == "cukrzyca")
            {
                filtratedFood = filtratedFood.Where(f => f.GlycemicIndex < 50).OrderBy(f => f.GlycemicIndex).ToList();
            }
            else if (temp == "insulinoodporność")
            {
                // wróc tu pozniej i zmien wylicz mniej wiecej
                filtratedFood = filtratedFood.Where(f => f.GlycemicIndex < 50).OrderBy(f => f.GlycemicIndex).ToList();
            }
            else if (temp == "nadciśnienie")
            {
                filtratedFood = filtratedFood.Where(f => f.Sodium < 100 && f.Magnesium > 100).OrderByDescending(f => f.Magnesium).ThenBy(f =>f.Sodium).ToList();
                
            } 
            else if (temp == "niedobór magnezu")
            {
                filtratedFood = filtratedFood.OrderByDescending(f => f.Magnesium).ToList();
                help = "Magnesium";
            }
            else if (temp == "niedobór żelaza")
            {
                filtratedFood = filtratedFood.OrderByDescending(f => f.Iron).ToList();
            }
            else if (temp == "niedobór potasu")
            {
                filtratedFood = filtratedFood.OrderByDescending(f => f.Potassium).ToList();
            }
            else if (temp == "niedobór wapnia")
            {
                filtratedFood = filtratedFood.OrderByDescending(f => f.Calcium).ToList();
            }
            else if (temp == "niedobór cynku")
            {
                filtratedFood = filtratedFood.OrderByDescending(f => f.Zinc).ToList();
            }
            else if (temp == "niedobór witaminy D")
            {
                filtratedFood = filtratedFood.OrderByDescending(f => f.VitaminD).ToList();
            }
            else
            {
                filtratedFood = filtratedFood.OrderByDescending(f => f.Calories).ToList();
            }

            if (Limits.DailyPortion.ContainsKey(help))
            {
                dailyPortion = Limits.DailyPortion[help];
            }
            else
            {   
                dailyPortion = double.MaxValue;
            }

            foreach (var food in filtratedFood.Take(2))
            {
                ingredientPortion = 0;

                if (temp == "niedobór cynku")
                {
                    ingredientPortion = food.Zinc;
                }
                else if (temp == "niedobór żelaza")
                {
                    ingredientPortion = food.Iron;
                }
                else if (temp == "niedobór magnezu")
                {
                    ingredientPortion = food.Magnesium;
                }
                else if (temp == "niedobór potasu")
                {
                    ingredientPortion = food.Potassium;
                }
                else if (temp == "niedobór wapnia")
                {
                    ingredientPortion = food.Calcium;
                }
                
                else if (temp == "niedobór witaminy D")
                {
                    ingredientPortion = food.VitaminD;
                }
                
                double amount = Math.Min(((dailyPortion / ingredientPortion)*100), 100);
                amount = Math.Round(amount, 2);
                double providedValue = ingredientPortion * (amount / 100);
                providedValue = Math.Round(providedValue, 0);
                algorithmResult.Add(new UserDietDto() 
                    {
                        Food = food.Name,
                        Grams = amount,
                        ProvidedValue = providedValue
                    }
                
                );
            }

            return algorithmResult;
        }
       
    }
    
    
}

