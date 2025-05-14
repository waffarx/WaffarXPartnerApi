namespace WaffarXPartnerApi.Domain.Models.PartnerSqlModels;
public class AuditLogModel
{
    public Guid UserId { get; set; }
    public string ActionType { get; set; }
    public string EntityType { get; set; }
    public Guid? EntityId { get; set; }
    public string OldValues { get; set; }
    public string NewValues { get; set; }
    public int ClientApiId { get; set; }
}
