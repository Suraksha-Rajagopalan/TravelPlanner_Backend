using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TravelPlannerAPI.Dtos
{
    public record AdminUserDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;

        public DateTime? LastLoginDate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Number of trips must be zero or greater.")]
        public int NumberOfTrips { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Role cannot exceed 50 characters.")]
        public string Role { get; set; } = "User";

        public List<string> TripTitles { get; set; } = new();
    }
}
