namespace TravelPlannerAPI.Dtos
{
    public class AdminUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime? LastLoginDate { get; set; }
        public bool IsActive { get; set; }
        public int NumberOfTrips { get; set; }
        public string Role { get; set; } = "User";
        public List<string> TripTitles { get; set; } = new();
    }

}
