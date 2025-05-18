using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;

namespace WaffarXPartnerApi.Infrastructure.Data.Configurations;
public class ClientPageConfigration : IEntityTypeConfiguration<ClientPage>
{
    public void Configure(EntityTypeBuilder<ClientPage> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.ClientApiId).IsRequired();
        builder.Property(x => x.PageId).IsRequired();
        builder.HasOne(x => x.Page).WithMany().HasForeignKey(x => x.PageId);
    }
}
