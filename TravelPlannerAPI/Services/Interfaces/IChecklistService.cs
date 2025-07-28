using TravelPlannerAPI.Dtos;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TravelPlannerAPI.Services.Interfaces
{
    public interface IChecklistService
    {
        Task<ChecklistWithAccessDto> GetChecklistAsync(int tripId, int userId);
        Task<ChecklistItemDto> AddItemAsync(ChecklistItemDto dto, int userId);
        Task<ChecklistItemDto> UpdateItemAsync(int id, ChecklistItemUpdateDto dto, int userId);
        Task<bool> DeleteItemAsync(int id, int userId);
        Task<ChecklistItemDto> ToggleCompletionAsync(int id, int userId);
    }
}
