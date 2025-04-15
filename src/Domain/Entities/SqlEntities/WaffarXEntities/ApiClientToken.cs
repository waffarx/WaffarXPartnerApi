namespace WaffarXPartnerApi.Domain.Entities.SqlEntities.WaffarXEntities;

public partial class ApiClientToken
{
    public int Id { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string Token { get; set; }

    public string ClientKey { get; set; } = null!;

    public int ClientId { get; set; }

    public virtual ApiClient Client { get; set; } = null!;
}
