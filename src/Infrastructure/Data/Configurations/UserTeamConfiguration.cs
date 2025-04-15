using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;

namespace WaffarXPartnerApi.Infrastructure.Data.Configurations;
public class UserTeamConfiguration : IEntityTypeConfiguration<UserTeam>
{
    public void Configure(EntityTypeBuilder<UserTeam> builder)
    {
        builder.HasKey(ut => ut.Id);

        builder.Property(ut => ut.Id)
            .HasDefaultValueSql("NEWID()");

        builder.Property(ut => ut.UserId)
            .IsRequired();

        builder.Property(ut => ut.TeamId)
            .IsRequired();

        builder.Property(ut => ut.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(ut => ut.UpdatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.HasIndex(ut => new { ut.UserId, ut.TeamId })
            .IsUnique();

        builder.HasOne(ut => ut.User)
            .WithMany(u => u.UserTeams)
            .HasForeignKey(ut => ut.UserId);

        builder.HasOne(ut => ut.Team)
            .WithMany(t => t.UserTeams)
            .HasForeignKey(ut => ut.TeamId);
    }
}
