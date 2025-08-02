using System.ComponentModel.DataAnnotations.Schema;
using TravelPlannerAPI.Models.Enums;
using TravelPlannerAPI.Models;
using System.Text.Json.Serialization;

public class TripShareModel
{
    public int Id { get; set; }

    public int TripId { get; set; }

    [JsonIgnore]
    public TripModel Trip { get; set; } = null!;

    public int OwnerId { get; set; }

    [ForeignKey("OwnerId")]
    [InverseProperty("OwnedTripShares")]
    public UserModel Owner { get; set; } = null!;

    public int SharedWithUserId { get; set; }

    [ForeignKey("SharedWithUserId")]
    [InverseProperty("ReceivedTripShares")]
    public UserModel SharedWithUser { get; set; } = null!;

    public AccessLevel AccessLevel { get; set; }
}
