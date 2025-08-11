using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Services.Implementations;
using TravelPlannerAPI.Services.Interfaces;

namespace TravelPlannerAPI.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    //[ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly ITripService _service;
        private readonly IMapper _mapper;

        public TripController(ITripService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        private int UserId =>
            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
            ? id : 0;


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripDto>>> GetTrips()
        {
            var trips = await _service.GetTripsAsync(UserId); 

            var tripDtos = trips.Select(t => new TripDto
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
                    ?.Where(r => r.UserId == UserId)
                    .Select(r => new ReviewDto
                    {
                        TripId = r.TripId,
                        UserId = r.UserId,
                        Rating = r.Rating,
                        Review = r.ReviewText
                    })
                    .FirstOrDefault()
            }).ToList();

            return Ok(tripDtos);
        }

        [HttpGet("page")]
        public async Task<IActionResult> GetTrips([FromQuery] PaginationParamsDto paginationParams)
        {
            var pagedResult = await _service.GetPaginatedTripsAsync(paginationParams, UserId);
            return Ok(pagedResult);
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<TripDto>> GetTrip(int id)
        {
            var trip = await _service.GetTripByIdAsync(id, UserId);
            if (trip == null) return NotFound();
            return Ok(_mapper.Map<TripDto>(trip));
        }

        
        [HttpPost]
        public async Task<ActionResult<TripDto>> CreateTrip([FromBody] TripCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var trip = await _service.CreateTripAsync(dto, UserId);
            return CreatedAtAction(nameof(GetTrip), new { id = trip.Id }, _mapper.Map<TripDto>(trip));
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrip(int id, [FromBody] TripUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != dto.Id) return BadRequest();
            Console.WriteLine("==> TripController PUT method triggered");
            var ok = await _service.UpdateTripAsync(dto, UserId);
            if (!ok) return NotFound();
            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            var ok = await _service.DeleteTripAsync(id, UserId);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
