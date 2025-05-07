using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;

namespace WaffarXPartnerApi.Infrastructure.Data.Configurations;
public class TeamPageActionConfiguration : IEntityTypeConfiguration<TeamPageAction>
{
    public void Configure(EntityTypeBuilder<TeamPageAction> builder)
    {
        builder.HasKey(tpa => tpa.Id);

        builder.Property(tpa => tpa.Id)
            .HasDefaultValueSql("NEWID()");

        builder.Property(tpa => tpa.TeamId)
            .IsRequired();

        builder.Property(tpa => tpa.PageActionId)
            .IsRequired();

        builder.Property(tpa => tpa.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(tpa => tpa.UpdatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(tpa => tpa.CreatedBy)
            .IsRequired();

        builder.HasOne(tpa => tpa.Team)
            .WithMany(t => t.TeamPageActions)
            .HasForeignKey(tpa => tpa.TeamId);

        //builder.HasOne(tpa => tpa.PageAction)
        //    .WithMany(p => p.TeamPageActions)
        //    .HasForeignKey(tpa => tpa.PageActionId);

        builder.HasOne(tpa => tpa.CreatedByUser)
            .WithMany(u => u.TeamPageActions)
            .HasForeignKey(tpa => tpa.CreatedBy);

        builder.HasIndex(tpa => new { tpa.TeamId, tpa.PageActionId })
            .HasDatabaseName("UQ_TeamPageAction_TeamId_PageId")
            .IsUnique();

        builder.HasIndex(tpa => tpa.TeamId)
            .HasDatabaseName("IX_TeamPageAction_TeamId");

        builder.HasIndex(tpa => tpa.PageActionId)
            .HasDatabaseName("IX_TeamPageAction_PageActionId");

        // Added index for CreatedBy
        builder.HasIndex(tpa => tpa.CreatedBy)
            .HasDatabaseName("IX_TeamPageAction_CreatedBy");
    }
}
