using System.ComponentModel.DataAnnotations;

namespace DietFitter_backend.DTO;

public class ChangeEmailDto
{
    [Required]
    [EmailAddress]
    public string NewEmail { get; set; }
}