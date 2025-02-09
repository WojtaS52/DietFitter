using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DietFitter_backend.Database;

namespace DietFitter_backend.Models
{
    public class UserLikedRecommendation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } // Powiązanie z użytkownikiem

        [Required]
        [ForeignKey("UserDietRecommendation")] // Poprawiona nazwa tabeli
        public int RecommendationId { get; set; }

        public virtual UserDietRecommendation Recommendation { get; set; }
    }
}