using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelPlannerAPI.Models
{
    [Table("ItineraryItems")]

    public class ItineraryItemsModel
    {
        public int Id { get; set; }
        public int TripId { get; set; }          // Foreign key
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ScheduledDateTime { get; set; }

        // link back to trip
        public TripModel? Trip { get; set; }
    }

}
