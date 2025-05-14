using System;
using System.Collections.Generic;
using WaffarXPartnerApi.Domain.Models.PartnerSqlModels;

namespace WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
public interface IAuditService
{
    /// <summary>
    /// Logs the creation of a new entity
    /// </summary>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="params">Parameters for logging the creation</param>
    Task LogCreationAsync<T>(AuditCreationParams<T> @params);

    /// <summary>
    /// Logs the update of an existing entity
    /// </summary>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="params">Parameters for logging the update</param>
    Task LogUpdateAsync<T>(AuditUpdateParams<T> @params);

    /// <summary>
    /// Logs the deletion of an entity
    /// </summary>
    /// <typeparam name="T">Type of entity</typeparam>
    /// <param name="params">Parameters for logging the deletion</param>
    Task LogDeletionAsync<T>(AuditDeletionParams<T> @params);
}
