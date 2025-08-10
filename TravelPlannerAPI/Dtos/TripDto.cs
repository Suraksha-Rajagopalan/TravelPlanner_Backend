using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TravelPlannerAPI.Attributes;

namespace TravelPlannerAPI.Dtos
{
    public record TripDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "Destination cannot exceed 100 characters.")]
        public string Destination { get; set; } = string.Empty;

        [SmartRequired(ErrorMessage = "Start date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        public DateTime EndDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Budget must be a positive number.")]
        public decimal Budget { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Travel mode cannot exceed 50 characters.")]
        public string TravelMode { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string Notes { get; set; } = string.Empty;

        [Required]
        public int UserId { get; set; }

        [StringLength(200, ErrorMessage = "Image path/URL cannot exceed 200 characters.")]
        public string? Image { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [StringLength(50, ErrorMessage = "Duration cannot exceed 50 characters.")]
        public string? Duration { get; set; }

        [StringLength(100, ErrorMessage = "Best time cannot exceed 100 characters.")]
        public string? BestTime { get; set; }

        public List<string>? Essentials { get; set; }

        public List<string>? TouristSpots { get; set; }

        public BudgetDetailsDto? BudgetDetails { get; set; }

        public ReviewDto? Review { get; set; }
    }
}
