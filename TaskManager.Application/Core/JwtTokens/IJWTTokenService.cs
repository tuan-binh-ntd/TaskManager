namespace TaskManager.Application.Core.JwtTokens;

public interface IJWTTokenService
{
    Task<string> CreateToken(AppUser user);
}
