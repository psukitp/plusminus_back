using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using plusminus.Dtos.Expenses;
using plusminus.Dtos.Incomes;
using plusminus.Middlewares;
using plusminus.Models;
using plusminus.Services.IncomesService;

namespace plusminus.Controllers
{
    [ServiceFilter(typeof(AuthorizeFilter))]
    [ApiController]
    [Route("api/[controller]")]
    public class IncomesController : ControllerBase
    {
        private readonly IIncomesService _incomesService;

        public IncomesController(IIncomesService incomesService)
        {
            _incomesService = incomesService;
        }

        [HttpGet("incomes")]
        public async Task<ActionResult<ServiceResponse<List<GetIncomesDto>>>> GetIncomes([FromQuery] string date)
        {
            if (!DateOnly.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly parsedDate))
            {
                return BadRequest("Неверный формат даты. Используйте формат yyyy-MM-dd.");
            }
            var userId = (int)HttpContext.Items["UserId"]!;
            return Ok(await _incomesService.GetIncomesByUserId(userId, parsedDate));
        }

        [HttpGet("incomes/bycategory")]
        public async Task<ActionResult<ServiceResponse<List<IncomesByCategory>>>> GetIncomesByCategory(
            [FromQuery] string date)
        {
            if (!DateOnly.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly parsedDate))
            {
                return BadRequest("Неверный формат даты. Используйте формат yyyy-MM-dd.");
            }
            var userId = (int)HttpContext.Items["UserId"]!;
            return Ok(await _incomesService.GetIncomesByCategory(userId, parsedDate));
        }

        [HttpPost("incomes/add")]
        public async Task<ActionResult<ServiceResponse<List<GetIncomesDto>>>> AddExpenses(AddIncomesDto newIncomes)
        {
            var userId = (int)HttpContext.Items["UserId"]!;
            return Ok(await _incomesService.AddIncomes(newIncomes, userId));
        }

        [HttpPatch("incomes/update")]
        public async Task<ActionResult<ServiceResponse<GetIncomesDto>>> UpdateExpenses(UpdateIncomesDto updatedIncomes)
        {
            var userId = (int)HttpContext.Items["UserId"]!;
            return Ok(await _incomesService.UpdateIncomes(updatedIncomes, userId));
        }

        [HttpDelete("incomes/{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetIncomesDto>>>> DeleteExpenses(int id)
        {
            var userId = (int)HttpContext.Items["UserId"]!;
            return Ok(await _incomesService.DeleteIncomesById(id, userId));
        }

        [HttpGet("incomes/sum")]
        public async Task<ActionResult<ServiceResponse<GetIncomesThisMonthStat>>> GetIncomesSum()
        {
            var userId = (int)HttpContext.Items["UserId"]!;
            return Ok(await _incomesService.GetIncomesSum(userId));
        }
        
        [HttpGet("incomes/dynamicmonth")]
        public async Task<ActionResult<ServiceResponse<GetThisYearExpenses>>> GetIncomesThisYear()
        {
            var userId = (int)HttpContext.Items["UserId"]!;
            return Ok(await _incomesService.GetIncomesLastFourMonth(userId));
        }
        
        [HttpGet("incomes/totalDiff")]
        public async Task<ActionResult<ServiceResponse<GetThisYearExpenses>>> GetTotalDiff()
        {
            var userId = (int)HttpContext.Items["UserId"]!;
            return Ok(await _incomesService.GetTotalDiff(userId));
        }
    }
}
