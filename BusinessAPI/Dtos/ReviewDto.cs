namespace BusinessAPI.Dtos
{
    public class ReviewDto
    {
        public int TripId { get; set; }
        public int UserId { get; set; } 
        public int Rating { get; set; }
        public string Review { get; set; } = string.Empty;
    }
}
