using BusinessAPI.Models;

namespace BusinessAPI.Dtos
{
    public class TripWithAccessDto
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        public string TripName { get; set; }
        public string OwnerEmail { get; set; }
        public string AccessLevel { get; set; } 
    }
}

