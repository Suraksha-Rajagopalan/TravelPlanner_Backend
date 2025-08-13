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

        public Task<IEnumerable<TripModel>> GetTripsAsync(int userId)
            => _repo.GetByUserAsync(userId);

        public async Task<PaginatedResult<TripDto>> GetPaginatedTripsAsync(PaginationParamsDto pagination, int userId)
        {
            var paginatedEntities = await _repo.GetPaginatedTripsAsync(pagination, userId);

            var mappedDtos = paginatedEntities.Items.Select(t => new TripDto
            {
                Id = t.Id,
                Title = t.Title,
                Destination = t.Destination,
                StartDate = t.StartDate,
                EndDate = t.EndDate,
                Budget = t.Budget,
                TravelMode = t.TravelMode,
                Notes = t.Notes,
                UserId = t.UserId,
                Image = t.Image,
                Description = t.Description,
                Duration = t.Duration,
                BestTime = t.BestTime,
                Essentials = t.Essentials?.ToList(),
                TouristSpots = t.TouristSpots?.ToList(),
                BudgetDetails = t.BudgetDetails == null ? null : new BudgetDetailsDto
                {
                    Food = t.BudgetDetails.Food,
                    Hotel = t.BudgetDetails.Hotel
                },
                    Review = t.Reviews
                    ?.Where(r => r.UserId == userId)
                    .Select(r => new ReviewDto
                {
                    TripId = r.TripId,
                    UserId = r.UserId,
                    Rating = r.Rating,
                    Review = r.ReviewText
                    })
                        .FirstOrDefault()
             }).ToList();


            return new PaginatedResult<TripDto>(
                mappedDtos,
                paginatedEntities.TotalCount,
                pagination.PageNumber,
                pagination.PageSize
            );
        }


        public async Task<TripModel?> GetTripByIdAsync(int id, int userId)
        {
            var trip = await _repo.GetByIdWithIncludesAsync(id);
            if (trip == null || !await _access.HasAccessToTripAsync(id, userId))
                return null;
            return trip;
        }

        public async Task<TripModel> CreateTripAsync(TripCreateDto dto, int userId)
        {
            // Map DTO → entity
            var trip = _mapper.Map<TripModel>(dto);
            trip.UserId = userId;
            // Persist
            await _repo.AddAsync(trip);
            await _unitOfwork.CompleteAsync();
            return trip;
        }

        public async Task<bool> UpdateTripAsync(TripUpdateDto dto, int userId)
        {
            // Fetch trip with related BudgetDetails
            var existing = await _repo.GetByIdWithIncludesAsync(dto.Id);
            if (existing == null || !await _access.HasAccessToTripAsync(dto.Id, userId))
                return false;

            // Update basic fields
            existing.Title = dto.Title;
            existing.Destination = dto.Destination;
            existing.StartDate = dto.StartDate;
            existing.EndDate = dto.EndDate;
            existing.Budget = dto.Budget;
            existing.TravelMode = dto.TravelMode;
            existing.Notes = dto.Notes;
            existing.Image = dto.Image;
            existing.Description = dto.Description;
            existing.Duration = dto.Duration;
            existing.BestTime = dto.BestTime;

            // Update list fields
            existing.Essentials = dto.Essentials ?? new List<string>();
            existing.TouristSpots = dto.TouristSpots ?? new List<string>();

            // Handle BudgetDetails (one-to-one)
            if (existing.BudgetDetails != null)
            {
                // Update existing budget
                existing.BudgetDetails.Food = dto.BudgetDetails?.Food ?? 0;
                existing.BudgetDetails.Hotel = dto.BudgetDetails?.Hotel ?? 0;
            }
            else if (dto.BudgetDetails != null)
            {
                // Create new budget with correct TripId
                existing.BudgetDetails = new BudgetDetailsModel
                {
                    TripId = existing.Id,
                    Food = dto.BudgetDetails.Food,
                    Hotel = dto.BudgetDetails.Hotel
                };
            }

            // Persist changes
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
