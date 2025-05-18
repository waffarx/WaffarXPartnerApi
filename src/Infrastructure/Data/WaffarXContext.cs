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
    public virtual DbSet<ApiClientExitClick> ApiClientExitClicks { get; set; }
    public virtual DbSet<CashBack> CashBacks { get; set; }
    public virtual DbSet<ExitClick> ExitClicks { get; set; }
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

        modelBuilder.Entity<ApiClientExitClick>(entity =>
        {
            entity.Property(e => e.ApiClientId).HasComment("ClientId In ApiClient Table");
            entity.Property(e => e.AppUserId).HasComment("Id of Client AppUser");
            entity.Property(e => e.HasCashback).HasComment("Just Flag Setted by partner to tell if the ExitClick has cashback or not");
            entity.Property(e => e.SubId1).HasMaxLength(100);
            entity.Property(e => e.SubId2).HasMaxLength(100);
            entity.Property(e => e.Uid)
                .HasMaxLength(100)
                .HasComment("Identifier Sent From Partner")
                .HasColumnName("UID");
        });

        modelBuilder.Entity<CashBack>(entity =>
        {
            entity.ToTable(tb =>
            {
                tb.HasComment("Marks the state of the exit click; 0 = Unresolved - 1 = Pending - 2 = Accepted - 3 = Rejected(Cancelled) - 4 = Has Error (Ticket)");
                tb.HasTrigger("TRGUpdateColumns");
                tb.HasTrigger("TRG_CashBack");
                tb.HasTrigger("TRG_CashBackLog");
                tb.HasTrigger("TRG_CashOutUpdate");
                tb.HasTrigger("TRG_DINO");
                tb.HasTrigger("TRG_FBOfflineConversion");
                tb.HasTrigger("TRG_FBPixel");
                tb.HasTrigger("TRG_SendAdjustData");
            });

            entity.HasIndex(e => new { e.CashBackTypeId, e.IsResolved, e.StatusId }, "CashBackTypeID_IsResolved_StatusID");

            entity.HasIndex(e => new { e.CashBackTypeId, e.AdvertiserId }, "CashBackTypeId&AdvertiserID");

            entity.HasIndex(e => new { e.CashBackTypeId, e.UserId }, "CashBackTypeId&UserId");

            entity.HasIndex(e => new { e.CashBackTypeId, e.StatusId, e.Date, e.UserId }, "CashBackTypeId_StatusId_Date_UserId+all");

            entity.HasIndex(e => e.Date, "Date");

            entity.HasIndex(e => new { e.Date, e.UserId }, "Date&UserID").HasFillFactor(90);

            entity.HasIndex(e => e.ExitClickId, "ExitClickID");

            entity.HasIndex(e => e.Id, "ID")
                .IsDescending()
                .HasFillFactor(90);

            entity.HasIndex(e => new { e.Id, e.ConversionId }, "ID&ConversionID")
                .IsDescending()
                .HasFillFactor(90);

            entity.HasIndex(e => new { e.AdvertiserId, e.StatusId, e.UserId }, "IDX_AdvertiserID_StatusId_UserId+Date+UserConvertedValue+OrderValueUSD");

            entity.HasIndex(e => e.CashBackTypeId, "IDX_CashBackTypeId+AllUserValues");

            entity.HasIndex(e => new { e.CashBackTypeId, e.AdvertiserId, e.StatusId }, "IDX_CashBackTypeId_AdvertiserID_StatusId");

            entity.HasIndex(e => new { e.CashBackTypeId, e.Date }, "IDX_CashBackTypeId_Date");

            entity.HasIndex(e => new { e.CashBackTypeId, e.IsResolved, e.StatusId }, "IDX_CashBackTypeId_IsResolved_StatusId+Id+Date+ConvertedValue+UserConvertedValue");

            entity.HasIndex(e => new { e.CashBackTypeId, e.StatusId }, "IDX_CashBackTypeId_StatusId+Date+UserId+IsResolved+UserConvertedValue");

            entity.HasIndex(e => new { e.CashBackTypeId, e.StatusId, e.Date }, "IDX_CashBackTypeId_StatusId_Date+UserId+ExitClickID+UserConvertedValue+WaffarXConvertedValue+OrderValueUSD+ConvertedValueUSD");

            entity.HasIndex(e => new { e.CashBackTypeId, e.StatusId, e.Date, e.UserId, e.AdvertiserId }, "IDX_CashBackTypeId_StatusId_Date_UserId_AdvertiserID+UserConvertedValue+WaffarXConvertedValue+OrderValueUSD+ConvertedValueUSD");

            entity.HasIndex(e => e.Date, "IDX_Date+UserId+AdvertiserID+OrderValueEGP");

            entity.HasIndex(e => new { e.ProviderId, e.ConvertedValue }, "IDX_ProviderID_ConvertedValue+OrderID");

            entity.HasIndex(e => new { e.StatusId, e.UserId }, "IDX_StatusID_UserID+Id");

            entity.HasIndex(e => new { e.StatusId, e.CashBackTypeId, e.AdvertiserId }, "IDX_StatusId_CashBackTypeId_AdvertiserID+UserId");

            entity.HasIndex(e => new { e.StatusId, e.IsResolved, e.CashBackTypeId }, "IDX_StatusId_IsResolved_CashBackTypeId");

            entity.HasIndex(e => e.UserId, "IDX_UserID+UserConvertedValues");

            entity.HasIndex(e => e.ApiconversionId, "IX_ApiConversionID");

            entity.HasIndex(e => new { e.CashBackTypeId, e.AdvertiserId }, "IX_CashBackTypeId_AdvertiserID");

            entity.HasIndex(e => new { e.CashBackTypeId, e.Date, e.ExitClickId }, "IX_CashBackTypeId_Date_ExitClickID");

            entity.HasIndex(e => new { e.CashBackTypeId, e.ProviderId, e.IsResolved }, "IX_CashBackTypeId_ProviderID_IsResolved");

            entity.HasIndex(e => new { e.CashBackTypeId, e.StatusId, e.Date }, "IX_CashBackTypeId_StatusId_Date");

            entity.HasIndex(e => e.Date, "IX_Date");

            entity.HasIndex(e => new { e.Date, e.UserId }, "IX_Date_UserIDWithCols");

            entity.HasIndex(e => new { e.StatusId, e.UserId }, "IX_StatusID_UserID+Id");

            entity.HasIndex(e => new { e.UserId, e.AdvertiserId }, "IX_UserID_AdvertiserID");

            entity.HasIndex(e => e.ReferalLogId, "ReferalLogID");

            entity.HasIndex(e => new { e.StatusId, e.AdvertiserId }, "Status&StoreWithUser");

            entity.HasIndex(e => e.UserId, "UserID");

            entity.HasIndex(e => e.UserId, "UserIDDESC").IsDescending();

            entity.HasIndex(e => new { e.Id, e.CashBackTypeId, e.UserId }, "_dta_index_CashBacks_5_1489878580__K1_K12_K7");

            entity.HasIndex(e => new { e.CashBackTypeId, e.Date, e.UserId, e.StatusId }, "_dta_index_CashBacks_7_1489878580__K12_K6_K7_K5_1_29_30_33");

            entity.HasIndex(e => new { e.CashBackTypeId, e.AdvertiserId }, "_dta_index_CashBacks_7_2117180818__K12_K15_f110").HasFilter("([CashBacks].[AdvertiserID]>(0))");

            entity.HasIndex(e => new { e.CashBackTypeId, e.Date, e.StatusId, e.UserId }, "_dta_index_CashBacks_7_2117180818__K12_K6_K5_K7_4_9_10_20");

            entity.HasIndex(e => new { e.CashBackTypeId, e.Date, e.UserId }, "_dta_index_CashBacks_7_2117180818__K12_K6_K7");

            entity.HasIndex(e => new { e.CashBackTypeId, e.UserId, e.Date }, "_dta_index_CashBacks_7_2117180818__K12_K7_K6");

            entity.HasIndex(e => new { e.CashBackTypeId, e.UserId, e.Date, e.StatusId }, "_dta_index_CashBacks_7_2117180818__K12_K7_K6_K5");

            entity.HasIndex(e => e.AdvertiserId, "_dta_index_CashBacks_7_2117180818__K15");

            entity.HasIndex(e => new { e.ConversionId, e.AdvertiserId, e.ExitClickId, e.ProviderId, e.Id }, "_dta_index_CashBacks_7_2117180818__K17_K15_K16_K13_K1_2_3_4_5_6_7_8_9_10_11_12_14_18_19_20_21_22_23");

            entity.HasIndex(e => new { e.UserId, e.AdvertiserId }, "ix_UserId_AdvertiserId+StatusId");

            entity.Property(e => e.AdvertiserId).HasColumnName("AdvertiserID");
            entity.Property(e => e.AdvertiserName).HasMaxLength(150);
            entity.Property(e => e.ApiconversionId).HasColumnName("APIConversionID");
            entity.Property(e => e.BankId).HasColumnName("BankID");
            entity.Property(e => e.Bonus)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.Cdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CDate");
            entity.Property(e => e.CommissionType).HasDefaultValue(0);
            entity.Property(e => e.ConversionId).HasColumnName("ConversionID");
            entity.Property(e => e.ConvertedValue)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ConvertedValueUsd)
                .HasColumnType("money")
                .HasColumnName("ConvertedValueUSD");
            entity.Property(e => e.CurrencySymbol)
                .HasMaxLength(50)
                .HasColumnName("currency_symbol");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.ExitClickId).HasColumnName("ExitClickID");
            entity.Property(e => e.Gclid)
                .HasMaxLength(1000)
                .HasColumnName("GCLID");
            entity.Property(e => e.IsFixed).HasDefaultValue(false);
            entity.Property(e => e.IsMailSent).HasDefaultValue(true);
            entity.Property(e => e.NetworkCommision).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OrderCurrencySymbol)
                .HasMaxLength(50)
                .HasColumnName("order_currency_symbol");
            entity.Property(e => e.OrderId)
                .HasMaxLength(100)
                .HasColumnName("Order_ID");
            entity.Property(e => e.OrderValue)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OrderValueEgp)
                .HasColumnType("decimal(18, 6)")
                .HasColumnName("OrderValueEGP");
            entity.Property(e => e.OrderValueSar)
                .HasColumnType("decimal(18, 6)")
                .HasColumnName("OrderValueSAR");
            entity.Property(e => e.OrderValueUsd)
                .HasColumnType("decimal(18, 6)")
                .HasColumnName("OrderValueUSD");
            entity.Property(e => e.OrginalValue)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.PaymentCycleId).HasColumnName("PaymentCycleID");
            entity.Property(e => e.PendingValue)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.ProviderId).HasColumnName("ProviderID");
            entity.Property(e => e.ReferalLogId).HasColumnName("ReferalLogID");
            entity.Property(e => e.UserBonusCommission)
                .HasDefaultValue(100m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UserConvertedValue).HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UserConvertedValueEgp)
                .HasColumnType("decimal(18, 6)")
                .HasColumnName("UserConvertedValueEGP");
            entity.Property(e => e.UserConvertedValueSar)
                .HasColumnType("decimal(18, 6)")
                .HasColumnName("UserConvertedValueSAR");
            entity.Property(e => e.UserId).HasDefaultValue(0);
            entity.Property(e => e.WaffarXconvertedValue)
                .HasColumnType("decimal(18, 6)")
                .HasColumnName("WaffarXConvertedValue");
            entity.Property(e => e.WaffarXconvertedValueEgp)
                .HasColumnType("decimal(18, 6)")
                .HasColumnName("WaffarXConvertedValueEGP");
            entity.Property(e => e.WaffarxConvertedValueSar)
                .HasColumnType("decimal(18, 6)")
                .HasColumnName("WaffarxConvertedValueSAR");
        });

        modelBuilder.Entity<ExitClick>(entity =>
        {
            entity.ToTable("ExitClick", tb => tb.HasComment("Marks the state of the exit click; 0 = Unresolved - 1 = Pending - 2 = Accepted - 3 = Rejected(Cancelled) - 4 = Has Error (Ticket)"));

            entity.HasIndex(e => new { e.ClickSource, e.UserId }, "ClickSource_UserID");

            entity.HasIndex(e => e.AdvertiserId, "IX_AdvertiserID&UserID");

            entity.HasIndex(e => e.Cdate, "IX_ExitClick_Cdate_UserID");

            entity.HasIndex(e => e.UserId, "IX_ExitClick_UserID").HasFillFactor(90);

            entity.HasIndex(e => e.UserId, "IX_ExitClick_UserID2").HasFillFactor(90);

            entity.HasIndex(e => new { e.UserId, e.AdvertiserId }, "IX_ExitClick_UserID_AdvertiserID").HasFillFactor(90);

            entity.HasIndex(e => new { e.Id, e.UserId, e.AdvertiserId, e.ProviderId, e.CategoryId, e.ProductId }, "NonClusteredAll")
                .IsDescending(true, false, true, false, false, true)
                .HasFillFactor(90);

            entity.HasIndex(e => new { e.Id, e.Cdate }, "NonClusteredDate")
                .IsDescending()
                .HasFillFactor(90);

            entity.HasIndex(e => new { e.UserId, e.HasTransaction }, "NonClusteredIndex-UserID-HasTransaction").IsDescending(false, true);

            entity.HasIndex(e => new { e.Cdate, e.HasTransaction }, "NonClusteredIndex_HasTransactionAndCDate").IsDescending();

            entity.HasIndex(e => e.UserId, "NonClusteredUserID")
                .IsDescending()
                .HasFillFactor(90);

            entity.HasIndex(e => new { e.ClickSource, e.UserId }, "_dta_index_ExitClick_7_1510036600__K15_K2_3_11");

            entity.HasIndex(e => e.Cdate, "_dta_index_ExitClick_7_1510036600__K3");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AdvertiserCommision).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AdvertiserId).HasColumnName("AdvertiserID");
            entity.Property(e => e.AdvertiserPayout).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BankId).HasColumnName("BankID");
            entity.Property(e => e.Bonus)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.CategoryId)
                .HasDefaultValue(0)
                .HasColumnName("CategoryID");
            entity.Property(e => e.Cdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ClickSource)
                .HasDefaultValue((short)0)
                .HasComment("0 =WaffarX, 1 = MobileApiAndroid, 2 = MobileApiIOS, 3 = BrowserExtension, 7 = Client Api");
            entity.Property(e => e.DeeplinkIos).HasColumnName("DeeplinkIOS");
            entity.Property(e => e.ExitType).HasMaxLength(50);
            entity.Property(e => e.Fbc)
                .HasMaxLength(150)
                .HasColumnName("fbc");
            entity.Property(e => e.Fbp)
                .HasMaxLength(50)
                .HasColumnName("fbp");
            entity.Property(e => e.HasTransaction).HasDefaultValue(false);
            entity.Property(e => e.IsFixed).HasDefaultValue(false);
            entity.Property(e => e.IsMobile).HasDefaultValue(false);
            entity.Property(e => e.IsResolved).HasDefaultValue(0);
            entity.Property(e => e.MobileType)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.ProductId)
                .HasDefaultValue(0)
                .HasColumnName("ProductID");
            entity.Property(e => e.ProviderId).HasColumnName("ProviderID");
            entity.Property(e => e.UserBonusCommission)
                .HasDefaultValue(100m)
                .HasColumnType("decimal(18, 6)");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Xclid).HasColumnName("XCLID");
        });

        base.OnModelCreating(modelBuilder);
    }

}
