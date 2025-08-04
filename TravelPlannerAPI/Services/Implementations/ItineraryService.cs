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

        public ItineraryService(
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<ItineraryItemsModel>> GetItineraryItemsByTripIdAsync(int tripId)
            => _unitOfWork.Itineraries.GetByTripIdAsync(tripId);

        public Task<ItineraryItemsModel?> GetItineraryItemByIdAsync(int id)
            => _unitOfWork.Itineraries.GetByIdAsync(id);

        public async Task<ItineraryItemsModel> AddItineraryItemAsync(int tripId, ItineraryItemCreateDto dto)
        {
            var item = _mapper.Map<ItineraryItemsModel>(dto);
            item.TripId = tripId;

            await _unitOfWork.Itineraries.AddAsync(item);
            return item;
        }

        public async Task<bool> UpdateItineraryItemAsync(int id, ItineraryItemCreateDto dto)
        {
            if (!await _unitOfWork.Itineraries.ExistsAsync(id))
                return false;

            var item = await _unitOfWork.Itineraries.GetByIdAsync(id);
            _mapper.Map(dto, item);

            if (item!=null)
            {
                await _unitOfWork.Itineraries.UpdateAsync(item);
                return true;
            }
            return false;

            
        }

        public async Task<bool> DeleteItineraryItemAsync(int id)
        {
            var item = await _unitOfWork.Itineraries.GetByIdAsync(id);
            if (item == null) return false;

            await _unitOfWork.Itineraries.DeleteAsync(item);
            return true;
        }

        public Task<IEnumerable<ItineraryItemsModel>> GetSharedItineraryAsync(int tripId, int userId)
            => _unitOfWork.Itineraries.GetSharedItineraryAsync(tripId, userId);
    }
}
