using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessAPI.Dtos;

namespace BusinessAPI.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<AdminUserDto>> GetAllUsersAsync();
        Task<bool> DeleteUserAsync(int id);
    }
}
