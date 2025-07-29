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
        private readonly IItineraryRepository _repo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ItineraryService(
            IItineraryRepository repository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _repo = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<ItineraryItem>> GetItineraryItemsByTripIdAsync(int tripId)
            => _repo.GetByTripIdAsync(tripId);

        public Task<ItineraryItem> GetItineraryItemByIdAsync(int id)
            => _repo.GetByIdAsync(id);

        public async Task<ItineraryItem> AddItineraryItemAsync(int tripId, ItineraryItemCreateDto dto)
        {
            var item = _mapper.Map<ItineraryItem>(dto);
            item.TripId = tripId;

            await _repo.AddAsync(item);
            return item;
        }

        public async Task<bool> UpdateItineraryItemAsync(int id, ItineraryItemCreateDto dto)
        {
            if (!await _repo.ExistsAsync(id))
                return false;

            var item = await _repo.GetByIdAsync(id);
            _mapper.Map(dto, item);

            await _repo.UpdateAsync(item);
            return true;
        }

        public async Task<bool> DeleteItineraryItemAsync(int id)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item == null) return false;

            await _repo.DeleteAsync(item);
            return true;
        }

        public Task<IEnumerable<ItineraryItem>> GetSharedItineraryAsync(int tripId, int userId)
            => _repo.GetSharedItineraryAsync(tripId, userId);
    }
}
