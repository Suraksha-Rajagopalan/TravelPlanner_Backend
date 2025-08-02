using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http.HttpResults;

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

        private int? UserId
        {
            get
            {
                var claim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (claim == null) return null;               // handle missing claim
                if (int.TryParse(claim.Value, out var id))
                    return id;                                // valid integer
                return null;                                  // parse failed
            }
        }


        
        [HttpGet("{tripId}")]
        public async Task<IActionResult> GetChecklist(int tripId)
        {
            if (UserId == null)
                return BadRequest(new { message = "UserId cannot be null" });

            var dto = await _service.GetChecklistAsync(tripId, UserId.Value);
            if (dto == null) return Forbid();
            return Ok(dto);
        }

        
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] ChecklistItemDto dto)
        {
            if (UserId == null)
                return BadRequest(new { message = "UserId cannot be null" });

            var result = await _service.AddItemAsync(dto, UserId.Value);
            if (result == null) return Forbid();
            return Ok(result);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] ChecklistItemUpdateDto dto)
        {
            if (UserId == null)
                return BadRequest(new { message = "UserId cannot be null" });

            var result = await _service.UpdateItemAsync(id, dto, UserId.Value);
            if (result == null) return NotFound();
            return Ok(result);
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            if (UserId == null)
                return BadRequest(new { message = "UserId cannot be null" });

            if (!await _service.DeleteItemAsync(id, UserId.Value))
                return NotFound();
            return NoContent();
        }

        
        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> ToggleCompletion(int id)
        {
            if (UserId == null)
                return BadRequest(new { message = "UserId cannot be null" });

            var result = await _service.ToggleCompletionAsync(id, UserId.Value);
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}
