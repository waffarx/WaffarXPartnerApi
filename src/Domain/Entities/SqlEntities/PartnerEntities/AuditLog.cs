namespace WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;
public class AuditLog
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string ActionType { get; set; }
    public string EntityType { get; set; }
    public string OldValues { get; set; }
    public string NewValues { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ClientApiId { get; set; }

    // The SQL has a reference to EntityId in an index but it's not in the table definition
    // Adding it here for completeness
    public Guid? EntityId { get; set; }

    // Navigation property
    public virtual User User { get; set; }
}
