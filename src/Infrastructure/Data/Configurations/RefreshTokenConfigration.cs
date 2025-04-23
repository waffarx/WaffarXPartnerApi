﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;

namespace WaffarXPartnerApi.Infrastructure.Data.Configurations;
public class RefreshTokenConfigration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);
        builder.Property(rt => rt.Token).IsRequired();
        builder.HasIndex(rt => rt.Token).IsUnique();

        builder.HasOne(rt => rt.User)
              .WithMany(u => u.RefreshTokens)
              .HasForeignKey(rt => rt.UserId);
    }
}

