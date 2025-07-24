using BusinessAPI.Dtos;
using System.Threading.Tasks;

namespace BusinessAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> SignupAsync(SignupRequest request);
        Task<AuthResponseDto> LoginAsync(LoginRequest request);
    }
}
