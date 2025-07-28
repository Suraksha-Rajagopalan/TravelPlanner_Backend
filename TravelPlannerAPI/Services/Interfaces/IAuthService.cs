using TravelPlannerAPI.Dtos;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> SignupAsync(SignupRequest request);
        Task<AuthResponseDto> LoginAsync(LoginRequest request);
    }
}
