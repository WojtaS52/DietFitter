using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DietFitter_backend.Database;

public class UserStats
{
             [Key]
             public Guid Id { get; set; }
     
             [Required]
             public string UserId { get; set; }
     
             [ForeignKey("UserId")]
             public User User { get; set; }
     
             [Required]
             public DateTime Date { get; set; } = DateTime.UtcNow;
     
             public float? Weight { get; set; }
             public float? BMI { get; set; }
             public float? BloodSugar { get; set; }
     
             public float? Cholesterol { get; set; } 
        
             // 7 because it can be like 120/80 for example
             [MaxLength(7)]
             public string? BloodPressure { get; set; }
     
             public float? VitaminD { get; set; }
             public float? Magnesium { get; set; }
             public float? Iron { get; set; }
             public float? Potassium { get; set; }
             public float? Zinc { get; set; }
             public float? Calcium { get; set; }
   
}