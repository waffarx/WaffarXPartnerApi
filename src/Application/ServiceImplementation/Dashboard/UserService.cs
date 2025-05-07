using Microsoft.AspNetCore.Http;
using WaffarXPartnerApi.Application.Common.Models.SharedModels;
using WaffarXPartnerApi.Application.Helper;
using WaffarXPartnerApi.Application.ServiceImplementation.Shared;
using WaffarXPartnerApi.Application.ServiceInterface.Dashboard;
using WaffarXPartnerApi.Domain.Models.SharedModels;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.Partner;

namespace WaffarXPartnerApi.Application.ServiceImplementation.Dashboard;
public class UserService : JWTUserBaseService, IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IPasswordHasher passwordHasher, IRefreshTokenRepository refreshTokenRepository) : base(httpContextAccessor)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _refreshTokenRepository = refreshTokenRepository;
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
        catch(Exception)
        {
            throw;
        }
    }

    public async Task<GenericResponse<bool>> ResetPasswordAsync(ResetPasswordRequestDto model)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(UserId);
            if(user == null)
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

            // Hash password
            var (hashedPassword, hashKey) = _passwordHasher.HashPassword(model.NewPassword);
            user.Password = hashedPassword;
            user.HashKey = hashKey;
            
            await _userRepository.UpdateAsync(user);
            return new GenericResponse<bool>
            {
                Status = StaticValues.Success,
                Message = "Password reset successfully",
            };

        }
        catch (Exception )
        {
            throw;
        }
    }
}
