using TravelPlannerAPI.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TravelPlannerAPI.Dtos
{
    public record SharedTripDto
    {
        [Required]
        public int TripId { get; set; }

        [Required]
        public AccessLevel AccessLevel { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Title name cannot exceed 100 characters.")]
        public string TitleName { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "Destination cannot exceed 100 characters.")]
        public string Destination { get; set; } = string.Empty;

        [Required]
        public DateTime? StartDate { get; set; }

        [Required]
        public DateTime? EndDate { get; set; }

        [StringLength(500, ErrorMessage = "Trip description cannot exceed 500 characters.")]
        public string TripDescription { get; set; } = string.Empty;

        [StringLength(50)]
        public string TravelMode { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Trip budget must be a positive value.")]
        public decimal? TripBudget { get; set; }

        [StringLength(500)]
        public string TripNotes { get; set; } = string.Empty;

        public string TripImage { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Duration { get; set; }

        [StringLength(100)]
        public string BestTime { get; set; } = string.Empty;

        public List<string> Essentials { get; set; } = new();

        public List<string> TouristSpots { get; set; } = new();

        public string Title { get; internal set; } = string.Empty;
        public string? Description { get; internal set; }
        public decimal Budget { get; internal set; }
        public string Notes { get; internal set; } = string.Empty;
        public string? Image { get; internal set; }
    }
}
