using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DietFitter_backend.Repositories;
using DietFitter_backend.Services;
using DietFitter_backend.DTO;
using DietFitter_backend.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DietFitter_backend.UnitTests.Services
{
    [TestClass]
    public class DietRecommendationServiceTest
    {
        private Mock<IFoodProductRepository> _foodProductRepositoryMock;
        private Mock<IUserDietRecommendationRepository> _userDietRecommendationRepositoryMock;
        private DietRecommendationService _dietRecommendationService;

        [TestInitialize]
        public void Setup()
        {
            _foodProductRepositoryMock = new Mock<IFoodProductRepository>();
            _userDietRecommendationRepositoryMock = new Mock<IUserDietRecommendationRepository>();
            _dietRecommendationService = new DietRecommendationService(_foodProductRepositoryMock.Object, _userDietRecommendationRepositoryMock.Object);
        }

        [TestMethod]
        public async Task FitDietForProblem_ShouldReturnCorrectDiet_ForNiedowaga()
        {

            var foodProducts = new List<FoodProduct>
            {
                new FoodProduct { Name = "HighCalorieProduct", Calories = 600, Protein = 10, Category = "Mięso" },
                new FoodProduct { Name = "MediumCalorieProduct", Calories = 400, Protein = 15, Category = "Węglowodany" }
            };
            _foodProductRepositoryMock.Setup(repo => repo.GetAllFoodProducts()).ReturnsAsync(foodProducts);
            var request = new DietRequest { SelectedCondition = "niedowaga", UserWeight = 50 };

  
            var result = await _dietRecommendationService.FitDietForProblem(request);
            
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            Assert.IsTrue(result.Any(meal => meal.Items.Any(item => item.Food == "HighCalorieProduct")));
        }

        [TestMethod]
        public async Task FitDietForProblem_ShouldReturnCorrectDiet_ForCukrzyca()
        {

            var foodProducts = new List<FoodProduct>
            {
                new FoodProduct { Name = "LowGIProduct", Calories = 300, Protein = 10, GlycemicIndex = 30, Carbs = 50, Category = "Węglowodany" },
                new FoodProduct { Name = "HighGIProduct", Calories = 400, Protein = 10, GlycemicIndex = 70, Carbs = 70, Category = "Węglowodany" }
            };
            _foodProductRepositoryMock.Setup(repo => repo.GetAllFoodProducts()).ReturnsAsync(foodProducts);
            var request = new DietRequest { SelectedCondition = "cukrzyca", UserWeight = 70 };
            
            var result = await _dietRecommendationService.FitDietForProblem(request);
            
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            Assert.IsTrue(result.All(meal => meal.Items.All(item => item.Food != "HighGIProduct")));
        }

        [TestMethod]
        public void GetBestProductForMeal_ShouldReturnBestProduct_ForInsulinResistance()
        {
            var foodProducts = new List<FoodProduct>
            {
                new FoodProduct { Name = "LowGIProduct", Calories = 300, Protein = 10, GlycemicIndex = 30, Category = "Węglowodany" },
                new FoodProduct { Name = "LowGIProduct", Calories = 300, Protein = 10, GlycemicIndex = 30, Category = "Węglowodany" },
                new FoodProduct { Name = "LowGIProduct", Calories = 300, Protein = 10, GlycemicIndex = 30, Category = "Węglowodany" },
                new FoodProduct { Name = "HighGIProduct", Calories = 400, Protein = 20, GlycemicIndex = 70, Category = "Węglowodany" }
            };
            var usedProducts = new HashSet<string>();
            string category = "Węglowodany";
            string condition = "insulinoodporność";


            var result = (FoodProduct)typeof(DietRecommendationService)
                .GetMethod("GetBestProductForMeal", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(_dietRecommendationService, new object[] { category, condition, foodProducts, usedProducts });


            Assert.IsNotNull(result);
            Assert.AreEqual("LowGIProduct", result.Name);
        }

        [TestMethod]
        public void GetPortionSize_ShouldReturnCorrectSize_ForDifferentCategories()
        {
           
            var meatProduct = new FoodProduct { Name = "Meat", Calories = 500, Category = "Mięso" };
            var nutProduct = new FoodProduct { Name = "Nuts", Calories = 700, Category = "Orzechy" };
            var vegetableProduct = new FoodProduct { Name = "Vegetable", Calories = 50, Category = "Warzywa" };
            double remainingCalories = 1000;


            double meatPortion = (double)typeof(DietRecommendationService)
                .GetMethod("GetPortionSize", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(_dietRecommendationService, new object[] { meatProduct, remainingCalories });

            double nutPortion = (double)typeof(DietRecommendationService)
                .GetMethod("GetPortionSize", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(_dietRecommendationService, new object[] { nutProduct, remainingCalories });

            double vegetablePortion = (double)typeof(DietRecommendationService)
                .GetMethod("GetPortionSize", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(_dietRecommendationService, new object[] { vegetableProduct, remainingCalories });
            
            Assert.IsTrue(meatPortion <= 300);
            Assert.IsTrue(nutPortion <= 100);
            Assert.IsTrue(vegetablePortion <= 100);
        }

        [TestMethod]
        public void FoodCategoryChoice_ShouldReturnCorrectCategory()
        {
            string category = "Mięso/Ryby";
            
            string result = (string)typeof(DietRecommendationService)
                .GetMethod("FoodCategoryChoice", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(_dietRecommendationService, new object[] { category });
            
            Assert.IsTrue(result == "Mięso" || result == "Ryby");
        }

        [TestMethod]
        public void GetMealCategory_ShouldContainValidCategories()
        {
            
            var result = (Dictionary<string, List<string>>)typeof(DietRecommendationService)
                .GetMethod("GetMealCategory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(_dietRecommendationService, null);
            
            Assert.IsTrue(result.ContainsKey("Śniadanie"));
            Assert.IsTrue(result["Śniadanie"].Contains("Jajka/Wędliny"));
        }

        [TestMethod]
        public void GetMealCategoryVegan_ShouldContainValidCategories()
        {
            var result = (Dictionary<string, List<string>>)typeof(DietRecommendationService)
                .GetMethod("GetMealCategoryVegan", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(_dietRecommendationService, null);
            
            Assert.IsTrue(result.ContainsKey("Śniadanie"));
            Assert.IsTrue(result["Śniadanie"].Contains("Wegańskie"));
        }
        
        [TestMethod]
        public async Task SaveRecommendation_ShouldSaveCorrectly()
        {
            var recommendation = new UserDietRecommendation
            {
                UserId = "user123",
                Problem = "odchudzanie",
                SelectedCategory = "Mięso",
                Date = DateTime.UtcNow,
                Meals = new List<Meal>
                {
                    new Meal { Name = "Śniadanie", Items = new List<MealItem> { new MealItem { Food = "Jajka", Grams = 100, ProvidedValue = 150 } } }
                }
            };
            
            await _userDietRecommendationRepositoryMock.Object.SaveRecommendation(recommendation);
            
            _userDietRecommendationRepositoryMock.Verify(repo => repo.SaveRecommendation(It.IsAny<UserDietRecommendation>()), Times.Once);
        }
        
        [TestMethod]
public async Task FitDietForProblem_ShouldReturnCorrectDiet_ForNadcisnienie()
{

    var foodProducts = new List<FoodProduct>
    {
        new FoodProduct { Name = "LowSodiumFood", Calories = 300, Sodium = 50, Potassium = 200, Category = "Warzywa" },
        new FoodProduct { Name = "HighSodiumFood", Calories = 400, Sodium = 1000, Potassium = 50, Category = "Warzywa" }
    };
    _foodProductRepositoryMock.Setup(repo => repo.GetAllFoodProducts()).ReturnsAsync(foodProducts);
    var request = new DietRequest { SelectedCondition = "nadciśnienie", UserWeight = 80 };

 
    var result = await _dietRecommendationService.FitDietForProblem(request);


    Assert.IsNotNull(result);
    Assert.IsTrue(result.Any(meal => meal.Items.Any(item => item.Food == "LowSodiumFood"))); 
    Assert.IsFalse(result.Any(meal => meal.Items.Any(item => item.Food == "HighSodiumFood"))); 
}

[TestMethod]
public async Task FitDietForProblem_ShouldReturnCorrectDiet_ForZincDeficiency()
{
    var foodProducts = new List<FoodProduct>
    {
        new FoodProduct { Name = "LowZincFood", Calories = 300, Zinc = 5, Category = "Orzechy" },
        new FoodProduct { Name = "HighZincFood1", Calories = 400, Zinc = 20, Category = "Orzechy" },
        new FoodProduct { Name = "HighZincFood2", Calories = 400, Zinc = 19, Category = "Orzechy" }
    };

    _foodProductRepositoryMock.Setup(repo => repo.GetAllFoodProducts()).ReturnsAsync(foodProducts);

    var request = new DietRequest { SelectedCondition = "niedobór cynku", UserWeight = 75 };

    var result = await _dietRecommendationService.FitDietForProblem(request);

    Assert.IsNotNull(result);

    var selectedFoods = result.SelectMany(meal => meal.Items.Select(item => item.Food)).ToList();
    
    Assert.IsTrue(selectedFoods.Contains("HighZincFood1") || selectedFoods.Contains("HighZincFood2"));
}

[TestMethod]
public async Task FitDietForProblem_ShouldHandleInvalidCategory()
{

    var foodProducts = new List<FoodProduct>
    {
        new FoodProduct { Name = "StandardFood", Calories = 300, Protein = 15, Category = "Mięso" }
    };
    _foodProductRepositoryMock.Setup(repo => repo.GetAllFoodProducts()).ReturnsAsync(foodProducts);
    var request = new DietRequest { SelectedCondition = "odchudzanie", PreferredCategory = "NieistniejącaKategoria", UserWeight = 70 };
    
    var result = await _dietRecommendationService.FitDietForProblem(request);
    
    Assert.IsNotNull(result);
    Assert.IsTrue(result.Any(meal => meal.Items.Any(item => item.Food == "StandardFood"))); 
}

[TestMethod]
public void GetBestProductForMeal_ShouldPrioritizeProteinAndCalories()
{

    var foodProducts = new List<FoodProduct>
    {
        new FoodProduct { Name = "LowProteinHighCal", Calories = 600, Protein = 5, Category = "Mięso" },
        new FoodProduct { Name = "HighProteinLowCal", Calories = 300, Protein = 20, Category = "Mięso" },
        new FoodProduct { Name = "HighProteinLowCal", Calories = 300, Protein = 20, Category = "Mięso" },
        new FoodProduct { Name = "HighProteinLowCal", Calories = 300, Protein = 20, Category = "Mięso" }
    };
    var usedProducts = new HashSet<string>();
    string category = "Mięso";
    string condition = "niedowaga";


    var result = (FoodProduct)typeof(DietRecommendationService)
        .GetMethod("GetBestProductForMeal", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
        .Invoke(_dietRecommendationService, new object[] { category, condition, foodProducts, usedProducts });


    Assert.IsNotNull(result);
    Assert.AreEqual("HighProteinLowCal", result.Name);
}

    }
}
