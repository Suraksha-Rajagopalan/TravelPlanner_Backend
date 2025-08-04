using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Models.Enums;
using TravelPlannerAPI.Repository.Interface;
using TravelPlannerAPI.Services.Interfaces;
using TravelPlannerAPI.UoW;

namespace TravelPlannerAPI.Services.Implementations
{
    public class ChecklistService : IChecklistService
    {
        private readonly IAccessService _access;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ChecklistService(
            IAccessService accessService,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _access = accessService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ChecklistWithAccessDto?> GetChecklistAsync(int tripId, int userId)
        {
            if (!await _access.HasAccessToTripAsync(tripId, userId))
                return null;

            var items = await _unitOfWork.Checklists.GetByTripAndUserAsync(tripId, userId);

            if (items == null)
                return null;

            var accessLevel = await _access.GetAccessLevelAsync(tripId, userId);
            var accessString = accessLevel?.ToString();
            if (accessString != null)
            {
                return new ChecklistWithAccessDto
                {
                    AccessLevel = accessString,
                    Items = items.Select(i => _mapper.Map<ChecklistItemDto?>(i)).ToList()
                };
            }
            return null;
            
        }


        public async Task<ChecklistItemDto?> AddItemAsync(ChecklistItemDto dto, int userId)
        {
            if (!await _access.HasAccessToTripAsync(dto.TripId, userId))
                return null;

            var entity = _mapper.Map<ChecklistItemModel>(dto);
            entity.UserId = userId;

            await _unitOfWork.Checklists.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ChecklistItemDto>(entity);
        }

        public async Task<ChecklistItemDto?> UpdateItemAsync(int id, ChecklistItemUpdateDto dto, int userId)
        {
            var item = await _unitOfWork.Checklists.GetByIdAsync(id);
            if (item == null || item.UserId != userId ||
                !await _access.HasAccessToTripAsync(item.TripId, userId))
                return null;

            item.Description = dto.Description;
            item.IsCompleted = dto.IsCompleted;

            _unitOfWork.Checklists.Update(item);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ChecklistItemDto>(item);
        }

        public async Task<bool> DeleteItemAsync(int id, int userId)
        {
            var item = await _unitOfWork.Checklists.GetByIdAsync(id);
            if (item == null || item.UserId != userId ||
                !await _access.HasAccessToTripAsync(item.TripId, userId))
                return false;

            _unitOfWork.Checklists.Delete(item);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<ChecklistItemDto?> ToggleCompletionAsync(int id, int userId)
        {
            var item = await _unitOfWork.Checklists.GetByIdAsync(id);
            if (item == null || item.UserId != userId ||
                !await _access.HasAccessToTripAsync(item.TripId, userId))
                return null;

            item.IsCompleted = !item.IsCompleted;
            _unitOfWork.Checklists.Update(item);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ChecklistItemDto>(item);
        }
    }
}
