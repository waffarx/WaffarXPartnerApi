﻿using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;
using WaffarXPartnerApi.Domain.Models.PartnerSqlModels;

namespace WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.Partner;
public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id);
    Task<User> GetByUsernameAsync(string username);
    Task<User> GetByEmailAsync(string email);
    Task<bool> CreateAsync(User user);
    Task<bool> UpdateAsync(User user);
    Task<(User, User)> AssignUserToTeam(Guid userId, List<string> teamIds, int clientApiId);
    Task<List<UserPageActionsModel>> GetUserPageAndPageAction(Guid userId);
    Task<PaginationResultModel<List<UserSearchModel>>> SearchUserByEmail(UserSearchRequestModel model);
    Task<UserDetailModel> GetUserDetails(Guid userId);

}

