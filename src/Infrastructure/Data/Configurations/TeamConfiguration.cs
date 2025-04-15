using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;

namespace WaffarXPartnerApi.Infrastructure.Data.Configurations;
public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        // Changed from TeamId to Id
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
               .HasDefaultValueSql("NEWID()");

        builder.Property(t => t.TeamName)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(t => t.TeamName)
            .IsUnique();

        builder.Property(t => t.Description)
            .HasMaxLength(255);

        builder.Property(t => t.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(t => t.UpdatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(p => p.ClientApiId)
            .IsRequired();
        // Configure relationships
        builder.HasMany(t => t.TeamPageActions)
            .WithOne(tpa => tpa.Team)
            .HasForeignKey(tpa => tpa.TeamId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.UserTeams)
            .WithOne(ut => ut.Team)
            .HasForeignKey(ut => ut.TeamId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

