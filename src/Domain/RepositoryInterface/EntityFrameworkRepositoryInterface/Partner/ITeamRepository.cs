namespace WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.Partner;
public interface ITeamRepository
{
    Task<bool> CreateTeam(CreateTeamWithActionModel model);
    Task<bool> UpdateTeam(UpdateTeamWithActionModel model);
    Task<bool> DeleteTeam(Guid id);
    Task<List<TeamModel>> GetAllTeams(int clientApiId);
    Task<TeamDetailsModel> GetTeamDetails(Guid id);
}

public class UpdateTeamWithActionModel : BaseTeamActionModel
{
    public Guid UpdatedBy { get; set; }
    public string UpdatedByUserName { get; set; }
    public Guid TeamId { get; set; }
}

public class CreateTeamWithActionModel : BaseTeamActionModel
{
    public Guid CreatedBy { get; set; }
    public string CreatedByUserName { get; set; }
}
public class BaseTeamActionModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public int ClientApiId { get; set; }
    public List<Guid> PageActionIds { get; set; } = new List<Guid>();
}
public class TeamModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
public class UserModel 
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
}

public class TeamDetailsModel : TeamModel
{
    public List<UserModel> Users { get; set; } = new List<UserModel>();
}
