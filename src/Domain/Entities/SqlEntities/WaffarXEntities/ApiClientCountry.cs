namespace WaffarXPartnerApi.Domain.Entities.SqlEntities.WaffarXEntities;

public partial class ApiClientCountry
{
    public int Id { get; set; }

    public string ClientGuid { get; set; } = null!;

    public int CountryId { get; set; }

    public int ClientId { get; set; }

    public virtual ApiClient Client { get; set; } = null!;
}
