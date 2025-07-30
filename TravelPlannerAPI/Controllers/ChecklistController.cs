using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TravelPlannerAPI.Controllers
{
    [ApiVersion("1.0")]
    //[ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class ChecklistController : ControllerBase
    {
        private readonly IChecklistService _service;

        public ChecklistController(IChecklistService service)
        {
            _service = service;
        }

        private int UserId =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        [MapToApiVersion("1.0")]
        [HttpGet("{tripId}")]
        public async Task<IActionResult> GetChecklist(int tripId)
        {
            var dto = await _service.GetChecklistAsync(tripId, UserId);
            if (dto == null) return Forbid();
            return Ok(dto);
        }

        [MapToApiVersion("1.0")]
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] ChecklistItemDto dto)
        {
            var result = await _service.AddItemAsync(dto, UserId);
            if (result == null) return Forbid();
            return Ok(result);
        }

        [MapToApiVersion("1.0")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] ChecklistItemUpdateDto dto)
        {
            var result = await _service.UpdateItemAsync(id, dto, UserId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [MapToApiVersion("1.0")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            if (!await _service.DeleteItemAsync(id, UserId))
                return NotFound();
            return NoContent();
        }

        [MapToApiVersion("1.0")]
        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> ToggleCompletion(int id)
        {
            var result = await _service.ToggleCompletionAsync(id, UserId);
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}
