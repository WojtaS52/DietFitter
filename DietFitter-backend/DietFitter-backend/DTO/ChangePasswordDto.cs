using System.ComponentModel.DataAnnotations;

namespace DietFitter_backend.DTO;

public class ChangePasswordDto
{
    [Required]
    public string OldPassword { get; set; }

    [Required]
    public string NewPassword { get; set; }
}