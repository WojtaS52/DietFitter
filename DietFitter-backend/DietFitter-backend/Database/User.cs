using Microsoft.AspNetCore.Identity;

namespace DietFitter_backend.Database;

public class User : IdentityUser
{
    public string? Initials { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public double? Weight { get; set; }
    public double? Height { get; set; }
}