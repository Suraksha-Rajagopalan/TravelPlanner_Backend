using System.Collections.Generic;
using System.Threading.Tasks;
using TravelPlannerAPI.Dtos;

namespace TravelPlannerAPI.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<AdminUserDto>> GetAllUsersAsync();
        Task<bool> DeleteUserAsync(int id);
    }
}
