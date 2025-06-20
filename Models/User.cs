using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TravelPlannerAPI.Models
{
    [Table("users")]
    public class User : IdentityUser<int>
    {
        public string Name { get; set; } = string.Empty;

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
