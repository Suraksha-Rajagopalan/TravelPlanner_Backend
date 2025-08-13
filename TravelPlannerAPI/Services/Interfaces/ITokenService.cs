using System.Security.Claims;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Services.Interfaces
{
    public interface ITokenService
    {
        Task<(string token, DateTime? expiry)> GenerateAccessToken(UserModel user);
        Task<(string token, DateTime expiry)> GenerateRefreshToken(UserModel user);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token, bool isRefreshToken);
    }
}