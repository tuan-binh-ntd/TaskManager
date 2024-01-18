namespace TaskManager.Infrastructure.Services;

public class JWTTokenService : IJWTTokenService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SymmetricSecurityKey _key;
    public JWTTokenService(
        IOptionsMonitor<JwtSettings> jwtSettingsOptions,
        UserManager<AppUser> userManager
        )
    {
        var jwtSettings = jwtSettingsOptions.CurrentValue;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
        _userManager = userManager;
    }

    public async Task<string> CreateToken(AppUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId , user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName , user.UserName!),
        };

        var roles = await _userManager.GetRolesAsync(user);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds,
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
