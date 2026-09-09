using MiniEnv.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MiniEnv.Infrastructure.Persistence;

public class MiniEnvDbContext : DbContext
{
    public MiniEnvDbContext(DbContextOptions<MiniEnvDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(MiniEnvDbContext).Assembly
        );

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserCredential> UserCredentials { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<SignUp> SignUps { get; set; }
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
    public DbSet<Pipeline> Pipelines { get; set; }
    public DbSet<Stage> Stages { get; set; }
    public DbSet<Deal> Deals { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<DealCustomer> DealCustomers { get; set; }
    public DbSet<DealObserver> DealObservers { get; set; }
    public DbSet<Invitation> Invitations { get; set; }
}