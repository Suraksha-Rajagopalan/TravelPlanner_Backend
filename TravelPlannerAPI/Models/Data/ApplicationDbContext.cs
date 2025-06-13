using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TravelPlannerAPI.Models.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<BudgetDetails> BudgetDetails { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<ItineraryItem> ItineraryItems { get; set; }

        public DbSet<ChecklistItem> ChecklistItems { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.User)
                .WithMany(u => u.Trips)
                .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.BudgetDetails)
                .WithOne(b => b.Trip)
                .HasForeignKey<BudgetDetails>(b => b.TripId)
                .OnDelete(DeleteBehavior.Cascade);


            // Rename Identity tables to lowercase or your preferred names (optional)
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<IdentityRole<int>>().ToTable("role");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("user_roles");
            modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("user_claims");
            modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("user_logins");
            modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("role_claims");
            modelBuilder.Entity<IdentityUserToken<int>>().ToTable("user_tokens");

            modelBuilder.Entity<Trip>().ToTable("trips");
            modelBuilder.Entity<BudgetDetails>().ToTable("budgetdetails");

            // For Reviews
            modelBuilder.Entity<Review>(entity =>
            {
                // One Trip has many Reviews
                entity.HasOne(r => r.Trip)
                      .WithMany(t => t.Reviews)
                      .HasForeignKey(r => r.TripId)
                      .OnDelete(DeleteBehavior.Cascade);

                // One User has many Reviews
                entity.HasOne(r => r.User)
                      .WithMany(u => u.Reviews)
                      .HasForeignKey(r => r.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Unique composite index on TripId + UserId to prevent duplicate reviews by same user on the same trip
                entity.HasIndex(r => new { r.TripId, r.UserId })
                      .IsUnique();
            });



        }
    }
}
