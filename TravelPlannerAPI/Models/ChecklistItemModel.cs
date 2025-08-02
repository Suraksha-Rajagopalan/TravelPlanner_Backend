namespace TravelPlannerAPI.Models
{
    public class ChecklistItemModel
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public int UserId { get; set; }
        public TripModel? Trip { get; set; }
        public UserModel? User { get; set; }
    }

}
