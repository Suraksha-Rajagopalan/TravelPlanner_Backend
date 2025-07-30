namespace TravelPlannerAPI.Dtos
{
    public record ChecklistWithAccessDto
    {
        public List<ChecklistItemDto> Items { get; set; }
        public string AccessLevel { get; set; }
    }

}
