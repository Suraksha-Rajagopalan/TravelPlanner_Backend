using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Repository.Interface
{
    public interface IAuthRepository : IGenericRepository<UserModel>
    {
        Task<UserModel?> GetByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(UserModel user, string password);
        Task<(bool succeeded, IEnumerable<IdentityError> errors)> CreateUserAsync(UserModel user, string password);
        Task UpdateUserAsync(UserModel user);
    }
}
