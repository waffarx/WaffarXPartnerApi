using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;
using WaffarXPartnerApi.Domain.Models.PartnerMongoModels.TeamModel;

namespace WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.Partner;
public interface ITeamRepository
{
    Task<Team> CreateTeam(CreateTeamWithActionModel model);
    Task<(Team, Team)> UpdateTeam(UpdateTeamWithActionModel model);
    Task<Team> DeleteTeam(Guid id);
    Task<List<TeamModel>> GetAllTeams(int clientApiId);
    Task<TeamDetailsModel> GetTeamDetails(Guid id);
}
