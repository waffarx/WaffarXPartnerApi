using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string HashKey { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsSuperAdmin { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastLogin { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int ClientApiId { get; set; }

    // Navigation properties
    public virtual ICollection<TeamPageAction> TeamPageActions { get; set; }
    public virtual ICollection<UserTeam> UserTeams { get; set; }
    public virtual ICollection<AuditLog> AuditLogs { get; set; }
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; }

}
