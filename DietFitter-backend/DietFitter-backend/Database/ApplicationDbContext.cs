using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DietFitter_backend.DTO;
namespace DietFitter_backend.Database;

public class ApplicationDbContext :IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }
    
    //public DbSet<UserStats> UserStats { get; set; } // Rejestracja nowej tabeli w bazie danych
    public DbSet<UserStats> UserStats { get; set; } 
    public DbSet<FoodProduct> FoodProducts { get; set; }
    
    public DbSet<UserDietRecommendation> UserDietRecommendations { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>().Property(u => u.Initials).HasMaxLength(5);
        builder.HasDefaultSchema("identity");
        
         //builder.Entity<UserStats>()
           //         .HasOne(up => up.User)
             //       .WithMany()
               //     .HasForeignKey(up => up.UserId)
                 //   .OnDelete(DeleteBehavior.Cascade);
                 
        builder.Entity<UserStats>()
                    .HasOne(us => us.User)
                    .WithMany()
                    .HasForeignKey(us => us.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<UserDietDto>().HasNoKey();
        
        builder.Entity<UserDietRecommendation>()
                .Property(r => r.DietJson)
                .HasColumnType("jsonb");
        
    }
    
}