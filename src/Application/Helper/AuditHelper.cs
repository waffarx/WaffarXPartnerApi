using Newtonsoft.Json;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;
using WaffarXPartnerApi.Domain.Models.PartnerSqlModels;

namespace WaffarXPartnerApi.Application.Helper;
public class AuditHelper
{
    /// <summary>
    /// Creates an audit log entry comparing old and new values of an entity
    /// </summary>
    /// <typeparam name="T">Type of entity being audited</typeparam>
    /// <param name="params">Parameters for creating the audit log</param>
    /// <returns>An AuditLog entry ready to be saved</returns>
    public static AuditLog CreateAuditLog<T>(AuditLogParams<T> @params)
    {
        string oldValueJson = @params.OldValue != null ? SerializeObject(@params.OldValue) : null;
        string newValueJson = @params.NewValue != null ? SerializeObject(@params.NewValue) : null;

        return new AuditLog
        {
            Id = Guid.NewGuid(),
            UserId = @params.UserId,
            ActionType = @params.ActionType.ToString(),
            EntityType = @params.EntityType.ToString(),
            OldValues = oldValueJson,
            NewValues = newValueJson,
            CreatedAt = DateTime.UtcNow,
            ClientApiId = @params.ClientApiId,
            EntityId = @params.EntityId
        };
    }

    /// <summary>
    /// Serializes an object to JSON, ignoring null values and circular references
    /// </summary>
    /// <typeparam name="T">Type of object to serialize</typeparam>
    /// <param name="obj">Object to serialize</param>
    /// <returns>JSON string representation of the object</returns>
    public static string SerializeObject<T>(T obj)
    {
        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None
        };

        return JsonConvert.SerializeObject(obj, settings);
    }

    /// <summary>
    /// Deserializes a JSON string back to an object
    /// </summary>
    /// <typeparam name="T">Type to deserialize to</typeparam>
    /// <param name="json">JSON string</param>
    /// <returns>Deserialized object</returns>
    public static T DeserializeObject<T>(string json)
    {
        if (string.IsNullOrEmpty(json))
            return default;

        return JsonConvert.DeserializeObject<T>(json);
    }
}
