using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace TravelPlannerAPI.Models
{
    public class TripModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Destination { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public decimal Budget { get; set; } = 0;

        public string TravelMode { get; set; } = "Unknown";

        public string Notes { get; set; } = string.Empty;

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [JsonIgnore]
        public UserModel User { get; set; } = null!;

        public string? Image { get; set; } = null;
        public string? Description { get; set; } = null;
        public string? Duration { get; set; } = null;
        public string? BestTime { get; set; } = null;

        // Internal JSON storage - ignored in API responses
        [JsonIgnore]
        public string? EssentialsJson { get; set; } = JsonSerializer.Serialize(new List<string>());

        [JsonIgnore]
        public string? TouristSpotsJson { get; set; } = JsonSerializer.Serialize(new List<string>());

        // Public deserialized lists for API exposure
        [NotMapped]
        public List<string> Essentials
        {
            get => string.IsNullOrWhiteSpace(EssentialsJson)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(EssentialsJson)!;

            set => EssentialsJson = JsonSerializer.Serialize(value ?? new List<string>());
        }

        [NotMapped]
        public List<string> TouristSpots
        {
            get => string.IsNullOrWhiteSpace(TouristSpotsJson)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(TouristSpotsJson)!;

            set => TouristSpotsJson = JsonSerializer.Serialize(value ?? new List<string>());
        }

        public BudgetDetailsModel? BudgetDetails { get; set; }

        public ICollection<ReviewModel> Reviews { get; set; } = new List<ReviewModel>();

        public ICollection<TripShareModel> SharedUsers { get; set; } = new List<TripShareModel>();

    };
}
