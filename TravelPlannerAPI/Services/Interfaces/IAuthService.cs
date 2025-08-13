using System.Threading.Tasks;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> SignupAsync(SignupRequest request);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
        Task<AuthResponseDto> UpdateProfileAsync(string userId, UpdateProfileDto dto);
    }
}