namespace TravelPlannerAPI.Models
{
    public class ChecklistItem
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public int UserId { get; set; }
        public Trip? Trip { get; set; }
        public User User { get; set; }
    }

}
