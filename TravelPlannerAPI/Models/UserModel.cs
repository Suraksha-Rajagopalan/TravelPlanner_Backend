using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TravelPlannerAPI.Models
{
    [Table("users")]
    public class UserModel : IdentityUser<int>
    {
        public string Name { get; set; } = string.Empty;

        public DateTime LastLoginDate { get; set; } = DateTime.UtcNow;

        public bool IsAdmin { get; set; } = false;

        public string Role { get; set; } = "User"; // Default role

        public bool IsActive { get; set; } = true;

        // User-created trips
        public ICollection<TripModel> Trips { get; set; } = new List<TripModel>();

        // User reviews
        public ICollection<ReviewModel> Reviews { get; set; } = new List<ReviewModel>();

        // Trips this user has shared with others
        [InverseProperty("Owner")]
        public ICollection<TripShareModel> OwnedTripShares { get; set; } = new List<TripShareModel>();

        // Trips this user has received from others
        [InverseProperty("SharedWithUser")]
        public ICollection<TripShareModel> ReceivedTripShares { get; set; } = new List<TripShareModel>();

        public ICollection<ExpenseModel> Expenses { get; set; } = new List<ExpenseModel>();

    }
}
