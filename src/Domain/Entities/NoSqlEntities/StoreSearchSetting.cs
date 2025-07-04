﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using WaffarXPartnerApi.Domain.Entities.NoSqlEntities;

namespace WaffarXPartnerApi.Domain.Entities.NoSqlEnitities;
[BsonIgnoreExtraElements]
public class StoreSearchSetting
{
    [BsonId]
    public ObjectId Id { get; set; }

    public string SettingType { get; set; }

    public List<StoreSettings> Stores { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int CreatedBy { get; set; }

    public int UpdatedBy { get; set; }

    public int ClientApiId { get; set; }
}


