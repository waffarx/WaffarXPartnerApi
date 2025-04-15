using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;

namespace WaffarXPartnerApi.Infrastructure.Data.Configurations;
public class PageActionConfiguration : IEntityTypeConfiguration<PageAction>
{
    public void Configure(EntityTypeBuilder<PageAction> builder)
    {

        builder.HasKey(pa => pa.Id);

        builder.Property(pa => pa.Id)
            .HasDefaultValueSql("NEWID()");

        builder.Property(pa => pa.PageId)
            .IsRequired();

        builder.Property(pa => pa.ActionName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(pa => pa.Description)
            .HasMaxLength(255);

        builder.Property(pa => pa.IsActive)
            .HasDefaultValue(true);

        builder.Property(pa => pa.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(pa => pa.UpdatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.HasIndex(pa => pa.PageId)
            .HasDatabaseName("IX_PageActions_PageId");

        builder.HasOne(pa => pa.Page)
            .WithMany(p => p.PageActions)
            .HasForeignKey(pa => pa.PageId);

        builder.HasIndex(pa => new { pa.PageId, pa.ActionName })
            .HasDatabaseName("UQ_PageActions_PageId_ActionName")
            .IsUnique();
    }
}
