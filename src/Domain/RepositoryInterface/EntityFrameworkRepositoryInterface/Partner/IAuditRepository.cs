using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;
using WaffarXPartnerApi.Domain.Models.PartnerSqlModels;

namespace WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.Partner;
public interface IAuditRepository
{
    /// <summary>
    /// Adds an audit log entry to the database
    /// </summary>
    /// <param name="auditLog">Audit log entry to add</param>
    Task AddAuditLogAsync(AuditLog auditLog);

    /// <summary>
    /// Creates and adds an audit log entry for an entity change
    /// </summary>
    /// <typeparam name="T">Type of entity being audited</typeparam>
    /// <param name="auditParams">Parameters for creating the audit log</param>
    Task LogEntityChangeAsync<T>(AuditLogParams<T> auditParams);

    /// <summary>
    /// Retrieves audit logs for a specific entity
    /// </summary>
    /// <param name="entityId">ID of the entity</param>
    /// <param name="entityType">Type of entity</param>
    /// <returns>List of audit logs for the entity</returns>
    Task<List<AuditLog>> GetAuditLogsForEntityAsync(Guid entityId, EntityTypeEnum entityType);

    /// <summary>
    /// Retrieves audit logs by user
    /// </summary>
    /// <param name="userId">ID of the user</param>
    /// <returns>List of audit logs by the user</returns>
    Task<List<AuditLog>> GetAuditLogsByUserAsync(Guid userId);
}
