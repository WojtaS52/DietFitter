using System.Security.Claims;
using DietFitter_backend.Database;
using DietFitter_backend.Extensions;
using DietFitter_backend.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DietFitter_backend.Services; 

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") 
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); 
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
        options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    })
    .AddCookie(IdentityConstants.ApplicationScheme)
    .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<UserStatsService>();
builder.Services.AddScoped<AlgorithmService>();
builder.Services.AddScoped<DietRecommendationService>();
builder.Services.AddScoped<FoodProductRepository>();
builder.Services.AddScoped<IFoodProductRepository, FoodProductRepository>();
builder.Services.AddScoped<UserDietRecommendationRepository>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); 


// Przykładowy endpoint chroniony
app.MapGet("users/me", async (ClaimsPrincipal claims, ApplicationDbContext context) =>
    {
        string userId = claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        return await context.Users.FindAsync(userId);
    }
).RequireAuthorization();

app.MapIdentityApi<User>();
app.Run();