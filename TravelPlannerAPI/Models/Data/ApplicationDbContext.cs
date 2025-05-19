using Microsoft.EntityFrameworkCore;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Models.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
