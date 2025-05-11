using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Team;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.User;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;

namespace WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
public interface IUserService
{
    Task<GenericResponse<bool>> ResetPasswordAsync(ResetPasswordRequestDto model);
    Task<GenericResponse<bool>> Logout();
    Task<GenericResponse<bool>> UpdateTeamAsync(UpdateTeamDto dto);
    Task<GenericResponse<bool>> CreateTeamAsync(CreateTeamDto dto);
    Task<GenericResponse<bool>> DeleteTeamAsync(Guid id);
    Task<GenericResponse<List<TeamDto>>> GetAllTeamsAsync();
    Task<GenericResponse<TeamDetailsDto>> GetTeamDetailsAsync(Guid id);
    Task<GenericResponse<bool>> AddUserToTeamAsync(AssignUserToTeamRequestDto model);
    Task<GenericResponseWithCount<List<UserSearchResultDto>>> SearchForUser(UserSearchRequestDto request);

}
