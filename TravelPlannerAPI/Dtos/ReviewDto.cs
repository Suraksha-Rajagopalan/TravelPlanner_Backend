using System.ComponentModel.DataAnnotations;

namespace TravelPlannerAPI.Dtos
{
    public record ReviewDto
    {
        [Required]
        public int TripId { get; set; }

        [Required]
        public int UserId { get; set; } 

        public int Rating { get; set; }

        [StringLength(300, ErrorMessage = "Description can't be longer than 200 characters.")]
        public string Review { get; set; } = string.Empty;
    }
}
