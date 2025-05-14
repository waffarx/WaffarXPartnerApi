using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;

namespace WaffarXPartnerApi.Infrastructure.Data.Configurations;
public class PartnerPostbackConfiguration : IEntityTypeConfiguration<PartnerPostback>
{
    public void Configure(EntityTypeBuilder<PartnerPostback> builder)
    {
       
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
               .HasDefaultValueSql("NEWID()");

        builder.Property(t => t.PostbackUrl)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(t => t.PostbackType)
            .IsRequired();
        
        builder.Property(t => t.PostbackMethod)
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(t => t.UpdatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder.Property(p => p.ClientApiId)
            .IsRequired();
      
    }
}
