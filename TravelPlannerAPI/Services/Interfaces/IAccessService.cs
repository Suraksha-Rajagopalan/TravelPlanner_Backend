using System.Threading.Tasks;

namespace TravelPlannerAPI.Services.Interfaces
{
    public interface IAccessService
    {
        Task<bool> HasAccessToTripAsync(int tripId, int userId);

        /// <summary>
        /// Returns "Edit" if the user is the owner, otherwise "View".
        /// </summary>
        Task<string> GetAccessLevelAsync(int tripId, int userId);
    }
}
