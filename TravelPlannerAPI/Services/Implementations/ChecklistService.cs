using AutoMapper;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Repository.Interface;
using TravelPlannerAPI.UoW;
using TravelPlannerAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace TravelPlannerAPI.Services.Implementations
{
    public class ChecklistService : IChecklistService
    {
        private readonly IChecklistRepository _repo;
        private readonly IAccessService _access;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ChecklistService(
            IChecklistRepository repo,
            IAccessService accessService,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _access = accessService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ChecklistWithAccessDto> GetChecklistAsync(int tripId, int userId)
        {
            if (!await _access.HasAccessToTripAsync(tripId, userId))
                return null;

            var items = await _repo.GetByTripAndUserAsync(tripId, userId);
            return new ChecklistWithAccessDto
            {
                AccessLevel = await _access.GetAccessLevelAsync(tripId, userId),
                Items = items.Select(i => _mapper.Map<ChecklistItemDto>(i)).ToList()
            };
        }


        public async Task<ChecklistItemDto> AddItemAsync(ChecklistItemDto dto, int userId)
        {
            if (!await _access.HasAccessToTripAsync(dto.TripId, userId))
                return null;

            var entity = _mapper.Map<ChecklistItem>(dto);
            entity.UserId = userId;

            await _repo.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ChecklistItemDto>(entity);
        }

        public async Task<ChecklistItemDto> UpdateItemAsync(int id, ChecklistItemUpdateDto dto, int userId)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item == null || item.UserId != userId ||
                !await _access.HasAccessToTripAsync(item.TripId, userId))
                return null;

            item.Text = dto.Description;
            item.IsCompleted = dto.IsCompleted;

            _repo.Update(item);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ChecklistItemDto>(item);
        }

        public async Task<bool> DeleteItemAsync(int id, int userId)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item == null || item.UserId != userId ||
                !await _access.HasAccessToTripAsync(item.TripId, userId))
                return false;

            _repo.Delete(item);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<ChecklistItemDto> ToggleCompletionAsync(int id, int userId)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item == null || item.UserId != userId ||
                !await _access.HasAccessToTripAsync(item.TripId, userId))
                return null;

            item.IsCompleted = !item.IsCompleted;
            _repo.Update(item);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ChecklistItemDto>(item);
        }
    }
}
