using TravelPlannerAPI.Models.Enums;
using System;
using System.Collections.Generic;

namespace TravelPlannerAPI.Dtos
{
    public class SharedTripDto
    {
        public int TripId { get; set; }
        public AccessLevel AccessLevel { get; set; }
        public string TitleName { get; set; }
        public string Destination { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string TripDescription { get; set; }
        public string TravelMode { get; set; }
        public decimal? TripBudget { get; set; }
        public string TripNotes { get; set; }
        public string TripImage { get; set; }
        public string? Duration { get; set; }
        public string BestTime { get; set; }
        public List<string> Essentials { get; set; } = new();
        public List<string> TouristSpots { get; set; } = new();
        public string Title { get; internal set; }
        public string? Description { get; internal set; }
        public decimal Budget { get; internal set; }
        public string Notes { get; internal set; }
        public string? Image { get; internal set; }
    }
}
