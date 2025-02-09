namespace DietFitter_backend.DTO
{
    public class MealItemDto
    {
        public string Food { get; set; } = string.Empty;
        public double Grams { get; set; }
        public double ProvidedValue { get; set; }
    }
}