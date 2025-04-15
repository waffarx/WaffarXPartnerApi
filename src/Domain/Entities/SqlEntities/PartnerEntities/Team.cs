namespace WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;
public class Team
{
    public Guid Id { get; set; }
    public string TeamName { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int ClientApiId { get; set; }


    // Navigation properties
    public virtual ICollection<TeamPageAction> TeamPageActions { get; set; }
    public virtual ICollection<UserTeam> UserTeams { get; set; }

}
