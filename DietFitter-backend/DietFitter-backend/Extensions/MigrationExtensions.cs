
using DietFitter_backend.Database;
using Microsoft.EntityFrameworkCore;

namespace DietFitter_backend.Extensions;

public static class MigrationExtensions
{

    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        
        using ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        context.Database.Migrate();
        
    }
}