using System.Security.Claims;

namespace TaskManager.API.Extensions
{
    public static class ClaimsPricipleExtensions
    {
        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value!;
        }
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        }

    }
}
