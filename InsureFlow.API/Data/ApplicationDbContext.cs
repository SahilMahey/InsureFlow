using Microsoft.EntityFrameworkCore;
using InsureFlow.API.Models;

namespace InsureFlow.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Application> Applications { get; set; }

    public DbSet<Policy> Policies { get; set; }
    public DbSet<UnderwritingRule> UnderwritingRules { get; set; }
    public DbSet<User> Users { get; set; }
}
