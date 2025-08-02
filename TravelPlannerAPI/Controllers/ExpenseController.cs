using TravelPlannerAPI.Models;
using TravelPlannerAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BusinessAPI.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    //[ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        private int? UserId
        {
            get
            {
                var claim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (claim == null) return null;

                if (int.TryParse(claim.Value, out var id))
                    return id;

                return null;
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddExpense(int tripId, [FromBody] ExpenseModel dto)
        {
            if (UserId == null)
                return BadRequest(new { message = "User ID is missing or invalid." });

            var result = await _expenseService.AddExpenseAsync(tripId, dto, UserId.Value);
            return Ok(result);
        }

        
        [HttpGet]
        public async Task<IActionResult> GetExpenses(int tripId)
        {
            if (UserId == null)
                return BadRequest(new { message = "User ID is missing or invalid." });

            var result = await _expenseService.GetExpensesAsync(tripId, UserId.Value);
            return Ok(result);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int tripId, int id, [FromBody] ExpenseModel dto)
        {
            if (UserId == null)
                return BadRequest(new { message = "User ID is missing or invalid." });

            var result = await _expenseService.UpdateExpenseAsync(tripId, id, dto, UserId.Value);
            if (result == null)
                return NotFound("Expense not found or you don't have permission.");
            return Ok(result);
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int tripId, int id)
        {
            if (UserId == null)
                return BadRequest(new { message = "User ID is missing or invalid." });

            var success = await _expenseService.DeleteExpenseAsync(tripId, id, UserId.Value);
            if (!success)
                return NotFound("Expense not found or you don't have permission.");
            return NoContent();
        }
    }
}
