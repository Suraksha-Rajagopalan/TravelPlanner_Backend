using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessAPI.Models
{
    [Table("ItineraryItems")]
    public class ItineraryItem
    {
        public int Id { get; set; }
        public int TripId { get; set; }          // Foreign key
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ScheduledDateTime { get; set; }

        // link back to trip
        public Trip? Trip { get; set; }
    }

}
