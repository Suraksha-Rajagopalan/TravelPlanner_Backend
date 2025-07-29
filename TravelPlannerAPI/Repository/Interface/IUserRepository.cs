using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Repository.Interface
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<List<AdminUserDto>> GetAllAdminUserDtosAsync();
    }
}
