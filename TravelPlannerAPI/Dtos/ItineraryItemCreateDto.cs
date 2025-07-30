using System.ComponentModel.DataAnnotations;

namespace TravelPlannerAPI.Dtos
{
    public record ItineraryItemCreateDto
    {
        [Required]
        public string Title { get; set; }

        [StringLength(300, ErrorMessage = "Description can't be longer than 200 characters.")]

        public string Description { get; set; }

        [Required]
        public DateTime ScheduledDateTime { get; set; }
    }

}
