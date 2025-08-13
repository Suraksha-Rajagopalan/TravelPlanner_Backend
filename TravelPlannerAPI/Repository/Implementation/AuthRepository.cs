using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Models.Data;
using TravelPlannerAPI.Repository.Interface;

namespace TravelPlannerAPI.Repository.Implementation
{
    public class AuthRepository : GenericRepository<UserModel> ,IAuthRepository
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly ApplicationDbContext _context;

        public AuthRepository(ApplicationDbContext context, UserManager<UserModel> userManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<UserModel?> GetByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }
        public async Task<UserModel?> GetByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<UserModel?> GetByTokenAsync(string refreshToken)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task<bool> CheckPasswordAsync(UserModel user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<(bool succeeded, IEnumerable<IdentityError> errors)> CreateUserAsync(UserModel user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return (result.Succeeded, result.Errors);
        }

        public async Task UpdateUserAsync(UserModel user)
        {
            // Optionally use either one:
            await _userManager.UpdateAsync(user);
            // or:
            //_userRepo.Update(user);
            //await _userRepo.SaveAsync();
        }
    }
}
