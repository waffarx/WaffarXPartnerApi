using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;
using WaffarXPartnerApi.Infrastructure.Data.Configurations;

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
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<PartnerPostback> PartnerPostbacks { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new AuditLogConfiguration());
        builder.ApplyConfiguration(new PageActionConfiguration());
        builder.ApplyConfiguration(new PageConfiguration());
        builder.ApplyConfiguration(new TeamConfiguration());
        builder.ApplyConfiguration(new TeamPageActionConfiguration());
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new UserTeamConfiguration());
        builder.ApplyConfiguration(new RefreshTokenConfigration());
        builder.ApplyConfiguration(new PartnerPostbackConfiguration());
    }
}
public class WaffarXPartnerDbContextFactory : IDesignTimeDbContextFactory<WaffarXPartnerDbContext>
{
    public WaffarXPartnerDbContext CreateDbContext(string[] args)
    {

        var connectionString = "Server=34.73.19.195;Database=WaffarxPartners;user id=wx-partners;password=r1trERlb@6ci4$e;MultipleActiveResultSets=true;TrustServerCertificate=True;";

        var optionsBuilder = new DbContextOptionsBuilder<WaffarXPartnerDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new WaffarXPartnerDbContext(optionsBuilder.Options);
    }
}
