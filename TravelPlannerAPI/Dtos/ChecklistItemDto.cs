namespace TravelPlannerAPI.Dtos
{
    public class ChecklistItemDto
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public int? UserId { get; set; } // must match logged-in user
    }

}
