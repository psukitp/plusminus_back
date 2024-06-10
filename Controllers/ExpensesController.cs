using Microsoft.AspNetCore.Mvc;
using plusminus.Dtos.Expenses;
using plusminus.Models;
using plusminus.Services.ExpensesService;
using System.Globalization;
using plusminus.Middlewares;

namespace plusminus.Controllers
{
    [ServiceFilter(typeof(AuthorizeFilter))]
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
            var userId = (int)HttpContext.Items["UserId"]!;

            return Ok(await _expensesService.GetExpensesByUserId(userId, parsedDate));
        }

        [HttpPost("expanses/add")]
        public async Task<ActionResult<ServiceResponse<List<GetExpensesDto>>>> AddExpenses(AddExpensesDto newExpenses) {
            var userId = (int)HttpContext.Items["UserId"]!;
            return Ok(await _expensesService.AddExpenses(newExpenses, userId));
        }


        [HttpPatch("expanses/update")]
        public async Task<ActionResult<ServiceResponse<GetExpensesDto>>> UpdateExpenses(UpdateExpensesDto newExpenses)
        {
            var userId = (int)HttpContext.Items["UserId"]!;
            return Ok(await _expensesService.UpdateExpenses(newExpenses, userId));
        }

        [HttpDelete("expanses/{id}")]
        public async Task<ActionResult<ServiceResponse<int>>> DeleteExpenses(int id)
        {
            var userId = (int)HttpContext.Items["UserId"]!;
            return Ok(await _expensesService.DeleteExpensesById(id, userId));
        }

        [HttpGet("expanses/bycategory")]
        public async Task<ActionResult<ServiceResponse<List<ExpensesByCategory>>>> GetExpensesByCategory([FromQuery] string date)
        {
            if (!DateOnly.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly parsedDate))
            {
                return BadRequest("Неверный формат даты. Используйте формат yyyy-MM-dd.");
            }
            var userId = (int)HttpContext.Items["UserId"]!;
            return Ok(await _expensesService.GetExpansesByCategory(userId, parsedDate));
        }

        [HttpGet("expanses/sum")]
        public async Task<ActionResult<ServiceResponse<double>>> GetExpensesSum()
        {
            var userId = (int)HttpContext.Items["UserId"]!;
            return Ok(await _expensesService.GetExpensesSum(userId));
        }
        
        [HttpGet("expanses/bycategory/month")]
        public async Task<ActionResult<ServiceResponse<List<ExpensesByCategory>>>> GetExpensesByCategoryMonth()
        {
            var userId = (int)HttpContext.Items["UserId"]!;
            return Ok(await _expensesService.GetExpansesByCategoryMonth(userId));
        }

        [HttpGet("expanses/dynamicmonth")]
        public async Task<ActionResult<ServiceResponse<GetThisYearExpenses>>> GetExpensesLastFourMonth()
        {
            var userId = (int)HttpContext.Items["UserId"]!;
            return Ok(await _expensesService.GetExpensesThisYear(userId));
        }
    }
}
