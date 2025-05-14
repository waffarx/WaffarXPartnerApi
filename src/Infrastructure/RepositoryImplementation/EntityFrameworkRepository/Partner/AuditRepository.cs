using Microsoft.EntityFrameworkCore;
using WaffarXPartnerApi.Application.Helper;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;
using WaffarXPartnerApi.Domain.Enums;
using WaffarXPartnerApi.Domain.Models.PartnerSqlModels;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.Partner;
using WaffarXPartnerApi.Infrastructure.Data;

namespace WaffarXPartnerApi.Infrastructure.RepositoryImplementation.EntityFrameworkRepository.Partner;
public class AuditRepository : IAuditRepository
{
    private readonly WaffarXPartnerDbContext _context;

    public AuditRepository(WaffarXPartnerDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Adds an audit log entry to the database
    /// </summary>
    /// <param name="auditLog">Audit log entry to add</param>
    public async Task AddAuditLogAsync(AuditLog auditLog)
    {
        await _context.AuditLogs.AddAsync(auditLog);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Creates and adds an audit log entry for an entity change
    /// </summary>
    /// <typeparam name="T">Type of entity being audited</typeparam>
    /// <param name="auditParams">Parameters for creating the audit log</param>
    public async Task LogEntityChangeAsync<T>(AuditLogParams<T> auditParams)
    {
        var auditLog = AuditHelper.CreateAuditLog(auditParams);
        await AddAuditLogAsync(auditLog);
    }

    /// <summary>
    /// Retrieves audit logs for a specific entity
    /// </summary>
    /// <param name="entityId">ID of the entity</param>
    /// <param name="entityType">Type of entity</param>
    /// <returns>List of audit logs for the entity</returns>
    public async Task<List<AuditLog>> GetAuditLogsForEntityAsync(Guid entityId, EntityTypeEnum entityType)
    {
        return await _context.AuditLogs
            .Where(a => a.EntityId == entityId && a.EntityType == entityType.ToString())
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves audit logs by user
    /// </summary>
    /// <param name="userId">ID of the user</param>
    /// <returns>List of audit logs by the user</returns>
    public async Task<List<AuditLog>> GetAuditLogsByUserAsync(Guid userId)
    {
        return await _context.AuditLogs
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }
}
