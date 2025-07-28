using System.Threading.Tasks;

namespace TravelPlannerAPI.Repository.Interfaces
{
    public interface IAccessRepository
    {
        /// <summary>Returns true if the trip exists and is owned by the user.</summary>
        Task<bool> IsOwnerAsync(int tripId, int userId);

        /// <summary>Returns true if the trip is shared with the user.</summary>
        Task<bool> IsSharedAsync(int tripId, int userId);
    }
}
