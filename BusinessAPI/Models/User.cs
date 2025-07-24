using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BusinessAPI.Models
{
    [Table("users")]
    public class User : IdentityUser<int>
    {
        public string Name { get; set; } = string.Empty;

        public DateTime LastLoginDate { get; set; } = DateTime.UtcNow;

        public bool IsAdmin { get; set; } = false;

        public string Role { get; set; } = "User"; // Default role

        public bool IsActive { get; set; } = true;

        // User-created trips
        public ICollection<Trip> Trips { get; set; } = new List<Trip>();

        // User reviews
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        // Trips this user has shared with others
        [InverseProperty("Owner")]
        public ICollection<TripShare> OwnedTripShares { get; set; } = new List<TripShare>();

        // Trips this user has received from others
        [InverseProperty("SharedWithUser")]
        public ICollection<TripShare> ReceivedTripShares { get; set; } = new List<TripShare>();


    }
}
