﻿namespace WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;
public class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsRevoked { get; set; }
    public bool IsUsed { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }

    // Navigation properties
    public virtual User User { get; set; }
}
