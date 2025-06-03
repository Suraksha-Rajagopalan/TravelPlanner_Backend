using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TravelPlannerAPI.Models
{
    [Table("users")]
    public class User : IdentityUser<int>
    {
        public string Name { get; set; } = string.Empty;

        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
    }
}
