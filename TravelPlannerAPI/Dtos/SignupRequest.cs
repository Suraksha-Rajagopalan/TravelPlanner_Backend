using System.ComponentModel.DataAnnotations;

namespace TravelPlannerAPI.Dtos
{
    public record SignupRequest
    {
        public required string Name { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
