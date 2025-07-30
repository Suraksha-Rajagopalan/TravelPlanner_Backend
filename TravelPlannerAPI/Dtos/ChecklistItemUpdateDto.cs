namespace TravelPlannerAPI.Dtos
{
    public record ChecklistItemUpdateDto
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }

}
