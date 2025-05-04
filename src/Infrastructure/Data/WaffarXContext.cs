using Microsoft.EntityFrameworkCore;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.WaffarXEntities;

namespace WaffarXPartnerApi.Infrastructure.Data;

public  class WaffarXContext : DbContext
{
    public WaffarXContext(DbContextOptions<WaffarXContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ApiClient> ApiClients { get; set; }
    public virtual DbSet<ApiClientCountry> ApiClientCountries { get; set; }
    public virtual DbSet<ApiClientToken> ApiClientTokens { get; set; }
    public virtual DbSet<ApiRefreshToken> ApiRefreshTokens { get; set; }
    public virtual DbSet<AppUsersClient> AppUsersClients { get; set; }
    public virtual DbSet<Vadvertiser> Vadvertisers { get; set; }
    public virtual DbSet<Advertiser> Advertisers { get; set; }
    public virtual DbSet<Resource> Resources { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<ApiClient>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("PK_Client");

            entity.ToTable("ApiClient");

            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.ClientName).HasMaxLength(50);
            entity.Property(e => e.Clientkey)
                .IsRequired()
                .HasMaxLength(450);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.ExpiresIn).HasColumnName("expires_in");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.Language)
                .HasMaxLength(50)
                .HasColumnName("language");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.Secret).HasMaxLength(500);
            entity.Property(e => e.TokenType)
                .HasMaxLength(50)
                .HasColumnName("token_type");
        });

        modelBuilder.Entity<ApiClientCountry>(entity =>
        {
            entity.ToTable("ApiClientCountry");

            entity.Property(e => e.ClientGuid).HasMaxLength(50);
            entity.Property(e => e.ClientId).HasColumnName("ClientID");

            entity.HasOne(d => d.Client).WithMany(p => p.ApiClientCountries)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApiClientCountry_ApiClient");
        });

        modelBuilder.Entity<ApiClientToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ApiClien__3214EC07ACCD9949");

            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.ClientKey).HasMaxLength(450);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Token)
                .IsUnicode(false)
                .HasColumnName("token");

            entity.HasOne(d => d.Client).WithMany(p => p.ApiClientTokens)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApiClientTokens_ApiClient");
        });

        modelBuilder.Entity<ApiRefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_RefreshToken");

            entity.ToTable("ApiRefreshToken");

            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.Clientkey).HasMaxLength(450);

            entity.HasOne(d => d.Client).WithMany(p => p.ApiRefreshTokens)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApiRefreshToken_ApiClient");
        });

        modelBuilder.Entity<AppUsersClient>(entity =>
        {
            entity.ToTable("AppUsers_Clients");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Cdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CDate");
            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserToken).HasMaxLength(50);

            entity.HasOne(d => d.Client).WithMany(p => p.AppUsersClients)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AppUsers_Clients_ApiClient");
        });

        modelBuilder.Entity<Vadvertiser>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToView("VAdvertisers");

            entity.Property(e => e.AdvertiserTitle).HasMaxLength(200);
            entity.Property(e => e.Advertisername).HasMaxLength(200);
            entity.Property(e => e.AdvertisernameAr).HasMaxLength(200);
            entity.Property(e => e.Categories).HasMaxLength(500);
            entity.Property(e => e.CautionOfRedirectionPageAr).HasColumnName("CautionOfRedirectionPageAR");
            entity.Property(e => e.Cdate)
                .HasColumnType("datetime")
                .HasColumnName("CDate");
            entity.Property(e => e.CoverImageUrl).HasColumnName("CoverImageURL");
            entity.Property(e => e.Currency).HasMaxLength(10);
            entity.Property(e => e.DeepLinkIos).HasColumnName("DeepLinkIOS");
            entity.Property(e => e.FlatPayout)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("flat_payout");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.IsIos).HasColumnName("IsIOS");
            entity.Property(e => e.LogoPng).HasMaxLength(100);
            entity.Property(e => e.MaxAdvertiserOption).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MobImage).HasMaxLength(150);
            entity.Property(e => e.MultipleFromDate).HasColumnType("datetime");
            entity.Property(e => e.MultipleToDate).HasColumnType("datetime");
            entity.Property(e => e.MultipleValue).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.PercentagePayout)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("percentage_payout");
            entity.Property(e => e.Programurl).HasMaxLength(500);
            entity.Property(e => e.ProviderId).HasColumnName("ProviderID");
            entity.Property(e => e.ShopButton).HasMaxLength(50);
            entity.Property(e => e.SolidColor).HasMaxLength(10);
            entity.Property(e => e.StoreGuid).HasColumnName("StoreGUID");
            entity.Property(e => e.Upto)
                .HasColumnType("decimal(38, 6)")
                .HasColumnName("UPTO");
            entity.Property(e => e.UserPercentage).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.WaffaeXlogo)
                .HasMaxLength(150)
                .HasColumnName("WaffaeXLogo");
            entity.Property(e => e.WaffarXpercentage)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("WaffarXPercentage");
            entity.Property(e => e.Was).HasColumnType("decimal(38, 6)");
        });

        modelBuilder.Entity<Advertiser>(entity =>
        {
            entity.ToTable("Advertisers");

            entity.HasIndex(e => e.Id, "ID")
                .IsDescending()
                .HasFillFactor(90);

            entity.HasIndex(e => new { e.Id, e.IsDeleted, e.IsActive }, "ID&IsActive&IsDeleted")
                .IsDescending(true, false, true)
                .HasFillFactor(90);

            entity.HasIndex(e => new { e.IsDeleted, e.IsActive, e.AdvertiserType }, "IDX_IsDeleted_IsActive_AdvertiserType+All");

            entity.HasIndex(e => new { e.IsDeleted, e.IsActive }, "IsActive&IsDeleted")
                .IsDescending(false, true)
                .HasFillFactor(90);

            entity.HasIndex(e => e.Id, "NonClusteredIndex-20171123-040301")
                .IsDescending()
                .HasFillFactor(90);

            entity.HasIndex(e => new { e.Id, e.IsActive }, "_dta_index_Advertisers_5_1802691835__K1_K26_2_8_9_18_64_67");

            entity.HasIndex(e => new { e.Id, e.IsActive, e.IsDeleted }, "_dta_index_Advertisers_5_1802691835__K1_K26_K25_2_3_4_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_9987").HasFilter("([Advertisers].[IsDeleted]=(0) AND [Advertisers].[IsActive]=(1))");

            entity.HasIndex(e => new { e.IsDeleted, e.IsActive, e.Id }, "_dta_index_Advertisers_5_1802691835__K25_K26_K1_37_8809");

            entity.HasIndex(e => new { e.IsDeleted, e.IsActive, e.Id }, "_dta_index_Advertisers_5_1802691835__K25_K26_K1_6960");

            entity.HasIndex(e => new { e.IsDeleted, e.IsActive, e.IsExtension }, "_dta_index_Advertisers_5_1802691835__K25_K26_K52_1_10");

            entity.HasIndex(e => new { e.IsActive, e.Id }, "_dta_index_Advertisers_5_1802691835__K26_K1_2_8_9_18_19_22_23_27_58_59_60_64_67");

            entity.HasIndex(e => new { e.ProviderId, e.Advertisername, e.Childcategory, e.Parentcategory }, "_dta_index_Advertisers_5_1802691835__K2_K8_K16_K15_1_3_4_5_6_7_10_11_12_13_14_17_18_19_20_22_23_24_25_26_27_29_32_33_35_38_39_");

            entity.HasIndex(e => new { e.Parentcategory, e.IsActive, e.Id }, "_dta_index_Advertisers_7_1447884425__K14_K24_K1_2_8_17_20_25_31").HasFillFactor(90);

            entity.HasIndex(e => e.Id, "_dta_index_Advertisers_7_1447884425__K1D_31")
                .IsDescending()
                .HasFillFactor(90);

            entity.HasIndex(e => new { e.Id, e.IsActive }, "_dta_index_Advertisers_7_1447884425__K1_K24_2_8_17_20_25_27_31").HasFillFactor(90);

            entity.HasIndex(e => new { e.Id, e.IsActive }, "_dta_index_Advertisers_7_2011456901__K1_K26_2_8_9_18_19_22_23_27_58_59_60");

            entity.HasIndex(e => new { e.Id, e.IsActive, e.IsDeleted }, "_dta_index_Advertisers_7_2011456901__K1_K26_K25_2_3_4_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_23_24_27_28_29_30_31_32_");

            entity.HasIndex(e => new { e.IsDeleted, e.IsActive, e.Advertisername }, "_dta_index_Advertisers_7_2011456901__K25_K26_K8_1_2_3_4_5_6_7_9_10_11_12_13_14_15_16_17_18_19_20_21_22_23_24_27_28_29_30_31_32_");

            entity.HasIndex(e => new { e.IsActive, e.IsDeleted, e.Id }, "_dta_index_Advertisers_7_2011456901__K26_K25_K1");

            entity.HasIndex(e => new { e.Advertiserid, e.ProviderId }, "_dta_index_Advertisers_7_2011456901__K3_K2_1_4_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19_20_21_22_23_24_25_26_27_28_29_30_31_32_");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AdvertiserTitle).HasMaxLength(200);
            entity.Property(e => e.AdvertiserType).HasComment("1= Online, 2= Instore, 3= Both");
            entity.Property(e => e.Advertisername).HasMaxLength(200);
            entity.Property(e => e.AdvertisernameAr).HasMaxLength(200);
            entity.Property(e => e.Categories).HasMaxLength(500);
            entity.Property(e => e.CautionOfRedirectionPageAr).HasColumnName("CautionOfRedirectionPageAR");
            entity.Property(e => e.Cdate)
                .HasColumnType("datetime")
                .HasColumnName("CDate");
            entity.Property(e => e.Childcategory).HasMaxLength(200);
            entity.Property(e => e.CoverImageUrl).HasColumnName("CoverImageURL");
            entity.Property(e => e.Currency).HasMaxLength(10);
            entity.Property(e => e.DeepLinkIos).HasColumnName("DeepLinkIOS");
            entity.Property(e => e.ExitClickMsgResource).HasMaxLength(100);
            entity.Property(e => e.FlatPayout)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("flat_payout");
            entity.Property(e => e.IsIos).HasColumnName("IsIOS");
            entity.Property(e => e.IsMobileIcon).HasDefaultValue(false);
            entity.Property(e => e.Language).HasMaxLength(10);
            entity.Property(e => e.Logo).HasMaxLength(500);
            entity.Property(e => e.LogoPng).HasMaxLength(100);
            entity.Property(e => e.MobImage).HasMaxLength(150);
            entity.Property(e => e.MultipleFromDate).HasColumnType("datetime");
            entity.Property(e => e.MultipleToDate).HasColumnType("datetime");
            entity.Property(e => e.MultipleValue).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.OldPercentagePayout)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("old_percentage_payout");
            entity.Property(e => e.PercentagePayout)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("percentage_payout");
            entity.Property(e => e.Programurl).HasMaxLength(500);
            entity.Property(e => e.ProviderId).HasColumnName("ProviderID");
            entity.Property(e => e.Relationshipstatus).HasComment("1 = Joind, 0 = Else");
            entity.Property(e => e.SeoDescrAr).HasMaxLength(300);
            entity.Property(e => e.SeoDescrEn).HasMaxLength(300);
            entity.Property(e => e.SeoTitleAr).HasMaxLength(300);
            entity.Property(e => e.SeoTitleEn).HasMaxLength(300);
            entity.Property(e => e.Sevendayepc).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ShopButton).HasMaxLength(50);
            entity.Property(e => e.SocialDescrAr).HasMaxLength(300);
            entity.Property(e => e.SocialDescrEn).HasMaxLength(300);
            entity.Property(e => e.SocialTitleAr).HasMaxLength(300);
            entity.Property(e => e.SocialTitleEn).HasMaxLength(300);
            entity.Property(e => e.SolidColor).HasMaxLength(10);
            entity.Property(e => e.StoreGuid).HasColumnName("StoreGUID");
            entity.Property(e => e.StoreRedirectionType)
                .HasDefaultValue(1)
                .HasComment("1 = open in app webview, 2 = make user choose, 3 = force to open in external app");
            entity.Property(e => e.Threemonthepc).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UserPercentage)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UserValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.WaffaeXlogo)
                .HasMaxLength(150)
                .HasColumnName("WaffaeXLogo");
            entity.Property(e => e.WaffarXpercentage)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("WaffarXPercentage");
            entity.Property(e => e.XContent)
                .HasMaxLength(50)
                .HasComment("Use to differentiate ads")
                .HasColumnName("X_Content");
            entity.Property(e => e.XMedium)
                .HasMaxLength(100)
                .HasComment("Marketing medium: (e.g. cpc, banner, email)")
                .HasColumnName("X_Medium");
            entity.Property(e => e.XName)
                .HasMaxLength(50)
                .HasComment("Product, promo code, or slogan (e.g. spring_sale)")
                .HasColumnName("X_Name");
            entity.Property(e => e.XSource)
                .HasMaxLength(100)
                .HasComment("The referrer: (e.g. google, newsletter)")
                .HasColumnName("X_Source");
            entity.Property(e => e.XTerm)
                .HasMaxLength(50)
                .HasComment("Identify the paid keywords e.g(search terms)")
                .HasColumnName("X_Term");
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("Resources");
            entity.Property(e => e.ValueSa).HasColumnName("ValueSA");
        });

        base.OnModelCreating(modelBuilder);
    }

}
