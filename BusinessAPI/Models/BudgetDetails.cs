using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BusinessAPI.Models
{
    public class BudgetDetails
    {
        [Key]
        public int Id { get; set; }

        public decimal Food { get; set; } = 0;
        public decimal Hotel { get; set; } = 0;

        [ForeignKey("Trip")]
        public int TripId { get; set; }

        [JsonIgnore]
        public Trip Trip { get; set; } = null!;
    }
}
