namespace WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;
public class ClientPage
{
    public Guid Id { get; set; }
    public Guid PageId { get; set; }
    public bool IsActive { get; set; }
    public int ClientApiId { get; set; }
    public virtual Page Page { get; set; }
}
