using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using plusminus.Dtos.Expenses;
using plusminus.Dtos.Incomes;
using plusminus.Models;
using plusminus.Services.IncomesService;

namespace plusminus.Controllers
{
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
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
            {
                return Unauthorized();
            }

            if (!int.TryParse(authenticateResult.Principal.FindFirstValue("id"), out int userId))
            {
                return BadRequest("Неверный идентификатор пользователя.");
            }
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
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
            {
                return Unauthorized();
            }

            if (!int.TryParse(authenticateResult.Principal.FindFirstValue("id"), out int userId))
            {
                return BadRequest("Неверный идентификатор пользователя.");
            }

            return Ok(await _incomesService.GetIncomesByCategory(userId, parsedDate));
        }

        [HttpPost("incomes/add")]
        public async Task<ActionResult<ServiceResponse<List<GetIncomesDto>>>> AddExpenses(AddIncomesDto newIncomes)
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

            return Ok(await _incomesService.AddIncomes(newIncomes, userId));
        }

        [HttpPatch("incomes/update")]
        public async Task<ActionResult<ServiceResponse<GetIncomesDto>>> UpdateExpenses(UpdateIncomesDto updatedIncomes) => Ok(await _incomesService.UpdateIncomes(updatedIncomes));

        [HttpDelete("incomes/{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetIncomesDto>>>> DeleteExpenses(int id) => Ok(await _incomesService.DeleteIncomesById(id));
    }
}
