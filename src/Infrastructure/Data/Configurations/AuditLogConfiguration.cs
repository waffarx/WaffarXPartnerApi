using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;

namespace WaffarXPartnerApi.Infrastructure.Data.Configurations;
public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {

        builder.HasKey(al => al.Id);

        builder.Property(al => al.Id)
            .HasDefaultValueSql("NEWID()");

        builder.Property(al => al.UserId)
            .IsRequired();

        builder.Property(al => al.ActionType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(al => al.EntityType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(al => al.OldValues)
            .HasColumnType("NVARCHAR(MAX)");

        builder.Property(al => al.NewValues)
            .HasColumnType("NVARCHAR(MAX)");

        builder.Property(al => al.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        // Explicitly configure EntityId which was commented in the entity
        builder.Property(al => al.EntityId);

        builder.Property(al => al.ClientApiId)
            .IsRequired();

        builder.HasIndex(al => al.UserId)
            .HasDatabaseName("IX_AuditLogs_UserId");

        builder.HasIndex(al => al.CreatedAt)
            .HasDatabaseName("IX_AuditLogs_Timestamp");

        builder.HasIndex(al => new { al.EntityType })
            .HasDatabaseName("IX_AuditLogs_EntityId");

        builder.HasOne(al => al.User)
            .WithMany(u => u.AuditLogs)
            .HasForeignKey(al => al.UserId);
    }
}
