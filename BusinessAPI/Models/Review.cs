using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessAPI.Models
{
    [Table("reviews")]
    public class Review
    {
        public int Id { get; set; }               // Primary key
        public int TripId { get; set; }           // Foreign key to Trip
        public int UserId { get; set; }           // Foreign key to User

        public int Rating { get; set; }           // 1 to 5

        [Column("review")]
        public string ReviewText { get; set; } = string.Empty;

        public Trip? Trip { get; set; }           // Navigation property

        public User? User { get; set; }

    }

}
