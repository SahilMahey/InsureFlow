using InsureFlow.API.Data;
using InsureFlow.API.Services;
using InsureFlow.API.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Read JWT key from configuration (User Secrets included automatically in dev)
var jwtKey = builder.Configuration["Jwt:Key"];
var keyBytes = Encoding.ASCII.GetBytes(jwtKey);

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
// CORS POLICY
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// DATABASE
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=InsureFlow.db"));

// SERVICES
builder.Services.AddScoped<UnderwritingService>();
builder.Services.AddScoped<PremiumService>();
builder.Services.AddScoped<PolicyService>();

// CONTROLLERS
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// ======================
// DATABASE SEEDING
// ======================

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    db.Database.EnsureCreated();

    // Seed underwriting rules
    if (!db.UnderwritingRules.Any())
    {
        db.UnderwritingRules.AddRange(
            new UnderwritingRule { Field = "AnnualRevenue", Operator = ">", Value = "5000000", RiskPoints = 20 },
            new UnderwritingRule { Field = "ClaimsHistory", Operator = ">", Value = "2", RiskPoints = 30 },
            new UnderwritingRule { Field = "BusinessType", Operator = "==", Value = "Construction", RiskPoints = 25 },
            new UnderwritingRule { Field = "YearsInBusiness", Operator = "<", Value = "2", RiskPoints = 15 }
        );
    }

    // Seed users
    if (!db.Users.Any())
    {
        db.Users.AddRange(
            new User
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = "Admin"
            },
            new User
            {
                Username = "agent",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("agent123"),
                Role = "Agent"
            },
            new User
            {
                Username = "underwriter",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("under123"),
                Role = "Underwriter"
            }
        );
    }

    db.SaveChanges();
}


// ======================
// MIDDLEWARE PIPELINE
// ======================

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();