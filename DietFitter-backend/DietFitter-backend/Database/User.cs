using Microsoft.AspNetCore.Identity;

namespace DietFitter_backend.Database;

public class User : IdentityUser
{
    public string? Initials { get; set; }
}