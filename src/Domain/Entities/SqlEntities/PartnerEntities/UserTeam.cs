namespace WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;
public class UserTeam
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual User User { get; set; }
    public virtual Team Team { get; set; }
}
