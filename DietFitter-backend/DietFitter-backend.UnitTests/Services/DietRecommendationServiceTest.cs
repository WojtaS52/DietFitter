using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DietFitter_backend.Repositories;
using DietFitter_backend.Services;
using DietFitter_backend.DTO;
using DietFitter_backend.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FluentAssertions;

namespace DietFitter_backend.UnitTests.Services
{
    [TestClass]
    public class DietRecommendationServiceTest
    {
        private Mock<IFoodProductRepository> _foodProductRepositoryMock;
        private DietRecommendationService _dietRecommendationService;

        [TestInitialize]
        public void Setup()
        {
            _foodProductRepositoryMock = new Mock<IFoodProductRepository>();
            _dietRecommendationService = new DietRecommendationService(_foodProductRepositoryMock.Object);
        }

        [TestMethod]
        public async Task FitDietForProblem_ShouldReturnCorrectDiet_ForOdchudzanie()
        {
            // Arrange
            var foodProducts = new List<FoodProduct>
            {
                new FoodProduct { Name = "Product1", Calories = 400, Protein = 15 },
                new FoodProduct { Name = "Product2", Calories = 300, Protein = 20 }
            };
            _foodProductRepositoryMock.Setup(repo => repo.GetAllFoodProducts()).ReturnsAsync(foodProducts);

            var request = new DietRequest { SelectedCondition = "odchudzanie", UserWeight = 70 };

            // Act
            var result = await _dietRecommendationService.FitDietForProblem(request);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].Food.Should().Be("Product2");
            result[1].Food.Should().Be("Product1");
        }
    }
}
