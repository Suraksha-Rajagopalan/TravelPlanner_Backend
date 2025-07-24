namespace BusinessAPI.Services.Interfaces
{
    public interface IAccessService
    {
        Task<bool> HasAccessToTripAsync(int tripId, int userId);
    }

}
