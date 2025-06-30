using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TravelPlannerAPI.Models
{
    public class Expense
    {
        public int Id { get; set; }

        [ForeignKey("Trip")]
        public int TripId { get; set; }

        public string Category { get; set; } = null!; // 'Food', 'Travel', etc.
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [ValidateNever]
        [JsonIgnore]
        public Trip Trip { get; set; } = null!;
    }
}
