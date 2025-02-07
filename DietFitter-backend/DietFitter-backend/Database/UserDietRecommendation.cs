using DietFitter_backend.DTO;
using System.Text.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace DietFitter_backend.Database
{
    public class UserDietRecommendation
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Problem { get; set; } = string.Empty;
        public string SelectedCategory { get; set; } = "Wszystkie";
        public DateTime Date { get; set; } = DateTime.UtcNow;
        //public List<UserDietDto> Diet { get; set; } = new List<UserDietDto>();
        public string DietJson { get; set; } = string.Empty;
        
        [NotMapped]
        public List<UserDietDto> Diet 
        {
             get => string.IsNullOrEmpty(DietJson) ? new List<UserDietDto>() : JsonSerializer.Deserialize<List<UserDietDto>>(DietJson) ?? new List<UserDietDto>();
             set => DietJson = JsonSerializer.Serialize(value);
        }
        
    }
}