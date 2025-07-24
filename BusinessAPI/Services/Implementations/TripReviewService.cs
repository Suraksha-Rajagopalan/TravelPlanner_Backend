using BusinessAPI.Dtos;
using BusinessAPI.Models.Data;
using BusinessAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessAPI.Services
{
    public class TripReviewService : ITripReviewService
    {
        private readonly ApplicationDbContext _context;

        public TripReviewService(ApplicationDbContext context)
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
                .Where(r => EF.Functions.Like(r.Trip.Destination, $"%{destination}%"))
                .Select(r => new TripReviewDto
                {
                    TripId = r.TripId,
                    TripName = r.Trip.Destination,
                    Username = r.User.Name,
                    Rating = r.Rating,
                    Comment = r.ReviewText
                })
                .ToListAsync();

            return reviews;
        }
    }
}
