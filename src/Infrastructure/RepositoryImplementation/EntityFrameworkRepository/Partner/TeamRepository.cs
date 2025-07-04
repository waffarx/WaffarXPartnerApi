﻿using Microsoft.EntityFrameworkCore;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;
using WaffarXPartnerApi.Domain.Models.PartnerMongoModels.TeamModel;
using WaffarXPartnerApi.Domain.Models.PartnerSqlModels;
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

    public async Task<Team> CreateTeam(CreateTeamWithActionModel model) 
    {
        try
        {
            // check if the team name already exists for this client api id
            var teamNameExists = await _waffarXPartnerDbContext.Teams.AnyAsync(x => x.TeamName == model.Name && x.ClientApiId == model.ClientApiId);
            if (teamNameExists)
            {
                return null; // team name already exists
            }
            var teamToCreate = await _waffarXPartnerDbContext.Teams.AddAsync(new Team
            {
                TeamName = model.Name,
                Description = model.Description,
                ClientApiId = model.ClientApiId,
                CreatedAt = new DateTime(),

            });

            if (teamToCreate != null)
            {
                await _waffarXPartnerDbContext.SaveChangesAsync();
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
                    return teamToCreate.Entity;
                }
                else // not all actions are valid
                {
                    return null;
                }

            }
            else // fail to create team
            {
                return null;
            }

        }
        catch
        {
            throw;
        }
    }
    public async Task<(Team,Team)> UpdateTeam(UpdateTeamWithActionModel model)
    {
        try
        {
            // get the team by id and api client id 
            var team = await _waffarXPartnerDbContext.Teams.FirstOrDefaultAsync(x => x.Id == model.TeamId && x.ClientApiId == model.ClientApiId);
            if(team != null)
            {
                //chech the team name not exists before 
                var teamNameExists = await _waffarXPartnerDbContext.Teams.AnyAsync(x => x.TeamName == model.Name && x.ClientApiId == model.ClientApiId && x.Id != model.TeamId);
                if (teamNameExists)
                {
                    return (null, null); // team name already exists
                }

                var copiedTeam = new Team
                {
                    Id = team.Id,
                    TeamName = team.TeamName,
                    Description = team.Description,
                    CreatedAt = team.CreatedAt,
                    UpdatedAt = team.UpdatedAt,
                    ClientApiId = team.ClientApiId,
                    UserTeams = team.UserTeams,
                    TeamPageActions = team.TeamPageActions,
                };
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
                return (copiedTeam,team); // team updated successfully
            }
            else
            {
                return (null,null);
            }

        }
        catch(Exception)
        {
            throw;
        }
    }
    public async Task<Team> DeleteTeam(Guid id)
    {
        try
        {
            // chech no user is no user is assigned to this team
            var userTeam = await _waffarXPartnerDbContext.UserTeams.FirstOrDefaultAsync(x => x.TeamId == id);
            if (userTeam != null) 
            {
                return null; // user is assigned to this team
            }
            // remove page actions for this team first 
            var pageActions = await _waffarXPartnerDbContext.TeamPageActions.Where(x => x.TeamId == id).ToListAsync();
           
            // remove the team
            var team = await _waffarXPartnerDbContext.Teams.FirstOrDefaultAsync(x => x.Id == id);
            var copiedTeam = new Team
            {
                Id = team.Id,
                TeamName = team.TeamName,
                Description = team.Description,
                CreatedAt = team.CreatedAt,
                UpdatedAt = team.UpdatedAt,
                ClientApiId = team.ClientApiId,
                UserTeams = team.UserTeams,
                TeamPageActions = team.TeamPageActions,
            };
            if (pageActions != null)
            {
                _waffarXPartnerDbContext.TeamPageActions.RemoveRange(pageActions);
            }
            if (team != null) 
            {
                _waffarXPartnerDbContext.Teams.Remove(team);
                await _waffarXPartnerDbContext.SaveChangesAsync();
                return copiedTeam; // team removed successfully
            }
            else
            {
                return null; // team not found
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
                Description = x.Description
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
                                    .Include(x => x.TeamPageActions)
                                        .ThenInclude(x => x.PageAction)
                                            .ThenInclude(x => x.Page)
                                    .AsSplitQuery()
                                    .AsNoTracking()
                                    .Where(x => x.Id == id)
                                    .Select(x => new TeamDetailsModel
                                    {
                                         Id = x.Id,
                                         Name = x.TeamName,
                                         Description = x.Description,
                                         Users = x.UserTeams.Select(u => new UserModel
                                         {
                                             UserId = u.UserId,
                                             UserName = u.User.Username,
                                             Email = u.User.Email,
                                         }).ToList(),
                                         Pages = x.TeamPageActions
                                             .GroupBy(p => p.PageAction.Page)
                                             .Select(g => new PageDetailModel
                                             {
                                                 Page = new PageModel
                                                 {
                                                     Id = g.Key.Id,
                                                     Name = g.Key.PageName,
                                                     PageActions = g.Select(a => new PageActionModel
                                             {
                                                 Id = a.PageAction.Id,
                                                 Name = a.PageAction.ActionName,
                                                 Description = a.PageAction.Description
                                             }).ToList(),
                                         },
                                     }).ToList(),
                             }).FirstOrDefaultAsync();

            return teamDetails;
        }
        catch(Exception)
        {
            throw;
        }
    }

}
