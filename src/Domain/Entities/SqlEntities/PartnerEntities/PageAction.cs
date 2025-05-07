namespace WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;
public class PageAction
{
    public Guid Id { get; set; }
    public Guid PageId { get; set; }
    public string ActionName { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation property
    public virtual Page Page { get; set; }
    public virtual ICollection<TeamPageAction> TeamPageActions { get; set; }
}
