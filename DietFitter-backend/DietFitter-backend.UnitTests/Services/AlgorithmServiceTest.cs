using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DietFitter_backend.Repositories;
using DietFitter_backend.Services;
using DietFitter_backend.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using DietFitter_backend.Database; 

namespace DietFitter_backend.UnitTests.Services
{
    [TestClass]
    public class AlgorithmServiceTest
    {
        private ApplicationDbContext _dbContext;
        private FoodProductRepository _foodProductRepository;
        private UserDietRecommendationRepository _userDietRecommendationRepository;
        private AlgorithmService _algorithmService;

        [TestInitialize]
        public void Setup()
        {
            
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            
            _dbContext = new ApplicationDbContext(options);
            _foodProductRepository = new FoodProductRepository(_dbContext);
            _userDietRecommendationRepository = new UserDietRecommendationRepository(_dbContext);
            _algorithmService = new AlgorithmService(_foodProductRepository, _userDietRecommendationRepository);
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            _dbContext.FoodProducts.AddRange(new List<FoodProduct>
            {
                new FoodProduct { Name = "Product1", Calories = 400, Protein = 15, Magnesium = 50 },
                new FoodProduct { Name = "Product2", Calories = 300, Protein = 20, Magnesium = 100 },
                new FoodProduct { Name = "Product2", Calories = 300, Protein = 20, Magnesium = 100 },
                new FoodProduct { Name = "Product2", Calories = 300, Protein = 20, Magnesium = 100, Category = "Warzywa" },
            });

            _dbContext.SaveChanges();
        }

        [TestMethod]
        public async Task FitDietForProblem_ShouldReturnCorrectDiet_ForNadwaga()
        {
            var request = new DietRequest { SelectedCondition = "nadwaga", PreferredCategory = null };
            var result = await _algorithmService.FitDietForProblem(request);
            
            Assert.IsNotNull(result, "Lista wynikowa nie powinna być pusta.");
            Assert.IsTrue(result.Count > 0, "Powinno być zwróconych kilka produktów.");
        }
        
        [TestMethod]
        public async Task FitDietForProblem_ShouldReturnCorrectDiet_ForNiedoborCynku()
        {
            
            var request = new DietRequest { SelectedCondition = "niedobór cynku", PreferredCategory = null };
            var result = await _algorithmService.FitDietForProblem(request);
            
            Assert.IsNotNull(result, "Lista wynikowa nie powinna być pusta.");
            Assert.IsTrue(result.Count > 0, "Powinno być zwróconych kilka produktów.");
        }
        
        [TestMethod]
        public async Task FitDietForProblem_ShouldReturnCorrectDiet_ForNiedoborMagnezuWarzywa()
        {
            
            var request = new DietRequest { SelectedCondition = "niedobór magnezu", PreferredCategory = "Warzywa" };
            var result = await _algorithmService.FitDietForProblem(request);

            Assert.IsNotNull(result, "Lista wynikowa nie powinna być pusta.");
            Assert.IsTrue(result.Count > 0, "Powinno być zwróconych kilka produktów.");
        }
        
        
        [TestMethod]
        public async Task FitDietForProblem_ShouldReturnCorrectDiet_ForNiedoborMagnezu()
        {
            
            var request = new DietRequest { SelectedCondition = "niedobór magnezu", PreferredCategory = null };
            var result = await _algorithmService.FitDietForProblem(request);
            
            Assert.IsNotNull(result, "Lista wynikowa nie powinna być pusta.");
            Assert.IsTrue(result.Count > 0, "Powinno być zwróconych kilka produktów.");
        }
    }
}
