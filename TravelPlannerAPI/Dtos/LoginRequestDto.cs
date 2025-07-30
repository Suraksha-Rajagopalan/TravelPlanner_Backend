using System.ComponentModel.DataAnnotations;

namespace TravelPlannerAPI.Dtos
{
    public record LoginRequestDto
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
