namespace TravelPlannerAPI.Models.Dtos
{
    public class ExpenseDto
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public string Category { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
