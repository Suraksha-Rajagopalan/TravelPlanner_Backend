using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TravelPlannerAPI.Models
{
    public class BudgetDetailsModel
    {
        [Key]
        public int Id { get; set; }

        public decimal Food { get; set; } = 0;
        public decimal Hotel { get; set; } = 0;

        [ForeignKey("Trip")]
        public int TripId { get; set; }

        [JsonIgnore]
        public TripModel Trip { get; set; } = null!;
    }
}
