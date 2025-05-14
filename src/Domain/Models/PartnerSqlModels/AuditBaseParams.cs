namespace WaffarXPartnerApi.Domain.Models.PartnerSqlModels;

public abstract class AuditBaseParams
{
    public EntityTypeEnum EntityType { get; set; }
    public Guid UserId { get; set; }
    public int ClientApiId { get; set; }
    public Guid? EntityId { get; set; }
}

public class AuditCreationParams<T> : AuditBaseParams
{
    public T Entity { get; set; }
}

public class AuditUpdateParams<T> : AuditBaseParams
{
    public T OldEntity { get; set; }

    public T NewEntity { get; set; }
}

public class AuditDeletionParams<T> : AuditBaseParams
{
    public T Entity { get; set; }
}

public class AuditLogParams<T> : AuditBaseParams
{
    public T OldValue { get; set; }

    public T NewValue { get; set; }
    public ActionTypeEnum ActionType { get; set; }
}
