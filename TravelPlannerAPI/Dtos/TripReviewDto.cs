using System.ComponentModel.DataAnnotations;

namespace TravelPlannerAPI.Dtos
{
    public record TripReviewDto
    {
        [Required]
        public int TripId { get; set; }
        public string TripName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public int Rating { get; set; }
        [StringLength(300, ErrorMessage = "Destination cannot exceed 300 characters.")]
        public string? Comment { get; set; }
    }

}
