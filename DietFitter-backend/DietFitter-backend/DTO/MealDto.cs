namespace DietFitter_backend.DTO
{
    public class MealDto
    {
        public string Name { get; set; } = string.Empty;
        public List<MealItemDto> Items { get; set; } = new();
    }
}