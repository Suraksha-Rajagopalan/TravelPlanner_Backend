using System.ComponentModel.DataAnnotations;

namespace BusinessAPI.Dtos
{
    public class ItineraryItemCreateDto
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime ScheduledDateTime { get; set; }
    }

}
