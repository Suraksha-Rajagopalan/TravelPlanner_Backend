using System.ComponentModel.DataAnnotations;
using TravelPlannerAPI.Models.Enums;


namespace TravelPlannerAPI.Dtos
{
        public record TripShareRequestDto
        {
        [Required]
            public int TripId { get; set; }
        [EmailAddress]
            public string SharedWithEmail { get; set; } = string.Empty;
        public string AccessLevel { get; set; } = string.Empty; // "View" or "Edit"
    }

}
