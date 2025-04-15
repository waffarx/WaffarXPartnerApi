namespace WaffarXPartnerApi.Domain.Entities.SqlEntities.WaffarXEntities;

public partial class ApiRefreshToken
{
    public int Id { get; set; }

    public string Token { get; set; }

    public DateTime Expires { get; set; }

    public DateTime Created { get; set; }

    public string CreatedByIp { get; set; }

    public DateTime? Revoked { get; set; }

    public string RevokedByIp { get; set; }

    public string ReplacedByToken { get; set; }

    public string Clientkey { get; set; } = null!;

    public int ClientId { get; set; }

    public virtual ApiClient Client { get; set; } = null!;
}
