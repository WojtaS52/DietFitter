using DietFitter_backend.Database;
using DietFitter_backend.Repositories;
using DietFitter_backend.DTO;
using DietFitter_backend.Utils;

namespace DietFitter_backend.Services
{
    public class DietRecommendationService
    {
        private readonly IFoodProductRepository _foodProductRepository;
        private readonly IUserDietRecommendationRepository _userDietRecommendationRepository;
        
        public DietRecommendationService(IFoodProductRepository foodProductRepository, IUserDietRecommendationRepository userDietRecommendationRepository)
        {
            _foodProductRepository = foodProductRepository;
            _userDietRecommendationRepository = userDietRecommendationRepository;
        }

        private double CalculateDailyCaloriesRequirement(double weight, string condition)
        {
            double dailyCaloriesRequirement = 31 * weight;
            if (condition == "odchudzanie") dailyCaloriesRequirement -= 300;
            else if (condition == "niedowaga") dailyCaloriesRequirement += 300;
            return dailyCaloriesRequirement;
        }

        private Dictionary<string, List<string>> GetMealCategory()
        {
            return new Dictionary<string, List<string>>
            {
                { "Śniadanie", new List<string> { "Jajka/Wędliny", "Pieczywo", "Nabiał", "Orzechy/Owoce" } },
                { "Drugie Śniadanie", new List<string> { "Orzechy", "Warzywa","Nabiał","Owoce","Pieczywo" } },
                { "Obiad", new List<string> { "Mięso/Ryby","Węglowodany" ,"Warzywa", "Warzywa" } },
                { "Kolacja", new List<string> { "Mięso","Węglowodany", "Warzywa", "Owoce" } }
            };
        }
        
        private Dictionary<string, List<string>> GetMealCategoryVegan()
        {
            return new Dictionary<string, List<string>>
            {
                { "Śniadanie", new List<string> { "Wegańskie", "Pieczywo", "Warzywa", "Orzechy/Owoce" } },
                 { "Drugie Śniadanie", new List<string> { "Orzechy", "Warzywa","Owoce" } },   
                { "Obiad", new List<string> { "Wegańskie", "Węglowodany", "Warzywa","Owoce" } },
                { "Kolacja", new List<string> { "Orzechy", "Wegańskie", "Warzywa","Warzywa","Węglowodany" } }
            };
        }

        private FoodProduct? GetBestProductForMeal(string category, string condition, List<FoodProduct> foodDB, HashSet<string> usedProducts)
        {
            var availableFoods = foodDB.Where(f => f.Category == category && !usedProducts.Contains(f.Name)).ToList();
            if (!availableFoods.Any()) return null;
        
            List<FoodProduct> sortedFoods = condition switch
            {
                "niedobór magnezu" => availableFoods.OrderByDescending(f => f.Magnesium).ToList(),
                "niedobór żelaza" => availableFoods.OrderByDescending(f => f.Iron).ToList(),
                "niedobór potasu" => availableFoods.OrderByDescending(f => f.Potassium).ToList(),
                "niedobór cynku" => availableFoods.OrderByDescending(f => f.Zinc).ToList(),
                "insulinoodporność" => availableFoods.Where(f => f.GlycemicIndex < 50 && f.Protein < 20)
                                                .OrderBy(f => f.GlycemicIndex)
                                                .ToList(),
                "niedobór witaminy D" => availableFoods.OrderByDescending(f => f.VitaminD).ToList(),                          
                "cukrzyca" => availableFoods.Where(f => f.GlycemicIndex < 50 && f.Protein < 20 && f.Carbs < 55)
                                                .OrderBy(f => f.GlycemicIndex)
                                                .ToList(),
                "odchudzanie" or "nadwaga" => availableFoods.Where(f => f.Calories < 500 && f.Protein > 10)
                                                                .OrderByDescending(f => f.Protein)
                                                                .ThenBy(f => f.Calories)
                                                                .ToList(),
                "niedowaga" => availableFoods.Where(f => f.Calories > 200 && f.Protein > 3)
                                             .OrderByDescending(f => f.Protein)
                                             .ThenBy(f => f.Calories)
                                             .ToList(),
                "nadciśnienie" => availableFoods.Where(f => f.Sodium < 300  && f.Potassium > 40)
                                                                   .OrderByDescending(f => f.Magnesium)
                                                                   .ThenByDescending(f => f.Potassium)
                                                                   .ThenBy(f => f.Sodium)
                                                                   .ToList(),      
                _ => availableFoods.OrderByDescending(f => (f.Protein + f.Fat + f.Carbs) / 3.0).ToList() 
            };
        
            
            var topFoods = sortedFoods.Take(3).ToList();
            var random = new Random();
            return topFoods.Any() ? topFoods[random.Next(topFoods.Count)] : null;
        }
        
        
        private string FoodCategoryChoice(string category)
        {
            Random random = new Random();
            
            return category switch
            {
                "Mięso/Ryby" => random.Next(2) == 0 ? "Mięso" : "Ryby",
                "Mięso/Nabiał" => random.Next(2) == 0 ? "Mięso" : "Nabiał",
                "Jajka/Nabiał" => random.Next(2) == 0 ? "Jajka" : "Nabiał",
                "Nabiał/Owoce" => random.Next(2) == 0 ? "Nabiał" : "Owoce",
                "Orzechy/Owoce" => random.Next(2) == 0 ? "Orzechy" : "Owoce",
                "Jajka/Wędliny" => random.Next(2) == 0 ? "Jajka" : "Wędliny",
                _ => category
            };
        }
                


