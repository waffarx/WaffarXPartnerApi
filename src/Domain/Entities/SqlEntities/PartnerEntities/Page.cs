namespace WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;
public class Page
{
    public Guid Id { get; set; }
    public string PageName { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ClientApiId { get; set; }

    // Navigation properties
    public virtual ICollection<PageAction> PageActions { get; set; }
}
