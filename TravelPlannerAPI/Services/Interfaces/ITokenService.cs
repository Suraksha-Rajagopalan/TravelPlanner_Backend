using System.Security.Claims;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken(User user);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token, bool isRefreshToken);
    }
}
