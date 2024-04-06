using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using plusminus.Dtos.Expenses;
using plusminus.Helpers;
using plusminus.Models;
using plusminus.Services.ExpensesService;
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
        public async Task<ActionResult<ServiceResponse<List<GetExpensesDto>>>> GetExpanses()
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

            return Ok(await _expensesService.GetExpensesByUserId(userId));
        }

        [HttpPost("expanses/add")]
        public async Task<ActionResult<ServiceResponse<List<GetExpensesDto>>>> AddExpenses(AddExpensesDto newExpenses) => Ok(await _expensesService.AddExpenses(newExpenses));
        

        [HttpPatch("expanses/update")]
        public async Task<ActionResult<ServiceResponse<GetExpensesDto>>> UpdateExpenses(UpdateExpensesDto newExpenses) => Ok(await _expensesService.UpdateExpenses(newExpenses));

        [HttpDelete("expanses/{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetExpensesDto>>>> DeleteExpenses(int id) => Ok(await _expensesService.DeleteExpensesById(id));

        [HttpGet("expanses/bycategory")]
        public async Task<ActionResult<ServiceResponse<List<ExpensesByCategory>>>> GetExpensesByCategory()
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

            return Ok(await _expensesService.GetExpansesByCategory(userId));
        }
    }
}
