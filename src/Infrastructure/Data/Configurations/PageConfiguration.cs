using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;

namespace WaffarXPartnerApi.Infrastructure.Data.Configurations;
public class PageConfiguration : IEntityTypeConfiguration<Page>
{
    public void Configure(EntityTypeBuilder<Page> builder)
    {

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasDefaultValueSql("NEWID()");

        builder.Property(p => p.PageName)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(p => p.PageName)
            .IsUnique();

        builder.Property(p => p.Description)
            .HasMaxLength(255);

        builder.Property(p => p.IsActive)
            .HasDefaultValue(true);

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(p => p.ClientApiId)
               .IsRequired();
        builder.Property(p => p.IsSuperAdminPage).IsRequired();

        builder.HasMany(p => p.PageActions)
               .WithOne(pa => pa.Page)
               .HasForeignKey(pa => pa.PageId);

      
    }

}
