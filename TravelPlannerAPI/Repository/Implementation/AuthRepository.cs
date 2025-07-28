using TravelPlannerAPI.Models;
using TravelPlannerAPI.Repositories.Interfaces;
using TravelPlannerAPI.Generic;
using Microsoft.AspNetCore.Identity;

namespace TravelPlannerAPI.Repositories.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly IGenericRepository<User> _userRepo;

        public AuthRepository(UserManager<User> userManager, IGenericRepository<User> userRepo)
        {
            _userManager = userManager;
            _userRepo = userRepo;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<(bool succeeded, IEnumerable<IdentityError> errors)> CreateUserAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return (result.Succeeded, result.Errors);
        }

        public async Task UpdateUserAsync(User user)
        {
            // Optionally use either one:
            await _userManager.UpdateAsync(user);
            // or:
            //_userRepo.Update(user);
            //await _userRepo.SaveAsync();
        }
    }
}
