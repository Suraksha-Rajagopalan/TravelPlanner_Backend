using System.ComponentModel.DataAnnotations;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Dtos
{
    public record TripWithAccessDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int TripId { get; set; }
        public string TripName { get; set; }
        [EmailAddress]
        public string OwnerEmail { get; set; }
        public string AccessLevel { get; set; } 
    }
}

