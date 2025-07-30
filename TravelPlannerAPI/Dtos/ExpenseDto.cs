using System.ComponentModel.DataAnnotations;

namespace TravelPlannerAPI.Models.Dtos
{
    public class ExpenseDto
    {

        public int Id { get; set; }
        [Required]
        public int TripId { get; set; }
        public string Category { get; set; } = null!;
        [StringLength(300, ErrorMessage = "Description can't be longer than 200 characters.")]
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
