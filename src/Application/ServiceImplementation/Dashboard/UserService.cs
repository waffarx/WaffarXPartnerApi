using Microsoft.AspNetCore.Http;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Team;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Team.CreateTeam;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.Team.UpdateTeam;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.User;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.User.AssignUserToTeam;
using WaffarXPartnerApi.Application.Common.DTOs.Dashboard.User.ResetPassword;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
using WaffarXPartnerApi.Application.Helper;
using WaffarXPartnerApi.Application.ServiceImplementation.Shared;
using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;
using WaffarXPartnerApi.Domain.Enums;
using WaffarXPartnerApi.Domain.Models.PartnerMongoModels.TeamModel;
using WaffarXPartnerApi.Domain.Models.PartnerSqlModels;
using WaffarXPartnerApi.Domain.Models.SharedModels;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.Partner;

namespace WaffarXPartnerApi.Application.ServiceImplementation.Dashboard;
public class UserService : JWTUserBaseService, IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITeamRepository _teamRepository;
    private readonly IMapper _mapper;

    private readonly IAuditService _auditService;

    public UserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IPasswordHasher passwordHasher, IRefreshTokenRepository refreshTokenRepository, ITeamRepository teamRepository, IMapper mapper, IAuditService auditService) : base(httpContextAccessor)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _refreshTokenRepository = refreshTokenRepository;
        _teamRepository = teamRepository;
        _mapper = mapper;
        _auditService = auditService;
    }


    public async Task<GenericResponse<bool>> Logout()
    {
        try
        {
            // Invalidate the refresh token
            var refreshToken = await _refreshTokenRepository.GetByUserIdAsync(UserId);
            if (refreshToken != null)
            {
                await _refreshTokenRepository.RevokeAllUserTokensAsync(UserId);
            }
            return new GenericResponse<bool>
            {
                Status = StaticValues.Success,
                Message = "Logout successful",
                Data = true
            };

        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<GenericResponse<bool>> ResetPasswordAsync(ResetPasswordRequestDto model)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(UserId);
            if (user == null)
            {
                return new GenericResponse<bool>
                {
                    Status = StaticValues.Error,
                    Message = "User not found",
                };
            }
            // Verify password using the stored hash key
            if (!_passwordHasher.VerifyPassword(model.OldPassword, user.Password, user.HashKey))
            {
                return new GenericResponse<bool>
                {
                    Status = StaticValues.Error,
                    Message = "Invalid password"
                };
            }
            var oldUser = user;
          

            // Hash password
            var (hashedPassword, hashKey) = _passwordHasher.HashPassword(model.NewPassword);
            user.Password = hashedPassword;
            user.HashKey = hashKey;

            await _userRepository.UpdateAsync(user);

            await _auditService.LogUpdateAsync(new AuditUpdateParams<User>
            {
                OldEntity = oldUser,
                NewEntity = user,
                EntityType = EntityTypeEnum.User,
                ClientApiId = ClientApiId,
                EntityId = user.Id
            });

            return new GenericResponse<bool>
            {
                Status = StaticValues.Success,
                Message = "Password reset successfully",
            };

        }
        catch (Exception)
        {
            throw;
        }
    }
    #region User Function
    public async Task<GenericResponseWithCount<List<UserSearchResultDto>>> SearchForUser(UserSearchRequestDto request)
    {
        try
        {
            UserSearchRequestModel userSearchRequestModel = new UserSearchRequestModel
            {
                ClientApiId = ClientApiId,
                PageSize = request.PageSize,
                PageNumber = request.PageNumber,
                Email = request.Email
            };
            var result = await _userRepository.SearchUserByEmail(userSearchRequestModel);
            if (result.Data != null && result.Data.Count == 0)
            {
                return new GenericResponseWithCount<List<UserSearchResultDto>>
                {
                    Status = StaticValues.Error,
                    Message = "No users found",
                    Data = new List<UserSearchResultDto>(),
                    TotalCount = 0
                };
            }
            return new GenericResponseWithCount<List<UserSearchResultDto>>
            {
                Status = StaticValues.Success,
                Message = "User search completed successfully",
                Data = _mapper.Map<List<UserSearchResultDto>>(result.Data),
                TotalCount = result.TotalRecords,
            };

        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<GenericResponse<UserDetailDto>> GetUserDetails(string userId)
    {
        try
        {
            var userGuid = Guid.TryParse(userId, out var userIdGuid) ? userIdGuid : Guid.Empty;
            var result = await _userRepository.GetUserDetails(userGuid);
            if (result == null)
            {
                return new GenericResponse<UserDetailDto>
                {
                    Status = StaticValues.Error,
                    Message = "User not found",
                    Data = null
                };
            }
            return new GenericResponse<UserDetailDto>
            {
                Status = StaticValues.Success,
                Message = "User details retrieved successfully",
                Data =  _mapper.Map<UserDetailDto>(result) 
            };
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<GenericResponse<bool>> AddUserToTeamAsync(AssignUserToTeamRequestDto model)
    {
        try
        {
            var userGuid = Guid.TryParse(model.UserId.ToString(), out var userId) ? userId : Guid.Empty;
            var result = await _userRepository.AssignUserToTeam(userGuid, model.TeamIds, ClientApiId);
            return new GenericResponse<bool>
            {
                Status = result ? StaticValues.Success : StaticValues.Error,
                Message = result ? "User added to team successfully" : "Failed to add user to team",
                Data = result
            };
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region Team Function
    public async Task<GenericResponse<bool>> CreateTeamAsync(CreateTeamDto dto)
    {
        try
        {
            var model = _mapper.Map<CreateTeamWithActionModel>(dto);
            model.CreatedBy = UserId;
            model.ClientApiId = ClientApiId;
            var result = await _teamRepository.CreateTeam(model);
            if (result != Guid.Empty)
            {
                await _auditService.LogCreationAsync(new AuditCreationParams<Team>
                {
                    EntityType = EntityTypeEnum.Team,
                    ClientApiId = ClientApiId,
                    UserId = UserId,
                    EntityId = result,
                    Entity = new Team 
                    {
                        ClientApiId = ClientApiId,
                        Description = dto.Description,
                        TeamName = dto.Name,
                        
                    },
                });
            }

            return new GenericResponse<bool>
            {
                Status =  result != Guid.Empty ?  StaticValues.Success : StaticValues.Error,
                Message = result != Guid.Empty ? "Team created successfully" : "Failed to create team",
                Data = result != Guid.Empty ? true : false
            };
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<GenericResponse<bool>> UpdateTeamAsync(UpdateTeamDto dto)
    {
        try
        {
            var model = _mapper.Map<UpdateTeamWithActionModel>(dto);
            model.UpdatedBy = UserId;
            model.ClientApiId = ClientApiId;
            var result = await _teamRepository.UpdateTeam(model);
            return new GenericResponse<bool>
            {
                Status = result ? StaticValues.Success : StaticValues.Error,
                Message = result ? "Team updated successfully" : "Failed to update team",
                Data = result
            };
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<GenericResponse<bool>> DeleteTeamAsync(Guid id)
    {
        try
        {
            var result = await _teamRepository.DeleteTeam(id);
            return new GenericResponse<bool>
            {
                Status = result ? StaticValues.Success : StaticValues.Error,
                Message = result ? "Team deleted successfully" : "Failed to delete team",
                Data = result
            };
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<GenericResponse<List<TeamDto>>> GetAllTeamsAsync()
    {
        try
        {
            var teams = await _teamRepository.GetAllTeams(ClientApiId);
            var teamDtos = _mapper.Map<List<TeamDto>>(teams);
            return new GenericResponse<List<TeamDto>>
            {
                Status = StaticValues.Success,
                Message = "Teams retrieved successfully",
                Data = teamDtos
            };
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<GenericResponse<TeamDetailsDto>> GetTeamDetailsAsync(Guid id)
    {
        try
        {
            var teamDetails = await _teamRepository.GetTeamDetails(id);
            var teamDetailsDto = _mapper.Map<TeamDetailsDto>(teamDetails);
            return new GenericResponse<TeamDetailsDto>
            {
                Status = StaticValues.Success,
                Message = "Team details retrieved successfully",
                Data = teamDetailsDto
            };
        }
        catch (Exception)
        {
            throw;
        }
    }


    #endregion
}
