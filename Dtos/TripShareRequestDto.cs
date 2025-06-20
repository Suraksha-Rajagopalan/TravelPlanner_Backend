using TravelPlannerAPI.Models.Enums;

namespace TravelPlannerAPI.Dtos
{
        public class TripShareRequestDto
        {
            public int TripId { get; set; }
            public string SharedWithEmail { get; set; } = string.Empty;
        public string AccessLevel { get; set; } = string.Empty; // "View" or "Edit"
    }

}
