using System.ComponentModel.DataAnnotations;

namespace TravelPlannerAPI.Dtos
{
    public record TripUpdateDto : TripCreateDto
    {
        [Required]
        public int Id { get; set; }
    }
}
