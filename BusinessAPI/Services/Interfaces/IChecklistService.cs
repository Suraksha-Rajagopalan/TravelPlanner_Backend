using BusinessAPI.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessAPI.Services.Interfaces
{
    public interface IChecklistService
    {
        Task<(bool CanView, bool CanEdit)> GetAccessAsync(int tripId, int userId);
        Task<IEnumerable<ChecklistItemDto>> GetChecklistAsync(int tripId, int userId);
        Task<ChecklistItemDto> AddItemAsync(int tripId, ChecklistItemDto itemDto, int userId);
        Task<bool> UpdateItemAsync(int tripId, int id, ChecklistItemUpdateDto updateDto, int userId);
        Task<bool> DeleteItemAsync(int tripId, int id, int userId);
        Task<IEnumerable<ChecklistItemDto>> GetSharedTripChecklistAsync(int tripId, int userId);
    }
}
