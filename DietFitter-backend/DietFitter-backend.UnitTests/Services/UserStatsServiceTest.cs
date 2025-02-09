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
           
            var dto = new UserStatsDto { MonitoringType = "weight", Value = 75 };
            string userId = "user3";

            var result = await _userStatsService.AddUserStats(dto, userId);

            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.UserId);
            Assert.AreEqual(75, result.Weight);
        }

        [TestMethod]
        public async Task GetUserStats_ShouldReturnUserStats()
        {

            var result = await _userStatsService.GetUserStats("user1");

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
        
        [TestMethod]
        public async Task AddUserStats_ShouldAddNewStat_ForMagnesium()
        {
            var dto = new UserStatsDto { MonitoringType = "magnesium", Value = 65 };
            string userId = "user1";

            var result = await _userStatsService.AddUserStats(dto, userId);
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.UserId);
            Assert.AreEqual(65, result.Magnesium);
        }
        
        [TestMethod]
        public async Task GetUserStats_ShouldReturnEmptyList_WhenUserNotFound()
        {

            var result = await _userStatsService.GetUserStats("user999"); 
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count); 
        }
        
        [TestMethod]
        public async Task AddUserStats_ShouldThrowException_ForInvalidMonitoringType()
        {

            var dto = new UserStatsDto { MonitoringType = "invalidType", Value = 100 };
            string userId = "user1";
        

            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
                await _userStatsService.AddUserStats(dto, userId));
        }

        
       
    }
}
