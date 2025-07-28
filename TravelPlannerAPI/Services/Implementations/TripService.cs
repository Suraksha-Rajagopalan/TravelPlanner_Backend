using AutoMapper;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Repository.Interfaces;
using TravelPlannerAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Services.Implementations
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _repo;
        private readonly IAccessService _access;
        private readonly IMapper _mapper;

        public TripService(
            ITripRepository repo,
            IAccessService accessService,
            IMapper mapper)
        {
            _repo = repo;
            _access = accessService;
            _mapper = mapper;
        }

        public Task<IEnumerable<Trip>> GetTripsAsync(int userId)
            => _repo.GetByUserAsync(userId);

        public async Task<Trip> GetTripByIdAsync(int id, int userId)
        {
            var trip = await _repo.GetByIdWithIncludesAsync(id);
            if (trip == null || !await _access.HasAccessToTripAsync(id, userId))
                return null;
            return trip;
        }

        public async Task<Trip> CreateTripAsync(TripCreateDto dto, int userId)
        {
            // Map DTO → entity
            var trip = _mapper.Map<Trip>(dto);
            trip.UserId = userId;
            // Persist
            await _repo.AddAsync(trip);
            await _repo.SaveAsync();
            return trip;
        }

        public async Task<bool> UpdateTripAsync(TripUpdateDto dto, int userId)
        {
            var existing = await _repo.GetByIdWithIncludesAsync(dto.Id);
            if (existing == null || !await _access.HasAccessToTripAsync(dto.Id, userId))
                return false;

            // Map changes
            _mapper.Map(dto, existing);
            await _repo.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteTripAsync(int id, int userId)
        {
            var trip = await _repo.GetByIdAsync(id);
            if (trip == null || trip.UserId != userId)
                return false;

            _repo.Delete(trip);
            await _repo.SaveAsync();
            return true;
        }
    }
}
