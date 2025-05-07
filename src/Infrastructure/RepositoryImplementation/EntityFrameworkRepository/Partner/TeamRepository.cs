using Microsoft.EntityFrameworkCore;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.Partner;
using WaffarXPartnerApi.Infrastructure.Data;

namespace WaffarXPartnerApi.Infrastructure.RepositoryImplementation.EntityFrameworkRepository.Partner;
public class TeamRepository : ITeamRepository
{
    private readonly WaffarXPartnerDbContext _waffarXPartnerDbContext;

    public TeamRepository(WaffarXPartnerDbContext waffarXPartnerDbContext)
    {
        _waffarXPartnerDbContext = waffarXPartnerDbContext;
    }

    public async Task<bool> CreateTeam(CreateTeamWithActionModel model)
    {
        try
        {
            var teamToCreate = await _waffarXPartnerDbContext.Teams.AddAsync(new Team
            {
                TeamName = model.Name,
                Description = model.Description,
                ClientApiId = model.ClientApiId,
                CreatedAt = new DateTime(),

            });
            if (teamToCreate != null)
            {
                // chech all actions in the input are valid 
                var actions = await _waffarXPartnerDbContext.PageActions.Where(x => model.PageActionIds.Contains(x.Id)).ToListAsync();
                if (actions.Count == model.PageActionIds.Count)
                {
                    List<TeamPageAction> teamPageActionsToAdd = new List<TeamPageAction>();
                    foreach (var action in actions)
                    {
                        teamPageActionsToAdd.Add(new TeamPageAction
                        {
                            PageActionId = action.Id,
                            TeamId = teamToCreate.Entity.Id,
                            CreatedAt = new DateTime(),
                            CreatedBy = model.CreatedBy,
                        });
                    }
                    await _waffarXPartnerDbContext.TeamPageActions.AddRangeAsync(teamPageActionsToAdd);
                    await _waffarXPartnerDbContext.SaveChangesAsync();
                    return true;
                }
                else // not all actions are valid
                {
                    return false;
                }

            }
            else // fail to create team
            {
                return false;
            }

        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> DeleteTeam(Guid id)
    {
        try
        {
            // chech no user is no user is assigned to this team
            var userTeam = await _waffarXPartnerDbContext.UserTeams.FirstOrDefaultAsync(x => x.TeamId == id);
            if (userTeam != null) 
            {
                return false; // user is assigned to this team
            }
            // remove page actions for this team first 
            var pageActions = await _waffarXPartnerDbContext.TeamPageActions.Where(x => x.TeamId == id).ToListAsync();
            if (pageActions != null)
            {
                _waffarXPartnerDbContext.TeamPageActions.RemoveRange(pageActions);
            }
            // remove the team
            var team = await _waffarXPartnerDbContext.Teams.FirstOrDefaultAsync(x => x.Id == id);
            if (team != null) 
            {
                _waffarXPartnerDbContext.Teams.Remove(team);
                await _waffarXPartnerDbContext.SaveChangesAsync();
                return true; // team removed successfully
            }
            else
            {
                return false; // team not found
            }

        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<TeamModel>> GetAllTeams(int clientApiId)
    {
        try
        {
            // get all active team and map it to team model 
            var teams = await _waffarXPartnerDbContext.Teams.Where(x => x.ClientApiId == clientApiId).Select(x => new TeamModel
            {
                Id = x.Id,
                Name = x.TeamName,
            }).ToListAsync();
            return teams;
        }
        catch(Exception)
        {
            throw;
        }
    }

    public async Task<TeamDetailsModel> GetTeamDetails(Guid id)
    {
        try
        {
            // by team id get the team and join to the user under this team and map back to the model
            var teamDetails = await _waffarXPartnerDbContext.Teams
                .Include(x => x.UserTeams)
                .ThenInclude(x => x.User)
                .AsSplitQuery()
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new TeamDetailsModel
                {
                    Id = x.Id,
                    Name = x.TeamName,
                    Users = x.UserTeams.Select(u => new UserModel
                    {
                        UserId = u.UserId,
                        UserName = u.User.Username,
                        Email = u.User.Email,
                    }).ToList(),
                }).FirstOrDefaultAsync();

            return teamDetails;
        }
        catch(Exception)
        {
            throw;
        }
    }

    public async Task<bool> UpdateTeam(UpdateTeamWithActionModel model)
    {
        try
        {
            // get the team by id and api client id 
            var team = await _waffarXPartnerDbContext.Teams.FirstOrDefaultAsync(x => x.Id == model.TeamId && x.ClientApiId == model.ClientApiId);
            if(team != null)
            {
                // update the team 
                team.TeamName = model.Name;
                team.Description = model.Description;
                team.UpdatedAt = new DateTime();
                 _waffarXPartnerDbContext.Teams.Update(team);
                // remove all page actions for this team 
                var pageActions = await _waffarXPartnerDbContext.TeamPageActions.Where(x => x.TeamId == model.TeamId).ToListAsync();
                if (pageActions != null)
                {
                   _waffarXPartnerDbContext.TeamPageActions.RemoveRange(pageActions);
                }
                // add the new page actions
                List<TeamPageAction> teamPageActionsToAdd = new List<TeamPageAction>();
                foreach (var action in model.PageActionIds)
                {
                    teamPageActionsToAdd.Add(new TeamPageAction
                    {
                        PageActionId = action,
                        TeamId = model.TeamId,
                        CreatedAt = new DateTime(),
                        CreatedBy = model.UpdatedBy,
                    });
                }
                await _waffarXPartnerDbContext.TeamPageActions.AddRangeAsync(teamPageActionsToAdd);
                await _waffarXPartnerDbContext.SaveChangesAsync();
                return true; // team updated successfully
            }
            else
            {
                return false;
            }

        }
        catch(Exception)
        {
            throw;
        }
    }
}
