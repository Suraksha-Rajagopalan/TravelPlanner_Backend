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
    [ApiController]
    [Route("api/trips/{tripId}/expenses")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [HttpPost]
        public async Task<IActionResult> AddExpense(int tripId, [FromBody] Expense dto)
        {
            var result = await _expenseService.AddExpenseAsync(tripId, dto, GetUserId());
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetExpenses(int tripId)
        {
            var result = await _expenseService.GetExpensesAsync(tripId, GetUserId());
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int tripId, int id, [FromBody] Expense dto)
        {
            var result = await _expenseService.UpdateExpenseAsync(tripId, id, dto, GetUserId());
            if (result == null)
                return NotFound("Expense not found or you don't have permission.");
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int tripId, int id)
        {
            var success = await _expenseService.DeleteExpenseAsync(tripId, id, GetUserId());
            if (!success)
                return NotFound("Expense not found or you don't have permission.");
            return NoContent();
        }
    }
}
