namespace BusinessAPI.Models
{
    public class TripReview
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }

        public Trip Trip { get; set; } = null!;
        public User User { get; set; } = null!;
    }

}