       public async Task<List<MealDto>> FitDietForProblem(DietRequest request)
       {
           var foodDB = await _foodProductRepository.GetAllFoodProducts();
           double dailyKcal = CalculateDailyCaloriesRequirement(request.UserWeight, request.SelectedCondition);
      
           var mealCalories = new Dictionary<string, double>
           {
               { "Śniadanie", dailyKcal * 0.25 },
               { "Drugie Śniadanie", dailyKcal * 0.15 }, 
               { "Obiad", dailyKcal * 0.35 },
               { "Kolacja", dailyKcal * 0.25 }
           };
       
           var meals = new List<MealDto>();
           var usedProducts = new HashSet<string>();
       
           var mealCategories = request.PreferredCategory?.ToLower() == "wegańskie"
               ? GetMealCategoryVegan()
               : GetMealCategory();
       
           foreach (var (mealName, calories) in mealCalories)
           {
               var meal = new MealDto { Name = mealName, Items = new List<MealItemDto>() };
               double remainingCalories = calories;
               var condition = request.SelectedCondition;
       
               foreach (var category in mealCategories.ContainsKey(mealName) ? mealCategories[mealName] : new List<string>())
               {
                   string chosenCategory = FoodCategoryChoice(category);
                   var product = GetBestProductForMeal(chosenCategory, condition, foodDB, usedProducts);
                   if (product == null) continue;
       
                   double portionSize = GetPortionSize(product, remainingCalories);
                   remainingCalories -= (portionSize / 100) * product.Calories;
                   usedProducts.Add(product.Name);
       
                   meal.Items.Add(new MealItemDto
                   {
                       Food = product.Name,
                       Grams = Math.Round(portionSize, 1),
                       ProvidedValue = Math.Round(product.Calories * (portionSize / 100), 2)
                   });
               }
       
               meals.Add(meal);
           }
       
           var recommendation = new UserDietRecommendation
           {
               UserId = request.UserId,
               Problem = request.SelectedCondition,
               SelectedCategory = request.PreferredCategory ?? "Wszystkie",
               Date = DateTime.UtcNow,
               Meals = meals.Select(m => new Meal
               {
                   Name = m.Name,
                   Items = m.Items.Select(i => new MealItem
                   {
                       Food = i.Food,
                       Grams = i.Grams,
                       ProvidedValue = i.ProvidedValue
                   }).ToList()
               }).ToList()
           };
       
           await _userDietRecommendationRepository.SaveRecommendation(recommendation);
           return meals;
       }
       
       private double GetPortionSize(FoodProduct product, double remainingCalories)
       {
           double portionSize = (remainingCalories / 3) / product.Calories * 100;
       
           var category = product.Category;
          
          switch (category) 
          {
                case "Mięso":
                case "Ryby":
                case "Jajka":
                    portionSize = Math.Min(portionSize, 300);
                    break;
                case "Orzechy":
                case "Owoce":
                    portionSize = Math.Min(portionSize, 100);
                    break;
                case "Pieczywo":
                case "Nabiał":
                    portionSize = Math.Min(portionSize, 150);
                    break;
                case "Warzywa":
                    portionSize = Math.Min(portionSize, 100);
                    break;
                case "Wegańskie":
                    portionSize = Math.Min(portionSize, 250);
                    break;
                case "Węglowodany":
                    portionSize = Math.Min(portionSize, 130);
                    break;
            }
       
           return portionSize;
       }


    }
}
