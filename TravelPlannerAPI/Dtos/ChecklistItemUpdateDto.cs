namespace TravelPlannerAPI.Dtos
{
    public record ChecklistItemUpdateDto
    {
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }

}
