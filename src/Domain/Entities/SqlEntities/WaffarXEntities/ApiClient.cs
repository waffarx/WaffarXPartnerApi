namespace WaffarXPartnerApi.Domain.Entities.SqlEntities.WaffarXEntities;

public partial class ApiClient
{
    public int ClientId { get; set; }

    public string Clientkey { get; set; } = null!;

    public string Id { get; set; }

    public string ClientName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Language { get; set; }

    public long ExpiresIn { get; set; }

    public string TokenType { get; set; }

    public string Secret { get; set; }

    public string Email { get; set; }

    public bool? IsActive { get; set; }

    public bool IsOnProduction { get; set; }

    public virtual ICollection<ApiClientCountry> ApiClientCountries { get; set; } = new List<ApiClientCountry>();

    public virtual ICollection<ApiClientToken> ApiClientTokens { get; set; } = new List<ApiClientToken>();

    public virtual ICollection<ApiRefreshToken> ApiRefreshTokens { get; set; } = new List<ApiRefreshToken>();

    public virtual ICollection<AppUsersClient> AppUsersClients { get; set; } = new List<AppUsersClient>();
}
