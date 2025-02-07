using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DietFitter_backend.Database;
using DietFitter_backend.DTO;

namespace DietFitter_backend.Services
{
    public class UserStatsService
    {
        private readonly ApplicationDbContext _context;

        public UserStatsService(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public async Task<UserStats> AddUserStats(UserStatsDto dto, string userId)
        {
            
            var userStats = new UserStats
            {
                UserId = userId,
                Date = DateTime.UtcNow
            };
            
            string monitoringType = dto.MonitoringType.ToLower();
            
            if (monitoringType == "weight")
            {
                userStats.Weight = dto.Value;
            }
            else if (monitoringType == "bloodpressure")
            {
                userStats.BloodPressure = dto.Value.ToString();
            }
            else if (monitoringType == "iron")
            {
                userStats.Iron = dto.Value;
            }
            else if (monitoringType == "vitamind")
            {
                userStats.VitaminD = dto.Value;
            }
            else if (monitoringType == "magnesium")
            {
                userStats.Magnesium = dto.Value;
            }
            else if (monitoringType == "cholesterol")
            {
                userStats.Cholesterol = dto.Value;
            }
            else if (monitoringType == "bloodsugar")
            {
                userStats.BloodSugar = dto.Value;
            }
            else if (monitoringType == "potassium")
            {
                userStats.Potassium = dto.Value;
            }
            else if (monitoringType == "zinc")
            {
                userStats.Zinc = dto.Value;
            }
            else if (monitoringType == "calcium")
            {
                userStats.Calcium = dto.Value;
            }
            else
            {
                throw new ArgumentException("Nie ma takiego typu");
            }
        
            _context.UserStats.Add(userStats);
            await _context.SaveChangesAsync();
        
            return userStats;
        }

        public async Task<List<UserStats>> GetUserStats(string userId)
        {
            return await _context.UserStats
                .Where(s => s.UserId == userId)
                .OrderBy(s => s.Date)
                .ToListAsync();
        }

        public async Task<List<UserStats>> GetUserStatsInRange(string userId, DateTime from, DateTime to)
        {
            return await _context.UserStats
                .Where(s => s.UserId == userId && s.Date >= from && s.Date <= to)
                .OrderBy(s => s.Date)
                .ToListAsync();
        }

        
        public async Task<UserStats?> UpdateUserStats(Guid id, string userId, Dictionary<string, object> updates)
        {
            var stats = await _context.UserStats.FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);
            if (stats == null) return null;

            foreach (var update in updates)
            {
                var propertyName = update.Key;
                var newValue = update.Value;

                var propertyInfo = typeof(UserStats).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null) continue;

                try
                {
                    var convertedValue = Convert.ChangeType(newValue, propertyInfo.PropertyType);
                    propertyInfo.SetValue(stats, convertedValue);
                }
                catch
                {
                    return null; 
                }
            }

            await _context.SaveChangesAsync();
            return stats;
        }

        public async Task<List<object>> GetSpecificStat(string userId, string fieldName)
        {
            var statsQuery = _context.UserStats
                .Where(s => s.UserId == userId)
                .OrderBy(s => s.Date);

            switch (fieldName.ToLower())
            {
                case "magnesium":
                    return await statsQuery.Select(s => new { s.Date, s.Magnesium }).ToListAsync<object>();
                case "cholesterol":
                    return await statsQuery.Select(s => new { s.Date, s.Cholesterol }).ToListAsync<object>();
                case "bloodsugar":
                    return await statsQuery.Select(s => new { s.Date, s.BloodSugar }).ToListAsync<object>();
                case "weight":
                    return await statsQuery.Select(s => new { s.Date, s.Weight }).ToListAsync<object>();
                case "bmi":
                    return await statsQuery.Select(s => new { s.Date, s.BMI }).ToListAsync<object>();
                case "vitamind":
                    return await statsQuery.Select(s => new { s.Date, s.VitaminD }).ToListAsync<object>();
                case "iron":
                    return await statsQuery.Select(s => new { s.Date, s.Iron }).ToListAsync<object>();
                case "potassium":
                    return await statsQuery.Select(s => new { s.Date, s.Potassium }).ToListAsync<object>();
                case "zinc":
                    return await statsQuery.Select(s => new { s.Date, s.Zinc }).ToListAsync<object>();
                case "calcium":
                    return await statsQuery.Select(s => new { s.Date, s.Calcium }).ToListAsync<object>();
                case "bloodpressure":
                    return await statsQuery.Select(s => new { s.Date, s.BloodPressure }).ToListAsync<object>();
                default:
                    return null; 
            }
        }
    }
}
