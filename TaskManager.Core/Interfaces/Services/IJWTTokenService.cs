namespace TaskManager.Core.Interfaces.Services;

public interface IJWTTokenService
{
    Task<string> CreateToken(AppUser user);
}
