namespace TravelPlannerAPI.Models
{
    public class TripReviewModel
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }

        public TripModel Trip { get; set; } = null!;
        public UserModel User { get; set; } = null!;
    }

}
