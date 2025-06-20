namespace TravelPlannerAPI.Dtos
{
    public class TripReviewDto
    {
        public int TripId { get; set; }
        public string TripName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }

}
