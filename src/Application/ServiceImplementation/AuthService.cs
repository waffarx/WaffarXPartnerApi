using Microsoft.AspNetCore.Http;
using WaffarXPartnerApi.Application.Common.DTOs;
using WaffarXPartnerApi.Application.Helper;
using WaffarXPartnerApi.Application.ServiceImplementation.Shared;
using WaffarXPartnerApi.Application.ServiceInterface;
using WaffarXPartnerApi.Domain.Entities.SqlEntities.PartnerEntities;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface;
using WaffarXPartnerApi.Domain.RepositoryInterface.EntityFrameworkRepositoryInterface.Partner;

namespace WaffarXPartnerApi.Application.ServiceImplementation;

public class AuthService : JWTUserBaseService, IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(
        IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IJwtService jwtService,
        IPasswordHasher passwordHasher): base(httpContextAccessor)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
    }

    public async Task<TokenResponse> LoginAsync(string Email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(Email);

        if (user == null)
        {
            return new TokenResponse { Success = false, Message = "User not found" };
        }

        if (!user.IsActive)
        {
            return new TokenResponse { Success = false, Message = "User account is inactive" };
        }

        // Verify password using the stored hash key
        if (!_passwordHasher.VerifyPassword(password, user.Password, user.HashKey))
        {
            return new TokenResponse { Success = false, Message = "Invalid password" };
        }

        // Update last login time
        user.LastLogin = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        var pages =  await _userRepository.GetUserPageAndPageAction(user.Id);
        // Generate tokens
        var tokenResult = await GenerateTokensAsync(user);

        return tokenResult;
    }

    public async Task<TokenResponse> RegisterAsync(RegisterRequestDto model)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(model.Username);

        if (existingUser != null)
        {
            return new TokenResponse { Success = false, Message = "Username already exists" };
        }

        existingUser = await _userRepository.GetByEmailAsync(model.Email);

        if (existingUser != null)
        {
            return new TokenResponse { Success = false, Message = "Email already exists" };
        }

        // Hash password
        var (hashedPassword, hashKey) = _passwordHasher.HashPassword(model.Password);
        User user = new User
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Username = model.Username,
            Email = model.Email,
            IsActive = true,
            IsSuperAdmin = false,
            ClientApiId = ClientApiId,
            Password = hashedPassword,
            HashKey = hashKey,
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
        };
        var success = await _userRepository.CreateAsync(user);
        if (!success)
        {
            return new TokenResponse { Success = false, Message = "Failed to create user" };
        }
        await _userRepository.AssignUserToTeam(user.Id, model.TeamsIds);

        // Generate tokens
        var tokenResult = await GenerateTokensAsync(user);

        return tokenResult;
    }

    public async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
    {
        var storedRefreshToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

        if (storedRefreshToken == null)
        {
            return new TokenResponse { Success = false, Message = "Invalid refresh token" };
        }

        if (storedRefreshToken.IsRevoked)
        {
            // Revoke all descendant tokens in case this token has been compromised
            await _refreshTokenRepository.RevokeAllUserTokensAsync(storedRefreshToken.UserId);
            return new TokenResponse { Success = false, Message = "Refresh token has been revoked" };
        }

        if (storedRefreshToken.IsUsed)
        {
            return new TokenResponse { Success = false, Message = "Refresh token has already been used" };
        }

        if (storedRefreshToken.ExpiryDate < DateTime.UtcNow)
        {
            return new TokenResponse { Success = false, Message = "Refresh token has expired" };
        }

        // Mark current token as used
        storedRefreshToken.IsUsed = true;
        await _refreshTokenRepository.UpdateAsync(storedRefreshToken);

        // Get user
        var user = await _userRepository.GetByIdAsync(storedRefreshToken.UserId);

        if (user == null)
        {
            return new TokenResponse { Success = false, Message = "User not found" };
        }

        if (!user.IsActive)
        {
            return new TokenResponse { Success = false, Message = "User account is inactive" };
        }

        // Generate new tokens
        var tokenResult = await GenerateTokensAsync(user);

        return tokenResult;
    }

    public async Task<bool> RevokeTokenAsync(string refreshToken)
    {
        var storedRefreshToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

        if (storedRefreshToken == null)
        {
            return false;
        }

        if (storedRefreshToken.IsRevoked)
        {
            return false;
        }

        storedRefreshToken.IsRevoked = true;
        await _refreshTokenRepository.UpdateAsync(storedRefreshToken);

        return true;
    }

    public async Task<TokenValidationResult> ValidateJwtTokenAsync(string token)
    {
        try
        {
            var principal = await _jwtService.ValidateTokenAsync(token);

            if (principal == null)
            {
                return new TokenValidationResult { Success = false, Message = "Invalid token" };
            }

            var userId = principal.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            {
                return new TokenValidationResult { Success = false, Message = "Invalid user ID in token" };
            }

            var user = await _userRepository.GetByIdAsync(userGuid);

            if (user == null)
            {
                return new TokenValidationResult { Success = false, Message = "User not found" };
            }

            if (!user.IsActive)
            {
                return new TokenValidationResult { Success = false, Message = "User account is inactive" };
            }

            return new TokenValidationResult
            {
                Success = true,
                User = user
            };
        }
        catch (Exception ex)
        {
            return new TokenValidationResult { Success = false, Message = $"Token validation error: {ex.Message}" };
        }
    }

    private async Task<TokenResponse> GenerateTokensAsync(User user)
    {
        var accessToken = await _jwtService.GenerateAccessTokenAsync(user);
        var accessTokenExpiry = DateTime.UtcNow.AddMinutes(15); // Short lived token - 15 minutes

        var refreshToken = await GenerateRefreshTokenAsync(user);

        return new TokenResponse
        {
            Success = true,
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            AccessTokenExpiresAt = accessTokenExpiry,
            RefreshTokenExpiresAt = refreshToken.ExpiryDate,
            //User = new UserDto
            //{
            //    Id = user.Id,
            //    Username = user.Username,
            //    Email = user.Email,
            //    FirstName = user.FirstName,
            //    LastName = user.LastName,
            //    IsSuperAdmin = user.IsSuperAdmin,
            //    ClientApiId = user.ClientApiId
            //}
        };
    }

    private async Task<RefreshToken> GenerateRefreshTokenAsync(User user)
    {
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = GenerateRandomTokenString(),
            ExpiryDate = DateTime.UtcNow.AddDays(7), // 7 days
            CreatedAt = DateTime.UtcNow,
            IsRevoked = false,
            IsUsed = false
        };

        await _refreshTokenRepository.CreateAsync(refreshToken);

        return refreshToken;
    }

    private string GenerateRandomTokenString()
    {
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        var randomBytes = new byte[40];
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}
