using System.ComponentModel.DataAnnotations;

namespace TravelPlannerAPI.Dtos
{
    public class TripUpdateDto : TripCreateDto
    {
        [Required]
        public int Id { get; set; }
    }
}
