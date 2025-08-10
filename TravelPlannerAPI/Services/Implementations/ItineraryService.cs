using AutoMapper;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Repository.Interface;
using TravelPlannerAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelPlannerAPI.UoW;

namespace TravelPlannerAPI.Services.Implementations
{
    public class ItineraryService : IItineraryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccessService _access;

        public ItineraryService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IAccessService accessService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _access = accessService;
        }

        public Task<IEnumerable<ItineraryItemsModel>> GetItineraryItemsByTripIdAsync(int tripId)
            => _unitOfWork.Itineraries.GetByTripIdAsync(tripId);

        public Task<ItineraryItemsModel?> GetItineraryItemByIdAsync(int id)
            => _unitOfWork.Itineraries.GetByIdAsync(id);

        public async Task<ItineraryItemsModel?> AddItineraryItemAsync(int tripId, ItineraryItemCreateDto dto, int userId)
        {
            if (!await _access.HasAccessToTripAsync(tripId, userId))
                return null;

            var item = _mapper.Map<ItineraryItemsModel>(dto);
            item.TripId = tripId;

            await _unitOfWork.Itineraries.AddAsync(item);
            await _unitOfWork.CompleteAsync();

            return item;
        }

        public async Task<bool> UpdateItineraryItemAsync(int id, ItineraryItemCreateDto dto, int userId)
        {
            var item = await _unitOfWork.Itineraries.GetByIdAsync(id);
            if (item == null)
                return false;

            if (!await _access.HasAccessToTripAsync(item.TripId, userId))
                return false;

            _mapper.Map(dto, item);
            await _unitOfWork.Itineraries.UpdateAsync(item);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteItineraryItemAsync(int id, int userId)
        {
            var item = await _unitOfWork.Itineraries.GetByIdAsync(id);
            if (item == null)
                return false;

            if (!await _access.HasAccessToTripAsync(item.TripId, userId))
                return false;

            await _unitOfWork.Itineraries.DeleteAsync(item);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public Task<IEnumerable<ItineraryItemsModel>> GetSharedItineraryAsync(int tripId, int userId)
            => _unitOfWork.Itineraries.GetSharedItineraryAsync(tripId, userId);
    }
}
