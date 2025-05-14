using WaffarXPartnerApi.Domain.Models.PartnerMongoModels.TeamModel;

namespace WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.Partner;
public interface ITeamRepository
{
    Task<Guid> CreateTeam(CreateTeamWithActionModel model);
    Task<bool> UpdateTeam(UpdateTeamWithActionModel model);
    Task<bool> DeleteTeam(Guid id);
    Task<List<TeamModel>> GetAllTeams(int clientApiId);
    Task<TeamDetailsModel> GetTeamDetails(Guid id);
}
