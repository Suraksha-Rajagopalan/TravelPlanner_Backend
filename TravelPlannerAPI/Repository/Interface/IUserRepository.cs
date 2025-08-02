using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Repository.Interface
{
    public interface IUserRepository : IGenericRepository<UserModel>
    {
        Task<List<AdminUserDto>> GetAllAdminUserDtosAsync();
    }
}
