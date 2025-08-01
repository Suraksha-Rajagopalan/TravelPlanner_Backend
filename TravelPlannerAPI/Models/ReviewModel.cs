﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelPlannerAPI.Models
{
    [Table("reviews")]
    public class ReviewModel
    {
        public int Id { get; set; }               // Primary key
        public int TripId { get; set; }           // Foreign key to Trip
        public int UserId { get; set; }           // Foreign key to User

        public int Rating { get; set; }           // 1 to 5

        [Column("review")]
        public string ReviewText { get; set; } = string.Empty;

        public TripModel? Trip { get; set; }           // Navigation property

        public UserModel? User { get; set; }

    }

}
