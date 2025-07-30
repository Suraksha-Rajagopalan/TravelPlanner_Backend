using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TravelPlannerAPI.Dtos
{
    public record TripCreateDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Destination { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public decimal Budget { get; set; } = 0;

        public string TravelMode { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;

        [Required]
        public int UserId { get; set; }

        public string? Image { get; set; }

        public string? Description { get; set; }

        public string? Duration { get; set; }

        public string? BestTime { get; set; }

        public List<string>? Essentials { get; set; } = new();

        public List<string>? TouristSpots { get; set; } = new();

        public BudgetDetailsDto? BudgetDetails { get; set; }
    }

    public class BudgetDetailsDto
    {
        public decimal Food { get; set; } = 0;
        public decimal Hotel { get; set; } = 0;
    }
}
