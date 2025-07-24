namespace BusinessAPI.Dtos
{
    public class ChecklistWithAccessDto
    {
        public List<ChecklistItemDto> Items { get; set; }
        public string AccessLevel { get; set; }
    }

}
