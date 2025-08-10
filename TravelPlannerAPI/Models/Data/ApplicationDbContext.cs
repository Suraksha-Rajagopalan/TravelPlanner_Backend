using TravelPlannerAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TravelPlannerAPI.Controllers;

namespace TravelPlannerAPI.Models.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserModel, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<TripModel> Trips { get; set; }
        public DbSet<BudgetDetailsModel> BudgetDetails { get; set; }
        public DbSet<ReviewModel> Reviews { get; set; }
        public DbSet<ItineraryItemsModel> ItineraryItems { get; set; }
        public DbSet<ChecklistItemModel> ChecklistItems { get; set; }
        public DbSet<ExpenseModel> Expenses { get; set; }
        public DbSet<TripShareModel> TripShares { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Identity table renaming
            modelBuilder.Entity<UserModel>().ToTable("Users");
            modelBuilder.Entity<IdentityRole<int>>().ToTable("role");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("user_roles");
            modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("user_claims");
            modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("user_logins");
            modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("role_claims");
            modelBuilder.Entity<IdentityUserToken<int>>().ToTable("user_tokens");

            // Table names
            modelBuilder.Entity<TripModel>().ToTable("Trips");
            modelBuilder.Entity<BudgetDetailsModel>().ToTable("budgetdetails");

            // Trip → User
            modelBuilder.Entity<TripModel>()
                .HasOne(t => t.User)
                .WithMany(u => u.Trips)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Trip → BudgetDetails
            modelBuilder.Entity<TripModel>()
                .HasOne(t => t.BudgetDetails)
                .WithOne(b => b.Trip)
                .HasForeignKey<BudgetDetailsModel>(b => b.TripId)
                .OnDelete(DeleteBehavior.Cascade);

            // ChecklistItem → User
            modelBuilder.Entity<ChecklistItemModel>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // TripShare → Trip
            modelBuilder.Entity<TripShareModel>()
                .HasOne(ts => ts.Trip)
                .WithMany(t => t.SharedUsers)
                .HasForeignKey(ts => ts.TripId)
                .OnDelete(DeleteBehavior.Restrict);

            // TripShare → Owner
            modelBuilder.Entity<TripShareModel>()
                .HasOne(ts => ts.Owner)
                .WithMany(u => u.OwnedTripShares)
                .HasForeignKey(ts => ts.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // TripShare → SharedWithUser
            modelBuilder.Entity<TripShareModel>()
                .HasOne(ts => ts.SharedWithUser)
                .WithMany(u => u.ReceivedTripShares)
                .HasForeignKey(ts => ts.SharedWithUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // TripShare Unique Constraint
            modelBuilder.Entity<TripShareModel>()
                .HasIndex(ts => new { ts.TripId, ts.SharedWithUserId })
                .IsUnique();

            // Reviews
            modelBuilder.Entity<ReviewModel>(entity =>
            {
                entity.HasOne(r => r.Trip)
                      .WithMany(t => t.Reviews)
                      .HasForeignKey(r => r.TripId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.User)
                      .WithMany(u => u.Reviews)
                      .HasForeignKey(r => r.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(r => new { r.TripId, r.UserId }).IsUnique();
            });

            // Admin
            modelBuilder.Entity<TripModel>()
                 .HasOne(t => t.User)
                 .WithMany(u => u.Trips)
                 .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<ExpenseModel>()
                 .HasOne(e => e.User)
                 .WithMany(u => u.Expenses)
                 .HasForeignKey(e => e.UserId);

        }

    }
}
