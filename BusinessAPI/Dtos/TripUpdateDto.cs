using System.ComponentModel.DataAnnotations;

namespace BusinessAPI.DTOs
{
    public class TripUpdateDto : TripCreateDto
    {
        [Required]
        public int Id { get; set; }
    }
}
