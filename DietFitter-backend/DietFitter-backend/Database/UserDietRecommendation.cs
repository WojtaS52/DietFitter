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
        
        [NotMapped]
        public List<Meal> Meals { get; set; } = new();
/*
        public string MealsJson
        {
            get => JsonSerializer.Serialize(Meals);
            set => Meals = JsonSerializer.Deserialize<List<Meal>>(value) ?? new List<Meal>();
        }*/
        public string MealsJson
        {
            get => JsonSerializer.Serialize(Meals);
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Meals = new List<Meal>(); 
                }
                else
                {
                    try
                    {
                        Meals = JsonSerializer.Deserialize<List<Meal>>(value) ?? new List<Meal>();
                    }
                    catch (JsonException)
                    {
                        Meals = new List<Meal>(); 
                    }
                }
            }
        }

    
    
        
    }
}