using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using plusminus.Dtos.Expenses;
using plusminus.Models;
using plusminus.Services.ExpensesService;
using System.Globalization;
using System.Security.Claims;

namespace plusminus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpensesService _expensesService;

        public ExpensesController(IExpensesService expensesService)
        {
            _expensesService = expensesService;
        }

        [HttpGet("expanses")]
        public async Task<ActionResult<ServiceResponse<List<GetExpensesDto>>>> GetExpanses([FromQuery] string date)
        {
            if (!DateOnly.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly parsedDate))
            {
                return BadRequest("Неверный формат даты. Используйте формат yyyy-MM-dd.");
            }
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
            {
                return Unauthorized();
            }

            if (!int.TryParse(authenticateResult.Principal.FindFirstValue("id"), out int userId))
            {
                return BadRequest("Неверный идентификатор пользователя.");
            }

            return Ok(await _expensesService.GetExpensesByUserId(userId, parsedDate));
        }

        [HttpPost("expanses/add")]
        public async Task<ActionResult<ServiceResponse<List<GetExpensesDto>>>> AddExpenses(AddExpensesDto newExpenses) {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
            {
                return Unauthorized();
            }

            if (!int.TryParse(authenticateResult.Principal.FindFirstValue("id"), out int userId))
            {
                return BadRequest("Неверный идентификатор пользователя.");
            }
            var newExpense = new AddExpensesDto
            {
                Amount = newExpenses.Amount,
                CategoryId = newExpenses.CategoryId,
                Date = newExpenses.Date,
            };

            return Ok(await _expensesService.AddExpenses(newExpense, userId));
        }
        

        [HttpPatch("expanses/update")]
        public async Task<ActionResult<ServiceResponse<GetExpensesDto>>> UpdateExpenses(UpdateExpensesDto newExpenses) => Ok(await _expensesService.UpdateExpenses(newExpenses));

        [HttpDelete("expanses/{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetExpensesDto>>>> DeleteExpenses(int id) => Ok(await _expensesService.DeleteExpensesById(id));

        [HttpGet("expanses/bycategory")]
        public async Task<ActionResult<ServiceResponse<List<ExpensesByCategory>>>> GetExpensesByCategory([FromQuery] string date)
        {
            if (!DateOnly.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly parsedDate))
            {
                return BadRequest("Неверный формат даты. Используйте формат yyyy-MM-dd.");
            }
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
            {
                return Unauthorized();
            }

            if (!int.TryParse(authenticateResult.Principal.FindFirstValue("id"), out int userId))
            {
                return BadRequest("Неверный идентификатор пользователя.");
            }

            return Ok(await _expensesService.GetExpansesByCategory(userId, parsedDate));
        }

        [HttpGet("expanses/sum")]
        public async Task<ActionResult<ServiceResponse<Double>>> GetExpensesSum()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
            {
                return Unauthorized();
            }

            if (!int.TryParse(authenticateResult.Principal.FindFirstValue("id"), out int userId))
            {
                return BadRequest("Неверный идентификатор пользователя.");
            }

            return Ok(await _expensesService.GetExpensesSum(userId));
        }
    }
}
