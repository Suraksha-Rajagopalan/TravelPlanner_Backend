using System.Security.Claims;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(UserModel user);
        string GenerateRefreshToken(UserModel user);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token, bool isRefreshToken);
    }
}
