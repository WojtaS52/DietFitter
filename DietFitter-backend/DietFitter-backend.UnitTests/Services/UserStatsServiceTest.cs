using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DietFitter_backend.Database;
using DietFitter_backend.DTO;
using DietFitter_backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DietFitter_backend.UnitTests.Services
{
    [TestClass]
    public class UserStatsServiceTest
    {
        private ApplicationDbContext _dbContext;
        private UserStatsService _userStatsService;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _userStatsService = new UserStatsService(_dbContext);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            _dbContext.UserStats.AddRange(new List<UserStats>
            {
                new UserStats { Id = Guid.NewGuid(), UserId = "user1", Date = DateTime.UtcNow.AddDays(-5), Weight = 80, Magnesium = 50 },
                new UserStats { Id = Guid.NewGuid(), UserId = "user1", Date = DateTime.UtcNow.AddDays(-3), Weight = 78, Magnesium = 55 },
                new UserStats { Id = Guid.NewGuid(), UserId = "user2", Date = DateTime.UtcNow.AddDays(-2), Weight = 70, Magnesium = 60 }
            });

            _dbContext.SaveChanges();
        }

        [TestMethod]
        public async Task AddUserStats_ShouldAddNewStat_ForWeight()
        {
            // Arrange
            var dto = new UserStatsDto { MonitoringType = "weight", Value = 75 };
            string userId = "user3";

            // Act
            var result = await _userStatsService.AddUserStats(dto, userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.UserId);
            Assert.AreEqual(75, result.Weight);
        }

        [TestMethod]
        public async Task GetUserStats_ShouldReturnUserStats()
        {
            // Act
            var result = await _userStatsService.GetUserStats("user1");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
            Assert.IsTrue(result.All(s => s.UserId == "user1"));
        }
        
        [TestMethod]
        public async Task AddUserStats_ShouldAddNewStat2()
        {
            var dto = new UserStatsDto { MonitoringType = "weight", Value = 75 };
            var result = await _userStatsService.AddUserStats(dto, "user1");

            Assert.IsNotNull(result);
            Assert.AreEqual(75, result.Weight);
        }

       
    }
}
