namespace TravelPlannerAPI.Dtos
{
    public class UpdateProfileDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }        // optional: handle carefully in production
        public string? Phone { get; set; }
        public string? ProfileImage { get; set; }
        public string? Address { get; set; }
        public string? Bio { get; set; }
    }
}
