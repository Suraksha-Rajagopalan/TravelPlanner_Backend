using System.ComponentModel.DataAnnotations;

namespace TravelPlannerAPI.DTOs
{
    public class TripUpdateDto : TripCreateDto
    {
        [Required]
        public int Id { get; set; }
    }
}
