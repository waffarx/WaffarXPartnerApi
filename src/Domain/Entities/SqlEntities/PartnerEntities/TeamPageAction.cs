namespace WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;
public class TeamPageAction
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public Guid PageId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid CreatedBy { get; set; }

    // Navigation properties
    public virtual Team Team { get; set; }
    public virtual Page Page { get; set; }
    public virtual User CreatedByUser { get; set; }
}
