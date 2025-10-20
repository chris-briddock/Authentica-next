using Authentica.Service.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System.Reflection;

namespace Authentica.Service.Identity.Persistence.Contexts;

public sealed class AppDbContext : DbContext
{
    public IConfiguration Configuration { get; }

    public AppDbContext(DbContextOptions opt, IConfiguration configuration) : base(opt)
    {
        Configuration = configuration;

    }

    /// <summary>
    /// Configures the DbContext options such as database connection string and retry policy.
    /// </summary>
    /// <param name="optionsBuilder">The options builder used to configure DbContext options.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), opt =>
            {
                opt.EnableRetryOnFailure();
            });


        }
    }

    /// <summary>
    /// Applies entity configurations.
    /// </summary>
    /// <param name="modelBuilder">Model builder instance.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Role> Roles => Set<Role>();
    public DbSet<RoleClaim> RoleClaims => Set<RoleClaim>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserClaim> UserClaims => Set<UserClaim>();
    public DbSet<UserLogin> UserLogins => Set<UserLogin>();
    public DbSet<UserMultiFactorSettings> UserMultiFactorSettings => Set<UserMultiFactorSettings>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();
    public DbSet<UserToken> UserTokens => Set<UserToken>();
}