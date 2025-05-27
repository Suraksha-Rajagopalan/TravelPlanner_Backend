using System.Collections.Generic;

namespace TravelPlannerAPI.Models
{
    public class User
    {
        public int Id { get; set; }  // Primary key
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

        public ICollection<Trip> Trips { get; set; }
    }
}
