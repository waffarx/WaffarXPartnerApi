namespace WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;
public class PartnerPostback
{
    public Guid Id { get; set; }
    public int ClientApiId { get; set; }
    public string PostbackUrl { get; set; }
    public int PostbackType { get; set; }
    public int PostbackMethod { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
