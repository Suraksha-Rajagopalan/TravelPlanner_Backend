namespace TravelPlannerAPI.Dtos
{
    public record ChecklistWithAccessDto
    {
        public List<ChecklistItemDto?> Items { get; set; } = new();
        public string AccessLevel { get; set; } = string.Empty;
    }

}
