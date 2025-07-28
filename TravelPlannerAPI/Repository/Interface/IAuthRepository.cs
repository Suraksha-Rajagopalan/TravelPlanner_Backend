using TravelPlannerAPI.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<(bool succeeded, IEnumerable<IdentityError> errors)> CreateUserAsync(User user, string password);
        Task UpdateUserAsync(User user);
    }
}
