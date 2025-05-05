using Microsoft.EntityFrameworkCore;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface;
using WaffarXPartnerApi.Infrastructure.Data;

namespace WaffarXPartnerApi.Infrastructure.RepositoryImplementation.EntityFrameworkRepository;
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
}
