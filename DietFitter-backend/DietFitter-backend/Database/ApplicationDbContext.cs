using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DietFitter_backend.DTO;
using DietFitter_backend.Models;
namespace DietFitter_backend.Database;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;
using System.Text.Json.Serialization;

public class ApplicationDbContext :IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }
    
    public DbSet<UserStats> UserStats { get; set; } 
    public DbSet<FoodProduct> FoodProducts { get; set; }
    public DbSet<Meal> Meals { get; set; }  
    public DbSet<MealItem> MealItems { get; set; } 
    public DbSet<UserDietRecommendation> UserDietRecommendations { get; set; } 
    public DbSet<UserLikedRecommendation> UserLikedRecommendations { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>().Property(u => u.Initials).HasMaxLength(5);
        builder.HasDefaultSchema("identity");
        
        builder.Entity<UserStats>()
                    .HasOne(us => us.User)
                    .WithMany()
                    .HasForeignKey(us => us.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<UserDietRecommendation>()
                   .HasMany(r => r.Meals)
                   .WithOne(m => m.Recommendation)
                   .HasForeignKey(m => m.RecommendationId)
                   .OnDelete(DeleteBehavior.Cascade); 
           
       builder.Entity<Meal>()
           .HasMany(m => m.Items)
           .WithOne(i => i.Meal)
           .HasForeignKey(i => i.MealId)
           .OnDelete(DeleteBehavior.Cascade);
       
       
       builder.Entity<UserLikedRecommendation>()
           .HasOne(lr => lr.Recommendation)
           .WithMany()
           .HasForeignKey(lr => lr.RecommendationId)
           .OnDelete(DeleteBehavior.Cascade);
    }
    
}