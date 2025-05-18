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
            var oldUser = new User
            {
                UserTeams = user.UserTeams,
                UserId = user.UserId,
                Username = user.Username,
                TeamPageActions = user.TeamPageActions,
                ClientApiId = user.ClientApiId,
                CreatedAt = user.CreatedAt,
                Email = user.Email,
                FirstName = user.FirstName,
                HashKey = user.HashKey,
                Id = user.Id,
                IsActive = user.IsActive,
                IsSuperAdmin = user.IsSuperAdmin,
                LastLogin = user.LastLogin,
                LastName = user.LastName,
                Password = user.Password,
                UpdatedAt = user.UpdatedAt,
            };
          

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
                EntityId = user.Id,
                UserId =UserId,
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
            var resultTuble = await _userRepository.AssignUserToTeam(userGuid, model.TeamIds, ClientApiId);
            if(resultTuble.Item1 == null || resultTuble.Item2 == null)
            {
                return new GenericResponse<bool>
                {
                    Status = StaticValues.Error,
                    Message = "Failed to add user to team",
                    Data = false
                };
            }
            await _auditService.LogUpdateAsync(new AuditUpdateParams<User>
            {
                OldEntity = resultTuble.Item1,
                NewEntity = resultTuble.Item2,
                EntityType = EntityTypeEnum.User,
                ClientApiId = ClientApiId,
                EntityId = resultTuble.Item2.Id,
                UserId = UserId,
                
                
            });
            return new GenericResponse<bool>
            {
                Status =  StaticValues.Success ,
                Message =  "User added to team successfully" ,
                Data = true
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
            if (result != null)
            {
                await _auditService.LogCreationAsync(new AuditCreationParams<Team>
                {
                    EntityType = EntityTypeEnum.Team,
                    ClientApiId = ClientApiId,
                    UserId = UserId,
                    EntityId = result.Id,
                    Entity = result,
                    
                });
            }

            return new GenericResponse<bool>
            {
                Status =  result != null ?  StaticValues.Success : StaticValues.Error,
                Message = result != null ? "Team created successfully" : "Failed to create team",
                Data = result != null ? true : false
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
            var resultTuple = await _teamRepository.UpdateTeam(model);
            if(resultTuple.Item1 == null || resultTuple.Item2 == null)
            {
                return new GenericResponse<bool>
                {
                    Status = StaticValues.Error,
                    Message = "Failed to update team",
                    Data = false
                };
            }
            await _auditService.LogUpdateAsync(new AuditUpdateParams<Team>
            {
                OldEntity = resultTuple.Item1,
                NewEntity = resultTuple.Item2,
                EntityType = EntityTypeEnum.Team,
                ClientApiId = ClientApiId,
                EntityId = resultTuple.Item2.Id,
                UserId = UserId,
                
            });
            return new GenericResponse<bool>
            {
                Status = StaticValues.Success ,
                Message =  "Team updated successfully",
                Data = true
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
            if(result == null)
            {
                return new GenericResponse<bool>
                {
                    Status = StaticValues.Error,
                    Message = "Failed to delete team",
                    Data = false
                };
            }
            await _auditService.LogDeletionAsync(new AuditDeletionParams<Team>
            {
                EntityType = EntityTypeEnum.Team,
                ClientApiId = ClientApiId,
                EntityId = id,
                UserId = UserId,
                Entity = result,
            });
            return new GenericResponse<bool>
            {
                Status = StaticValues.Success ,
                Message = "Team deleted successfully" ,
                Data = true
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
