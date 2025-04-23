using Microsoft.EntityFrameworkCore;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.WaffarXEntities;

namespace WaffarXPartnerApi.Infrastructure.Data;

public partial class WaffarXContext : DbContext
{
    public WaffarXContext()
    {
    }

    public WaffarXContext(DbContextOptions<WaffarXContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ApiClient> ApiClients { get; set; }

    public virtual DbSet<ApiClientCountry> ApiClientCountries { get; set; }

    public virtual DbSet<ApiClientToken> ApiClientTokens { get; set; }

    public virtual DbSet<ApiRefreshToken> ApiRefreshTokens { get; set; }

    public virtual DbSet<AppUsersClient> AppUsersClients { get; set; }

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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
