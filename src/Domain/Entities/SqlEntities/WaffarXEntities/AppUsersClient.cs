namespace WaffarXPartnerApi.Domain.Entities.SqlEntities.WaffarXEntities;

public partial class AppUsersClient
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public int ClientId { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public DateTime? Cdate { get; set; }

    public string UserToken { get; set; }

    public virtual ApiClient Client { get; set; } = null!;
}
