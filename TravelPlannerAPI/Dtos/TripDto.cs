namespace TravelPlannerAPI.Dtos
{
    public class TripDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Destination { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Budget { get; set; }
        public string TravelMode { get; set; }
        public string Notes { get; set; }
        public int UserId { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public string? Duration { get; set; }
        public string? BestTime { get; set; }
        public List<string>? Essentials { get; set; }
        public List<string>? TouristSpots { get; set; }
        public BudgetDetailsDto? BudgetDetails { get; set; }
    }
}
