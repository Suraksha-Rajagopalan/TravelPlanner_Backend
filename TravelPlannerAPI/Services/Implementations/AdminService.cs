using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Models.Data;
using TravelPlannerAPI.Services.Interfaces;
using TravelPlannerAPI.UoW;

namespace TravelPlannerAPI.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<UserModel> _userManager;

        public AdminService(IUnitOfWork unitOfWork, UserManager<UserModel> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<List<AdminUserDto>> GetAllUsersAsync()
        {
            return await _unitOfWork.Users.GetAllAdminUserDtosAsync();
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            await _unitOfWork.Expenses.RemoveByUserIdAsync(id);
            await _unitOfWork.Reviews.RemoveByUserIdAsync(id);
            //await _unitOfWork.Itineraries.RemoveByUserIdAsync(id);
            await _unitOfWork.Checklists.RemoveByUserIdAsync(id);
            await _unitOfWork.Trips.RemoveByUserIdAsync(id);
            await _unitOfWork.TripShares.RemoveByUserIdAsync(id);

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            await _unitOfWork.CompleteAsync();

            return result.Succeeded;
        }

    }

}
