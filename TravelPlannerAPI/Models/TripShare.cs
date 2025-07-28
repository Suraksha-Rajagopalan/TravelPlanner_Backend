using System.ComponentModel.DataAnnotations.Schema;
using TravelPlannerAPI.Models.Enums;
using TravelPlannerAPI.Models;
using System.Text.Json.Serialization;

public class TripShare
{
    public int Id { get; set; }

    public int TripId { get; set; }

    [JsonIgnore]
    public Trip Trip { get; set; } = null!;

    public int OwnerId { get; set; }

    [ForeignKey("OwnerId")]
    [InverseProperty("OwnedTripShares")]
    public User Owner { get; set; } = null!;

    public int SharedWithUserId { get; set; }

    [ForeignKey("SharedWithUserId")]
    [InverseProperty("ReceivedTripShares")]
    public User SharedWithUser { get; set; } = null!;

    public AccessLevel AccessLevel { get; set; }
}
