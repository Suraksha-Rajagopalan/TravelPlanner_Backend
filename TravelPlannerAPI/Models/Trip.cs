using System.ComponentModel.DataAnnotations;

namespace TravelPlannerAPI.Models
{
    public class Trip
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Destination { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }

        public int UserId { get; set; }  //Foreign Key
        public User User { get; set; }  //Navigation property
    }
}
