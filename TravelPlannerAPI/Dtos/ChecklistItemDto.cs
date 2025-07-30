using System.ComponentModel.DataAnnotations;

namespace TravelPlannerAPI.Dtos
{
    public record ChecklistItemDto
    {
        public int Id { get; set; }

        [Required]
        public int TripId { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "Description can't be longer than 200 characters.")]
        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }

        public int? UserId { get; set; } // you can validate this in controller/service using user context
    }
}
