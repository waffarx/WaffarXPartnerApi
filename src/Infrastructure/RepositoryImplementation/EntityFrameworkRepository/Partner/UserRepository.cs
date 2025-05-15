using Microsoft.EntityFrameworkCore;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;
using WaffarXPartnerApi.Domain.Models.PartnerMongoModels.TeamModel;
using WaffarXPartnerApi.Domain.Models.PartnerSqlModels;
using WaffarXPartnerApi.Domain.Models.SharedModels;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.Partner;
using WaffarXPartnerApi.Infrastructure.Data;

namespace WaffarXPartnerApi.Infrastructure.RepositoryImplementation.EntityFrameworkRepository.Partner;
public class UserRepository : IUserRepository
{
    private readonly WaffarXPartnerDbContext _context;

    public UserRepository(WaffarXPartnerDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        try
        {
            var userToGet = await _context.Users
                    .Include(u => u.UserTeams)
                    .Include(u => u.RefreshTokens)
                    .FirstOrDefaultAsync(u => u.Id == id);
            return userToGet;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(u => u.UserTeams)
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.UserTeams)
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> CreateAsync(User user)
    {
        _context.Users.Add(user);
        var created = await _context.SaveChangesAsync();
        return created > 0;
    }

    public async Task<bool> UpdateAsync(User user)
    {
        var updated = 0;
        if (user != null)
        {
            user.UpdatedAt = DateTime.UtcNow;
            // Copy all properties EXCEPT UserId
            _context.Entry(user).CurrentValues.SetValues(user);

            // Explicitly prevent UserId from being updated
            _context.Entry(user).Property(x => x.UserId).IsModified = false;

            updated = await _context.SaveChangesAsync();
        }
        return updated > 0;
    }

    public async Task<bool> AssignUserToTeam(Guid userId, List<string> teamIds, int clientApiId)
    {
        try
        {
            var userBelongToClientApiId = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId && x.ClientApiId == clientApiId);
            if (userBelongToClientApiId == null)
            {
                return false;
            }
            var teamGuids = teamIds
                .Select(t => Guid.TryParse(t, out var parsedGuid) ? parsedGuid : Guid.Empty)
                .Where(g => g != Guid.Empty)
                .ToList();
            // check all those guids are valid in teams table
            var teams = await _context.Teams
                .Where(t => teamGuids.Contains(t.Id) && t.ClientApiId == clientApiId )
                .ToListAsync();
            if (teams.Count != teamGuids.Count)
            {
                // not all guids are valid
                return false;
            }
            // get teams user in 
            var existingUserTeams = await _context.UserTeams
                .Where(ut => ut.UserId == userId)
                .ToListAsync();
            if (existingUserTeams.Count > 0)
            {
                 _context.RemoveRange(existingUserTeams);
            }

            List<UserTeam> userTeamsToAdd = new List<UserTeam>();
            foreach (var teamId in teamGuids)
            {
                userTeamsToAdd.Add(new UserTeam
                {
                    CreatedAt = DateTime.Now,
                    TeamId = teamId,
                    UserId = userId,
                    Id = Guid.NewGuid(),
                });
            }
            await _context.UserTeams.AddRangeAsync(userTeamsToAdd);
            var added = await _context.SaveChangesAsync();
            return added > 0;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<UserPageActionsModel>> GetUserPageAndPageAction(Guid userId)
    {
        try
        {
            var user = await _context.Users
                .Include(u => u.UserTeams)
                    .ThenInclude(ut => ut.Team)
                        .ThenInclude(t => t.TeamPageActions)
                            .ThenInclude(tpa => tpa.PageAction)
                                .ThenInclude(p => p.Page)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return new List<UserPageActionsModel>();
            }


            var pageActions = user.UserTeams
                             .SelectMany(ut => ut.Team.TeamPageActions)
                             .Where(tpa => tpa.PageAction != null && tpa.PageAction.Page != null)
                             .GroupBy(tpa => new
                             {
                                 tpa.PageAction.Page.Id,
                                 tpa.PageAction.Page.PageName,
                                 tpa.PageAction.Page.Description
                             })
                             .Select(g => new UserPageActionsModel
                             {
                                 PageId = g.Key.Id,
                                 PageName = g.Key.PageName,
                                 PageDescription = g.Key.Description,
                                 Actions = g.Select(tpa => new PageActionModel
                                 {
                                     Id = tpa.PageAction.Id,
                                     Name = tpa.PageAction.ActionName,
                                     Description = tpa.PageAction.Description
                                 }).Distinct().ToList()
                             }).ToList();


            return pageActions;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<PaginationResultModel<List<UserSearchModel>>> SearchUserByEmail(UserSearchRequestModel model)
    {
        try
        {
            var isEmptyEmail = string.IsNullOrEmpty(model.Email);
            var users = _context.Users
                .AsSplitQuery()
                .AsNoTracking()
                .Include(x => x.UserTeams)
                    .ThenInclude(x => x.Team)
                .Where(x => x.ClientApiId == model.ClientApiId && x.IsSuperAdmin == false && (!isEmptyEmail ? EF.Functions.Like(x.Email, $"%{model.Email}%") : true))
                .OrderByDescending(u => u.Id)
                .Select(u => new UserSearchModel
                {
                    Email = u.Email,
                    Teams = u.UserTeams.Select(ut => ut.Team.TeamName).ToList(),
                    UserId = u.Id.ToString(),
                    UserName = u.Username,
                    IsActive = u.IsActive
                });
            var totalCount = await users.CountAsync();

            var paginatedUsers = await users
                .Skip((model.PageNumber - 1) * model.PageSize)
                .Take(model.PageSize)
                .ToListAsync();
            return new PaginationResultModel<List<UserSearchModel>>
            {
                Data = paginatedUsers,
                TotalRecords = totalCount,
            };

        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<UserDetailModel> GetUserDetails(Guid userId)
    {
        try
        {
            // Fetch the user along with their teams, team page actions, and related pages
           var userDetails = await _context.Users
                .Include(u => u.UserTeams)
                    .ThenInclude(ut => ut.Team)
                .AsSplitQuery()
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => new UserDetailModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    UserName = u.Username,
                    Teams = u.UserTeams != null
                        ? u.UserTeams.Select(ut => new TeamModel
                        {
                            Id = ut.Team.Id,
                            Name = ut.Team.TeamName,
                        }).ToList() : new List<TeamModel>()
                }).FirstOrDefaultAsync();

            return userDetails;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
