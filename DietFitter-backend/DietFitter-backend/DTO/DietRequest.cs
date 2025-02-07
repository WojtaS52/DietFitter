namespace DietFitter_backend.DTO
{
    public class DietRequest
    {
        public string UserId { get; set; } = string.Empty;
        public double UserWeight { get; set; }
        public double? UserHeight { get; set; }
        public string SelectedCondition { get; set; } = string.Empty;
        public string? PreferredCategory { get; set; }
    }
}