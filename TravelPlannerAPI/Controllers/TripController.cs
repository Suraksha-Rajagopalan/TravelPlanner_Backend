using AutoMapper;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
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
            return Ok(_mapper.Map<IEnumerable<TripDto>>(trips));
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
