using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;

namespace WaffarXPartnerApi.Infrastructure.Data;

public class WaffarXPartnerDbContext : DbContext
{
    public WaffarXPartnerDbContext(DbContextOptions<WaffarXPartnerDbContext> options)
            : base(options)
    {
    }

    // DbSets for all entities
    public DbSet<User> Users { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Page> Pages { get; set; }
    public DbSet<PageAction> PageActions { get; set; }
    public DbSet<TeamPageAction> TeamPageActions { get; set; }
    public DbSet<UserTeam> UserTeams { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
