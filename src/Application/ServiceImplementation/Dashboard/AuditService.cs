using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
using WaffarXPartnerApi.Domain.Enums;
using WaffarXPartnerApi.Domain.Models.PartnerSqlModels;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.Partner;

namespace WaffarXPartnerApi.Application.ServiceImplementation.Dashboard;
public class AuditService : IAuditService
{
    private readonly IAuditRepository _auditRepository;

    public AuditService(IAuditRepository auditRepository)
    {
        _auditRepository = auditRepository;
    }

    /// <summary>
    /// Logs the creation of a new entity
    /// </summary>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="params">Parameters for logging the creation</param>
    public async Task LogCreationAsync<T>(AuditCreationParams<T> @params)
    {
        var auditParams = new AuditLogParams<T>
        {
            OldValue = default,
            NewValue = @params.Entity,
            ActionType = ActionTypeEnum.Added,
            EntityType = @params.EntityType,
            UserId = @params.UserId,
            ClientApiId = @params.ClientApiId,
            EntityId = @params.EntityId
        };

        await _auditRepository.LogEntityChangeAsync(auditParams);
    }

    /// <summary>
    /// Logs the update of an existing entity
    /// </summary>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="params">Parameters for logging the update</param>
    public async Task LogUpdateAsync<T>(AuditUpdateParams<T> @params)
    {
        var auditParams = new AuditLogParams<T>
        {
            OldValue = @params.OldEntity,
            NewValue = @params.NewEntity,
            ActionType = ActionTypeEnum.Updated,
            EntityType = @params.EntityType,
            UserId = @params.UserId,
            ClientApiId = @params.ClientApiId,
            EntityId = @params.EntityId
        };

        await _auditRepository.LogEntityChangeAsync(auditParams);
    }

    /// <summary>
    /// Logs the deletion of an entity
    /// </summary>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="params">Parameters for logging the deletion</param>
    public async Task LogDeletionAsync<T>(AuditDeletionParams<T> @params)
    {
        var auditParams = new AuditLogParams<T>
        {
            OldValue = @params.Entity,
            NewValue = default,
            ActionType = ActionTypeEnum.Deleted,
            EntityType = @params.EntityType,
            UserId = @params.UserId,
            ClientApiId = @params.ClientApiId,
            EntityId = @params.EntityId
        };

        await _auditRepository.LogEntityChangeAsync(auditParams);
    }
}
