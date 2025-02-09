using System.Text.Json.Serialization;
using DietFitter_backend.Database;

public class Meal
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public int RecommendationId { get; set; }
    
    [JsonIgnore] // 🚨 Blokuje cykl w serializacji
    public UserDietRecommendation Recommendation { get; set; } = null!;
    
    public List<MealItem> Items { get; set; } = new();
}