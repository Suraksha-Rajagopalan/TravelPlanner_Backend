using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models.Data;
using TravelPlannerAPI.Repository.Interface;

namespace TravelPlannerAPI.Repository.Implementation
{
    public class TripReviewRepository : ITripReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public TripReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TripReviewDto>> SearchReviewsByDestinationAsync(string destination)
        {
            if (string.IsNullOrWhiteSpace(destination))
                return new List<TripReviewDto>();

            var reviews = await _context.Reviews
                .Include(r => r.Trip)
                .Include(r => r.User)
                .Where(r => r.Trip != null && EF.Functions.Like(r.Trip.Destination!, $"%{destination}%"))
                .Select(r => new TripReviewDto
                {
                    TripId = r.TripId,
                    TripName = r.Trip != null ? r.Trip.Destination : "Unknown Trip",
                    Username = r.User != null ? r.User.Name : "Unknown User",
                    Rating = r.Rating,
                    Comment = r.ReviewText
                })
                .ToListAsync();
            return reviews;
        }
    }
}
