using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Helpers;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Repository.Implementation;
using TravelPlannerAPI.Repository.Interface;
using TravelPlannerAPI.Services.Interfaces;
using TravelPlannerAPI.UoW;

namespace TravelPlannerAPI.Services.Implementations
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _repo;
        private readonly IAccessService _access;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfwork;

        public TripService(
            ITripRepository repo,
            IAccessService accessService,
            IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _access = accessService;
            _mapper = mapper;
            _unitOfwork = unitOfWork;
        }

        public Task<IEnumerable<Trip>> GetTripsAsync(int userId)
            => _repo.GetByUserAsync(userId);

        public async Task<PaginatedResult<TripDto>> GetPaginatedTripsAsync(PaginationParamsDto pagination)
        {
            var paginatedEntities = await _repo.GetPaginatedTripsAsync(pagination);

            var mappedDtos = _mapper.Map<List<TripDto>>(paginatedEntities.Items);

            return new PaginatedResult<TripDto>(
                mappedDtos,
                paginatedEntities.TotalCount,
                pagination.PageNumber,
                pagination.PageSize
            );
        }

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
            await _unitOfwork.CompleteAsync();
            return trip;
        }

        public async Task<bool> UpdateTripAsync(TripUpdateDto dto, int userId)
        {
            var existing = await _repo.GetByIdWithIncludesAsync(dto.Id);
            if (existing == null || !await _access.HasAccessToTripAsync(dto.Id, userId))
                return false;

            // Map changes
            _mapper.Map(dto, existing);
            await _unitOfwork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteTripAsync(int id, int userId)
        {
            var trip = await _repo.GetByIdAsync(id);
            if (trip == null || trip.UserId != userId)
                return false;

            _repo.Delete(trip);
            await _unitOfwork.CompleteAsync();
            return true;
        }
    }
}
