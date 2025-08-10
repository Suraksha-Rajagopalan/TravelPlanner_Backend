using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TravelPlannerAPI.Models
{
    public class ExpenseModel
    {
        public int Id { get; set; }

        [ForeignKey("Trip")]
        public int TripId { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        public string Category { get; set; } = null!; // 'Food', 'Travel', etc.
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [ValidateNever]
        [JsonIgnore]
        public TripModel Trip { get; set; } = null!;
        [ValidateNever]
        [JsonIgnore]
        public UserModel User { get; set; } = null!;
    }
}
