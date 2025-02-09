using System.Text.Json.Serialization;

namespace DietFitter_backend.Database
{
    public class MealItem
    {
        public int Id { get; set; }
        public string Food { get; set; } = string.Empty;
        public double Grams { get; set; }
        public double ProvidedValue { get; set; }

        public int MealId { get; set; }
        [JsonIgnore]
        public Meal Meal { get; set; } = null!;
    }
}
