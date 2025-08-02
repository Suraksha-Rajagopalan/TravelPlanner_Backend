using AutoMapper;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    //[ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ItineraryController : ControllerBase
    {
        private readonly IItineraryService _service;
        private readonly IMapper _mapper;

        public ItineraryController(
            IItineraryService service,
            IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetItineraryItems(int tripId)
        {
            var items = await _service.GetItineraryItemsByTripIdAsync(tripId);
            return Ok(items);
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateItineraryItem(
            int tripId,
            [FromBody] ItineraryItemCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = await _service.AddItineraryItemAsync(tripId, dto);
            return Ok(item);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItineraryItem(
            int tripId,
            int id,
            [FromBody] ItineraryItemCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _service.UpdateItineraryItemAsync(id, dto);
            if (!updated) return NotFound();
            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItineraryItem(int tripId, int id)
        {
            var deleted = await _service.DeleteItineraryItemAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
